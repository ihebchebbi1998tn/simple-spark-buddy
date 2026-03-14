namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20160901141000)]
	public class AddTransactionIdToCrmPosting : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Posting]", new Column("TransactionId", DbType.String, ColumnProperty.Null));
		}
	}
}