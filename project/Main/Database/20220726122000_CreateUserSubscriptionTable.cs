namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220726122000)]
	public class CreateUserSubscriptionTable : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[UserSubscription]"))
			{
				Database.AddTable(
					"[CRM].[UserSubscription]",
					new Column("UserSubscriptionId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Username", DbType.String, ColumnProperty.NotNull),
					new Column("EntityType", DbType.String, ColumnProperty.NotNull),
					new Column("EntityKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("IsSubscribed", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'")
				);
			}
		}
	}
}
