namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140502233100)]
    public class  AdjustServiceOrderTimesConstraints : Migration
	{
		public override void Up()
		{
            Database.ExecuteNonQuery(@"IF (SELECT   count( * )     FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS     WHERE CONSTRAINT_NAME ='FK_ServiceOrderTimes_User') >0
ALTER TABLE [SMS].[ServiceOrderTimes] DROP CONSTRAINT [FK_ServiceOrderTimes_User]
");
            Database.ExecuteNonQuery(@"IF (SELECT   count( * )     FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS     WHERE CONSTRAINT_NAME ='FK_ServiceOrderTimes_User1') >0
ALTER TABLE [SMS].[ServiceOrderTimes] DROP CONSTRAINT [FK_ServiceOrderTimes_User1]
");

            Database.ExecuteNonQuery(@"if(SELECT   count(*) FROM sys.tables t
INNER JOIN sys.default_constraints dc ON t.object_id = dc.parent_object_id
INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND c.column_id = dc.parent_column_id
WHERE c.object_id =  OBJECT_ID('[SMS].[ServiceOrderTimes]')  and c.Name ='CreateUser') <1
ALTER TABLE [SMS].[ServiceOrderTimes] ADD  CONSTRAINT [DF__SMS_ServiceOrder__CreateUser]  DEFAULT ('Anonymous') FOR [CreateUser]");


        
        }

		public override void Down()
		{
		}
	}
}