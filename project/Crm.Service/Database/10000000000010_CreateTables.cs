namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(10000000000010)]
	public class CreateTables : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[SchemaInfo_Crm_Service]") > 0)
			{
				return;
			}

			Database.ExecuteNonQuery(@"
CREATE TABLE [LU].[CauseOfFailure](
	[CauseOfFailureId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCauseOfFailure_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCauseOfFailure_ModifyUser] DEFAULT ('Modifier'),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUCauseOfFailure_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUCauseOfFailure_ModifyDate] DEFAULT (getutcdate()),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUCauseOfFailure_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[CauseOfFailure] ADD CONSTRAINT [PK__CauseOfF__A9B0C1820CFADF99] PRIMARY KEY
	CLUSTERED
	(
		[CauseOfFailureId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[InstallationAddressRelationshipType](
	[InstallationAddressRelationshipTypeId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUInstallationAddressRelationshipType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUInstallationAddressRelationshipType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUInstallationAddressRelationshipType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUInstallationAddressRelationshipType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUInstallationAddressRelationshipType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[InstallationAddressRelationshipType] ADD CONSTRAINT [PK__Installa__018D4819597B3B93] PRIMARY KEY
	CLUSTERED
	(
		[InstallationAddressRelationshipTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[Manufacturer](
	[ManufacturerId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUManufacturer_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUManufacturer_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUManufacturer_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUManufacturer_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUManufacturer_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[Manufacturer] ADD CONSTRAINT [PK__Manufact__357E5CC15E3FF0B0] PRIMARY KEY
	CLUSTERED
	(
		[ManufacturerId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[NoCausingItemPreviousSerialNoReason](
	[NoCausingItemPreviousSerialNoReasonId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__NoCausing__Favor__1960B67E] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__NoCausing__SortO__1A54DAB7] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUNoCausingItemPreviousSerialNoReason_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUNoCausingItemPreviousSerialNoReason_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUNoCausingItemPreviousSerialNoReason_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUNoCausingItemPreviousSerialNoReason_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUNoCausingItemPreviousSerialNoReason_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[NoCausingItemPreviousSerialNoReason] ADD CONSTRAINT [PK__NoCausin__D7B8C3F317786E0C] PRIMARY KEY
	CLUSTERED
	(
		[NoCausingItemPreviousSerialNoReasonId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[NoCausingItemSerialNoReason](
	[NoCausingItemSerialNoReasonId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__NoCausing__Favor__13A7DD28] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__NoCausing__SortO__149C0161] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUNoCausingItemSerialNoReason_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUNoCausingItemSerialNoReason_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUNoCausingItemSerialNoReason_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUNoCausingItemSerialNoReason_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUNoCausingItemSerialNoReason_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[NoCausingItemSerialNoReason] ADD CONSTRAINT [PK__NoCausin__1834D80111BF94B6] PRIMARY KEY
	CLUSTERED
	(
		[NoCausingItemSerialNoReasonId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[NoPreviousSerialNoReason](
	[NoPreviousSerialNoReasonId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__NoPreviou__Favor__7CC477D0] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__NoPreviou__SortO__7DB89C09] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUNoPreviousSerialNoReason_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUNoPreviousSerialNoReason_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUNoPreviousSerialNoReason_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUNoPreviousSerialNoReason_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUNoPreviousSerialNoReason_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[NoPreviousSerialNoReason] ADD CONSTRAINT [PK__NoPrevio__696289E27ADC2F5E] PRIMARY KEY
	CLUSTERED
	(
		[NoPreviousSerialNoReasonId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[ServiceContractAddressRelationshipType](
	[ServiceContractAddressRelationshipTypeId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUServiceContractAddressRelationshipType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUServiceContractAddressRelationshipType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUServiceContractAddressRelationshipType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUServiceContractAddressRelationshipType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUServiceContractAddressRelationshipType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[ServiceContractAddressRelationshipType] ADD CONSTRAINT [PK__ServiceC__A9B9BDB06D823440] PRIMARY KEY
	CLUSTERED
	(
		[ServiceContractAddressRelationshipTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[ServiceObjectCategory](
	[ServiceObjectCategoryId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ServiceOb__Favor__07420643] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ServiceOb__SortO__08362A7C] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUServiceObjectCategory_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUServiceObjectCategory_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUServiceObjectCategory_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUServiceObjectCategory_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUServiceObjectCategory_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceOb__Color__53433F50] DEFAULT ('#9E9E9E')
) ON [PRIMARY]

ALTER TABLE [LU].[ServiceObjectCategory] ADD CONSTRAINT [PK__ServiceO__2509DAD80559BDD1] PRIMARY KEY
	CLUSTERED
	(
		[ServiceObjectCategoryId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[SparePartsBudgetInvoiceType](
	[SparePartsBudgetInvoiceTypeId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUSparePartsBudgetInvoiceType_CreateDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUSparePartsBudgetInvoiceType_CreateUser] DEFAULT ('Creater'),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUSparePartsBudgetInvoiceType_ModifyDate] DEFAULT (getutcdate()),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUSparePartsBudgetInvoiceType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUSparePartsBudgetInvoiceType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[SparePartsBudgetInvoiceType] ADD CONSTRAINT [PK__SparePar__C2348C8E3AC1AA49] PRIMARY KEY
	CLUSTERED
	(
		[SparePartsBudgetInvoiceTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[SparePartsBudgetTimeSpanUnit](
	[SparePartsBudgetTimeSpanUnitId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUSparePartsBudgetTimeSpanUnit_CreateDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUSparePartsBudgetTimeSpanUnit_CreateUser] DEFAULT ('Creater'),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUSparePartsBudgetTimeSpanUnit_ModifyDate] DEFAULT (getutcdate()),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUSparePartsBudgetTimeSpanUnit_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUSparePartsBudgetTimeSpanUnit_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[SparePartsBudgetTimeSpanUnit] ADD CONSTRAINT [PK__SparePar__9FF70FC535FCF52C] PRIMARY KEY
	CLUSTERED
	(
		[SparePartsBudgetTimeSpanUnitId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[CommissioningStatus](
	[CommissioningStatusId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_CommissioningStatus_Language] DEFAULT ('en'),
	[Value] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CommissioningStatus_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Commissio__Favor__0880433F] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Commissio__SortO__09746778] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSCommissioningStatus_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSCommissioningStatus_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSCommissioningStatus_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSCommissioningStatus_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSCommissioningStatus_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[CommissioningStatus] ADD CONSTRAINT [PK_CommissioningStatus_1] PRIMARY KEY
	CLUSTERED
	(
		[CommissioningStatusId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[Components](
	[ComponentId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_Components_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Component__Favor__07E124C1] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Component__SortO__08D548FA] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSComponents_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSComponents_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSComponents_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSComponents_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSComponents_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[Components] ADD CONSTRAINT [PK_Components] PRIMARY KEY
	CLUSTERED
	(
		[ComponentId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]


CREATE TABLE [SMS].[ErrorCode](
	[ErrorCodeId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (250) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ErrorCode_Language] DEFAULT ('en'),
	[Value] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Component] [int] NULL,
	[Priority] [int] NOT NULL CONSTRAINT [DF_ErrorCode_Priority] DEFAULT ((0)),
	[QualityPlanId] [int] NULL,
	[MonitoringCode] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[InstallationType] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[RdsPpClassification] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StandardAction] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[StandardActionExecuteCount] [int] NULL,
	[StandardActionExecuteTimeout] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[TemplateOrderNo] [nchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ReactionTime] [int] NULL,
	[ReactionTimeType] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[BpChecklistId] [int] NULL,
	[Remark] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ErrorCode__Favor__02284B6B] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ErrorCode__SortO__031C6FA4] DEFAULT ((0)),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSErrorCode_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSErrorCode_ModifyUser] DEFAULT ('Modifier'),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSErrorCode_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSErrorCode_ModifyDate] DEFAULT (getutcdate()),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSErrorCode_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ErrorCode] ADD CONSTRAINT [PK_ErrorCode] PRIMARY KEY
	CLUSTERED
	(
		[ErrorCodeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[InstallationHeadStatus](
	[InstallationHeadStatusId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_InstallationHeadStatus_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Installat__Favor__38EE7070] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Installat__SortO__39E294A9] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSInstallationHeadStatus_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSInstallationHeadStatus_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSInstallationHeadStatus_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSInstallationHeadStatus_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSInstallationHeadStatus_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Installat__Color__7F56CBB8] DEFAULT ('#9E9E9E')
) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationHeadStatus] ADD CONSTRAINT [PK_InstallationHeadStatus] PRIMARY KEY
	CLUSTERED
	(
		[InstallationHeadStatusId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[InstallationType](
	[InstallationTypeId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (250) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_InstallationType_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_InstallationType_Value] DEFAULT ('0'),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Installat__Favor__1FCDBCEB] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Installat__SortO__20C1E124] DEFAULT ((0)),
	[GroupKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSInstallationType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSInstallationType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSInstallationType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSInstallationType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSInstallationType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationType] ADD CONSTRAINT [PK_InstallationType] PRIMARY KEY
	CLUSTERED
	(
		[InstallationTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[MonitoringDataType](
	[MonitoringDataTypeId] [int] NOT NULL IDENTITY (1,1) NOT FOR REPLICATION,
	[Name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_MonitoringDataType_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_MonitoringDataType_Value] DEFAULT ((0)),
	[DataTypeKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Min] [float] (53) NULL,
	[Max] [float] (53) NULL,
	[BitIndex] [int] NULL,
	[QuantityUnit] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Monitorin__Favor__02C769E9] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Monitorin__SortO__03BB8E22] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSMonitoringDataType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSMonitoringDataType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSMonitoringDataType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSMonitoringDataType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSMonitoringDataType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[MonitoringDataType] ADD CONSTRAINT [PK_MonitoringDataType_1] PRIMARY KEY
	CLUSTERED
	(
		[MonitoringDataTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[NotificationStandardAction](
	[NotificationStandardActionId] [int] NOT NULL IDENTITY (1,1) NOT FOR REPLICATION,
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_NotificationStandardAction_Language] DEFAULT ('en'),
	[Value] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_NotificationStandardAction_Value] DEFAULT ((0)),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [MSmerge_df_rowguid_EB4545EAA4B04AD2B33FAE6B9EA29A6F] DEFAULT (newsequentialid()) ROWGUIDCOL,
	[TemplateOrderNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[TargetStatus] [int] NULL,
	[WorkflowTarget] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Notificat__Favor__65C116E7] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Notificat__SortO__66B53B20] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSNotificationStandardAction_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSNotificationStandardAction_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSNotificationStandardAction_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSNotificationStandardAction_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSNotificationStandardAction_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[NotificationStandardAction] ADD CONSTRAINT [PK_NotificationStandardAction_1] PRIMARY KEY
	CLUSTERED
	(
		[NotificationStandardActionId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[RdsPpStructure](
	[RdsPpStructureId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_RdsPpStructure_RdsPpStructureId] DEFAULT (newsequentialid()),
	[ParentRdsPpStructureKey] [uniqueidentifier] NULL,
	[Description] [nvarchar] (200) COLLATE Latin1_General_CI_AS NOT NULL,
	[RdsPpClassification] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[UniqueId] [int] NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__RdsPpStru__Favor__3C54ED00] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__RdsPpStru__SortO__3D491139] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [SMS].[RdsPpStructure] ADD CONSTRAINT [PK_RdsPpStructure_1] PRIMARY KEY
	CLUSTERED
	(
		[RdsPpStructureId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ReplenishmentOrder](
	[ResponsibleUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsClosed] [bit] NOT NULL,
	[CloseDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Replenish__IsAct__27AED5D5] DEFAULT ((1)),
	[ReplenishmentOrderIdOld] [int] NULL,
	[ReplenishmentOrderId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__Replenish__Reple__573DED66] DEFAULT (newsequentialid()),
	[IsSent] [bit] NOT NULL CONSTRAINT [DF__Replenish__IsSen__6576FE24] DEFAULT ((0)),
	[RetryCounter] [int] NOT NULL CONSTRAINT [DF__Replenish__Retry__666B225D] DEFAULT ((0)),
	[ClosedBy] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[IsExported] [bit] NOT NULL CONSTRAINT [DF__Replenish__IsExp__210CC53E] DEFAULT ((0)),
	[SendingError] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ReplenishmentOrder] ADD CONSTRAINT [PK_ReplenishmentOrder] PRIMARY KEY
	CLUSTERED
	(
		[ReplenishmentOrderId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceContractLimitType](
	[ServiceContractLimitTypeId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceContractLimitType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceContractLimitType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceContractLimitType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceContractLimitType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceContractLimitType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContractLimitType] ADD CONSTRAINT [PK__ServiceC__C77C6327353DDB1D] PRIMARY KEY
	CLUSTERED
	(
		[ServiceContractLimitTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceContractStatus](
	[ServiceContractStatusId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
	[Language] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Favorite] [bit] NULL,
	[SortOrder] [int] NULL,
	[SettableStatuses] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NULL,
	[ModifyDate] [datetime] NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceContractType](
	[ServiceContractTypeId] [int] NOT NULL IDENTITY (1,1) NOT FOR REPLICATION,
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_ServiceContractType_Color] DEFAULT ('#000000'),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ServiceCo__Favor__536D5C82] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ServiceCo__SortO__546180BB] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceContractType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceContractType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceContractType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceContractType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceContractType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContractType] ADD CONSTRAINT [PK_MaintenanceContractHeadType] PRIMARY KEY
	CLUSTERED
	(
		[ServiceContractTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceNotificationCategory](
	[ServiceNotificationCategoryId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceNotificationCategory_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceNotificationCategory_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceNotificationCategory_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceNotificationCategory_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceNotificationCategory_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceNo__Color__5713D034] DEFAULT ('#9E9E9E')
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceNotificationCategory] ADD CONSTRAINT [PK_ServiceNotificationCategory] PRIMARY KEY
	CLUSTERED
	(
		[ServiceNotificationCategoryId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceNotificationStatus](
	[ServiceNotificationStatusId] [int] NOT NULL IDENTITY (1,1) NOT FOR REPLICATION,
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ServiceNotificationStatus_Language] DEFAULT ('en'),
	[Value] [int] NOT NULL CONSTRAINT [DF_ServiceNotificationStatus_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ServiceNo__Favor__467D75B8] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ServiceNo__SortO__477199F1] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceNotificationStatus_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceNotificationStatus_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceNotificationStatus_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceNotificationStatus_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceNotificationStatus_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceNo__Color__5807F46D] DEFAULT ('#9E9E9E'),
	[Groups] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[SettableStatuses] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceNotificationStatus] ADD CONSTRAINT [PK_ServiceNotificationStatus] PRIMARY KEY
	CLUSTERED
	(
		[ServiceNotificationStatusId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderDispatchRejectReason](
	[ServiceOrderDispatchRejectReasonId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchRejectReason_CreateDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchRejectReason_CreateUser] DEFAULT ('Creater'),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchRejectReason_ModifyDate] DEFAULT (getutcdate()),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchRejectReason_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchRejectReason_IsActive] DEFAULT ((1)),
	[ServiceOrderStatus] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[ShowInMobileClient] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__ShowI__4B380934] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderDispatchRejectReason] ADD CONSTRAINT [PK__ServiceO__AEC00E1840AF8DC9] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderDispatchRejectReasonId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderDispatchStatus](
	[ServiceOrderDispatchTechnicianStatusId] [int] NOT NULL IDENTITY (1,1),
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_ServiceOrderDispatchTechnicianStatus_Language] DEFAULT ('en'),
	[Name] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_ServiceOrderDispatchTechnicianStatus_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF_ServiceOrderDispatchTechnicianStatus_Favorite] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_ServiceOrderDispatchTechnicianStatus_SortOrder] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchStatus_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchStatus_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchStatus_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchStatus_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderDispatchStatus_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) NOT NULL CONSTRAINT [DF__ServiceOr__Color__554B8353] DEFAULT ('#AAAAAA')
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderDispatchStatus] ADD CONSTRAINT [PK_ServiceOrderDispatchTechnicianStatus] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderDispatchTechnicianStatusId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderInvoiceReason](
	[ServiceOrderInvoiceReasonId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderInvoiceReason_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderInvoiceReason_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderInvoiceReason_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderInvoiceReason_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderInvoiceReason_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderInvoiceReason] ADD CONSTRAINT [PK__ServiceO__A9D563601D314762] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderInvoiceReasonId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderNoInvoiceReason](
	[ServiceOrderNoInvoiceReasonId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderNoInvoiceReason_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderNoInvoiceReason_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderNoInvoiceReason_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderNoInvoiceReason_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderNoInvoiceReason_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderNoInvoiceReason] ADD CONSTRAINT [PK__ServiceO__F10E2735457442E6] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderNoInvoiceReasonId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderStatus](
	[ServiceOrderStatusId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ServiceOrderHeadStatus_Language] DEFAULT ('en'),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Favor__15702A09] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ServiceOr__SortO__25A691D2] DEFAULT ((0)),
	[Groups] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
	[SettableStatuses] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderStatus_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderStatus_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderStatus_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderStatus_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderStatus_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderStatus] ADD CONSTRAINT [PK_ServiceOrderHeadStatus] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderStatusId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderTimeCategory](
	[ServiceOrderTimeCategoryId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeCategory_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeCategory_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeCategory_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeCategory_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeCategory_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimeCategory] ADD CONSTRAINT [PK__ServiceO__540C67677CA47C3F] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderTimeCategoryId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderTimeLocation](
	[ServiceOrderTimeLocationId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeLocation_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeLocation_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeLocation_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeLocation_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeLocation_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimeLocation] ADD CONSTRAINT [PK__ServiceO__B568B1DF1F98B2C1] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderTimeLocationId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderTimePriority](
	[ServiceOrderTimePriorityId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimePriority_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimePriority_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimePriority_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimePriority_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimePriority_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimePriority] ADD CONSTRAINT [PK__ServiceO__75E9EC7B49AEE81E] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderTimePriorityId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderTimeStatus](
	[ServiceOrderTimeStatusId] [int] NOT NULL IDENTITY (1,1) NOT FOR REPLICATION,
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ServiceOrderTimeStatus_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_ServiceOrderTimeStatus_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Favor__5FB337D6] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ServiceOr__SortO__60A75C0F] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeStatus_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeStatus_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeStatus_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeStatus_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderTimeStatus_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceOr__Color__6FDF7DFE] DEFAULT ('#9E9E9E')
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimeStatus] ADD CONSTRAINT [PK_ServiceOrderTimeStatus_1] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderTimeStatusId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceOrderType](
	[ServiceOrderType] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ServiceOrderType_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Favor__72C60C4A] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ServiceOr__SortO__73BA3083] DEFAULT ((0)),
	[NumberingSequence] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__ServiceOr__Numbe__16EE5E27] DEFAULT ('SMS.ServiceOrder'),
	[MaintenanceOrder] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Maint__17E28260] DEFAULT ((0)),
	[ShowInMobileClient] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__ShowI__0EE3280B] DEFAULT ((1)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServiceOrderType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServiceOrderType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServiceOrderType_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceOr__Color__004AEFF1] DEFAULT ('#9E9E9E')
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderType] ADD CONSTRAINT [PK_ServiceOrderType] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderType] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServicePriority](
	[ServiceNotificationPriorityId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServicePriority_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSServicePriority_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServicePriority_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSServicePriority_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSServicePriority_IsActive] DEFAULT ((1)),
	[Color] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServicePr__Color__013F142A] DEFAULT ('#9E9E9E'),
	[IsFastLane] [bit] NOT NULL CONSTRAINT [DF__ServicePr__IsFas__08E035F2] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServicePriority] ADD CONSTRAINT [PK_ServiceNotificationPriority] PRIMARY KEY
	CLUSTERED
	(
		[ServiceNotificationPriorityId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]


CREATE TABLE [SMS].[InstallationAdditionalContacts](
	[InstallationIdOld] [int] NULL,
	[ContactIdOld] [int] NULL,
	[InstallationId] [uniqueidentifier] NOT NULL,
	[ContactId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD CONSTRAINT [PK_InstallationAdditionalContacts] PRIMARY KEY
	CLUSTERED
	(
		[InstallationId] ASC
		,[ContactId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD CONSTRAINT [FK_InstallationAdditionalContact_Contact] FOREIGN KEY
	(
		[ContactId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[InstallationAdditionalContacts] ADD CONSTRAINT [FK_InstallationAdditionalContact_Installation] FOREIGN KEY
	(
		[InstallationId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 


CREATE TABLE [SMS].[Location](
	[LocationNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSLocation_CreateDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSLocation_CreateUser] DEFAULT ('Creater'),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSLocation_ModifyDate] DEFAULT (getutcdate()),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSLocation_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSLocation_IsActive] DEFAULT ((1)),
	[LocationIdOld] [int] NULL,
	[LocationId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Location_LocationId] DEFAULT (newsequentialid()),
	[StoreId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[Location] ADD CONSTRAINT [PK_Location] PRIMARY KEY
	CLUSTERED
	(
		[LocationId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Location_StoreId] ON [SMS].[Location]
(
	[StoreId] ASC
)ON [PRIMARY]

ALTER TABLE [SMS].[Location] ADD CONSTRAINT [FK_Location_Store] FOREIGN KEY
	(
		[StoreId]
	)
	REFERENCES [SMS].[Store]
	(
		[StoreId]
	) 

CREATE TABLE [SMS].[MaintenancePlan](
	[ContactKeyOld] [int] NULL,
	[ServiceContractKeyOld] [int] NULL,
	[FirstDate] [datetime] NULL,
	[RhythmValue] [int] NULL,
	[RhythmUnitKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[NextDate] [datetime] NULL,
	[GenerateMaintenanceOrders] [bit] NOT NULL CONSTRAINT [DF__Maintenan__Gener__3726238F] DEFAULT ((1)),
	[AllowPrematureMaintenance] [bit] NOT NULL CONSTRAINT [DF__Maintenan__Allow__381A47C8] DEFAULT ((0)),
	[ContactKey] [uniqueidentifier] NOT NULL,
	[ServiceContractKey] [uniqueidentifier] NOT NULL,
	[ServiceOrderTemplateId] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[MaintenancePlan] ADD CONSTRAINT [PK_MaintenancePlan] PRIMARY KEY
	CLUSTERED
	(
		[ContactKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[MaintenancePlan] ADD CONSTRAINT [FK_MaintenancePlan_Contact] FOREIGN KEY
	(
		[ContactKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[MaintenancePlan] ADD CONSTRAINT [FK_MaintenancePlan_ServiceContract] FOREIGN KEY
	(
		[ServiceContractKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[MaintenancePlan] ADD CONSTRAINT [FK_MaintenancePlan_ServiceOrderTemplate] FOREIGN KEY
	(
		[ServiceOrderTemplateId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

CREATE TABLE [SMS].[ReplenishmentOrderItem](
	[ArticleNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Description] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
	[Quantity] [decimal] (19,5) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[QuantityUnitKey] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[Remark] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Replenish__IsAct__28A2FA0E] DEFAULT ((1)),
	[ReplenishmentOrderItemIdOld] [int] NULL,
	[ReplenishmentOrderItemId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__Replenish__Reple__5B0E7E4A] DEFAULT (newsequentialid()),
	[ReplenishmentOrderKeyOld] [int] NULL,
	[ReplenishmentOrderKey] [uniqueidentifier] NULL,
	[ArticleId] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD CONSTRAINT [PK_ReplenishmentOrderItem] PRIMARY KEY
	CLUSTERED
	(
		[ReplenishmentOrderItemId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ReplenishmentOrderItem_ArticleId] ON [SMS].[ReplenishmentOrderItem]
(
	[ArticleId] ASC
)ON [PRIMARY]

ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD CONSTRAINT [FK_ReplenishmentOrderItem_Article] FOREIGN KEY
	(
		[ArticleId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ReplenishmentOrderItem] ADD CONSTRAINT [FK_ReplenishmentOrderItem_ReplenishmentOrder] FOREIGN KEY
	(
		[ReplenishmentOrderKey]
	)
	REFERENCES [SMS].[ReplenishmentOrder]
	(
		[ReplenishmentOrderId]
	) ON DELETE CASCADE ON UPDATE CASCADE

CREATE TABLE [SMS].[ServiceObject](
	[ContactKeyOld] [int] NULL,
	[Category] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ObjectNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ContactKey] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceObject] ADD CONSTRAINT [PK_ServiceObject] PRIMARY KEY
	CLUSTERED
	(
		[ContactKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceObject] ADD CONSTRAINT [FK_ServiceObject_Contact] FOREIGN KEY
	(
		[ContactKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

CREATE TABLE [SMS].[ServiceOrderSkill](
	[ServiceOrderIdOld] [int] NULL,
	[SkillKey] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
	[ServiceOrderId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderSkill] ADD CONSTRAINT [PK_ServiceOrderSkill] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderId] ASC
		,[SkillKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderSkill] ADD CONSTRAINT [FK_ServiceOrderSkill_ServiceOrder] FOREIGN KEY
	(
		[ServiceOrderId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 


CREATE TABLE [SMS].[InstallationAddressRelationship](
	[InstallationAddressRelationshipIdOld] [int] NOT NULL IDENTITY (1,1),
	[InstallationKeyOld] [int] NULL,
	[AddressKeyOld] [int] NULL,
	[RelationshipType] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Information] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Installat__IsAct__569ECEE8] DEFAULT ((1)),
	[AddressKey] [uniqueidentifier] NOT NULL,
	[InstallationKey] [uniqueidentifier] NOT NULL,
	[InstallationAddressRelationshipId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_InstallationAddressRelationship_InstallationAddressRelationshipId] DEFAULT (newsequentialid())
) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationAddressRelationship] ADD CONSTRAINT [PK_InstallationAddressRelationship] PRIMARY KEY
	CLUSTERED
	(
		[InstallationAddressRelationshipId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationAddressRelationship] ADD CONSTRAINT [FK_InstallationAddressRelationship_Address] FOREIGN KEY
	(
		[AddressKey]
	)
	REFERENCES [CRM].[Address]
	(
		[AddressId]
	) 

ALTER TABLE [SMS].[InstallationAddressRelationship] ADD CONSTRAINT [FK_InstallationAddressRelationship_Installation] FOREIGN KEY
	(
		[InstallationKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 


CREATE TABLE [SMS].[InstallationHead](
	[InstallationNo] [nvarchar] (30) COLLATE Latin1_General_CI_AS NOT NULL,
	[LegacyInstallationId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[LocationContactIdOld] [int] NULL,
	[Description] [nvarchar] (450) COLLATE Latin1_General_CI_AS NOT NULL,
	[Priority] [int] NOT NULL,
	[InstallationType] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[MaintenanceContractNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[SoftwareVersion] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Status] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[WarrantyUntil] [datetime2] (7) NULL,
	[LocationAddressKeyOld] [int] NULL,
	[ContactKeyOld] [int] NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Installat__Favor__3DB3258D] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Installat__SortO__4460231C] DEFAULT ((0)),
	[LocationPersonIdOld] [int] NULL,
	[KickOffDate] [datetime2] (7) NULL,
	[WarrantyFrom] [datetime2] (7) NULL,
	[PreferredUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[TechnicianInformation] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[Room] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[ExactPlace] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[ManufactureDate] [datetime] NULL,
	[Manufacturer] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[FolderKeyOld] [int] NULL,
	[ExternalReference] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[LocationAddressKey] [uniqueidentifier] NULL,
	[LocationContactId] [uniqueidentifier] NULL,
	[ContactKey] [uniqueidentifier] NOT NULL,
	[LocationPersonId] [uniqueidentifier] NULL,
	[FolderKey] [uniqueidentifier] NULL,
	[StationKey] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [PK_InstallationHead] PRIMARY KEY
	CLUSTERED
	(
		[ContactKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_InstallationHead_ContactKey] ON [SMS].[InstallationHead]
(
	[ContactKey] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_InstallationHead_LocationContactId] ON [SMS].[InstallationHead]
(
	[LocationContactId] ASC
)ON [PRIMARY]

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_Contact] FOREIGN KEY
	(
		[ContactKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_Folder] FOREIGN KEY
	(
		[FolderKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_LocationAddress] FOREIGN KEY
	(
		[LocationAddressKey]
	)
	REFERENCES [CRM].[Address]
	(
		[AddressId]
	) 

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_LocationContact] FOREIGN KEY
	(
		[LocationContactId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_LocationPerson] FOREIGN KEY
	(
		[LocationPersonId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_PreferredUser] FOREIGN KEY
	(
		[PreferredUser]
	)
	REFERENCES [CRM].[User]
	(
		[Username]
	) 

ALTER TABLE [SMS].[InstallationHead] ADD CONSTRAINT [FK_InstallationHead_Station] FOREIGN KEY
	(
		[StationKey]
	)
	REFERENCES [CRM].[Station]
	(
		[StationId]
	) 


CREATE TABLE [SMS].[ServiceContract](
	[ContactKeyOld] [int] NULL,
	[ContractNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ContractTypeKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ValidFrom] [datetime] NULL,
	[ValidTo] [datetime] NULL,
	[ReactionTimeFirstAnswerValue] [smallint] NULL,
	[ReactionTimeServiceCompletedValue] [smallint] NULL,
	[ReactionTimeFirstAnswerUnitKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ReactionTimeServiceCompletedUnitKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[BudgetServiceProvisionValue] [decimal] (10,2) NULL,
	[BudgetServiceProvisionUnitKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[BudgetServiceProvisionPerTimeSpanUnitKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[BudgetSparePartsValue] [decimal] (10,2) NULL,
	[BudgetSparePartsUnitKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[BudgetSparePartsPerTimeSpanUnitKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ExternalReference] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[LimitType] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[PaymentCondition] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[PaymentInterval] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[PaymentType] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[PriceCurrency] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[IncreasedPriceCurrency] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[InvoiceSpecialConditions] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[InternalInvoiceInformation] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Price] [money] NULL,
	[PriceModificationDate] [datetime] NULL,
	[NoPaymentsUntil] [datetime] NULL,
	[IncreasedPrice] [money] NULL,
	[IncreaseByPercent] [decimal] (19,5) NULL,
	[PriceGuaranteedUntil] [datetime] NULL,
	[InvoiceAddressKeyOld] [int] NULL,
	[InvoicedUntil] [datetime] NULL,
	[LastInvoiceNo] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL,
	[InvoiceRecipientIdOld] [int] NULL,
	[PayerIdOld] [int] NULL,
	[PayerAddressIdOld] [int] NULL,
	[SparePartsBudgetInvoiceType] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[ServiceObjectIdOld] [int] NULL,
	[InvoiceAddressKey] [uniqueidentifier] NULL,
	[PayerAddressId] [uniqueidentifier] NULL,
	[ContactKey] [uniqueidentifier] NOT NULL,
	[InvoiceRecipientId] [uniqueidentifier] NULL,
	[PayerId] [uniqueidentifier] NULL,
	[ServiceObjectId] [uniqueidentifier] NULL,
	[StatusKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceCo__Statu__6482D9EB] DEFAULT ('Active')
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [PK_ServiceContract] PRIMARY KEY
	CLUSTERED
	(
		[ContactKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [UC_ServiceContract_ContractNo] UNIQUE
	NONCLUSTERED
	(
		[ContractNo] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_Contact] FOREIGN KEY
	(
		[ContactKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_InvoiceAddress] FOREIGN KEY
	(
		[InvoiceAddressKey]
	)
	REFERENCES [CRM].[Address]
	(
		[AddressId]
	) 

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_InvoiceRecipient] FOREIGN KEY
	(
		[InvoiceRecipientId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_Payer] FOREIGN KEY
	(
		[PayerId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceContract_PayerAddress] FOREIGN KEY
	(
		[PayerAddressId]
	)
	REFERENCES [CRM].[Address]
	(
		[AddressId]
	) 

ALTER TABLE [SMS].[ServiceContract] ADD CONSTRAINT [FK_ServiceObject_ServiceObject] FOREIGN KEY
	(
		[ServiceObjectId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

CREATE TABLE [SMS].[ServiceContractAddressRelationship](
	[ServiceContractAddressRelationshipIdOld] [int] NOT NULL IDENTITY (1,1),
	[ServiceContractKeyOld] [int] NULL,
	[AddressKeyOld] [int] NULL,
	[RelationshipType] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Information] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceCo__IsAct__6AA5C795] DEFAULT ((1)),
	[AddressKey] [uniqueidentifier] NOT NULL,
	[ServiceContractKey] [uniqueidentifier] NOT NULL,
	[ServiceContractAddressRelationshipId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ServiceContractAddressRelationship_ServiceContractAddressRelationshipId] DEFAULT (newsequentialid())
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD CONSTRAINT [PK_ServiceContractAddressRelationship] PRIMARY KEY
	CLUSTERED
	(
		[ServiceContractAddressRelationshipId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD CONSTRAINT [FK_ServiceContractAddressRelationship_Address] FOREIGN KEY
	(
		[AddressKey]
	)
	REFERENCES [CRM].[Address]
	(
		[AddressId]
	) 

ALTER TABLE [SMS].[ServiceContractAddressRelationship] ADD CONSTRAINT [FK_ServiceContractAddressRelationship_ServiceContract] FOREIGN KEY
	(
		[ServiceContractKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

CREATE TABLE [SMS].[ServiceOrderHead](
	[OrderNo] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
	[CustomerContactIdOld] [int] NULL,
	[MaintenancePlanIdOld] [int] NULL,
	[CostingUnit] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ErrorCode] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[ErrorMessage] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Component] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Reported] [datetime] NULL,
	[Planned] [datetime] NULL,
	[Deadline] [datetime] NULL,
	[OrderType] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[CloseDate] [datetime] NULL,
	[PredecessorOrderNo] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL,
	[PredecessorNotificationIdOld] [int] NULL,
	[FromWarehouse] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[FromLocationNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ToWarehouse] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ToLocationNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Status] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Priority] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[LegacyTransferDate] [datetime] NULL,
	[ContactKeyOld] [int] NULL,
	[LegacyTransferFlag] [int] NOT NULL CONSTRAINT [DF__ServiceOr__Legac__21D600EE] DEFAULT ((0)),
	[ServiceCaseKeyOld] [int] NULL,
	[PurchaseOrderNo] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[CommissionNo] [nvarchar] (30) COLLATE Latin1_General_CI_AS NULL,
	[Latitude] [float] (53) NULL,
	[Longitude] [float] (53) NULL,
	[GeocodingRetryCounter] [int] NOT NULL CONSTRAINT [DF__ServiceOr__Geoco__1451E89E] DEFAULT ((0)),
	[Name1] [nvarchar] (180) COLLATE Latin1_General_CI_AS NULL,
	[Name2] [nvarchar] (180) COLLATE Latin1_General_CI_AS NULL,
	[Name3] [nvarchar] (180) COLLATE Latin1_General_CI_AS NULL,
	[Street] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[City] [nvarchar] (80) COLLATE Latin1_General_CI_AS NULL,
	[ZipCode] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[CountryKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ServiceLocationPhone] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[ServiceLocationMobile] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[ServiceLocationFax] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[ServiceLocationEmail] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[ServiceLocationResponsiblePerson] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[NoInvoiceReason] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[UserGroupKeyOld] [int] NULL,
	[ReportRecipients] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[PreferredTechnician] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[PreferredTechnicianUsergroupOld] [int] NULL,
	[PlannedTime] [datetime] NULL,
	[PlannedDateFix] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Plann__705EA0EB] DEFAULT ((0)),
	[MaintenancePlanningRun] [uniqueidentifier] NULL,
	[InitiatorIdOld] [int] NULL,
	[PayerIdOld] [int] NULL,
	[PayerAddressIdOld] [int] NULL,
	[InvoiceRecipientIdOld] [int] NULL,
	[InvoiceRecipientAddressIdOld] [int] NULL,
	[ServiceObjectIdOld] [int] NULL,
	[CompleteDate] [datetime] NULL,
	[InvoiceReason] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[InvoiceRemark] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[InvoicingType] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ReportSent] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Repor__322C6448] DEFAULT ((0)),
	[ReportSaved] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Repor__33208881] DEFAULT ((0)),
	[ReportSavingError] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[PurchaseDate] [datetime] NULL,
	[InitiatorPersonIdOld] [int] NULL,
	[InvoiceRecipientAddressId] [uniqueidentifier] NULL,
	[PayerAddressId] [uniqueidentifier] NULL,
	[CustomerContactId] [uniqueidentifier] NULL,
	[MaintenancePlanId] [uniqueidentifier] NULL,
	[PredecessorNotificationId] [uniqueidentifier] NULL,
	[ContactKey] [uniqueidentifier] NOT NULL,
	[ServiceCaseKey] [uniqueidentifier] NULL,
	[InitiatorId] [uniqueidentifier] NULL,
	[PayerId] [uniqueidentifier] NULL,
	[InvoiceRecipientId] [uniqueidentifier] NULL,
	[ServiceObjectId] [uniqueidentifier] NULL,
	[InitiatorPersonId] [uniqueidentifier] NULL,
	[CommissioningStatusKey] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ServiceOrder_CommissionStatusKey] DEFAULT (N'0'),
	[StationKey] [uniqueidentifier] NULL,
	[PreferredTechnicianUsergroup] [uniqueidentifier] NULL,
	[UsergroupKey] [uniqueidentifier] NULL,
	[InstallationId] [uniqueidentifier] NULL,
	[ServiceContractId] [uniqueidentifier] NULL,
	[CloseReason] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[IsTemplate] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsTem__2CE88C3E] DEFAULT ((0)),
	[ServiceOrderTemplateId] [uniqueidentifier] NULL,
	[IsCostLumpSum] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsCos__165A2CA1] DEFAULT ((0)),
	[IsMaterialLumpSum] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsMat__174E50DA] DEFAULT ((0)),
	[IsTimeLumpSum] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsTim__18427513] DEFAULT ((0)),
	[RegionKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ReportSendingError] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyMainAssemblyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeySubAssemblyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyAssemblyGroupKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyFaultImageKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyRemedyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyCauseKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyWeightingKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyCauserKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[CurrencyKey] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [PK_ServiceOrderHead] PRIMARY KEY
	CLUSTERED
	(
		[ContactKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_CloseDate] ON [SMS].[ServiceOrderHead]
(
	[CloseDate] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ContactKey] ON [SMS].[ServiceOrderHead]
(
	[ContactKey] ASC
)
INCLUDE
(
	[Status]
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_OrderNo] ON [SMS].[ServiceOrderHead]
(
	[OrderNo] ASC
)
INCLUDE
(
	[ContactKey]
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderHead_InstallationId] ON [SMS].[ServiceOrderHead]
(
	[InstallationId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderHead_MaintenancePlanId] ON [SMS].[ServiceOrderHead]
(
	[MaintenancePlanId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderHead_ServiceContractId] ON [SMS].[ServiceOrderHead]
(
	[ServiceContractId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Status] ON [SMS].[ServiceOrderHead]
(
	[Status] ASC
)
INCLUDE
(
	[ContactKey]
)ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_Contact] FOREIGN KEY
	(
		[ContactKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_CustomerContact] FOREIGN KEY
	(
		[CustomerContactId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_Initiator] FOREIGN KEY
	(
		[InitiatorId]
	)
	REFERENCES [CRM].[Company]
	(
		[ContactKey]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_InitiatorPerson] FOREIGN KEY
	(
		[InitiatorPersonId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_InstallationHead] FOREIGN KEY
	(
		[InstallationId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipient] FOREIGN KEY
	(
		[InvoiceRecipientId]
	)
	REFERENCES [CRM].[Company]
	(
		[ContactKey]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_InvoiceRecipientAddress] FOREIGN KEY
	(
		[InvoiceRecipientAddressId]
	)
	REFERENCES [CRM].[Address]
	(
		[AddressId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_MaintenancePlan] FOREIGN KEY
	(
		[MaintenancePlanId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_Payer] FOREIGN KEY
	(
		[PayerId]
	)
	REFERENCES [CRM].[Company]
	(
		[ContactKey]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_PayerAddress] FOREIGN KEY
	(
		[PayerAddressId]
	)
	REFERENCES [CRM].[Address]
	(
		[AddressId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_PredecessorNotification] FOREIGN KEY
	(
		[PredecessorNotificationId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_PreferredTechnician] FOREIGN KEY
	(
		[PreferredTechnician]
	)
	REFERENCES [CRM].[User]
	(
		[Username]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_PreferredTechnicianUsergroup] FOREIGN KEY
	(
		[PreferredTechnicianUsergroup]
	)
	REFERENCES [CRM].[Usergroup]
	(
		[UsergroupId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_ServiceCase] FOREIGN KEY
	(
		[ServiceCaseKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_ServiceContract] FOREIGN KEY
	(
		[ServiceContractId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_ServiceObject] FOREIGN KEY
	(
		[ServiceObjectId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_ServiceOrderTemplate] FOREIGN KEY
	(
		[ServiceOrderTemplateId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_Station] FOREIGN KEY
	(
		[StationKey]
	)
	REFERENCES [CRM].[Station]
	(
		[StationId]
	) 

ALTER TABLE [SMS].[ServiceOrderHead] ADD CONSTRAINT [FK_ServiceOrderHead_UserGroup] FOREIGN KEY
	(
		[UsergroupKey]
	)
	REFERENCES [CRM].[Usergroup]
	(
		[UsergroupId]
	) 


CREATE TABLE [SMS].[InstallationPos](
	[PosNo] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
	[ItemNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Description] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
	[QuantityUnit] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[Quantity] [real] NOT NULL,
	[isInstalled] [int] NOT NULL,
	[InstallDate] [datetime] NULL,
	[Comment] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[WarrantyStartSupplier] [datetime] NULL,
	[WarrantyEndSupplier] [datetime] NULL,
	[WarrantyStartCustomer] [datetime] NULL,
	[WarrantyEndCustomer] [datetime] NULL,
	[RdsPpClassification] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[LegacyInstallationId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[IsGroupItem] [int] NOT NULL,
	[GroupLevel] [int] NOT NULL,
	[UserDate01] [datetime] NULL,
	[UserDate02] [datetime] NULL,
	[UserDate03] [datetime] NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_InstallationPos_Id] DEFAULT (newsequentialid()),
	[ReferenceId] [uniqueidentifier] NULL,
	[RemoveDate] [datetime] NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Installat__Favor__3F9B6DFF] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Installat__SortO__46486B8E] DEFAULT ((0)),
	[ArticleId] [uniqueidentifier] NULL,
	[InstallationId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Installat__IsAct__524F1B17] DEFAULT ((1)),
	[BatchNo] [nvarchar] (250) NULL,
	[RelatedInstallationId] [uniqueidentifier] NULL,
	[SerialNo] [nvarchar] (250) NULL,
	[IsSerial] [bit] NOT NULL CONSTRAINT [DF_InstallationPos_IsSerial] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationPos] ADD CONSTRAINT [PK_InstallationPos] PRIMARY KEY
	CLUSTERED
	(
		[id] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_InstallationPos_InstallationId] ON [SMS].[InstallationPos]
(
	[InstallationId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_InstallationPos_ReferenceId] ON [SMS].[InstallationPos]
(
	[ReferenceId] ASC
)
INCLUDE
(
	[InstallationId],
	[IsActive],
	[isInstalled]
)ON [PRIMARY]

ALTER TABLE [SMS].[InstallationPos] ADD CONSTRAINT [FK_InstallationPos_Article] FOREIGN KEY
	(
		[ArticleId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[InstallationPos] ADD CONSTRAINT [FK_InstallationPos_InstallationHead] FOREIGN KEY
	(
		[InstallationId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[InstallationPos] ADD CONSTRAINT [FK_InstallationPos_Parent] FOREIGN KEY
	(
		[ReferenceId]
	)
	REFERENCES [SMS].[InstallationPos]
	(
		[id]
	) 

ALTER TABLE [SMS].[InstallationPos] ADD CONSTRAINT [FK_InstallationPos_RelatedInstallation] FOREIGN KEY
	(
		[RelatedInstallationId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 


CREATE TABLE [SMS].[InstallationPosSerials](
	[SerialNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsInstalled] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[WarrantyStartSupplier] [datetime] NULL,
	[WarrantyEndSupplier] [datetime] NULL,
	[WarrantyStartCustomer] [datetime] NULL,
	[WarrantyEndCustomer] [datetime] NULL,
	[RdsPpClassification] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_InstallationPosSerials_Id] DEFAULT (newsequentialid()),
	[InstallationPosId] [uniqueidentifier] NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Installat__Favor__658C0CBD] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Installat__SortO__668030F6] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationPosSerials] ADD CONSTRAINT [PK_InstallationPosSerials] PRIMARY KEY
	CLUSTERED
	(
		[id] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[InstallationPosSerials] ADD CONSTRAINT [FK_InstallationPosSerials_InstallationPos] FOREIGN KEY
	(
		[InstallationPosId]
	)
	REFERENCES [SMS].[InstallationPos]
	(
		[id]
	) 


CREATE TABLE [SMS].[ServiceOrderTimes](
	[PosNo] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
	[ItemNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
	[Comment] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[ActualDuration] [real] NULL,
	[EstimatedDuration] [real] NULL,
	[Price] [money] NULL,
	[TotalValue] [money] NULL,
	[DiscountPercent] [decimal] (18,2) NULL,
	[DiscountCurrency] [decimal] (18,2) NULL,
	[TransferDate] [datetime] NULL,
	[CreatedLocal] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[HasTool] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__HasTo__47677850] DEFAULT ((0)),
	[Status] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_ServiceOrderTimes_Status] DEFAULT ('Created'),
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ServiceOrderTimes_Id] DEFAULT (newsequentialid()),
	[InstallationPosId] [uniqueidentifier] NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__ServiceOr__Creat__1293BD5E] DEFAULT ('Anonymous'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__ServiceOr__Modif__22CA2527] DEFAULT ('Anonymous'),
	[Location] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Category] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Priority] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[InvoiceDuration] [real] NULL CONSTRAINT [DF__ServiceOr__Invoi__1FEDB87C] DEFAULT ((0)),
	[IsExported] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsExp__7869D707] DEFAULT ((0)),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsAct__26EFBBC6] DEFAULT ((1)),
	[InstallationIdOld] [int] NULL,
	[CausingItemNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[CausingItemSerialNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[CausingItemPreviousSerialNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[NoCausingItemSerialNoReason] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[NoCausingItemPreviousSerialNoReason] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Diagnosis] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[InstallationId] [uniqueidentifier] NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[ArticleId] [uniqueidentifier] NULL,
	[TestGuid] [uniqueidentifier] NULL,
	[CompleteDate] [datetime] NULL,
	[CompleteUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[IsCostLumpSum] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsCos__1936994C] DEFAULT ((0)),
	[IsMaterialLumpSum] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsMat__1A2ABD85] DEFAULT ((0)),
	[IsTimeLumpSum] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsTim__1B1EE1BE] DEFAULT ((0)),
	[InvoicingTypeKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[DiscountType] [int] NOT NULL CONSTRAINT [DF__ServiceOr__Disco__77A09B57] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimes] ADD CONSTRAINT [PK_ServiceOrderTimes] PRIMARY KEY
	CLUSTERED
	(
		[id] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimes_ArticleId] ON [SMS].[ServiceOrderTimes]
(
	[ArticleId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimes_OrderId] ON [SMS].[ServiceOrderTimes]
(
	[OrderId] ASC
)ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimes] ADD CONSTRAINT [FK_ServiceOrderTimes_Article] FOREIGN KEY
	(
		[ArticleId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderTimes] ADD CONSTRAINT [FK_ServiceOrderTimes_Installation] FOREIGN KEY
	(
		[InstallationId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderTimes] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderTimes_InstallationPos] FOREIGN KEY
	(
		[InstallationPosId]
	)
	REFERENCES [SMS].[InstallationPos]
	(
		[id]
	) 

ALTER TABLE [SMS].[ServiceOrderTimes] ADD CONSTRAINT [FK_ServiceOrderTimes_ServiceOrderHead] FOREIGN KEY
	(
		[OrderId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 


CREATE TABLE [SMS].[ServiceOrderChecklist](
	[DynamicFormReferenceKeyOld] [bigint] NULL,
	[ServiceOrderTimeKey] [uniqueidentifier] NULL,
	[DispatchIdOld] [bigint] NULL,
	[RequiredForServiceOrderCompletion] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Requi__0618D7E0] DEFAULT ((0)),
	[SendToCustomer] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__SendT__070CFC19] DEFAULT ((0)),
	[DispatchId] [uniqueidentifier] NULL,
	[DynamicFormReferenceKey] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderChecklist] ADD CONSTRAINT [PK_ServiceOrderChecklist] PRIMARY KEY
	CLUSTERED
	(
		[DynamicFormReferenceKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderChecklist] ADD CONSTRAINT [FK_ServiceOrderChecklist_ServiceOrderTime] FOREIGN KEY
	(
		[ServiceOrderTimeKey]
	)
	REFERENCES [SMS].[ServiceOrderTimes]
	(
		[id]
	) 


CREATE TABLE [SMS].[ServiceOrderDispatch](
	[OrderIdOld] [int] NULL,
	[Username] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[Status] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Date] [datetime] NOT NULL,
	[Time] [datetime] NOT NULL,
	[DurationInMinutes] [int] NOT NULL,
	[Remark] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[IsFixed] [bit] NOT NULL,
	[Signature] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[SignatureContactName] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[RequiredOperations] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[FollowUpServiceOrder] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Follo__3DD3211E] DEFAULT ((0)),
	[FollowUpServiceOrderRemark] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[RejectRemark] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[RejectReason] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Component] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Diagnosis] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[CauseOfFailure] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ErrorCode] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[LegacyId] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[SignatureTechnician] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[SignatureTechnicianName] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[SignatureOriginator] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[SignatureOriginatorName] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[LatitudeOnDispatchStart] [float] (53) NULL,
	[LongitudeOnDispatchStart] [float] (53) NULL,
	[ReportSent] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Repor__30441BD6] DEFAULT ((0)),
	[ReportSaved] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Repor__3138400F] DEFAULT ((0)),
	[ReportSavingError] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[SignatureDate] [datetime] NULL,
	[SignatureTechnicianDate] [datetime] NULL,
	[SignatureOriginatorDate] [datetime] NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[DispatchIdOld] [int] NULL,
	[DispatchId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__ServiceOr__Dispa__47919582] DEFAULT (newsequentialid()),
	[SignPrivacyPolicyAccepted] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__SignP__168F36CB] DEFAULT ((0)),
	[CurrentServiceOrderTimeId] [uniqueidentifier] NULL,
	[ReportSendingError] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[DispatchNo] [nvarchar] (20) NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderDispatch] ADD CONSTRAINT [PK_ServiceOrderDispatch] PRIMARY KEY
	CLUSTERED
	(
		[DispatchId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_LegacyId] ON [SMS].[ServiceOrderDispatch]
(
	[LegacyId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_OrderId] ON [SMS].[ServiceOrderDispatch]
(
	[OrderId] ASC
)ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderDispatch] ADD CONSTRAINT [FK_ServiceOrderDispatch_OrderId] FOREIGN KEY
	(
		[OrderId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderDispatch] ADD CONSTRAINT [FK_ServiceOrderDispatch_ServiceOrderTime] FOREIGN KEY
	(
		[CurrentServiceOrderTimeId]
	)
	REFERENCES [SMS].[ServiceOrderTimes]
	(
		[id]
	) 

ALTER TABLE [SMS].[ServiceOrderDispatch] ADD CONSTRAINT [FK_ServiceOrderDispatch_User] FOREIGN KEY
	(
		[Username]
	)
	REFERENCES [CRM].[User]
	(
		[Username]
	) 


CREATE TABLE [SMS].[ServiceOrderMaterial](
	[PosNo] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
	[ItemNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Description] [nvarchar] (500) COLLATE Latin1_General_CI_AS NOT NULL,
	[InternalRemark] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[QuantityUnit] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[ActualQuantity] [decimal] (18,2) NOT NULL CONSTRAINT [DF_ServiceOrderMaterial_ActualQuantity] DEFAULT ((0)),
	[EstimatedQuantity] [decimal] (18,2) NOT NULL CONSTRAINT [DF_ServiceOrderMaterial_EstimatedQuantity] DEFAULT ((0)),
	[HourlyRate] [money] NULL,
	[TotalValue] [money] NULL,
	[DiscountPercent] [decimal] (18,2) NULL,
	[FromWarehouse] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[FromLocationNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ToWarehouse] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ToLocationNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Status] [int] NOT NULL,
	[TransferDate] [datetime] NULL,
	[BuiltIn] [int] NOT NULL,
	[IsSerial] [int] NOT NULL,
	[CreatedLocal] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ServiceOrderTimeId] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ServiceOrderMaterial_Id] DEFAULT (newsequentialid()),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Favor__3DE82FB7] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__ServiceOr__SortO__3EDC53F0] DEFAULT ((0)),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__ServiceOr__Creat__3FD07829] DEFAULT ('Anonymous'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__ServiceOr__Modif__40C49C62] DEFAULT ('Anonymous'),
	[InvoiceQuantity] [decimal] (18,2) NOT NULL CONSTRAINT [DF_ServiceOrderMaterial_InvoiceQuantity] DEFAULT ((0)),
	[IsExported] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsExp__7775B2CE] DEFAULT ((0)),
	[SignedByCustomer] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Signe__795DFB40] DEFAULT ((0)),
	[ExternalRemark] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsAct__25FB978D] DEFAULT ((1)),
	[LegacyVersion] [bigint] NULL,
	[DispatchIdOld] [int] NULL,
	[DispatchId] [uniqueidentifier] NULL,
	[ReplenishmentOrderItemIdOld] [int] NULL,
	[ReplenishmentOrderItemId] [uniqueidentifier] NULL,
	[CommissioningStatusKey] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_ServiceMaterial_CommissionStatusKey] DEFAULT (N'0'),
	[OrderId] [uniqueidentifier] NOT NULL,
	[ArticleId] [uniqueidentifier] NULL,
	[ArticleTypeKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[DiscountType] [int] NOT NULL CONSTRAINT [DF__ServiceOr__Disco__75B852E5] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT [PK_ServiceOrderMaterial_1] PRIMARY KEY
	CLUSTERED
	(
		[Id] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderMaterial_ArticleId] ON [SMS].[ServiceOrderMaterial]
(
	[ArticleId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderMaterial_OrderId] ON [SMS].[ServiceOrderMaterial]
(
	[OrderId] ASC
)ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT [FK_ServiceOrderMaterial_Article] FOREIGN KEY
	(
		[ArticleId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderMaterial] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderMaterial_ServiceOrderDispatch] FOREIGN KEY
	(
		[DispatchId]
	)
	REFERENCES [SMS].[ServiceOrderDispatch]
	(
		[DispatchId]
	) 

ALTER TABLE [SMS].[ServiceOrderMaterial] ADD CONSTRAINT [FK_ServiceOrderMaterial_ServiceOrderHead] FOREIGN KEY
	(
		[OrderId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderMaterial] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderMaterial_ServiceOrderTimes] FOREIGN KEY
	(
		[ServiceOrderTimeId]
	)
	REFERENCES [SMS].[ServiceOrderTimes]
	(
		[id]
	) 


CREATE TABLE [SMS].[ServiceOrderMaterialSerials](
	[SerialNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsInstalled] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ServiceOrderMaterialSerials_Id] DEFAULT (newsequentialid()),
	[OrderMaterialId] [uniqueidentifier] NOT NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceOr__Creat__3A379A64] DEFAULT ('Anonymous'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceOr__Modif__3B2BBE9D] DEFAULT ('Anonymous'),
	[PreviousSerialNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsAct__77FFC2B3] DEFAULT ((1)),
	[NoPreviousSerialNoReason] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderMaterialSerials] ADD CONSTRAINT [PK_ServiceOrderMaterialSerials] PRIMARY KEY
	CLUSTERED
	(
		[Id] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderMaterialSerials] ADD CONSTRAINT [FK_ServiceOrderMaterialSerials_ServiceOrderMaterial] FOREIGN KEY
	(
		[OrderMaterialId]
	)
	REFERENCES [SMS].[ServiceOrderMaterial]
	(
		[Id]
	) 


CREATE TABLE [SMS].[ServiceOrderTimePostings](
	[UserId] [uniqueidentifier] NULL,
	[From] [datetime] NULL,
	[To] [datetime] NULL,
	[Price] [money] NULL,
	[DiscountPercent] [real] NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[Description] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ServiceOrderTimePostings_Id] DEFAULT (newsequentialid()),
	[OrderTimesId] [uniqueidentifier] NULL,
	[CreateUser] [nvarchar] (256) NULL,
	[ModifyUser] [nvarchar] (256) NULL,
	[UserUsername] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[ItemNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[SignedByCustomer] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__Signe__7A521F79] DEFAULT ((0)),
	[Date] [datetime] NOT NULL,
	[DurationInMinutes] [int] NULL CONSTRAINT [DF__ServiceOr__Durat__05C3D225] DEFAULT ((0)),
	[InternalRemark] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsAct__27E3DFFF] DEFAULT ((1)),
	[BreakInMinutes] [int] NULL,
	[Kilometers] [int] NULL,
	[IsExported] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsExp__32616E72] DEFAULT ((0)),
	[CostCenter] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[IsClosed] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsClo__0A1E72EE] DEFAULT ((0)),
	[DispatchIdOld] [int] NULL,
	[DispatchId] [uniqueidentifier] NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[ArticleId] [uniqueidentifier] NULL,
	[PerDiemReportId] [uniqueidentifier] NULL,
	[PlannedDurationInMinutes] [int] NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimePostings] ADD CONSTRAINT [PK_ServiceOrderTimePostings] PRIMARY KEY
	CLUSTERED
	(
		[id] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimePostings_ArticleId] ON [SMS].[ServiceOrderTimePostings]
(
	[ArticleId] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimePostings_IsActive_Included] ON [SMS].[ServiceOrderTimePostings]
(
	[IsActive] ASC
)
INCLUDE
(
	[Date],
	[IsClosed],
	[ModifyDate],
	[OrderId],
	[OrderTimesId],
	[PerDiemReportId]
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_ServiceOrderTimePostings_OrderId] ON [SMS].[ServiceOrderTimePostings]
(
	[OrderId] ASC
)ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderTimePostings] ADD CONSTRAINT [FK_ServiceOrderTimePostings_Article] FOREIGN KEY
	(
		[ArticleId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderTimePostings] WITH NOCHECK ADD CONSTRAINT [FK_ServiceOrderTimePostings_ServiceOrderDispatch] FOREIGN KEY
	(
		[DispatchId]
	)
	REFERENCES [SMS].[ServiceOrderDispatch]
	(
		[DispatchId]
	) 

ALTER TABLE [SMS].[ServiceOrderTimePostings] ADD CONSTRAINT [FK_ServiceOrderTimePostings_ServiceOrderHead] FOREIGN KEY
	(
		[OrderId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceOrderTimePostings] ADD CONSTRAINT [FK_ServiceOrderTimePostings_User] FOREIGN KEY
	(
		[UserUsername]
	)
	REFERENCES [CRM].[User]
	(
		[Username]
	) 

CREATE TABLE [SMS].[ServiceOrderDispatchReportRecipient](
	[ServiceOrderDispatchReportRecipientId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__ServiceOr__Servi__4BF72343] DEFAULT (newsequentialid()),
	[DispatchId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[Locale] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceOr__Creat__4CEB477C] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceOr__Modif__4DDF6BB5] DEFAULT ('Setup'),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__ServiceOr__Creat__4ED38FEE] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__ServiceOr__Modif__4FC7B427] DEFAULT (getutcdate()),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceOr__IsAct__50BBD860] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderDispatchReportRecipient] ADD CONSTRAINT [PK__ServiceO__D3DB9E096E80793E] PRIMARY KEY
	CLUSTERED
	(
		[ServiceOrderDispatchReportRecipientId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceOrderDispatchReportRecipient] ADD CONSTRAINT [FK_ServiceOrderDispatchReportRecipient_ServiceOrderDispatch] FOREIGN KEY
	(
		[DispatchId]
	)
	REFERENCES [SMS].[ServiceOrderDispatch]
	(
		[DispatchId]
	) 

CREATE TABLE [SMS].[ServiceCaseTemplate](
	[ServiceCaseTemplateId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__ServiceCa__Servi__5AE46118] DEFAULT (newsequentialid()),
	[CategoryKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[PriorityKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ResponsibleUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__ServiceCa__Creat__5BD88551] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__ServiceCa__Modif__5CCCA98A] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceCa__Creat__5DC0CDC3] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__ServiceCa__Modif__5EB4F1FC] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceCa__IsAct__5FA91635] DEFAULT ((1)),
	[AuthDataId] [uniqueidentifier] NULL,
	[CompletionDynamicFormId] [uniqueidentifier] NULL,
	[CreationDynamicFormId] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceCaseTemplate] ADD CONSTRAINT [PK__ServiceC__BCCCBC864C71BE3D] PRIMARY KEY
	CLUSTERED
	(
		[ServiceCaseTemplateId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceCaseTemplate] ADD CONSTRAINT [FK_ServiceCaseTemplate_EntityAuthData] FOREIGN KEY
	(
		[AuthDataId]
	)
	REFERENCES [dbo].[EntityAuthData]
	(
		[UId]
	) 

CREATE TABLE [SMS].[ServiceNotifications](
	[LegacyId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[LegacyInstallationId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[ErrorCode] [int] NULL,
	[ErrorMessage] [nvarchar] (500) COLLATE Latin1_General_CI_AS NOT NULL,
	[Remark] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Reported] [datetime] NOT NULL,
	[Planned] [datetime] NULL,
	[Executed] [datetime] NULL,
	[PredecessorOrderNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[PredecessorNotificationId] [int] NULL,
	[Status] [int] NOT NULL,
	[TemplateOrderNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[ContactKeyOld] [int] NULL,
	[AppliedCauseOfFailureSolutionId] [uniqueidentifier] NULL,
	[PickedUpDate] [datetime] NULL,
	[LegacyDeviceId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[ContactPersonKeyOld] [int] NULL,
	[Category] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Priority] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[AffectedCompanyKeyOld] [int] NULL,
	[AffectedComponentKey] [uniqueidentifier] NULL,
	[UserGroupKeyOld] [int] NULL,
	[AffectedInstallationKeyOld] [int] NULL,
	[ServiceCaseNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[ContactKey] [uniqueidentifier] NOT NULL,
	[ContactPersonKey] [uniqueidentifier] NULL,
	[AffectedCompanyKey] [uniqueidentifier] NULL,
	[AffectedInstallationKey] [uniqueidentifier] NULL,
	[UsergroupKey] [uniqueidentifier] NULL,
	[ServiceCaseTemplateId] [uniqueidentifier] NULL,
	[CompletionDate] [datetime] NULL,
	[CompletionServiceOrderId] [uniqueidentifier] NULL,
	[CompletionUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[AffectedDynamicFormElementId] [uniqueidentifier] NULL,
	[AffectedDynamicFormReferenceId] [uniqueidentifier] NULL,
	[OriginatingServiceOrderId] [uniqueidentifier] NULL,
	[ServiceObjectId] [uniqueidentifier] NULL,
	[ServiceOrderTimeId] [uniqueidentifier] NULL,
	[OriginatingServiceOrderTimeId] [uniqueidentifier] NULL,
	[StationKey] [uniqueidentifier] NULL,
	[StatisticsKeyProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyMainAssemblyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeySubAssemblyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyAssemblyGroupKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyFaultImageKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyRemedyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyCauseKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyWeightingKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[StatisticsKeyCauserKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[ServiceCaseCreateUser] [nvarchar] (60) NOT NULL CONSTRAINT [DF__ServiceNo__Servi__685E57C7] DEFAULT ('System'),
	[ServiceCaseCreateDate] [datetime] NOT NULL CONSTRAINT [DF__ServiceNo__Servi__69527C00] DEFAULT (getutcdate())
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [PK_ServiceNotifications] PRIMARY KEY
	CLUSTERED
	(
		[ContactKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_AffectedCompany] FOREIGN KEY
	(
		[AffectedCompanyKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_AffectedInstallation] FOREIGN KEY
	(
		[AffectedInstallationKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_Contact] FOREIGN KEY
	(
		[ContactKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotification_ContactPerson] FOREIGN KEY
	(
		[ContactPersonKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_CompletionServiceOrder] FOREIGN KEY
	(
		[CompletionServiceOrderId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_OriginatingServiceOrder] FOREIGN KEY
	(
		[OriginatingServiceOrderId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_OriginatingServiceOrderTime] FOREIGN KEY
	(
		[OriginatingServiceOrderTimeId]
	)
	REFERENCES [SMS].[ServiceOrderTimes]
	(
		[id]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_ServiceCaseTemplate] FOREIGN KEY
	(
		[ServiceCaseTemplateId]
	)
	REFERENCES [SMS].[ServiceCaseTemplate]
	(
		[ServiceCaseTemplateId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_ServiceObject] FOREIGN KEY
	(
		[ServiceObjectId]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_ServiceOrderTime] FOREIGN KEY
	(
		[ServiceOrderTimeId]
	)
	REFERENCES [SMS].[ServiceOrderTimes]
	(
		[id]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_Station] FOREIGN KEY
	(
		[StationKey]
	)
	REFERENCES [CRM].[Station]
	(
		[StationId]
	) 

ALTER TABLE [SMS].[ServiceNotifications] ADD CONSTRAINT [FK_ServiceNotifications_Usergroup] FOREIGN KEY
	(
		[UsergroupKey]
	)
	REFERENCES [CRM].[Usergroup]
	(
		[UsergroupId]
	) ON DELETE SET NULL ON UPDATE CASCADE


CREATE TABLE [SMS].[ServiceContractInstallationRelationship](
	[ServiceContractInstallationRelationshipIdOld] [int] NOT NULL IDENTITY (1,1),
	[ServiceContractKeyOld] [int] NULL,
	[InstallationKeyOld] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Information] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[TimeAllocation] [datetime] NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__ServiceCo__IsAct__4B2D1C3C] DEFAULT ((1)),
	[ServiceContractKey] [uniqueidentifier] NOT NULL,
	[InstallationKey] [uniqueidentifier] NOT NULL,
	[ServiceContractInstallationRelationshipId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ServiceContractInstallationRelationship_ServiceContractInstallationRelationshipId] DEFAULT (newsequentialid())
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT [PK_ServiceContractInstallationRelationship] PRIMARY KEY
	CLUSTERED
	(
		[ServiceContractInstallationRelationshipId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT [FK_ServiceContractInstallationRelationship_Installation] FOREIGN KEY
	(
		[InstallationKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 

ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT [FK_ServiceContractInstallationRelationship_Installation2] FOREIGN KEY
	(
		[InstallationKey]
	)
	REFERENCES [SMS].[InstallationHead]
	(
		[ContactKey]
	) 

ALTER TABLE [SMS].[ServiceContractInstallationRelationship] ADD CONSTRAINT [FK_ServiceContractInstallationRelationship_ServiceContract] FOREIGN KEY
	(
		[ServiceContractKey]
	)
	REFERENCES [CRM].[Contact]
	(
		[ContactId]
	) 


CREATE TABLE [LU].[StatisticsKeyAssemblyGroup](
	[StatisticsKeyAssemblyGroupId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[MainAssemblyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[SubAssemblyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__6033261A] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__61274A53] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__621B6E8C] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__630F92C5] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__6403B6FE] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__64F7DB37] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__65EBFF70] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyAssemblyGroup] ADD CONSTRAINT [PK__Statisti__C373799734825A76] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyAssemblyGroupId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeyCause](
	[StatisticsKeyCauseId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__79F2F81D] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__7AE71C56] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__7BDB408F] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__7CCF64C8] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__7DC38901] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__7EB7AD3A] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__7FABD173] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyCause] ADD CONSTRAINT [PK__Statisti__7C3583160084E9BA] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyCauseId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeyCauser](
	[StatisticsKeyCauserId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__0B1D841F] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__0C11A858] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__0D05CC91] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__0DF9F0CA] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__0EEE1503] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__0FE2393C] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__10D65D75] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyCauser] ADD CONSTRAINT [PK__Statisti__85B1355DCEC528D9] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyCauserId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeyFaultImage](
	[StatisticsKeyFaultImageId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[AssemblyGroupKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__68C86C1B] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__69BC9054] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__6AB0B48D] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__6BA4D8C6] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__6C98FCFF] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__6D8D2138] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__6E814571] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyFaultImage] ADD CONSTRAINT [PK__Statisti__0F08560CB914D603] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyFaultImageId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeyMainAssembly](
	[StatisticsKeyMainAssemblyId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__4F089A18] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__4FFCBE51] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__50F0E28A] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__51E506C3] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__52D92AFC] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__53CD4F35] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__54C1736E] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyMainAssembly] ADD CONSTRAINT [PK__Statisti__FBD2D294B43E6B05] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyMainAssemblyId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeyProductType](
	[StatisticsKeyProductTypeId] [int] NOT NULL IDENTITY (1,1),
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__46735417] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__47677850] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__485B9C89] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__494FC0C2] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__4A43E4FB] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__4B380934] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__4C2C2D6D] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyProductType] ADD CONSTRAINT [PK__Statisti__465DD03B84F2B78B] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyProductTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeyRemedy](
	[StatisticsKeyRemedyId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__715DB21C] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__7251D655] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__7345FA8E] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__743A1EC7] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__752E4300] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__76226739] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__77168B72] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyRemedy] ADD CONSTRAINT [PK__Statisti__12B7727B57AABD7B] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyRemedyId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeySubAssembly](
	[StatisticsKeySubAssemblyId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[MainAssemblyKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__579DE019] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__58920452] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__5986288B] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__5A7A4CC4] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__5B6E70FD] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__5C629536] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__5D56B96F] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeySubAssembly] ADD CONSTRAINT [PK__Statisti__4A1C5178C3F11A2B] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeySubAssemblyId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[StatisticsKeyWeighting](
	[StatisticsKeyWeightingId] [int] NOT NULL IDENTITY (1,1),
	[ProductTypeKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Code] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Statistic__Favor__02883E1E] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Statistic__SortO__037C6257] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Creat__04708690] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Statistic__Modif__0564AAC9] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Creat__0658CF02] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Statistic__Modif__074CF33B] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Statistic__IsAct__08411774] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[StatisticsKeyWeighting] ADD CONSTRAINT [PK__Statisti__D58762AFF49E3636] PRIMARY KEY
	CLUSTERED
	(
		[StatisticsKeyWeightingId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[ServiceCaseTemplateSkill](
	[ServiceCaseTemplateId] [uniqueidentifier] NOT NULL,
	[Skill] [nvarchar] (10) NOT NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceCaseTemplateSkill] ADD CONSTRAINT [FK__ServiceCa__Servi__46FD63FC] FOREIGN KEY
	(
		[ServiceCaseTemplateId]
	)
	REFERENCES [SMS].[ServiceCaseTemplate]
	(
		[ServiceCaseTemplateId]
	) 

CREATE TABLE [SMS].[ServiceNotificationsSkill](
	[ContactKey] [uniqueidentifier] NOT NULL,
	[Skill] [nvarchar] (10) NOT NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[ServiceNotificationsSkill] ADD CONSTRAINT [FK__ServiceNo__Conta__45151B8A] FOREIGN KEY
	(
		[ContactKey]
	)
	REFERENCES [SMS].[ServiceNotifications]
	(
		[ContactKey]
	) 


ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [FK_DocumentAttribute_ServiceOrderDispatch] FOREIGN KEY
	(
		[DispatchId]
	)
	REFERENCES [SMS].[ServiceOrderDispatch]
	(
		[DispatchId]
	) 

ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [FK_DocumentAttribute_ServiceOrderMaterial] FOREIGN KEY
	(
		[ServiceOrderMaterialId]
	)
	REFERENCES [SMS].[ServiceOrderMaterial]
	(
		[Id]
	) 

ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [FK_DocumentAttribute_ServiceOrderTime] FOREIGN KEY
	(
		[ServiceOrderTimeId]
	)
	REFERENCES [SMS].[ServiceOrderTimes]
	(
		[id]
	) 
");
			Database.ExecuteNonQuery(@"INSERT INTO [dbo].[SchemaInfo_Crm_Service] ([Version]) VALUES 
(20110819205602),
(20120215104522),
(20120521233617),
(20120827160312),
(20120923113326),
(20120927105924),
(20120927105925),
(20120927133851),
(20120928160900),
(20121012114900),
(20121012114901),
(20121018114629),
(20121019150004),
(20121019150005),
(20121130120535),
(20121130145903),
(20121205164559),
(20130205134300),
(20130205135300),
(20130205141000),
(20130205142000),
(20130205142100),
(20130205144300),
(20130205145300),
(20130506102719),
(20130506163416),
(20130506163417),
(20130506163418),
(20130506201829),
(20130507092910),
(20130507163618),
(20130508115126),
(20130617183725),
(20130618104014),
(20130624084202),
(20130624105015),
(20130625084202),
(20130628112022),
(20130702114800),
(20130704100818),
(20130705103922),
(20130705103923),
(20130705103924),
(20130705103925),
(20130705153608),
(20130705153609),
(20130708164100),
(20130709182536),
(20130716171402),
(20130729092100),
(20130731131647),
(20130801093400),
(20130802130141),
(20130802130949),
(20130802133444),
(20130806101112),
(20130806114125),
(20130814121000),
(20130815161212),
(20130816124300),
(20130823101920),
(20130827113629),
(20130827154609),
(20130827154610),
(20130828144700),
(20130904110626),
(20130904112022),
(20130905085407),
(20130905130747),
(20130905150706),
(20130905153952),
(20130909150302),
(20130910101011),
(20130910130848),
(20130911102618),
(20130917091100),
(20130919103619),
(20130926162317),
(20130926162318),
(20130926162319),
(20131009152769),
(20131010085408),
(20131010090501),
(20131015154104),
(20131015154105),
(20131015154106),
(20131029143053),
(20131030131041),
(20131030184912),
(20131031163214),
(20131031163215),
(20131031183234),
(20131031190605),
(20131031190606),
(20131101110525),
(20131101110526),
(20131101164216),
(20131105112123),
(20131105193819),
(20131105193820),
(20131105193821),
(20131105193822),
(20131106092012),
(20131106135725),
(20131106135726),
(20131106143851),
(20131106171112),
(20131106171114),
(20131111162211),
(20131111162212),
(20131112095334),
(20131126104013),
(20131203085612),
(20131204142600),
(20131205170100),
(20131218095739),
(20131218095740),
(20131218101515),
(20131218101516),
(20132904100212),
(20140121125500),
(20140124124734),
(20140128111930),
(20140128111931),
(20140128111932),
(20140131151712),
(20140131151713),
(20140131151714),
(20140204142645),
(20140205131123),
(20140205142523),
(20140218092900),
(20140224141000),
(20140224141100),
(20140226100200),
(20140227152800),
(20140305101200),
(20140305151800),
(20140305153600),
(20140310145300),
(20140312162100),
(20140313135100),
(20140313135101),
(20140415152100),
(20140416151200),
(20140416151201),
(20140416161200),
(20140416161201),
(20140416165000),
(20140502233100),
(20140508121400),
(20140612171000),
(20140612171001),
(20140630141800),
(20140708154400),
(20140715165800),
(20140729160300),
(20140731102200),
(20140822130100),
(20140902091412),
(20140903101700),
(20140930094912),
(20140930185112),
(20140930190112),
(20141027101711),
(20141027101712),
(20141030092812),
(20150310133900),
(20150310133901),
(20150408175000),
(20150413144700),
(20150413151400),
(20150413151401),
(20150413155300),
(20150413155301),
(20150413155302),
(20150413155303),
(20150413155304),
(20150413161400),
(20150413161900),
(20150413161901),
(20150413162000),
(20150413162001),
(20150413163200),
(20150413163201),
(20150413163202),
(20150413163203),
(20150413163204),
(20150413164500),
(20150413164501),
(20150413164502),
(20150413164503),
(20150413164504),
(20150413164505),
(20150413164506),
(20150413164507),
(20150413164508),
(20150413164509),
(20150413164510),
(20150413171100),
(20150413171600),
(20150414155100),
(20151111173000),
(20151121133306),
(20151121133349),
(20160108160600),
(20160125142149),
(20160126103700),
(20160204100000),
(20160223132311),
(20160404161100),
(20160404161101),
(20160404161102),
(20160404161103),
(20160404161104),
(20160404161105),
(20160415100300),
(20160415102900),
(20160415103400),
(20160415113200),
(20160415164800),
(20160512170011),
(20160517150800),
(20160517151100),
(20160524092900),
(20160524093700),
(20160606140200),
(20160617184400),
(20160623132700),
(20160623141000),
(20160623164400),
(20160701145100),
(20160713101200),
(20160811112500),
(20160819144200),
(20160907091900),
(20160907115400),
(20160907115500),
(20160928115300),
(20160928163800),
(20160929100400),
(20161114161400),
(20161114161600),
(20161121125900),
(20161201114400),
(20161201114900),
(20170120110700),
(20170309200000),
(20170309205000),
(20170309210000),
(20170331151000),
(20170523083200),
(20170523145800),
(20170620110000),
(20170620120000),
(20170711120000),
(20170720173700),
(20170721103000),
(20170724120005),
(20170822153901),
(20170822153902),
(20171120171900),
(20171120172700),
(20180101110000),
(20180220110001),
(20180221150000),
(20180222100000),
(20180222101500),
(20180424161900),
(20180625094000),
(20180625094500),
(20180626130000),
(20180717153300),
(20180717155800),
(20180717165700),
(20180717181100),
(20180718092900),
(20180718094000),
(20180718130400),
(20180718130700),
(20180718130900),
(20180719134200),
(20180719152800),
(20180816092600),
(20180816124800),
(20180816124900),
(20180820092600),
(20180820124800),
(20180820124900),
(20180820144700),
(20180917172500),
(20180918110500),
(20180918112100),
(20180918114300),
(20180921140300),
(20181001163400),
(20181004184700),
(20181004184800),
(20181008145800),
(20181008200000),
(20181206092800),
(20181206093000),
(20190221190001),
(20190251430000),
(20190408174500),
(20190411141300),
(20190703102459),
(20190705123000),
(20190731113000),
(20190902110200),
(20190910084200),
(20190910101400),
(20190910102000),
(20190911092300),
(20190917162000),
(20190917162100),
(20190919162100),
(20190920080800),
(20190924083700),
(20190924091600),
(20190924190000),
(20191001131000),
(20191002111700),
(20191002111900),
(20191002112300),
(20191002154700),
(20191014133700),
(20191014150900),
(20191015130300),
(20191015130600),
(20191111164700),
(20191125153300),
(20191125154500),
(20191127095700),
(20200121144200),
(20200218145600),
(20200219082820),
(20200219132300),
(20200304180000),
(20200317084200),
(20200318131300),
(20200318131500),
(20200318132100),
(20200422110000),
(20200422113000),
(20200605141000),
(20200609105000),
(20200617150600),
(20200617150700),
(20200618171700),
(20200723145000),
(20200916163000),
(20201007103200),
(20201102143000),
(20201111123500),
(20210115124200),
(20210124145000),
(20210217153000),
(20210218152300),
(20210308090400),
(20210428163200),
(20210519121000),
(20210625132500),
(20210630152000),
(20210819162000),
(20210916133600),
(20210921173000),
(20211004110000),
(20211004153000),
(20211004182000),
(20211005090000),
(20211022120900),
(20211102164000),
(20211105121500),
(20211118144500),
(20211209150000),
(20220105140300),
(20220110140500),
(20220110141100),
(20220110141600),
(20220110144600),
(20220110144700),
(20220112160000),
(20220114104400),
(20220114105000),
(20220222155222),
(20220302140000),
(20220329130000),
(20220330105300),
(20220330105400),
(20220406130000),
(20220411090900),
(20220411114400),
(20220411143000),
(20220630092200),
(20220704101500),
(20220714140300),
(20220719164500),
(20220720083455),
(20220729143411),
(20220817112000),
(20220817140000),
(20220913141250),
(20220914102300),
(20220919161728),
(20221108120000),
(20221110113400),
(20221409112500),
(20230323113500),
(20230405111700),
(20230405114000),
(20181010222000)
");
			Database.ExecuteNonQuery(@"

SET IDENTITY_INSERT [LU].[InstallationAddressRelationshipType] ON
INSERT [LU].[InstallationAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[InstallationAddressRelationshipTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2014-06-24T11:34:08.310',N'Setup',0,1,1,N'de','2019-02-06T21:57:38.460',N'Setup',N'Rechnungsempfänger',0,N'InvoiceRecipient')
INSERT [LU].[InstallationAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[InstallationAddressRelationshipTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2014-06-24T11:34:08.313',N'Setup',0,2,1,N'en','2019-02-06T21:57:38.460',N'Setup',N'Invoice recipient',0,N'InvoiceRecipient')
INSERT [LU].[InstallationAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[InstallationAddressRelationshipTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2019-02-06T21:57:38.233',N'Setup',0,4,1,N'fr','2019-02-06T21:57:38.233',N'Setup',N'Destinataire de la facture',0,N'InvoiceRecipient')
INSERT [LU].[InstallationAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[InstallationAddressRelationshipTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.653',N'Creater',0,5,1,N'es','2021-07-13T08:43:50.653',N'Modifier',N'Destinatario de la factura',0,N'InvoiceRecipient')
SET IDENTITY_INSERT [LU].[InstallationAddressRelationshipType] OFF


SET IDENTITY_INSERT [LU].[NoCausingItemPreviousSerialNoReason] ON
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.730',N'Migration_20170523083200',0,1,N'en','2019-02-07T09:03:17.023',N'Setup',N'Not available',1,0,N'NotAvailable')
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.730',N'Migration_20170523083200',0,1,N'de','2019-02-07T09:03:17.023',N'Setup',N'Nicht vorhanden',2,0,N'NotAvailable')
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.730',N'Migration_20170523083200',0,1,N'en','2019-02-07T09:03:06.037',N'Setup',N'Nonreadable',3,0,N'Nonreadable')
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.730',N'Migration_20170523083200',0,1,N'de','2019-02-07T09:03:06.033',N'Setup',N'Nicht lesbar',4,0,N'Nonreadable')
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2019-02-07T09:03:05.810',N'Setup',0,1,N'fr','2019-02-07T09:03:05.810',N'Setup',N'Illisible',7,0,N'Nonreadable')
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2019-02-07T09:03:16.797',N'Setup',0,1,N'fr','2019-02-07T09:03:16.797',N'Setup',N'Indisponible',8,0,N'NotAvailable')
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.347',N'Creater',0,1,N'es','2021-07-13T08:43:50.347',N'Modifier',N'No legible',9,0,N'Nonreadable')
INSERT [LU].[NoCausingItemPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.347',N'Creater',0,1,N'es','2021-07-13T08:43:50.347',N'Modifier',N'No disponible',10,0,N'NotAvailable')
SET IDENTITY_INSERT [LU].[NoCausingItemPreviousSerialNoReason] OFF

SET IDENTITY_INSERT [LU].[NoCausingItemSerialNoReason] ON
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.737',N'Migration_20170523083200',0,1,N'en','2019-02-07T09:04:09.047',N'Setup',N'Not available',1,0,N'NotAvailable')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.737',N'Migration_20170523083200',0,1,N'de','2019-02-07T09:04:09.047',N'Setup',N'Nicht vorhanden',2,0,N'NotAvailable')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.737',N'Migration_20170523083200',0,1,N'en','2019-02-07T09:04:24.293',N'Setup',N'Not replaced',3,0,N'NotReplaced')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.737',N'Migration_20170523083200',0,1,N'de','2019-02-07T09:04:24.293',N'Setup',N'nicht getauscht',4,0,N'NotReplaced')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.737',N'Migration_20170523083200',0,1,N'hu','2019-02-07T09:04:09.047',N'Setup',N'##hu: NoCausingItemSerialNoReason_NotAvailable',5,0,N'NotAvailable')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.737',N'Migration_20170523083200',0,1,N'hu','2019-02-07T09:04:24.293',N'Setup',N'##hu: NoCausingItemSerialNoReason_NotReplaced',6,0,N'NotReplaced')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2019-02-07T09:04:08.817',N'Setup',0,1,N'fr','2019-02-07T09:04:08.817',N'Setup',N'Indisponible',7,0,N'NotAvailable')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2019-02-07T09:04:24.073',N'Setup',0,1,N'fr','2019-02-07T09:04:24.073',N'Setup',N'Non remplacé',8,0,N'NotReplaced')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.350',N'Creater',0,1,N'es','2021-07-13T08:43:50.350',N'Modifier',N'No sustituido',9,0,N'NotReplaced')
INSERT [LU].[NoCausingItemSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoCausingItemSerialNoReasonId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.350',N'Creater',0,1,N'es','2021-07-13T08:43:50.350',N'Modifier',N'No disponible',10,0,N'NotAvailable')
SET IDENTITY_INSERT [LU].[NoCausingItemSerialNoReason] OFF

SET IDENTITY_INSERT [LU].[NoPreviousSerialNoReason] ON
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.743',N'Migration_20170523083200',0,1,N'en','2019-02-07T09:05:02.773',N'Setup',N'Not available',1,0,N'NotAvailable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.743',N'Migration_20170523083200',0,1,N'de','2019-02-07T09:05:02.773',N'Setup',N'Nicht vorhanden',2,0,N'NotAvailable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.743',N'Migration_20170523083200',0,1,N'en','2019-02-07T09:04:52.443',N'Setup',N'Nonreadable',3,1,N'Nonreadable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.743',N'Migration_20170523083200',0,1,N'de','2019-02-07T09:04:52.443',N'Setup',N'Nicht lesbar',4,1,N'Nonreadable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.743',N'Migration_20170523083200',0,1,N'hu','2019-02-07T09:05:02.773',N'Setup',N'##hu: NoPreviousSerialNoReason_NotAvailable',5,0,N'NotAvailable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.743',N'Migration_20170523083200',0,1,N'hu','2019-02-07T09:04:52.443',N'Setup',N'##hu: NoPreviousSerialNoReason_Nonreadable',6,1,N'Nonreadable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2019-02-07T09:04:52.220',N'Setup',0,1,N'fr','2019-02-07T09:04:52.220',N'Setup',N'Illisible',7,1,N'Nonreadable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2019-02-07T09:05:02.560',N'Setup',0,1,N'fr','2019-02-07T09:05:02.560',N'Setup',N'Indisponible',8,0,N'NotAvailable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.357',N'Creater',0,1,N'es','2021-07-13T08:43:50.357',N'Modifier',N'No legible',9,1,N'Nonreadable')
INSERT [LU].[NoPreviousSerialNoReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NoPreviousSerialNoReasonId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.357',N'Creater',0,1,N'es','2021-07-13T08:43:50.357',N'Modifier',N'No disponible',10,0,N'NotAvailable')
SET IDENTITY_INSERT [LU].[NoPreviousSerialNoReason] OFF


SET IDENTITY_INSERT [LU].[ServiceContractAddressRelationshipType] ON
INSERT [LU].[ServiceContractAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractAddressRelationshipTypeId],[SortOrder],[Value])
VALUES('2014-06-24T11:34:08.613',N'Setup',0,1,N'de','2019-02-07T08:29:54.373',N'Setup',N'Rechnungsempfänger',1,0,N'InvoiceRecipient')
INSERT [LU].[ServiceContractAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractAddressRelationshipTypeId],[SortOrder],[Value])
VALUES('2014-06-24T11:34:08.613',N'Setup',0,1,N'en','2019-02-07T08:29:54.373',N'Setup',N'Invoice recipient',2,0,N'InvoiceRecipient')
INSERT [LU].[ServiceContractAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractAddressRelationshipTypeId],[SortOrder],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:29:54.373',N'Setup',N'##hu: ServiceContractAddressRelationshipType_InvoiceRecipient',3,0,N'InvoiceRecipient')
INSERT [LU].[ServiceContractAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractAddressRelationshipTypeId],[SortOrder],[Value])
VALUES('2019-02-07T08:29:54.130',N'Setup',0,1,N'fr','2019-02-07T08:29:54.130',N'Setup',N'Destinataire de la facture',4,0,N'InvoiceRecipient')
INSERT [LU].[ServiceContractAddressRelationshipType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractAddressRelationshipTypeId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.660',N'Creater',0,1,N'es','2021-07-13T08:43:50.660',N'Modifier',N'Destinatario de la factura',5,0,N'InvoiceRecipient')
SET IDENTITY_INSERT [LU].[ServiceContractAddressRelationshipType] OFF


SET IDENTITY_INSERT [LU].[ServiceObjectCategory] ON
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'en','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Transport',1,0,N'Transport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'de','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Transport',2,0,N'Transport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'en','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Public facilities',3,1,N'PublicFacilities')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'de','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Öffentliche Einrichtungen',4,1,N'PublicFacilities')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'en','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Sport',5,2,N'Sport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'de','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Sport / Freizeit',6,2,N'Sport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'en','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Shopping',7,3,N'Shopping')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'de','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Shopping',8,3,N'Shopping')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'en','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Industry',9,4,N'Industry')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'de','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'Industrie / Gewerbe',10,4,N'Industry')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'hu','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'##hu: ServiceObjectCategory_Transport',11,0,N'Transport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'hu','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'##hu: ServiceObjectCategory_PublicFacilities',12,1,N'PublicFacilities')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'hu','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'##hu: ServiceObjectCategory_Sport',13,2,N'Sport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'hu','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'##hu: ServiceObjectCategory_Shopping',14,3,N'Shopping')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.790',N'Migration_20170523083200',0,1,N'hu','2018-05-24T15:17:45.790',N'Migration_20170523083200',N'##hu: ServiceObjectCategory_Industry',15,4,N'Industry')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-06T16:12:58.733',N'Setup',0,1,N'fr','2019-02-06T16:12:58.733',N'Setup',N'Industrie',16,4,N'Industry')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-06T16:13:29.877',N'Setup',0,1,N'fr','2019-02-06T16:13:29.877',N'Setup',N'Aménagements publics',17,1,N'PublicFacilities')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-06T16:13:47.267',N'Setup',0,1,N'fr','2019-02-06T16:13:47.267',N'Setup',N'Shopping',18,3,N'Shopping')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-06T16:13:51.997',N'Setup',0,1,N'fr','2019-02-06T16:13:51.997',N'Setup',N'Sport',19,2,N'Sport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-06T16:13:57.817',N'Setup',0,1,N'fr','2019-02-06T16:13:57.817',N'Setup',N'Transport',20,0,N'Transport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.667',N'Creater',0,1,N'es','2021-07-13T08:43:50.667',N'Modifier',N'Transporte',21,0,N'Transport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.667',N'Creater',0,1,N'es','2021-07-13T08:43:50.667',N'Modifier',N'Instalaciones públicas',22,1,N'PublicFacilities')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.667',N'Creater',0,1,N'es','2021-07-13T08:43:50.667',N'Modifier',N'Deporte',23,2,N'Sport')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.667',N'Creater',0,1,N'es','2021-07-13T08:43:50.667',N'Modifier',N'Compras',24,3,N'Shopping')
INSERT [LU].[ServiceObjectCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceObjectCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.667',N'Creater',0,1,N'es','2021-07-13T08:43:50.667',N'Modifier',N'Industria',25,4,N'Industry')
SET IDENTITY_INSERT [LU].[ServiceObjectCategory] OFF



SET IDENTITY_INSERT [LU].[SparePartsBudgetInvoiceType] ON
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2014-06-24T11:34:10.873',N'Setup',0,1,N'en','2019-02-07T08:35:24.587',N'Setup',N'Invoice difference',0,1,N'InvoiceDifference')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2014-06-24T11:34:10.873',N'Setup',0,1,N'de','2019-02-07T08:35:24.583',N'Setup',N'Differenzberechnung',0,2,N'InvoiceDifference')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2014-06-24T11:34:10.873',N'Setup',0,1,N'en','2019-02-07T08:35:13.787',N'Setup',N'Complete invoice',1,3,N'CompleteInvoice')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2014-06-24T11:34:10.877',N'Setup',0,1,N'de','2019-02-07T08:35:13.787',N'Setup',N'Vollberechnung',1,4,N'CompleteInvoice')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:35:24.587',N'Setup',N'##hu: SparePartsBudgetInvoiceType_InvoiceDifference',0,5,N'InvoiceDifference')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:35:13.787',N'Setup',N'##hu: SparePartsBudgetInvoiceType_CompleteInvoice',1,6,N'CompleteInvoice')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2019-02-07T08:35:13.567',N'Setup',0,1,N'fr','2019-02-07T08:35:13.567',N'Setup',N'Facture complète',1,7,N'CompleteInvoice')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2019-02-07T08:35:24.377',N'Setup',0,1,N'fr','2019-02-07T08:35:24.377',N'Setup',N'Différence de facture',0,8,N'InvoiceDifference')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2021-07-13T08:43:50.670',N'Creater',0,1,N'es','2021-07-13T08:43:50.670',N'Modifier',N'Diferencia de la factura',0,9,N'InvoiceDifference')
INSERT [LU].[SparePartsBudgetInvoiceType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetInvoiceTypeId],[Value])
VALUES('2021-07-13T08:43:50.670',N'Creater',0,1,N'es','2021-07-13T08:43:50.670',N'Modifier',N'Factura completa',1,10,N'CompleteInvoice')
SET IDENTITY_INSERT [LU].[SparePartsBudgetInvoiceType] OFF


SET IDENTITY_INSERT [LU].[SparePartsBudgetTimeSpanUnit] ON
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.830',N'Setup',0,1,N'en','2019-02-07T08:36:36.290',N'Setup',N'per year',0,1,N'PerYear')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.830',N'Setup',0,1,N'de','2019-02-07T08:36:36.290',N'Setup',N'pro Jahr',0,2,N'PerYear')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.830',N'Setup',0,1,N'en','2019-02-07T08:36:38.460',N'Setup',N'per quarter',1,3,N'PerQuarter')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.833',N'Setup',0,1,N'de','2019-02-07T08:36:38.460',N'Setup',N'pro Quartal',1,4,N'PerQuarter')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.833',N'Setup',0,1,N'en','2019-02-07T08:36:40.797',N'Setup',N'per month',2,5,N'PerMonth')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.837',N'Setup',0,1,N'de','2019-02-07T08:36:40.797',N'Setup',N'pro Monat',2,6,N'PerMonth')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.837',N'Setup',1,1,N'en','2019-02-07T08:36:37.397',N'Setup',N'per service order',3,7,N'PerServiceOrder')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2014-06-24T11:34:10.837',N'Setup',1,1,N'de','2019-02-07T08:36:37.393',N'Setup',N'pro Serviceauftrag',3,8,N'PerServiceOrder')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:36:36.290',N'Setup',N'##hu: SparePartsBudgetTimeSpanUnit_PerYear',0,9,N'PerYear')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:36:38.460',N'Setup',N'##hu: SparePartsBudgetTimeSpanUnit_PerQuarter',1,10,N'PerQuarter')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:36:40.797',N'Setup',N'##hu: SparePartsBudgetTimeSpanUnit_PerMonth',2,11,N'PerMonth')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',1,1,N'hu','2019-02-07T08:36:37.397',N'Setup',N'##hu: SparePartsBudgetTimeSpanUnit_PerServiceOrder',3,12,N'PerServiceOrder')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2019-02-07T08:35:49.573',N'Setup',0,1,N'fr','2019-02-07T08:36:40.797',N'Setup',N'par mois',2,13,N'PerMonth')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2019-02-07T08:36:09.533',N'Setup',0,1,N'fr','2019-02-07T08:36:38.460',N'Setup',N'par trimestre',1,14,N'PerQuarter')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2019-02-07T08:36:23.853',N'Setup',1,1,N'fr','2019-02-07T08:36:37.397',N'Setup',N'par ordre de service',3,15,N'PerServiceOrder')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2019-02-07T08:36:36.063',N'Setup',0,1,N'fr','2019-02-07T08:36:36.063',N'Setup',N'par an',0,16,N'PerYear')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2021-07-13T08:43:50.677',N'Creater',0,1,N'es','2021-07-13T08:43:50.677',N'Modifier',N'por año',0,17,N'PerYear')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2021-07-13T08:43:50.677',N'Creater',0,1,N'es','2021-07-13T08:43:50.677',N'Modifier',N'por trimestre',1,18,N'PerQuarter')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2021-07-13T08:43:50.677',N'Creater',0,1,N'es','2021-07-13T08:43:50.677',N'Modifier',N'por mes',2,19,N'PerMonth')
INSERT [LU].[SparePartsBudgetTimeSpanUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[SparePartsBudgetTimeSpanUnitId],[Value])
VALUES('2021-07-13T08:43:50.677',N'Creater',1,1,N'es','2021-07-13T08:43:50.677',N'Modifier',N'por pedido de servicio',3,20,N'PerServiceOrder')
SET IDENTITY_INSERT [LU].[SparePartsBudgetTimeSpanUnit] OFF

SET IDENTITY_INSERT [SMS].[InstallationHeadStatus] ON
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#607d8b','2014-06-24T13:34:06.317',N'Setup',0,19,1,'en','2019-02-06T21:55:12.693',N'Setup',N'planned',0,N'0')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#607d8b','2014-06-24T13:34:06.317',N'Setup',0,20,1,'de','2019-02-06T21:55:12.693',N'Setup',N'In Planung',0,N'0')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#2196F3','2014-06-24T13:34:06.317',N'Setup',0,21,1,'en','2019-02-06T21:55:11.600',N'Setup',N'Installing',0,N'1')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#2196F3','2014-06-24T13:34:06.317',N'Setup',0,22,1,'de','2019-02-06T21:55:11.600',N'Setup',N'Errichtung',0,N'1')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#4CAF50','2014-06-24T13:34:06.317',N'Setup',0,23,1,'en','2019-02-06T21:55:10.360',N'Setup',N'Running',0,N'2')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#4CAF50','2014-06-24T13:34:06.317',N'Setup',0,24,1,'de','2019-02-06T21:55:10.360',N'Setup',N'In Betrieb',0,N'2')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#607d8b','2017-03-20T15:54:16',N'LookupManager',0,25,1,'hu','2019-02-06T21:55:12.697',N'Setup',N'tervezett',0,N'0')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#2196F3','2017-03-20T15:54:16',N'LookupManager',0,26,1,'hu','2019-02-06T21:55:11.600',N'Setup',N'telepítése',0,N'1')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#4CAF50','2017-03-20T15:54:16',N'LookupManager',0,27,1,'hu','2019-02-06T21:55:10.360',N'Setup',N'Működés közben',0,N'2')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#607d8b','2018-10-18T09:30:58.987',N'Setup',0,28,1,'fr','2019-02-06T21:55:12.697',N'Setup',N'Planifié',0,N'0')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#2196F3','2018-10-18T09:31:02.187',N'Setup',0,29,1,'fr','2019-02-06T21:55:11.600',N'Setup',N'En cours d''installation',0,N'1')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#4CAF50','2018-10-18T09:31:05.120',N'Setup',0,30,1,'fr','2019-02-06T21:55:10.360',N'Setup',N'En fonctionnement',0,N'2')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#ff9800','2018-10-18T09:31:07.800',N'Setup',0,31,1,'de','2019-02-06T21:55:43.847',N'Setup',N'Eingeschränkt betriebsbereit',0,N'3')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#ff9800','2018-10-18T09:31:20.703',N'Setup',0,32,1,'en','2019-02-06T21:55:43.847',N'Setup',N'Limited availability',0,N'3')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#ff9800','2018-10-18T09:31:20.707',N'Setup',0,33,1,'fr','2019-02-06T21:55:43.847',N'Setup',N'Disponibilité limitée',0,N'3')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#ff9800','2018-10-18T09:31:20.710',N'Setup',0,34,1,'hu','2019-02-06T21:55:43.847',N'Setup',N'Korlátozott elérhetőség',0,N'3')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#F44336','2018-10-18T09:31:27.767',N'Setup',0,35,1,'de','2019-02-06T21:55:42.083',N'Setup',N'Stillstand',0,N'4')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#F44336','2018-10-18T09:32:45.647',N'Setup',0,36,1,'en','2019-02-06T21:55:42.083',N'Setup',N'Breakdown',0,N'4')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#F44336','2018-10-18T09:32:45.650',N'Setup',0,37,1,'fr','2019-02-06T21:55:42.083',N'Setup',N'Arrêt',0,N'4')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#F44336','2018-10-18T09:32:45.650',N'Setup',0,38,1,'hu','2019-02-06T21:55:42.083',N'Setup',N'Bontás',0,N'4')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#607d8b','2021-07-13T08:43:50.727',N'Creater',0,39,1,'es','2021-07-13T08:43:50.727',N'Modifier',N'Planificado',0,N'0')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#2196F3','2021-07-13T08:43:50.727',N'Creater',0,40,1,'es','2021-07-13T08:43:50.727',N'Modifier',N'En instalación',0,N'1')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#4CAF50','2021-07-13T08:43:50.727',N'Creater',0,41,1,'es','2021-07-13T08:43:50.727',N'Modifier',N'Funcionando',0,N'2')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#ff9800','2021-07-13T08:43:50.727',N'Creater',0,42,1,'es','2021-07-13T08:43:50.727',N'Modifier',N'Funcionamiento limitado',0,N'3')
INSERT [SMS].[InstallationHeadStatus]([Color],[CreateDate],[CreateUser],[Favorite],[InstallationHeadStatusId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES(N'#F44336','2021-07-13T08:43:50.727',N'Creater',0,43,1,'es','2021-07-13T08:43:50.727',N'Modifier',N'Averiado',0,N'4')
SET IDENTITY_INSERT [SMS].[InstallationHeadStatus] OFF


SET IDENTITY_INSERT [SMS].[InstallationType] ON
INSERT [SMS].[InstallationType]([CreateDate],[CreateUser],[Favorite],[GroupKey],[InstallationTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.717',N'Migration_20170523083200',0,null,1,1,'en','2019-02-06T21:53:21.773',N'Setup',N'Other',0,N'0')
INSERT [SMS].[InstallationType]([CreateDate],[CreateUser],[Favorite],[GroupKey],[InstallationTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.717',N'Migration_20170523083200',0,null,2,1,'de','2019-02-06T21:53:21.773',N'Setup',N'Sonstige',0,N'0')
INSERT [SMS].[InstallationType]([CreateDate],[CreateUser],[Favorite],[GroupKey],[InstallationTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.717',N'Migration_20170523083200',0,null,3,1,'hu','2019-02-06T21:53:21.773',N'Setup',N'##hu: InstallationType_0',0,N'0')
INSERT [SMS].[InstallationType]([CreateDate],[CreateUser],[Favorite],[GroupKey],[InstallationTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2019-02-06T21:53:21.543',N'Setup',0,null,4,1,'fr','2019-02-06T21:53:21.543',N'Setup',N'Autre',0,N'0')
INSERT [SMS].[InstallationType]([CreateDate],[CreateUser],[Favorite],[GroupKey],[InstallationTypeId],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.720',N'Creater',0,null,5,1,'es','2021-07-13T08:43:50.720',N'Modifier',N'Otro',0,N'0')
SET IDENTITY_INSERT [SMS].[InstallationType] OFF


SET IDENTITY_INSERT [SMS].[NotificationStandardAction] ON
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'en','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Dismiss Immediately',3,'ffebd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'dismiss',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'de','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Sofort archivieren',4,'00ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'dismiss',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'en','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Set To Resolved',5,'01ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'resolved',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'de','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Auf gelöst setzen',6,'02ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'resolved',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'en','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Convert To Order',7,'03ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'convert',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'de','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Zum Auftrag machen',8,'04ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'convert',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'en','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Information',9,'05ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'info',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'de','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Informativ',10,'06ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'info',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'en','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'In Progress',11,'07ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'progress',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'de','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'Im Gange',12,'08ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'progress',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'hu','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'##hu: NotificationStandardAction_dismiss',13,'6bbe3578-850d-e711-b8da-001999cfc03d',0,null,null,N'dismiss',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'hu','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'##hu: NotificationStandardAction_resolved',14,'6cbe3578-850d-e711-b8da-001999cfc03d',0,null,null,N'resolved',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'hu','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'##hu: NotificationStandardAction_convert',15,'6dbe3578-850d-e711-b8da-001999cfc03d',0,null,null,N'convert',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'hu','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'##hu: NotificationStandardAction_info',16,'6ebe3578-850d-e711-b8da-001999cfc03d',0,null,null,N'info',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2018-05-24T15:17:45.750',N'Migration_20170523083200',0,1,'hu','2018-05-24T15:17:45.753',N'Migration_20170523083200',N'##hu: NotificationStandardAction_progress',17,'6fbe3578-850d-e711-b8da-001999cfc03d',0,null,null,N'progress',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2020-03-19T10:21:45.753',N'Migration_20170523083200',0,1,'fr','2020-03-19T10:21:45.753',N'Migration_20170523083200',N'Convertir en commande',18,'03ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'convert',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2020-03-19T10:21:45.753',N'Migration_20170523083200',0,1,'fr','2020-03-19T10:21:45.753',N'Migration_20170523083200',N'Renvoi immédiat',19,'ffebd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'dismiss',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2020-03-19T10:21:45.753',N'Migration_20170523083200',0,1,'fr','2020-03-19T10:21:45.753',N'Migration_20170523083200',N'Information',20,'05ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'info',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2020-03-19T10:21:45.753',N'Migration_20170523083200',0,1,'fr','2020-03-19T10:21:45.753',N'Migration_20170523083200',N'En cours',21,'07ecd3e3-2afb-e011-9911-000c299ffe9f',0,null,null,N'progress',null)
INSERT [SMS].[NotificationStandardAction]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[NotificationStandardActionId],[rowguid],[SortOrder],[TargetStatus],[TemplateOrderNo],[Value],[WorkflowTarget])
VALUES('2020-03-19T10:21:45.753',N'Migration_20170523083200',0,1,'fr','2020-03-19T10:21:45.753',N'Migration_20170523083200',N'Résolu',22,'6cbe3578-850d-e711-b8da-001999cfc03d',0,null,null,N'resolved',null)
SET IDENTITY_INSERT [SMS].[NotificationStandardAction] OFF


SET IDENTITY_INSERT [SMS].[ServiceContractLimitType] ON
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'en','2019-02-07T08:37:27.487',N'Setup',N'No material invoice',1,0,N'1')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'de','2019-02-07T08:37:27.487',N'Setup',N'Keine Materialberechnung',2,0,N'1')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'en','2019-02-07T08:38:07.347',N'Setup',N'From limit full invoice',3,1,N'2')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'de','2019-02-07T08:38:07.343',N'Setup',N'Ab Limit volle Berechnung',4,1,N'2')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'en','2019-02-07T08:39:04.797',N'Setup',N'From limit differential invoice',5,2,N'3')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'de','2019-02-07T08:39:04.797',N'Setup',N'Ab Limit Differenz-Berechnung',6,2,N'3')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'hu','2019-02-07T08:37:27.487',N'Setup',N'##hu: ServiceContractLimitType_1',7,0,N'1')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'hu','2019-02-07T08:38:07.347',N'Setup',N'##hu: ServiceContractLimitType_2',8,1,N'2')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.773',N'Migration_20170523083200',0,1,N'hu','2019-02-07T08:39:04.797',N'Setup',N'##hu: ServiceContractLimitType_3',9,2,N'3')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2019-02-07T08:37:27.260',N'Setup',0,1,N'fr','2019-02-07T08:37:27.260',N'Setup',N'Aucune facture matérielle',10,0,N'1')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2019-02-07T08:38:07.120',N'Setup',0,1,N'fr','2019-02-07T08:38:07.120',N'Setup',N'A partir d''une facture complète limitée',11,1,N'2')
INSERT [SMS].[ServiceContractLimitType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractLimitTypeId],[SortOrder],[Value])
VALUES('2019-02-07T08:39:04.573',N'Setup',0,1,N'fr','2019-02-07T08:39:04.573',N'Setup',N'A partir d''une facture différentielle limitée',12,2,N'3')
SET IDENTITY_INSERT [SMS].[ServiceContractLimitType] OFF


SET IDENTITY_INSERT [SMS].[ServiceContractStatus] ON
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'de','2018-05-24T17:17:45.383',N'Setup',N'Inaktiv',1,N'Pending,Active',2,N'Inactive')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',1,1,N'de','2018-05-24T17:17:45.383',N'Setup',N'Aktiv',2,N'Inactive',3,N'Active')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'de','2018-05-24T17:17:45.383',N'Setup',N'Abgelaufen',3,null,4,N'Expired')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'de','2018-05-24T17:17:45.383',N'Setup',N'Ausstehend',4,N'Inactive',1,N'Pending')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'en','2018-05-24T17:17:45.383',N'Setup',N'Inactive',5,N'Pending,Active',2,N'Inactive')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',1,1,N'en','2018-05-24T17:17:45.383',N'Setup',N'Active',6,N'Inactive',3,N'Active')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'en','2018-05-24T17:17:45.383',N'Setup',N'Expired',7,null,4,N'Expired')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'en','2018-05-24T17:17:45.383',N'Setup',N'Pending',8,N'Inactive',1,N'Pending')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'fr','2018-05-24T17:17:45.383',N'Setup',N'Inactif',9,N'Pending,Active',2,N'Inactive')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',1,1,N'fr','2018-05-24T17:17:45.383',N'Setup',N'Active',10,N'Inactive',3,N'Active')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'fr','2018-05-24T17:17:45.383',N'Setup',N'Expiré',11,null,4,N'Expired')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'fr','2018-05-24T17:17:45.383',N'Setup',N'En attente',12,N'Inactive',1,N'Pending')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'hu','2018-05-24T17:17:45.383',N'Setup',N'Inaktív',13,N'Pending,Active',2,N'Inactive')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',1,1,N'hu','2018-05-24T17:17:45.383',N'Setup',N'Aktív',14,N'Inactive',3,N'Active')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'hu','2018-05-24T17:17:45.383',N'Setup',N'Lejárt',15,null,4,N'Expired')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T17:17:45.383',N'Setup',0,1,N'hu','2018-05-24T17:17:45.383',N'Setup',N'Függoben',16,N'Inactive',1,N'Pending')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-12-15T10:43:49.393',N'Setup',0,1,N'es','2021-12-15T10:43:49.393',N'Setup',N'Inactivo',17,N'Pending,Active',2,N'Inactive')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-12-15T10:43:49.393',N'Setup',1,1,N'es','2021-12-15T10:43:49.393',N'Setup',N'Activo',18,N'Inactive',3,N'Active')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-12-15T10:43:49.393',N'Setup',0,1,N'es','2021-12-15T10:43:49.393',N'Setup',N'Caducado',19,null,4,N'Expired')
INSERT [SMS].[ServiceContractStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-12-15T10:43:49.393',N'Setup',0,1,N'es','2021-12-15T10:43:49.393',N'Setup',N'Pendiente',20,N'Inactive',1,N'Pending')
SET IDENTITY_INSERT [SMS].[ServiceContractStatus] OFF


SET IDENTITY_INSERT [SMS].[ServiceContractType] ON
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#000000','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'de','2019-02-07T08:42:22.117',N'Setup',N'Unbekannt',1,1,N'0')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#000000','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'en','2019-02-07T08:42:22.117',N'Setup',N'Unknown',2,1,N'0')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ededef','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'de','2019-02-07T08:42:41.627',N'Setup',N'Platin',3,2,N'1')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ededef','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'en','2019-02-07T08:42:41.627',N'Setup',N'Platinum',4,2,N'1')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ffd700','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'de','2019-02-07T08:43:05.733',N'Setup',N'Gold',5,3,N'2')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ffd700','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'en','2019-02-07T08:43:05.733',N'Setup',N'Gold',6,3,N'2')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#c0c0c0','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'de','2019-02-07T08:43:23.493',N'Setup',N'Silber',7,4,N'3')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#c0c0c0','2018-05-24T15:17:45.780',N'Migration_20170523083200',0,1,'en','2019-02-07T08:43:23.493',N'Setup',N'Silver',8,4,N'3')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#000000','2018-05-24T15:25:40.970',N'LookupManager',0,1,'hu','2019-02-07T08:42:22.117',N'Setup',N'Ismeretlen',9,1,N'0')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#000000','2019-02-07T08:42:21.867',N'Setup',0,1,'fr','2019-02-07T08:42:21.867',N'Setup',N'Inconnu',10,1,N'0')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ededef','2019-02-07T08:42:41.197',N'Setup',0,1,'fr','2019-02-07T08:42:41.197',N'Setup',N'Platine',11,2,N'1')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ededef','2019-02-07T08:42:41.410',N'Setup',0,1,'hu','2019-02-07T08:42:41.410',N'Setup',N'Platina',12,2,N'1')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ffd700','2019-02-07T08:43:05.297',N'Setup',0,1,'fr','2019-02-07T08:43:05.297',N'Setup',N'Or',13,3,N'2')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ffd700','2019-02-07T08:43:05.517',N'Setup',0,1,'hu','2019-02-07T08:43:05.517',N'Setup',N'Arany',14,3,N'2')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#c0c0c0','2019-02-07T08:43:23.067',N'Setup',0,1,'fr','2019-02-07T08:43:23.067',N'Setup',N'Argent',15,4,N'3')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#c0c0c0','2019-02-07T08:43:23.280',N'Setup',0,1,'hu','2019-02-07T08:43:23.280',N'Setup',N'Ezüst',16,4,N'3')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#000000','2021-07-13T08:43:50.730',N'Creater',0,1,'es','2021-07-13T08:43:50.730',N'Modifier',N'Desconocido',17,1,N'0')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ededef','2021-07-13T08:43:50.730',N'Creater',0,1,'es','2021-07-13T08:43:50.730',N'Modifier',N'Platinum',18,2,N'1')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#ffd700','2021-07-13T08:43:50.730',N'Creater',0,1,'es','2021-07-13T08:43:50.730',N'Modifier',N'Gold',19,3,N'2')
INSERT [SMS].[ServiceContractType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceContractTypeId],[SortOrder],[Value])
VALUES(N'#c0c0c0','2021-07-13T08:43:50.730',N'Creater',0,1,'es','2021-07-13T08:43:50.730',N'Modifier',N'Silver',20,4,N'3')
SET IDENTITY_INSERT [SMS].[ServiceContractType] OFF


SET IDENTITY_INSERT [SMS].[ServiceNotificationCategory] ON
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'en','2019-02-08T07:35:48.420',N'Setup',N'Error',1,0,N'0')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'de','2019-02-08T07:35:48.417',N'Setup',N'Fehler',2,0,N'0')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'en','2019-02-08T07:35:52.790',N'Setup',N'Question',3,0,N'1')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'de','2019-02-08T07:35:52.790',N'Setup',N'Frage',4,0,N'1')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'en','2019-02-08T07:36:04.637',N'Setup',N'Complaint',5,0,N'2')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'de','2019-02-08T07:36:04.637',N'Setup',N'Beschwerde',6,0,N'2')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'en','2019-02-08T07:36:12.220',N'Setup',N'Suggestion',7,0,N'3')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'de','2019-02-08T07:36:12.220',N'Setup',N'Anregung',8,0,N'3')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'hu','2019-02-08T07:35:48.420',N'Setup',N'##hu: ServiceCaseCategory_0',9,0,N'0')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'hu','2019-02-08T07:35:52.790',N'Setup',N'##hu: ServiceCaseCategory_1',10,0,N'1')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'hu','2019-02-08T07:36:04.640',N'Setup',N'##hu: ServiceCaseCategory_2',11,0,N'2')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.760',N'Migration_20170523083200',0,1,'hu','2019-02-08T07:36:12.220',N'Setup',N'##hu: ServiceCaseCategory_3',12,0,N'3')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:35:48.200',N'Setup',0,1,'fr','2019-02-08T07:35:48.200',N'Setup',N'Erreur',13,0,N'0')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:35:52.570',N'Setup',0,1,'fr','2019-02-08T07:35:52.570',N'Setup',N'Question',14,0,N'1')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:36:04.413',N'Setup',0,1,'fr','2019-02-08T07:36:04.413',N'Setup',N'Réclamation',15,0,N'2')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:36:12.007',N'Setup',0,1,'fr','2019-02-08T07:36:12.007',N'Setup',N'Proposition',16,0,N'3')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.697',N'Creater',0,1,'es','2021-07-13T08:43:50.697',N'Modifier',N'Error',17,0,N'0')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.697',N'Creater',0,1,'es','2021-07-13T08:43:50.697',N'Modifier',N'Pregunta',18,0,N'1')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.697',N'Creater',0,1,'es','2021-07-13T08:43:50.697',N'Modifier',N'Queja',19,0,N'2')
INSERT [SMS].[ServiceNotificationCategory]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationCategoryId],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.697',N'Creater',0,1,'es','2021-07-13T08:43:50.697',N'Modifier',N'Sugerencia',20,0,N'3')
SET IDENTITY_INSERT [SMS].[ServiceNotificationCategory] OFF


SET IDENTITY_INSERT [SMS].[ServiceNotificationStatus] ON
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Preparation',1,'en','2019-02-08T07:36:46.760',N'Setup',N'New',1,N'2,4,6',0,0)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Preparation',1,'de','2019-02-08T07:36:46.760',N'Setup',N'Neu',2,N'2,4,6',0,0)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Preparation',1,'en','2019-02-08T07:36:55.060',N'Setup',N'On Hold',3,N'4,5,6',2,2)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Preparation',1,'de','2019-02-08T07:36:55.060',N'Setup',N'Zurückgestellt',4,N'4,5,6',2,2)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'InProgress',1,'en','2019-02-08T07:36:59.993',N'Setup',N'In Progress',5,N'2,6',4,4)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'InProgress',1,'de','2019-02-08T07:36:59.993',N'Setup',N'In Bearbeitung',6,N'2,6',4,4)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'InProgress',1,'en','2020-04-01T11:18:29.653',N'default.1',N'Reopened',7,N'2,4,6',5,5)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'InProgress',1,'de','2020-04-01T11:18:29.647',N'default.1',N'Wieder eröffnet',8,N'2,4,6',5,5)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Closed',1,'en','2019-02-08T07:43:34.010',N'Setup',N'Closed',9,N'5',6,6)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Closed',1,'de','2019-02-08T07:43:34.007',N'Setup',N'Geschlossen',10,N'5',6,6)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Preparation',1,'hu','2019-02-08T07:36:46.760',N'Setup',N'##hu: ServiceCaseStatus_0',11,N'2,4,6',0,0)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Preparation',1,'hu','2019-02-08T07:36:55.060',N'Setup',N'##hu: ServiceCaseStatus_2',12,N'4,5,6',2,2)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'InProgress',1,'hu','2019-02-08T07:36:59.993',N'Setup',N'##hu: ServiceCaseStatus_4',13,N'2,6',4,4)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'InProgress',1,'hu','2018-05-24T15:17:45.770',N'Migration_20170523083200',N'##hu: ServiceCaseStatus_5',14,N'2,4,6',5,5)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2018-05-24T15:17:45.767',N'Migration_20170523083200',0,N'Closed',1,'hu','2019-02-08T07:43:34.010',N'Setup',N'##hu: ServiceCaseStatus_6',15,N'5',6,6)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:36:46.533',N'Setup',0,N'Preparation',1,'fr','2019-02-08T07:36:46.533',N'Setup',N'Nouveau',16,N'2,4,6',0,0)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:36:54.840',N'Setup',0,N'Preparation',1,'fr','2019-02-08T07:36:54.840',N'Setup',N'En attente',17,N'4,5,6',2,2)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:36:59.780',N'Setup',0,N'InProgress',1,'fr','2019-02-08T07:36:59.780',N'Setup',N'En cours',18,N'2,6',4,4)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2019-02-08T07:37:05.453',N'Setup',0,N'Closed',1,'fr','2019-02-08T07:43:34.010',N'Setup',N'Clôturé',19,N'5',6,6)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2020-04-01T11:18:29.613',N'default.1',0,N'InProgress',1,'fr','2020-04-01T11:18:29.613',N'default.1',N'A rouvert',21,N'2,4,6',5,5)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.683',N'Creater',0,N'Preparation',1,'es','2021-07-13T08:43:50.683',N'Modifier',N'Nuevo',22,N'2,4,6',0,0)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.683',N'Creater',0,N'Preparation',1,'es','2021-07-13T08:43:50.683',N'Modifier',N'En espera',23,N'4,5,6',2,2)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.683',N'Creater',0,N'InProgress',1,'es','2021-07-13T08:43:50.683',N'Modifier',N'En curso',24,N'2,6',4,4)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.683',N'Creater',0,N'InProgress',1,'es','2021-07-13T08:43:50.683',N'Modifier',N'Reabierto',25,N'2,4,6',5,5)
INSERT [SMS].[ServiceNotificationStatus]([Color],[CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES(N'#9E9E9E','2021-07-13T08:43:50.683',N'Creater',0,N'Closed',1,'es','2021-07-13T08:43:50.683',N'Modifier',N'Cerrado',26,N'5',6,6)
SET IDENTITY_INSERT [SMS].[ServiceNotificationStatus] OFF


SET IDENTITY_INSERT [SMS].[ServiceOrderDispatchRejectReason] ON
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.777',N'Setup',0,1,N'en','2019-02-07T08:33:54.327',N'Setup',N'False alarm',1,N'Completed',1,0,N'FalseAlarm')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.780',N'Setup',0,1,N'de','2019-02-07T08:33:54.327',N'Setup',N'Fehlalarm',2,N'Completed',1,0,N'FalseAlarm')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.780',N'Setup',0,1,N'en','2019-02-07T08:33:16.970',N'Setup',N'Conflicting dates',3,N'ReadyForScheduling',1,1,N'ConflictingDates')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.780',N'Setup',0,1,N'de','2019-02-07T08:33:16.970',N'Setup',N'Terminkonflikt',4,N'ReadyForScheduling',1,1,N'ConflictingDates')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.783',N'Setup',0,1,N'en','2019-02-07T08:33:28.253',N'Setup',N'Customer not accessible',5,N'ReadyForScheduling',1,2,N'CustomerNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.783',N'Setup',0,1,N'de','2019-02-07T08:33:28.253',N'Setup',N'Kunde nicht erreichbar',6,N'ReadyForScheduling',1,2,N'CustomerNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.783',N'Setup',0,1,N'en','2019-02-07T08:34:05.280',N'Setup',N'Installation not accessible',7,N'ReadyForScheduling',1,3,N'InstallationNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2014-06-24T11:34:07.787',N'Setup',0,1,N'de','2019-02-07T08:34:05.280',N'Setup',N'Anlage nicht erreichbar',8,N'ReadyForScheduling',1,3,N'InstallationNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:33:54.327',N'Setup',N'##hu: ServiceOrderDispatchRejectReason_FalseAlarm',9,N'Completed',1,0,N'FalseAlarm')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:33:16.970',N'Setup',N'##hu: ServiceOrderDispatchRejectReason_ConflictingDates',10,N'ReadyForScheduling',1,1,N'ConflictingDates')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:33:28.253',N'Setup',N'##hu: ServiceOrderDispatchRejectReason_CustomerNotAccessible',11,N'ReadyForScheduling',1,2,N'CustomerNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:34:05.280',N'Setup',N'##hu: ServiceOrderDispatchRejectReason_InstallationNotAccessible',12,N'ReadyForScheduling',1,3,N'InstallationNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2018-05-24T15:17:49.820',N'Migration_20171120172700',0,1,N'en','2019-02-07T08:34:22.717',N'Setup',N'Rejected by system',13,N'ReadyForScheduling',0,4,N'RejectedBySystem')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2018-05-24T15:17:49.820',N'Migration_20171120172700',0,1,N'de','2019-02-07T08:34:22.717',N'Setup',N'Vom System abgelehnt',14,N'ReadyForScheduling',0,4,N'RejectedBySystem')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2019-02-07T08:33:16.747',N'Setup',0,1,N'fr','2019-02-07T08:33:16.747',N'Setup',N'Dates contradictoires',15,N'ReadyForScheduling',1,1,N'ConflictingDates')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2019-02-07T08:33:28.040',N'Setup',0,1,N'fr','2019-02-07T08:33:28.040',N'Setup',N'Client non accessible',16,N'ReadyForScheduling',1,2,N'CustomerNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2019-02-07T08:33:47.237',N'Setup',0,1,N'fr','2019-02-07T08:33:54.327',N'Setup',N'Fausse alerte',17,N'Completed',1,0,N'FalseAlarm')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2019-02-07T08:34:05.057',N'Setup',0,1,N'fr','2019-02-07T08:34:05.057',N'Setup',N'Installation non accessible',18,N'ReadyForScheduling',1,3,N'InstallationNotAccessible')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2019-02-07T08:34:22.280',N'Setup',0,1,N'fr','2019-02-07T08:34:22.280',N'Setup',N'Rejeté par le système',19,N'ReadyForScheduling',0,4,N'RejectedBySystem')
INSERT [SMS].[ServiceOrderDispatchRejectReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchRejectReasonId],[ServiceOrderStatus],[ShowInMobileClient],[SortOrder],[Value])
VALUES('2019-02-07T08:34:22.503',N'Setup',0,1,N'hu','2019-02-07T08:34:22.503',N'Setup',N'A rendszer elutasította',20,N'ReadyForScheduling',0,4,N'RejectedBySystem')
SET IDENTITY_INSERT [SMS].[ServiceOrderDispatchRejectReason] OFF


SET IDENTITY_INSERT [SMS].[ServiceOrderDispatchStatus] ON
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#ECFFE5','2014-06-24T13:34:06.203',N'Setup',0,1,N'de','2014-06-24T13:34:06.227',N'Setup',N'Eingeplant',1,1,N'Scheduled')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#ECFFE5','2014-06-24T13:34:06.203',N'Setup',0,1,N'en','2014-06-24T13:34:06.227',N'Setup',N'Scheduled',2,1,N'Scheduled')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#C6FFB2','2014-06-24T13:34:06.203',N'Setup',0,1,N'de','2014-06-24T13:34:06.227',N'Setup',N'Freigegeben',3,2,N'Released')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#C6FFB2','2014-06-24T13:34:06.203',N'Setup',0,1,N'en','2014-06-24T13:34:06.227',N'Setup',N'Released',4,2,N'Released')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#2196F3','2014-06-24T13:34:06.203',N'Setup',0,1,N'de','2014-06-24T13:34:06.227',N'Setup',N'In Bearbeitung',5,4,N'InProgress')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#2196F3','2014-06-24T13:34:06.203',N'Setup',0,1,N'en','2014-06-24T13:34:06.227',N'Setup',N'In Progress',6,4,N'InProgress')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#0A6EBD','2014-06-24T13:34:06.203',N'Setup',0,1,N'de','2014-06-24T13:34:06.227',N'Setup',N'Vom Kunden unterschrieben',7,5,N'SignedByCustomer')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#0A6EBD','2014-06-24T13:34:06.203',N'Setup',0,1,N'en','2014-06-24T13:34:06.227',N'Setup',N'Signed by customer',8,5,N'SignedByCustomer')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#EDEDED','2014-06-24T13:34:06.203',N'Setup',0,1,N'de','2014-06-24T13:34:06.227',N'Setup',N'Abgeschlossen (Folgeeinsatz erforderlich)',9,6,N'ClosedNotComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#EDEDED','2014-06-24T13:34:06.203',N'Setup',0,1,N'en','2014-06-24T13:34:06.227',N'Setup',N'Closed (Follow-up dispatch required)',10,6,N'ClosedNotComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#D3D3D3','2014-06-24T13:34:06.203',N'Setup',0,1,N'de','2014-06-24T13:34:06.227',N'Setup',N'Abgeschlossen',11,7,N'ClosedComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#D3D3D3','2014-06-24T13:34:06.203',N'Setup',0,1,N'en','2014-06-24T13:34:06.227',N'Setup',N'Closed',12,7,N'ClosedComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#8DFF66','2014-06-24T11:34:07.753',N'Setup',0,1,N'de','2014-06-24T11:34:07.753',N'Setup',N'Gelesen',13,3,N'Read')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#8DFF66','2014-06-24T11:34:07.757',N'Setup',0,1,N'en','2014-06-24T11:34:07.757',N'Setup',N'Read',14,3,N'Read')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#FF0000','2014-06-24T11:34:07.757',N'Setup',0,1,N'de','2014-06-24T11:34:07.757',N'Setup',N'Zurückgewiesen',15,8,N'Rejected')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#FF0000','2014-06-24T11:34:07.757',N'Setup',0,1,N'en','2014-06-24T11:34:07.757',N'Setup',N'Rejected',16,8,N'Rejected')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#ECFFE5','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Tervezve',17,1,N'Scheduled')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#C6FFB2','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Kiadva',18,2,N'Released')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#2196F3','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Folyamatban',19,4,N'InProgress')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#0A6EBD','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Ügyfél által aláírva',20,5,N'SignedByCustomer')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#EDEDED','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Lezárva (Nem befejezett)',21,6,N'ClosedNotComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#D3D3D3','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Lezárva',22,7,N'ClosedComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#8DFF66','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Olvasva',23,3,N'Read')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#FF0000','2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'Visszautasítva',24,8,N'Rejected')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#ECFFE5','2018-05-24T15:25:41.047',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.047',N'LookupManager',N'Planifié',25,1,N'Scheduled')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#C6FFB2','2018-05-24T15:25:41.050',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.050',N'LookupManager',N'Validé',26,2,N'Released')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#2196F3','2018-05-24T15:25:41.053',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.053',N'LookupManager',N'En cours',27,4,N'InProgress')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#0A6EBD','2018-05-24T15:25:41.057',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.057',N'LookupManager',N'Signé par le client',28,5,N'SignedByCustomer')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#EDEDED','2018-05-24T15:25:41.060',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.060',N'LookupManager',N'Clôturé (Intervention de suivi nécessaire)',29,6,N'ClosedNotComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#D3D3D3','2018-05-24T15:25:41.063',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.063',N'LookupManager',N'Clôturé',30,7,N'ClosedComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#8DFF66','2018-05-24T15:25:41.067',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.067',N'LookupManager',N'Lu',31,3,N'Read')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#FF0000','2018-05-24T15:25:41.070',N'LookupManager',0,1,N'fr','2018-05-24T15:25:41.070',N'LookupManager',N'Rejeté',32,8,N'Rejected')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#ECFFE5','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'Programado',33,1,N'Scheduled')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#C6FFB2','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'Publicado',34,2,N'Released')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#2196F3','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'En curso',35,4,N'InProgress')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#0A6EBD','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'Firmado por el cliente',36,5,N'SignedByCustomer')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#EDEDED','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'Cerrado (se requiere una orden de seguimiento)',37,6,N'ClosedNotComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#D3D3D3','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'Cerrado',38,7,N'ClosedComplete')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#8DFF66','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'Leído',39,3,N'Read')
INSERT [SMS].[ServiceOrderDispatchStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderDispatchTechnicianStatusId],[SortOrder],[Value])
VALUES(N'#FF0000','2021-07-13T08:43:50.713',N'Creater',0,1,N'es','2021-07-13T08:43:50.713',N'Modifier',N'Rechazado',40,8,N'Rejected')
SET IDENTITY_INSERT [SMS].[ServiceOrderDispatchStatus] OFF

SET IDENTITY_INSERT [SMS].[ServiceOrderNoInvoiceReason] ON
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'en','2019-02-07T08:32:16.917',N'Setup',N'Goodwill',1,0,N'Goodwill')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'de','2019-02-07T08:32:16.917',N'Setup',N'Kulanz',2,0,N'Goodwill')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'en','2019-02-07T08:32:30.160',N'Setup',N'Rectification',3,1,N'Rectification')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'de','2019-02-07T08:32:30.160',N'Setup',N'Nachbesserung',4,1,N'Rectification')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'en','2019-02-07T08:32:41.660',N'Setup',N'Warranty',5,2,N'Warranty')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'de','2019-02-07T08:32:41.660',N'Setup',N'Gewährleistung',6,2,N'Warranty')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'hu','2019-02-07T08:32:16.917',N'Setup',N'##hu: ServiceOrderNoInvoiceReason_Goodwill',7,0,N'Goodwill')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'hu','2019-02-07T08:32:30.160',N'Setup',N'##hu: ServiceOrderNoInvoiceReason_Rectification',8,1,N'Rectification')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.803',N'Migration_20170523083200',0,1,N'hu','2019-02-07T08:32:41.660',N'Setup',N'##hu: ServiceOrderNoInvoiceReason_Warranty',9,2,N'Warranty')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2019-02-07T08:32:16.697',N'Setup',0,1,N'fr','2019-02-07T08:32:16.697',N'Setup',N'A volonté',10,0,N'Goodwill')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2019-02-07T08:32:29.883',N'Setup',0,1,N'fr','2019-02-07T08:32:29.883',N'Setup',N'Rectification',11,1,N'Rectification')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2019-02-07T08:32:41.430',N'Setup',0,1,N'fr','2019-02-07T08:32:41.430',N'Setup',N'Garantie',12,2,N'Warranty')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2021-12-15T10:43:49.407',N'Setup',0,1,N'es','2021-12-15T10:43:49.407',N'Setup',N'Buena voluntad',13,0,N'Goodwill')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2021-12-15T10:43:49.407',N'Setup',0,1,N'es','2021-12-15T10:43:49.407',N'Setup',N'Rectificación',14,1,N'Rectification')
INSERT [SMS].[ServiceOrderNoInvoiceReason]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderNoInvoiceReasonId],[SortOrder],[Value])
VALUES('2021-12-15T10:43:49.407',N'Setup',0,1,N'es','2021-12-15T10:43:49.407',N'Setup',N'Garantía',15,2,N'Warranty')
SET IDENTITY_INSERT [SMS].[ServiceOrderNoInvoiceReason] OFF

SET IDENTITY_INSERT [SMS].[ServiceOrderStatus] ON
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Preparation',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Neu',62,N'ReadyForScheduling',1,N'New')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Preparation',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'New',63,N'ReadyForScheduling',1,N'New')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Bereit zum Einplanen',64,N'New',2,N'ReadyForScheduling')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Ready for Scheduling',65,N'New',2,N'ReadyForScheduling')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Eingeplant',66,null,3,N'Scheduled')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Scheduled',67,null,3,N'Scheduled')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Teilweise freigegeben',68,null,4,N'PartiallyReleased')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Partially Released',69,null,4,N'PartiallyReleased')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Freigegeben',70,null,5,N'Released')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Released',71,null,5,N'Released')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'In Bearbeitung',72,null,6,N'InProgress')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'In Progress',73,null,6,N'InProgress')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Teilweise abgeschlossen',74,null,7,N'PartiallyCompleted')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Partially completed',75,null,7,N'PartiallyCompleted')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Technisch abgeschlossen',76,N'PostProcessing',8,N'Completed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Technically completed',77,N'PostProcessing',8,N'Completed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Nachbearbeitung',78,N'ReadyForInvoice,Completed',9,N'PostProcessing')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Post-processing',79,N'ReadyForInvoice,Completed',9,N'PostProcessing')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Bereit zur Faktura',80,N'PostProcessing,Invoiced',10,N'ReadyForInvoice')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Ready for Invoice',81,N'PostProcessing,Invoiced',10,N'ReadyForInvoice')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Fakturiert',82,N'Closed',11,N'Invoiced')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Invoiced',83,N'Closed',11,N'Invoiced')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Closed',1,'de','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Geschlossen',84,null,12,N'Closed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Closed',1,'en','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Closed',85,null,12,N'Closed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Preparation',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Új',86,N'ReadyForScheduling',1,N'New')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Tervezésre kész',87,N'New',2,N'ReadyForScheduling')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Tervezve',88,null,3,N'Scheduled')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Részben kiadva',89,null,4,N'PartiallyReleased')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Scheduling',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Kiadva',90,null,5,N'Released')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Folyamatban',91,null,6,N'InProgress')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Részben befejezett',92,null,7,N'PartiallyCompleted')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'InProgress',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Befejezve',93,N'PostProcessing',8,N'Completed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Utó feldolgozás',94,N'ReadyForInvoice,Completed',9,N'PostProcessing')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Számlázásra kész',95,N'PostProcessing,Invoiced',10,N'ReadyForInvoice')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'PostProcessing',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Számlázva',96,N'Closed',11,N'Invoiced')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.810',N'Migration_20170523083200',0,N'Closed',1,'hu','2018-05-24T15:17:45.813',N'Migration_20170523083200',N'Lezárva',97,null,12,N'Closed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.873',N'LookupManager',0,N'Preparation',1,'fr','2018-05-24T15:25:40.873',N'LookupManager',N'Nouveau',98,N'ReadyForScheduling',1,N'New')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.877',N'LookupManager',0,N'Scheduling',1,'fr','2018-05-24T15:25:40.877',N'LookupManager',N'Prêt pour planification',99,N'New',2,N'ReadyForScheduling')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.880',N'LookupManager',0,N'Scheduling',1,'fr','2018-05-24T15:25:40.880',N'LookupManager',N'Planifié',100,null,3,N'Scheduled')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.883',N'LookupManager',0,N'Scheduling',1,'fr','2018-05-24T15:25:40.883',N'LookupManager',N'Partiellement validé',101,null,4,N'PartiallyReleased')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.887',N'LookupManager',0,N'Scheduling',1,'fr','2018-05-24T15:25:40.887',N'LookupManager',N'Attribué',102,null,5,N'Released')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.890',N'LookupManager',0,N'InProgress',1,'fr','2018-05-24T15:25:40.890',N'LookupManager',N'En cours',103,null,6,N'InProgress')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.893',N'LookupManager',0,N'InProgress',1,'fr','2018-05-24T15:25:40.893',N'LookupManager',N'Partiellement clôturé',104,null,7,N'PartiallyCompleted')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.900',N'LookupManager',0,N'InProgress',1,'fr','2018-05-24T15:25:40.900',N'LookupManager',N'Fermé',105,N'PostProcessing',8,N'Completed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.903',N'LookupManager',0,N'PostProcessing',1,'fr','2018-05-24T15:25:40.903',N'LookupManager',N'Post traitement',106,N'ReadyForInvoice,Completed',9,N'PostProcessing')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.907',N'LookupManager',0,N'PostProcessing',1,'fr','2018-05-24T15:25:40.907',N'LookupManager',N'Prêt pour facturation',107,N'PostProcessing,Invoiced',10,N'ReadyForInvoice')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.910',N'LookupManager',0,N'PostProcessing',1,'fr','2018-05-24T15:25:40.910',N'LookupManager',N'Facturé',108,N'Closed',11,N'Invoiced')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2018-05-24T15:25:40.913',N'LookupManager',0,N'Closed',1,'fr','2018-05-24T15:25:40.913',N'LookupManager',N'Clôturé',109,null,12,N'Closed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'Preparation',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Nuevo',110,N'ReadyForScheduling',1,N'New')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'Scheduling',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Listo para programación',111,N'New',2,N'ReadyForScheduling')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'Scheduling',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Programado',112,null,3,N'Scheduled')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'Scheduling',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Parcialmente publicado',113,null,4,N'PartiallyReleased')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'Scheduling',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Publicado',114,null,5,N'Released')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'InProgress',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'En curso',115,null,6,N'InProgress')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'InProgress',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Parcialmente completado',116,N'PostProcessing',7,N'PartiallyCompleted')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'InProgress',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Técnicamente completado',117,null,8,N'Completed')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'PostProcessing',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Postprocesamiento',118,N'ReadyForInvoice,Completed',9,N'PostProcessing')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'PostProcessing',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Listo para la factura',119,N'PostProcessing,Invoiced',10,N'ReadyForInvoice')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'PostProcessing',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Facturado',120,N'Closed',11,N'Invoiced')
INSERT [SMS].[ServiceOrderStatus]([CreateDate],[CreateUser],[Favorite],[Groups],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderStatusId],[SettableStatuses],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.707',N'Creater',0,N'Closed',1,'es','2021-07-13T08:43:50.707',N'Modifier',N'Cerrado',121,null,12,N'Closed')
SET IDENTITY_INSERT [SMS].[ServiceOrderStatus] OFF

SET IDENTITY_INSERT [SMS].[ServiceOrderTimeStatus] ON
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#9e9e9e','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'de','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Erstellt',1,0,N'Created')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#9e9e9e','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'en','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Created',2,0,N'Created')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#9e9e9e','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'fr','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Établi',3,0,N'Created')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#9e9e9e','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'hu','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Alkotó',4,0,N'Created')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#2196f3','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'de','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Gestartet',5,1,N'Started')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#2196f3','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'en','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Started',6,1,N'Started')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#2196f3','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'fr','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Commencé',7,1,N'Started')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#2196f3','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'hu','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Lépések',8,1,N'Started')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#ff9800','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'de','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Unterbrochen',9,2,N'Interrupted')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#ff9800','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'en','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Interrupted',10,2,N'Interrupted')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#ff9800','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'fr','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Interrompu',11,2,N'Interrupted')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#ff9800','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'hu','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Megszakított',12,2,N'Interrupted')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#4caf50','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'de','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Abgeschlossen',13,3,N'Finished')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#4caf50','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'en','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Finished',14,3,N'Finished')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#4caf50','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'fr','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Terminé',15,3,N'Finished')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#4caf50','2019-12-05T18:36:02.937',N'Migration_20191015130600',0,1,'hu','2019-12-05T18:36:02.937',N'Migration_20191015130600',N'Befejezett',16,3,N'Finished')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#9e9e9e','2021-12-15T10:43:49.420',N'Setup',0,1,'es','2021-12-15T10:43:49.420',N'Setup',N'Creado',17,0,N'Created')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#2196f3','2021-12-15T10:43:49.420',N'Setup',0,1,'es','2021-12-15T10:43:49.420',N'Setup',N'Empezado',18,1,N'Started')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#ff9800','2021-12-15T10:43:49.420',N'Setup',0,1,'es','2021-12-15T10:43:49.420',N'Setup',N'Interrumpido',19,2,N'Interrupted')
INSERT [SMS].[ServiceOrderTimeStatus]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceOrderTimeStatusId],[SortOrder],[Value])
VALUES(N'#4caf50','2021-12-15T10:43:49.420',N'Setup',0,1,'es','2021-12-15T10:43:49.420',N'Setup',N'Finalizado',20,3,N'Finished')
SET IDENTITY_INSERT [SMS].[ServiceOrderTimeStatus] OFF

SET IDENTITY_INSERT [SMS].[ServiceOrderType] ON
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#009688','2018-05-24T15:17:45.850',N'Migration_20170523083200',0,1,'en',1,'2019-02-07T08:50:22.920',N'Setup',N'Maintenance Order',N'SMS.ServiceOrderHead.MaintenanceOrder',1,1,0,N'0')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#009688','2018-05-24T15:17:45.850',N'Migration_20170523083200',0,1,'de',1,'2019-02-07T08:50:22.920',N'Setup',N'Wartungsauftrag',N'SMS.ServiceOrderHead.MaintenanceOrder',2,1,0,N'0')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#FF9800','2018-05-24T15:17:45.850',N'Migration_20170523083200',1,1,'en',0,'2019-02-07T08:50:44.843',N'Setup',N'Correctice Serviceorder',N'SMS.ServiceOrderHead.ServiceOrder',3,1,0,N'1')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#FF9800','2018-05-24T15:17:45.850',N'Migration_20170523083200',1,1,'de',0,'2019-02-07T08:50:44.843',N'Setup',N'Reparaturauftrag',N'SMS.ServiceOrderHead.ServiceOrder',4,1,0,N'1')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#607D8B','2018-05-24T15:17:45.850',N'Migration_20170523083200',0,1,'en',0,'2019-02-07T08:50:59.770',N'Setup',N'Template Order',N'SMS.ServiceOrderHead.ServiceOrder',5,1,0,N'2')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#607D8B','2018-05-24T15:17:45.850',N'Migration_20170523083200',0,1,'de',0,'2019-02-07T08:50:59.770',N'Setup',N'Auftragsvorlage',N'SMS.ServiceOrderHead.ServiceOrder',6,1,0,N'2')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#009688','2018-05-24T15:17:45.850',N'Migration_20170523083200',0,1,'hu',1,'2019-02-07T08:50:22.920',N'Setup',N'##hu: ServiceOrderType_0',N'SMS.ServiceOrderHead.MaintenanceOrder',7,1,0,N'0')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#FF9800','2018-05-24T15:17:45.850',N'Migration_20170523083200',1,1,'hu',0,'2019-02-07T08:50:44.843',N'Setup',N'##hu: ServiceOrderType_1',N'SMS.ServiceOrderHead.ServiceOrder',8,1,0,N'1')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#607D8B','2018-05-24T15:17:45.850',N'Migration_20170523083200',0,1,'hu',0,'2019-02-07T08:50:59.770',N'Setup',N'##hu: ServiceOrderType_2',N'SMS.ServiceOrderHead.ServiceOrder',9,1,0,N'2')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#009688','2019-02-07T08:50:22.693',N'Setup',0,1,'fr',1,'2019-02-07T08:50:22.693',N'Setup',N'Ordre de maintenance',N'SMS.ServiceOrderHead.MaintenanceOrder',10,1,0,N'0')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#FF9800','2019-02-07T08:50:44.623',N'Setup',1,1,'fr',0,'2019-02-07T08:50:44.623',N'Setup',N'Ordre de service correctif',N'SMS.ServiceOrderHead.ServiceOrder',11,1,0,N'1')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#607D8B','2019-02-07T08:50:59.540',N'Setup',0,1,'fr',0,'2019-02-07T08:50:59.540',N'Setup',N'Ordre de modèle',N'SMS.ServiceOrderHead.ServiceOrder',12,1,0,N'2')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#009688','2021-07-13T08:43:50.700',N'Creater',0,1,'es',1,'2021-07-13T08:43:50.700',N'Modifier',N'Orden de mantenimiento',N'SMS.ServiceOrderHead.MaintenanceOrder',13,1,0,N'0')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#FF9800','2021-07-13T08:43:50.700',N'Creater',1,1,'es',0,'2021-07-13T08:43:50.700',N'Modifier',N'Orden de servicio correctivo',N'SMS.ServiceOrderHead.ServiceOrder',14,1,0,N'1')
INSERT [SMS].[ServiceOrderType]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[MaintenanceOrder],[ModifyDate],[ModifyUser],[Name],[NumberingSequence],[ServiceOrderType],[ShowInMobileClient],[SortOrder],[Value])
VALUES(N'#607D8B','2021-07-13T08:43:50.700',N'Creater',0,1,'es',0,'2021-07-13T08:43:50.700',N'Modifier',N'Orden de plantilla',N'SMS.ServiceOrderHead.ServiceOrder',15,1,0,N'2')
SET IDENTITY_INSERT [SMS].[ServiceOrderType] OFF

SET IDENTITY_INSERT [SMS].[ServicePriority] ON
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#03A9F4','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'en','2019-02-07T08:45:05.230',N'Setup',N'Low',1,2,N'0')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#03A9F4','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'de','2019-02-07T08:45:05.230',N'Setup',N'Niedrig',2,2,N'0')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#FF9800','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'en','2019-02-07T08:45:20.350',N'Setup',N'Middle',3,1,N'1')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#FF9800','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'de','2019-02-07T08:45:20.350',N'Setup',N'Mittel',4,1,N'1')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#F44336','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'en','2019-02-07T08:45:33.067',N'Setup',N'High',5,0,N'2')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#F44336','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'de','2019-02-07T08:45:33.067',N'Setup',N'Hoch',6,0,N'2')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#03A9F4','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'hu','2019-02-07T08:45:05.230',N'Setup',N'##hu: ServicePriority_0',7,2,N'0')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#FF9800','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'hu','2019-02-07T08:45:20.353',N'Setup',N'##hu: ServicePriority_1',8,1,N'1')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#F44336','2018-05-24T15:17:45.857',N'Migration_20170523083200',0,1,0,'hu','2019-02-07T08:45:33.067',N'Setup',N'##hu: ServicePriority_2',9,0,N'2')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#03A9F4','2019-02-07T08:45:05.003',N'Setup',0,1,0,'fr','2019-02-07T08:45:05.003',N'Setup',N'Faible',10,2,N'0')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#FF9800','2019-02-07T08:45:20.130',N'Setup',0,1,0,'fr','2019-02-07T08:45:20.130',N'Setup',N'Moyen',11,1,N'1')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#F44336','2019-02-07T08:45:32.847',N'Setup',0,1,0,'fr','2019-02-07T08:45:32.847',N'Setup',N'Haut',12,0,N'2')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#03A9F4','2021-07-13T08:43:50.690',N'Creater',0,1,0,'es','2021-07-13T08:43:50.690',N'Modifier',N'Baja',13,2,N'0')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#FF9800','2021-07-13T08:43:50.690',N'Creater',0,1,0,'es','2021-07-13T08:43:50.690',N'Modifier',N'Media',14,1,N'1')
INSERT [SMS].[ServicePriority]([Color],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsFastLane],[Language],[ModifyDate],[ModifyUser],[Name],[ServiceNotificationPriorityId],[SortOrder],[Value])
VALUES(N'#F44336','2021-07-13T08:43:50.690',N'Creater',0,1,0,'es','2021-07-13T08:43:50.690',N'Modifier',N'Alta',15,0,N'2')
SET IDENTITY_INSERT [SMS].[ServicePriority] OFF

SET IDENTITY_INSERT [SMS].[TimeEntryType] ON
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'de','2014-06-24T11:34:08.400',N'Setup',N'Tanken',1,0,0,1,null,N'100')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'en','2014-06-24T11:34:08.400',N'Setup',N'Refueling',1,0,0,2,null,N'100')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'de','2014-06-24T11:34:08.400',N'Setup',N'Reisezeit',1,0,0,3,null,N'101')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'en','2014-06-24T11:34:08.400',N'Setup',N'Travelling',1,0,0,4,null,N'101')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'de','2014-06-24T11:34:08.400',N'Setup',N'Verkaufsunterstützung',1,0,0,5,null,N'102')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'en','2014-06-24T11:34:08.400',N'Setup',N'Sales support',1,0,0,6,null,N'102')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'de','2014-06-24T11:34:08.400',N'Setup',N'Standzeit (Büro, Post, Lager, Auto)',1,0,0,7,null,N'103')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'en','2014-06-24T11:34:08.400',N'Setup',N'Sparetime (Office, Shipments, Stock, Car)',1,0,0,8,null,N'103')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'de','2014-06-24T11:34:08.400',N'Setup',N'Rüstzeit',1,0,0,9,null,N'104')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'en','2014-06-24T11:34:08.400',N'Setup',N'Set-Up time',1,0,0,10,null,N'104')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'de','2014-06-24T11:34:08.400',N'Setup',N'Sonstige',1,0,0,11,null,N'105')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2014-06-24T11:34:08.383',N'Setup',0,null,0,1,N'en','2014-06-24T11:34:08.400',N'Setup',N'Other',1,0,0,12,null,N'105')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2017-03-20T15:54:16',N'LookupManager',0,null,0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'##hu: TimeEntryType_100',1,0,0,13,null,N'100')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2017-03-20T15:54:16',N'LookupManager',0,null,0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'##hu: TimeEntryType_101',1,0,0,14,null,N'101')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2017-03-20T15:54:16',N'LookupManager',0,null,0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'##hu: TimeEntryType_102',1,0,0,15,null,N'102')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2017-03-20T15:54:16',N'LookupManager',0,null,0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'##hu: TimeEntryType_103',1,0,0,16,null,N'103')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2017-03-20T15:54:16',N'LookupManager',0,null,0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'##hu: TimeEntryType_104',1,0,0,17,null,N'104')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2017-03-20T15:54:16',N'LookupManager',0,null,0,1,N'hu','2017-03-20T15:54:16',N'LookupManager',N'##hu: TimeEntryType_105',1,0,0,18,null,N'105')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#CDDC39','2019-03-14T16:42:34.237',N'default.1',1,null,0,1,N'de','2019-03-14T16:43:44.293',N'default.1',N'Urlaub',0,1,0,19,null,N'106')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#CDDC39','2019-03-14T16:43:44.270',N'default.1',1,null,0,1,N'en','2019-03-14T16:43:44.270',N'default.1',N'Vacation',0,1,0,20,null,N'106')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#CDDC39','2019-03-14T16:43:44.280',N'default.1',1,null,0,1,N'fr','2019-03-14T16:43:44.280',N'default.1',N'Vacances',0,1,0,21,null,N'106')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#CDDC39','2019-03-14T16:43:44.283',N'default.1',1,null,0,1,N'hu','2019-03-14T16:43:44.283',N'default.1',N'Ünnep',0,1,0,22,null,N'106')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2020-03-18T16:54:44.283',N'default.1',0,null,0,1,N'fr','2020-03-18T16:54:44.283',N'default.1',N'Ravitaillement',1,0,0,23,null,N'100')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2020-03-18T16:54:44.283',N'default.1',0,null,0,1,N'fr','2020-03-18T16:54:44.283',N'default.1',N'voyageant',1,0,0,24,null,N'101')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2020-03-18T16:54:44.283',N'default.1',0,null,0,1,N'fr','2020-03-18T16:54:44.283',N'default.1',N'soutien aux ventes',1,0,0,25,null,N'102')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2020-03-18T16:54:44.283',N'default.1',0,null,0,1,N'fr','2020-03-18T16:54:44.283',N'default.1',N'Sparetime (bureau, expéditions, stock, voiture)',1,0,0,26,null,N'103')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2020-03-18T16:54:44.283',N'default.1',0,null,0,1,N'fr','2020-03-18T16:54:44.283',N'default.1',N'Temps d''installation',1,0,0,27,null,N'104')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2020-03-18T16:54:44.283',N'default.1',0,null,0,1,N'fr','2020-03-18T16:54:44.283',N'default.1',N'Autre',1,0,0,28,null,N'105')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2021-07-13T08:43:50.800',N'Creater',0,null,0,1,N'es','2021-07-13T08:43:50.800',N'Modifier',N'Repostaje',1,0,0,29,null,N'100')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2021-07-13T08:43:50.800',N'Creater',0,null,0,1,N'es','2021-07-13T08:43:50.800',N'Modifier',N'Viaje',1,0,0,30,null,N'101')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2021-07-13T08:43:50.800',N'Creater',0,null,0,1,N'es','2021-07-13T08:43:50.800',N'Modifier',N'Soporte de ventas',1,0,0,31,null,N'102')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2021-07-13T08:43:50.800',N'Creater',0,null,0,1,N'es','2021-07-13T08:43:50.800',N'Modifier',N'Tiempo libre (oficina, envíos, stock, coche)',1,0,0,32,null,N'103')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2021-07-13T08:43:50.800',N'Creater',0,null,0,1,N'es','2021-07-13T08:43:50.800',N'Modifier',N'Tiempo de preparación',1,0,0,33,null,N'104')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#AAAAAA','2021-07-13T08:43:50.800',N'Creater',0,null,0,1,N'es','2021-07-13T08:43:50.800',N'Modifier',N'Otro',1,0,0,34,null,N'105')
INSERT [SMS].[TimeEntryType]([Color],[CreateDate],[CreateUser],[DecreasesCapacity],[DefaultDurationInMinutes],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ShowInMobileClient],[ShowInScheduler],[SortOrder],[TimeEntryTypeId],[ValidCostCenters],[Value])
VALUES(N'#CDDC39','2021-07-13T08:43:50.800',N'Creater',1,null,0,1,N'es','2021-07-13T08:43:50.800',N'Modifier',N'Vacaciones',0,1,0,35,null,N'106')
SET IDENTITY_INSERT [SMS].[TimeEntryType] OFF
");
		}
	}
}
