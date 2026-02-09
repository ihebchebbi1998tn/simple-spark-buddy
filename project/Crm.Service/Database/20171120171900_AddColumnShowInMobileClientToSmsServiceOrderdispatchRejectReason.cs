namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20171120171900)]
	public class AddColumnShowInMobileClientToSmsServiceOrderdispatchRejectReason : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderdispatchRejectReason]", new Column("ShowInMobileClient", DbType.Boolean, ColumnProperty.Null, true));

		    Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderDispatchRejectReason] SET ShowInMobileClient = 1");

		    Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatchRejectReason] ALTER COLUMN [ShowInMobileClient] bit NOT NULL");
        }

		public override void Down()
		{
		}
	}
}