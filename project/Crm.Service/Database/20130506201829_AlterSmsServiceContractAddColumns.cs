namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130506201829)]
	public class AlterSmsServiceContractAddColumns : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("LimitType", DbType.String, 20, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PaymentCondition", DbType.String, 20, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PaymentInterval", DbType.String, 20, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PaymentType", DbType.String, 20, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PriceCurrency", DbType.String, 20, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("IncreasedPriceCurrency", DbType.String, 20, ColumnProperty.Null));

			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("InvoiceSpecialConditions", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("InternalInvoiceInformation", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("Price", DbType.Currency, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PriceModificationDate", DbType.DateTime, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("NoPaymentsUntil", DbType.DateTime, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("IncreasedPrice", DbType.Currency, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("IncreaseByPercent", DbType.Decimal, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceContract]", new Column("PriceGuaranteedUntil", DbType.DateTime, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}