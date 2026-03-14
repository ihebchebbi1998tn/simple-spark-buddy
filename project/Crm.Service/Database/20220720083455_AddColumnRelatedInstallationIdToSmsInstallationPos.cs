namespace Crm.Database.Modify
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using System.Data;

	[Migration(20220720083455)]
	public class AddColumnRelatedInstallationIdToSmsInstallationPos : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[InstallationPos]") && !Database.ColumnExists("[SMS].[InstallationPos]", "RelatedInstallationId"))
			{
				Database.AddColumn("SMS.InstallationPos", new Column("RelatedInstallationId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_InstallationPos_RelatedInstallation", "SMS.InstallationPos", "RelatedInstallationId", "CRM.Contact", "ContactId");
			}
		}
	}
}
