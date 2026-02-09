namespace Crm.Service.Generators.NoteGenerators
{
	using System;
	using System.Collections.Generic;

	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Modularization.Events;
	using Crm.Model.Notes;
	using Crm.Service.Model;
	using Crm.Service.Model.Notes;

	public class ServiceCaseCreatedNoteGenerator : NoteGenerator<EntityCreatedEvent<ServiceCase>>
	{
		private readonly Func<ServiceCaseCreatedNote> noteFactory;
		// Methods
		public override Note GenerateNote(EntityCreatedEvent<ServiceCase> e)
		{
			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = e.Entity.Id;
			note.Contact = e.Entity;
			note.ContactType = "ServiceCase";
			var names = new List<string>();
			if (e.Entity.ServiceCaseNo != null)
			{
				names.Add(e.Entity.ServiceCaseNo);
			}

			if (e.Entity.ErrorMessage != null)
			{
				names.Add(e.Entity.ErrorMessage);
			}
			note.ContactName = string.Join(" - ", names.ToArray());
			note.Text = "Servicefall angelegt: {0}".WithArgs(e.Entity.ServiceCaseNo);
			note.Plugin = "Crm.Service";
			return note;
		}
		public ServiceCaseCreatedNoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository, Func<ServiceCaseCreatedNote> noteFactory)
			: base(noteRepository)
		{
			this.noteFactory = noteFactory;
		}
	}
}
