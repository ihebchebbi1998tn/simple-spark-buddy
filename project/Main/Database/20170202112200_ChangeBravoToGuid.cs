namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170202112200)]
	public class ChangeBravoToGuid : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Bravo]", "BravoIdOld"))
			{
				Database.ExecuteNonQuery("ALTER TABLE CRM.Bravo ADD BravoIdOld int NULL");
				Database.ExecuteNonQuery("UPDATE CRM.Bravo SET BravoIdOld = BravoId");
				Database.ExecuteNonQuery("ALTER TABLE CRM.Bravo DROP Constraint PK_Bravo");
				Database.ExecuteNonQuery("ALTER TABLE CRM.Bravo DROP COLUMN BravoId");
				Database.ExecuteNonQuery("ALTER TABLE CRM.Bravo ADD BravoId uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())");
				Database.ExecuteNonQuery("ALTER TABLE CRM.Bravo ADD PRIMARY KEY (BravoId)");
			}
		}
	}
}
