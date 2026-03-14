namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130205141000)]
	public class RemoveMaintenanceContractTables : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[Old_MaintenancePlan]"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[Old_MaintenancePlan] DROP FK_MaintenancePlan_MaintenanceContractHead");
			}

			Database.RemoveTable("[SMS].[MaintenanceContractHead]");
			Database.RemoveTable("[SMS].[MaintenanceContractStatus]");
		}

		public override void Down()
		{
			if (!Database.TableExists("[SMS].[MaintenanceContractHead]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[MaintenanceContractHead](");
				query.AppendLine("[MaintenanceContractNo] [nvarchar](20) NOT NULL,");
				query.AppendLine("[StartDate] [datetime] NOT NULL,");
				query.AppendLine("[EndDate] [datetime] NULL,");
				query.AppendLine("[ContractType] [int] NOT NULL,");
				query.AppendLine("[CancellingDue] [int] NOT NULL,");
				query.AppendLine("[InitialBudget] [money] NOT NULL,");
				query.AppendLine("[CurrentBudget] [money] NOT NULL,");
				query.AppendLine("[CurrentBudgetUsed] [money] NOT NULL,");
				query.AppendLine("[CustomerContactId] [int] NOT NULL,");
				query.AppendLine("[InternalContactId] [int] NULL,");
				query.AppendLine("[ReactionTime] [int] NULL,");
				query.AppendLine("[InstallationNo] [nvarchar](30) NULL,");
				query.AppendLine("[ErectionDate] [datetime] NULL,");
				query.AppendLine("[TravelCostFixed] [money] NULL,");
				query.AppendLine("[TravelDistanceFixed] [int] NULL,");
				query.AppendLine("[TravelCostRefId] [int] NULL,");
				query.AppendLine("[PaymentType] [int] NULL,");
				query.AppendLine("[ParkNo] [nvarchar](20) NULL,");
				query.AppendLine("[Status] [int] NOT NULL,");
				query.AppendLine("[Remark] [text] NULL,");
				query.AppendLine("[CreateDate] [datetime] NOT NULL,");
				query.AppendLine("[CreatorId] [uniqueidentifier] NOT NULL,");
				query.AppendLine("[ModifyDate] [datetime] NULL,");
				query.AppendLine("[ModifyId] [uniqueidentifier] NULL,");
				query.AppendLine("[GoLiveDate] [datetime] NULL,");
				query.AppendLine("[TravelCostZone] [nvarchar](20) NULL,");
				query.AppendLine("[WarrantyStart] [datetime] NULL,");
				query.AppendLine("[WarrantyEnd] [datetime] NULL,");
				query.AppendLine("[ContactLink01] [int] NULL,");
				query.AppendLine("[ContactLink02] [int] NULL,");
				query.AppendLine("[ContactLink03] [int] NULL,");
				query.AppendLine("[UserFlag01] [int] NOT NULL,");
				query.AppendLine("[UserFlag02] [int] NOT NULL,");
				query.AppendLine("[UserFlag03] [int] NOT NULL,");
				query.AppendLine("[UserFlag04] [int] NOT NULL,");
				query.AppendLine("[UserFlag05] [int] NOT NULL,");
				query.AppendLine("[UserString01] [nvarchar](100) NULL,");
				query.AppendLine("[UserString02] [nvarchar](100) NULL,");
				query.AppendLine("[UserString03] [nvarchar](100) NULL,");
				query.AppendLine("[UserString04] [nvarchar](100) NULL,");
				query.AppendLine("[UserString05] [nvarchar](100) NULL,");
				query.AppendLine("[BudgetCurrency] [nvarchar](20) NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_MaintenenceOrderHead] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[MaintenanceContractNo] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractHead] ADD  CONSTRAINT [DF_MaintenanceContractHead_UserFlag01]  DEFAULT ((0)) FOR [UserFlag01]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractHead] ADD  CONSTRAINT [DF_MaintenanceContractHead_UserFlag02]  DEFAULT ((0)) FOR [UserFlag02]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractHead] ADD  CONSTRAINT [DF_MaintenanceContractHead_UserFlag03]  DEFAULT ((0)) FOR [UserFlag03]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractHead] ADD  CONSTRAINT [DF_MaintenanceContractHead_UserFlag04]  DEFAULT ((0)) FOR [UserFlag04]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractHead] ADD  CONSTRAINT [DF_MaintenanceContractHead_UserFlag05]  DEFAULT ((0)) FOR [UserFlag05]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractHead] ADD  CONSTRAINT [DF_MaintenanceContractHead_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractHead] ADD  CONSTRAINT [DF_MaintenanceContractHead_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}

			if (!Database.TableExists("[SMS].[MaintenanceContractStatus]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[MaintenanceContractStatus](");
				query.AppendLine("[MaintenanceContractStatusId] [int] IDENTITY(1,1) NOT NULL,");
				query.AppendLine("[Name] [nvarchar](50) NOT NULL,");
				query.AppendLine("[Language] [char](2) NULL,");
				query.AppendLine("[Value] [int] NOT NULL,");
				query.AppendLine("[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_MaintenanceContractStatus] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[MaintenanceContractStatusId] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("SET ANSI_PADDING OFF");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractStatus] ADD  CONSTRAINT [DF_MaintenanceContractStatus_Language]  DEFAULT ('en') FOR [Language]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractStatus] ADD  CONSTRAINT [DF_MaintenanceContractStatus_Value]  DEFAULT ((0)) FOR [Value]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractStatus] ADD  CONSTRAINT [DF_MaintenanceContractStatus_rowguid]  DEFAULT (newsequentialid()) FOR [rowguid]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractStatus] ADD  CONSTRAINT [DF_MaintenanceContractStatus_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceContractStatus] ADD  CONSTRAINT [DF_MaintenanceContractStatus_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}