namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180424161900)]
	public class AddColumnSignPrivacyPolicyAccepted : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderDispatch", new Column("SignPrivacyPolicyAccepted", DbType.Boolean, ColumnProperty.NotNull, 0));
		}
	}
}