namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170519094900)]
	public class AddIsRetiredColumnToCrmPerson : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Person]", "IsRetired"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Person] ADD [IsRetired] bit NOT NULL CONSTRAINT [DF_Person_IsRetired] DEFAULT ((0)) WITH VALUES");

				Database.ExecuteNonQuery(@"UPDATE p
																		SET IsRetired = ~c.IsActive
																		FROM CRM.Person p
																		JOIN CRM.Contact c ON p.ContactKey = c.ContactId");

				Database.ExecuteNonQuery(@"UPDATE CRM.Contact SET IsActive = 1, ModifyDate = getutcdate(), ModifyUser = 'Setup' WHERE ContactType = 'Person'");
			}
		}
	}
}