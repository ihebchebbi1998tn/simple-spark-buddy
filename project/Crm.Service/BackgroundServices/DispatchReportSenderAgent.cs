namespace Crm.Service.BackgroundServices
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Net.Mime;
	using System.Security.Principal;
	using System.Text;
	using System.Threading;

	using Crm.Library.AutoFac;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Globalization;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Services.Interfaces;
	using Crm.Services.Interfaces;

	using log4net;

	using Main.Model;
	using Main.Model.Lookups;

	using Microsoft.Extensions.Hosting;

	using MimeKit;

	using Quartz;

	[DisallowConcurrentExecution]
	public class DispatchReportSenderAgent : ManualSessionHandlingJobBase, IDocumentGeneratorService
	{
		private readonly IServiceOrderService serviceOrderService;
		private readonly IUserService userService;
		private readonly IDispatchReportSenderConfiguration dispatchReportSenderConfiguration;
		private readonly IEnumerable<IDispatchReportAttachmentProvider> dispatchReportAttachmentProviders;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository;
		private readonly ILog logger;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IFileService fileService;
		private readonly Func<Message> messageFactory;
		private readonly Func<ServiceOrderDispatchReportRecipient> serviceOrderDispatchReportRecipientFactory;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly ILookupManager lookupManager;

		protected override void Run(IJobExecutionContext context)
		{
			var sendDispatchReportsOnCompletion = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchReportsOnCompletion);
			if (!sendDispatchReportsOnCompletion)
			{
				return;
			}

			var dispatchIdsWithoutReport = GetPendingDocuments().Cast<ServiceOrderDispatch>().ToList().Select(x => x.Id);

			foreach (var dispatchId in dispatchIdsWithoutReport)
			{
				if (receivedShutdownSignal)
				{
					break;
				}

				try
				{
					BeginRequest();
					var dispatch = serviceOrderDispatchRepository.Get(dispatchId);
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
					SendDispatchReport(dispatch);
					serviceOrderDispatchRepository.SaveOrUpdate(dispatch);
				}
				catch (Exception exception)
				{
					EndRequest();
					BeginRequest();
					var dispatch = serviceOrderDispatchRepository.Get(dispatchId);
					dispatch.ReportSendingError = exception.ToString();
					serviceOrderDispatchRepository.SaveOrUpdate(dispatch);
					logger.Error($"failed sending dispatch report {dispatchId}", exception);
				}
				finally
				{
					EndRequest();
				}
			}
		}

		protected virtual IEnumerable<ServiceOrderDispatchReportRecipient> GetRecipients(ServiceOrderDispatch dispatch)
		{
			var recipients = new List<ServiceOrderDispatchReportRecipient>(dispatch.ReportRecipients);

			recipients.AddRange(appSettingsProvider.GetValue(ServicePlugin.Settings.Email.DispatchReportRecipients).Select(x =>
			{
				var recipient = serviceOrderDispatchReportRecipientFactory();
				recipient.Email = x;
				recipient.Internal = true;
				return recipient;
			}).ToList());
			
			var sendDispatchReportToDispatcher = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchReportToDispatcher);
			if (sendDispatchReportToDispatcher)
			{
				var dispatcher = userService.GetUser(dispatch.CreateUser);
				if (dispatcher != null && dispatcher.Email.IsNotNullOrEmpty() && !recipients.Any(x => x.Email == dispatcher.Email))
				{
					var recipient = serviceOrderDispatchReportRecipientFactory();
					recipient.Email = dispatcher.Email;
					recipient.Internal = true;
					recipients.Add(recipient);
				}
			}

			var sendDispatchReportToTechnician = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchReportToTechnician);
			if (sendDispatchReportToTechnician && !recipients.Any(x => x.Email == dispatch.DispatchedUser.Email))
			{
				var recipient = serviceOrderDispatchReportRecipientFactory();
				recipient.Email = dispatch.DispatchedUser.Email;
				recipient.Internal = true;
				recipients.Add(recipient);
			}

			foreach (var recipient in recipients.Where(x => x.Language == null))
			{
				recipient.Language = clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault();
				recipient.Locale = clientSideGlobalizationService.GetCurrentCultureNameOrDefault();
			}

			foreach (var recipient in recipients.Where(x => x.Locale == null))
			{
				recipient.Locale = lookupManager.Get<Language>(recipient.Language)?.DefaultLocale ?? recipient.Language;
			}

			return recipients;
		}

		protected virtual void SendDispatchReport(ServiceOrderDispatch dispatch)
		{
			var recipients = GetRecipients(dispatch);

			var currentUiCulture = Thread.CurrentThread.CurrentUICulture;

			foreach (var recipientsForLanguage in recipients.GroupBy(x => new { x.Language, x.Locale, x.Internal }))
			{
				Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(recipientsForLanguage.Key.Language);
				clientSideGlobalizationService.SetCurrentLanguageName(recipientsForLanguage.Key.Language);
				clientSideGlobalizationService.SetCurrentCultureName(recipientsForLanguage.Key.Locale);
				var attachments = new List<FileResource>();
				var bytes = serviceOrderService.CreateDispatchReportAsPdf(dispatch);
				attachments.Add(fileService.CreateAndSaveFileResource(dispatch, bytes, MediaTypeNames.Application.Pdf, dispatchReportSenderConfiguration.GetReportFileName(dispatch).AppendIfMissing(".pdf")));
				foreach (var dispatchReportAttachmentProvider in dispatchReportAttachmentProviders)
				{
					attachments.AddRange(dispatchReportAttachmentProvider.GetAttachments(dispatch, recipientsForLanguage.Key.Internal).Select(x =>
					{
						using var memoryStream = new MemoryStream();
						((MimePart)x).Content.WriteTo(memoryStream);
						return fileService.CreateAndSaveFileResource(dispatch,
							memoryStream.ToArray(),
							x.ContentType.MimeType,
							x.ContentType.Name);
					}));
				}
				SendDispatchReport(dispatch, attachments.Select(x => x.Id).ToList(), recipientsForLanguage);
			}
			
			Thread.CurrentThread.CurrentUICulture = currentUiCulture;

			dispatch.ReportSent = true;
		}

		protected virtual void SendDispatchReport(ServiceOrderDispatch dispatch, ICollection<Guid> fileResourceIds, IEnumerable<ServiceOrderDispatchReportRecipient> recipients)
		{
			foreach (var recipient in recipients)
			{
				if (!recipient.Email.IsValidEmailAddress())
				{
					logger.WarnFormat("Dispatch report sending skipped for dispatch {0} / order {1} because recipient {2} is not a valid email address", dispatch.Id, dispatch.OrderHead != null ? dispatch.OrderHead.OrderNo : dispatch.OrderId.ToString(), recipient);
					continue;
				}
				var message = messageFactory();
				if (dispatch.DispatchedUser != null && dispatch.DispatchedUser.Email.IsValidEmailAddress())
				{
					message.From = new MailboxAddress(dispatch.DispatchedUser.DisplayName, dispatch.DispatchedUser.Email).ToString();
				}

				message.Recipients.Add(recipient.Email);
				message.AttachmentIds = fileResourceIds;
				message.Subject = dispatchReportSenderConfiguration.GetSubject(dispatch);
				message.Body = dispatchReportSenderConfiguration.GetEmailText(dispatch, recipient.Internal);
				message.LanguageKey = recipient.Language;
				messageRepository.SaveOrUpdate(message);
			}
		}

		public DispatchReportSenderAgent(ISessionProvider sessionProvider,
			IServiceOrderService serviceOrderService,
			IUserService userService,
			IDispatchReportSenderConfiguration dispatchReportSenderConfiguration,
			IEnumerable<IDispatchReportAttachmentProvider> dispatchReportAttachmentProviders,
			IRepositoryWithTypedId<ServiceOrderDispatch, Guid> serviceOrderDispatchRepository,
			IAppSettingsProvider appSettingsProvider,
			ILog logger,
			IHostApplicationLifetime hostApplicationLifetime,
			IRepositoryWithTypedId<Message, Guid> messageRepository,
			IFileService fileService,
			Func<Message> messageFactory,
			Func<ServiceOrderDispatchReportRecipient> serviceOrderDispatchReportRecipientFactory,
			IClientSideGlobalizationService clientSideGlobalizationService,
			ILookupManager lookupManager)
			: base(sessionProvider, logger, hostApplicationLifetime)

		{
			this.serviceOrderService = serviceOrderService;
			this.userService = userService;
			this.dispatchReportSenderConfiguration = dispatchReportSenderConfiguration;
			this.dispatchReportAttachmentProviders = dispatchReportAttachmentProviders;
			this.serviceOrderDispatchRepository = serviceOrderDispatchRepository;
			this.appSettingsProvider = appSettingsProvider;
			this.logger = logger;
			this.messageRepository = messageRepository;
			this.fileService = fileService;
			this.messageFactory = messageFactory;
			this.serviceOrderDispatchReportRecipientFactory = serviceOrderDispatchReportRecipientFactory;
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.lookupManager = lookupManager;
		}
		public virtual IQueryable GetFailedDocuments()
		{
			return serviceOrderDispatchRepository
				.GetAll()
				.Where(x =>
					(x.StatusKey == "ClosedComplete" || x.StatusKey == "ClosedNotComplete")
					&& !x.ReportSent && x.ReportSendingError != null); 
		}
		public virtual IQueryable GetPendingDocuments()
		{
			return serviceOrderDispatchRepository
				.GetAll()
				.Where(x =>
					(x.StatusKey == "ClosedComplete" || x.StatusKey == "ClosedNotComplete")
					&& !x.ReportSent && x.ReportSendingError == null);
		}
		public virtual void Retry(Guid id)
		{
			var serviceOrderDispatch = serviceOrderDispatchRepository.Get(id);
			serviceOrderDispatch.ReportSendingError = null;
			serviceOrderDispatchRepository.SaveOrUpdate(serviceOrderDispatch);
		}
	}

	public class DefaultDispatchReportSenderConfiguration : IDispatchReportSenderConfiguration
	{
		private readonly IResourceManager resourceManager;
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		public DefaultDispatchReportSenderConfiguration(IResourceManager resourceManager, IClientSideGlobalizationService clientSideGlobalizationService)
		{
			this.resourceManager = resourceManager;
			this.clientSideGlobalizationService = clientSideGlobalizationService;
		}
		public virtual string GetSubject(ServiceOrderDispatch dispatch)
		{
			return $"{GetTranslation("ServiceOrder")} {dispatch.OrderHead.OrderNo} - {GetTranslation("ServiceOrderDispatch")}"
				+ $" {GetTranslation("by")} {dispatch.DispatchedUser.FirstName} {dispatch.DispatchedUser.LastName}"
				+ $" {GetTranslation("OnDate")} {dispatch.Date.ToString("d", CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault()))}";
		}
		public virtual string GetEmailText(ServiceOrderDispatch dispatch, bool includeInternalInformation = false)
		{
			var sb = new StringBuilder();
			sb.AppendLine(GetTranslation("TechnicianCompletedDispatch").WithArgs(dispatch.DispatchedUser.FirstName + " " + dispatch.DispatchedUser.LastName, dispatch.Date.ToString("d", CultureInfo.GetCultureInfo(clientSideGlobalizationService.GetCurrentCultureNameOrDefault()))));
			sb.AppendLine();
			if (dispatch.IsClosedNotComplete())
			{
				sb.AppendFormatLine("{0}: {1}", GetTranslation("FollowUpDispatchRequired"), dispatch.RequiredOperations);
			}
			sb.AppendLine();
			if (dispatch.OrderHead.Name1.IsNotNullOrEmpty() || dispatch.OrderHead.Street.IsNotNullOrEmpty())
			{
				sb.AppendLine(GetTranslation("ServiceLocation"));
				sb.AppendFormatLine("{0} {1} {2}", dispatch.OrderHead.Name1, dispatch.OrderHead.Name2, dispatch.OrderHead.Name3);
				if (dispatch.OrderHead.Street.IsNotNullOrEmpty())
				{
					sb.AppendLine(dispatch.OrderHead.Street);
				}
				if (dispatch.OrderHead.ZipCode.IsNotNullOrEmpty() || dispatch.OrderHead.City.IsNotNullOrEmpty())
				{
					sb.AppendFormatLine("{0} {1}", dispatch.OrderHead.ZipCode, dispatch.OrderHead.City);
				}
				sb.AppendLine();
			}
			sb.AppendFormatLine("{0}: {1}", GetTranslation("ErrorMessage"), dispatch.OrderHead.ErrorMessage);
			if (includeInternalInformation)
			{
				var internalInformations = dispatch.TimePostings
					.Select(x => x.InternalRemark).Union(dispatch.ServiceOrderMaterial.Select(x => x.InternalRemark))
					.Where(x => !String.IsNullOrWhiteSpace(x))
					.ToList();

				if (internalInformations.Any() || dispatch.FollowUpServiceOrder)
				{
					sb.AppendLine();
					sb.AppendLine(GetTranslation("InternalInformationDoNotRedirect"));
				}

				if (internalInformations.Any())
				{
					foreach (string internalInformation in internalInformations)
					{
						sb.AppendLine(internalInformation);
					}
				}

				if (dispatch.FollowUpServiceOrder)
				{
					sb.AppendLine();
					sb.AppendFormatLine("{0}: {1}", GetTranslation("FollowUpServiceOrder"), dispatch.FollowUpServiceOrderRemark);
				}
			}
			return sb.ToString();
		}
		public virtual string GetReportFileName(ServiceOrderDispatch dispatch)
		{
			return $"{dispatch.OrderHead.OrderNo} - {dispatch.Date.ToLocalTime().ToIsoDateString()} {dispatch.Date.ToFormattedString("HH-mm")} - {dispatch.DispatchedUser.DisplayName}";
		}
		protected virtual string GetTranslation(string resourceKey)
		{
			var language = clientSideGlobalizationService.GetCurrentLanguageCultureNameOrDefault();
			return resourceManager.GetTranslation(resourceKey, language);
		}
	}

	public interface IDispatchReportSenderConfiguration : IDependency
	{
		string GetSubject(ServiceOrderDispatch dispatch);
		string GetEmailText(ServiceOrderDispatch dispatch, bool includeInternalInformation = false);
		string GetReportFileName(ServiceOrderDispatch dispatch);
	}

	public interface IDispatchReportAttachmentProvider : IDependency
	{
		IEnumerable<MimeEntity> GetAttachments(ServiceOrderDispatch dispatch, bool includeInternalInformation = false);
		byte[] CreatePdf(Guid serviceOrderChecklistId, out string filename);
	}
}
