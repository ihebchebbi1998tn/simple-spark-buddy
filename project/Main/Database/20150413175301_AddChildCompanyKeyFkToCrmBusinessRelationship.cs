namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413175301)]
	public class AddChildCompanyKeyFkToCrmBusinessRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_BusinessRelationship_Child'") == 0)
			{
				Database.ExecuteNonQuery("DELETE br FROM [CRM].[BusinessRelationship] br LEFT OUTER JOIN [CRM].[Contact] c ON br.[ChildCompanyKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_BusinessRelationship_Child", "[CRM].[BusinessRelationship]", "ChildCompanyKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}