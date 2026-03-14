namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170524164800)]
	public class SetDataOnlyNumbersToComputed : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"ALTER TABLE CRM.Communication DROP COLUMN DataOnlyNumbers
																ALTER TABLE CRM.Communication ADD DataOnlyNumbers AS (
																	CASE 
																		WHEN GroupKey IN('Phone','Fax') THEN ISNULL(CallingCode, '') + ISNULL(AreaCode, '') + REPLACE(SUBSTRING([Data], PATINDEX('%[0-9]%', [Data]), LEN([Data])), ' ', '')
																		ELSE NULL
																	END
																);");
		}
	}
}
