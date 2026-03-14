namespace Crm.Service.Model.Maps.Notes
{
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Service.Model.Notes;

	using NHibernate.Mapping.ByCode.Conformist;

	public class ServiceOrderDispatchCompletedNoteMap : SubclassMapping<ServiceOrderDispatchCompletedNote>, IDatabaseMapping
	{
		public ServiceOrderDispatchCompletedNoteMap()
		{
			DiscriminatorValue("ServiceOrderDispatchCompletedNote");
		}
	}
}