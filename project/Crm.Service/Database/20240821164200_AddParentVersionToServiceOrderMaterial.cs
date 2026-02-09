namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240821164200)]
	public class AddParentVersionToServiceOrderMaterial : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderMaterial", "ParentServiceOrderMaterialVersion"))
			{
				Database.AddColumn("SMS.ServiceOrderMaterial",
					new Column("ParentServiceOrderMaterialVersion",
						DbType.Int64,
						ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE m SET m.ParentServiceOrderMaterialVersion = p.Version from SMS.ServiceOrderMaterial m join SMS.ServiceOrderMaterial p on m.ParentServiceOrderMaterialId = p.id");
			}
		}
	}
}
