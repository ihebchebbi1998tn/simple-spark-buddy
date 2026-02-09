namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240308160000)]
	public class AddClientConfigFieldToRplProfile : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[RPL].[Profile]", "ClientConfig"))
			{
				Database.AddColumn("RPL.Profile", new Column("ClientConfig", DbType.String, int.MaxValue, ColumnProperty.Null));
			}
		}
	}
}
