namespace Crm.Service.BackgroundServices
{
	using System;
	using System.Linq;
	using System.Net.Mime;

	using Crm.Library.AutoFac;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;
	using Crm.Services.Interfaces;

	using log4net;

	using Main.Model;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class ServiceOrderReportSenderAgent : JobBase, IDocumentGeneratorService
	{
		private readonly IServiceOrderService serviceOrderService;
		private readonly IUserService userService;
		private readonly ILog logger;
		private readonly IServiceOrderReportSenderConfiguration configuration;
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<Message> messageFactory;
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IFileService fileService;

		protected override void Run(IJobExecutionContext context)
		{
			var sendServiceOrderReportsOnCompletion = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendServiceOrderReportsOnCompletion);
			if (!sendServiceOrderReportsOnCompletion)
			{
				return;
			}

			var serviceOrdersWithoutReport = GetPendingDocuments().Cast<ServiceOrderHead>().ToList();

			foreach (var serviceOrder in serviceOrdersWithoutReport)
			{
				if (receivedShutdownSignal)
				{
					break;
				}

				if (!HasReportRecipients(serviceOrder))
				{
					var noRecipientsMessage = $"ServiceOrder report sending failed for service order {serviceOrder.OrderNo}. {nameof(ServiceOrderReportSenderAgent)} is activated but sender cannot send report without recipients.";
					serviceOrder.ReportSendingError = noRecipientsMessage;
					serviceOrder.ReportSent = true;
					serviceOrderRepository.SaveOrUpdate(serviceOrder);
					serviceOrderRepository.Session.Flush();
					logger.WarnFormat(noRecipientsMessage);
					break;
				}

				try
				{
					SendServiceOrderReport(serviceOrder);
					logger.InfoFormat("ServiceOrder report sent for service order {0}", serviceOrder.Id);
					serviceOrder.ReportSent = true;
				}
				catch (Exception ex)
				{
					serviceOrder.ReportSendingError = ex.ToString();
					serviceOrder.ReportSent = false;
					logger.Error("Error sending Service Order Report", ex);
				}
				finally
				{
					serviceOrderRepository.SaveOrUpdate(serviceOrder);
					serviceOrderRepository.Session.Flush();
				}
			}
		}

		protected virtual bool HasReportRecipients(ServiceOrderHead serviceOrder)
		{
			if (appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendServiceOrderReportToDispatchers))
			{
				var dispatchers = serviceOrder.Dispatches.Select(x => x.CreateUser).Distinct().Select(x => userService.GetUser(x));
				var dispatcherEmails = dispatchers.Where(x => x.Email.IsNotNullOrEmpty() && x.Email.IsValidEmailAddress()).Select(x => x.Email).Distinct();
				if (dispatcherEmails.Any())
				{
					return true;
				}
				else
				{
					logger.WarnFormat("{0} config is activated but {1} cannot send report without valid dispatcher email address.",
						nameof(ServicePlugin.Settings.Email.SendServiceOrderReportToDispatchers),
						nameof(ServiceOrderReportSenderAgent));
				}
			}
			if (appSettingsProvider.GetValue(ServicePlugin.Settings.Email.DispatchReportRecipients).Any())
			{
				return true;
			}
			return false;
		}

		protected virtual void SendServiceOrderReport(ServiceOrderHead order)
		{
			var fileResource = fileService.CreateAndSaveFileResource(order, serviceOrderService.CreateServiceOrderReportAsPdf(order), MediaTypeNames.Application.Pdf, configuration.GetReportFileName(order));
			var message = messageFactory();
			message.Body = configuration.GetEmailText(order);
			message.Subject = configuration.GetSubject(order);
			var sendServiceOrderReportToDispatchers = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendServiceOrderReportToDispatchers);
			if (sendServiceOrderReportToDispatchers)
			{
				var dispatchers = order.Dispatches.Select(x => x.CreateUser).Distinct().Select(x => userService.GetUser(x));
				var dispatcherEmails = dispatchers.Where(x => x.Email.IsNotNullOrEmpty()).Select(x => x.Email).Distinct();
				dispatcherEmails.ForEach(e => message.Recipients.Add(e));
			}
			var dispatchReportRecipients = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.DispatchReportRecipients);
			dispatchReportRecipients.ForEach(e => message.Bcc.Add(e));
			message.AttachmentIds.Add(fileResource.Id);
			message.LanguageKey = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
			messageRepository.SaveOrUpdate(message);
		}

		public ServiceOrderReportSenderAgent(ISessionProvider sessionProvider, IServiceOrderService serviceOrderService, IUserService userService, IServiceOrderReportSenderConfiguration configuration, IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository, ILog logger, IAppSettingsProvider appSettingsProvider, IHostApplicationLifetime hostApplicationLifetime, IRepositoryWithTypedId<Message, Guid> messageRepository, Func<Message> messageFactory, IFileService fileService)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.serviceOrderService = serviceOrderService;
			this.userService = userService;
			this.configuration = configuration;
			this.serviceOrderRepository = serviceOrderRepository;
			this.logger = logger;
			this.appSettingsProvider = appSettingsProvider;
			this.messageFactory = messageFactory;
			this.messageRepository = messageRepository;
			this.fileService = fileService;
		}
		public virtual IQueryable GetFailedDocuments()
		{
			return serviceOrderRepository
				.GetAll()
				.Where(
					x =>
						x.StatusKey == "Closed"
						&& !x.IsTemplate
						&& !x.ReportSent
						&& x.ReportSendingError != null);
		}
		public virtual IQueryable GetPendingDocuments()
		{
			return serviceOrderRepository
				.GetAll()
				.Where(
					x =>
						x.StatusKey == "Closed"
						&& !x.IsTemplate
						&& !x.ReportSent
						&& x.ReportSendingError == null);
		}
		public virtual void Retry(Guid id)
		{
			var serviceOrder = serviceOrderRepository.Get(id);
			serviceOrder.ReportSendingError = null;
			serviceOrderRepository.SaveOrUpdate(serviceOrder);
		}
	}

	public class DefaultServiceOrderReportSenderConfiguration : IServiceOrderReportSenderConfiguration
	{
		private readonly IResourceManager resourceManager;

		public DefaultServiceOrderReportSenderConfiguration(IResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}
		public virtual string GetSubject(ServiceOrderHead serviceOrderHead)
		{
			return resourceManager.GetTranslation("ServiceOrder") + " " + serviceOrderHead.OrderNo;
		}
		public virtual string GetEmailText(ServiceOrderHead serviceOrderHead)
		{
			return String.Empty;
		}
		public virtual string GetReportFileName(ServiceOrderHead serviceOrderHead)
		{
			return String.Format("{0}.pdf", serviceOrderHead.OrderNo);
		}
	}

	public interface IServiceOrderReportSenderConfiguration : IDependency
	{
		string GetSubject(ServiceOrderHead serviceOrderHead);
		string GetEmailText(ServiceOrderHead serviceOrderHead);
		string GetReportFileName(ServiceOrderHead serviceOrderHead);
	}
}
