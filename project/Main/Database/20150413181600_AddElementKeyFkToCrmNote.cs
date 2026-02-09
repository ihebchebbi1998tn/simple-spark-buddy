namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413181600)]
	public class AddElementKeyFkToCrmNote : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Note_Contact'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE n SET n.[ElementKey] = NULL FROM [CRM].[Note] n LEFT OUTER JOIN [CRM].[Contact] c ON n.[ElementKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_Note_Contact", "[CRM].[Note]", "ElementKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}