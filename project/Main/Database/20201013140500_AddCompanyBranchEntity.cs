namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201013140500)]
	public class AddCompanyBranchEntity : Migration
	{
		public override void Up()
		{
			const string tableName = "[CRM].[CompanyBranch]";
			if (Database.TableExists(tableName))
			{
				return;
			}
			Database.ExecuteNonQuery(@"
				CREATE TABLE [CRM].[CompanyBranch](
					[CompanyBranchId] [uniqueidentifier] NOT NULL,
					[CompanyKey] [uniqueidentifier] NOT NULL,
					[Branch1Key] [nvarchar](255) NULL,
					[Branch2Key] [nvarchar](255) NULL,
					[Branch3Key] [nvarchar](255) NULL,
					[Branch4Key] [nvarchar](255) NULL,
					[IsActive] [bit] NOT NULL,
					[CreateDate] [datetime] NOT NULL,
					[ModifyDate] [datetime] NOT NULL,
					[CreateUser] [nvarchar](256) NOT NULL,
					[ModifyUser] [nvarchar](256) NOT NULL,
					CONSTRAINT [PK_CompanyBranch_1] PRIMARY KEY CLUSTERED 
					(
						[CompanyBranchId] ASC
					)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
				) ON [PRIMARY]
					ALTER TABLE [CRM].[CompanyBranch] ADD  CONSTRAINT [DF_CompanyBranch_IsActive]  DEFAULT ((1)) FOR [IsActive]
					ALTER TABLE [CRM].[CompanyBranch] ADD  CONSTRAINT [DF_CompanyBranch_CreateDate]  DEFAULT (getutcdate()) FOR [CreateDate]
					ALTER TABLE [CRM].[CompanyBranch] ADD  CONSTRAINT [DF_CompanyBranch_ModifyDate]  DEFAULT (getutcdate()) FOR [ModifyDate]
			");
		}
	}
}
