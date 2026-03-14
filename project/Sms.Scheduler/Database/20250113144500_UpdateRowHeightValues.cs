using Crm.Library.Data.MigratorDotNet.Framework;

namespace Sms.Scheduler.Database
{
	[Migration(20250113144500)]
	public class UpdateRowHeightValues : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery($@"
				WITH
				profilesToUpdate as (
				SELECT [rpl].[Profile].[Id], RowData.[value] as ResourceRowHeight
					FROM [RPL].[Profile]
					CROSS APPLY	OPENJSON([RPL].[Profile].[ClientConfig]) as RowData
					Where RowData.[key] = 'ResourceRowHeight'
				)
				update [rpl].[Profile]	set ClientConfig = JSON_MODIFY(clientconfig,N'$.ResourceRowHeight',N'15') 
				from [rpl].[Profile] inner join profilesToUpdate on [rpl].[Profile].id=profilesToUpdate.id
				");
		}
	}
}
