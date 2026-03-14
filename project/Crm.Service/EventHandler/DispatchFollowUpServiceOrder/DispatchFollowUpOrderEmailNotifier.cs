namespace Crm.Service.EventHandler.DispatchFollowUpServiceOrder
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main.Model;

	public class DispatchFollowUpOrderEmailNotifier : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>, IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>
	{
		protected readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		protected readonly IDispatchFollowUpOrderEmailConfiguration dispatchFollowUpOrderEmailConfiguration;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<Message> messageFactory;

		public DispatchFollowUpOrderEmailNotifier(IRepositoryWithTypedId<Message, Guid> messageRepository, 
			IDispatchFollowUpOrderEmailConfiguration dispatchFollowUpOrderEmailConfiguration, 
			IAppSettingsProvider appSettingsProvider, 
			Func<Message> messageFactory)
		{
			this.messageRepository = messageRepository;
			this.dispatchFollowUpOrderEmailConfiguration = dispatchFollowUpOrderEmailConfiguration;
			this.appSettingsProvider = appSettingsProvider;
			this.messageFactory = messageFactory;
		}

		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchFollowUpOrderNotificationEmails))
			{
				return;
			}

			var dispatch = e.Entity;
			if (dispatch.FollowUpServiceOrder && (dispatch.IsClosedComplete() || dispatch.IsClosedNotComplete()))
			{
				SendMessage(dispatch);
			}
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchFollowUpOrderNotificationEmails))
			{
				return;
			}

			var dispatchAfter = e.Entity;
			var dispatchBefore = e.EntityBeforeChange;

			if (dispatchAfter.FollowUpServiceOrder && !dispatchBefore.IsClosedComplete() && !dispatchBefore.IsClosedNotComplete() && (dispatchAfter.IsClosedComplete() || dispatchAfter.IsClosedNotComplete()))
			{
				SendMessage(dispatchAfter);
			}
		}

		protected virtual void SendMessage(ServiceOrderDispatch dispatch)
		{
			var recipients = dispatchFollowUpOrderEmailConfiguration.GetRecipients(dispatch);

			foreach (var recipient in recipients)
			{
				var message = messageFactory();
				message.Recipients.Add(recipient);
				message.Subject = dispatchFollowUpOrderEmailConfiguration.GetSubject(dispatch);
				message.Body = dispatchFollowUpOrderEmailConfiguration.GetEmailText(dispatch);
				message.LanguageKey = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
				messageRepository.SaveOrUpdate(message);
			}
		}
	}
}
