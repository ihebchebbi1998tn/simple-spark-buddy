namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140508121400)]
	public class AddSignatureDateColumnsToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("SignatureDate", DbType.DateTime, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("SignatureTechnicianDate", DbType.DateTime, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("SignatureOriginatorDate", DbType.DateTime, ColumnProperty.Null));
		}
	}
}