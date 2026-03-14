namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201015151000)]
	public class IncreaseDescLengthDocumentAttributes : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("CRM.DocumentAttributes", "Description")) {
				Database.ChangeColumn("CRM.DocumentAttributes", new Column("Description", DbType.String, 255, ColumnProperty.NotNull));
			}
		}
	}
}
