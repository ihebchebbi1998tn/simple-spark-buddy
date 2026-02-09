namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170331144500)]
	public class AddLuNoteType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
CREATE TABLE [LU].[NoteType](
	[NoteTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,		
	[Language] [nvarchar](2) NOT NULL,
	[Value] [nvarchar](256) NOT NULL,
	[Color] [nvarchar](32) NOT NULL,
	[Icon] [nvarchar](32) NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[TenantKey] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar](100) NOT NULL,
	[ModifyUser] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	CONSTRAINT [PK_NoteTypeId] PRIMARY KEY CLUSTERED ([NoteTypeId] ASC)
)

ALTER TABLE [LU].[NoteType] ADD  CONSTRAINT [DF_NoteType_Color] DEFAULT ('#00bcd4') FOR [Color]
ALTER TABLE [LU].[NoteType] ADD  CONSTRAINT [DF_NoteType_Icon] DEFAULT ('\\f25c') FOR [Icon]
			");
			Database.ExecuteNonQuery(@"
INSERT INTO [LU].[NoteType]
           ([Name]
           ,[Language]
           ,[Value]
		   ,[Color]
		   ,[Icon]
           ,[Favorite]
           ,[SortOrder]
           ,[TenantKey]
           ,[CreateDate]
           ,[ModifyDate]
           ,[CreateUser]
           ,[ModifyUser]
           ,[IsActive])
     VALUES
('E-Mail',					'de', 'EmailNote',			'#2196F3', '\f15a', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331144500', 'Migration_20170331144500', 1)
,('Email',					'en', 'EmailNote',			'#2196F3', '\f15a', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331144500', 'Migration_20170331144500', 1)

,('Benutzernotiz',			'de', 'UserNote',			'#2196f3', '\f25b', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331144500', 'Migration_20170331144500', 1)
,('User Note',				'en', 'UserNote',			'#2196f3', '\f25b', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331144500', 'Migration_20170331144500', 1)
	
,('Abgeschlossene Aufgabe', 'de', 'TaskCompletedNote',	'#4caf50', '\f108', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331144500', 'Migration_20170331144500', 1)
,('Completed Task', 		'en', 'TaskCompletedNote',	'#4caf50', '\f108', 0, 0, NULL, GETUTCDATE(), GETUTCDATE(), 'Migration_20170331144500', 'Migration_20170331144500', 1)
			");
		}
	}
}
