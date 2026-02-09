namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240419150000)]
	public class AddColumnsCancellationReasonAndCancellationRemarkToSmsServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("Sms.ServiceOrderDispatch",
				new Column("CancellationReason",
					DbType.String,
					ColumnProperty.Null));
			Database.AddColumnIfNotExisting("Sms.ServiceOrderDispatch",
				new Column("CancellationRemark",
					DbType.String,
					ColumnProperty.Null));
		}
	}
}
