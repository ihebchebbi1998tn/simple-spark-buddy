namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190910102000)]
	public class AddInstallationIdToInstallationPos : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.InstallationPos", "InstallationId"))
			{
				Database.AddColumn("SMS.InstallationPos", new Column("InstallationId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.InstallationPos SET SMS.InstallationPos.InstallationId = SMS.InstallationHead.ContactKey FROM SMS.InstallationPos JOIN SMS.InstallationHead ON SMS.InstallationHead.InstallationNo = SMS.InstallationPos.InstallationNo");
				Database.ExecuteNonQuery("DELETE FROM SMS.InstallationPos WHERE InstallationId IS NULL");
				Database.ChangeColumn("SMS.InstallationPos", new Column("InstallationId", DbType.Guid, ColumnProperty.NotNull));
				Database.RemoveColumn("SMS.InstallationPos", "InstallationNo");
				Database.AddForeignKey("FK_InstallationPos_InstallationHead", "SMS.InstallationPos", "InstallationId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_InstallationPos_InstallationId] ON [SMS].[InstallationPos] ([InstallationId] ASC)");
			}
		}
	}
}