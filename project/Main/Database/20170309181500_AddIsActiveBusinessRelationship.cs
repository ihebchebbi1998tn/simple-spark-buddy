namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20170309181500)]
	public class AddIsActiveBusinessRelationship : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[BusinessRelationship]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
			Database.ExecuteNonQuery("UPDATE [CRM].[BusinessRelationship] SET ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20170309181500'");
		}
	}
}