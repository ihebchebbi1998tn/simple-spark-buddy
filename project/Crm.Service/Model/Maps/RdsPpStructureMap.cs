namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	
	public class RdsPpStructureMap : EntityClassMapping<RdsPpStructure>
	{
		public RdsPpStructureMap()
		{
			Schema("SMS");
			Table("RdsPpStructure");

			Id(x => x.Id, map =>
				{
					map.Column("RdsPpStructureId");
					map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(x => x.ParentRdsPpStructureKey);
			Property(x => x.Description);
			Property(x => x.RdsPpClassification);
		}
	}
}