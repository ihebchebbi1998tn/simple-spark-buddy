namespace Crm.Service.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Services.Interfaces;
	using System.Globalization;
	using System.Net.Mime;

	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.ViewModels;

	using System.Text;

	using Crm.Services.Interfaces;

	using Main.Model;
	using Main.Services;

	public class ReplenishmentOrderService : IReplenishmentOrderService
	{
		private readonly IRepositoryWithTypedId<ReplenishmentOrder, Guid> replenishmentOrderRepository;
		private readonly IRepositoryWithTypedId<ReplenishmentOrderItem, Guid> replenishmentOrderItemRepository;
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IFileService fileService;
		private readonly IUserService userService;
		private readonly IResourceManager resourceManager;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<Message> messageFactory;
		private readonly IRenderViewToStringService renderViewToStringService;
		private readonly IPdfService pdfService;

		public virtual byte[] CreateReportAsPdf(Guid replenishmentOrderId)
		{
			var model = new HtmlTemplateViewModel { Id = replenishmentOrderId, ViewModel = "Crm.Service.ViewModels.ReplenishmentOrderReportViewModel" };
			var reportHtml = renderViewToStringService.RenderViewToString("Crm.Service", "ReplenishmentOrder", "Report", model);
			var reportPdf = pdfService.Html2Pdf(reportHtml);
			return reportPdf;
		}

		public virtual bool SendOrderAsPdf(ReplenishmentOrder replenishmentOrder)
		{
			if (replenishmentOrder == null)
			{
				throw new ApplicationException("ReplenishmentOrderDoesNotExist");
			}

			if (!replenishmentOrder.IsClosed)
			{
				throw new ApplicationException("ReplenishmentOrderIsNotClosed");
			}

			var recipientEmails = GetDefaultReplenishmentOrderRecipientEmails(replenishmentOrder).ToList();
			if (!recipientEmails.Any())
			{
				return false;
			}

			var user = userService.GetUser(replenishmentOrder.ResponsibleUser);
			var cultureInfo = CultureInfo.GetCultureInfo(user.IsNotNull() ? user.DefaultLanguageKey : userService.CurrentUser.DefaultLanguageKey);
			var message = messageFactory();
			message.From = user.Email;
			message.Subject = resourceManager.GetTranslation("ReplenishmentOrderFromUserDatedOf", cultureInfo)
				.WithArgs(userService.GetDisplayName(replenishmentOrder.ResponsibleUser), replenishmentOrder.CloseDate?.ToShortDateString() ?? string.Empty);
			var bytes = CreateReportAsPdf(replenishmentOrder.Id);
			message.AttachmentIds.Add(fileService.CreateAndSaveFileResource(replenishmentOrder, bytes, MediaTypeNames.Application.Pdf, message.Subject.AppendIfMissing(".pdf")).Id);
			message.Recipients = recipientEmails;
			var closeUser = userService.GetUser(replenishmentOrder.ClosedBy);
			var messagebody = new StringBuilder();
			messagebody.AppendLine(resourceManager.GetTranslation("DefaultGreeting", cultureInfo));
			messagebody.AppendLine("\n" + resourceManager.GetTranslation("ReplenishmentOrderMessageBody", cultureInfo).WithArgs(replenishmentOrder.CloseDate?.ToShortDateString() ?? string.Empty, closeUser.DisplayName));
			if (!string.IsNullOrEmpty((string)user.ExtensionValues["DefaultStoreNo"]))
				messagebody.AppendLine(resourceManager.GetTranslation("UserDefaultStoreMessage", cultureInfo).WithArgs(user.DisplayName, user.ExtensionValues["DefaultStoreNo"]));
			message.Body = messagebody.ToString();
			message.LanguageKey = cultureInfo.TwoLetterISOLanguageName;
			messageRepository.SaveOrUpdate(message);
			return true;
		}

		public virtual string[] GetDefaultReplenishmentOrderRecipientEmails(ReplenishmentOrder replenishmentOrder)
		{
			var recipientEmails = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.ReplenishmentOrderRecipients).ToList();
			if (appSettingsProvider.GetValue(ServicePlugin.Settings.Email.ClosedByRecipientForReplenishmentReport))
			{
				var recipient = userService.GetUser(replenishmentOrder.ClosedBy);
				if (recipient != null)
				{
					recipientEmails.AddItemsNotContained(recipient.Email.AsEnumerable(), StringComparison.InvariantCultureIgnoreCase);
				}
			}
			return recipientEmails.ToArray();
		}

		public virtual IEnumerable<string> GetUsedQuantityUnits()
		{
			return replenishmentOrderItemRepository.GetAll().Select(c => c.QuantityUnitKey).Distinct();
		}

		public ReplenishmentOrderService(IUserService userService, 
			IRepositoryWithTypedId<ReplenishmentOrder, Guid> replenishmentOrderRepository, 
			IRepositoryWithTypedId<ReplenishmentOrderItem, Guid> replenishmentOrderItemRepository, 
			IResourceManager resourceManager,
			IAppSettingsProvider appSettingsProvider,
			Func<Message> messageFactory,
			IRenderViewToStringService renderViewToStringService,
			IPdfService pdfService,
			IRepositoryWithTypedId<Message, Guid> messageRepository,
			IFileService fileService)
		{
			this.userService = userService;
			this.replenishmentOrderRepository = replenishmentOrderRepository;
			this.replenishmentOrderItemRepository = replenishmentOrderItemRepository;
			this.resourceManager = resourceManager;
			this.appSettingsProvider = appSettingsProvider;
			this.messageFactory = messageFactory;
			this.renderViewToStringService = renderViewToStringService;
			this.pdfService = pdfService;
			this.messageRepository = messageRepository;
			this.fileService = fileService;
		}
	}
}
