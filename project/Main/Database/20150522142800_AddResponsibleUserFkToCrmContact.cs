namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20150522142800)]
	public class AddResponsibleUserFkToCrmContact : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Contact_ResponsibleUser'") == 0)
			{
				Database.ChangeColumn("CRM.Contact", new Column("ResponsibleUser", DbType.String, 256));
				Database.ExecuteNonQuery("UPDATE c SET c.[ResponsibleUser] = NULL FROM [CRM].[Contact] c LEFT OUTER JOIN [CRM].[User] u ON c.[ResponsibleUser] = u.[Username] WHERE u.[Username] IS NULL");
				Database.AddForeignKey("FK_Contact_ResponsibleUser", "[CRM].[Contact]", "ResponsibleUser", "[CRM].[User]", "Username", ForeignKeyConstraint.NoAction);
			}
		}
	}
}