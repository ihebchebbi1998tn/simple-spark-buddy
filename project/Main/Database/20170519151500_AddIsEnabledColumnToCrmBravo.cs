namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170519151500)]
	public class AddIsEnabledColumnToCrmBravo : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Bravo]", "IsEnabled"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Bravo] ADD [IsEnabled] bit NOT NULL CONSTRAINT [DF_Bravo_IsEnabled] DEFAULT ((1)) WITH VALUES");

				Database.ExecuteNonQuery("UPDATE CRM.Bravo SET IsEnabled = IsActive");

				Database.ExecuteNonQuery("UPDATE CRM.Bravo SET IsActive = 1, ModifyDate = getutcdate(), ModifyUser = 'Setup'");
			}
		}
	}
}