namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20231020145000)]
	public class DropTableLuCauseOfFailure : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[SMS].[ServiceOrderDispatch]", "CauseOfFailure"))
			{
				Database.RemoveColumn("[SMS].[ServiceOrderDispatch]", "CauseOfFailure");
			}
			if (Database.TableExists("[Lu].[CauseOfFailure]"))
			{
				Database.RemoveTable("[Lu].[CauseOfFailure]");
			}
		}
	}
}
