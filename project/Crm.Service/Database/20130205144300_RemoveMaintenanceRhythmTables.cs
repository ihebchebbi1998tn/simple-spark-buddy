namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130205144300)]
	public class RemoveMaintenanceRhythmTables : Migration
	{
		public override void Up()
		{
			Database.RemoveTable("[SMS].[MaintenanceRhythms]");
			Database.RemoveTable("[SMS].[MaintenanceRhythmType]");
		}

		public override void Down()
		{
			if (!Database.TableExists("[SMS].[MaintenanceRhythms]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[MaintenanceRhythms](");
				query.AppendLine("[Id] [int] IDENTITY(1,1) NOT NULL,");
				query.AppendLine("[MaintenanceContractNo] [nvarchar](20) NOT NULL,");
				query.AppendLine("[MaintenanceType] [nvarchar](10) NOT NULL,");
				query.AppendLine("[FirstMaintenance] [datetime] NOT NULL,");
				query.AppendLine("[Rhythm] [int] NOT NULL,");
				query.AppendLine("[RhythmType] [int] NOT NULL,");
				query.AppendLine("[QualityPlanId] [int] NULL,");
				query.AppendLine("[LatencyLower] [int] NULL,");
				query.AppendLine("[LatencyUpper] [int] NULL,");
				query.AppendLine("[CreateDate] [datetime] NOT NULL,");
				query.AppendLine("[CreatorId] [uniqueidentifier] NOT NULL,");
				query.AppendLine("[ModifyDate] [datetime] NULL,");
				query.AppendLine("[ModifyId] [uniqueidentifier] NULL,");
				query.AppendLine("[InstallationNo] [nvarchar](30) NULL,");
				query.AppendLine("[TemplateOrderNo] [nvarchar](20) NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_MaintenanceRhythms] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[Id] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceRhythms] ADD  CONSTRAINT [DF_MaintenanceRhythms_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceRhythms] ADD  CONSTRAINT [DF_MaintenanceRhythms_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}

			if (!Database.TableExists("[SMS].[MaintenanceRhythmType]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[MaintenanceRhythmType](");
				query.AppendLine("[MaintenanceRhythmTypeId] [int] IDENTITY(1,1) NOT NULL,");
				query.AppendLine("[Name] [nvarchar](50) NOT NULL,");
				query.AppendLine("[Language] [char](2) NULL,");
				query.AppendLine("[Value] [int] NOT NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_MaintenanceRhythmType] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[MaintenanceRhythmTypeId] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("SET ANSI_PADDING OFF");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceRhythmType] ADD  CONSTRAINT [DF_MaintenanceRhythmType_Language]  DEFAULT ('en') FOR [Language]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceRhythmType] ADD  CONSTRAINT [DF_MaintenanceRhythmType_Value]  DEFAULT ((0)) FOR [Value]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceRhythmType] ADD  CONSTRAINT [DF_MaintenanceRhythmType_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceRhythmType] ADD  CONSTRAINT [DF_MaintenanceRhythmType_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}