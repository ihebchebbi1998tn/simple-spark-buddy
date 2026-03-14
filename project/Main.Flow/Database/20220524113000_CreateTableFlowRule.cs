using System.Data;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Main.Flow.Database
{
	[Migration(20220524113000)]
	public class CreateTableFlowRule : Migration
	{
		public override void Up()
		{
			Database.AddTable("[CRM].[FlowRule]",
					new Column("FlowRuleId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("EntityType", DbType.String, ColumnProperty.NotNull),
					new Column("Description", DbType.String, ColumnProperty.Null),
					new Column("Endpoint", DbType.String, ColumnProperty.NotNull),
					new Column("Username", DbType.String, ColumnProperty.Null),
					new Column("Password", DbType.String, ColumnProperty.Null),
					new Column("Action", DbType.String, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'")
				);
		}
	}
}
