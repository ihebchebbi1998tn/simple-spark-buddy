namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20181008200000)]
	public class AddCloseReasonToServiceOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderHead", new Column("CloseReason", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}
