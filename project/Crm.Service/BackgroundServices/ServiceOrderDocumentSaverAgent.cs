namespace Crm.Service.BackgroundServices
{
	using System;
	using System.IO;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Environment.Network;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class ServiceOrderDocumentSaverAgent : JobBase, IDocumentGeneratorService
	{
		private const int BatchSize = 50;
		private readonly IServiceOrderService serviceOrderService;
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		private readonly IServiceOrderDocumentSaverConfiguration serviceOrderDocumentSaverConfiguration;
		private readonly IAppSettingsProvider appSettingsProvider;

		public ServiceOrderDocumentSaverAgent(ISessionProvider sessionProvider, IServiceOrderDocumentSaverConfiguration serviceOrderDocumentSaverConfiguration, IServiceOrderService serviceOrderService, IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository, ILog logger, IAppSettingsProvider appSettingsProvider, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.serviceOrderDocumentSaverConfiguration = serviceOrderDocumentSaverConfiguration;
			this.serviceOrderService = serviceOrderService;
			this.serviceOrderRepository = serviceOrderRepository;
			this.appSettingsProvider = appSettingsProvider;
		}

		protected override void Run(IJobExecutionContext context)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportServiceOrderReportsOnCompletion))
			{
				return;
			}

			var exportServiceOrderReportsPath = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportServiceOrderReportsPath);
			Action<ServiceOrderHead> SaveServiceOrder;

			if (IsUncPath(exportServiceOrderReportsPath))
			{
				var exportServiceOrderReportsUncDomain = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportServiceOrderReportsUncDomain);
				var exportServiceOrderReportsUncUser = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportServiceOrderReportsUncUser);
				var exportServiceOrderReportsUncPassword = appSettingsProvider.GetValue(ServicePlugin.Settings.Export.ExportServiceOrderReportsUncPassword);
				SaveServiceOrder = serviceOrder => SaveServiceOrderReportToUNC(serviceOrder, exportServiceOrderReportsPath, exportServiceOrderReportsUncDomain, exportServiceOrderReportsUncUser, exportServiceOrderReportsUncPassword);
			}
			else
			{
				SaveServiceOrder = serviceOrder => SaveServiceOrderReport(serviceOrder, exportServiceOrderReportsPath);
			}

			var serviceOrdersWithoutReport = GetPendingDocuments().Cast<ServiceOrderHead>().ToList();

			foreach (var serviceOrder in serviceOrdersWithoutReport)
			{
				if (receivedShutdownSignal)
				{
					break;
				}
				try
				{
					SaveServiceOrder(serviceOrder);
					serviceOrder.ReportSavingError = null;
					serviceOrder.ReportSaved = true;
				}
				catch (Exception ex)
				{
					serviceOrder.ReportSaved = false;
					serviceOrder.ReportSavingError = ex.ToString();
					Logger.Error("A problem occured exporting the Service order document", ex);
				}
				finally
				{
					try
					{
						serviceOrderRepository.SaveOrUpdate(serviceOrder);
						serviceOrderRepository.Session.Flush();
					}
					catch (Exception ex)
					{
						Logger.Error("A problem occured exporting the Dispatch document (ServiceOrderHead Transaction)", ex);
						receivedShutdownSignal = true;
					}
				}
			}
		}
		protected virtual void SaveServiceOrderReport(ServiceOrderHead order, string exportServiceOrderReportsPath)
		{
			var bytes = serviceOrderService.CreateServiceOrderReportAsPdf(order);
			var filename = serviceOrderDocumentSaverConfiguration.GetReportFileName(order);
			File.WriteAllBytes(Path.Combine(exportServiceOrderReportsPath, filename), bytes);
		}
		protected virtual  void SaveServiceOrderReportToUNC(ServiceOrderHead order, string exportServiceOrderReportsPath, string exportServiceOrderReportsUncDomain, string exportServiceOrderReportsUncUser, string exportServiceOrderReportsUncPassword)
		{
			using (var unc = new UNCAccessWithCredentials())
			{
				if (unc.NetUseWithCredentials(exportServiceOrderReportsPath, exportServiceOrderReportsUncUser, exportServiceOrderReportsUncDomain, exportServiceOrderReportsUncPassword))
				{
					SaveServiceOrderReport(order, exportServiceOrderReportsPath);
				}
				else
				{
					throw new ApplicationException($"Failed to connect to {ServicePlugin.Settings.Export.ExportServiceOrderReportsPath}\r\nLastError = {unc.LastError}");
				}
			}
		}
		private static bool IsUncPath(string path)
		{
			return Uri.TryCreate(path, UriKind.Absolute, out Uri uri) && uri.IsUnc;
		}
		public virtual IQueryable GetFailedDocuments()
		{
			return serviceOrderRepository
				.GetAll()
				.Where(x =>
					x.StatusKey == "Closed"
					&& !x.IsTemplate
					&& !x.ReportSaved
					&& x.ReportSavingError != null)
				.Take(BatchSize);
		}
		public virtual IQueryable GetPendingDocuments()
		{
			return serviceOrderRepository
				.GetAll()
				.Where(x =>
					x.StatusKey == "Closed"
					&& !x.IsTemplate
					&& !x.ReportSaved
					&& x.ReportSavingError == null)
				.Take(BatchSize);
		}
		public virtual void Retry(Guid id)
		{
			var serviceOrder = serviceOrderRepository.Get(id);
			serviceOrder.ReportSavingError = null;
			serviceOrderRepository.SaveOrUpdate(serviceOrder);
		}
	}

	public class DefaultServiceOrderDocumentSaverConfiguration : IServiceOrderDocumentSaverConfiguration
	{
		private readonly IResourceManager resourceManager;
		public DefaultServiceOrderDocumentSaverConfiguration(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}
		public virtual string GetReportFileName(ServiceOrderHead serviceOrderHead)
		{
			return String.Format("{0} {1}.pdf", resourceManager.GetTranslation("ServiceOrder"), serviceOrderHead.LegacyId ?? serviceOrderHead.OrderNo);
		}
	}

	public interface IServiceOrderDocumentSaverConfiguration : IDependency
	{
		string GetReportFileName(ServiceOrderHead serviceOrderHead);
	}
}
