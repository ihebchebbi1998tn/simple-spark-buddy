namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130628112022)]
	public class AddFollowUpOrderColumnsToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("FollowUpServiceOrder", DbType.Boolean, ColumnProperty.NotNull, false));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("FollowUpServiceOrderRemark", DbType.String, Int32.MaxValue, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}