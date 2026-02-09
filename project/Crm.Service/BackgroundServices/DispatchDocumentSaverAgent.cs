namespace Crm.Service.BackgroundServices
{
	using System;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Security.Principal;
	using System.Text.RegularExpressions;
	using System.Threading;

	using Crm.Library.AutoFac;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Environment.Network;
	using Crm.Library.Extensions;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class DispatchDocumentSaverAgent : ManualSessionHandlingJobBase, IDocumentGeneratorService
	{
		static readonly Regex ReplacePatternRegex = new Regex(@"(?<ReplacePattern>({[a-zA-Z_-]+?}))", RegexOptions.Compiled);
		private const int BatchSize = 50;
		private readonly IServiceOrderService serviceOrderService;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository;
		private readonly IDispatchReportExportConfiguration exportConfiguration;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly ILog logger;

		public DispatchDocumentSaverAgent(IServiceOrderService serviceOrderService, IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository, IDispatchReportExportConfiguration exportConfiguration, ISessionProvider sessionProvider, ILog logger, IAppSettingsProvider appSettingsProvider, IHostApplicationLifetime hostApplicationLifetime, IClientSideGlobalizationService clientSideGlobalizationService)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.serviceOrderService = serviceOrderService;
			this.serviceOrderDispatchRepository = serviceOrderDispatchRepository;
			this.exportConfiguration = exportConfiguration;
			this.appSettingsProvider = appSettingsProvider;
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.logger = logger;
		}

		protected override void Run(IJobExecutionContext context)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsOnCompletion))
			{
				return;
			}

			var exportPath = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsPath);
			var filename = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsFilePattern);
			var controlFilename = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsControlFilePattern);
			var controlFileExtension = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsControlFileExtension);
			var controlFileContent = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsControlFileContent);
			Action<ServiceOrderDispatch> ExportDispatch;

			if (IsUncPath(exportPath))
			{
				var exportDispatchReportsUncDomain = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsUncDomain);
				var exportDispatchReportsUncUser = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsUncUser);
				var exportDispatchReportsUncPassword = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportDispatchReportsUncPassword);
				ExportDispatch = dispatch => ExportDispatchReportToUNC(dispatch, exportPath, filename, controlFilename, controlFileExtension, controlFileContent, exportDispatchReportsUncDomain, exportDispatchReportsUncUser, exportDispatchReportsUncPassword);
			}
			else
			{
				ExportDispatch = dispatch => ExportDispatchReport(dispatch, exportPath, filename, controlFilename, controlFileExtension, controlFileContent);
			}

			var dispatchesWithoutReport = GetPendingDocuments().Cast<ServiceOrderDispatch>();

			foreach (var dispatch in dispatchesWithoutReport)
			{
				if (receivedShutdownSignal)
				{
					break;
				}
				try
				{
					var user = dispatch.DispatchedUser;
					if (user != null)
					{
						Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.GetIdentityString()), new string[0]);
						Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault());
						Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault());
					}
					else
					{
						logger.Warn($"could not set user '{dispatch.DispatchedUsername}' for localization");
					}
					ExportDispatch(dispatch);
					dispatch.ReportSavingError = null;
					dispatch.ReportSaved = true;
				}
				catch (Exception ex)
				{
					dispatch.ReportSaved = false;
					dispatch.ReportSavingError = ex.ToString();
					Logger.Error("A problem occured exporting the Dispatch document (ExportDispatch)", ex);
				}
				finally
				{
					try
					{
						BeginTransaction();
						serviceOrderDispatchRepository.SaveOrUpdate(dispatch);
						serviceOrderDispatchRepository.Session.Flush();
						EndTransaction();
					}
					catch (Exception ex)
					{
						Logger.Error("A problem occured exporting the Dispatch document (ServiceOrderDispatch Transaction)", ex);
						receivedShutdownSignal = true;
					}
				}
			}
		}

		protected virtual void ExportDispatchReport(ServiceOrderDispatch dispatch, string exportPath, string filename, string controlFilename, string controlFileExtension, string controlFileContent)
		{
			var bytes = serviceOrderService.CreateDispatchReportAsPdf(dispatch);

			exportPath = InflatePatternString(dispatch, exportPath);
			filename = InflatePatternString(dispatch, filename);
			controlFilename = InflatePatternString(dispatch, controlFilename);
			controlFileContent = InflatePatternString(dispatch, controlFileContent);
			filename = filename.AppendIfMissing(".pdf");
			controlFilename = controlFilename.AppendIfMissing(String.Format(".{0}", controlFileExtension));

			Logger.DebugFormat("writing dispatch {0} to {1} as {2} (control {3})", dispatch.Id, exportPath, filename, controlFilename);
			File.WriteAllBytes(Path.Combine(exportPath, filename), bytes);
			File.WriteAllText(Path.Combine(exportPath, controlFilename), controlFileContent);
		}
		protected virtual void ExportDispatchReportToUNC(ServiceOrderDispatch dispatch, string exportPath, string filename, string controlFilename, string controlFileExtension, string controlFileContent,
			string exportDispatchReportsUncDomain, string exportDispatchReportsUncUser, string exportDispatchReportsUncPassword)
		{
			using (var unc = new UNCAccessWithCredentials())
			{
				if (unc.NetUseWithCredentials(exportPath, exportDispatchReportsUncUser, exportDispatchReportsUncDomain, exportDispatchReportsUncPassword))
				{
					ExportDispatchReport(dispatch, exportPath, filename, controlFilename, controlFileExtension, controlFileContent);
				}
				else
				{
					throw new ApplicationException($"Failed to connect to {exportPath}\r\nLastError = {unc.LastError}");
				}
			}
		}

		protected virtual string InflatePatternString(ServiceOrderDispatch dispatch, string inflateString)
		{
			var matches = ReplacePatternRegex.Matches(inflateString);
			var result = inflateString;

			foreach (Match match in matches)
			{
				var replacePattern = match.Groups["ReplacePattern"].Value;
				var replacementString = exportConfiguration.GetPatternValue(dispatch, replacePattern);

				if (!String.IsNullOrEmpty(replacementString))
				{
					result = result.Replace(match.Groups[0].Value, replacementString);
				}
			}

			return result;
		}
		private static bool IsUncPath(string path)
		{
			return Uri.TryCreate(path, UriKind.Absolute, out Uri uri) && uri.IsUnc;
		}
		
		public virtual IQueryable GetFailedDocuments()
		{
			var serviceOrderDispatches = serviceOrderDispatchRepository
				.GetAll()
				.Where(x => !x.ReportSaved && x.ReportSavingError != null);
			return exportConfiguration.FilterDispatches(serviceOrderDispatches).Take(BatchSize);
		}
		public virtual IQueryable GetPendingDocuments()
		{
			var serviceOrderDispatches = serviceOrderDispatchRepository
				.GetAll()
				.Where(x => !x.ReportSaved && x.ReportSavingError == null);
				return exportConfiguration.FilterDispatches(serviceOrderDispatches).Take(BatchSize);
		}
		public virtual void Retry(Guid id)
		{
			var serviceOrderDispatch = serviceOrderDispatchRepository.Get(id);
			serviceOrderDispatch.ReportSavingError = null;
			serviceOrderDispatchRepository.SaveOrUpdate(serviceOrderDispatch);
		}
	}

	public class DefaultDispatchReportExportConfiguration : IDispatchReportExportConfiguration
	{
		public virtual string GetPatternValue(ServiceOrderDispatch dispatch, string pattern)
		{
			if (dispatch == null) return String.Empty;
			if (string.IsNullOrWhiteSpace(pattern)) return String.Empty;

			switch (pattern.ToLowerInvariant())
			{
				case "{dispatchid}":
					return dispatch.Id.ToString();
				case "{orderno}":
					return dispatch.OrderHead != null ? dispatch.OrderHead.OrderNo : string.Empty;
				case "{installationno}":
					return dispatch.OrderHead?.AffectedInstallation?.InstallationNo ?? string.Empty;
				case "{customerno}":
					return (dispatch.OrderHead != null && dispatch.OrderHead.CustomerContact != null) ? dispatch.OrderHead.CustomerContact.LegacyId : string.Empty;
				case "{date-yyyy}":
					return dispatch.Date.ToString("yyyy");
				case "{date-mm}":
					return dispatch.Date.ToString("MM");
				case "{date-dd}":
					return dispatch.Date.ToString("dd");
				case "{useradname}":
					return dispatch.DispatchedUser != null ? dispatch.DispatchedUser.AdName : string.Empty;
				case "{userpersonnelid}":
					return dispatch.DispatchedUser != null ? dispatch.DispatchedUser.PersonnelId : string.Empty;
				case "{userdisplayname}":
					return dispatch.DispatchedUser != null ? dispatch.DispatchedUser.DisplayName : string.Empty;
				default:
					return String.Empty;
			}
		}
		public virtual IQueryable<ServiceOrderDispatch> FilterDispatches(IQueryable<ServiceOrderDispatch> dispatchesWithoutReport)
		{
			return dispatchesWithoutReport.Where(x => x.StatusKey == "ClosedComplete" || x.StatusKey == "ClosedNotComplete");
		}
	}

	public interface IDispatchReportExportConfiguration : IDependency
	{
		string GetPatternValue(ServiceOrderDispatch dispatch, string pattern);
		IQueryable<ServiceOrderDispatch> FilterDispatches(IQueryable<ServiceOrderDispatch> dispatchesWithoutReport);
	}
}
