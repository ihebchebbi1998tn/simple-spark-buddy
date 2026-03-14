namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using System.Data;

	[Migration(20230809110500)]
	public class AddBatchNoToMaterial : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterial", new Column("BatchNo", DbType.String, 250, ColumnProperty.Null));
		}
	}
}