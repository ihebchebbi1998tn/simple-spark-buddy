namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20150413171600)]
	public class AddUserFkToSmsServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderTimePostings_User'") == 0)
			{
				Database.ChangeColumn("[SMS].[ServiceOrderTimePostings]", new Column("UserUsername", DbType.String, 256, ColumnProperty.Null));
				Database.ExecuteNonQuery("UPDATE sotp SET sotp.[UserUsername] = NULL FROM [SMS].[ServiceOrderTimePostings] sotp LEFT OUTER JOIN [CRM].[User] u on sotp.[UserUsername] = u.[Username] WHERE u.[Username] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderTimePostings_User", "[SMS].[ServiceOrderTimePostings]", "UserUsername", "[CRM].[User]", "Username", ForeignKeyConstraint.NoAction);
			}
		}
	}
}