namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210217140700)]
	public class ChangeCrmMessageStateToString : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("CRM.Message", new Column("State", DbType.String, ColumnProperty.NotNull));
			Database.ExecuteNonQuery("UPDATE [CRM].[Message] SET [State] = 'Pending' WHERE [State] = '0'");
			Database.ExecuteNonQuery("UPDATE [CRM].[Message] SET [State] = 'Dispatched' WHERE [State] = '1'");
			Database.ExecuteNonQuery("UPDATE [CRM].[Message] SET [State] = 'Failed' WHERE [State] = '2'");
		}
	}
}
