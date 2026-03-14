namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230510152205)]
	public class AddLanguageKeyToMessage : Migration
	{
		public override void Up()
		{
			Database.AddColumn("[CRM].[Message]", new Column("LanguageKey")
			{
				Type = DbType.String,
				ColumnProperty = ColumnProperty.Null
			});
		}
	}
}
