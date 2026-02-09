namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131015154105)]
	public class AddColumnCategoryToSmsServiceObject : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceObject]", new Column("Category", DbType.String, 20, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}