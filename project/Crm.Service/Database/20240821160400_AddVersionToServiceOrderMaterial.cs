namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240821160400)]
	public class AddVersionToServiceOrderMaterial : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderMaterial", "Version"))
			{
				Database.AddColumn("SMS.ServiceOrderMaterial",
					new Column("Version",
						DbType.Int64,
						ColumnProperty.NotNull,
						1));
			}
		}
	}
}
