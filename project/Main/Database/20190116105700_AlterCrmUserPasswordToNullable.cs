namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190116105700)]
	public class AlterCrmUserPasswordToNullable : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[CRM].[User]", new Column("Password", DbType.String, 128, ColumnProperty.Null));
		}
	}
}
