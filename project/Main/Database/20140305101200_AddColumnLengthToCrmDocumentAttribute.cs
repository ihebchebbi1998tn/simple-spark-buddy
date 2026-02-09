namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140305101200)]
	public class AddColumnLengthToCrmDocumentAttribute : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[DocumentAttributes]", new Column("Length", DbType.Int64, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}