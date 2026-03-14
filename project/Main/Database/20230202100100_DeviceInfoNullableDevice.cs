namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230202100100)]
	public class DeviceInfoNullableDevice : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[CRM].[Device]", new Column("DeviceInfo", DbType.String, ColumnProperty.Null));
		}
	}
}
