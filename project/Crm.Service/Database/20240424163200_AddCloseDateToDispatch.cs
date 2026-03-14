namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	using System.Data;
	[Migration(20240424163200)]
	public class AddCloseDateToDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("CloseDate", DbType.DateTime, ColumnProperty.Null));
		}
	}
}
