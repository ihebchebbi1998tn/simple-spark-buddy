namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211117123800)]
	public class AddOpenIdIdentifierToUser : Migration
	{
		public override void Up()
		{
			if(!Database.ColumnExists("[CRM].[User]", "OpenIdIdentifier"))
			{
				Database.AddColumn("[CRM].[User]", new Column("OpenidIdentifier", System.Data.DbType.String, ColumnProperty.Null));
			}
		}
	}
}
