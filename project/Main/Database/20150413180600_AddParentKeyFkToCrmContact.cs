namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413180600)]
	public class AddParentKeyFkToCrmContact : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Contact_Parent'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE c SET c.[ParentKey] = NULL FROM [CRM].[Contact] c LEFT OUTER JOIN [CRM].[Contact] p ON c.[ParentKey] = p.[ContactId] WHERE p.[ContactId] IS NULL");
				Database.AddForeignKey("FK_Contact_Parent", "[CRM].[Contact]", "ParentKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}