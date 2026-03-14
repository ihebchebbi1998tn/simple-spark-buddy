namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180629101400)]
	public class ChangeCrmTaskResponsibleGroupToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='Task' AND COLUMN_NAME='ResponsibleGroup' AND DATA_TYPE = 'nvarchar')
				BEGIN
					EXEC sp_rename '[CRM].[Task].[ResponsibleGroup]', 'ResponsibleGroupOld', 'COLUMN'
					ALTER TABLE [CRM].[Task] ADD [ResponsibleGroup] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[ResponsibleGroup] = b.[UsergroupId] FROM [CRM].[Task] a LEFT OUTER JOIN [CRM].[Usergroup] b ON a.[ResponsibleGroupOld] = CONVERT(nvarchar, b.[UserGroupIdOld])')
					ALTER TABLE [CRM].[Task] ADD CONSTRAINT [FK_Task_Usergroup] FOREIGN KEY([ResponsibleGroup]) REFERENCES [CRM].[Usergroup]([UsergroupId]) ON UPDATE CASCADE ON DELETE SET NULL
				END");
		}
	}
}
