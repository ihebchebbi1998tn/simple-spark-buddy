namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220914102300)]
	public class AddDiscountTypeToServiceOrderTime : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimes", new Column("DiscountType", DbType.Int32, ColumnProperty.NotNull,1));
		}
	}
}