namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220817112000)]
	public class AddCurrencyToServiceOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumn("SMS.ServiceOrderHead", new Column("CurrencyKey", DbType.String));
		}
	}
}
