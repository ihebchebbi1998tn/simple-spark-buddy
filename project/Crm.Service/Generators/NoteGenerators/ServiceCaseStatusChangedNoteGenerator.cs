namespace Crm.Service.Generators.NoteGenerators
{
	using System;
	using System.Collections.Generic;

	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model.Notes;
	using Crm.Service.Events;
	using Crm.Service.Model.Notes;

	public class ServiceCaseStatusChangedNoteGenerator : NoteGenerator<ServiceCaseStatusChangedEvent>
	{
		private readonly Func<ServiceCaseStatusChangedNote> noteFactory;
		public override Note GenerateNote(ServiceCaseStatusChangedEvent e)
		{
			var serviceCase = e.ServiceCase;
			var noteText = !string.IsNullOrWhiteSpace(e.NoteText)
				? e.NoteText
				: serviceCase.Status.Value;

			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = serviceCase.Id;
			note.Contact = serviceCase;
			note.ContactType = "ServiceCase";
			var names = new List<string>();
			if (serviceCase.ServiceCaseNo != null)
			{
				names.Add(serviceCase.ServiceCaseNo);
			}

			if (serviceCase.ErrorMessage != null)
			{
				names.Add(serviceCase.ErrorMessage);
			}
			note.ContactName = string.Join(" - ", names.ToArray());
			note.Text = noteText;
			note.Plugin = "Crm.Service";

			return note;
		}
		public ServiceCaseStatusChangedNoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository, Func<ServiceCaseStatusChangedNote> noteFactory)
			: base(noteRepository)
		{
			this.noteFactory = noteFactory;
		}
	}
}
