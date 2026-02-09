namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413175300)]
	public class AddParentCompanyKeyFkToCrmBusinessRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_BusinessRelationship_Parent'") == 0)
			{
				Database.ExecuteNonQuery("DELETE br FROM [CRM].[BusinessRelationship] br LEFT OUTER JOIN [CRM].[Contact] c ON br.[ParentCompanyKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_BusinessRelationship_Parent", "[CRM].[BusinessRelationship]", "ParentCompanyKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}