namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413175900)]
	public class AddContactKeyFkToCrmCommunication : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Communication_Contact'") == 0)
			{
				Database.ExecuteNonQuery("DELETE com FROM [CRM].[Communication] com LEFT OUTER JOIN [CRM].[Contact] c ON com.[ContactKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_Communication_Contact", "[CRM].[Communication]", "ContactKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}