namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170519130300)]
	public class AddIsEnabledColumnToCrmCompany : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Company]", "IsEnabled"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Company] ADD [IsEnabled] bit NOT NULL CONSTRAINT [DF_Company_IsEnabled] DEFAULT ((1)) WITH VALUES");

				Database.ExecuteNonQuery(@"UPDATE com
																		SET IsEnabled = c.IsActive
																		FROM CRM.Company com
																		JOIN CRM.Contact c ON com.ContactKey = c.ContactId");

				Database.ExecuteNonQuery(@"UPDATE CRM.Contact SET IsActive = 1, ModifyDate = getutcdate(), ModifyUser = 'Setup' WHERE ContactType = 'Company'");
			}
		}
	}
}