namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	using Microsoft.Data.SqlClient;

	[Migration(20140822130100)]
	public class UpdateServiceContractNumberingSequence : Migration
	{
		public override void Up()
		{
			var numberingSequenceExists = (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[NumberingSequence] WHERE [SequenceName] = 'SMS.ServiceContract'");
			if (numberingSequenceExists > 0)
			{
				try
				{
					Database.ExecuteNonQuery("UPDATE [dbo].[NumberingSequence] SET [LastNumber] = (SELECT MAX(CONVERT(INT,SUBSTRING([ContractNo], PATINDEX('%[0-9]%', [ContractNo]), LEN([ContractNo]) - PATINDEX('%[0-9]%', [ContractNo]) + 1))) FROM [SMS].[ServiceContract]) WHERE [SequenceName] = 'SMS.ServiceContract'");
				}
				catch (SqlException) // not all contract nos could be parsed, using numbering sequence for contracts not possible
				{
					Database.ExecuteNonQuery("DELETE FROM [dbo].[NumberingSequence] WHERE [SequenceName] = 'SMS.ServiceContract'");
				}
			}
		}

		public override void Down()
		{
		}
	}
}
