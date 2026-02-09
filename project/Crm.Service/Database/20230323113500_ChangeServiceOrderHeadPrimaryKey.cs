namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230323113500)]
	public class ChangeServiceOrderHeadPrimaryKey : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderHead]"))
			{
				if(Database.ConstraintExists("[SMS].[ServiceOrderHead]", "FK_ServiceOrderHead_ServiceOrderHead1"))
					Database.RemoveConstraint("[SMS].[ServiceOrderHead]", "FK_ServiceOrderHead_ServiceOrderHead1");
				Database.RemoveConstraint("[SMS].[ServiceOrderHead]", "PK_ServiceOrderHead");
				Database.AddPrimaryKey("PK_ServiceOrderHead", "[SMS].[ServiceOrderHead]", "ContactKey");
			}
		}
	}
}
