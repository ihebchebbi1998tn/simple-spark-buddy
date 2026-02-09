namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130506102719)]
	public class AddTableSmsServiceContractLimitType : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceContractLimitType]"))
			{
				Database.AddTable("[SMS].[ServiceContractLimitType]",
					new Column("ServiceContractLimitTypeId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull));

				InsertServiceContractLimitType("1", "No material invoice", "en", "0", "0");
				InsertServiceContractLimitType("1", "Keine Materialberechnung", "de", "0", "0");
				InsertServiceContractLimitType("2", "From limit full invoice", "en", "0", "1");
				InsertServiceContractLimitType("2", "Ab Limit volle Berechnung", "de", "0", "1");
				InsertServiceContractLimitType("3", "From limit differential invoice", "en", "0", "2");
				InsertServiceContractLimitType("3", "Ab Limit Differenz-Berechnung", "de", "0", "2");
			}
		}
		private void InsertServiceContractLimitType(string value, string name, string language, string favorite, string sortOrder)
		{
			Database.Insert("[SMS].[ServiceContractLimitType]",
				new[] { "Value", "Name", "Language", "Favorite", "SortOrder" },
				new[] { value, name, language, favorite, sortOrder });
		}
		public override void Down()
		{
			throw new System.NotImplementedException();
		}
	}
}