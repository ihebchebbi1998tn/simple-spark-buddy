namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160108140300)]
	public class ChangeCrmPersonAddressKeyOld : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[CRM].[Person]", "[AddressKeyOld]"))
			{
				Database.ChangeColumn("[CRM].[Person]", new Column("AddressKeyOld", DbType.Int32, ColumnProperty.Null));
			}
		}
	}
}