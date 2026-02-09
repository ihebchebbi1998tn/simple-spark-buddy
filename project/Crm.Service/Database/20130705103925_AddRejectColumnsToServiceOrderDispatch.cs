namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130705103925)]
	public class AddRejectColumnsToServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("RejectRemark", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("RejectReason", DbType.String, 50, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}