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
	public class ServiceOrderErrorCauseConfirmedEventHandler : IEventHandler<EntityModifiedEvent<ServiceOrderErrorCause>>
	{
		private readonly Func<ServiceOrderErrorCauseConfirmedNote> noteFactory;
		private readonly IRepositoryWithTypedId<Note, Guid> noteRepository;
		private readonly IResourceManager resourceManager;
		private readonly IUserService userService;

		public ServiceOrderErrorCauseConfirmedEventHandler(Func<ServiceOrderErrorCauseConfirmedNote> noteFactory, IRepositoryWithTypedId<Note, Guid> noteRepository, IResourceManager resourceManager, IUserService userService)
		{
			this.noteFactory = noteFactory;
			this.noteRepository = noteRepository;
			this.resourceManager = resourceManager;
			this.userService = userService;
		}

		public virtual void Handle(EntityModifiedEvent<ServiceOrderErrorCause> e)
		{
			var errorCause = e.Entity;
			if (errorCause.ServiceOrderErrorType == null || errorCause.ServiceOrderErrorType.ServiceCaseId == null)
			{
				return;
			}
			var user = userService.CurrentUser;
			var translationCulture = CultureInfo.GetCultureInfo(user.DefaultLanguageKey ?? "en");
			if (!e.EntityBeforeChange.IsConfirmed && errorCause.IsConfirmed)

			{

				var errorCauseValue = errorCause.StatisticsKeyCause != null ? errorCause.StatisticsKeyCause.Value : string.Empty;
				var note = noteFactory();
				note.IsActive = true;
				note.ContactId = errorCause.ServiceOrderErrorType.ServiceCaseId;
				note.Contact = errorCause.ServiceOrderErrorType.ServiceCase;
				note.Text = resourceManager.GetTranslation("ErrorCauseConfirmed", translationCulture).WithArgs(errorCauseValue);
				note.Plugin = "Crm.Service";
				noteRepository.SaveOrUpdate(note);
			}
		}
	}

}
