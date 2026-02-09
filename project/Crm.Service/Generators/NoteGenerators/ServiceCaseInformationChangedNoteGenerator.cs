namespace Crm.Service.Generators.NoteGenerators
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model.Notes;
	using Crm.Service.Events;
	using Crm.Service.Model.Notes;

	public class ServiceCaseInformationChangedNoteGenerator : NoteGenerator<ServiceCaseInformationChangedEvent>
	{
		private readonly Func<ServiceCaseInformationChangedNote> noteFactory;
		public ServiceCaseInformationChangedNoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository, Func<ServiceCaseInformationChangedNote> noteFactory)
			: base(noteRepository)
		{
			this.noteFactory = noteFactory;
		}
		
		public override Note GenerateNote(ServiceCaseInformationChangedEvent e)
		{
			var serviceCase = e.ServiceCase;

			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = serviceCase.Id;
			note.Contact = serviceCase;
			note.Text = e.ModifiedFieldsJson;
			note.Plugin = "Crm.Service";

			return note;
		}
	}
}
