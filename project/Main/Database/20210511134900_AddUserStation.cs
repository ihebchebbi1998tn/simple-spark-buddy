namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20210511134900)]
	public class AddUserStation : Migration
	{
		public override void Up()
		{
			Database.AddTable("CRM.UserStation", new Column[]
			{
				new Column("UserKey", DbType.Guid, ColumnProperty.NotNull),
				new Column("StationKey", DbType.Guid, ColumnProperty.NotNull)
			});

			Database.AddForeignKey("FK_UserStation_UserId", "[CRM].[UserStation]", "UserKey", "[CRM].[User]", "UserId", ForeignKeyConstraint.Cascade);
			Database.AddForeignKey("FK_UserStation_StationId", "[CRM].[UserStation]", "StationKey", "[CRM].[Station]", "StationId", ForeignKeyConstraint.Cascade);
		}
	}
}
