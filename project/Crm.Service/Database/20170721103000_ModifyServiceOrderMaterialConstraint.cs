namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170721103000)]
	public class ModifyServiceOrderMaterialConstraint : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
			IF EXISTS (SELECT 1 FROM sys.objects WHERE name = 'UC_ServiceOrderMaterial' AND parent_object_id = OBJECT_ID('SMS.ServiceOrderMaterial'))
			BEGIN
				ALTER TABLE [SMS].[ServiceOrderMaterial] DROP CONSTRAINT [UC_ServiceOrderMaterial]
			END");
			Database.ExecuteNonQuery("CREATE UNIQUE NONCLUSTERED INDEX IX_ServiceOrderMaterial ON [SMS].[ServiceOrderMaterial] (OrderNo, PosNo, IsActive) WHERE IsActive = 1");
		}
		public override void Down()
		{
		}
	}
}