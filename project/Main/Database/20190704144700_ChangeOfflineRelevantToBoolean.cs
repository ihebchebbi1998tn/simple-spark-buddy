namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190704144700)]
	public class ChangeOfflineRelevantToBoolean : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE [CRM].[DocumentAttributes] SET OfflineRelevant = 0 WHERE OfflineRelevant <> 1");
			Database.ChangeColumn("[CRM].[DocumentAttributes]", new Column("OfflineRelevant", DbType.Boolean, ColumnProperty.NotNull));
		}
	}
}
