namespace Crm.Service.Model.Maps.Notes
{
	using System.Diagnostics;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Service.Model.Notes;

	using NHibernate.Mapping.ByCode.Conformist;

	public class ServiceCaseInformationChangedNoteMap : SubclassMapping<ServiceCaseInformationChangedNote>, IDatabaseMapping
	{
		public ServiceCaseInformationChangedNoteMap()
		{
			DiscriminatorValue("ServiceCaseInformationChangedNote");
		}
	}
}
