using Crm.Library.Data.MigratorDotNet.Framework;

namespace Sms.Scheduler.Database
{
	[Migration(20250306122000)]
	public class MigratePipelineGroupServiceOrderTimes : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery($@"
WITH
profilesGroups as (
	select 
		[rpl].[Profile].[Id],
		PipelineGroup.[key] COLLATE DATABASE_DEFAULT as [Key],
		PipelineGroup.[value] COLLATE DATABASE_DEFAULT as [value]
	from [rpl].[Profile] cross apply OPENJSON(rpl.Profile.ClientConfig, N'$.PipelineGroup') as PipelineGroup
),
profilesToUpdate as (
	select [Id] from profilesGroups where [value] = N'ServiceOrder.ServiceOrderTimes'
),
profilesWithOtherGroups as (
	select profilesGroups.[Id] from
		profilesGroups inner join profilesToUpdate on profilesGroups.Id = profilesToUpdate.Id and profilesGroups.value != N'ServiceOrder.ServiceOrderTimes'
	group by profilesGroups.[Id]
),
profilesWithoutOtherGroups as (
	select id from profilesToUpdate where profilesToUpdate.Id not in (select id from profilesWithOtherGroups) 
),
newProfilesGroups as (
	select id,'[]' as newvalue from profilesWithoutOtherGroups
	union
	select profilesWithOtherGroups.Id, T.J from profilesWithOtherGroups cross apply (
		SELECT JSON_QUERY(CONCAT('[',STUFF((SELECT ',' + CONCAT('""',[value],'""') from profilesGroups where profilesGroups.Id = profilesWithOtherGroups.Id and profilesGroups.value != N'ServiceOrder.ServiceOrderTimes' ORDER BY [key] FOR XML PATH('')), 1, 1, ''),']')) as J
	) T
),
modifiedClientConfigs as (
select
	[rpl].[Profile].id,
	JSON_MODIFY([rpl].[Profile].ClientConfig, '$.PipelineGroup',JSON_QUERY(newProfilesGroups.newvalue)) as ClientConfig
from [rpl].[Profile] inner join newProfilesGroups on [rpl].[Profile].Id = newProfilesGroups.Id
)
update [rpl].[Profile]
	set ClientConfig = JSON_MODIFY(ISNULL(modifiedClientConfigs.ClientConfig,[rpl].[Profile].ClientConfig),'$.ServiceOrderTimesScheduling',IIF(modifiedClientConfigs.id is null,CAST(0 as BIT),CAST(1 as BIT)))
from [rpl].[Profile] left outer join modifiedClientConfigs on [rpl].[Profile].Id = modifiedClientConfigs.Id
");
		}
	}
}
