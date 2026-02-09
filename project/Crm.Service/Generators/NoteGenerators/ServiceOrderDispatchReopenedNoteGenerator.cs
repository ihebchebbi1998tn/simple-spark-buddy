namespace Crm.Service.Generators.NoteGenerators
{
	using System;

	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model.Notes;
	using Crm.Service.Events;
	using Crm.Service.Model.Notes;

	public class ServiceOrderDispatchReopenedNoteGenerator : NoteGenerator<ServiceOrderDispatchReopenedEvent>
	{
		private readonly Func<ServiceOrderDispatchReopenedNote> noteFactory;
		public ServiceOrderDispatchReopenedNoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository, Func<ServiceOrderDispatchReopenedNote> noteFactory)
			: base(noteRepository)
		{
			this.noteFactory = noteFactory;
		}
		public override Note GenerateNote(ServiceOrderDispatchReopenedEvent e)
		{
			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = e.ServiceOrderDispatch.OrderId;
			note.Contact = e.ServiceOrderDispatch.OrderHead;
			note.ContactType = "Dispatch";
			note.ContactName = e.ServiceOrderDispatch.OrderHead.OrderNo;
			note.Text = e.ServiceOrderDispatch.StatusKey;
			note.Meta = string.Join(";", e.ServiceOrderDispatch.DispatchedUsername);
			note.Plugin = ServicePlugin.PluginName;
			return note;
		}
	}
}
