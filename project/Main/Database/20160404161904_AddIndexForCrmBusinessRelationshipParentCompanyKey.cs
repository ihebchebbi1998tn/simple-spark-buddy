namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404161904)]
	public class AddIndexForCrmBusinessRelationshipParentCompanyKey : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[BusinessRelationship]') AND name = N'IX_BusinessRelationship_ParentCompanyKey'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_BusinessRelationship_ParentCompanyKey] ON [CRM].[BusinessRelationship]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_BusinessRelationship_ParentCompanyKey] ON [CRM].[BusinessRelationship] ([ParentCompanyKey] ASC) INCLUDE ([BusinessRelationshipId], [ChildCompanyKey], [RelationshipType])");
		}
	}
}