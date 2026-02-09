using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20220330105400)]
	public class AddDispatchDataToNumberingSequenceTable : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[NumberingSequence] WHERE SequenceName = 'SMS.ServiceOrderDispatch'") == 0)
			{
				Database.ExecuteNonQuery(@"INSERT INTO [dbo].[NumberingSequence] ([SequenceName], [LastNumber], [Prefix], [Format], [Suffix], [MaxLow])	
					VALUES(
						'SMS.ServiceOrderDispatch', 
						1,
						'D', 
						'00000',
						NULL, 
						32)");
			}
		}
	}
}
