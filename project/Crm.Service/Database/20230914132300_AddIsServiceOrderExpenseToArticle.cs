namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230914132300)]
	public class AddIsServiceOrderExpenseToArticle : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Article", new Column("IsServiceOrderExpense", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
