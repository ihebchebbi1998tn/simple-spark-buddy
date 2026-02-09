namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170519164800)]
	public class AddIsEnabledColumnToCrmStation : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Station]", "IsEnabled"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Station] ADD [IsEnabled] bit NOT NULL CONSTRAINT [DF_Station_IsEnabled] DEFAULT ((1)) WITH VALUES");

				Database.ExecuteNonQuery("UPDATE CRM.Station SET IsEnabled = IsActive");

				Database.ExecuteNonQuery("UPDATE CRM.Station SET IsActive = 1, ModifyDate = getutcdate(), ModifyUser = 'Setup'");
			}
		}
	}
}