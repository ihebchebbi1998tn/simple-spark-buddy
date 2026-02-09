namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180101110000)]
	public class FixValueServiceContractType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				DECLARE @value INT = (SELECT MAX(TRY_CONVERT(INT, [Value])) FROM [SMS].[ServiceContractType])
				IF @value IS NOT null
 				BEGIN
 					UPDATE SMS.ServiceContractType
 					SET [Value] = @value + 1, ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20180101110000'
					WHERE [Name] = 'Platin' AND [Language] = 'de' AND [Value] = 0
						OR [Name] = 'Platinum' AND [Language] = 'en' AND [Value] = 0
				END
			");
			Database.ExecuteNonQuery(@"
				DECLARE @value INT = (SELECT MAX(TRY_CONVERT(INT, [Value])) FROM [SMS].[ServiceContractType])
				IF @value IS NOT null
 				BEGIN
 					UPDATE SMS.ServiceContractType
 					SET [Value] = @value + 1, ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20180101110000'
					WHERE [Name] = 'Gold' AND [Language] = 'de' AND [Value] = 0
						OR [Name] = 'Gold' AND [Language] = 'en' AND [Value] = 0
				END
			");
			Database.ExecuteNonQuery(@"
				DECLARE @value INT = (SELECT MAX(TRY_CONVERT(INT, [Value])) FROM [SMS].[ServiceContractType])
				IF @value IS NOT null
 				BEGIN
 					UPDATE SMS.ServiceContractType
 					SET [Value] = @value + 1, ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20180101110000'
					WHERE [Name] = 'Silber' AND [Language] = 'de' AND [Value] = 0
						OR [Name] = 'Silver' AND [Language] = 'en' AND [Value] = 0
				END
			");
		}
	}
}