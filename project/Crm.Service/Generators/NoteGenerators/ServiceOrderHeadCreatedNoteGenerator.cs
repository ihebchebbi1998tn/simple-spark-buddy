namespace Crm.Service.Generators.NoteGenerators
{
	using System;

	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Modularization.Events;
	using Crm.Model.Notes;
	using Crm.Service.Model;
	using Crm.Service.Model.Notes;

	public class ServiceOrderHeadCreatedNoteGenerator : NoteGenerator<EntityCreatedEvent<ServiceOrderHead>>
	{
		private readonly Func<ServiceOrderHeadCreatedNote> noteFactory;
		// Methods
		public override Note GenerateNote(EntityCreatedEvent<ServiceOrderHead> e)
		{
			if (e.Entity.IsTemplate)
			{
				return null;
			}
			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = e.Entity.Id;
			note.Contact = e.Entity;
			note.ContactType = "ServiceOrder";
			note.ContactName = e.Entity.OrderNo;
			note.Text = "Auftrag angelegt: {0}".WithArgs(e.Entity.OrderNo);
			note.Plugin = "Crm.Service";
			return note;
		}
		public ServiceOrderHeadCreatedNoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository, Func<ServiceOrderHeadCreatedNote> noteFactory)
			: base(noteRepository)
		{
			this.noteFactory = noteFactory;
		}
	}
}
