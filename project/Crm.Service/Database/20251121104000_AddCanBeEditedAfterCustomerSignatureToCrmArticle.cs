namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20251121104000)]
	public class AddCanBeEditedAfterCustomerSignatureToCrmArticle : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Article", new Column("CanBeEditedAfterCustomerSignature", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
