namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150724125000)]
	public class AddDischargedToCrmUser : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[Crm].[User]", "Discharged"))
			{
				Database.AddColumn("[Crm].[User]", "Discharged", DbType.Boolean, false);
				Database.ExecuteNonQuery("UPDATE [CRM].[User] SET Discharged = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END");
				Database.ExecuteNonQuery("UPDATE [CRM].[User] SET IsActive = 1");
			}
		}
	}
}