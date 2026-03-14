namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180219180000)]
	public class MessageToGuid : Migration
	{
		public override void Up()
		{
			var messageIdType = Database.ExecuteScalar(@"
				SELECT t.[name]
				FROM sys.columns c
				JOIN sys.types t ON c.system_type_id = t.system_type_id
				WHERE object_id = object_id('CRM.Message') AND c.[name] = 'MessageId'").ToString();
			if (messageIdType == "int")
			{
				Database.ExecuteNonQuery(@"sp_rename 'CRM.Message.MessageId', 'MessageIdOld', 'COLUMN'");
				Database.ExecuteNonQuery(@"
					Declare @pkName nvarchar(200)
					SET @pkName = (SELECT Name FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('CRM.Message'))
					IF EXISTS (SELECT LEN(@pkName) having LEN(@pkName) > 0) BEGIN
						EXEC('ALTER TABLE [CRM].[Message] DROP CONSTRAINT [' + @pkName + ']')
					END;
				");
				Database.ExecuteNonQuery(@"
					ALTER TABLE CRM.[Message]
					ADD MessageId UNIQUEIDENTIFIER
					CONSTRAINT DF_Message_MessageId DEFAULT(NEWSEQUENTIALID())
					CONSTRAINT PK_Message PRIMARY KEY");
			}
		}
	}
}
