namespace Crm.Service.Model.Maps.Notes
{
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Service.Model.Notes;

	using NHibernate.Mapping.ByCode.Conformist;

	public class ServiceOrderErrorCauseConfirmedNoteMap : SubclassMapping<ServiceOrderErrorCauseConfirmedNote>, IDatabaseMapping
	{
		public ServiceOrderErrorCauseConfirmedNoteMap()
		{
			DiscriminatorValue("ServiceOrderErrorCauseConfirmedNote");
		}
	}
}
