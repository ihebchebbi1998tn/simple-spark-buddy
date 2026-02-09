namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130827113629)]
	public class RemoveCreateUserAndModifyUserConstraintsFromSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[SMS].[FK_ServiceOrderDispatch_User]') AND parent_object_id = OBJECT_ID (N'[SMS].[ServiceOrderDispatch]')) ALTER TABLE [SMS].[ServiceOrderDispatch] DROP CONSTRAINT [FK_ServiceOrderDispatch_User] ");
			Database.ExecuteNonQuery("IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[SMS].[FK_ServiceOrderDispatch_User1]') AND parent_object_id = OBJECT_ID (N'[SMS].[ServiceOrderDispatch]')) ALTER TABLE [SMS].[ServiceOrderDispatch] DROP CONSTRAINT [FK_ServiceOrderDispatch_User1] ");
			Database.ExecuteNonQuery("IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[SMS].[FK_ServiceOrderDispatch_User2]') AND parent_object_id = OBJECT_ID (N'[SMS].[ServiceOrderDispatch]')) ALTER TABLE [SMS].[ServiceOrderDispatch] DROP CONSTRAINT [FK_ServiceOrderDispatch_User2] ");
			Database.AddForeignKey("FK_ServiceOrderDispatch_User", "SMS.ServiceOrderDispatch", "Username", "CRM.[User]", "Username");
		}
	}
}