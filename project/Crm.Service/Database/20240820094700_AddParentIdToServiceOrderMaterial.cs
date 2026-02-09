namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20240820094700)]
	public class AddParentIdToServiceOrderMaterial : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderMaterial", "ParentServiceOrderMaterialId"))
			{
				Database.AddColumn("SMS.ServiceOrderMaterial",
					new Column("ParentServiceOrderMaterialId",
						DbType.Guid,
						ColumnProperty.Null));

				Database.AddForeignKey("FK_ServiceOrderMaterial_ParentServiceOrderMaterial",
					"[SMS].[ServiceOrderMaterial]",
					"ParentServiceOrderMaterialId",
					"[SMS].[ServiceOrderMaterial]",
					"Id",
					ForeignKeyConstraint.NoAction);
			}
		}
	}
}
