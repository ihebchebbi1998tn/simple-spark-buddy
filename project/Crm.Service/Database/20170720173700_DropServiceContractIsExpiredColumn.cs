namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170720173700)]
	public class DropServiceContractIsExpiredColumn : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"DECLARE @ConstraintName NVARCHAR(200)
																SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('[SMS].[ServiceContract]') AND PARENT_COLUMN_ID = (SELECT column_id FROM SYS.COLUMNS WHERE NAME = N'IsExpired' AND OBJECT_ID = OBJECT_ID(N'[SMS].[ServiceContract]'))
																IF @ConstraintName IS NOT NULL
																	EXEC('ALTER TABLE [SMS].[ServiceContract] DROP CONSTRAINT ' + @ConstraintName)
																IF EXISTS (SELECT * FROM SYSCOLUMNS WHERE ID=OBJECT_ID(N'[Sms].[ServiceContract]') AND NAME=N'IsExpired')
																	EXEC('ALTER TABLE [SMS].[ServiceContract] DROP COLUMN IsExpired')");
		}
	}
}