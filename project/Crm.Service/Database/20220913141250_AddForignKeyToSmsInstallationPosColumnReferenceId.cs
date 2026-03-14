namespace Crm.Service.Database
{
	using System.Text;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220913141250)]
	public class AddForignKeyToSmsInstallationPosColumnReferenceId : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE p1 SET p1.[ReferenceId] = NULL FROM [SMS].[InstallationPos] p1 LEFT OUTER JOIN [SMS].[InstallationPos] p2 on p2.[id] = p1.[ReferenceId] WHERE p1.[ReferenceId] IS NOT NULL AND p2.[id] IS NULL");
			
			if (Database.ColumnExists("[SMS].[InstallationPos]", "ReferenceId") && !Database.ConstraintExists("[SMS].[InstallationPos]", "FK_InstallationPos_Parent"))
			{
				Database.AddForeignKey("FK_InstallationPos_Parent", "SMS.InstallationPos", "ReferenceId", "SMS.InstallationPos", "id");
			}

			if (!Database.IndexExists("[SMS].[InstallationPos]", "IX_InstallationPos_ReferenceId"))
			{
				Database.ExecuteNonQuery(@"	CREATE NONCLUSTERED INDEX IX_InstallationPos_ReferenceId
												ON [SMS].[InstallationPos] ([ReferenceId]) include ([IsActive], [IsInstalled], [InstallationId])");
			}
		}
	}
}
