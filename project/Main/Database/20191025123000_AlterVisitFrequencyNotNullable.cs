namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191025123000)]
	public class AlterVisitFrequencyNotNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE CRM.Company ADD CONSTRAINT DF_VisitFrequencyValue DEFAULT 0 FOR VisitFrequencyValue;");
			Database.ExecuteNonQuery("UPDATE CRM.Company SET VisitFrequencyValue = 0 WHERE VisitFrequencyValue IS NULL;");
			Database.ExecuteNonQuery("ALTER TABLE CRM.Company ALTER COLUMN VisitFrequencyValue int NOT NULL;");
		}
	}
}
