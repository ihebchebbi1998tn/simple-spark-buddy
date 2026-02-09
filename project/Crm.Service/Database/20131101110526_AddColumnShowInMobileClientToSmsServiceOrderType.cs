namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131101110526)]
	public class AddColumnShowInMobileClientToSmsServiceOrderType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderType]", new Column("ShowInMobileClient", DbType.Boolean, ColumnProperty.NotNull, true));
		}

		public override void Down()
		{
		}
	}
}