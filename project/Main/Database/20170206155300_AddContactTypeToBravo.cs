namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170206155300)]
	public class AddContactTypeToBravo : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF NOT EXISTS(SELECT *
																		FROM   INFORMATION_SCHEMA.COLUMNS
																		WHERE  TABLE_NAME = 'BravoCategory' AND COLUMN_NAME = 'ContactType')
																	ALTER TABLE[LU].BravoCategory ADD ContactType nvarchar(21)");
		}
	}
}
