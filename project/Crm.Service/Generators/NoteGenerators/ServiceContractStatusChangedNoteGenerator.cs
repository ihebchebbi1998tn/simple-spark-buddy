namespace Crm.Service.Generators.NoteGenerators
{
	using System;

	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model.Notes;
	using Crm.Service.Events;
	using Crm.Service.Model.Notes;

	public class ServiceContractStatusChangedNoteGenerator : NoteGenerator<ServiceContractStatusChangedEvent>
	{
		private readonly Func<ServiceContractStatusChangedNote> noteFactory;
		public override Note GenerateNote(ServiceContractStatusChangedEvent e)
		{
			var serviceContract = e.ServiceContract;

			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = serviceContract.Id;
			note.Contact = serviceContract;
			note.ContactType = "ServiceContract";
			note.ContactName = serviceContract.ContractNo;
			note.Text = serviceContract.StatusKey;
			note.Plugin = "Crm.Service";

			return note;
		}
		public ServiceContractStatusChangedNoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository, Func<ServiceContractStatusChangedNote> noteFactory)
			: base(noteRepository)
		{
			this.noteFactory = noteFactory;
		}
	}
}
