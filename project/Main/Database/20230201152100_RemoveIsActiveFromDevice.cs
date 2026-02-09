namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230201152100)]
	public class RemoveIsActiveFromDevice : Migration
	{
		public override void Up()
		{
			if (Database.IndexExists("[CRM].[Device]", "IX_Device_AuthDataId_IsActive"))
			{
				Database.ExecuteNonQuery(@"DROP INDEX [IX_Device_AuthDataId_IsActive] ON [CRM].[Device]");
			}
			Database.RemoveColumn("[CRM].[Device]", "IsActive");
			if (!Database.IndexExists("[CRM].[Device]", "IX_Device_AuthDataId"))
			{
				Database.ExecuteNonQuery(@"CREATE NONCLUSTERED INDEX [IX_Device_AuthDataId] ON [CRM].[Device] ([AuthDataId] ASC)");
			}
		}
	}
}
