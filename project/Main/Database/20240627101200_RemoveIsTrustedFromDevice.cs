namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240627101200)]
	public class RemoveIsTrustedFromDevice : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("[CRM].[Device]", "IsTrusted");
		}
	}
}
