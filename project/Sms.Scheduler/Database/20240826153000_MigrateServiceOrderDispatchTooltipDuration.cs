using Crm.Library.Data.MigratorDotNet.Framework;

namespace Sms.Scheduler.Database
{
	[Migration(20240826153000)]
	public class MigrateServiceOrderDispatchTooltipDuration : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery($@"
WITH
profilesToUpdate as (
select [rpl].[Profile].[Id],ServiceOrderDispatchTooltip.[key] COLLATE DATABASE_DEFAULT as [Key]
from [rpl].[Profile] cross apply OPENJSON(rpl.Profile.ClientConfig, N'$.ServiceOrderDispatchTooltip') as ServiceOrderDispatchTooltip
where ServiceOrderDispatchTooltip.value= N'ServiceOrderDispatch.Duration' 
)
update [rpl].[Profile]
set ClientConfig = JSON_MODIFY(clientconfig,N'$.ServiceOrderDispatchTooltip['+[key]+N']',N'ServiceOrderDispatch.NetWorkMinutes') 
from [rpl].[Profile] inner join profilesToUpdate on [rpl].[Profile].id=profilesToUpdate.id
");
		}
	}
}
