namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20211110085000)]
	public class AddCompanyDataToNumberingSequenceTable : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[NumberingSequence] WHERE SequenceName = 'CRM.Company'") == 0)
			{
				Database.ExecuteNonQuery(@"INSERT INTO [dbo].[NumberingSequence] ([SequenceName], [LastNumber], [Prefix], [Format], [Suffix], [MaxLow])	
					VALUES(
						'CRM.Company', 
						1,
						'CO', 
						'00000',
						NULL, 
						32)");
			}

		}
	}
}
