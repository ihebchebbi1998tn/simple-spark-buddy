namespace Crm.Service.EventHandler
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Crm.Library.AutoFac;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main.Model;

	public class DispatchRejectEmailNotifier : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>, IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>
	{
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IDispatchRejectEmailConfiguration dispatchRejectEmailConfiguration;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<Message> messageFactory;

		public DispatchRejectEmailNotifier(IRepositoryWithTypedId<Message, Guid> messageRepository, 
			IDispatchRejectEmailConfiguration dispatchRejectEmailConfiguration, 
			IAppSettingsProvider appSettingsProvider, 
			Func<Message> messageFactory)
		{
			this.messageRepository = messageRepository;
			this.dispatchRejectEmailConfiguration = dispatchRejectEmailConfiguration;
			this.appSettingsProvider = appSettingsProvider;
			this.messageFactory = messageFactory;
		}

		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchRejectNotificationEmails))
			{
				return;
			}

			var dispatch = e.Entity;
			if (dispatch.Status.IsRejected() && dispatch.OrderHead != null)
			{
				SendMessage(dispatch);
			}
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchRejectNotificationEmails))
			{
				return;
			}

			var dispatch = e.Entity;
			var dispatchBeforeChange = e.EntityBeforeChange;
			if (dispatch.Status.IsRejected() && !dispatchBeforeChange.Status.IsRejected())
			{
				SendMessage(dispatch);
			}
		}

		protected virtual void SendMessage(ServiceOrderDispatch dispatch)
		{
			var recipients = dispatchRejectEmailConfiguration.GetRecipients(dispatch);

			foreach (var recipient in recipients)
			{
				var message = messageFactory();
				message.Recipients.Add(recipient);
				message.Subject = dispatchRejectEmailConfiguration.GetSubject(dispatch);
				message.Body = dispatchRejectEmailConfiguration.GetEmailText(dispatch);
				message.LanguageKey = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
				messageRepository.SaveOrUpdate(message);
			}			
		}
	}

	public class DefaultDispatchRejectEmailConfiguration : IDispatchRejectEmailConfiguration
	{
		protected readonly IResourceManager resourceManager;
		protected readonly IAppSettingsProvider appSettingsProvider;
		protected readonly IUserService userService;

		public DefaultDispatchRejectEmailConfiguration(IResourceManager resourceManager,
			IAppSettingsProvider appSettingsProvider,
			IUserService userService)
		{
			this.resourceManager = resourceManager;
			this.appSettingsProvider = appSettingsProvider;
			this.userService = userService;
		}
		public virtual string GetSubject(ServiceOrderDispatch dispatch)
		{
			return resourceManager.GetTranslation("DispatchRejectEmailSubject").WithArgs(dispatch.DispatchedUser.DisplayName, dispatch.Date.ToShortDateString(), dispatch.OrderHead.OrderNo, dispatch.RejectReason, dispatch.RejectRemark);
		}
		public virtual string GetEmailText(ServiceOrderDispatch dispatch)
		{
			var sb = new StringBuilder();
			sb.AppendLine(resourceManager.GetTranslation("DispatchRejectEmailText").WithArgs(dispatch.DispatchedUser.DisplayName, dispatch.Date.ToShortDateString(), dispatch.OrderHead.OrderNo, dispatch.RejectReason, dispatch.RejectRemark));
			if (!String.IsNullOrWhiteSpace(dispatch.RejectRemark))
			{
				sb.AppendLine();
				sb.AppendFormatLine("{0}: {1}", resourceManager.GetTranslation("Remark"), dispatch.RejectRemark);
			}
			return sb.ToString();
		}

		public virtual string[] GetRecipients(ServiceOrderDispatch dispatch)
		{
			var recipients = new List<string>();
				
			var dispatchReportRecipients = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.DispatchReportRecipients);
			recipients = recipients.Concat(dispatchReportRecipients).ToList();

			var sendDispatchReportToDispatcher = appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchReportToDispatcher);
			if (sendDispatchReportToDispatcher)
			{
				var recipient = userService.GetUser(dispatch.CreateUser);
				recipients.Add(recipient?.Email);
			}
			
			return recipients.Where(x => x != null && x.IsValidEmailAddress()).Distinct().ToArray();
		}
	}

	public interface IDispatchRejectEmailConfiguration : IDependency
	{
		string [] GetRecipients(ServiceOrderDispatch dispatch);
		string GetSubject(ServiceOrderDispatch dispatch);
		string GetEmailText(ServiceOrderDispatch dispatch);
	}
}
