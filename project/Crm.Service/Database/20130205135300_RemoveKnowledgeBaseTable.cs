namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130205135300)]
	public class RemoveKnowledgeBaseTable : Migration
	{
		public override void Up()
		{
			Database.RemoveTable("[SMS].[KnowledgeBase]");
		}

		public override void Down()
		{
			if (!Database.TableExists("[SMS].[KnowledgeBase]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[KnowledgeBase](");
				query.AppendLine("[Id] [int] IDENTITY(1,1) NOT NULL,");
				query.AppendLine("[OrderNo] [nvarchar](20) NULL,");
				query.AppendLine("[InstallationNo] [nvarchar](30) NOT NULL,");
				query.AppendLine("[ErrorCode] [nvarchar](20) NOT NULL,");
				query.AppendLine("[Component] [nvarchar](20) NOT NULL,");
				query.AppendLine("[ErrorMessage] [text] NOT NULL,");
				query.AppendLine("[Solution] [text] NOT NULL,");
				query.AppendLine("[internalRemark] [text] NULL,");
				query.AppendLine("[relevant] [int] NOT NULL,");
				query.AppendLine("[FollowUp] [int] NULL,");
				query.AppendLine("[FollowUpRemark] [text] NULL,");
				query.AppendLine("[CreateDate] [datetime] NOT NULL,");
				query.AppendLine("[CreatorId] [uniqueidentifier] NOT NULL,");
				query.AppendLine("[ModifyDate] [datetime] NULL,");
				query.AppendLine("[ModifyId] [uniqueidentifier] NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_KnowledgeBase] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[Id] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[KnowledgeBase] ADD  CONSTRAINT [DF_KnowledgeBase_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[KnowledgeBase] ADD  CONSTRAINT [DF_KnowledgeBase_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}