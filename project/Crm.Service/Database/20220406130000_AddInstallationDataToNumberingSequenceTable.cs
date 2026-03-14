using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20220406130000)]
	public class AddInstallationDataToNumberingSequenceTable : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[NumberingSequence] WHERE SequenceName = 'SMS.Installation'") == 0)
			{
				Database.ExecuteNonQuery(@"INSERT INTO [dbo].[NumberingSequence] ([SequenceName], [LastNumber], [Prefix], [Format], [Suffix], [MaxLow])	
					VALUES(
						'SMS.Installation', 
						1,
						'Inst', 
						'00000',
						NULL, 
						32)");
			}
		}
	}
}
