namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130205134300)]
	public class RemoveInfoBoardTables : Migration
	{
		public override void Up()
		{
			Database.RemoveTable("[SMS].[InfoBoardConfirmations]");
			Database.RemoveTable("[SMS].[InfoBoardEntries]");
		}

		public override void Down()
		{
			if (!Database.TableExists("[SMS].[InfoBoardConfirmations]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[InfoBoardConfirmations](");
				query.AppendLine("[id] [int] IDENTITY(1,1) NOT NULL,");
				query.AppendLine("[InfoBoardId] [int] NOT NULL,");
				query.AppendLine("[UserId] [int] NOT NULL,");
				query.AppendLine("[Confirmed] [int] NULL,");
				query.AppendLine("[ConirmDate] [datetime] NULL,");
				query.AppendLine("[CreateDate] [datetime] NOT NULL,");
				query.AppendLine("[CreatorId] [uniqueidentifier] NOT NULL,");
				query.AppendLine("[ModifyDate] [datetime] NULL,");
				query.AppendLine("[ModifyId] [uniqueidentifier] NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_InfoBoardConfirmations] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[id] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[InfoBoardConfirmations] ADD  CONSTRAINT [DF_InfoBoardConfirmation_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[InfoBoardConfirmations] ADD  CONSTRAINT [DF_InfoBoardConfirmation_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}

			if (!Database.TableExists("[SMS].[InfoBoardEntries]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[InfoBoardEntries](");
				query.AppendLine("[id] [int] IDENTITY(1,1) NOT NULL,");
				query.AppendLine("[Title] [text] NOT NULL,");
				query.AppendLine("[ContextText] [text] NOT NULL,");
				query.AppendLine("[MustConfirm] [int] NOT NULL,");
				query.AppendLine("[CreateDate] [datetime] NOT NULL,");
				query.AppendLine("[CreatorId] [uniqueidentifier] NOT NULL,");
				query.AppendLine("[ModifyDate] [datetime] NULL,");
				query.AppendLine("[ModifyId] [uniqueidentifier] NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_InfoBoardEntries] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[id] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[InfoBoardEntries] ADD  CONSTRAINT [DF_InfoBoardEntries_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[InfoBoardEntries] ADD  CONSTRAINT [DF_InfoBoardEntries_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}