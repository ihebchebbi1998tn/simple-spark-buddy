namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140731102200)]
	public class RemoveUserFKConstraints : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF OBJECT_ID('SMS.FK_ServiceOrderMaterial_User', 'F') IS NOT NULL ALTER TABLE [SMS].[ServiceOrderMaterial] DROP CONSTRAINT [FK_ServiceOrderMaterial_User]");
			Database.ExecuteNonQuery("IF OBJECT_ID('SMS.FK_ServiceOrderMaterial_User1', 'F') IS NOT NULL ALTER TABLE [SMS].[ServiceOrderMaterial] DROP CONSTRAINT [FK_ServiceOrderMaterial_User1]");
		}
		public override void Down()
		{
		}
	}
}