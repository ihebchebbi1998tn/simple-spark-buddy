namespace Crm.Service.EventHandler
{
	using System;
	using System.Globalization;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization.Events;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using Main.Model;

	public class DispatchEmailNotifier : IEventHandler<EntityCreatedEvent<ServiceOrderDispatch>>, IEventHandler<EntityModifiedEvent<ServiceOrderDispatch>>
	{
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IResourceManager resourceManager;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<Message> messageFactory;

		public DispatchEmailNotifier(IRepositoryWithTypedId<Message, Guid> messageRepository, IResourceManager resourceManager, IAppSettingsProvider appSettingsProvider, Func<Message> messageFactory)
		{
			this.messageRepository = messageRepository;
			this.resourceManager = resourceManager;
			this.appSettingsProvider = appSettingsProvider;
			this.messageFactory = messageFactory;
		}

		public virtual void Handle(EntityCreatedEvent<ServiceOrderDispatch> e)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchNotificationEmails))
			{
				return;
			}

			var dispatch = e.Entity;

			if (dispatch.Status.IsReleased() && dispatch.CreateUser != dispatch.DispatchedUser.Id)
			{
				SendMessage(dispatch);
			}
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderDispatch> e)
		{
			if (!appSettingsProvider.GetValue(ServicePlugin.Settings.Email.SendDispatchNotificationEmails))
			{
				return;
			}

			var dispatch = e.Entity;
			var dispatchBeforeChange = e.EntityBeforeChange;

			if (dispatch.Status.IsReleased() && dispatchBeforeChange.Status.IsScheduled() && dispatch.CreateUser != dispatch.DispatchedUser.Id)
			{
				SendMessage(dispatch);
			}
		}

		protected virtual void SendMessage(ServiceOrderDispatch dispatch)
		{
			var message = messageFactory();
			message.Recipients.Add(dispatch.DispatchedUser.Email);
			message.Subject = resourceManager.GetTranslation("DispatchCreatedSubject", CultureInfo.GetCultureInfo(dispatch.DispatchedUser.DefaultLanguageKey));
			message.Body = resourceManager.GetTranslation("DispatchCreatedText", CultureInfo.GetCultureInfo(dispatch.DispatchedUser.DefaultLanguageKey)).WithArgs(dispatch.OrderHead.OrderNo, dispatch.Date.ToLocalTime().ToShortDateString(), dispatch.Date.ToLocalTime().ToShortTimeString());
			message.LanguageKey = dispatch.DispatchedUser.DefaultLanguageKey;
			messageRepository.SaveOrUpdate(message);
		}
	}
}
