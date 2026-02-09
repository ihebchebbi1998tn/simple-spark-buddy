namespace Sms.Scheduler.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240408085000)]
	public class MigrateSettingsToClientConfigField : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[RPL].[Profile]", "ClientConfig"))
			{
				Database.ExecuteNonQuery(@$"
												DECLARE @compatibility_level INT;
												SELECT @compatibility_level = (SELECT [compatibility_level] from sys.databases WHERE [name] = db_name())
												IF @compatibility_level >= 140
												BEGIN
													UPDATE [RPL].[Profile] SET [ClientConfig] = (
													SELECT
														CASE WHEN [ResourceRowHeight] < 40 THEN 40 ELSE [ResourceRowHeight] END as 'ResourceRowHeight',
														(SELECT JSON_QUERY('[' + STRING_AGG('""' + value + '""', ',') + ']') FROM (
															SELECT value FROM STRING_SPLIT([ServiceOrderTooltip], ';') WHERE LEN(value)>0) AS data) as 'ServiceOrderTooltip',
														(SELECT JSON_QUERY('[' + STRING_AGG('""' + value + '""', ',') + ']') FROM (
															SELECT value FROM STRING_SPLIT([ServiceOrderDispatchTooltip], ';') WHERE LEN(value)>0) AS data) as 'ServiceOrderDispatchTooltip',
														(SELECT JSON_QUERY('[' + STRING_AGG('""' + value + '""', ',') + ']') FROM (
															SELECT value FROM STRING_SPLIT([ResourceTooltip], ';') WHERE LEN(value)>0) AS data) as 'ResourceTooltip',
														(SELECT JSON_QUERY(STRING_AGG(value, ',')) FROM (
															SELECT TRANSLATE([NonWorkingHours], 'fromto', 'FromTo') as value) AS data) as 'NonWorkingHours',
														CASE WHEN [PipelineRowHeight] < 20 THEN 20 ELSE [PipelineRowHeight] END as 'PipelineRowHeight',
														[EnablePlanningConfirmations],
														[LowerBound],
														[UpperBound],
														(SELECT JSON_QUERY('[' + STRING_AGG('""' + value + '""', ',') + ']') FROM (
															SELECT value FROM STRING_SPLIT([PipelineGroupStorage], ';') WHERE LEN(value)>0) AS data) as 'PipelineGroup'
													FOR JSON PATH, WITHOUT_ARRAY_WRAPPER );
												END
												ELSE
												BEGIN
													UPDATE [RPL].[Profile] SET [ClientConfig] = (
													SELECT
														CASE WHEN [ResourceRowHeight] < 40 THEN 40 ELSE [ResourceRowHeight] END as 'ResourceRowHeight',
														JSON_QUERY(CONCAT('[', STUFF((
															SELECT ',' + CONCAT('""', [value], '""') from (SELECT [value] FROM STRING_SPLIT([ServiceOrderTooltip], ';') WHERE LEN(value) > 0) as DATA FOR XML PATH('')), 1, 1, ''), ']')) as 'ServiceOrderTooltip',
														JSON_QUERY(CONCAT('[', STUFF((
															SELECT ',' + CONCAT('""', [value], '""') from (SELECT [value] FROM STRING_SPLIT([ServiceOrderDispatchTooltip], ';') WHERE LEN(value) > 0) as DATA FOR XML PATH('')), 1, 1, ''), ']')) as 'ServiceOrderDispatchTooltip',
														JSON_QUERY(CONCAT('[', STUFF((
															SELECT ',' + CONCAT('""', [value], '""') from (SELECT [value] FROM STRING_SPLIT([ResourceTooltip], ';') WHERE LEN(value) > 0) as DATA FOR XML PATH('')), 1, 1, ''), ']')) as 'ResourceTooltip',
														[EnablePlanningConfirmations],
														[LowerBound],
														[UpperBound],
														JSON_QUERY(CONCAT('[', STUFF((
															SELECT ',' + CONCAT('""', [value], '""') from (SELECT value FROM STRING_SPLIT([PipelineGroupStorage], ';') WHERE LEN(value) > 0) as DATA FOR XML PATH('')), 1, 1, ''), ']')) as 'PipelineGroup'
													FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
												END
											");
			}
		}
	}
}
