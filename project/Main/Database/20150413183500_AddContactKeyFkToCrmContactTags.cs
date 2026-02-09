namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413183500)]
	public class AddContactKeyFkToCrmContactTags : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ContactTags_Contact'") == 0)
			{
				Database.ExecuteNonQuery("DELETE ct FROM [CRM].[ContactTags] ct LEFT OUTER JOIN [CRM].[Contact] c ON ct.[ContactKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ContactTags_Contact", "[CRM].[ContactTags]", "ContactKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}