namespace Crm.Service.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130205145300)]
	public class RemoveMaintenanceTypeTable : Migration
	{
		public override void Up()
		{
			Database.RemoveTable("[SMS].[MaintenanceType]");
		}

		public override void Down()
		{
			if (!Database.TableExists("[SMS].[MaintenanceType]"))
			{
				var query = new StringBuilder();

				query.AppendLine("CREATE TABLE [SMS].[MaintenanceType](");
				query.AppendLine("[MaintenanceTypeId] [int] IDENTITY(1,1) NOT NULL,");
				query.AppendLine("[Name] [nvarchar](50) NOT NULL,");
				query.AppendLine("[Language] [char](2) NULL,");
				query.AppendLine("[Value] [nvarchar](10) NOT NULL,");
				query.AppendLine("[Favorite] [bit] NOT NULL,");
				query.AppendLine("[SortOrder] [int] NOT NULL,");
				query.AppendLine("CONSTRAINT [PK_MaintenanceType] PRIMARY KEY CLUSTERED ");
				query.AppendLine("(");
				query.AppendLine("[MaintenanceTypeId] ASC");
				query.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
				query.AppendLine(") ON [PRIMARY]");

				query.AppendLine("GO");

				query.AppendLine("SET ANSI_PADDING OFF");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceType] ADD  CONSTRAINT [DF_MaintenanceType_Language]  DEFAULT ('en') FOR [Language]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceType] ADD  CONSTRAINT [DF_MaintenanceType_Value]  DEFAULT ((0)) FOR [Value]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceType] ADD  CONSTRAINT [DF_MaintenanceType_Favorite]  DEFAULT ((0)) FOR [Favorite]");
				query.AppendLine("GO");

				query.AppendLine("ALTER TABLE [SMS].[MaintenanceType] ADD  CONSTRAINT [DF_MaintenanceType_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
				query.AppendLine("GO");

				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}