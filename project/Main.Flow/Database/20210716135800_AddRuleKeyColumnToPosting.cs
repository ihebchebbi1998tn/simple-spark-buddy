using System.Data;
using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

namespace Main.Flow.Database
{
	[Migration(20210716135800)]
	public class AddActionRuleKeyColumnToPosting : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Posting]", new Column("RuleKey", DbType.Guid, ColumnProperty.Null));
		}
	}
}
