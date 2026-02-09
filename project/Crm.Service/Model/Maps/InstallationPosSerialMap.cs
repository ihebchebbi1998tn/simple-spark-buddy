namespace Crm.Service.Model.Maps
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	
    public class InstallationPosSerialMap : EntityClassMapping<InstallationPosSerial>
    {
        public InstallationPosSerialMap()
        {
            Schema("SMS");
            Table("InstallationPosSerials");

            Id(x => x.Id, map =>
                {
                    map.Column("id");
                    map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
                    map.UnsavedValue(Guid.Empty);
                });

            Property(x => x.SerialNo);
            Property(x => x.IsInstalled);
            Property(x => x.InstallationPosId);
        }
    }
}