namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250509112900)]
	public class MigrateCauseOfErrorTypesToSmsErrorCauseTypeRelationship : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[Sms].[ErrorCauseTypeRelationship]"))
			{
				Database.ExecuteNonQuery(@"
					INSERT INTO SMS.ErrorCauseTypeRelationship (
						[CreateDate],
						[ModifyDate],
						[CreateUser],
						[ModifyUser],
						[IsActive],
						[StatisticsKeyCauseKey],
						[ErrorTypeKey]
					)
					SELECT
						GETUTCDATE(),
						GETUTCDATE(),
						'Migration_20250509112900',
						'Migration_20250509112900',
						1,
						skc.StatisticsKeyCauseId,
						skfi.StatisticsKeyFaultImageId
					FROM [LU].[StatisticsKeyCause] skc
					CROSS APPLY STRING_SPLIT(skc.ErrorTypes, ',') AS e join
						[LU].[StatisticsKeyFaultImage] skfi on TRIM(e.value) = skfi.[Value]
					WHERE
						e.value IS NOT NULL
						AND LEN(e.value) > 0;
				");
			}
		}
	}
}
