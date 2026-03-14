namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130802133444)]
	public class AddColumnShowInMobileClientToSmsTimeEntryType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[TimeEntryType]", new Column("ShowInMobileClient", DbType.Boolean, ColumnProperty.NotNull, true));
		}

		public override void Down()
		{
		}
	}
}