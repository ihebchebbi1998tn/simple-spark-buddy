namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180718092900)]
	public class ChangeServiceOrderInstallationNoToInstallationId : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderHead", "InstallationId") && Database.ColumnExists("SMS.ServiceOrderHead", "InstallationNo"))
			{
				Database.AddColumn("SMS.ServiceOrderHead", new Column("InstallationId", DbType.Guid, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE SMS.ServiceOrderHead SET SMS.ServiceOrderHead.InstallationId = SMS.InstallationHead.ContactKey FROM SMS.ServiceOrderHead JOIN SMS.InstallationHead ON SMS.InstallationHead.InstallationNo = SMS.ServiceOrderHead.InstallationNo");
				Database.RemoveForeignKey("SMS.ServiceOrderHead", "FK_ServiceOrderHead_InstallationHead");
				Database.RemoveColumn("SMS.ServiceOrderHead", "InstallationNo");
				Database.AddForeignKey("FK_ServiceOrderHead_InstallationHead", "SMS.ServiceOrderHead", "InstallationId", "CRM.Contact", "ContactId");
				Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ServiceOrderHead_InstallationId] ON [SMS].[ServiceOrderHead] ([InstallationId] ASC)");
			}
		}
	}
}