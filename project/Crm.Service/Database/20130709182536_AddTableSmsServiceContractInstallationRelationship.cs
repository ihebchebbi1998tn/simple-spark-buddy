namespace Crm.Service.Database
{
	using System;
	using System.Data;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130709182536)]
	public class AddTableSmsServiceContractInstallationRelationship : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[ServiceContractInstallationRelationship]"))
			{
				Database.AddTable("[SMS].[ServiceContractInstallationRelationship]",
					new Column("ServiceContractInstallationRelationshipId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ServiceContractKey", DbType.Int32, ColumnProperty.NotNull),
					new Column("InstallationKey", DbType.Int32, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("Information", DbType.String, Int32.MaxValue, ColumnProperty.Null),
					new Column("TimeAllocation", DbType.DateTime, ColumnProperty.Null));

				var migrate = new StringBuilder();
				migrate.AppendLine("INSERT INTO [SMS].[ServiceContractInstallationRelationship]");
				migrate.AppendLine("(ServiceContractKey, InstallationKey, CreateDate, CreateUser, ModifyDate, ModifyUser, Information, TimeAllocation)");
				migrate.AppendLine("SELECT sci.[ServiceContractKey], i.[ContactKey], GETUTCDATE(), 'Setup', GETUTCDATE(), 'Setup', NULL, NULL");
				migrate.AppendLine("FROM [SMS].[ServiceContractInstallation] sci");
				migrate.AppendLine("JOIN [SMS].[InstallationHead] i ON i.InstallationNo = sci.InstallationNo");
				Database.ExecuteNonQuery(migrate.ToString());
				
				Database.RemoveTable("[SMS].[ServiceContractInstallation]");
			}
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}