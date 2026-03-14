namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20231016125300)]
	public class CreatePasswordResetTokenTable : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[dbo].[PasswordResetToken]"))
			{
				return;
			}
			
			Database.AddTable("[dbo].[PasswordResetToken]", new []
			{
				new Column("Id", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
				new Column("ExpiryDate", DbType.DateTime, ColumnProperty.NotNull),
				new Column("Username", DbType.String, ColumnProperty.NotNull),
				new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
				new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
				new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
				new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'")
			});
		}
	}
}
