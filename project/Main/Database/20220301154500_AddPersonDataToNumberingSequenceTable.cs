namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220301154500)]
    public class AddPersonDataToNumberingSequenceTable : Migration
    {
        public override void Up()
        {
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[NumberingSequence] WHERE SequenceName = 'CRM.Person'") == 0)
			{
				Database.ExecuteNonQuery(@"INSERT INTO [dbo].[NumberingSequence] ([SequenceName], [LastNumber], [Prefix], [Format], [Suffix], [MaxLow])	
					VALUES(
						'CRM.Person', 
						1,
						'P', 
						'00000',
						NULL, 
						32)");
			}
		}
    }
}
