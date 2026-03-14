namespace Crm.Service.EventHandler
{
	using System;
	using System.Globalization;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Model.Notes;
	using Crm.Service.Model;
	using Crm.Service.Model.Notes;

		public class ServiceOrderErrorTypeConfirmedEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderErrorType>>
		{
		private readonly Func<ServiceOrderErrorTypeConfirmedNote> noteFactory;
		private readonly IRepositoryWithTypedId<Note, Guid> noteRepository;
		private readonly IResourceManager resourceManager;
		private readonly IUserService userService;

		public ServiceOrderErrorTypeConfirmedEventHandler(Func<ServiceOrderErrorTypeConfirmedNote> noteFactory, IRepositoryWithTypedId<Note, Guid> noteRepository, IResourceManager resourceManager, IUserService userService)
		{
			this.noteFactory = noteFactory;
			this.noteRepository = noteRepository;
			this.resourceManager = resourceManager;
			this.userService = userService;
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderErrorType> e)
		{
			var error = e.Entity;
			if (error.ServiceCaseId == null)
			{
				return;
			}
			var user = userService.CurrentUser;
			var translationCulture = CultureInfo.GetCultureInfo(user.DefaultLanguageKey ?? "en");
			if (!e.EntityBeforeChange.IsConfirmed && error.IsConfirmed)
			{
				var errorTypeValue = error.StatisticsKeyFaultImage != null ? error.StatisticsKeyFaultImage.Value : string.Empty;
				var note = noteFactory();
				note.IsActive = true;
				note.ContactId = error.ServiceCaseId;
				note.Contact = error.ServiceCase;
				note.Text = resourceManager.GetTranslation("ErrorTypeConfirmed", translationCulture).WithArgs(errorTypeValue);
				note.Plugin = "Crm.Service";

				noteRepository.SaveOrUpdate(note);
			}
		}
	}

}
