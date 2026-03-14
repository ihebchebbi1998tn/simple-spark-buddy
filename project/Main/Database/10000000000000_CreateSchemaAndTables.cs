namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(10000000000000)]
	public class CreateSchemaAndTables : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [dbo].[SchemaInfo_Main]") > 0)
			{
				return;
			}
			Database.ExecuteNonQuery("REVOKE CONNECT TO [guest]");
			Database.ExecuteNonQuery("CREATE SCHEMA [CRM] AUTHORIZATION [dbo]");
			Database.ExecuteNonQuery("CREATE SCHEMA [LU] AUTHORIZATION [dbo]");
			Database.ExecuteNonQuery("CREATE SCHEMA [RPL] AUTHORIZATION [dbo]");
			Database.ExecuteNonQuery("CREATE SCHEMA [SMS] AUTHORIZATION [dbo]");

			Database.ExecuteNonQuery(@"
CREATE TABLE [CRM].[Log](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[Date] [datetime] NOT NULL,
	[Thread] [nvarchar] (512) COLLATE Latin1_General_CI_AS NULL,
	[Context] [varchar] (512) COLLATE Latin1_General_CI_AS NULL,
	[Level] [varchar] (512) COLLATE Latin1_General_CI_AS NOT NULL,
	[Logger] [varchar] (512) COLLATE Latin1_General_CI_AS NOT NULL,
	[Message] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL,
	[Exception] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Log__ModifyDate__07EC11B9] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Log__CreateUser__08E035F2] DEFAULT ('System'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Log__ModifyUser__09D45A2B] DEFAULT ('System'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Log__IsActive__0AC87E64] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [CRM].[Log] ADD CONSTRAINT [PK_Log] PRIMARY KEY
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
		,FILLFACTOR = 90
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Log_Date] ON [CRM].[Log]
(
	[Date] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Log_Level] ON [CRM].[Log]
(
	[Level] ASC
)WITH (FILLFACTOR=90)ON [PRIMARY]

CREATE TABLE [CRM].[Message](
	[MessageIdOld] [int] NOT NULL IDENTITY (1,1),
	[Recipients] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL,
	[Subject] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Body] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL,
	[State] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Message_CreateDate] DEFAULT (getdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_Message_ModifyDate] DEFAULT (getdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Message_CreateUser] DEFAULT (''),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Message_ModifyUser] DEFAULT (''),
	[MessageId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Message_MessageId] DEFAULT (newsequentialid()),
	[Bcc] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[ErrorMessage] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[IsBodyHtml] [bit] NOT NULL CONSTRAINT [DF__Message__IsBodyH__491AB698] DEFAULT ((0)),
	[From] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]

ALTER TABLE [CRM].[Message] ADD CONSTRAINT [PK_Message] PRIMARY KEY
	CLUSTERED
	(
		[MessageId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [CRM].[Posting](
	[PostingId] [int] NOT NULL IDENTITY (1,1),
	[State] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
	[StateDetails] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NULL,
	[Type] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[EntityTypeName] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL,
	[SerializedEntity] [nvarchar] (MAX) COLLATE Latin1_General_CI_AS NOT NULL,
	[EntityId] [nvarchar] (36) COLLATE Latin1_General_CI_AS NULL,
	[TransactionId] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[RetryAfter] [datetime] NULL,
	[Retries] [int] NOT NULL CONSTRAINT [DF__Posting__Retries__1F8E9120] DEFAULT ((0)),
	[Category] [nvarchar] (30) NOT NULL CONSTRAINT [DF__Posting__Categor__2878DCDC] DEFAULT ('Posting'),
	[RuleKey] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [CRM].[Posting] ADD CONSTRAINT [PK_Posting] PRIMARY KEY
	CLUSTERED
	(
		[PostingId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
		,FILLFACTOR = 90
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Posting_State] ON [CRM].[Posting]
(
	[State] ASC
)ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Posting_Transaction] ON [CRM].[Posting]
(
	[TransactionId] ASC
)
INCLUDE
(
	[State]
)ON [PRIMARY]

CREATE TABLE [CRM].[User](
	[UserID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_User_UserID] DEFAULT (newsequentialid()),
	[Username] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Email] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Password] [nvarchar] (128) COLLATE Latin1_General_CI_AS NULL,
	[DefaultLanguage] [nvarchar] (5) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_User_CreateDate] DEFAULT (getdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_User_ModifyDate] DEFAULT (getdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[LastLoginDate] [datetime] NULL,
	[PersonnelId] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[DischargeDate] [datetime] NULL,
	[GeneralToken] [nchar] (15) COLLATE Latin1_General_CI_AS NULL,
	[DropboxToken] [nchar] (15) COLLATE Latin1_General_CI_AS NULL,
	[Latitude] [float] (53) NULL,
	[Longitude] [float] (53) NULL,
	[StatusKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[StatusMessage] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[LastStatusUpdate] [datetime] NULL,
	[TimeZoneName] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
	[FirstName] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
	[LastName] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
	[Remark] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[MasterRecordNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[IdentificationNo] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[TravelTimeToBranchInMinutes] [int] NULL,
	[Adname] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[AppleDeviceToken] [nvarchar] (64) COLLATE Latin1_General_CI_AS NULL,
	[DefaultStoreNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[DefaultLocationNo] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[LegacyId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[Discharged] [bit] NULL CONSTRAINT [DF__User__Discharged__64B7E415] DEFAULT ((0)),
	[Avatar] [varbinary] (MAX) NULL,
	[DefaultLocale] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[HomeAddressId] [uniqueidentifier] NULL,
	[PublicHolidayRegionKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[OpenidIdentifier] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[LicensedAt] [datetime] NULL
) ON [PRIMARY]

ALTER TABLE [CRM].[User] ADD CONSTRAINT [PK_User] PRIMARY KEY
	CLUSTERED
	(
		[UserID] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [CRM].[User] ADD CONSTRAINT [IX_Username] UNIQUE
	NONCLUSTERED
	(
		[Username] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_User_Discharged] ON [CRM].[User]
(
	[Discharged] ASC
)ON [PRIMARY]

CREATE TABLE [CRM].[Usergroup](
	[Name] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL,
	[LegacyId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[LegacyVersion] [bigint] NULL,
	[MainResourceId] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Usergroup__Creat__60132A89] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Usergroup__Modif__61074EC2] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Usergroup__Creat__61FB72FB] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Usergroup__Modif__62EF9734] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__Usergroup__IsAct__63E3BB6D] DEFAULT ((1)),
	[UserGroupIdOld] [int] NULL,
	[UsergroupId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__Usergroup__Userg__59662CFA] DEFAULT (newsequentialid())
) ON [PRIMARY]

ALTER TABLE [CRM].[Usergroup] ADD CONSTRAINT [PK_Usergroup] PRIMARY KEY
	CLUSTERED
	(
		[UsergroupId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [CRM].[UserSkill](
	[Username] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[SkillKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]

ALTER TABLE [CRM].[UserSkill] ADD CONSTRAINT [PK_UserSkill_1] PRIMARY KEY
	CLUSTERED
	(
		[Username] ASC
		,[SkillKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [CRM].[UserUserGroup](
	[UserGroupKeyOld] [int] NULL,
	[Username] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__UserUserG__Usern__43C1049E] DEFAULT ('username'),
	[UsergroupKey] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

ALTER TABLE [CRM].[UserUserGroup] ADD CONSTRAINT [PK_UserUserGroup] PRIMARY KEY
	CLUSTERED
	(
		[Username] ASC
		,[UsergroupKey] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [CRM].[UserUserGroup] ADD CONSTRAINT [FK_UserUserGroup_UserGroup] FOREIGN KEY
	(
		[UsergroupKey]
	)
	REFERENCES [CRM].[Usergroup]
	(
		[UsergroupId]
	) ON DELETE CASCADE ON UPDATE CASCADE

CREATE TABLE [dbo].[Domain](
	[UId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__Domain__UId__4E1475DF] DEFAULT (newsequentialid()),
	[Version] [bigint] NOT NULL CONSTRAINT [DF__Domain__Version__4F089A18] DEFAULT ((1)),
	[CreatedBy] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__Domain__CreatedB__4FFCBE51] DEFAULT ('System'),
	[CreatedAt] [datetime] NOT NULL CONSTRAINT [DF__Domain__CreatedA__50F0E28A] DEFAULT (getutcdate()),
	[ModifiedBy] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__Domain__Modified__51E506C3] DEFAULT ('System'),
	[ModifiedAt] [datetime] NOT NULL CONSTRAINT [DF__Domain__Modified__52D92AFC] DEFAULT (getutcdate()),
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF__Domain__IsDelete__53CD4F35] DEFAULT ((0)),
	[DeletedAt] [datetime] NULL,
	[Name] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Type] [int] NOT NULL CONSTRAINT [DF__Domain__Type__54C1736E] DEFAULT ((1)),
	[MasterId] [uniqueidentifier] NULL,
	[LegacyId] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[LegacyVersion] [bigint] NULL,
	[TenantKeyOld] [int] NULL,
	[DefaultLanguageKey] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
	[DefaultLocale] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[Host] [nvarchar] (100) COLLATE Latin1_General_CI_AS NULL,
	[MaterialLogo] [image] NULL,
	[MaterialTheme] [nvarchar] (30) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Domain__Material__0B1D841F] DEFAULT ('bluegray'),
	[ReportLogo] [image] NULL,
	[SiteLogo] [image] NULL,
	[Theme] [nvarchar] (30) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Domain__Theme__0C11A858] DEFAULT ('standard'),
	[CompanyName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Domain__CompanyN__5402595F] DEFAULT ('L-mobile solutions GmbH & Co. KG'),
	[ResponsibleAddress] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Domain__Responsi__54F67D98] DEFAULT ('L-mobile solutions GmbH & Co. KG, Im Horben 7, D-71560 Sulzbach/Murr, E-Mail: support@l-mobile.com'),
	[DataProtectionOfficer] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__Domain__DataProt__55EAA1D1] DEFAULT ('L-mobile solutions GmbH & Co. KG, Im Horben 7, D-71560 Sulzbach/Murr, E-Mail: support@l-mobile.com'),
	[ReportFooterCol1] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[ReportFooterCol2] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[ReportFooterCol3] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
	[ReportLogoContentType] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[ReportFooterImage] [image] NULL,
	[ReportFooterImageContentType] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[ContractNo] [nvarchar] (255) NULL,
	[ProjectId] [uniqueidentifier] NULL,
	[DefaultCountryKey] [nvarchar] (255) NULL CONSTRAINT [DF__Domain__DefaultC__32024716] DEFAULT ('100'),
	[LicenseKey] [nvarchar] (4000) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[Domain] ADD CONSTRAINT [PK__Domain__C5B19662841DBF3C] PRIMARY KEY
	CLUSTERED
	(
		[UId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [dbo].[Domain] ADD CONSTRAINT [FK_Domain_MasterId] FOREIGN KEY
	(
		[MasterId]
	)
	REFERENCES [dbo].[Domain]
	(
		[UId]
	) 

CREATE TABLE [dbo].[EntityType](
	[UId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__EntityType__UId__64F7DB37] DEFAULT (newsequentialid()),
	[Version] [bigint] NOT NULL CONSTRAINT [DF__EntityTyp__Versi__65EBFF70] DEFAULT ((1)),
	[CreatedBy] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__EntityTyp__Creat__66E023A9] DEFAULT ('System'),
	[CreatedAt] [datetime] NOT NULL CONSTRAINT [DF__EntityTyp__Creat__67D447E2] DEFAULT (getutcdate()),
	[ModifiedBy] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__EntityTyp__Modif__68C86C1B] DEFAULT ('System'),
	[ModifiedAt] [datetime] NOT NULL CONSTRAINT [DF__EntityTyp__Modif__69BC9054] DEFAULT (getutcdate()),
	[IsDeleted] [bit] NULL CONSTRAINT [DF__EntityTyp__IsDel__6AB0B48D] DEFAULT ((0)),
	[DeletedAt] [datetime] NULL,
	[Name] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[EntityType] ADD CONSTRAINT [PK__EntityTy__C5B196629355193A] PRIMARY KEY
	CLUSTERED
	(
		[UId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [IX_UQ_ActiveEntityType] ON [dbo].[EntityType]
(
	[Name] ASC,
	[IsDeleted] ASC
)
WHERE ([IsDeleted]=(0))
ON [PRIMARY]

CREATE TABLE [dbo].[hibernate_unique_key_old](
	[next_hi] [int] NOT NULL,
	[tablename] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[NumberingSequence](
	[SequenceName] [nvarchar] (128) COLLATE Latin1_General_CI_AS NOT NULL,
	[LastNumber] [bigint] NOT NULL,
	[Prefix] [nvarchar] (32) COLLATE Latin1_General_CI_AS NULL,
	[Format] [nvarchar] (32) COLLATE Latin1_General_CI_AS NULL,
	[Suffix] [nvarchar] (32) COLLATE Latin1_General_CI_AS NULL,
	[MaxLow] [bigint] NULL CONSTRAINT [DF__Numbering__MaxLo__2B4A5C8F] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [dbo].[NumberingSequence] ADD CONSTRAINT [PK_NumberingSequence] PRIMARY KEY
	CLUSTERED
	(
		[SequenceName] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[CommunicationType](
	[CommTypeDetailId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[CommTypeGroupKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CommTypeDetail_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CommTypeDetail_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Communica__Favor__27C3E46E] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Communica__SortO__28B808A7] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [LU].[CommunicationType] ADD CONSTRAINT [PK_CommTypeDetail] PRIMARY KEY
	CLUSTERED
	(
		[CommTypeDetailId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[CostCenter](
	[CostCenterId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUCostCenter_CreateDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCostCenter_CreateUser] DEFAULT ('Creater'),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUCostCenter_ModifyDate] DEFAULT (getutcdate()),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCostCenter_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUCostCenter_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[CostCenter] ADD CONSTRAINT [PK__CostCent__89D876F14E0988E7] PRIMARY KEY
	CLUSTERED
	(
		[CostCenterId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[Country](
	[CountryId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Country_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Country_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Country__Favorit__23BE4960] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Country__SortOrd__24B26D99] DEFAULT ((0)),
	[Iso2Code] [nvarchar] (2) COLLATE Latin1_General_CI_AS NULL,
	[Iso3Code] [nvarchar] (3) COLLATE Latin1_General_CI_AS NULL,
	[CallingCode] [nvarchar] (5) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUCountry_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUCountry_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCountry_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCountry_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUCountry_IsActive] DEFAULT ((1)),
	[IsGreat] [bit] NOT NULL CONSTRAINT [DF__Country__IsGreat__4C9641C1] DEFAULT ((1)),
	[LegacyId] [uniqueidentifier] NULL,
	[Rating] [decimal] (19,5) NULL
) ON [PRIMARY]

ALTER TABLE [LU].[Country] ADD CONSTRAINT [PK_Country] PRIMARY KEY
	CLUSTERED
	(
		[CountryId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[Currency](
	[ProjectCategoriyId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Currency_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Currency_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Currency__Favori__2EA5EC27] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Currency__SortOr__2F9A1060] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUCurrency_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUCurrency_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCurrency_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUCurrency_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUCurrency_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[Currency] ADD CONSTRAINT [PK_Currency] PRIMARY KEY
	CLUSTERED
	(
		[ProjectCategoriyId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[InvoicingType](
	[InvoicingTypeId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUInvoicingType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUInvoicingType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUInvoicingType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUInvoicingType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUInvoicingType_IsActive] DEFAULT ((1)),
	[IsCostLumpSum] [bit] NOT NULL CONSTRAINT [DF__Invoicing__IsCos__137DBFF6] DEFAULT ((0)),
	[IsMaterialLumpSum] [bit] NOT NULL CONSTRAINT [DF__Invoicing__IsMat__1471E42F] DEFAULT ((0)),
	[IsTimeLumpSum] [bit] NOT NULL CONSTRAINT [DF__Invoicing__IsTim__15660868] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [LU].[InvoicingType] ADD CONSTRAINT [PK__Invoicin__D0C282900E240DFC] PRIMARY KEY
	CLUSTERED
	(
		[InvoicingTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[Language](
	[LangId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Lang_Language] DEFAULT ('en'),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Lang_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Language__Favori__50C5FA01] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Language__SortOr__51BA1E3A] DEFAULT ((0)),
	[IsSystemLanguage] [bit] NOT NULL CONSTRAINT [DF__Language__IsSyst__52AE4273] DEFAULT ('false'),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LULanguage_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LULanguage_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LULanguage_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LULanguage_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LULanguage_IsActive] DEFAULT ((1)),
	[DefaultLocale] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__Language__Defaul__52A420D2] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [LU].[Language] ADD CONSTRAINT [PK_Lang] PRIMARY KEY
	CLUSTERED
	(
		[LangId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[LengthUnit](
	[LengthUnitId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LULengthUnit_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LULengthUnit_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LULengthUnit_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LULengthUnit_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LULengthUnit_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[LengthUnit] ADD CONSTRAINT [PK_LengthUnit] PRIMARY KEY
	CLUSTERED
	(
		[LengthUnitId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[PaymentCondition](
	[PaymentConditionId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUPaymentCondition_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUPaymentCondition_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUPaymentCondition_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUPaymentCondition_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUPaymentCondition_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[PaymentCondition] ADD CONSTRAINT [PK__PaymentC__19C41B177093AB15] PRIMARY KEY
	CLUSTERED
	(
		[PaymentConditionId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[PaymentInterval](
	[PaymentIntervalId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUPaymentInterval_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUPaymentInterval_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUPaymentInterval_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUPaymentInterval_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUPaymentInterval_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[PaymentInterval] ADD CONSTRAINT [PK__PaymentI__94E44A506CC31A31] PRIMARY KEY
	CLUSTERED
	(
		[PaymentIntervalId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[PaymentType](
	[PaymentTypeId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUPaymentType_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUPaymentType_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUPaymentType_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUPaymentType_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUPaymentType_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[PaymentType] ADD CONSTRAINT [PK__PaymentT__BA430B3568F2894D] PRIMARY KEY
	CLUSTERED
	(
		[PaymentTypeId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[Region](
	[RegionId] [int] NOT NULL IDENTITY (1,1),
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Region_Language] DEFAULT ('en'),
	[Name] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Region_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Region__Favorite__05D8E0BE] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Region__SortOrde__06CD04F7] DEFAULT ((0)),
	[CountryKey] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LURegion_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LURegion_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LURegion_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LURegion_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LURegion_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[Region] ADD CONSTRAINT [PK_Region] PRIMARY KEY
	CLUSTERED
	(
		[RegionId] ASC
		,[Language] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[TimeUnit](
	[TimeUnitId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[TimeUnitsPerYear] [int] NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUTimeUnit_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUTimeUnit_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUTimeUnit_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUTimeUnit_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUTimeUnit_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[TimeUnit] ADD CONSTRAINT [PK_TimeUnit] PRIMARY KEY
	CLUSTERED
	(
		[TimeUnitId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[UserStatus](
	[UserStatusId] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (60) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUUserStatus_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUUserStatus_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUUserStatus_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUUserStatus_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUUserStatus_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [LU].[UserStatus] ADD CONSTRAINT [PK__UserStat__A33F543A1ABEEF0B] PRIMARY KEY
	CLUSTERED
	(
		[UserStatusId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [LU].[ZipCodeFilter](
	[Id] [int] NOT NULL IDENTITY (1,1),
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL,
	[Value] [nvarchar] (20) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[CountryKey] [nvarchar] (2) COLLATE Latin1_General_CI_AS NULL,
	[ZipCodeFrom] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[ZipCodeTo] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_LUZipCodeFilter_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_LUZipCodeFilter_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUZipCodeFilter_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_LUZipCodeFilter_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_LUZipCodeFilter_IsActive] DEFAULT ((1))
) ON [PRIMARY]

CREATE TABLE [SMS].[Skill](
	[SkillId] [int] NOT NULL IDENTITY (1,1) NOT FOR REPLICATION,
	[Name] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [char] (2) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_Skill_Language] DEFAULT ('en'),
	[Value] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Skill_Value] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__Skill__Favorite__1293BD5E] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__Skill__SortOrder__1387E197] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_SMSSkill_CreateDate] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_SMSSkill_ModifyDate] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSSkill_CreateUser] DEFAULT ('Creater'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SMSSkill_ModifyUser] DEFAULT ('Modifier'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SMSSkill_IsActive] DEFAULT ((1))
) ON [PRIMARY]

ALTER TABLE [SMS].[Skill] ADD CONSTRAINT [PK_Skill_1] PRIMARY KEY
	CLUSTERED
	(
		[SkillId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [SMS].[UserSkill](
	[UserSkillId] [int] NOT NULL IDENTITY (1,1),
	[UserName] [nvarchar] (32) COLLATE Latin1_General_CI_AS NOT NULL,
	[SkillId] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Favorite] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[ValidTo] [datetime] NULL
) ON [PRIMARY]

ALTER TABLE [SMS].[UserSkill] ADD CONSTRAINT [PK_UserSkill] PRIMARY KEY
	CLUSTERED
	(
		[UserSkillId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [CRM].[UserRecentPages](
	[Username] [varchar] (32) COLLATE Latin1_General_CI_AS NOT NULL,
	[Title] [varchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Url] [varchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Count] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ContactIdOld] [int] NULL,
	[CreateUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__UserRecen__Creat__24D2692A] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__UserRecen__Modif__25C68D63] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__UserRecen__IsAct__26BAB19C] DEFAULT ((1)),
	[ContactId] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF__UserRecentPa__Id__0BBCA29D] DEFAULT (newsequentialid()),
	[IsMaterial] [bit] NOT NULL CONSTRAINT [DF__UserRecen__IsMat__3F073C79] DEFAULT ((0)),
	[Category] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]

ALTER TABLE [CRM].[UserRecentPages] ADD CONSTRAINT [PK__UserRecentPages] PRIMARY KEY
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

CREATE NONCLUSTERED INDEX [IX_UserRecentPages_Username] ON [CRM].[UserRecentPages]
(
	[Username] ASC
)
INCLUDE
(
	[ModifyDate],
	[Title],
	[Url]
)ON [PRIMARY]

CREATE TABLE [LU].[DocumentCategory](
	[DocumentCategoryId] [int] NOT NULL IDENTITY (1,1),
	[Value] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Language] [nvarchar] (2) COLLATE Latin1_General_CI_AS NOT NULL,
	[OfflineRelevant] [bit] NOT NULL CONSTRAINT [DF__DocumentC__Offli__06C2E356] DEFAULT ((0)),
	[Favorite] [bit] NOT NULL CONSTRAINT [DF__DocumentC__Favor__07B7078F] DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF__DocumentC__SortO__08AB2BC8] DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__DocumentC__Creat__099F5001] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__DocumentC__Modif__0A93743A] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__DocumentC__Creat__0B879873] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF__DocumentC__Modif__0C7BBCAC] DEFAULT ('Setup'),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__DocumentC__IsAct__0D6FE0E5] DEFAULT ((1)),
	[SalesRelated] [bit] NOT NULL CONSTRAINT [DF__DocumentC__Sales__29A20B3F] DEFAULT ((0))
) ON [PRIMARY]

ALTER TABLE [LU].[DocumentCategory] ADD CONSTRAINT [PK__Document__3996B1FAAF7E754F] PRIMARY KEY
	CLUSTERED
	(
		[DocumentCategoryId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [dbo].[EntityAuthData](
	[UId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__EntityAuthD__UId__5C4299A5] DEFAULT (newsequentialid()),
	[Version] [bigint] NOT NULL CONSTRAINT [DF__EntityAut__Versi__5D36BDDE] DEFAULT ((1)),
	[CreatedBy] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__EntityAut__Creat__5E2AE217] DEFAULT ('System'),
	[CreatedAt] [datetime] NOT NULL CONSTRAINT [DF__EntityAut__Creat__5F1F0650] DEFAULT (getutcdate()),
	[ModifiedBy] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF__EntityAut__Modif__60132A89] DEFAULT ('System'),
	[ModifiedAt] [datetime] NOT NULL CONSTRAINT [DF__EntityAut__Modif__61074EC2] DEFAULT (getutcdate()),
	[EntityId] [uniqueidentifier] NULL,
	[EntityTypeId] [uniqueidentifier] NOT NULL,
	[DomainId] [uniqueidentifier] NOT NULL,
	[Circle] [uniqueidentifier] NULL CONSTRAINT [DF__EntityAut__Circl__61FB72FB] DEFAULT ('00000000-0000-0000-0000-000000000000'),
	[CreatorUserId] [uniqueidentifier] NULL,
	[OwnerUserId] [uniqueidentifier] NULL,
	[SuperAuthDataId] [uniqueidentifier] NULL,
	[DomainType] [int] NOT NULL CONSTRAINT [DF__EntityAut__Domai__62EF9734] DEFAULT ((1)),
	[SuperEntityTypeId] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[EntityAuthData] ADD CONSTRAINT [PK__EntityAu__C5B19662314D44C6] PRIMARY KEY
	CLUSTERED
	(
		[UId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

ALTER TABLE [dbo].[EntityAuthData] ADD CONSTRAINT [FK_EntityAuthData_Domain] FOREIGN KEY
	(
		[DomainId]
	)
	REFERENCES [dbo].[Domain]
	(
		[UId]
	) 

ALTER TABLE [dbo].[EntityAuthData] ADD CONSTRAINT [FK_EntityAuthData_EntityType] FOREIGN KEY
	(
		[EntityTypeId]
	)
	REFERENCES [dbo].[EntityType]
	(
		[UId]
	) 

ALTER TABLE [dbo].[EntityAuthData] ADD CONSTRAINT [FK_EntityAuthData_SuperEntityAuthData] FOREIGN KEY
	(
		[SuperAuthDataId]
	)
	REFERENCES [dbo].[EntityAuthData]
	(
		[UId]
	) 

ALTER TABLE [dbo].[EntityAuthData] ADD CONSTRAINT [FK_EntityAuthData_SuperEntityType] FOREIGN KEY
	(
		[SuperEntityTypeId]
	)
	REFERENCES [dbo].[EntityType]
	(
		[UId]
	) 

CREATE TABLE [CRM].[Device](
	[DeviceId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__Device__DeviceId__62A57E71] DEFAULT (newsequentialid()),
	[Fingerprint] [nvarchar] (255) NOT NULL,
	[Token] [nvarchar] (255) NULL,
	[Username] [nvarchar] (255) NOT NULL,
	[DeviceInfo] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
	[IsTrusted] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__Device__CreateDa__648DC6E3] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__Device__ModifyDa__6581EB1C] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) NOT NULL CONSTRAINT [DF__Device__CreateUs__66760F55] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) NOT NULL CONSTRAINT [DF__Device__ModifyUs__676A338E] DEFAULT ('Setup')
) ON [PRIMARY]

ALTER TABLE [CRM].[Device] ADD CONSTRAINT [PK__Device__49E123114C51927B] PRIMARY KEY
	CLUSTERED
	(
		[DeviceId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]

CREATE TABLE [CRM].[UserSubscription](
	[UserSubscriptionId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__UserSubsc__UserS__6D230CE4] DEFAULT (newsequentialid()),
	[Username] [nvarchar] (255) NOT NULL,
	[EntityType] [nvarchar] (255) NOT NULL,
	[EntityKey] [uniqueidentifier] NOT NULL,
	[IsSubscribed] [bit] NOT NULL CONSTRAINT [DF__UserSubsc__IsSub__6E17311D] DEFAULT ((1)),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__UserSubsc__IsAct__6F0B5556] DEFAULT ((1)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF__UserSubsc__Creat__6FFF798F] DEFAULT (getutcdate()),
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF__UserSubsc__Modif__70F39DC8] DEFAULT (getutcdate()),
	[CreateUser] [nvarchar] (255) NOT NULL CONSTRAINT [DF__UserSubsc__Creat__71E7C201] DEFAULT ('Setup'),
	[ModifyUser] [nvarchar] (255) NOT NULL CONSTRAINT [DF__UserSubsc__Modif__72DBE63A] DEFAULT ('Setup'),
	[AuthDataId] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [CRM].[UserSubscription] ADD CONSTRAINT [PK__UserSubs__D1FD777C40F848B2] PRIMARY KEY
	CLUSTERED
	(
		[UserSubscriptionId] ASC
	)	WITH
	(
		PAD_INDEX = OFF
		,STATISTICS_NORECOMPUTE = OFF
		,IGNORE_DUP_KEY = OFF
		,ALLOW_ROW_LOCKS = ON
		,ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
");
			Database.ExecuteNonQuery(@"
CREATE PROCEDURE [dbo].[DropDefault]
@schemaname as nvarchar(255),
@tablename as nvarchar(255),
@columnname as nvarchar(255)
AS
BEGIN
SET NOCOUNT ON;
DECLARE @defname VARCHAR(100), @cmd VARCHAR(1000)
SET @defname = 
(SELECT name 
FROM sysobjects so JOIN sysconstraints sc
ON so.id = sc.constid 
WHERE object_name(so.parent_obj) = @tablename
AND so.xtype = 'D'
AND sc.colid = 
(SELECT colid FROM syscolumns 
WHERE id = object_id(@schemaname + '.' + @tablename) AND 
name = @columnname))
SET @cmd = 'ALTER TABLE ' + @schemaname + '.' + @tablename + ' DROP CONSTRAINT ' + @defname
EXEC(@cmd)
END
");
			Database.ExecuteNonQuery(@"INSERT INTO [dbo].[SchemaInfo_Main] ([Version]) VALUES 
(20170519094900),
(20130624084202),
(20140218091100),
(20140218091101),
(20140220061800),
(20140220062900),
(20140227152800),
(20140305101200),
(20140305151800),
(20140305153600),
(20140310145300),
(20140313140950),
(20140602110400),
(20140602111400),
(20140602111600),
(20140602111700),
(20140602111800),
(20140611163200),
(20140909134200),
(20140909134500),
(20140909134900),
(20141117113000),
(20141127171900),
(20141204164800),
(20141204164801),
(20141204164802),
(20150413175000),
(20150413175300),
(20150413175301),
(20150413175900),
(20150413175901),
(20150413180600),
(20150413180900),
(20150413181600),
(20150413183100),
(20150413183500),
(20150413183800),
(20150413183801),
(20150414112600),
(20150414112601),
(20150414112602),
(20150414112603),
(20150414150800),
(20150522142800),
(20150724125000),
(20151110164200),
(20151111111111),
(20151112154700),
(20151112154800),
(20151112154801),
(20151112154900),
(20151112155000),
(20151119162500),
(20160108140300),
(20160317104000),
(20160404161200),
(20160404161900),
(20160404161901),
(20160404161902),
(20160404161903),
(20160404161904),
(20160404162900),
(20160404162901),
(20160414163000),
(20160414163001),
(20160414163002),
(20160414163003),
(20160414163004),
(20160414163005),
(20160509102500),
(20160517135500),
(20160518164700),
(20160518165100),
(20160518165600),
(20160518170000),
(20160622151803),
(20160622151900),
(20160622151903),
(20160629162500),
(20160629163300),
(20160701105500),
(20160701112500),
(20160714111100),
(20160714111101),
(20160714111102),
(20160714111103),
(20160714111104),
(20160714111105),
(20160714111106),
(20160714111107),
(20160714134400),
(20160805142400),
(20160809161600),
(20160819154100),
(20160823134100),
(20160824210000),
(20160825210000),
(20160901141000),
(20161005111100),
(20161206200000),
(20161221090000),
(20161221100000),
(20161221100500),
(20161221101000),
(20161221101500),
(20161221102000),
(20161221102500),
(20161221103000),
(20161230145700),
(20170102181700),
(20170102182500),
(20170112150000),
(20170124144400),
(20170124174300),
(20170125132100),
(20170130124900),
(20170130124901),
(20170202104400),
(20170202112200),
(20170206155300),
(20170309163000),
(20170309180000),
(20170309181500),
(20170316113700),
(20170316133200),
(20170331143000),
(20170331144500),
(20170519130300),
(20170519151500),
(20170519164800),
(20170523101900),
(20170524164800),
(20170621154600),
(20170621154700),
(20170627164600),
(20170717152300),
(20170724120000),
(20170825111100),
(20170828112100),
(20180109130000),
(20180112144500),
(20180202124300),
(20180208163900),
(20180213130000),
(20180219170000),
(20180219180000),
(20180220100001),
(20180220110000),
(20180305174700),
(20180306155800),
(20180327113500),
(20180327114000),
(20180327115300),
(20180327122800),
(20180327123300),
(20180327124700),
(20180327124900),
(20180327125200),
(20180327180000),
(20180411170000),
(20180411180099),
(20180411190000),
(20180517091400),
(20180606135500),
(20180625093000),
(20180625093500),
(20180625095000),
(20180629101400),
(20180716142000),
(20180802112400),
(20180820144300),
(20180903160000),
(20180907150000),
(20180918113100),
(20180924155300),
(20181010150000),
(20181013131000),
(20190116105700),
(20190312174500),
(20190321142200),
(20190514115000),
(20190704130700),
(20190704144700),
(20190920200000),
(20190920200001),
(20190924190000),
(20191025123000),
(20191113145200),
(20191119113300),
(20191221142100),
(20191230132800),
(20200127100500),
(20200226124500),
(20200304092300),
(20200326152700),
(20200415100000),
(20200415110000),
(20200513144500),
(20200608143000),
(20200617113500),
(20200617153900),
(20200618134000),
(20200715200000),
(20200810161500),
(20200904102500),
(20201009141300),
(20201013140300),
(20201013140500),
(20201015151000),
(20201209093800),
(20201222144600),
(20210115123600),
(20210215102400),
(20210215105000),
(20210217135300),
(20210217140700),
(20210217161400),
(20210217170200),
(20210308132200),
(20210310114500),
(20210420112500),
(20210422103700),
(20210504082700),
(20210511134900),
(20210511135500),
(20210519044700),
(20210526100000),
(20210608133000),
(20211109090600),
(20211110085000),
(20211110085200),
(20211117123800),
(20211206113100),
(20211217114000),
(20220119101000),
(20220121100000),
(20220121100100),
(20220301152000),
(20220301154500),
(20220325172600),
(20220614164300),
(20220714120000),
(20220726122000),
(20220812113000),
(20221005132000),
(20221005134500),
(20221005135200),
(20221005142800),
(20221123141200),
(20230201152100),
(20230202100100),
(20230208112500)
");
			Database.ExecuteNonQuery(@"
INSERT [dbo].[Domain]([CompanyName],[ContractNo],[CreatedAt],[CreatedBy],[DataProtectionOfficer],[DefaultCountryKey],[DefaultLanguageKey],[DefaultLocale],[DeletedAt],[Host],[IsDeleted],[LegacyId],[LegacyVersion],[LicenseKey],[MasterId],[MaterialLogo],[MaterialTheme],[ModifiedAt],[ModifiedBy],[Name],[ProjectId],[ReportFooterCol1],[ReportFooterCol2],[ReportFooterCol3],[ReportFooterImage],[ReportFooterImageContentType],[ReportLogo],[ReportLogoContentType],[ResponsibleAddress],[SiteLogo],[TenantKeyOld],[Theme],[Type],[UId],[Version])
VALUES(N'L-mobile solutions GmbH & Co. KG',null,'2011-10-20T13:06:07.533',N'Anonymous',N'L-mobile solutions GmbH & Co. KG, Im Horben 7, D-71560 Sulzbach/Murr, E-Mail: support@l-mobile.com',N'100',N'en',N'en-US-POSIX',null,N'http://localhost:4711/',0,null,null,null,null,null,N'bluegray','2021-12-08T08:13:37.530',N'default.1',N'stable',null,N'L-mobile solutions GmbH & Co. KG' + char(13) + char(10) + N'Im Horben 7' + char(13) + char(10) + N'D-71560 Sulzbach an der Murr',null,N'Registergericht: Amtsgericht Stuttgart' + char(13) + char(10) + N'Resiternummer: HRB 271080' + char(13) + char(10) + N'Umsatzsteuerident-Nr.: DE247616633',null,null,0x89504E470D0A1A0A0000000D49484452000000100000001008060000001FF3FF61000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001C49444154384F63E06AFBF59F123C6AC0A801203C6AC03030E0D77F00EFAC891F0104D6650000000049454E44AE426082,N'image/png',N'L-mobile solutions GmbH & Co. KG, Im Horben 7, D-71560 Sulzbach/Murr, E-Mail: support@l-mobile.com',0x89504E470D0A1A0A0000000D49484452000000100000001008060000001FF3FF61000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001C49444154384F63E06AFBF59F123C6AC0A801203C6AC03030E0D77F00EFAC891F0104D6650000000049454E44AE426082,null,N'standard',1,'00000000-0000-0000-0000-000000000000',1)

INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',1,32,N'CMP',N'CRM.Campaign',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',1,32,N'CO',N'CRM.Company',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'000000',12,32,N'OF',N'CRM.Offer',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'000000',10,32,N'OC',N'CRM.Order',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',1,32,N'P',N'CRM.Person',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'000000',2,32,N'POT-',N'CRM.Potential',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'000000',7,32,N'PRJ-',N'CRM.Project',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',1,32,N'Inst',N'SMS.Installation',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'000',100,null,null,N'SMS.OfflineUser.RemoteOrderId',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',415,32,null,N'SMS.ServiceCase',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',5,32,N'W',N'SMS.ServiceContract',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',1,32,N'OB',N'SMS.ServiceObject',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',1,32,N'D',N'SMS.ServiceOrderDispatch',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'000000',11,32,N'AdHoc-',N'SMS.ServiceOrderHead.AdHoc',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',0,32,N'M',N'SMS.ServiceOrderHead.MaintenanceOrder',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',17,32,N'S',N'SMS.ServiceOrderHead.ServiceOrder',null)
INSERT [dbo].[NumberingSequence]([Format],[LastNumber],[MaxLow],[Prefix],[SequenceName],[Suffix])
VALUES(N'00000',0,null,N'T',N'SMS.ServiceOrderTemplate',null)

SET IDENTITY_INSERT [LU].[CommunicationType] ON
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(99,N'Phone',0,N'de',N'Arbeit',1,N'PhoneWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(100,N'Phone',0,N'en',N'Work',1,N'PhoneWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(101,N'Phone',0,N'de',N'Mobil',2,N'PhoneMobile')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(102,N'Phone',0,N'en',N'Mobile',2,N'PhoneMobile')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(103,N'Phone',0,N'de',N'Pager',4,N'PhonePager')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(104,N'Phone',0,N'en',N'Pager',4,N'PhonePager')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(105,N'Phone',0,N'de',N'Privat',3,N'PhoneHome')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(106,N'Phone',0,N'en',N'Home',3,N'PhoneHome')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(107,N'Phone',0,N'de',N'Zentrale',0,N'PhoneFront')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(108,N'Phone',0,N'en',N'Front-Desk',0,N'PhoneFront')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(109,N'Phone',0,N'de',N'Andere',5,N'PhoneOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(110,N'Phone',0,N'en',N'Other',5,N'PhoneOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(111,N'Fax',0,N'de',N'Arbeit',0,N'FaxWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(112,N'Fax',0,N'en',N'Work',0,N'FaxWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(113,N'Fax',0,N'de',N'Privat',1,N'FaxHome')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(114,N'Fax',0,N'en',N'Home',1,N'FaxHome')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(115,N'Fax',0,N'de',N'Andere',2,N'FaxOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(116,N'Fax',0,N'en',N'Other',2,N'FaxOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(117,N'Email',0,N'de',N'Arbeit',0,N'EmailWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(118,N'Email',0,N'en',N'Work',0,N'EmailWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(119,N'Email',0,N'de',N'Privat',1,N'EmailHome')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(120,N'Email',0,N'en',N'Home',1,N'EmailHome')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(121,N'Email',0,N'de',N'Andere',2,N'EmailOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(122,N'Email',0,N'en',N'Other',2,N'EmailOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(123,N'Website',0,N'de',N'Arbeit',0,N'WebsiteWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(124,N'Website',0,N'en',N'Work',0,N'WebsiteWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(125,N'Website',0,N'de',N'Privat',1,N'WebsitePersonal')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(126,N'Website',0,N'en',N'Personal',1,N'WebsitePersonal')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(127,N'Website',0,N'de',N'Andere',2,N'WebsiteOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(128,N'Website',0,N'en',N'Other',2,N'WebsiteOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(129,N'Website',0,N'es',N'Otro',2,N'WebsiteOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(130,N'Website',0,N'es',N'Personal',1,N'WebsitePersonal')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(131,N'Website',0,N'es',N'Work',0,N'WebsiteWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(132,N'Email',0,N'es',N'Otro',2,N'EmailOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(133,N'Email',0,N'es',N'Personal',1,N'EmailPersonal')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(134,N'Email',0,N'es',N'Work',0,N'EmailWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(135,N'Fax',0,N'es',N'Otro',2,N'FaxOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(136,N'Fax',0,N'es',N'Personal',1,N'FaxPersonal')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(137,N'Fax',0,N'es',N'Work',0,N'FaxWork')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(138,N'Phone',0,N'es',N'Otro',5,N'PhoneOther')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(139,N'Phone',0,N'es',N'Recepción',0,N'PhoneFront')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(140,N'Phone',0,N'es',N'Fijo',3,N'PhoneHome')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(141,N'Phone',0,N'es',N'Localizador',4,N'PhonePager')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(142,N'Phone',0,N'es',N'Móvil',2,N'PhoneMobile')
INSERT [LU].[CommunicationType]([CommTypeDetailId],[CommTypeGroupKey],[Favorite],[Language],[Name],[SortOrder],[Value])
VALUES(143,N'Phone',0,N'es',N'Trabajo',1,N'PhoneWork')
SET IDENTITY_INSERT [LU].[CommunicationType] OFF

SET IDENTITY_INSERT [LU].[Country] ON
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'49',1435,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'DE',N'DEU',N'de',null,'2019-02-06T21:37:08.503',N'Setup',N'Deutschland',null,0,N'100')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'49',1436,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'DE',N'DEU',N'en',null,'2019-02-06T21:37:08.503',N'Setup',N'Germany',null,0,N'100')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'93',1437,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AF',N'AFG',N'de',null,'2019-02-07T14:01:00.973',N'Setup',N'Afghanistan',null,0,N'101')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'93',1438,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AF',N'AFG',N'en',null,'2019-02-07T14:01:00.973',N'Setup',N'Afghanistan',null,0,N'101')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'355',1439,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AL',N'ALB',N'de',null,'2019-02-07T14:00:41.630',N'Setup',N'Albanien',null,0,N'102')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'355',1440,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AL',N'ALB',N'en',null,'2019-02-07T14:00:41.630',N'Setup',N'Albania',null,0,N'102')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'213',1441,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DZ',N'DZA',N'de',null,'2019-02-07T14:00:19.380',N'Setup',N'Algerien',null,0,N'103')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'213',1442,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DZ',N'DZA',N'en',null,'2019-02-07T14:00:19.380',N'Setup',N'Algeria',null,0,N'103')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1684',1443,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AS',N'ASM',N'de',null,'2019-02-07T14:01:22.070',N'Setup',N'Amerikanisch-Samoa',null,0,N'104')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1684',1444,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AS',N'ASM',N'en',null,'2019-02-07T14:01:22.070',N'Setup',N'American Samoa',null,0,N'104')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'376',1445,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AD',N'AND',N'de',null,'2019-02-07T14:01:32.643',N'Setup',N'Andorra',null,0,N'105')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'376',1446,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AD',N'AND',N'en',null,'2019-02-07T14:01:32.643',N'Setup',N'Andorra',null,0,N'105')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'244',1447,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AO',N'AGO',N'de',null,'2019-02-07T14:01:41.327',N'Setup',N'Angola',null,0,N'106')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'244',1448,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AO',N'AGO',N'en',null,'2019-02-07T14:01:41.330',N'Setup',N'Angola',null,0,N'106')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1264',1449,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AI',N'AIA',N'de',null,'2019-02-07T14:01:52.347',N'Setup',N'Anguilla',null,0,N'107')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1264',1450,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AI',N'AIA',N'en',null,'2019-02-07T14:01:52.347',N'Setup',N'Anguilla',null,0,N'107')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6721',1451,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AQ',N'ATA',N'de',null,'2019-02-07T14:02:02.743',N'Setup',N'Antarktis',null,0,N'108')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6721',1452,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AQ',N'ATA',N'en',null,'2019-02-07T14:02:02.743',N'Setup',N'Antarctica',null,0,N'108')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1268',1453,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AG',N'ATG',N'de',null,'2019-02-07T14:02:16.113',N'Setup',N'Antigua und Barbuda',null,0,N'109')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1268',1454,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AG',N'ATG',N'en',null,'2019-02-07T14:02:16.117',N'Setup',N'Antigua and Barbuda',null,0,N'109')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'54',1455,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AR',N'ARG',N'de',null,'2019-02-07T14:02:26.070',N'Setup',N'Argentinien',null,0,N'110')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'54',1456,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AR',N'ARG',N'en',null,'2019-02-07T14:02:26.070',N'Setup',N'Argentina',null,0,N'110')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'374',1457,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AM',N'ARM',N'de',null,'2019-02-07T14:02:38.613',N'Setup',N'Armenien',null,0,N'111')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'374',1458,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AM',N'ARM',N'en',null,'2019-02-07T14:02:38.613',N'Setup',N'Armenia',null,0,N'111')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'297',1459,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AW',N'ABW',N'de',null,'2019-02-07T14:02:47.790',N'Setup',N'Aruba',null,0,N'112')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'297',1460,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AW',N'ABW',N'en',null,'2019-02-07T14:02:47.790',N'Setup',N'Aruba',null,0,N'112')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'61',1461,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AU',N'AUS',N'de',null,'2019-02-07T14:03:01.357',N'Setup',N'Australien',null,0,N'113')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'61',1462,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AU',N'AUS',N'en',null,'2019-02-07T14:03:01.357',N'Setup',N'Australia',null,0,N'113')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'43',1463,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'AT',N'AUT',N'de',null,'2019-02-07T14:03:12.050',N'Setup',N'Österreich',null,1,N'114')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'43',1464,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'AT',N'AUT',N'en',null,'2019-02-07T14:03:12.050',N'Setup',N'Austria',null,1,N'114')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'994',1465,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AZ',N'AZE',N'de',null,'2019-02-07T14:03:23.007',N'Setup',N'Aserbaidschan',null,0,N'115')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'994',1466,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AZ',N'AZE',N'en',null,'2019-02-07T14:03:23.010',N'Setup',N'Azerbaijan',null,0,N'115')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1242',1467,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BS',N'BHS',N'de',null,'2019-02-07T14:10:44.307',N'Setup',N'Bahamas',null,0,N'116')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1242',1468,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BS',N'BHS',N'en',null,'2019-02-07T14:10:44.307',N'Setup',N'Bahamas',null,0,N'116')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'973',1469,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BH',N'BHR',N'de',null,'2019-02-07T14:10:54.527',N'Setup',N'Bahrain',null,0,N'117')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'973',1470,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BH',N'BHR',N'en',null,'2019-02-07T14:10:54.527',N'Setup',N'Bahrain',null,0,N'117')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'880',1471,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BD',N'BGD',N'de',null,'2019-02-07T14:11:17.347',N'Setup',N'Bangladesch',null,0,N'118')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'880',1472,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BD',N'BGD',N'en',null,'2019-02-07T14:11:17.347',N'Setup',N'Bangladesh',null,0,N'118')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1246',1473,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BB',N'BRB',N'de',null,'2019-02-07T14:11:29.587',N'Setup',N'Barbados',null,0,N'119')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1246',1474,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BB',N'BRB',N'en',null,'2019-02-07T14:11:29.590',N'Setup',N'Barbados',null,0,N'119')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'375',1475,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BY',N'BLR',N'de',null,'2019-02-07T14:11:40.170',N'Setup',N'Weißrussland',null,0,N'120')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'375',1476,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BY',N'BLR',N'en',null,'2019-02-07T14:11:40.170',N'Setup',N'Belarus',null,0,N'120')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'32',1477,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BE',N'BEL',N'de',null,'2019-02-07T14:11:49.583',N'Setup',N'Belgien',null,0,N'121')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'32',1478,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BE',N'BEL',N'en',null,'2019-02-07T14:11:49.583',N'Setup',N'Belgium',null,0,N'121')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'501',1479,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BZ',N'BLZ',N'de',null,'2019-02-07T14:11:59.583',N'Setup',N'Belize',null,0,N'122')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'501',1480,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BZ',N'BLZ',N'en',null,'2019-02-07T14:11:59.583',N'Setup',N'Belize',null,0,N'122')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'229',1481,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BJ',N'BEN',N'de',null,'2019-02-07T14:12:10.157',N'Setup',N'Benin',null,0,N'123')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'229',1482,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BJ',N'BEN',N'en',null,'2019-02-07T14:12:10.157',N'Setup',N'Benin',null,0,N'123')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1441',1483,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BM',N'BMU',N'de',null,'2019-02-07T14:12:23.343',N'Setup',N'Bermuda',null,0,N'124')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1441',1484,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BM',N'BMU',N'en',null,'2019-02-07T14:12:23.343',N'Setup',N'Bermuda',null,0,N'124')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'975',1485,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BT',N'BTN',N'de',null,'2019-02-07T14:12:33.073',N'Setup',N'Bhutan',null,0,N'125')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'975',1486,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BT',N'BTN',N'en',null,'2019-02-07T14:12:33.073',N'Setup',N'Bhutan',null,0,N'125')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'591',1487,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BO',N'BOL',N'de',null,'2019-02-07T14:12:45.797',N'Setup',N'Bolivien',null,0,N'126')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'591',1488,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BO',N'BOL',N'en',null,'2019-02-07T14:12:45.797',N'Setup',N'Bolivia',null,0,N'126')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'387',1489,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BA',N'BIH',N'de',null,'2019-02-07T14:12:55.880',N'Setup',N'Bosnien und Herzegowina',null,0,N'127')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'387',1490,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BA',N'BIH',N'en',null,'2019-02-07T14:12:55.880',N'Setup',N'Bosnia and Herzegovina',null,0,N'127')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'267',1491,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BW',N'BWA',N'de',null,'2019-02-07T14:13:06.123',N'Setup',N'Botswana',null,0,N'128')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'267',1492,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BW',N'BWA',N'en',null,'2019-02-07T14:13:06.123',N'Setup',N'Botswana',null,0,N'128')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1493,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BV',N'BVT',N'de',null,'2019-02-07T14:13:15.260',N'Setup',N'Bouvet-Insel',null,0,N'129')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1494,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BV',N'BVT',N'en',null,'2019-02-07T14:13:15.260',N'Setup',N'Bouvet Island',null,0,N'129')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'55',1495,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BR',N'BRA',N'de',null,'2019-02-07T14:13:26.480',N'Setup',N'Brasilien',null,0,N'130')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'55',1496,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BR',N'BRA',N'en',null,'2019-02-07T14:13:26.483',N'Setup',N'Brazil',null,0,N'130')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'246',1497,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IO',N'IOT',N'de',null,'2019-02-07T14:13:36.757',N'Setup',N'Britisches Territorium im Indischen Ozean',null,0,N'131')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'246',1498,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IO',N'IOT',N'en',null,'2019-02-07T14:13:36.757',N'Setup',N'British Indian Ocean Territory',null,0,N'131')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'673',1499,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BN',N'BRN',N'de',null,'2019-02-07T14:13:47.967',N'Setup',N'Brunei Darussalam',null,0,N'132')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'673',1500,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BN',N'BRN',N'en',null,'2019-02-07T14:13:47.967',N'Setup',N'Brunei Darussalam',null,0,N'132')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'359',1501,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BG',N'BGR',N'de',null,'2019-02-07T14:13:56.627',N'Setup',N'Bulgarien',null,0,N'133')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'359',1502,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BG',N'BGR',N'en',null,'2019-02-07T14:13:56.630',N'Setup',N'Bulgaria',null,0,N'133')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'226',1503,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BF',N'BFA',N'de',null,'2019-02-07T14:14:07.567',N'Setup',N'Burkina Faso',null,0,N'134')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'226',1504,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BF',N'BFA',N'en',null,'2019-02-07T14:14:07.567',N'Setup',N'Burkina Faso',null,0,N'134')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'257',1505,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BI',N'BDI',N'de',null,'2019-02-07T14:14:18.063',N'Setup',N'Burundi',null,0,N'135')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'257',1506,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'BI',N'BDI',N'en',null,'2019-02-07T14:14:18.063',N'Setup',N'Burundi',null,0,N'135')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'855',1507,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KH',N'KHM',N'de',null,'2019-02-07T14:14:26.483',N'Setup',N'Kambodscha',null,0,N'136')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'855',1508,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KH',N'KHM',N'en',null,'2019-02-07T14:14:26.483',N'Setup',N'Cambodia',null,0,N'136')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'237',1509,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CM',N'CMR',N'de',null,'2019-02-07T14:14:36.797',N'Setup',N'Kamerun',null,0,N'137')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'237',1510,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CM',N'CMR',N'en',null,'2019-02-07T14:14:36.797',N'Setup',N'Cameroon',null,0,N'137')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',1511,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CA',N'CAN',N'de',null,'2019-02-07T14:14:48.270',N'Setup',N'Kanada',null,0,N'138')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',1512,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CA',N'CAN',N'en',null,'2019-02-07T14:14:48.270',N'Setup',N'Canada',null,0,N'138')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'238',1513,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CV',N'CPV',N'de',null,'2019-02-07T14:14:56.757',N'Setup',N'Kap Verde',null,0,N'139')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'238',1514,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CV',N'CPV',N'en',null,'2019-02-07T14:14:56.757',N'Setup',N'Cape Verde',null,0,N'139')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1345',1515,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KY',N'CYM',N'de',null,'2019-02-07T14:15:06.253',N'Setup',N'Caymaninseln',null,0,N'140')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1345',1516,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KY',N'CYM',N'en',null,'2019-02-07T14:15:06.253',N'Setup',N'Cayman Islands',null,0,N'140')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'236',1517,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CF',N'CAF',N'de',null,'2019-02-07T14:15:15.973',N'Setup',N'Zentralafrikanische Republik',null,0,N'141')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'236',1518,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CF',N'CAF',N'en',null,'2019-02-07T14:15:15.977',N'Setup',N'Central African Republic',null,0,N'141')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'235',1519,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TD',N'TCD',N'de',null,'2019-02-07T14:15:25.197',N'Setup',N'Tschad',null,0,N'142')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'235',1520,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TD',N'TCD',N'en',null,'2019-02-07T14:15:25.197',N'Setup',N'Chad',null,0,N'142')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'56',1521,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CL',N'CHL',N'de',null,'2019-02-07T14:15:34.083',N'Setup',N'Chile',null,0,N'143')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'56',1522,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CL',N'CHL',N'en',null,'2019-02-07T14:15:34.083',N'Setup',N'Chile',null,0,N'143')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'86',1523,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CN',N'CHN',N'de',null,'2019-02-07T14:15:39.340',N'Setup',N'China',null,0,N'144')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'86',1524,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CN',N'CHN',N'en',null,'2019-02-07T14:15:39.343',N'Setup',N'China',null,0,N'144')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1525,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CX',N'CXR',N'de',null,'2019-02-07T14:15:49.020',N'Setup',N'Weihnachtsinsel',null,0,N'145')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1526,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CX',N'CXR',N'en',null,'2019-02-07T14:15:49.020',N'Setup',N'Christmas Island',null,0,N'145')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1527,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CC',N'CCK',N'de',null,'2019-02-07T14:15:59.057',N'Setup',N'Kokosinseln',null,0,N'146')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1528,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CC',N'CCK',N'en',null,'2019-02-07T14:15:59.057',N'Setup',N'Cocos (Keeling) Islands',null,0,N'146')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'57',1529,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CO',N'COL',N'de',null,'2019-02-07T14:16:08.273',N'Setup',N'Kolumbien',null,0,N'147')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'57',1530,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CO',N'COL',N'en',null,'2019-02-07T14:16:08.273',N'Setup',N'Colombia',null,0,N'147')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'269',1531,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KM',N'COM',N'de',null,'2019-02-07T14:16:18.890',N'Setup',N'Komoren',null,0,N'148')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'269',1532,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KM',N'COM',N'en',null,'2019-02-07T14:16:18.890',N'Setup',N'Comoros',null,0,N'148')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'242',1533,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CG',N'COG',N'de',null,'2019-02-07T14:16:28.327',N'Setup',N'Kongo',null,0,N'149')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'242',1534,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CG',N'COG',N'en',null,'2019-02-07T14:16:28.330',N'Setup',N'Congo',null,0,N'149')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'243',1535,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CD',N'COD',N'de',null,'2019-02-07T14:16:37.240',N'Setup',N'Demokratische Republik Kongo',null,0,N'150')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'243',1536,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CD',N'COD',N'en',null,'2019-02-07T14:16:37.240',N'Setup',N'The Democratic Republic of the Congo',null,0,N'150')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'682',1537,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CK',N'COK',N'de',null,'2019-02-07T14:16:47.097',N'Setup',N'Cookinseln',null,0,N'151')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'682',1538,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CK',N'COK',N'en',null,'2019-02-07T14:16:47.097',N'Setup',N'Cook Islands',null,0,N'151')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'506',1539,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CR',N'CRI',N'de',null,'2019-02-07T14:16:55.920',N'Setup',N'Costa Rica',null,0,N'152')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'506',1540,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CR',N'CRI',N'en',null,'2019-02-07T14:16:55.920',N'Setup',N'Costa Rica',null,0,N'152')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'225',1541,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CI',N'CIV',N'de',null,'2019-02-07T14:17:05.803',N'Setup',N'Republik D''Ivoire',null,0,N'153')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'225',1542,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CI',N'CIV',N'en',null,'2019-02-07T14:17:05.803',N'Setup',N'Cote D''Ivoire',null,0,N'153')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'385',1543,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HR',N'HRV',N'de',null,'2019-02-07T14:17:17.367',N'Setup',N'Kroatien',null,0,N'154')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'385',1544,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HR',N'HRV',N'en',null,'2019-02-07T14:17:17.367',N'Setup',N'Croatia',null,0,N'154')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'53',1545,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CU',N'CUB',N'de',null,'2019-02-07T14:17:25.463',N'Setup',N'Kuba',null,0,N'155')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'53',1546,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CU',N'CUB',N'en',null,'2019-02-07T14:17:25.463',N'Setup',N'Cuba',null,0,N'155')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'357',1547,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CY',N'CYP',N'de',null,'2019-02-07T14:17:36.943',N'Setup',N'Zypern',null,0,N'156')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'357',1548,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CY',N'CYP',N'en',null,'2019-02-07T14:17:36.943',N'Setup',N'Cyprus',null,0,N'156')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'420',1549,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CZ',N'CZE',N'de',null,'2019-02-07T14:17:44.930',N'Setup',N'Tschechische Republik',null,0,N'157')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'420',1550,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'CZ',N'CZE',N'en',null,'2019-02-07T14:17:44.930',N'Setup',N'Czech Republic',null,0,N'157')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'45',1551,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DK',N'DNK',N'de',null,'2019-02-07T14:17:54.533',N'Setup',N'Dänemark',null,0,N'158')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'45',1552,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DK',N'DNK',N'en',null,'2019-02-07T14:17:54.537',N'Setup',N'Denmark',null,0,N'158')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'253',1553,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DJ',N'DJI',N'de',null,'2019-02-07T14:18:03.137',N'Setup',N'Dschibuti',null,0,N'159')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'253',1554,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DJ',N'DJI',N'en',null,'2019-02-07T14:18:03.137',N'Setup',N'Djibouti',null,0,N'159')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1767',1555,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DM',N'DMA',N'de',null,'2019-02-07T14:18:14.350',N'Setup',N'Dominica',null,0,N'160')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1767',1556,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DM',N'DMA',N'en',null,'2019-02-07T14:18:14.350',N'Setup',N'Dominica',null,0,N'160')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1809',1557,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DO',N'DOM',N'de',null,'2019-02-07T14:18:28.693',N'Setup',N'Dominikanische Republik',null,0,N'161')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1809',1558,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'DO',N'DOM',N'en',null,'2019-02-07T14:18:28.693',N'Setup',N'Dominican Republic',null,0,N'161')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'593',1559,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EC',N'ECU',N'de',null,'2019-02-07T14:22:11.597',N'Setup',N'Ecuador',null,0,N'162')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'593',1560,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EC',N'ECU',N'en',null,'2019-02-07T14:22:11.597',N'Setup',N'Ecuador',null,0,N'162')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'20',1561,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EG',N'EGY',N'de',null,'2019-02-07T14:22:21.480',N'Setup',N'Ägypten',null,0,N'163')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'20',1562,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EG',N'EGY',N'en',null,'2019-02-07T14:22:21.480',N'Setup',N'Egypt',null,0,N'163')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'503',1563,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SV',N'SLV',N'de',null,'2019-02-07T14:22:30.003',N'Setup',N'El Salvador',null,0,N'164')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'503',1564,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SV',N'SLV',N'en',null,'2019-02-07T14:22:30.013',N'Setup',N'El Salvador',null,0,N'164')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'240',1565,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GQ',N'GNQ',N'de',null,'2019-02-07T14:22:40.810',N'Setup',N'Äquatorialguinea',null,0,N'165')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'240',1566,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GQ',N'GNQ',N'en',null,'2019-02-07T14:22:40.810',N'Setup',N'Equatorial Guinea',null,0,N'165')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'291',1567,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ER',N'ERI',N'de',null,'2019-02-07T14:22:49.693',N'Setup',N'Eritrea',null,0,N'166')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'291',1568,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ER',N'ERI',N'en',null,'2019-02-07T14:22:49.693',N'Setup',N'Eritrea',null,0,N'166')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'372',1569,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EE',N'EST',N'de',null,'2019-02-07T14:22:59.793',N'Setup',N'Estland',null,0,N'167')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'372',1570,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EE',N'EST',N'en',null,'2019-02-07T14:22:59.793',N'Setup',N'Estonia',null,0,N'167')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'251',1571,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ET',N'ETH',N'de',null,'2019-02-07T14:23:09.363',N'Setup',N'Äthiopien',null,0,N'168')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'251',1572,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ET',N'ETH',N'en',null,'2019-02-07T14:23:09.363',N'Setup',N'Ethiopia',null,0,N'168')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'500',1573,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FK',N'FLK',N'de',null,'2019-02-07T14:23:19.763',N'Setup',N'Falkland-Inseln',null,0,N'169')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'500',1574,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FK',N'FLK',N'en',null,'2019-02-07T14:23:19.763',N'Setup',N'Falkland Islands (Malvinas)',null,0,N'169')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'298',1575,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FO',N'FRO',N'de',null,'2019-02-07T14:23:29.490',N'Setup',N'Faroer-Inseln',null,0,N'170')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'298',1576,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FO',N'FRO',N'en',null,'2019-02-07T14:23:29.490',N'Setup',N'Faroe Islands',null,0,N'170')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'679',1577,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FJ',N'FJI',N'de',null,'2019-02-07T14:23:37.997',N'Setup',N'Fidschi',null,0,N'171')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'679',1578,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FJ',N'FJI',N'en',null,'2019-02-07T14:23:37.997',N'Setup',N'Fiji',null,0,N'171')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'358',1579,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FI',N'FIN',N'de',null,'2019-02-07T14:23:46.640',N'Setup',N'Finnland',null,0,N'172')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'358',1580,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FI',N'FIN',N'en',null,'2019-02-07T14:23:46.640',N'Setup',N'Finland',null,0,N'172')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'33',1581,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'FR',N'FRA',N'de',null,'2019-02-07T14:23:55.697',N'Setup',N'Frankreich',null,4,N'173')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'33',1582,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'FR',N'FRA',N'en',null,'2019-02-07T14:23:55.700',N'Setup',N'France',null,4,N'173')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'594',1583,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GF',N'GUF',N'de',null,'2019-02-07T14:24:04.270',N'Setup',N'Französisch-Guayana',null,0,N'174')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'594',1584,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GF',N'GUF',N'en',null,'2019-02-07T14:24:04.270',N'Setup',N'French Guiana',null,0,N'174')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'689',1585,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PF',N'PYF',N'de',null,'2019-02-07T14:24:14.367',N'Setup',N'Französisch-Polynesien',null,0,N'175')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'689',1586,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PF',N'PYF',N'en',null,'2019-02-07T14:24:14.367',N'Setup',N'French Polynesia',null,0,N'175')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1587,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TF',N'ATF',N'de',null,'2019-02-07T14:24:26.770',N'Setup',N'Französische Gebiete im südlichen Indischen Ozean',null,0,N'176')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1588,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TF',N'ATF',N'en',null,'2019-02-07T14:24:26.770',N'Setup',N'French Southern Territories',null,0,N'176')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'241',1589,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GA',N'GAB',N'de',null,'2019-02-07T14:24:35.980',N'Setup',N'Gabun',null,0,N'177')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'241',1590,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GA',N'GAB',N'en',null,'2019-02-07T14:24:35.983',N'Setup',N'Gabon',null,0,N'177')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'220',1591,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GM',N'GMB',N'de',null,'2019-02-07T14:24:44.003',N'Setup',N'Gambia',null,0,N'178')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'220',1592,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GM',N'GMB',N'en',null,'2019-02-07T14:24:44.007',N'Setup',N'Gambia',null,0,N'178')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'995',1593,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GE',N'GEO',N'de',null,'2019-02-07T14:24:52.307',N'Setup',N'Georgien',null,0,N'179')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'995',1594,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GE',N'GEO',N'en',null,'2019-02-07T14:24:52.307',N'Setup',N'Georgia',null,0,N'179')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'233',1595,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GH',N'GHA',N'de',null,'2019-02-07T14:25:00.957',N'Setup',N'Ghana',null,0,N'180')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'233',1596,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GH',N'GHA',N'en',null,'2019-02-07T14:25:00.957',N'Setup',N'Ghana',null,0,N'180')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'350',1597,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GI',N'GIB',N'de',null,'2019-02-07T14:25:09.867',N'Setup',N'Gibraltar',null,0,N'181')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'350',1598,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GI',N'GIB',N'en',null,'2019-02-07T14:25:09.867',N'Setup',N'Gibraltar',null,0,N'181')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'30',1599,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GR',N'GRC',N'de',null,'2019-02-07T14:25:18.827',N'Setup',N'Griechenland',null,0,N'182')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'30',1600,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GR',N'GRC',N'en',null,'2019-02-07T14:25:18.827',N'Setup',N'Greece',null,0,N'182')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'299',1601,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GL',N'GRL',N'de',null,'2019-02-07T14:25:28.253',N'Setup',N'Grönland',null,0,N'183')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'299',1602,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GL',N'GRL',N'en',null,'2019-02-07T14:25:28.253',N'Setup',N'Greenland',null,0,N'183')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1473',1603,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GD',N'GRD',N'de',null,'2019-02-07T14:25:38.690',N'Setup',N'Grenada',null,0,N'184')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1473',1604,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GD',N'GRD',N'en',null,'2019-02-07T14:25:38.690',N'Setup',N'Grenada',null,0,N'184')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'590',1605,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GP',N'GLP',N'de',null,'2019-02-07T14:25:47.197',N'Setup',N'Guadeloupe',null,0,N'185')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'590',1606,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GP',N'GLP',N'en',null,'2019-02-07T14:25:47.197',N'Setup',N'Guadeloupe',null,0,N'185')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1671',1607,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GU',N'GUM',N'de',null,'2019-02-07T14:25:55.427',N'Setup',N'Guam',null,0,N'186')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1671',1608,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GU',N'GUM',N'en',null,'2019-02-07T14:25:55.430',N'Setup',N'Guam',null,0,N'186')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'502',1609,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GT',N'GTM',N'de',null,'2019-02-07T14:26:03.800',N'Setup',N'Guatemala',null,0,N'187')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'502',1610,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GT',N'GTM',N'en',null,'2019-02-07T14:26:03.800',N'Setup',N'Guatemala',null,0,N'187')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'224',1611,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GN',N'GIN',N'de',null,'2019-02-07T14:26:13.057',N'Setup',N'Guinea',null,0,N'188')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'224',1612,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GN',N'GIN',N'en',null,'2019-02-07T14:26:13.057',N'Setup',N'Guinea',null,0,N'188')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'245',1613,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GW',N'GNB',N'de',null,'2019-02-07T14:26:22.527',N'Setup',N'Guinea-Bissau',null,0,N'189')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'245',1614,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GW',N'GNB',N'en',null,'2019-02-07T14:26:22.527',N'Setup',N'Guinea-Bissau',null,0,N'189')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'592',1615,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GY',N'GUY',N'de',null,'2019-02-07T14:26:32.313',N'Setup',N'Guyana',null,0,N'190')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'592',1616,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GY',N'GUY',N'en',null,'2019-02-07T14:26:32.313',N'Setup',N'Guyana',null,0,N'190')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'509',1617,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HT',N'HTI',N'de',null,'2019-02-07T14:26:41.917',N'Setup',N'Haiti',null,0,N'191')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'509',1618,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HT',N'HTI',N'en',null,'2019-02-07T14:26:41.917',N'Setup',N'Haiti',null,0,N'191')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1619,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HM',N'HMD',N'de',null,'2019-02-07T14:26:51.460',N'Setup',N'Heard und McDonald-Inseln',null,0,N'192')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1620,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HM',N'HMD',N'en',null,'2019-02-07T14:26:51.460',N'Setup',N'Heard Island and Mcdonald Islands',null,0,N'192')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',1621,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VA',N'VAT',N'de',null,'2019-02-07T14:27:00.557',N'Setup',N'Heiliger See (Vatikanstaat',null,0,N'193')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',1622,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VA',N'VAT',N'en',null,'2019-02-07T14:27:00.557',N'Setup',N'Holy See (Vatican City State)',null,0,N'193')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'504',1623,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HN',N'HND',N'de',null,'2019-02-07T14:27:15.047',N'Setup',N'Honduras',null,0,N'194')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'504',1624,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HN',N'HND',N'en',null,'2019-02-07T14:27:15.050',N'Setup',N'Honduras',null,0,N'194')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'852',1625,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HK',N'HKG',N'de',null,'2019-02-07T14:27:31.887',N'Setup',N'Hong Kong',null,0,N'195')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'852',1626,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HK',N'HKG',N'en',null,'2019-02-07T14:27:31.887',N'Setup',N'Hong Kong',null,0,N'195')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'36',1627,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HU',N'HUN',N'de',null,'2019-02-07T14:27:41.490',N'Setup',N'Ungarn',null,0,N'196')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'36',1628,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'HU',N'HUN',N'en',null,'2019-02-07T14:27:41.490',N'Setup',N'Hungary',null,0,N'196')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'354',1629,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IS',N'ISL',N'de',null,'2019-02-07T14:27:50.290',N'Setup',N'Island',null,0,N'197')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'354',1630,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IS',N'ISL',N'en',null,'2019-02-07T14:27:50.290',N'Setup',N'Iceland',null,0,N'197')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'91',1631,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IN',N'IND',N'de',null,'2019-02-07T14:28:01.843',N'Setup',N'Indien',null,0,N'198')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'91',1632,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IN',N'IND',N'en',null,'2019-02-07T14:28:01.843',N'Setup',N'India',null,0,N'198')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'62',1633,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ID',N'IDN',N'de',null,'2019-02-07T14:28:11.033',N'Setup',N'Indonesien',null,0,N'199')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'62',1634,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ID',N'IDN',N'en',null,'2019-02-07T14:28:11.037',N'Setup',N'Indonesia',null,0,N'199')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1635,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IR',N'IRN',N'de',null,'2019-02-07T14:28:21.050',N'Setup',N'Islamische Republik Iran',null,0,N'200')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1636,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IR',N'IRN',N'en',null,'2019-02-07T14:28:21.050',N'Setup',N'Islamic Republic of Iran',null,0,N'200')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'964',1637,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IQ',N'IRQ',N'de',null,'2019-02-07T14:28:30.737',N'Setup',N'Irak',null,0,N'201')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'964',1638,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IQ',N'IRQ',N'en',null,'2019-02-07T14:28:30.740',N'Setup',N'Iraq',null,0,N'201')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'353',1639,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IE',N'IRL',N'de',null,'2019-02-07T14:28:39.627',N'Setup',N'Irland',null,0,N'202')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'353',1640,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IE',N'IRL',N'en',null,'2019-02-07T14:28:39.627',N'Setup',N'Ireland',null,0,N'202')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'972',1641,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IL',N'ISR',N'de',null,'2017-01-23T14:37:54.013',N'Migration_20161221090000',N'Israel',null,0,N'203')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'972',1642,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'IL',N'ISR',N'en',null,'2017-01-23T14:37:54.013',N'Migration_20161221090000',N'Israel',null,0,N'203')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',1643,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'IT',N'ITA',N'de',null,'2019-02-07T14:29:20.850',N'Setup',N'Italien',null,5,N'204')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',1644,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'IT',N'ITA',N'en',null,'2019-02-07T14:29:20.850',N'Setup',N'Italy',null,5,N'204')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1876',1645,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'JM',N'JAM',N'de',null,'2019-02-07T14:29:30.363',N'Setup',N'Jamaika',null,0,N'205')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1876',1646,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'JM',N'JAM',N'en',null,'2019-02-07T14:29:30.363',N'Setup',N'Jamaica',null,0,N'205')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'81',1647,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'JP',N'JPN',N'de',null,'2019-02-07T14:29:40.053',N'Setup',N'Japan',null,0,N'206')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'81',1648,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'JP',N'JPN',N'en',null,'2019-02-07T14:29:40.053',N'Setup',N'Japan',null,0,N'206')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'962',1649,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'JO',N'JOR',N'de',null,'2019-02-07T14:29:47.793',N'Setup',N'Jordan',null,0,N'207')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'962',1650,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'JO',N'JOR',N'en',null,'2019-02-07T14:29:47.793',N'Setup',N'Jordan',null,0,N'207')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',1651,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KZ',N'KAZ',N'de',null,'2019-02-07T14:29:58.970',N'Setup',N'Kasachstan',null,0,N'208')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',1652,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KZ',N'KAZ',N'en',null,'2019-02-07T14:29:58.970',N'Setup',N'Kazakhstan',null,0,N'208')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'254',1653,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KE',N'KEN',N'de',null,'2019-02-07T14:30:07.177',N'Setup',N'Kenia',null,0,N'209')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'254',1654,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KE',N'KEN',N'en',null,'2019-02-07T14:30:07.177',N'Setup',N'Kenya',null,0,N'209')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'686',1655,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KI',N'KIR',N'de',null,'2019-02-07T14:30:15.967',N'Setup',N'Kiribati',null,0,N'210')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'686',1656,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KI',N'KIR',N'en',null,'2019-02-07T14:30:15.967',N'Setup',N'Kiribati',null,0,N'210')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1657,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KP',N'PRK',N'de',null,'2019-02-07T14:30:26.243',N'Setup',N'Demokratische Volksrepublik Korea',null,0,N'211')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1658,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KP',N'PRK',N'en',null,'2019-02-07T14:30:26.247',N'Setup',N'Democratic Peoples Republic of Korea',null,0,N'211')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'850',1659,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KR',N'KOR',N'de',null,'2019-02-07T14:30:41.780',N'Setup',N'Republik Korea',null,0,N'212')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'850',1660,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KR',N'KOR',N'en',null,'2019-02-07T14:30:41.780',N'Setup',N'Republic of Korea',null,0,N'212')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'965',1661,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KW',N'KWT',N'de',null,'2019-02-07T14:30:51.967',N'Setup',N'Kuwait',null,0,N'213')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'965',1662,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KW',N'KWT',N'en',null,'2019-02-07T14:30:51.967',N'Setup',N'Kuwait',null,0,N'213')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'996',1663,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KG',N'KGZ',N'de',null,'2019-02-07T14:31:02.723',N'Setup',N'Kirgisistan',null,0,N'214')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'996',1664,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KG',N'KGZ',N'en',null,'2019-02-07T14:31:02.723',N'Setup',N'Kyrgyzstan',null,0,N'214')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1665,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'',N'',N'de',null,'2019-02-07T14:31:11.870',N'Setup',N'Volksdemokratische Republik Laos',null,0,N'215')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1666,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'',N'',N'en',null,'2019-02-07T14:31:11.873',N'Setup',N'Lao People''s Democratic Republic',null,0,N'215')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'371',1667,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LV',N'LVA',N'de',null,'2019-02-07T14:31:20.570',N'Setup',N'Lettland',null,0,N'216')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'371',1668,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LV',N'LVA',N'en',null,'2019-02-07T14:31:20.573',N'Setup',N'Latvia',null,0,N'216')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'961',1669,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LB',N'LBN',N'de',null,'2019-02-07T14:31:30.643',N'Setup',N'Libanon',null,0,N'217')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'961',1670,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LB',N'LBN',N'en',null,'2019-02-07T14:31:30.643',N'Setup',N'Lebanon',null,0,N'217')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'266',1671,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LS',N'LSO',N'de',null,'2019-02-07T14:31:40.297',N'Setup',N'Lesotho',null,0,N'218')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'266',1672,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LS',N'LSO',N'en',null,'2019-02-07T14:31:40.297',N'Setup',N'Lesotho',null,0,N'218')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'231',1673,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LR',N'LBR',N'de',null,'2019-02-07T14:31:48.173',N'Setup',N'Liberia',null,0,N'219')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'231',1674,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LR',N'LBR',N'en',null,'2019-02-07T14:31:48.173',N'Setup',N'Liberia',null,0,N'219')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'218',1675,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LY',N'LBY',N'de',null,'2019-02-07T14:31:57.493',N'Setup',N'Libysch-Arabische Dschamahirija',null,0,N'220')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'218',1676,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LY',N'LBY',N'en',null,'2019-02-07T14:31:57.493',N'Setup',N'Libyan Arab Jamahiriya',null,0,N'220')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'423',1677,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LI',N'LIE',N'de',null,'2019-02-07T14:32:05.753',N'Setup',N'Lichtenstein',null,0,N'221')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'423',1678,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LI',N'LIE',N'en',null,'2019-02-07T14:32:05.753',N'Setup',N'Liechtenstein',null,0,N'221')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'370',1679,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LT',N'LTU',N'de',null,'2019-02-07T14:32:14.117',N'Setup',N'Litauen',null,0,N'222')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'370',1680,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LT',N'LTU',N'en',null,'2019-02-07T14:32:14.117',N'Setup',N'Lithuania',null,0,N'222')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'352',1681,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LU',N'LUX',N'de',null,'2019-02-07T14:32:22.457',N'Setup',N'Luxemburg',null,0,N'223')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'352',1682,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LU',N'LUX',N'en',null,'2019-02-07T14:32:22.457',N'Setup',N'Luxembourg',null,0,N'223')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'853',1683,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MO',N'MAC',N'de',null,'2019-02-07T14:32:31.507',N'Setup',N'Macao',null,0,N'224')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'853',1684,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MO',N'MAC',N'en',null,'2019-02-07T14:32:31.510',N'Setup',N'Macao',null,0,N'224')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1685,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MK',N'MKD',N'de',null,'2019-02-07T14:32:42.477',N'Setup',N'Ehemalige jugoslawische Republik Mazedonien',null,0,N'225')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1686,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MK',N'MKD',N'en',null,'2019-02-07T14:32:42.477',N'Setup',N'The Former Yugoslav Republic of Macedonia',null,0,N'225')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'261',1687,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MG',N'MDG',N'de',null,'2019-02-07T14:32:51.483',N'Setup',N'Madagaskar',null,0,N'226')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'261',1688,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MG',N'MDG',N'en',null,'2019-02-07T14:32:51.483',N'Setup',N'Madagascar',null,0,N'226')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'265',1689,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MW',N'MWI',N'de',null,'2019-02-07T14:33:01.213',N'Setup',N'Malawi',null,0,N'227')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'265',1690,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MW',N'MWI',N'en',null,'2019-02-07T14:33:01.213',N'Setup',N'Malawi',null,0,N'227')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'60',1691,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MY',N'MYS',N'de',null,'2019-02-07T14:33:11.093',N'Setup',N'Malaysia',null,0,N'228')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'60',1692,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MY',N'MYS',N'en',null,'2019-02-07T14:33:11.093',N'Setup',N'Malaysia',null,0,N'228')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'960',1693,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MV',N'MDV',N'de',null,'2019-02-07T14:33:20.433',N'Setup',N'Malediven',null,0,N'229')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'960',1694,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MV',N'MDV',N'en',null,'2019-02-07T14:33:20.433',N'Setup',N'Maldives',null,0,N'229')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'223',1695,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ML',N'MLI',N'de',null,'2019-02-07T14:33:30.827',N'Setup',N'Mali',null,0,N'230')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'223',1696,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ML',N'MLI',N'en',null,'2019-02-07T14:33:30.827',N'Setup',N'Mali',null,0,N'230')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'356',1697,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MT',N'MLT',N'de',null,'2019-02-07T14:33:45.423',N'Setup',N'Malta',null,0,N'231')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'356',1698,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MT',N'MLT',N'en',null,'2019-02-07T14:33:45.423',N'Setup',N'Malta',null,0,N'231')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'692',1699,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MH',N'MHL',N'de',null,'2019-02-07T14:33:55.610',N'Setup',N'Marshallinseln',null,0,N'232')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'692',1700,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MH',N'MHL',N'en',null,'2019-02-07T14:33:55.610',N'Setup',N'Marshall Islands',null,0,N'232')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'596',1701,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MQ',N'MTQ',N'de',null,'2019-02-07T14:34:04.720',N'Setup',N'Martinique',null,0,N'233')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'596',1702,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MQ',N'MTQ',N'en',null,'2019-02-07T14:34:04.720',N'Setup',N'Martinique',null,0,N'233')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'222',1703,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MR',N'MRT',N'de',null,'2019-02-07T14:34:14.790',N'Setup',N'Mauretanien',null,0,N'234')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'222',1704,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MR',N'MRT',N'en',null,'2019-02-07T14:34:14.790',N'Setup',N'Mauritania',null,0,N'234')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'230',1705,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MU',N'MUS',N'de',null,'2019-02-07T14:34:23.500',N'Setup',N'Mauritius',null,0,N'235')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'230',1706,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MU',N'MUS',N'en',null,'2019-02-07T14:34:23.500',N'Setup',N'Mauritius',null,0,N'235')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',1707,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'YT',N'MYT',N'de',null,'2019-02-07T14:34:38.120',N'Setup',N'Mayotte',null,0,N'236')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',1708,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'YT',N'MYT',N'en',null,'2019-02-07T14:34:38.120',N'Setup',N'Mayotte',null,0,N'236')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'52',1709,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MX',N'MEX',N'de',null,'2019-02-07T14:34:46.717',N'Setup',N'Mexiko',null,0,N'237')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'52',1710,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MX',N'MEX',N'en',null,'2019-02-07T14:34:46.717',N'Setup',N'Mexico',null,0,N'237')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1711,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FM',N'FSM',N'de',null,'2019-02-07T14:34:58.543',N'Setup',N'Föderierte Staaten von Mikronesien',null,0,N'238')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1712,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'FM',N'FSM',N'en',null,'2019-02-07T14:34:58.543',N'Setup',N'Federated States of Micronesia',null,0,N'238')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1713,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MD',N'MDA',N'de',null,'2019-02-07T14:35:10.207',N'Setup',N'Republik Moldau',null,0,N'239')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1714,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MD',N'MDA',N'en',null,'2019-02-07T14:35:10.207',N'Setup',N'Republic of Moldova',null,0,N'239')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'377',1715,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MC',N'MCO',N'de',null,'2019-02-07T14:35:18.783',N'Setup',N'Monaco',null,0,N'240')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'377',1716,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MC',N'MCO',N'en',null,'2019-02-07T14:35:18.783',N'Setup',N'Monaco',null,0,N'240')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'976',1717,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MN',N'MNG',N'de',null,'2019-02-07T14:35:28.690',N'Setup',N'Mongolei',null,0,N'241')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'976',1718,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MN',N'MNG',N'en',null,'2019-02-07T14:35:28.690',N'Setup',N'Mongolia',null,0,N'241')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1664',1719,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MS',N'MSR',N'de',null,'2019-02-07T14:35:37.870',N'Setup',N'Montserrat',null,0,N'242')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1664',1720,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MS',N'MSR',N'en',null,'2019-02-07T14:35:37.870',N'Setup',N'Montserrat',null,0,N'242')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',1721,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MA',N'MAR',N'de',null,'2019-02-07T14:35:47.353',N'Setup',N'Marokko',null,0,N'243')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',1722,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MA',N'MAR',N'en',null,'2019-02-07T14:35:47.353',N'Setup',N'Morocco',null,0,N'243')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'258',1723,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MZ',N'MOZ',N'de',null,'2019-02-07T14:35:55.920',N'Setup',N'Mosambik',null,0,N'244')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'258',1724,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MZ',N'MOZ',N'en',null,'2019-02-07T14:35:55.920',N'Setup',N'Mozambique',null,0,N'244')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'95',1725,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MM',N'MMR',N'de',null,'2019-02-07T14:36:04.560',N'Setup',N'Myanmar',null,0,N'245')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'95',1726,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MM',N'MMR',N'en',null,'2019-02-07T14:36:04.560',N'Setup',N'Myanmar',null,0,N'245')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'264',1727,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NA',N'NAM',N'de',null,'2019-02-07T14:36:13.783',N'Setup',N'Namibien',null,0,N'246')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'264',1728,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NA',N'NAM',N'en',null,'2019-02-07T14:36:13.783',N'Setup',N'Namibia',null,0,N'246')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'674',1729,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NR',N'NRU',N'de',null,'2019-02-07T14:36:22.237',N'Setup',N'Nauru',null,0,N'247')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'674',1730,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NR',N'NRU',N'en',null,'2019-02-07T14:36:22.237',N'Setup',N'Nauru',null,0,N'247')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'977',1731,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NP',N'NPL',N'de',null,'2019-02-07T14:36:30.470',N'Setup',N'Nepal',null,0,N'248')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'977',1732,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NP',N'NPL',N'en',null,'2019-02-07T14:36:30.470',N'Setup',N'Nepal',null,0,N'248')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'31',1733,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NL',N'NLD',N'de',null,'2019-02-07T14:36:39.420',N'Setup',N'Holland',null,0,N'249')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'31',1734,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NL',N'NLD',N'en',null,'2019-02-07T14:36:39.420',N'Setup',N'Netherlands',null,0,N'249')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'599',1735,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AN',N'ANT',N'de',null,'2019-02-07T14:36:49.673',N'Setup',N'Niederländische Antillen',null,0,N'250')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'599',1736,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AN',N'ANT',N'en',null,'2019-02-07T14:36:49.673',N'Setup',N'Netherlands Antilles',null,0,N'250')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'687',1737,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NC',N'NCL',N'de',null,'2019-02-07T14:37:00.253',N'Setup',N'Neukaledonien',null,0,N'251')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'687',1738,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NC',N'NCL',N'en',null,'2019-02-07T14:37:00.253',N'Setup',N'New Caledonia',null,0,N'251')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'64',1739,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NZ',N'NZL',N'de',null,'2019-02-07T14:37:09.177',N'Setup',N'Neuseeland',null,0,N'252')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'64',1740,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NZ',N'NZL',N'en',null,'2019-02-07T14:37:09.177',N'Setup',N'New Zealand',null,0,N'252')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'505',1741,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NI',N'NIC',N'de',null,'2019-02-07T14:37:19.350',N'Setup',N'Nicaragua',null,0,N'253')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'505',1742,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NI',N'NIC',N'en',null,'2019-02-07T14:37:19.350',N'Setup',N'Nicaragua',null,0,N'253')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'227',1743,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NE',N'NER',N'de',null,'2019-02-07T14:37:29.690',N'Setup',N'Niger',null,0,N'254')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'227',1744,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NE',N'NER',N'en',null,'2019-02-07T14:37:29.690',N'Setup',N'Niger',null,0,N'254')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'234',1745,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NG',N'NGA',N'de',null,'2019-02-07T14:37:38.800',N'Setup',N'Nigeria',null,0,N'255')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'234',1746,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NG',N'NGA',N'en',null,'2019-02-07T14:37:38.800',N'Setup',N'Nigeria',null,0,N'255')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'683',1747,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NU',N'NIU',N'de',null,'2019-02-07T14:37:49.033',N'Setup',N'Niue',null,0,N'256')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'683',1748,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NU',N'NIU',N'en',null,'2019-02-07T14:37:49.037',N'Setup',N'Niue',null,0,N'256')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6723',1749,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NF',N'NFK',N'de',null,'2019-02-07T14:37:59.043',N'Setup',N'Norfolkinsel',null,0,N'257')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6723',1750,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NF',N'NFK',N'en',null,'2019-02-07T14:37:59.043',N'Setup',N'Norfolk Island',null,0,N'257')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1670',1751,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MP',N'MNP',N'de',null,'2019-02-07T14:38:08.233',N'Setup',N'Nördliche Marianen',null,0,N'258')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1670',1752,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'MP',N'MNP',N'en',null,'2019-02-07T14:38:08.233',N'Setup',N'Northern Mariana Islands',null,0,N'258')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',1753,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NO',N'NOR',N'de',null,'2019-02-07T14:38:17.993',N'Setup',N'Norwegen',null,0,N'259')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',1754,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'NO',N'NOR',N'en',null,'2019-02-07T14:38:17.993',N'Setup',N'Norway',null,0,N'259')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'968',1755,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'OM',N'OMN',N'de',null,'2019-02-07T14:38:28.407',N'Setup',N'Oman',null,0,N'260')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'968',1756,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'OM',N'OMN',N'en',null,'2019-02-07T14:38:28.410',N'Setup',N'Oman',null,0,N'260')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'92',1757,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PK',N'PAK',N'de',null,'2019-02-07T14:38:36.340',N'Setup',N'Pakistan',null,0,N'261')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'92',1758,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PK',N'PAK',N'en',null,'2019-02-07T14:38:36.340',N'Setup',N'Pakistan',null,0,N'261')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'680',1759,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PW',N'PLW',N'de',null,'2019-02-07T14:38:49.573',N'Setup',N'Palau',null,0,N'262')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'680',1760,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PW',N'PLW',N'en',null,'2019-02-07T14:38:49.573',N'Setup',N'Palau',null,0,N'262')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1761,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PS',N'PSE',N'de',null,'2019-02-07T14:38:59.950',N'Setup',N'Palästina',null,0,N'263')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1762,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PS',N'PSE',N'en',null,'2019-02-07T14:38:59.950',N'Setup',N'Occupied Palestinian Territory',null,0,N'263')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'507',1763,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PA',N'PAN',N'de',null,'2019-02-07T14:39:13.083',N'Setup',N'Panama',null,0,N'264')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'507',1764,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PA',N'PAN',N'en',null,'2019-02-07T14:39:13.083',N'Setup',N'Panama',null,0,N'264')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'675',1765,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PG',N'PNG',N'de',null,'2019-02-07T14:39:22.963',N'Setup',N'Papua-Neuguinea',null,0,N'265')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'675',1766,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PG',N'PNG',N'en',null,'2019-02-07T14:39:22.963',N'Setup',N'Papua New Guinea',null,0,N'265')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'595',1767,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PY',N'PRY',N'de',null,'2019-02-07T14:41:47.403',N'Setup',N'Paraguay',null,0,N'266')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'595',1768,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PY',N'PRY',N'en',null,'2019-02-07T14:41:47.403',N'Setup',N'Paraguay',null,0,N'266')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'51',1769,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PE',N'PER',N'de',null,'2019-02-07T14:39:44.293',N'Setup',N'Peru',null,0,N'267')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'51',1770,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PE',N'PER',N'en',null,'2019-02-07T14:39:44.293',N'Setup',N'Peru',null,0,N'267')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'63',1771,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PH',N'PHL',N'de',null,'2019-02-07T14:39:52.843',N'Setup',N'Philippinen',null,0,N'268')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'63',1772,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PH',N'PHL',N'en',null,'2019-02-07T14:39:52.847',N'Setup',N'Philippines',null,0,N'268')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1773,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PN',N'PCN',N'de',null,'2019-02-07T14:40:01.737',N'Setup',N'Pitcairn',null,0,N'269')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1774,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PN',N'PCN',N'en',null,'2019-02-07T14:40:01.737',N'Setup',N'Pitcairn',null,0,N'269')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'48',1775,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PL',N'POL',N'de',null,'2019-02-07T14:40:10.143',N'Setup',N'Polen',null,0,N'270')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'48',1776,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PL',N'POL',N'en',null,'2019-02-07T14:40:10.143',N'Setup',N'Poland',null,0,N'270')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'351',1777,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PT',N'PRT',N'de',null,'2019-02-07T14:41:53.740',N'Setup',N'Portugal',null,0,N'271')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'351',1778,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PT',N'PRT',N'en',null,'2019-02-07T14:41:53.740',N'Setup',N'Portugal',null,0,N'271')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1787',1779,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PR',N'PRI',N'de',null,'2019-02-07T14:41:08.243',N'Setup',N'Puerto Rico',null,0,N'272')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1787',1780,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PR',N'PRI',N'en',null,'2019-02-07T14:41:08.243',N'Setup',N'Puerto Rico',null,0,N'272')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'974',1781,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'QA',N'QAT',N'de',null,'2019-02-07T14:41:09.817',N'Setup',N'Katar',null,0,N'273')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'974',1782,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'QA',N'QAT',N'en',null,'2019-02-07T14:41:09.817',N'Setup',N'Qatar',null,0,N'273')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',1783,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RE',N'REU',N'de',null,'2019-02-07T14:41:11.720',N'Setup',N'Réunion',null,0,N'274')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',1784,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RE',N'REU',N'en',null,'2019-02-07T14:41:11.720',N'Setup',N'Reunion',null,0,N'274')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'40',1785,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RO',N'ROU',N'de',null,'2019-02-07T14:41:57.863',N'Setup',N'Rumänien',null,0,N'275')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'40',1786,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RO',N'ROU',N'en',null,'2019-02-07T14:41:57.863',N'Setup',N'Romania',null,0,N'275')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',1787,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RU',N'RUS',N'de',null,'2019-02-07T14:41:59.457',N'Setup',N'Russische Föderation',null,0,N'276')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',1788,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RU',N'RUS',N'en',null,'2019-02-07T14:41:59.457',N'Setup',N'Russian Federation',null,0,N'276')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'250',1789,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RW',N'RWA',N'de',null,'2019-02-07T14:42:09.093',N'Setup',N'Ruanda',null,0,N'277')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'250',1790,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'RW',N'RWA',N'en',null,'2019-02-07T14:42:09.093',N'Setup',N'Rwanda',null,0,N'277')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'290',1791,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SH',N'SHN',N'de',null,'2019-02-07T14:42:24.507',N'Setup',N'St. Helena',null,0,N'278')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'290',1792,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SH',N'SHN',N'en',null,'2019-02-07T14:42:24.507',N'Setup',N'Saint Helena',null,0,N'278')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1869',1793,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KN',N'KNA',N'de',null,'2019-02-07T14:42:34.957',N'Setup',N'St. Kitts und Nevis',null,0,N'279')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1869',1794,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'KN',N'KNA',N'en',null,'2019-02-07T14:42:34.957',N'Setup',N'Saint Kitts and Nevis',null,0,N'279')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1758',1795,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LC',N'LCA',N'de',null,'2019-02-07T14:42:44.223',N'Setup',N'Saint Lucia',null,0,N'280')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1758',1796,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LC',N'LCA',N'en',null,'2019-02-07T14:42:44.227',N'Setup',N'Saint Lucia',null,0,N'280')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'508',1797,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PM',N'SPM',N'de',null,'2019-02-07T14:42:54.650',N'Setup',N'Saint Pierre und Miquelon',null,0,N'281')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'508',1798,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'PM',N'SPM',N'en',null,'2019-02-07T14:42:54.650',N'Setup',N'Saint Pierre and Miquelon',null,0,N'281')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1784',1799,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VC',N'VCT',N'de',null,'2019-02-07T14:43:05.770',N'Setup',N'St. Vincent und die Grenadinen',null,0,N'282')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1784',1800,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VC',N'VCT',N'en',null,'2019-02-07T14:43:05.770',N'Setup',N'Saint Vincent and the Grenadines',null,0,N'282')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'685',1801,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'WS',N'WSM',N'de',null,'2019-02-07T14:43:16.370',N'Setup',N'Samoa',null,0,N'283')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'685',1802,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'WS',N'WSM',N'en',null,'2019-02-07T14:43:16.370',N'Setup',N'Samoa',null,0,N'283')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'378',1803,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SM',N'SMR',N'de',null,'2019-02-07T14:43:27.007',N'Setup',N'San Marino',null,0,N'284')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'378',1804,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SM',N'SMR',N'en',null,'2019-02-07T14:43:27.007',N'Setup',N'San Marino',null,0,N'284')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'239',1805,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ST',N'STP',N'de',null,'2019-02-07T14:43:39.137',N'Setup',N'Sao Tome und Principe',null,0,N'285')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'239',1806,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ST',N'STP',N'en',null,'2019-02-07T14:43:39.137',N'Setup',N'Sao Tome and Principe',null,0,N'285')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'966',1807,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SA',N'SAU',N'de',null,'2019-02-07T14:43:49.077',N'Setup',N'Saudi-Arabien',null,0,N'286')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'966',1808,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SA',N'SAU',N'en',null,'2019-02-07T14:43:49.077',N'Setup',N'Saudi Arabia',null,0,N'286')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'221',1809,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SN',N'SEN',N'de',null,'2019-02-07T14:43:58.627',N'Setup',N'Senegal',null,0,N'287')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'221',1810,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SN',N'SEN',N'en',null,'2019-02-07T14:43:58.627',N'Setup',N'Senegal',null,0,N'287')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1811,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'',N'',N'de',null,'2019-02-07T14:45:59.907',N'Setup',N'Serbien und Montenegro',null,0,N'288')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1812,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'',N'',N'en',null,'2019-02-07T14:45:59.907',N'Setup',N'Serbia and Montenegro',null,0,N'288')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'248',1813,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SC',N'SYC',N'de',null,'2019-02-07T14:47:27.930',N'Setup',N'Seychellen',null,0,N'289')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'248',1814,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SC',N'SYC',N'en',null,'2019-02-07T14:47:27.930',N'Setup',N'Seychelles',null,0,N'289')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'232',1815,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SL',N'SLE',N'de',null,'2019-02-07T14:47:58.110',N'Setup',N'Sierra Leone',null,0,N'290')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'232',1816,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SL',N'SLE',N'en',null,'2019-02-07T14:47:58.113',N'Setup',N'Sierra Leone',null,0,N'290')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'65',1817,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SG',N'SGP',N'de',null,'2019-02-07T14:48:07.780',N'Setup',N'Singapur',null,0,N'291')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'65',1818,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SG',N'SGP',N'en',null,'2019-02-07T14:48:07.780',N'Setup',N'Singapore',null,0,N'291')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'421',1819,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SK',N'SVK',N'de',null,'2019-02-07T14:48:16.750',N'Setup',N'Slowakei',null,0,N'292')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'421',1820,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SK',N'SVK',N'en',null,'2019-02-07T14:48:16.750',N'Setup',N'Slovakia',null,0,N'292')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'386',1821,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SI',N'SVN',N'de',null,'2019-02-07T14:49:20.167',N'Setup',N'Slowenien',null,0,N'293')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'386',1822,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SI',N'SVN',N'en',null,'2019-02-07T14:49:20.167',N'Setup',N'Slovenia',null,0,N'293')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'677',1823,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SB',N'SLB',N'de',null,'2019-02-07T14:49:30.100',N'Setup',N'Solomoninseln',null,0,N'294')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'677',1824,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SB',N'SLB',N'en',null,'2019-02-07T14:49:30.100',N'Setup',N'Solomon Islands',null,0,N'294')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'252',1825,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SO',N'SOM',N'de',null,'2019-02-07T14:49:48.423',N'Setup',N'Somalia',null,0,N'295')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'252',1826,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SO',N'SOM',N'en',null,'2019-02-07T14:49:48.423',N'Setup',N'Somalia',null,0,N'295')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'27',1827,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ZA',N'ZAF',N'de',null,'2019-02-07T14:50:13.030',N'Setup',N'Südafrika',null,0,N'296')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'27',1828,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ZA',N'ZAF',N'en',null,'2019-02-07T14:50:13.030',N'Setup',N'South Africa',null,0,N'296')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1829,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GS',N'SGS',N'de',null,'2019-02-07T14:50:21.733',N'Setup',N'Südgeorgien und Südliche Sandwichinseln',null,0,N'297')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1830,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'GS',N'SGS',N'en',null,'2019-02-07T14:50:21.733',N'Setup',N'South Georgia and the South Sandwich Islands',null,0,N'297')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'34',1831,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ES',N'ESP',N'de',null,'2019-02-07T14:50:53.827',N'Setup',N'Spanien',null,0,N'298')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'34',1832,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ES',N'ESP',N'en',null,'2019-02-07T14:50:53.827',N'Setup',N'Spain',null,0,N'298')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'94',1833,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LK',N'LKA',N'de',null,'2019-02-07T14:51:05.720',N'Setup',N'Sri Lanka',null,0,N'299')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'94',1834,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'LK',N'LKA',N'en',null,'2019-02-07T14:51:05.720',N'Setup',N'Sri Lanka',null,0,N'299')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'249',1835,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SD',N'SDN',N'de',null,'2019-02-07T14:51:16.327',N'Setup',N'Sudan',null,0,N'300')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'249',1836,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SD',N'SDN',N'en',null,'2019-02-07T14:51:16.327',N'Setup',N'Sudan',null,0,N'300')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'597',1837,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SR',N'SUR',N'de',null,'2019-02-07T14:51:25.610',N'Setup',N'Suriname',null,0,N'301')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'597',1838,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SR',N'SUR',N'en',null,'2019-02-07T14:51:25.610',N'Setup',N'Suriname',null,0,N'301')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',1839,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SJ',N'SJM',N'de',null,'2019-02-07T14:51:35.140',N'Setup',N'Svalbard und Jan Mayen Inseln',null,0,N'302')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',1840,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SJ',N'SJM',N'en',null,'2019-02-07T14:51:35.143',N'Setup',N'Svalbard and Jan Mayen',null,0,N'302')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'268',1841,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SZ',N'SWZ',N'de',null,'2019-02-07T14:51:44.127',N'Setup',N'Swasiland',null,0,N'303')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'268',1842,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SZ',N'SWZ',N'en',null,'2019-02-07T14:51:44.127',N'Setup',N'Swaziland',null,0,N'303')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'46',1843,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SE',N'SWE',N'de',null,'2019-02-07T14:51:53.953',N'Setup',N'Schweden',null,0,N'304')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'46',1844,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SE',N'SWE',N'en',null,'2019-02-07T14:51:53.953',N'Setup',N'Sweden',null,0,N'304')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'41',1845,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'CH',N'CHE',N'de',null,'2019-02-07T14:52:03.620',N'Setup',N'Schweiz',null,2,N'305')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'41',1846,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'CH',N'CHE',N'en',null,'2019-02-07T14:52:03.620',N'Setup',N'Switzerland',null,2,N'305')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'963',1847,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SY',N'SYR',N'de',null,'2019-02-07T14:52:14.493',N'Setup',N'Syrische Arabische Republik',null,0,N'306')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'963',1848,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'SY',N'SYR',N'en',null,'2019-02-07T14:52:14.493',N'Setup',N'Syrian Arab Republic',null,0,N'306')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'886',1849,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TW',N'TWN',N'de',null,'2019-02-07T14:52:24.130',N'Setup',N'Taiwan (Provinz von China)',null,0,N'307')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'886',1850,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TW',N'TWN',N'en',null,'2019-02-07T14:52:24.130',N'Setup',N'Taiwan, Province of China',null,0,N'307')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'992',1851,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TJ',N'TJK',N'de',null,'2019-02-07T14:52:33.180',N'Setup',N'Tadschikistan',null,0,N'308')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'992',1852,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TJ',N'TJK',N'en',null,'2019-02-07T14:52:33.180',N'Setup',N'Tajikistan',null,0,N'308')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'255',1853,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TZ',N'TZA',N'de',null,'2019-02-07T14:52:42.127',N'Setup',N'Vereinigte Republik Tansania',null,0,N'309')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'255',1854,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TZ',N'TZA',N'en',null,'2019-02-07T14:52:42.127',N'Setup',N'United Republic of Tanzania',null,0,N'309')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'66',1855,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TH',N'THA',N'de',null,'2019-02-07T14:52:51.130',N'Setup',N'Thailand',null,0,N'310')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'66',1856,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TH',N'THA',N'en',null,'2019-02-07T14:52:51.130',N'Setup',N'Thailand',null,0,N'310')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'670',1857,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TL',N'TLS',N'de',null,'2019-02-07T14:53:00.507',N'Setup',N'Timor-Leste',null,0,N'311')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'670',1858,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TL',N'TLS',N'en',null,'2019-02-07T14:53:00.507',N'Setup',N'Timor-Leste',null,0,N'311')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'228',1859,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TG',N'TGO',N'de',null,'2019-02-07T14:53:10.180',N'Setup',N'Togo',null,0,N'312')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'228',1860,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TG',N'TGO',N'en',null,'2019-02-07T14:53:10.180',N'Setup',N'Togo',null,0,N'312')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'690',1861,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TK',N'TKL',N'de',null,'2019-02-07T14:53:19.890',N'Setup',N'Tokelau',null,0,N'313')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'690',1862,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TK',N'TKL',N'en',null,'2019-02-07T14:53:19.890',N'Setup',N'Tokelau',null,0,N'313')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'676',1863,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TO',N'TON',N'de',null,'2019-02-07T14:53:39.073',N'Setup',N'Tonga',null,0,N'314')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'676',1864,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TO',N'TON',N'en',null,'2019-02-07T14:53:39.073',N'Setup',N'Tonga',null,0,N'314')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1868',1865,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TT',N'TTO',N'de',null,'2019-02-07T14:53:50.390',N'Setup',N'Trinidad und Tobago',null,0,N'315')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1868',1866,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TT',N'TTO',N'en',null,'2019-02-07T14:53:50.390',N'Setup',N'Trinidad and Tobago',null,0,N'315')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'216',1867,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TN',N'TUN',N'de',null,'2019-02-07T14:53:56.073',N'Setup',N'Tunesien',null,0,N'316')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'216',1868,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TN',N'TUN',N'en',null,'2019-02-07T14:53:56.073',N'Setup',N'Tunisia',null,0,N'316')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'90',1869,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TR',N'TUR',N'de',null,'2019-02-07T14:54:12.613',N'Setup',N'Türkei',null,0,N'317')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'90',1870,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TR',N'TUR',N'en',null,'2019-02-07T14:54:12.613',N'Setup',N'Turkey',null,0,N'317')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'993',1871,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TM',N'TKM',N'de',null,'2019-02-07T14:54:21.987',N'Setup',N'Turkmenistan',null,0,N'318')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'993',1872,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TM',N'TKM',N'en',null,'2019-02-07T14:54:21.987',N'Setup',N'Turkmenistan',null,0,N'318')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1649',1873,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TC',N'TCA',N'de',null,'2019-02-07T14:54:30.690',N'Setup',N'Turks- und Caicosinseln',null,0,N'319')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1649',1874,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TC',N'TCA',N'en',null,'2019-02-07T14:54:30.690',N'Setup',N'Turks and Caicos Islands',null,0,N'319')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'688',1875,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TV',N'TUV',N'de',null,'2019-02-07T14:54:40.430',N'Setup',N'Tuvalu',null,0,N'320')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'688',1876,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'TV',N'TUV',N'en',null,'2019-02-07T14:54:40.430',N'Setup',N'Tuvalu',null,0,N'320')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'256',1877,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UG',N'UGA',N'de',null,'2019-02-07T14:54:51.777',N'Setup',N'Uganda',null,0,N'321')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'256',1878,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UG',N'UGA',N'en',null,'2019-02-07T14:54:51.777',N'Setup',N'Uganda',null,0,N'321')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'380',1879,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UA',N'UKR',N'de',null,'2019-02-07T14:55:01.013',N'Setup',N'Ukraine',null,0,N'322')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'380',1880,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UA',N'UKR',N'en',null,'2019-02-07T14:55:01.013',N'Setup',N'Ukraine',null,0,N'322')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'971',1881,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AE',N'ARE',N'de',null,'2019-02-07T14:55:11.210',N'Setup',N'Vereinigte Arabische Emirate',null,0,N'323')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'971',1882,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'AE',N'ARE',N'en',null,'2019-02-07T14:55:11.210',N'Setup',N'United Arab Emirates',null,0,N'323')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'44',1883,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'GB',N'GBR',N'de',null,'2019-02-07T14:55:20.117',N'Setup',N'Vereinigte Königreich',null,3,N'324')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'44',1884,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'GB',N'GBR',N'en',null,'2019-02-07T14:55:20.117',N'Setup',N'United Kingdom',null,3,N'324')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',1885,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'US',N'USA',N'de',null,'2019-02-07T14:55:29.870',N'Setup',N'Vereinigte Staaten',null,6,N'325')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',1886,'2017-01-23T14:37:54.013',N'Migration_20161221090000',1,1,1,N'US',N'USA',N'en',null,'2019-02-07T14:55:29.870',N'Setup',N'United States',null,6,N'325')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1887,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UM',N'UMI',N'de',null,'2019-02-07T14:55:40.977',N'Setup',N'Kleinere amerikanische Überseeinseln',null,0,N'326')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1888,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UM',N'UMI',N'en',null,'2019-02-07T14:55:40.977',N'Setup',N'United States Minor Outlying Islands',null,0,N'326')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'598',1889,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UY',N'URY',N'de',null,'2019-02-07T14:55:50.817',N'Setup',N'Uruguay',null,0,N'327')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'598',1890,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UY',N'URY',N'en',null,'2019-02-07T14:55:50.817',N'Setup',N'Uruguay',null,0,N'327')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'998',1891,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UZ',N'UZB',N'de',null,'2019-02-07T14:55:59.827',N'Setup',N'Usbekistan',null,0,N'328')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'998',1892,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'UZ',N'UZB',N'en',null,'2019-02-07T14:55:59.827',N'Setup',N'Uzbekistan',null,0,N'328')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'678',1893,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VU',N'VUT',N'de',null,'2019-02-07T14:56:08.943',N'Setup',N'Vanuatu',null,0,N'329')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'678',1894,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VU',N'VUT',N'en',null,'2019-02-07T14:56:08.943',N'Setup',N'Vanuatu',null,0,N'329')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'58',1895,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VE',N'VEN',N'de',null,'2019-02-07T14:56:18.537',N'Setup',N'Venezuela',null,0,N'330')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'58',1896,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VE',N'VEN',N'en',null,'2019-02-07T14:56:18.537',N'Setup',N'Venezuela',null,0,N'330')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'84',1897,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VN',N'VNM',N'de',null,'2019-02-07T14:56:27.330',N'Setup',N'Vietnam',null,0,N'331')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'84',1898,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VN',N'VNM',N'en',null,'2019-02-07T14:56:27.330',N'Setup',N'Vietnam',null,0,N'331')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1284',1899,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VG',N'VGB',N'de',null,'2019-02-07T14:56:42.423',N'Setup',N'Britische Jungferninseln',null,0,N'332')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1284',1900,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VG',N'VGB',N'en',null,'2019-02-07T14:56:42.423',N'Setup',N'British Virgin Islands',null,0,N'332')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1340',1901,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VI',N'VIR',N'de',null,'2019-02-07T14:56:52.297',N'Setup',N'Amerikanische Jungferninseln',null,0,N'333')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1340',1902,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'VI',N'VIR',N'en',null,'2019-02-07T14:56:52.297',N'Setup',N'U.S. Virgin Islands',null,0,N'333')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'681',1903,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'WF',N'WLF',N'de',null,'2019-02-07T14:57:01.420',N'Setup',N'Wallis und Futuna',null,0,N'334')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'681',1904,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'WF',N'WLF',N'en',null,'2019-02-07T14:57:01.420',N'Setup',N'Wallis and Futuna',null,0,N'334')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',1905,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EH',N'ESH',N'de',null,'2019-02-07T14:57:11.847',N'Setup',N'Westsahara',null,0,N'335')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',1906,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'EH',N'ESH',N'en',null,'2019-02-07T14:57:11.847',N'Setup',N'Western Sahara',null,0,N'335')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'967',1907,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'YE',N'YEM',N'de',null,'2019-02-07T14:57:20.603',N'Setup',N'Jemen',null,0,N'336')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'967',1908,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'YE',N'YEM',N'en',null,'2019-02-07T14:57:20.603',N'Setup',N'Yemen',null,0,N'336')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'260',1909,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ZM',N'ZMB',N'de',null,'2019-02-07T14:57:31.117',N'Setup',N'Sambia',null,0,N'337')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'260',1910,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ZM',N'ZMB',N'en',null,'2019-02-07T14:57:31.117',N'Setup',N'Zambia',null,0,N'337')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'263',1911,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ZW',N'ZWE',N'de',null,'2019-02-07T14:57:41.060',N'Setup',N'Zimbabwe',null,0,N'338')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'263',1912,'2017-01-23T14:37:54.013',N'Migration_20161221090000',0,1,1,N'ZW',N'ZWE',N'en',null,'2019-02-07T14:57:41.060',N'Setup',N'Zimbabwe',null,0,N'338')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'49',1913,'2017-03-20T15:54:15',N'LookupManager',1,1,1,N'DE',N'DEU',N'hu',null,'2019-02-06T21:37:08.503',N'Setup',N'##hu: Country_100',null,0,N'100')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'93',1914,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AF',N'AFG',N'hu',null,'2019-02-07T14:01:00.973',N'Setup',N'##hu: Country_101',null,0,N'101')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'355',1915,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AL',N'ALB',N'hu',null,'2019-02-07T14:00:41.630',N'Setup',N'##hu: Country_102',null,0,N'102')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'213',1916,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'DZ',N'DZA',N'hu',null,'2019-02-07T14:00:19.383',N'Setup',N'##hu: Country_103',null,0,N'103')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1684',1917,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AS',N'ASM',N'hu',null,'2019-02-07T14:01:22.070',N'Setup',N'##hu: Country_104',null,0,N'104')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'376',1918,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AD',N'AND',N'hu',null,'2019-02-07T14:01:32.647',N'Setup',N'##hu: Country_105',null,0,N'105')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'244',1919,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AO',N'AGO',N'hu',null,'2019-02-07T14:01:41.330',N'Setup',N'##hu: Country_106',null,0,N'106')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1264',1920,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AI',N'AIA',N'hu',null,'2019-02-07T14:01:52.347',N'Setup',N'##hu: Country_107',null,0,N'107')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6721',1921,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AQ',N'ATA',N'hu',null,'2019-02-07T14:02:02.743',N'Setup',N'##hu: Country_108',null,0,N'108')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1268',1922,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AG',N'ATG',N'hu',null,'2019-02-07T14:02:16.117',N'Setup',N'##hu: Country_109',null,0,N'109')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'54',1923,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AR',N'ARG',N'hu',null,'2019-02-07T14:02:26.070',N'Setup',N'##hu: Country_110',null,0,N'110')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'374',1924,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AM',N'ARM',N'hu',null,'2019-02-07T14:02:38.613',N'Setup',N'##hu: Country_111',null,0,N'111')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'297',1925,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AW',N'ABW',N'hu',null,'2019-02-07T14:02:47.790',N'Setup',N'##hu: Country_112',null,0,N'112')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'61',1926,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AU',N'AUS',N'hu',null,'2019-02-07T14:03:01.357',N'Setup',N'##hu: Country_113',null,0,N'113')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'43',1927,'2017-03-20T15:54:15',N'LookupManager',1,1,1,N'AT',N'AUT',N'hu',null,'2019-02-07T14:03:12.050',N'Setup',N'##hu: Country_114',null,1,N'114')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'994',1928,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'AZ',N'AZE',N'hu',null,'2019-02-07T14:03:23.010',N'Setup',N'##hu: Country_115',null,0,N'115')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1242',1929,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'BS',N'BHS',N'hu',null,'2019-02-07T14:10:44.310',N'Setup',N'##hu: Country_116',null,0,N'116')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'973',1930,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'BH',N'BHR',N'hu',null,'2019-02-07T14:10:54.527',N'Setup',N'##hu: Country_117',null,0,N'117')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'880',1931,'2017-03-20T15:54:15',N'LookupManager',0,1,1,N'BD',N'BGD',N'hu',null,'2019-02-07T14:11:17.347',N'Setup',N'##hu: Country_118',null,0,N'118')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1246',1932,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BB',N'BRB',N'hu',null,'2019-02-07T14:11:29.590',N'Setup',N'##hu: Country_119',null,0,N'119')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'375',1933,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BY',N'BLR',N'hu',null,'2019-02-07T14:11:40.173',N'Setup',N'##hu: Country_120',null,0,N'120')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'32',1934,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BE',N'BEL',N'hu',null,'2019-02-07T14:11:49.583',N'Setup',N'##hu: Country_121',null,0,N'121')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'501',1935,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BZ',N'BLZ',N'hu',null,'2019-02-07T14:11:59.583',N'Setup',N'##hu: Country_122',null,0,N'122')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'229',1936,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BJ',N'BEN',N'hu',null,'2019-02-07T14:12:10.157',N'Setup',N'##hu: Country_123',null,0,N'123')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1441',1937,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BM',N'BMU',N'hu',null,'2019-02-07T14:12:23.343',N'Setup',N'##hu: Country_124',null,0,N'124')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'975',1938,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BT',N'BTN',N'hu',null,'2019-02-07T14:12:33.073',N'Setup',N'##hu: Country_125',null,0,N'125')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'591',1939,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BO',N'BOL',N'hu',null,'2019-02-07T14:12:45.797',N'Setup',N'##hu: Country_126',null,0,N'126')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'387',1940,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BA',N'BIH',N'hu',null,'2019-02-07T14:12:55.883',N'Setup',N'##hu: Country_127',null,0,N'127')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'267',1941,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BW',N'BWA',N'hu',null,'2019-02-07T14:13:06.123',N'Setup',N'##hu: Country_128',null,0,N'128')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1942,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BV',N'BVT',N'hu',null,'2019-02-07T14:13:15.260',N'Setup',N'##hu: Country_129',null,0,N'129')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'55',1943,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BR',N'BRA',N'hu',null,'2019-02-07T14:13:26.483',N'Setup',N'##hu: Country_130',null,0,N'130')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'246',1944,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'IO',N'IOT',N'hu',null,'2019-02-07T14:13:36.757',N'Setup',N'##hu: Country_131',null,0,N'131')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'673',1945,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BN',N'BRN',N'hu',null,'2019-02-07T14:13:47.967',N'Setup',N'##hu: Country_132',null,0,N'132')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'359',1946,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BG',N'BGR',N'hu',null,'2019-02-07T14:13:56.630',N'Setup',N'##hu: Country_133',null,0,N'133')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'226',1947,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BF',N'BFA',N'hu',null,'2019-02-07T14:14:07.567',N'Setup',N'##hu: Country_134',null,0,N'134')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'257',1948,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'BI',N'BDI',N'hu',null,'2019-02-07T14:14:18.063',N'Setup',N'##hu: Country_135',null,0,N'135')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'855',1949,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KH',N'KHM',N'hu',null,'2019-02-07T14:14:26.487',N'Setup',N'##hu: Country_136',null,0,N'136')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'237',1950,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CM',N'CMR',N'hu',null,'2019-02-07T14:14:36.797',N'Setup',N'##hu: Country_137',null,0,N'137')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',1951,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CA',N'CAN',N'hu',null,'2019-02-07T14:14:48.270',N'Setup',N'##hu: Country_138',null,0,N'138')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'238',1952,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CV',N'CPV',N'hu',null,'2019-02-07T14:14:56.757',N'Setup',N'##hu: Country_139',null,0,N'139')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1345',1953,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KY',N'CYM',N'hu',null,'2019-02-07T14:15:06.253',N'Setup',N'##hu: Country_140',null,0,N'140')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'236',1954,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CF',N'CAF',N'hu',null,'2019-02-07T14:15:15.977',N'Setup',N'##hu: Country_141',null,0,N'141')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'235',1955,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TD',N'TCD',N'hu',null,'2019-02-07T14:15:25.197',N'Setup',N'##hu: Country_142',null,0,N'142')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'56',1956,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CL',N'CHL',N'hu',null,'2019-02-07T14:15:34.087',N'Setup',N'##hu: Country_143',null,0,N'143')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'86',1957,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CN',N'CHN',N'hu',null,'2019-02-07T14:15:39.343',N'Setup',N'##hu: Country_144',null,0,N'144')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1958,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CX',N'CXR',N'hu',null,'2019-02-07T14:15:49.020',N'Setup',N'##hu: Country_145',null,0,N'145')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1959,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CC',N'CCK',N'hu',null,'2019-02-07T14:15:59.060',N'Setup',N'##hu: Country_146',null,0,N'146')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'57',1960,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CO',N'COL',N'hu',null,'2019-02-07T14:16:08.273',N'Setup',N'##hu: Country_147',null,0,N'147')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'269',1961,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KM',N'COM',N'hu',null,'2019-02-07T14:16:18.890',N'Setup',N'##hu: Country_148',null,0,N'148')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'242',1962,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CG',N'COG',N'hu',null,'2019-02-07T14:16:28.330',N'Setup',N'##hu: Country_149',null,0,N'149')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'243',1963,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CD',N'COD',N'hu',null,'2019-02-07T14:16:37.243',N'Setup',N'##hu: Country_150',null,0,N'150')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'682',1964,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CK',N'COK',N'hu',null,'2019-02-07T14:16:47.097',N'Setup',N'##hu: Country_151',null,0,N'151')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'506',1965,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CR',N'CRI',N'hu',null,'2019-02-07T14:16:55.923',N'Setup',N'##hu: Country_152',null,0,N'152')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'225',1966,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CI',N'CIV',N'hu',null,'2019-02-07T14:17:05.803',N'Setup',N'##hu: Country_153',null,0,N'153')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'385',1967,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'HR',N'HRV',N'hu',null,'2019-02-07T14:17:17.367',N'Setup',N'##hu: Country_154',null,0,N'154')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'53',1968,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CU',N'CUB',N'hu',null,'2019-02-07T14:17:25.463',N'Setup',N'##hu: Country_155',null,0,N'155')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'357',1969,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CY',N'CYP',N'hu',null,'2019-02-07T14:17:36.943',N'Setup',N'##hu: Country_156',null,0,N'156')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'420',1970,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'CZ',N'CZE',N'hu',null,'2019-02-07T14:17:44.930',N'Setup',N'##hu: Country_157',null,0,N'157')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'45',1971,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'DK',N'DNK',N'hu',null,'2019-02-07T14:17:54.537',N'Setup',N'##hu: Country_158',null,0,N'158')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'253',1972,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'DJ',N'DJI',N'hu',null,'2019-02-07T14:18:03.137',N'Setup',N'##hu: Country_159',null,0,N'159')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1767',1973,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'DM',N'DMA',N'hu',null,'2019-02-07T14:18:14.350',N'Setup',N'##hu: Country_160',null,0,N'160')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1809',1974,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'DO',N'DOM',N'hu',null,'2019-02-07T14:18:28.693',N'Setup',N'##hu: Country_161',null,0,N'161')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'593',1975,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'EC',N'ECU',N'hu',null,'2019-02-07T14:22:11.600',N'Setup',N'##hu: Country_162',null,0,N'162')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'20',1976,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'EG',N'EGY',N'hu',null,'2019-02-07T14:22:21.480',N'Setup',N'##hu: Country_163',null,0,N'163')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'503',1977,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SV',N'SLV',N'hu',null,'2019-02-07T14:22:30.027',N'Setup',N'##hu: Country_164',null,0,N'164')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'240',1978,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GQ',N'GNQ',N'hu',null,'2019-02-07T14:22:40.810',N'Setup',N'##hu: Country_165',null,0,N'165')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'291',1979,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ER',N'ERI',N'hu',null,'2019-02-07T14:22:49.693',N'Setup',N'##hu: Country_166',null,0,N'166')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'372',1980,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'EE',N'EST',N'hu',null,'2019-02-07T14:22:59.793',N'Setup',N'##hu: Country_167',null,0,N'167')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'251',1981,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ET',N'ETH',N'hu',null,'2019-02-07T14:23:09.363',N'Setup',N'##hu: Country_168',null,0,N'168')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'500',1982,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'FK',N'FLK',N'hu',null,'2019-02-07T14:23:19.767',N'Setup',N'##hu: Country_169',null,0,N'169')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'298',1983,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'FO',N'FRO',N'hu',null,'2019-02-07T14:23:29.490',N'Setup',N'##hu: Country_170',null,0,N'170')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'679',1984,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'FJ',N'FJI',N'hu',null,'2019-02-07T14:23:38',N'Setup',N'##hu: Country_171',null,0,N'171')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'358',1985,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'FI',N'FIN',N'hu',null,'2019-02-07T14:23:46.640',N'Setup',N'##hu: Country_172',null,0,N'172')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'33',1986,'2017-03-20T15:54:16',N'LookupManager',1,1,1,N'FR',N'FRA',N'hu',null,'2019-02-07T14:23:55.700',N'Setup',N'##hu: Country_173',null,4,N'173')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'594',1987,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GF',N'GUF',N'hu',null,'2019-02-07T14:24:04.270',N'Setup',N'##hu: Country_174',null,0,N'174')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'689',1988,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PF',N'PYF',N'hu',null,'2019-02-07T14:24:14.367',N'Setup',N'##hu: Country_175',null,0,N'175')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',1989,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TF',N'ATF',N'hu',null,'2019-02-07T14:24:26.770',N'Setup',N'##hu: Country_176',null,0,N'176')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'241',1990,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GA',N'GAB',N'hu',null,'2019-02-07T14:24:35.983',N'Setup',N'##hu: Country_177',null,0,N'177')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'220',1991,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GM',N'GMB',N'hu',null,'2019-02-07T14:24:44.007',N'Setup',N'##hu: Country_178',null,0,N'178')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'995',1992,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GE',N'GEO',N'hu',null,'2019-02-07T14:24:52.307',N'Setup',N'##hu: Country_179',null,0,N'179')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'233',1993,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GH',N'GHA',N'hu',null,'2019-02-07T14:25:00.957',N'Setup',N'##hu: Country_180',null,0,N'180')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'350',1994,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GI',N'GIB',N'hu',null,'2019-02-07T14:25:09.867',N'Setup',N'##hu: Country_181',null,0,N'181')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'30',1995,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GR',N'GRC',N'hu',null,'2019-02-07T14:25:18.827',N'Setup',N'##hu: Country_182',null,0,N'182')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'299',1996,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GL',N'GRL',N'hu',null,'2019-02-07T14:25:28.253',N'Setup',N'##hu: Country_183',null,0,N'183')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1473',1997,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GD',N'GRD',N'hu',null,'2019-02-07T14:25:38.690',N'Setup',N'##hu: Country_184',null,0,N'184')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'590',1998,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GP',N'GLP',N'hu',null,'2019-02-07T14:25:47.197',N'Setup',N'##hu: Country_185',null,0,N'185')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1671',1999,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GU',N'GUM',N'hu',null,'2019-02-07T14:25:55.430',N'Setup',N'##hu: Country_186',null,0,N'186')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'502',2000,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GT',N'GTM',N'hu',null,'2019-02-07T14:26:03.800',N'Setup',N'##hu: Country_187',null,0,N'187')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'224',2001,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GN',N'GIN',N'hu',null,'2019-02-07T14:26:13.057',N'Setup',N'##hu: Country_188',null,0,N'188')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'245',2002,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GW',N'GNB',N'hu',null,'2019-02-07T14:26:22.527',N'Setup',N'##hu: Country_189',null,0,N'189')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'592',2003,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GY',N'GUY',N'hu',null,'2019-02-07T14:26:32.313',N'Setup',N'##hu: Country_190',null,0,N'190')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'509',2004,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'HT',N'HTI',N'hu',null,'2019-02-07T14:26:41.917',N'Setup',N'##hu: Country_191',null,0,N'191')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2005,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'HM',N'HMD',N'hu',null,'2019-02-07T14:26:51.460',N'Setup',N'##hu: Country_192',null,0,N'192')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',2006,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'VA',N'VAT',N'hu',null,'2019-02-07T14:27:00.557',N'Setup',N'##hu: Country_193',null,0,N'193')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'504',2007,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'HN',N'HND',N'hu',null,'2019-02-07T14:27:15.050',N'Setup',N'##hu: Country_194',null,0,N'194')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'852',2008,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'HK',N'HKG',N'hu',null,'2019-02-07T14:27:31.890',N'Setup',N'##hu: Country_195',null,0,N'195')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'36',2009,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'HU',N'HUN',N'hu',null,'2019-02-07T14:27:41.493',N'Setup',N'##hu: Country_196',null,0,N'196')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'354',2010,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'IS',N'ISL',N'hu',null,'2019-02-07T14:27:50.290',N'Setup',N'##hu: Country_197',null,0,N'197')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'91',2011,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'IN',N'IND',N'hu',null,'2019-02-07T14:28:01.847',N'Setup',N'##hu: Country_198',null,0,N'198')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'62',2012,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ID',N'IDN',N'hu',null,'2019-02-07T14:28:11.037',N'Setup',N'##hu: Country_199',null,0,N'199')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2013,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'IR',N'IRN',N'hu',null,'2019-02-07T14:28:21.050',N'Setup',N'##hu: Country_200',null,0,N'200')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'964',2014,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'IQ',N'IRQ',N'hu',null,'2019-02-07T14:28:30.740',N'Setup',N'##hu: Country_201',null,0,N'201')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'353',2015,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'IE',N'IRL',N'hu',null,'2019-02-07T14:28:39.627',N'Setup',N'##hu: Country_202',null,0,N'202')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'972',2016,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'IL',N'ISR',N'hu',null,'2017-03-20T15:54:16',N'LookupManager',N'##hu: Country_203',null,0,N'203')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',2017,'2017-03-20T15:54:16',N'LookupManager',1,1,1,N'IT',N'ITA',N'hu',null,'2019-02-07T14:29:20.850',N'Setup',N'##hu: Country_204',null,5,N'204')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1876',2018,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'JM',N'JAM',N'hu',null,'2019-02-07T14:29:30.363',N'Setup',N'##hu: Country_205',null,0,N'205')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'81',2019,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'JP',N'JPN',N'hu',null,'2019-02-07T14:29:40.053',N'Setup',N'##hu: Country_206',null,0,N'206')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'962',2020,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'JO',N'JOR',N'hu',null,'2019-02-07T14:29:47.793',N'Setup',N'##hu: Country_207',null,0,N'207')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',2021,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KZ',N'KAZ',N'hu',null,'2019-02-07T14:29:58.970',N'Setup',N'##hu: Country_208',null,0,N'208')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'254',2022,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KE',N'KEN',N'hu',null,'2019-02-07T14:30:07.177',N'Setup',N'##hu: Country_209',null,0,N'209')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'686',2023,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KI',N'KIR',N'hu',null,'2019-02-07T14:30:15.967',N'Setup',N'##hu: Country_210',null,0,N'210')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2024,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KP',N'PRK',N'hu',null,'2019-02-07T14:30:26.247',N'Setup',N'##hu: Country_211',null,0,N'211')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'850',2025,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KR',N'KOR',N'hu',null,'2019-02-07T14:30:41.780',N'Setup',N'##hu: Country_212',null,0,N'212')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'965',2026,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KW',N'KWT',N'hu',null,'2019-02-07T14:30:51.967',N'Setup',N'##hu: Country_213',null,0,N'213')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'996',2027,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KG',N'KGZ',N'hu',null,'2019-02-07T14:31:02.723',N'Setup',N'##hu: Country_214',null,0,N'214')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2028,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'',N'',N'hu',null,'2019-02-07T14:31:11.873',N'Setup',N'##hu: Country_215',null,0,N'215')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'371',2029,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LV',N'LVA',N'hu',null,'2019-02-07T14:31:20.573',N'Setup',N'##hu: Country_216',null,0,N'216')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'961',2030,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LB',N'LBN',N'hu',null,'2019-02-07T14:31:30.643',N'Setup',N'##hu: Country_217',null,0,N'217')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'266',2031,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LS',N'LSO',N'hu',null,'2019-02-07T14:31:40.297',N'Setup',N'##hu: Country_218',null,0,N'218')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'231',2032,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LR',N'LBR',N'hu',null,'2019-02-07T14:31:48.173',N'Setup',N'##hu: Country_219',null,0,N'219')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'218',2033,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LY',N'LBY',N'hu',null,'2019-02-07T14:31:57.493',N'Setup',N'##hu: Country_220',null,0,N'220')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'423',2034,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LI',N'LIE',N'hu',null,'2019-02-07T14:32:05.757',N'Setup',N'##hu: Country_221',null,0,N'221')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'370',2035,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LT',N'LTU',N'hu',null,'2019-02-07T14:32:14.117',N'Setup',N'##hu: Country_222',null,0,N'222')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'352',2036,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LU',N'LUX',N'hu',null,'2019-02-07T14:32:22.460',N'Setup',N'##hu: Country_223',null,0,N'223')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'853',2037,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MO',N'MAC',N'hu',null,'2019-02-07T14:32:31.510',N'Setup',N'##hu: Country_224',null,0,N'224')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2038,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MK',N'MKD',N'hu',null,'2019-02-07T14:32:42.477',N'Setup',N'##hu: Country_225',null,0,N'225')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'261',2039,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MG',N'MDG',N'hu',null,'2019-02-07T14:32:51.483',N'Setup',N'##hu: Country_226',null,0,N'226')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'265',2040,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MW',N'MWI',N'hu',null,'2019-02-07T14:33:01.213',N'Setup',N'##hu: Country_227',null,0,N'227')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'60',2041,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MY',N'MYS',N'hu',null,'2019-02-07T14:33:11.093',N'Setup',N'##hu: Country_228',null,0,N'228')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'960',2042,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MV',N'MDV',N'hu',null,'2019-02-07T14:33:20.433',N'Setup',N'##hu: Country_229',null,0,N'229')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'223',2043,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ML',N'MLI',N'hu',null,'2019-02-07T14:33:30.827',N'Setup',N'##hu: Country_230',null,0,N'230')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'356',2044,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MT',N'MLT',N'hu',null,'2019-02-07T14:33:45.423',N'Setup',N'##hu: Country_231',null,0,N'231')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'692',2045,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MH',N'MHL',N'hu',null,'2019-02-07T14:33:55.610',N'Setup',N'##hu: Country_232',null,0,N'232')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'596',2046,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MQ',N'MTQ',N'hu',null,'2019-02-07T14:34:04.723',N'Setup',N'##hu: Country_233',null,0,N'233')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'222',2047,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MR',N'MRT',N'hu',null,'2019-02-07T14:34:14.790',N'Setup',N'##hu: Country_234',null,0,N'234')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'230',2048,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MU',N'MUS',N'hu',null,'2019-02-07T14:34:23.500',N'Setup',N'##hu: Country_235',null,0,N'235')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',2049,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'YT',N'MYT',N'hu',null,'2019-02-07T14:34:38.120',N'Setup',N'##hu: Country_236',null,0,N'236')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'52',2050,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MX',N'MEX',N'hu',null,'2019-02-07T14:34:46.717',N'Setup',N'##hu: Country_237',null,0,N'237')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2051,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'FM',N'FSM',N'hu',null,'2019-02-07T14:34:58.543',N'Setup',N'##hu: Country_238',null,0,N'238')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2052,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MD',N'MDA',N'hu',null,'2019-02-07T14:35:10.207',N'Setup',N'##hu: Country_239',null,0,N'239')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'377',2053,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MC',N'MCO',N'hu',null,'2019-02-07T14:35:18.783',N'Setup',N'##hu: Country_240',null,0,N'240')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'976',2054,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MN',N'MNG',N'hu',null,'2019-02-07T14:35:28.690',N'Setup',N'##hu: Country_241',null,0,N'241')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1664',2055,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MS',N'MSR',N'hu',null,'2019-02-07T14:35:37.870',N'Setup',N'##hu: Country_242',null,0,N'242')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',2056,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MA',N'MAR',N'hu',null,'2019-02-07T14:35:47.353',N'Setup',N'##hu: Country_243',null,0,N'243')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'258',2057,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MZ',N'MOZ',N'hu',null,'2019-02-07T14:35:55.920',N'Setup',N'##hu: Country_244',null,0,N'244')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'95',2058,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MM',N'MMR',N'hu',null,'2019-02-07T14:36:04.560',N'Setup',N'##hu: Country_245',null,0,N'245')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'264',2059,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NA',N'NAM',N'hu',null,'2019-02-07T14:36:13.783',N'Setup',N'##hu: Country_246',null,0,N'246')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'674',2060,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NR',N'NRU',N'hu',null,'2019-02-07T14:36:22.237',N'Setup',N'##hu: Country_247',null,0,N'247')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'977',2061,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NP',N'NPL',N'hu',null,'2019-02-07T14:36:30.470',N'Setup',N'##hu: Country_248',null,0,N'248')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'31',2062,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NL',N'NLD',N'hu',null,'2019-02-07T14:36:39.420',N'Setup',N'##hu: Country_249',null,0,N'249')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'599',2063,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'AN',N'ANT',N'hu',null,'2019-02-07T14:36:49.673',N'Setup',N'##hu: Country_250',null,0,N'250')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'687',2064,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NC',N'NCL',N'hu',null,'2019-02-07T14:37:00.253',N'Setup',N'##hu: Country_251',null,0,N'251')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'64',2065,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NZ',N'NZL',N'hu',null,'2019-02-07T14:37:09.177',N'Setup',N'##hu: Country_252',null,0,N'252')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'505',2066,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NI',N'NIC',N'hu',null,'2019-02-07T14:37:19.350',N'Setup',N'##hu: Country_253',null,0,N'253')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'227',2067,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NE',N'NER',N'hu',null,'2019-02-07T14:37:29.690',N'Setup',N'##hu: Country_254',null,0,N'254')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'234',2068,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NG',N'NGA',N'hu',null,'2019-02-07T14:37:38.800',N'Setup',N'##hu: Country_255',null,0,N'255')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'683',2069,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NU',N'NIU',N'hu',null,'2019-02-07T14:37:49.037',N'Setup',N'##hu: Country_256',null,0,N'256')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6723',2070,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NF',N'NFK',N'hu',null,'2019-02-07T14:37:59.043',N'Setup',N'##hu: Country_257',null,0,N'257')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1670',2071,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'MP',N'MNP',N'hu',null,'2019-02-07T14:38:08.233',N'Setup',N'##hu: Country_258',null,0,N'258')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',2072,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'NO',N'NOR',N'hu',null,'2019-02-07T14:38:17.993',N'Setup',N'##hu: Country_259',null,0,N'259')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'968',2073,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'OM',N'OMN',N'hu',null,'2019-02-07T14:38:28.410',N'Setup',N'##hu: Country_260',null,0,N'260')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'92',2074,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PK',N'PAK',N'hu',null,'2019-02-07T14:38:36.340',N'Setup',N'##hu: Country_261',null,0,N'261')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'680',2075,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PW',N'PLW',N'hu',null,'2019-02-07T14:38:49.573',N'Setup',N'##hu: Country_262',null,0,N'262')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2076,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PS',N'PSE',N'hu',null,'2019-02-07T14:38:59.950',N'Setup',N'##hu: Country_263',null,0,N'263')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'507',2077,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PA',N'PAN',N'hu',null,'2019-02-07T14:39:13.083',N'Setup',N'##hu: Country_264',null,0,N'264')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'675',2078,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PG',N'PNG',N'hu',null,'2019-02-07T14:39:22.963',N'Setup',N'##hu: Country_265',null,0,N'265')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'595',2079,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PY',N'PRY',N'hu',null,'2019-02-07T14:41:47.407',N'Setup',N'##hu: Country_266',null,0,N'266')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'51',2080,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PE',N'PER',N'hu',null,'2019-02-07T14:39:44.297',N'Setup',N'##hu: Country_267',null,0,N'267')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'63',2081,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PH',N'PHL',N'hu',null,'2019-02-07T14:39:52.847',N'Setup',N'##hu: Country_268',null,0,N'268')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2082,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PN',N'PCN',N'hu',null,'2019-02-07T14:40:01.737',N'Setup',N'##hu: Country_269',null,0,N'269')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'48',2083,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PL',N'POL',N'hu',null,'2019-02-07T14:40:10.143',N'Setup',N'##hu: Country_270',null,0,N'270')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'351',2084,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PT',N'PRT',N'hu',null,'2019-02-07T14:41:53.740',N'Setup',N'##hu: Country_271',null,0,N'271')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1787',2085,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PR',N'PRI',N'hu',null,'2019-02-07T14:41:08.247',N'Setup',N'##hu: Country_272',null,0,N'272')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'974',2086,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'QA',N'QAT',N'hu',null,'2019-02-07T14:41:09.820',N'Setup',N'##hu: Country_273',null,0,N'273')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',2087,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'RE',N'REU',N'hu',null,'2019-02-07T14:41:11.723',N'Setup',N'##hu: Country_274',null,0,N'274')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'40',2088,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'RO',N'ROU',N'hu',null,'2019-02-07T14:41:57.863',N'Setup',N'##hu: Country_275',null,0,N'275')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',2089,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'RU',N'RUS',N'hu',null,'2019-02-07T14:41:59.457',N'Setup',N'##hu: Country_276',null,0,N'276')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'250',2090,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'RW',N'RWA',N'hu',null,'2019-02-07T14:42:09.093',N'Setup',N'##hu: Country_277',null,0,N'277')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'290',2091,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SH',N'SHN',N'hu',null,'2019-02-07T14:42:24.507',N'Setup',N'##hu: Country_278',null,0,N'278')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1869',2092,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'KN',N'KNA',N'hu',null,'2019-02-07T14:42:34.957',N'Setup',N'##hu: Country_279',null,0,N'279')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1758',2093,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LC',N'LCA',N'hu',null,'2019-02-07T14:42:44.227',N'Setup',N'##hu: Country_280',null,0,N'280')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'508',2094,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'PM',N'SPM',N'hu',null,'2019-02-07T14:42:54.650',N'Setup',N'##hu: Country_281',null,0,N'281')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1784',2095,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'VC',N'VCT',N'hu',null,'2019-02-07T14:43:05.770',N'Setup',N'##hu: Country_282',null,0,N'282')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'685',2096,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'WS',N'WSM',N'hu',null,'2019-02-07T14:43:16.370',N'Setup',N'##hu: Country_283',null,0,N'283')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'378',2097,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SM',N'SMR',N'hu',null,'2019-02-07T14:43:27.007',N'Setup',N'##hu: Country_284',null,0,N'284')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'239',2098,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ST',N'STP',N'hu',null,'2019-02-07T14:43:39.137',N'Setup',N'##hu: Country_285',null,0,N'285')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'966',2099,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SA',N'SAU',N'hu',null,'2019-02-07T14:43:49.080',N'Setup',N'##hu: Country_286',null,0,N'286')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'221',2100,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SN',N'SEN',N'hu',null,'2019-02-07T14:43:58.627',N'Setup',N'##hu: Country_287',null,0,N'287')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2101,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'',N'',N'hu',null,'2019-02-07T14:45:59.907',N'Setup',N'##hu: Country_288',null,0,N'288')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'248',2102,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SC',N'SYC',N'hu',null,'2019-02-07T14:47:27.930',N'Setup',N'##hu: Country_289',null,0,N'289')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'232',2103,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SL',N'SLE',N'hu',null,'2019-02-07T14:47:58.113',N'Setup',N'##hu: Country_290',null,0,N'290')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'65',2104,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SG',N'SGP',N'hu',null,'2019-02-07T14:48:07.780',N'Setup',N'##hu: Country_291',null,0,N'291')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'421',2105,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SK',N'SVK',N'hu',null,'2019-02-07T14:48:16.750',N'Setup',N'##hu: Country_292',null,0,N'292')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'386',2106,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SI',N'SVN',N'hu',null,'2019-02-07T14:49:20.167',N'Setup',N'##hu: Country_293',null,0,N'293')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'677',2107,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SB',N'SLB',N'hu',null,'2019-02-07T14:49:30.100',N'Setup',N'##hu: Country_294',null,0,N'294')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'252',2108,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SO',N'SOM',N'hu',null,'2019-02-07T14:49:48.423',N'Setup',N'##hu: Country_295',null,0,N'295')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'27',2109,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ZA',N'ZAF',N'hu',null,'2019-02-07T14:50:13.030',N'Setup',N'##hu: Country_296',null,0,N'296')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2110,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'GS',N'SGS',N'hu',null,'2019-02-07T14:50:21.733',N'Setup',N'##hu: Country_297',null,0,N'297')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'34',2111,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ES',N'ESP',N'hu',null,'2019-02-07T14:50:53.827',N'Setup',N'##hu: Country_298',null,0,N'298')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'94',2112,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'LK',N'LKA',N'hu',null,'2019-02-07T14:51:05.720',N'Setup',N'##hu: Country_299',null,0,N'299')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'249',2113,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SD',N'SDN',N'hu',null,'2019-02-07T14:51:16.330',N'Setup',N'##hu: Country_300',null,0,N'300')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'597',2114,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SR',N'SUR',N'hu',null,'2019-02-07T14:51:25.610',N'Setup',N'##hu: Country_301',null,0,N'301')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',2115,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SJ',N'SJM',N'hu',null,'2019-02-07T14:51:35.143',N'Setup',N'##hu: Country_302',null,0,N'302')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'268',2116,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SZ',N'SWZ',N'hu',null,'2019-02-07T14:51:44.127',N'Setup',N'##hu: Country_303',null,0,N'303')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'46',2117,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SE',N'SWE',N'hu',null,'2019-02-07T14:51:53.953',N'Setup',N'##hu: Country_304',null,0,N'304')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'41',2118,'2017-03-20T15:54:16',N'LookupManager',1,1,1,N'CH',N'CHE',N'hu',null,'2019-02-07T14:52:03.620',N'Setup',N'##hu: Country_305',null,2,N'305')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'963',2119,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'SY',N'SYR',N'hu',null,'2019-02-07T14:52:14.493',N'Setup',N'##hu: Country_306',null,0,N'306')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'886',2120,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TW',N'TWN',N'hu',null,'2019-02-07T14:52:24.130',N'Setup',N'##hu: Country_307',null,0,N'307')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'992',2121,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TJ',N'TJK',N'hu',null,'2019-02-07T14:52:33.183',N'Setup',N'##hu: Country_308',null,0,N'308')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'255',2122,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TZ',N'TZA',N'hu',null,'2019-02-07T14:52:42.127',N'Setup',N'##hu: Country_309',null,0,N'309')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'66',2123,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TH',N'THA',N'hu',null,'2019-02-07T14:52:51.130',N'Setup',N'##hu: Country_310',null,0,N'310')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'670',2124,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TL',N'TLS',N'hu',null,'2019-02-07T14:53:00.507',N'Setup',N'##hu: Country_311',null,0,N'311')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'228',2125,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TG',N'TGO',N'hu',null,'2019-02-07T14:53:10.180',N'Setup',N'##hu: Country_312',null,0,N'312')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'690',2126,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TK',N'TKL',N'hu',null,'2019-02-07T14:53:19.890',N'Setup',N'##hu: Country_313',null,0,N'313')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'676',2127,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TO',N'TON',N'hu',null,'2019-02-07T14:53:39.073',N'Setup',N'##hu: Country_314',null,0,N'314')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1868',2128,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TT',N'TTO',N'hu',null,'2019-02-07T14:53:50.390',N'Setup',N'##hu: Country_315',null,0,N'315')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'216',2129,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TN',N'TUN',N'hu',null,'2019-02-07T14:53:56.073',N'Setup',N'##hu: Country_316',null,0,N'316')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'90',2130,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TR',N'TUR',N'hu',null,'2019-02-07T14:54:12.613',N'Setup',N'##hu: Country_317',null,0,N'317')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'993',2131,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TM',N'TKM',N'hu',null,'2019-02-07T14:54:21.987',N'Setup',N'##hu: Country_318',null,0,N'318')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1649',2132,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TC',N'TCA',N'hu',null,'2019-02-07T14:54:30.690',N'Setup',N'##hu: Country_319',null,0,N'319')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'688',2133,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'TV',N'TUV',N'hu',null,'2019-02-07T14:54:40.433',N'Setup',N'##hu: Country_320',null,0,N'320')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'256',2134,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'UG',N'UGA',N'hu',null,'2019-02-07T14:54:51.777',N'Setup',N'##hu: Country_321',null,0,N'321')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'380',2135,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'UA',N'UKR',N'hu',null,'2019-02-07T14:55:01.013',N'Setup',N'##hu: Country_322',null,0,N'322')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'971',2136,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'AE',N'ARE',N'hu',null,'2019-02-07T14:55:11.210',N'Setup',N'##hu: Country_323',null,0,N'323')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'44',2137,'2017-03-20T15:54:16',N'LookupManager',1,1,1,N'GB',N'GBR',N'hu',null,'2019-02-07T14:55:20.120',N'Setup',N'##hu: Country_324',null,3,N'324')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',2138,'2017-03-20T15:54:16',N'LookupManager',1,1,1,N'US',N'USA',N'hu',null,'2019-02-07T14:55:29.870',N'Setup',N'##hu: Country_325',null,6,N'325')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2139,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'UM',N'UMI',N'hu',null,'2019-02-07T14:55:40.980',N'Setup',N'##hu: Country_326',null,0,N'326')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'598',2140,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'UY',N'URY',N'hu',null,'2019-02-07T14:55:50.817',N'Setup',N'##hu: Country_327',null,0,N'327')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'998',2141,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'UZ',N'UZB',N'hu',null,'2019-02-07T14:55:59.827',N'Setup',N'##hu: Country_328',null,0,N'328')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'678',2142,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'VU',N'VUT',N'hu',null,'2019-02-07T14:56:08.943',N'Setup',N'##hu: Country_329',null,0,N'329')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'58',2143,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'VE',N'VEN',N'hu',null,'2019-02-07T14:56:18.537',N'Setup',N'##hu: Country_330',null,0,N'330')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'84',2144,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'VN',N'VNM',N'hu',null,'2019-02-07T14:56:27.330',N'Setup',N'##hu: Country_331',null,0,N'331')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1284',2145,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'VG',N'VGB',N'hu',null,'2019-02-07T14:56:42.423',N'Setup',N'##hu: Country_332',null,0,N'332')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1340',2146,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'VI',N'VIR',N'hu',null,'2019-02-07T14:56:52.300',N'Setup',N'##hu: Country_333',null,0,N'333')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'681',2147,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'WF',N'WLF',N'hu',null,'2019-02-07T14:57:01.420',N'Setup',N'##hu: Country_334',null,0,N'334')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',2148,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'EH',N'ESH',N'hu',null,'2019-02-07T14:57:11.847',N'Setup',N'##hu: Country_335',null,0,N'335')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'967',2149,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'YE',N'YEM',N'hu',null,'2019-02-07T14:57:20.603',N'Setup',N'##hu: Country_336',null,0,N'336')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'260',2150,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ZM',N'ZMB',N'hu',null,'2019-02-07T14:57:31.117',N'Setup',N'##hu: Country_337',null,0,N'337')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'263',2151,'2017-03-20T15:54:16',N'LookupManager',0,1,1,N'ZW',N'ZWE',N'hu',null,'2019-02-07T14:57:41.060',N'Setup',N'##hu: Country_338',null,0,N'338')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'49',2152,'2019-02-06T21:37:08.277',N'Setup',1,1,1,N'DE',N'DEU',N'fr',null,'2019-02-06T21:37:08.277',N'Setup',N'Allemagne',null,0,N'100')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'213',2153,'2019-02-07T14:00:19.140',N'Setup',0,1,1,N'DZ',N'DZA',N'fr',null,'2019-02-07T14:00:19.140',N'Setup',N'Algérie',null,0,N'103')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'355',2154,'2019-02-07T14:00:41.403',N'Setup',0,1,1,N'AL',N'ALB',N'fr',null,'2019-02-07T14:00:41.403',N'Setup',N'Albanie',null,0,N'102')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'93',2155,'2019-02-07T14:01:00.723',N'Setup',0,1,1,N'AF',N'AFG',N'fr',null,'2019-02-07T14:01:00.723',N'Setup',N'Afghanistan',null,0,N'101')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1684',2156,'2019-02-07T14:01:21.853',N'Setup',0,1,1,N'AS',N'ASM',N'fr',null,'2019-02-07T14:01:21.853',N'Setup',N'Samoa américaines',null,0,N'104')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'376',2157,'2019-02-07T14:01:32.423',N'Setup',0,1,1,N'AD',N'AND',N'fr',null,'2019-02-07T14:01:32.423',N'Setup',N'Andorre',null,0,N'105')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'244',2158,'2019-02-07T14:01:41.110',N'Setup',0,1,1,N'AO',N'AGO',N'fr',null,'2019-02-07T14:01:41.110',N'Setup',N'Angola',null,0,N'106')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1264',2159,'2019-02-07T14:01:52.127',N'Setup',0,1,1,N'AI',N'AIA',N'fr',null,'2019-02-07T14:01:52.127',N'Setup',N'Anguilla',null,0,N'107')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6721',2160,'2019-02-07T14:02:02.517',N'Setup',0,1,1,N'AQ',N'ATA',N'fr',null,'2019-02-07T14:02:02.517',N'Setup',N'Antarctique',null,0,N'108')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1268',2161,'2019-02-07T14:02:15.900',N'Setup',0,1,1,N'AG',N'ATG',N'fr',null,'2019-02-07T14:02:15.900',N'Setup',N'Antigua-et-Barbuda',null,0,N'109')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'54',2162,'2019-02-07T14:02:25.850',N'Setup',0,1,1,N'AR',N'ARG',N'fr',null,'2019-02-07T14:02:25.850',N'Setup',N'Argentine',null,0,N'110')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'374',2163,'2019-02-07T14:02:38.393',N'Setup',0,1,1,N'AM',N'ARM',N'fr',null,'2019-02-07T14:02:38.393',N'Setup',N'Arménie',null,0,N'111')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'297',2164,'2019-02-07T14:02:47.573',N'Setup',0,1,1,N'AW',N'ABW',N'fr',null,'2019-02-07T14:02:47.573',N'Setup',N'Aruba',null,0,N'112')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'61',2165,'2019-02-07T14:03:01.140',N'Setup',0,1,1,N'AU',N'AUS',N'fr',null,'2019-02-07T14:03:01.140',N'Setup',N'Australie',null,0,N'113')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'43',2166,'2019-02-07T14:03:11.830',N'Setup',1,1,1,N'AT',N'AUT',N'fr',null,'2019-02-07T14:03:11.830',N'Setup',N'L''Autriche',null,1,N'114')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'994',2167,'2019-02-07T14:03:22.783',N'Setup',0,1,1,N'AZ',N'AZE',N'fr',null,'2019-02-07T14:03:22.783',N'Setup',N'Azerbaïdjan',null,0,N'115')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1242',2168,'2019-02-07T14:10:44.067',N'Setup',0,1,1,N'BS',N'BHS',N'fr',null,'2019-02-07T14:10:44.067',N'Setup',N'Bahamas',null,0,N'116')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'973',2169,'2019-02-07T14:10:54.303',N'Setup',0,1,1,N'BH',N'BHR',N'fr',null,'2019-02-07T14:10:54.303',N'Setup',N'Bahrein',null,0,N'117')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'880',2170,'2019-02-07T14:11:17.113',N'Setup',0,1,1,N'BD',N'BGD',N'fr',null,'2019-02-07T14:11:17.113',N'Setup',N'Bangladesh',null,0,N'118')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1246',2171,'2019-02-07T14:11:29.363',N'Setup',0,1,1,N'BB',N'BRB',N'fr',null,'2019-02-07T14:11:29.363',N'Setup',N'Barbade',null,0,N'119')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'375',2172,'2019-02-07T14:11:39.953',N'Setup',0,1,1,N'BY',N'BLR',N'fr',null,'2019-02-07T14:11:39.953',N'Setup',N'Biélorussie',null,0,N'120')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'32',2173,'2019-02-07T14:11:49.343',N'Setup',0,1,1,N'BE',N'BEL',N'fr',null,'2019-02-07T14:11:49.343',N'Setup',N'Belgique',null,0,N'121')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'501',2174,'2019-02-07T14:11:59.367',N'Setup',0,1,1,N'BZ',N'BLZ',N'fr',null,'2019-02-07T14:11:59.367',N'Setup',N'Belize',null,0,N'122')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'229',2175,'2019-02-07T14:12:09.943',N'Setup',0,1,1,N'BJ',N'BEN',N'fr',null,'2019-02-07T14:12:09.943',N'Setup',N'Bénin',null,0,N'123')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1441',2176,'2019-02-07T14:12:23.107',N'Setup',0,1,1,N'BM',N'BMU',N'fr',null,'2019-02-07T14:12:23.107',N'Setup',N'Bermudes',null,0,N'124')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'975',2177,'2019-02-07T14:12:32.850',N'Setup',0,1,1,N'BT',N'BTN',N'fr',null,'2019-02-07T14:12:32.850',N'Setup',N'Bhoutan',null,0,N'125')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'591',2178,'2019-02-07T14:12:45.570',N'Setup',0,1,1,N'BO',N'BOL',N'fr',null,'2019-02-07T14:12:45.570',N'Setup',N'Bolivie',null,0,N'126')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'387',2179,'2019-02-07T14:12:55.660',N'Setup',0,1,1,N'BA',N'BIH',N'fr',null,'2019-02-07T14:12:55.660',N'Setup',N'Bosnie Herzégovine',null,0,N'127')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'267',2180,'2019-02-07T14:13:05.903',N'Setup',0,1,1,N'BW',N'BWA',N'fr',null,'2019-02-07T14:13:05.903',N'Setup',N'Botswana',null,0,N'128')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2181,'2019-02-07T14:13:15.047',N'Setup',0,1,1,N'BV',N'BVT',N'fr',null,'2019-02-07T14:13:15.047',N'Setup',N'Bouvet Island',null,0,N'129')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'55',2182,'2019-02-07T14:13:26.263',N'Setup',0,1,1,N'BR',N'BRA',N'fr',null,'2019-02-07T14:13:26.263',N'Setup',N'Brésil',null,0,N'130')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'246',2183,'2019-02-07T14:13:36.530',N'Setup',0,1,1,N'IO',N'IOT',N'fr',null,'2019-02-07T14:13:36.530',N'Setup',N'Territoire britannique de l''océan Indien',null,0,N'131')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'673',2184,'2019-02-07T14:13:47.717',N'Setup',0,1,1,N'BN',N'BRN',N'fr',null,'2019-02-07T14:13:47.717',N'Setup',N'Brunei Darussalam',null,0,N'132')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'359',2185,'2019-02-07T14:13:56.403',N'Setup',0,1,1,N'BG',N'BGR',N'fr',null,'2019-02-07T14:13:56.403',N'Setup',N'Bulgarie',null,0,N'133')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'226',2186,'2019-02-07T14:14:07.333',N'Setup',0,1,1,N'BF',N'BFA',N'fr',null,'2019-02-07T14:14:07.333',N'Setup',N'Burkina Faso',null,0,N'134')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'257',2187,'2019-02-07T14:14:17.817',N'Setup',0,1,1,N'BI',N'BDI',N'fr',null,'2019-02-07T14:14:17.817',N'Setup',N'Burundi',null,0,N'135')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'855',2188,'2019-02-07T14:14:26.260',N'Setup',0,1,1,N'KH',N'KHM',N'fr',null,'2019-02-07T14:14:26.260',N'Setup',N'Cambodge',null,0,N'136')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'237',2189,'2019-02-07T14:14:36.573',N'Setup',0,1,1,N'CM',N'CMR',N'fr',null,'2019-02-07T14:14:36.573',N'Setup',N'Cameroun',null,0,N'137')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',2190,'2019-02-07T14:14:46.920',N'Setup',0,1,1,N'CA',N'CAN',N'fr',null,'2019-02-07T14:14:46.920',N'Setup',N'Canada',null,0,N'138')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'238',2191,'2019-02-07T14:14:56.517',N'Setup',0,1,1,N'CV',N'CPV',N'fr',null,'2019-02-07T14:14:56.517',N'Setup',N'Cap-Vert',null,0,N'139')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1345',2192,'2019-02-07T14:15:06.030',N'Setup',0,1,1,N'KY',N'CYM',N'fr',null,'2019-02-07T14:15:06.030',N'Setup',N'Îles Caïmans',null,0,N'140')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'236',2193,'2019-02-07T14:15:15.767',N'Setup',0,1,1,N'CF',N'CAF',N'fr',null,'2019-02-07T14:15:15.767',N'Setup',N'République centrafricaine',null,0,N'141')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'235',2194,'2019-02-07T14:15:24.963',N'Setup',0,1,1,N'TD',N'TCD',N'fr',null,'2019-02-07T14:15:24.963',N'Setup',N'Tchad',null,0,N'142')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'56',2195,'2019-02-07T14:15:33.857',N'Setup',0,1,1,N'CL',N'CHL',N'fr',null,'2019-02-07T14:15:33.857',N'Setup',N'Chili',null,0,N'143')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'86',2196,'2019-02-07T14:15:39.113',N'Setup',0,1,1,N'CN',N'CHN',N'fr',null,'2019-02-07T14:15:39.113',N'Setup',N'Chine',null,0,N'144')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2197,'2019-02-07T14:15:48.800',N'Setup',0,1,1,N'CX',N'CXR',N'fr',null,'2019-02-07T14:15:48.800',N'Setup',N'L''île de noël',null,0,N'145')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2198,'2019-02-07T14:15:58.840',N'Setup',0,1,1,N'CC',N'CCK',N'fr',null,'2019-02-07T14:15:58.840',N'Setup',N'Îles Cocos (Keeling)',null,0,N'146')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'57',2199,'2019-02-07T14:16:08.033',N'Setup',0,1,1,N'CO',N'COL',N'fr',null,'2019-02-07T14:16:08.033',N'Setup',N'Colombie',null,0,N'147')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'269',2200,'2019-02-07T14:16:18.637',N'Setup',0,1,1,N'KM',N'COM',N'fr',null,'2019-02-07T14:16:18.637',N'Setup',N'Les Comores',null,0,N'148')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'242',2201,'2019-02-07T14:16:28.093',N'Setup',0,1,1,N'CG',N'COG',N'fr',null,'2019-02-07T14:16:28.093',N'Setup',N'Congo',null,0,N'149')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'243',2202,'2019-02-07T14:16:37.017',N'Setup',0,1,1,N'CD',N'COD',N'fr',null,'2019-02-07T14:16:37.017',N'Setup',N'La République Démocratique du Congo',null,0,N'150')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'682',2203,'2019-02-07T14:16:46.877',N'Setup',0,1,1,N'CK',N'COK',N'fr',null,'2019-02-07T14:16:46.877',N'Setup',N'les Îles Cook',null,0,N'151')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'506',2204,'2019-02-07T14:16:55.687',N'Setup',0,1,1,N'CR',N'CRI',N'fr',null,'2019-02-07T14:16:55.687',N'Setup',N'Costa Rica',null,0,N'152')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'225',2205,'2019-02-07T14:17:05.577',N'Setup',0,1,1,N'CI',N'CIV',N'fr',null,'2019-02-07T14:17:05.577',N'Setup',N'Côte d''Ivoire',null,0,N'153')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'385',2206,'2019-02-07T14:17:17.140',N'Setup',0,1,1,N'HR',N'HRV',N'fr',null,'2019-02-07T14:17:17.140',N'Setup',N'Croatie',null,0,N'154')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'53',2207,'2019-02-07T14:17:25.247',N'Setup',0,1,1,N'CU',N'CUB',N'fr',null,'2019-02-07T14:17:25.247',N'Setup',N'Cuba',null,0,N'155')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'357',2208,'2019-02-07T14:17:36.700',N'Setup',0,1,1,N'CY',N'CYP',N'fr',null,'2019-02-07T14:17:36.700',N'Setup',N'Chypre',null,0,N'156')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'420',2209,'2019-02-07T14:17:44.713',N'Setup',0,1,1,N'CZ',N'CZE',N'fr',null,'2019-02-07T14:17:44.713',N'Setup',N'République Tchèque',null,0,N'157')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'45',2210,'2019-02-07T14:17:54.310',N'Setup',0,1,1,N'DK',N'DNK',N'fr',null,'2019-02-07T14:17:54.310',N'Setup',N'Danemark',null,0,N'158')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'253',2211,'2019-02-07T14:18:02.900',N'Setup',0,1,1,N'DJ',N'DJI',N'fr',null,'2019-02-07T14:18:02.900',N'Setup',N'Djibouti',null,0,N'159')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1767',2212,'2019-02-07T14:18:14.127',N'Setup',0,1,1,N'DM',N'DMA',N'fr',null,'2019-02-07T14:18:14.127',N'Setup',N'La Dominique',null,0,N'160')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1809',2213,'2019-02-07T14:18:28.470',N'Setup',0,1,1,N'DO',N'DOM',N'fr',null,'2019-02-07T14:18:28.470',N'Setup',N'République Dominicaine',null,0,N'161')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'593',2214,'2019-02-07T14:22:11.347',N'Setup',0,1,1,N'EC',N'ECU',N'fr',null,'2019-02-07T14:22:11.347',N'Setup',N'Équateur',null,0,N'162')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'20',2215,'2019-02-07T14:22:21.247',N'Setup',0,1,1,N'EG',N'EGY',N'fr',null,'2019-02-07T14:22:21.247',N'Setup',N'Egypte',null,0,N'163')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'503',2216,'2019-02-07T14:22:29.763',N'Setup',0,1,1,N'SV',N'SLV',N'fr',null,'2019-02-07T14:22:29.763',N'Setup',N'Le Salvador',null,0,N'164')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'240',2217,'2019-02-07T14:22:40.567',N'Setup',0,1,1,N'GQ',N'GNQ',N'fr',null,'2019-02-07T14:22:40.567',N'Setup',N'Guinée Équatoriale',null,0,N'165')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'291',2218,'2019-02-07T14:22:49.467',N'Setup',0,1,1,N'ER',N'ERI',N'fr',null,'2019-02-07T14:22:49.467',N'Setup',N'Erythrée',null,0,N'166')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'372',2219,'2019-02-07T14:22:59.570',N'Setup',0,1,1,N'EE',N'EST',N'fr',null,'2019-02-07T14:22:59.570',N'Setup',N'Estonie',null,0,N'167')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'251',2220,'2019-02-07T14:23:09.137',N'Setup',0,1,1,N'ET',N'ETH',N'fr',null,'2019-02-07T14:23:09.137',N'Setup',N'Ethiopie',null,0,N'168')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'500',2221,'2019-02-07T14:23:19.530',N'Setup',0,1,1,N'FK',N'FLK',N'fr',null,'2019-02-07T14:23:19.530',N'Setup',N'Îles Falkland (Malvinas)',null,0,N'169')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'298',2222,'2019-02-07T14:23:29.263',N'Setup',0,1,1,N'FO',N'FRO',N'fr',null,'2019-02-07T14:23:29.263',N'Setup',N'Îles Féroé',null,0,N'170')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'679',2223,'2019-02-07T14:23:37.750',N'Setup',0,1,1,N'FJ',N'FJI',N'fr',null,'2019-02-07T14:23:37.750',N'Setup',N'Fidji',null,0,N'171')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'358',2224,'2019-02-07T14:23:46.413',N'Setup',0,1,1,N'FI',N'FIN',N'fr',null,'2019-02-07T14:23:46.413',N'Setup',N'Finlande',null,0,N'172')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'33',2225,'2019-02-07T14:23:55.470',N'Setup',1,1,1,N'FR',N'FRA',N'fr',null,'2019-02-07T14:23:55.470',N'Setup',N'France',null,4,N'173')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'594',2226,'2019-02-07T14:24:04.037',N'Setup',0,1,1,N'GF',N'GUF',N'fr',null,'2019-02-07T14:24:04.037',N'Setup',N'Guinée Française',null,0,N'174')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'689',2227,'2019-02-07T14:24:14.157',N'Setup',0,1,1,N'PF',N'PYF',N'fr',null,'2019-02-07T14:24:14.157',N'Setup',N'Polynésie française',null,0,N'175')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2228,'2019-02-07T14:24:26.537',N'Setup',0,1,1,N'TF',N'ATF',N'fr',null,'2019-02-07T14:24:26.537',N'Setup',N'Terres Australes Françaises',null,0,N'176')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'241',2229,'2019-02-07T14:24:35.730',N'Setup',0,1,1,N'GA',N'GAB',N'fr',null,'2019-02-07T14:24:35.730',N'Setup',N'Gabon',null,0,N'177')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'220',2230,'2019-02-07T14:24:43.773',N'Setup',0,1,1,N'GM',N'GMB',N'fr',null,'2019-02-07T14:24:43.773',N'Setup',N'Gambie',null,0,N'178')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'995',2231,'2019-02-07T14:24:52.090',N'Setup',0,1,1,N'GE',N'GEO',N'fr',null,'2019-02-07T14:24:52.090',N'Setup',N'Géorgie',null,0,N'179')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'233',2232,'2019-02-07T14:25:00.730',N'Setup',0,1,1,N'GH',N'GHA',N'fr',null,'2019-02-07T14:25:00.730',N'Setup',N'Ghana',null,0,N'180')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'350',2233,'2019-02-07T14:25:09.643',N'Setup',0,1,1,N'GI',N'GIB',N'fr',null,'2019-02-07T14:25:09.643',N'Setup',N'Gibraltar',null,0,N'181')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'30',2234,'2019-02-07T14:25:18.600',N'Setup',0,1,1,N'GR',N'GRC',N'fr',null,'2019-02-07T14:25:18.600',N'Setup',N'Grèce',null,0,N'182')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'299',2235,'2019-02-07T14:25:28.007',N'Setup',0,1,1,N'GL',N'GRL',N'fr',null,'2019-02-07T14:25:28.007',N'Setup',N'Groenland',null,0,N'183')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1473',2236,'2019-02-07T14:25:38.463',N'Setup',0,1,1,N'GD',N'GRD',N'fr',null,'2019-02-07T14:25:38.463',N'Setup',N'Grenade',null,0,N'184')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'590',2237,'2019-02-07T14:25:46.963',N'Setup',0,1,1,N'GP',N'GLP',N'fr',null,'2019-02-07T14:25:46.963',N'Setup',N'Guadeloupe',null,0,N'185')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1671',2238,'2019-02-07T14:25:55.183',N'Setup',0,1,1,N'GU',N'GUM',N'fr',null,'2019-02-07T14:25:55.183',N'Setup',N'Guam',null,0,N'186')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'502',2239,'2019-02-07T14:26:03.560',N'Setup',0,1,1,N'GT',N'GTM',N'fr',null,'2019-02-07T14:26:03.560',N'Setup',N'Guatemala',null,0,N'187')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'224',2240,'2019-02-07T14:26:12.813',N'Setup',0,1,1,N'GN',N'GIN',N'fr',null,'2019-02-07T14:26:12.813',N'Setup',N'Guinée',null,0,N'188')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'245',2241,'2019-02-07T14:26:22.300',N'Setup',0,1,1,N'GW',N'GNB',N'fr',null,'2019-02-07T14:26:22.300',N'Setup',N'Guinée Bissau',null,0,N'189')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'592',2242,'2019-02-07T14:26:32.077',N'Setup',0,1,1,N'GY',N'GUY',N'fr',null,'2019-02-07T14:26:32.077',N'Setup',N'Guyane',null,0,N'190')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'509',2243,'2019-02-07T14:26:41.690',N'Setup',0,1,1,N'HT',N'HTI',N'fr',null,'2019-02-07T14:26:41.690',N'Setup',N'Haïti',null,0,N'191')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2244,'2019-02-07T14:26:51.220',N'Setup',0,1,1,N'HM',N'HMD',N'fr',null,'2019-02-07T14:26:51.220',N'Setup',N'Heard Island et les îles Mcdonald',null,0,N'192')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',2245,'2019-02-07T14:27:00.290',N'Setup',0,1,1,N'VA',N'VAT',N'fr',null,'2019-02-07T14:27:00.290',N'Setup',N'Saint-Siège (État de la cité du Vatican)',null,0,N'193')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'504',2246,'2019-02-07T14:27:14.797',N'Setup',0,1,1,N'HN',N'HND',N'fr',null,'2019-02-07T14:27:14.797',N'Setup',N'Honduras',null,0,N'194')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'852',2247,'2019-02-07T14:27:27.277',N'Setup',0,1,1,N'HK',N'HKG',N'fr',null,'2019-02-07T14:27:31.890',N'Setup',N'Hong Kong',null,0,N'195')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'36',2248,'2019-02-07T14:27:41.263',N'Setup',0,1,1,N'HU',N'HUN',N'fr',null,'2019-02-07T14:27:41.263',N'Setup',N'Hongrie',null,0,N'196')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'354',2249,'2019-02-07T14:27:50.080',N'Setup',0,1,1,N'IS',N'ISL',N'fr',null,'2019-02-07T14:27:50.080',N'Setup',N'Islande',null,0,N'197')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'91',2250,'2019-02-07T14:28:01.617',N'Setup',0,1,1,N'IN',N'IND',N'fr',null,'2019-02-07T14:28:01.617',N'Setup',N'Inde',null,0,N'198')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'62',2251,'2019-02-07T14:28:10.807',N'Setup',0,1,1,N'ID',N'IDN',N'fr',null,'2019-02-07T14:28:10.807',N'Setup',N'Indonésie',null,0,N'199')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2252,'2019-02-07T14:28:20.807',N'Setup',0,1,1,N'IR',N'IRN',N'fr',null,'2019-02-07T14:28:20.807',N'Setup',N'République islamique d''Iran',null,0,N'200')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'964',2253,'2019-02-07T14:28:30.517',N'Setup',0,1,1,N'IQ',N'IRQ',N'fr',null,'2019-02-07T14:28:30.517',N'Setup',N'Irak',null,0,N'201')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'353',2254,'2019-02-07T14:28:39.383',N'Setup',0,1,1,N'IE',N'IRL',N'fr',null,'2019-02-07T14:28:39.383',N'Setup',N'Irlande',null,0,N'202')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',2255,'2019-02-07T14:29:00.303',N'Setup',1,1,1,N'IT',N'ITA',N'fr',null,'2019-02-07T14:29:20.850',N'Setup',N'Italie',null,5,N'204')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1876',2256,'2019-02-07T14:29:30.140',N'Setup',0,1,1,N'JM',N'JAM',N'fr',null,'2019-02-07T14:29:30.140',N'Setup',N'Jamaïque',null,0,N'205')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'81',2257,'2019-02-07T14:29:39.820',N'Setup',0,1,1,N'JP',N'JPN',N'fr',null,'2019-02-07T14:29:39.820',N'Setup',N'Japon',null,0,N'206')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'962',2258,'2019-02-07T14:29:47.567',N'Setup',0,1,1,N'JO',N'JOR',N'fr',null,'2019-02-07T14:29:47.567',N'Setup',N'Jordan',null,0,N'207')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',2259,'2019-02-07T14:29:58.743',N'Setup',0,1,1,N'KZ',N'KAZ',N'fr',null,'2019-02-07T14:29:58.743',N'Setup',N'Kazakhstan',null,0,N'208')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'254',2260,'2019-02-07T14:30:06.950',N'Setup',0,1,1,N'KE',N'KEN',N'fr',null,'2019-02-07T14:30:06.950',N'Setup',N'Kenya',null,0,N'209')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'686',2261,'2019-02-07T14:30:15.737',N'Setup',0,1,1,N'KI',N'KIR',N'fr',null,'2019-02-07T14:30:15.737',N'Setup',N'Kiribati',null,0,N'210')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2262,'2019-02-07T14:30:26.013',N'Setup',0,1,1,N'KP',N'PRK',N'fr',null,'2019-02-07T14:30:26.013',N'Setup',N'République Populaire Démocratique de Corée',null,0,N'211')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'850',2263,'2019-02-07T14:30:41.560',N'Setup',0,1,1,N'KR',N'KOR',N'fr',null,'2019-02-07T14:30:41.560',N'Setup',N'République de Corée',null,0,N'212')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'965',2264,'2019-02-07T14:30:51.740',N'Setup',0,1,1,N'KW',N'KWT',N'fr',null,'2019-02-07T14:30:51.740',N'Setup',N'Koweit',null,0,N'213')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'996',2265,'2019-02-07T14:31:02.507',N'Setup',0,1,1,N'KG',N'KGZ',N'fr',null,'2019-02-07T14:31:02.507',N'Setup',N'Kirghizistan',null,0,N'214')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2266,'2019-02-07T14:31:11.653',N'Setup',0,1,1,N'',N'',N'fr',null,'2019-02-07T14:31:11.653',N'Setup',N'République démocratique populaire lao',null,0,N'215')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'371',2267,'2019-02-07T14:31:20.340',N'Setup',0,1,1,N'LV',N'LVA',N'fr',null,'2019-02-07T14:31:20.340',N'Setup',N'Lettonie',null,0,N'216')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'961',2268,'2019-02-07T14:31:30.400',N'Setup',0,1,1,N'LB',N'LBN',N'fr',null,'2019-02-07T14:31:30.400',N'Setup',N'Liban',null,0,N'217')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'266',2269,'2019-02-07T14:31:40.063',N'Setup',0,1,1,N'LS',N'LSO',N'fr',null,'2019-02-07T14:31:40.063',N'Setup',N'Lesotho',null,0,N'218')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'231',2270,'2019-02-07T14:31:47.930',N'Setup',0,1,1,N'LR',N'LBR',N'fr',null,'2019-02-07T14:31:47.930',N'Setup',N'Libéria',null,0,N'219')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'218',2271,'2019-02-07T14:31:57.243',N'Setup',0,1,1,N'LY',N'LBY',N'fr',null,'2019-02-07T14:31:57.243',N'Setup',N'Jamahiriya Arabe Libyenne',null,0,N'220')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'423',2272,'2019-02-07T14:32:05.523',N'Setup',0,1,1,N'LI',N'LIE',N'fr',null,'2019-02-07T14:32:05.523',N'Setup',N'Liechtenstein',null,0,N'221')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'370',2273,'2019-02-07T14:32:13.893',N'Setup',0,1,1,N'LT',N'LTU',N'fr',null,'2019-02-07T14:32:13.893',N'Setup',N'Lituanie',null,0,N'222')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'352',2274,'2019-02-07T14:32:22.237',N'Setup',0,1,1,N'LU',N'LUX',N'fr',null,'2019-02-07T14:32:22.237',N'Setup',N'Luxembourg',null,0,N'223')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'853',2275,'2019-02-07T14:32:31.293',N'Setup',0,1,1,N'MO',N'MAC',N'fr',null,'2019-02-07T14:32:31.293',N'Setup',N'Macao',null,0,N'224')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2276,'2019-02-07T14:32:42.247',N'Setup',0,1,1,N'MK',N'MKD',N'fr',null,'2019-02-07T14:32:42.247',N'Setup',N'L''ancienne république Yugoslave de Macédonie',null,0,N'225')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'261',2277,'2019-02-07T14:32:51.247',N'Setup',0,1,1,N'MG',N'MDG',N'fr',null,'2019-02-07T14:32:51.247',N'Setup',N'Madagascar',null,0,N'226')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'265',2278,'2019-02-07T14:33:00.983',N'Setup',0,1,1,N'MW',N'MWI',N'fr',null,'2019-02-07T14:33:00.983',N'Setup',N'Malawi',null,0,N'227')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'60',2279,'2019-02-07T14:33:10.870',N'Setup',0,1,1,N'MY',N'MYS',N'fr',null,'2019-02-07T14:33:10.870',N'Setup',N'Malaisie',null,0,N'228')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'960',2280,'2019-02-07T14:33:20.207',N'Setup',0,1,1,N'MV',N'MDV',N'fr',null,'2019-02-07T14:33:20.207',N'Setup',N'Maldives',null,0,N'229')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'223',2281,'2019-02-07T14:33:30.603',N'Setup',0,1,1,N'ML',N'MLI',N'fr',null,'2019-02-07T14:33:30.603',N'Setup',N'Mali',null,0,N'230')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'356',2282,'2019-02-07T14:33:45.197',N'Setup',0,1,1,N'MT',N'MLT',N'fr',null,'2019-02-07T14:33:45.197',N'Setup',N'Malte',null,0,N'231')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'692',2283,'2019-02-07T14:33:55.387',N'Setup',0,1,1,N'MH',N'MHL',N'fr',null,'2019-02-07T14:33:55.387',N'Setup',N'Iles Marshall',null,0,N'232')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'596',2284,'2019-02-07T14:34:04.493',N'Setup',0,1,1,N'MQ',N'MTQ',N'fr',null,'2019-02-07T14:34:04.493',N'Setup',N'Martinique',null,0,N'233')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'222',2285,'2019-02-07T14:34:14.543',N'Setup',0,1,1,N'MR',N'MRT',N'fr',null,'2019-02-07T14:34:14.543',N'Setup',N'Mauritanie',null,0,N'234')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'230',2286,'2019-02-07T14:34:23.260',N'Setup',0,1,1,N'MU',N'MUS',N'fr',null,'2019-02-07T14:34:23.260',N'Setup',N'Maurice',null,0,N'235')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',2287,'2019-02-07T14:34:37.897',N'Setup',0,1,1,N'YT',N'MYT',N'fr',null,'2019-02-07T14:34:37.897',N'Setup',N'Mayotte',null,0,N'236')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'52',2288,'2019-02-07T14:34:46.487',N'Setup',0,1,1,N'MX',N'MEX',N'fr',null,'2019-02-07T14:34:46.487',N'Setup',N'Mexique',null,0,N'237')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2289,'2019-02-07T14:34:58.323',N'Setup',0,1,1,N'FM',N'FSM',N'fr',null,'2019-02-07T14:34:58.323',N'Setup',N'États fédérés de Micronésie',null,0,N'238')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2290,'2019-02-07T14:35:09.983',N'Setup',0,1,1,N'MD',N'MDA',N'fr',null,'2019-02-07T14:35:09.983',N'Setup',N'République de Moldavie',null,0,N'239')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'377',2291,'2019-02-07T14:35:18.533',N'Setup',0,1,1,N'MC',N'MCO',N'fr',null,'2019-02-07T14:35:18.533',N'Setup',N'Monaco',null,0,N'240')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'976',2292,'2019-02-07T14:35:28.473',N'Setup',0,1,1,N'MN',N'MNG',N'fr',null,'2019-02-07T14:35:28.473',N'Setup',N'Mongolie',null,0,N'241')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1664',2293,'2019-02-07T14:35:37.650',N'Setup',0,1,1,N'MS',N'MSR',N'fr',null,'2019-02-07T14:35:37.650',N'Setup',N'Montserrat',null,0,N'242')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',2294,'2019-02-07T14:35:47.100',N'Setup',0,1,1,N'MA',N'MAR',N'fr',null,'2019-02-07T14:35:47.100',N'Setup',N'Maroc',null,0,N'243')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'258',2295,'2019-02-07T14:35:55.693',N'Setup',0,1,1,N'MZ',N'MOZ',N'fr',null,'2019-02-07T14:35:55.693',N'Setup',N'Mozambique',null,0,N'244')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'95',2296,'2019-02-07T14:36:04.310',N'Setup',0,1,1,N'MM',N'MMR',N'fr',null,'2019-02-07T14:36:04.310',N'Setup',N'Myanmar',null,0,N'245')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'264',2297,'2019-02-07T14:36:13.523',N'Setup',0,1,1,N'NA',N'NAM',N'fr',null,'2019-02-07T14:36:13.523',N'Setup',N'Namibie',null,0,N'246')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'674',2298,'2019-02-07T14:36:22.007',N'Setup',0,1,1,N'NR',N'NRU',N'fr',null,'2019-02-07T14:36:22.007',N'Setup',N'Nauru',null,0,N'247')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'977',2299,'2019-02-07T14:36:30.240',N'Setup',0,1,1,N'NP',N'NPL',N'fr',null,'2019-02-07T14:36:30.240',N'Setup',N'Népal',null,0,N'248')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'31',2300,'2019-02-07T14:36:39.183',N'Setup',0,1,1,N'NL',N'NLD',N'fr',null,'2019-02-07T14:36:39.183',N'Setup',N'Pays-Bas',null,0,N'249')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'599',2301,'2019-02-07T14:36:49.443',N'Setup',0,1,1,N'AN',N'ANT',N'fr',null,'2019-02-07T14:36:49.443',N'Setup',N'Antilles néerlandaises',null,0,N'250')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'687',2302,'2019-02-07T14:36:59.940',N'Setup',0,1,1,N'NC',N'NCL',N'fr',null,'2019-02-07T14:36:59.940',N'Setup',N'Nouvelle Calédonie',null,0,N'251')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'64',2303,'2019-02-07T14:37:08.953',N'Setup',0,1,1,N'NZ',N'NZL',N'fr',null,'2019-02-07T14:37:08.953',N'Setup',N'Nouvelle-Zélande',null,0,N'252')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'505',2304,'2019-02-07T14:37:19.120',N'Setup',0,1,1,N'NI',N'NIC',N'fr',null,'2019-02-07T14:37:19.120',N'Setup',N'Nicaragua',null,0,N'253')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'227',2305,'2019-02-07T14:37:29.467',N'Setup',0,1,1,N'NE',N'NER',N'fr',null,'2019-02-07T14:37:29.467',N'Setup',N'Niger',null,0,N'254')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'234',2306,'2019-02-07T14:37:38.567',N'Setup',0,1,1,N'NG',N'NGA',N'fr',null,'2019-02-07T14:37:38.567',N'Setup',N'Nigeria',null,0,N'255')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'683',2307,'2019-02-07T14:37:48.813',N'Setup',0,1,1,N'NU',N'NIU',N'fr',null,'2019-02-07T14:37:48.813',N'Setup',N'Niue',null,0,N'256')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6723',2308,'2019-02-07T14:37:58.810',N'Setup',0,1,1,N'NF',N'NFK',N'fr',null,'2019-02-07T14:37:58.810',N'Setup',N'l''ile de Norfolk',null,0,N'257')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1670',2309,'2019-02-07T14:38:08.013',N'Setup',0,1,1,N'MP',N'MNP',N'fr',null,'2019-02-07T14:38:08.013',N'Setup',N'Îles Mariannes du Nord',null,0,N'258')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',2310,'2019-02-07T14:38:17.760',N'Setup',0,1,1,N'NO',N'NOR',N'fr',null,'2019-02-07T14:38:17.760',N'Setup',N'Norvège',null,0,N'259')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'968',2311,'2019-02-07T14:38:28.173',N'Setup',0,1,1,N'OM',N'OMN',N'fr',null,'2019-02-07T14:38:28.173',N'Setup',N'Oman',null,0,N'260')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'92',2312,'2019-02-07T14:38:36.123',N'Setup',0,1,1,N'PK',N'PAK',N'fr',null,'2019-02-07T14:38:36.123',N'Setup',N'Pakistan',null,0,N'261')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'680',2313,'2019-02-07T14:38:49.343',N'Setup',0,1,1,N'PW',N'PLW',N'fr',null,'2019-02-07T14:38:49.343',N'Setup',N'Palau',null,0,N'262')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2314,'2019-02-07T14:38:59.730',N'Setup',0,1,1,N'PS',N'PSE',N'fr',null,'2019-02-07T14:38:59.730',N'Setup',N'Territoire palestinien occupé',null,0,N'263')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'507',2315,'2019-02-07T14:39:12.860',N'Setup',0,1,1,N'PA',N'PAN',N'fr',null,'2019-02-07T14:39:12.860',N'Setup',N'Panama',null,0,N'264')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'675',2316,'2019-02-07T14:39:22.737',N'Setup',0,1,1,N'PG',N'PNG',N'fr',null,'2019-02-07T14:39:22.737',N'Setup',N'Papouasie Nouvelle Guinée',null,0,N'265')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'595',2317,'2019-02-07T14:39:32.127',N'Setup',0,1,1,N'PY',N'PRY',N'fr',null,'2019-02-07T14:41:47.407',N'Setup',N'Paraguay',null,0,N'266')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'51',2318,'2019-02-07T14:39:44.057',N'Setup',0,1,1,N'PE',N'PER',N'fr',null,'2019-02-07T14:39:44.057',N'Setup',N'Pérou',null,0,N'267')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'63',2319,'2019-02-07T14:39:52.610',N'Setup',0,1,1,N'PH',N'PHL',N'fr',null,'2019-02-07T14:39:52.610',N'Setup',N'Philippines',null,0,N'268')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2320,'2019-02-07T14:40:01.497',N'Setup',0,1,1,N'PN',N'PCN',N'fr',null,'2019-02-07T14:40:01.497',N'Setup',N'Pitcairn',null,0,N'269')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'48',2321,'2019-02-07T14:40:09.923',N'Setup',0,1,1,N'PL',N'POL',N'fr',null,'2019-02-07T14:40:09.923',N'Setup',N'Pologne',null,0,N'270')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'351',2322,'2019-02-07T14:40:23.150',N'Setup',0,1,1,N'PT',N'PRT',N'fr',null,'2019-02-07T14:41:53.740',N'Setup',N'Portugal',null,0,N'271')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1787',2323,'2019-02-07T14:40:34.823',N'Setup',0,1,1,N'PR',N'PRI',N'fr',null,'2019-02-07T14:41:08.247',N'Setup',N'Porto Rico',null,0,N'272')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'974',2324,'2019-02-07T14:40:48.717',N'Setup',0,1,1,N'QA',N'QAT',N'fr',null,'2019-02-07T14:41:09.820',N'Setup',N'Qatar',null,0,N'273')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',2325,'2019-02-07T14:41:01.643',N'Setup',0,1,1,N'RE',N'REU',N'fr',null,'2019-02-07T14:41:11.723',N'Setup',N'La Réunion',null,0,N'274')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'40',2326,'2019-02-07T14:41:21.393',N'Setup',0,1,1,N'RO',N'ROU',N'fr',null,'2019-02-07T14:41:57.863',N'Setup',N'Roumanie',null,0,N'275')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',2327,'2019-02-07T14:41:30.453',N'Setup',0,1,1,N'RU',N'RUS',N'fr',null,'2019-02-07T14:41:59.457',N'Setup',N'Fédération Russe',null,0,N'276')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'250',2328,'2019-02-07T14:42:08.870',N'Setup',0,1,1,N'RW',N'RWA',N'fr',null,'2019-02-07T14:42:08.870',N'Setup',N'Rwanda',null,0,N'277')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'290',2329,'2019-02-07T14:42:24.283',N'Setup',0,1,1,N'SH',N'SHN',N'fr',null,'2019-02-07T14:42:24.283',N'Setup',N'Sainte Hélène',null,0,N'278')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1869',2330,'2019-02-07T14:42:34.723',N'Setup',0,1,1,N'KN',N'KNA',N'fr',null,'2019-02-07T14:42:34.723',N'Setup',N'Saint-Christophe-et-Niévès',null,0,N'279')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1758',2331,'2019-02-07T14:42:44',N'Setup',0,1,1,N'LC',N'LCA',N'fr',null,'2019-02-07T14:42:44',N'Setup',N'Sainte-Lucie',null,0,N'280')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'508',2332,'2019-02-07T14:42:54.423',N'Setup',0,1,1,N'PM',N'SPM',N'fr',null,'2019-02-07T14:42:54.423',N'Setup',N'Saint Pierre et Miquelon',null,0,N'281')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1784',2333,'2019-02-07T14:43:05.553',N'Setup',0,1,1,N'VC',N'VCT',N'fr',null,'2019-02-07T14:43:05.553',N'Setup',N'Saint-Vincent-et-les-Grenadines',null,0,N'282')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'685',2334,'2019-02-07T14:43:14.997',N'Setup',0,1,1,N'WS',N'WSM',N'fr',null,'2019-02-07T14:43:14.997',N'Setup',N'Samoa',null,0,N'283')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'378',2335,'2019-02-07T14:43:26.787',N'Setup',0,1,1,N'SM',N'SMR',N'fr',null,'2019-02-07T14:43:26.787',N'Setup',N'Saint Marin',null,0,N'284')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'239',2336,'2019-02-07T14:43:38.900',N'Setup',0,1,1,N'ST',N'STP',N'fr',null,'2019-02-07T14:43:38.900',N'Setup',N'Sao Tomé et Principe',null,0,N'285')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'966',2337,'2019-02-07T14:43:48.853',N'Setup',0,1,1,N'SA',N'SAU',N'fr',null,'2019-02-07T14:43:48.853',N'Setup',N'Arabie Saoudite',null,0,N'286')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'221',2338,'2019-02-07T14:43:58.403',N'Setup',0,1,1,N'SN',N'SEN',N'fr',null,'2019-02-07T14:43:58.403',N'Setup',N'Sénégal',null,0,N'287')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2339,'2019-02-07T14:45:59.677',N'Setup',0,1,1,N'',N'',N'fr',null,'2019-02-07T14:45:59.677',N'Setup',N'Serbie et Monténégro',null,0,N'288')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'248',2340,'2019-02-07T14:46:09.217',N'Setup',0,1,1,N'SC',N'SYC',N'fr',null,'2019-02-07T14:47:27.930',N'Setup',N'les Seychelles',null,0,N'289')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'232',2341,'2019-02-07T14:47:57.887',N'Setup',0,1,1,N'SL',N'SLE',N'fr',null,'2019-02-07T14:47:57.887',N'Setup',N'Sierra Leone',null,0,N'290')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'65',2342,'2019-02-07T14:48:07.563',N'Setup',0,1,1,N'SG',N'SGP',N'fr',null,'2019-02-07T14:48:07.563',N'Setup',N'Singapour',null,0,N'291')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'421',2343,'2019-02-07T14:48:16.523',N'Setup',0,1,1,N'SK',N'SVK',N'fr',null,'2019-02-07T14:48:16.523',N'Setup',N'Slovaquie',null,0,N'292')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'386',2344,'2019-02-07T14:49:19.943',N'Setup',0,1,1,N'SI',N'SVN',N'fr',null,'2019-02-07T14:49:19.943',N'Setup',N'Slovénie',null,0,N'293')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'677',2345,'2019-02-07T14:49:29.847',N'Setup',0,1,1,N'SB',N'SLB',N'fr',null,'2019-02-07T14:49:29.847',N'Setup',N'îles Salomon',null,0,N'294')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'252',2346,'2019-02-07T14:49:48.190',N'Setup',0,1,1,N'SO',N'SOM',N'fr',null,'2019-02-07T14:49:48.190',N'Setup',N'Somalie',null,0,N'295')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'27',2347,'2019-02-07T14:50:06.330',N'Setup',0,1,1,N'ZA',N'ZAF',N'fr',null,'2019-02-07T14:50:13.030',N'Setup',N'Afrique du Sud',null,0,N'296')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2348,'2019-02-07T14:50:21.500',N'Setup',0,1,1,N'GS',N'SGS',N'fr',null,'2019-02-07T14:50:21.500',N'Setup',N'Géorgie du Sud et les îles Sandwich du Sud',null,0,N'297')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'34',2349,'2019-02-07T14:50:53.610',N'Setup',0,1,1,N'ES',N'ESP',N'fr',null,'2019-02-07T14:50:53.610',N'Setup',N'Espagne',null,0,N'298')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'94',2350,'2019-02-07T14:51:05.493',N'Setup',0,1,1,N'LK',N'LKA',N'fr',null,'2019-02-07T14:51:05.493',N'Setup',N'Sri Lanka',null,0,N'299')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'249',2351,'2019-02-07T14:51:16.110',N'Setup',0,1,1,N'SD',N'SDN',N'fr',null,'2019-02-07T14:51:16.110',N'Setup',N'Soudan',null,0,N'300')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'597',2352,'2019-02-07T14:51:25.387',N'Setup',0,1,1,N'SR',N'SUR',N'fr',null,'2019-02-07T14:51:25.387',N'Setup',N'Suriname',null,0,N'301')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',2353,'2019-02-07T14:51:34.913',N'Setup',0,1,1,N'SJ',N'SJM',N'fr',null,'2019-02-07T14:51:34.913',N'Setup',N'Svalbard et Jan Mayen',null,0,N'302')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'268',2354,'2019-02-07T14:51:43.907',N'Setup',0,1,1,N'SZ',N'SWZ',N'fr',null,'2019-02-07T14:51:43.907',N'Setup',N'Swaziland',null,0,N'303')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'46',2355,'2019-02-07T14:51:53.720',N'Setup',0,1,1,N'SE',N'SWE',N'fr',null,'2019-02-07T14:51:53.720',N'Setup',N'Suède',null,0,N'304')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'41',2356,'2019-02-07T14:52:02.267',N'Setup',1,1,1,N'CH',N'CHE',N'fr',null,'2019-02-07T14:52:02.267',N'Setup',N'Suisse',null,2,N'305')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'963',2357,'2019-02-07T14:52:14.267',N'Setup',0,1,1,N'SY',N'SYR',N'fr',null,'2019-02-07T14:52:14.267',N'Setup',N'République arabe syrienne',null,0,N'306')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'886',2358,'2019-02-07T14:52:23.903',N'Setup',0,1,1,N'TW',N'TWN',N'fr',null,'2019-02-07T14:52:23.903',N'Setup',N'Taiwan, Province de Chine',null,0,N'307')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'992',2359,'2019-02-07T14:52:32.943',N'Setup',0,1,1,N'TJ',N'TJK',N'fr',null,'2019-02-07T14:52:32.943',N'Setup',N'Tadjikistan',null,0,N'308')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'255',2360,'2019-02-07T14:52:41.910',N'Setup',0,1,1,N'TZ',N'TZA',N'fr',null,'2019-02-07T14:52:41.910',N'Setup',N'République-Unie de Tanzanie',null,0,N'309')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'66',2361,'2019-02-07T14:52:50.907',N'Setup',0,1,1,N'TH',N'THA',N'fr',null,'2019-02-07T14:52:50.907',N'Setup',N'Thaïlande',null,0,N'310')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'670',2362,'2019-02-07T14:53:00.283',N'Setup',0,1,1,N'TL',N'TLS',N'fr',null,'2019-02-07T14:53:00.283',N'Setup',N'Timor-Leste',null,0,N'311')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'228',2363,'2019-02-07T14:53:09.940',N'Setup',0,1,1,N'TG',N'TGO',N'fr',null,'2019-02-07T14:53:09.940',N'Setup',N'Togo',null,0,N'312')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'690',2364,'2019-02-07T14:53:19.657',N'Setup',0,1,1,N'TK',N'TKL',N'fr',null,'2019-02-07T14:53:19.657',N'Setup',N'Tokélaou',null,0,N'313')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'676',2365,'2019-02-07T14:53:38.850',N'Setup',0,1,1,N'TO',N'TON',N'fr',null,'2019-02-07T14:53:38.850',N'Setup',N'Tonga',null,0,N'314')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1868',2366,'2019-02-07T14:53:50.167',N'Setup',0,1,1,N'TT',N'TTO',N'fr',null,'2019-02-07T14:53:50.167',N'Setup',N'Trinité-et-Tobago',null,0,N'315')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'216',2367,'2019-02-07T14:53:55.840',N'Setup',0,1,1,N'TN',N'TUN',N'fr',null,'2019-02-07T14:53:55.840',N'Setup',N'Tunisie',null,0,N'316')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'90',2368,'2019-02-07T14:54:12.390',N'Setup',0,1,1,N'TR',N'TUR',N'fr',null,'2019-02-07T14:54:12.390',N'Setup',N'Turquie',null,0,N'317')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'993',2369,'2019-02-07T14:54:21.763',N'Setup',0,1,1,N'TM',N'TKM',N'fr',null,'2019-02-07T14:54:21.763',N'Setup',N'Turkménistan',null,0,N'318')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1649',2370,'2019-02-07T14:54:30.467',N'Setup',0,1,1,N'TC',N'TCA',N'fr',null,'2019-02-07T14:54:30.467',N'Setup',N'îles Turques-et-Caïques',null,0,N'319')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'688',2371,'2019-02-07T14:54:40.213',N'Setup',0,1,1,N'TV',N'TUV',N'fr',null,'2019-02-07T14:54:40.213',N'Setup',N'Tuvalu',null,0,N'320')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'256',2372,'2019-02-07T14:54:51.557',N'Setup',0,1,1,N'UG',N'UGA',N'fr',null,'2019-02-07T14:54:51.557',N'Setup',N'Ouganda',null,0,N'321')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'380',2373,'2019-02-07T14:55:00.797',N'Setup',0,1,1,N'UA',N'UKR',N'fr',null,'2019-02-07T14:55:00.797',N'Setup',N'Ukraine',null,0,N'322')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'971',2374,'2019-02-07T14:55:10.980',N'Setup',0,1,1,N'AE',N'ARE',N'fr',null,'2019-02-07T14:55:10.980',N'Setup',N'Emirats Arabes Unis',null,0,N'323')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'44',2375,'2019-02-07T14:55:19.893',N'Setup',1,1,1,N'GB',N'GBR',N'fr',null,'2019-02-07T14:55:19.893',N'Setup',N'Royaume-Uni',null,3,N'324')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',2376,'2019-02-07T14:55:29.647',N'Setup',1,1,1,N'US',N'USA',N'fr',null,'2019-02-07T14:55:29.647',N'Setup',N'États Unis',null,6,N'325')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'',2377,'2019-02-07T14:55:40.727',N'Setup',0,1,1,N'UM',N'UMI',N'fr',null,'2019-02-07T14:55:40.727',N'Setup',N'Îles mineures éloignées des États-Unis',null,0,N'326')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'598',2378,'2019-02-07T14:55:50.590',N'Setup',0,1,1,N'UY',N'URY',N'fr',null,'2019-02-07T14:55:50.590',N'Setup',N'Uruguay',null,0,N'327')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'998',2379,'2019-02-07T14:55:59.600',N'Setup',0,1,1,N'UZ',N'UZB',N'fr',null,'2019-02-07T14:55:59.600',N'Setup',N'Ouzbekistan',null,0,N'328')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'678',2380,'2019-02-07T14:56:08.720',N'Setup',0,1,1,N'VU',N'VUT',N'fr',null,'2019-02-07T14:56:08.720',N'Setup',N'Vanuatu',null,0,N'329')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'58',2381,'2019-02-07T14:56:18.290',N'Setup',0,1,1,N'VE',N'VEN',N'fr',null,'2019-02-07T14:56:18.290',N'Setup',N'Venezuela',null,0,N'330')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'84',2382,'2019-02-07T14:56:27.107',N'Setup',0,1,1,N'VN',N'VNM',N'fr',null,'2019-02-07T14:56:27.107',N'Setup',N'Vietnam',null,0,N'331')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1284',2383,'2019-02-07T14:56:42.200',N'Setup',0,1,1,N'VG',N'VGB',N'fr',null,'2019-02-07T14:56:42.200',N'Setup',N'Îles Vierges britanniques',null,0,N'332')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1340',2384,'2019-02-07T14:56:52.060',N'Setup',0,1,1,N'VI',N'VIR',N'fr',null,'2019-02-07T14:56:52.060',N'Setup',N'Îles Vierges américaines',null,0,N'333')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'681',2385,'2019-02-07T14:57:01.193',N'Setup',0,1,1,N'WF',N'WLF',N'fr',null,'2019-02-07T14:57:01.193',N'Setup',N'Wallis et Futuna',null,0,N'334')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',2386,'2019-02-07T14:57:11.617',N'Setup',0,1,1,N'EH',N'ESH',N'fr',null,'2019-02-07T14:57:11.617',N'Setup',N'Sahara occidental',null,0,N'335')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'967',2387,'2019-02-07T14:57:20.380',N'Setup',0,1,1,N'YE',N'YEM',N'fr',null,'2019-02-07T14:57:20.380',N'Setup',N'Yémen',null,0,N'336')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'260',2388,'2019-02-07T14:57:30.890',N'Setup',0,1,1,N'ZM',N'ZMB',N'fr',null,'2019-02-07T14:57:30.890',N'Setup',N'Zambie',null,0,N'337')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'263',2389,'2019-02-07T14:57:40.827',N'Setup',0,1,1,N'ZW',N'ZWE',N'fr',null,'2019-02-07T14:57:40.827',N'Setup',N'Zimbabwe',null,0,N'338')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'972',2390,'2019-02-07T15:29:44.033',N'Setup',0,1,1,N'IL',N'ISR',N'fr',null,'2019-02-07T15:29:44.033',N'Setup',N'Israël',null,0,N'203')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'93',2391,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AF',N'AFG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Afganistán',null,0,N'101')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'355',2392,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AL',N'ALB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Albania',null,0,N'102')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'49',2393,'2021-07-13T08:43:50.493',N'Creater',1,1,1,N'DE',N'DEU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Alemania',null,0,N'100')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'213',2394,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'DZ',N'DZA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Argelia',null,0,N'103')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1684',2395,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AS',N'ASM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Samoa Americana',null,0,N'104')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'376',2396,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AD',N'AND',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Andorra',null,0,N'105')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'244',2397,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AO',N'AGO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Angola',null,0,N'106')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1264',2398,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AI',N'AIA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Anguilla',null,0,N'107')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6721',2399,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AQ',N'ATA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Antártica',null,0,N'108')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1268',2400,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AG',N'ATG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Antigua y Barbuda',null,0,N'109')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'54',2401,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AR',N'ARG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Argentina',null,0,N'110')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'374',2402,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AM',N'ARM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Armenia',null,0,N'111')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'297',2403,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AW',N'ABW',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Aruba',null,0,N'112')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'61',2404,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AU',N'AUS',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Australia',null,0,N'113')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'43',2405,'2021-07-13T08:43:50.493',N'Creater',1,1,1,N'AT',N'AUT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Austria',null,1,N'114')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'994',2406,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AZ',N'AZE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Azerbaiyán',null,0,N'115')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1242',2407,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BS',N'BHS',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bahamas',null,0,N'116')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'973',2408,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BH',N'BHR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Baréin',null,0,N'117')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'880',2409,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BD',N'BGD',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bangladés',null,0,N'118')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1246',2410,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BB',N'BRB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Barbados',null,0,N'119')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'375',2411,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BY',N'BLR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Belarús',null,0,N'120')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'32',2412,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BE',N'BEL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bélgica',null,0,N'121')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'501',2413,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BZ',N'BLZ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Belice',null,0,N'122')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'229',2414,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BJ',N'BEN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Benín',null,0,N'123')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1441',2415,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BM',N'BMU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bermudas',null,0,N'124')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'975',2416,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BT',N'BTN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bután',null,0,N'125')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'591',2417,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BO',N'BOL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bolivia',null,0,N'126')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'387',2418,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BA',N'BIH',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bosnia y Herzegovina',null,0,N'127')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'267',2419,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BW',N'BWA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Botsuana',null,0,N'128')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2420,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BV',N'BVT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Isla Bouvet',null,0,N'129')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'55',2421,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BR',N'BRA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Brasil',null,0,N'130')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'246',2422,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'IO',N'IOT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Territorio Británico del Océano Índico',null,0,N'131')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'673',2423,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BN',N'BRN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Brunei Darussalam',null,0,N'132')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'359',2424,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BG',N'BGR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Bulgaria',null,0,N'133')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'226',2425,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BF',N'BFA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Burkina Faso',null,0,N'134')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'226',2426,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'BI',N'BDI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Burundi',null,0,N'135')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'855',2427,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KH',N'KHM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Camboya',null,0,N'136')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'237',2428,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CM',N'CMR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Camerún',null,0,N'137')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',2429,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CA',N'CAN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Canadá',null,0,N'138')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'238',2430,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CV',N'CPV',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Cabo Verde',null,0,N'139')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1345',2431,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KY',N'CYM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Caimán',null,0,N'140')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'236',2432,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CF',N'CAF',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Centroafricana',null,0,N'141')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'235',2433,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TD',N'TCD',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Chad',null,0,N'142')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'56',2434,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CL',N'CHL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Chile',null,0,N'143')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'86',2435,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CN',N'CHN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'China',null,0,N'144')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2436,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CX',N'CXR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Isla Christmas',null,0,N'145')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2437,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CC',N'CCK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Cocos',null,0,N'146')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'57',2438,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CO',N'COL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Colombia',null,0,N'147')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'269',2439,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KM',N'COM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Comoras',null,0,N'148')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'242',2440,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CG',N'COG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Congo',null,0,N'149')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'243',2441,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CD',N'COD',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Democrática del Congo',null,0,N'150')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'682',2442,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CK',N'COK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Cook',null,0,N'151')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'506',2443,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CR',N'CRI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Costa Rica',null,0,N'152')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'225',2444,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CI',N'CIV',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Costa de Marfil',null,0,N'153')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'385',2445,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'HR',N'HRV',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Croacia',null,0,N'154')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'53',2446,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CU',N'CUB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Cuba',null,0,N'155')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'357',2447,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CY',N'CYP',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Chipre',null,0,N'156')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'420',2448,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'CZ',N'CZE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Checa',null,0,N'157')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'45',2449,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'DK',N'DNK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Dinamarca',null,0,N'158')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'253',2450,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'DJ',N'DJI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Yibuti',null,0,N'159')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1767',2451,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'DM',N'DMA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Dominica',null,0,N'160')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1809',2452,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'DO',N'DOM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Dominicana',null,0,N'161')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'593',2453,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'EC',N'ECU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Ecuador',null,0,N'162')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'20',2454,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'EG',N'EGY',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Egipto',null,0,N'163')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'503',2455,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SV',N'SLV',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'El Salvador',null,0,N'164')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'240',2456,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GQ',N'GNQ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guinea Ecuatorial',null,0,N'165')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'291',2457,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ER',N'ERI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Eritrea',null,0,N'166')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'372',2458,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'EE',N'EST',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Estonia',null,0,N'167')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'251',2459,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ET',N'ETH',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Etiopía',null,0,N'168')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'500',2460,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'FK',N'FLK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Malvinas',null,0,N'169')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'298',2461,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'FO',N'FRO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Feroe',null,0,N'170')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'679',2462,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'FJ',N'FJI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Fiyi',null,0,N'171')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'358',2463,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'FI',N'FIN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Finlandia',null,0,N'172')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'33',2464,'2021-07-13T08:43:50.493',N'Creater',1,1,1,N'FR',N'FRA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Francia',null,4,N'173')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'594',2465,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GF',N'GUF',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guyana Francesa',null,0,N'174')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'689',2466,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PF',N'PYF',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Polinesia Francesa',null,0,N'175')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2467,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TF',N'ATF',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Tierras Australes y Antárticas Francesas',null,0,N'176')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'241',2468,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GA',N'GAB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Gabón',null,0,N'177')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'220',2469,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GM',N'GMB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Gambia',null,0,N'178')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'995',2470,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GE',N'GEO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Georgia',null,0,N'179')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'233',2471,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GH',N'GHA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Ghana',null,0,N'180')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'350',2472,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GI',N'GIB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Gibraltar',null,0,N'181')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'30',2473,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GR',N'GRC',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Grecia',null,0,N'182')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'299',2474,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GL',N'GRL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Groenlandia',null,0,N'183')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1473',2475,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GD',N'GRD',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Granada',null,0,N'184')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'590',2476,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GP',N'GLP',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guadalupe',null,0,N'185')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1671',2477,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GU',N'GUM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guam',null,0,N'186')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'502',2478,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GT',N'GTM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guatemala',null,0,N'187')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'224',2479,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GN',N'GIN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guinea',null,0,N'188')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'245',2480,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GW',N'GNB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guinea-Bisáu',null,0,N'189')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'592',2481,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GY',N'GUY',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Guyana',null,0,N'190')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'509',2482,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'HT',N'HTI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Haití',null,0,N'191')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2483,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'HM',N'HMD',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Heard y McDonald',null,0,N'192')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',2484,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'VA',N'VAT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Estado de la Ciudad del Vaticano',null,0,N'193')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'504',2485,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'HN',N'HND',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Honduras',null,0,N'194')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'852',2486,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'HK',N'HKG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Hong Kong',null,0,N'195')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'36',2487,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'HU',N'HUN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Hungría',null,0,N'196')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'354',2488,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'IS',N'ISL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islandia',null,0,N'197')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'91',2489,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'IN',N'IND',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'India',null,0,N'198')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'62',2490,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ID',N'IDN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Indonesia',null,0,N'199')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2491,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'IR',N'IRN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Islámica de Irán',null,0,N'200')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'964',2492,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'IQ',N'IRQ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Irak',null,0,N'201')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'353',2493,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'IE',N'IRL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Irlanda',null,0,N'202')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'972',2494,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'IL',N'ISR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Israel',null,0,N'203')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'39',2495,'2021-07-13T08:43:50.493',N'Creater',1,1,1,N'IT',N'ITA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Italia',null,5,N'204')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1876',2496,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'JM',N'JAM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Jamaica',null,0,N'205')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'81',2497,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'JP',N'JPN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Japón',null,0,N'206')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'962',2498,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'JO',N'JOR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Jordania',null,0,N'207')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',2499,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KZ',N'KAZ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Kazajistán',null,0,N'208')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'254',2500,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KE',N'KEN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Kenia',null,0,N'209')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'686',2501,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KI',N'KIR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Kiribati',null,0,N'210')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2502,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KP',N'PRK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Popular Democrática de Corea',null,0,N'211')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'850',2503,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KR',N'KOR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República de Corea',null,0,N'212')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'965',2504,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KW',N'KWT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Kuwait',null,0,N'213')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'996',2505,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KG',N'KGZ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Kirguistán',null,0,N'214')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2506,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LA',N'LAO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Laos',null,0,N'215')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'371',2507,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LV',N'LVA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Letonia',null,0,N'216')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'961',2508,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LB',N'LBN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Líbano',null,0,N'217')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'266',2509,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LS',N'LSO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Lesoto',null,0,N'218')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'231',2510,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LR',N'LBR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Liberia',null,0,N'219')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'218',2511,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LY',N'LBY',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Jamahiriya Arabe Libia',null,0,N'220')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'423',2512,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LI',N'LIE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Liechtenstein',null,0,N'221')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'370',2513,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LT',N'LTU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Lituania',null,0,N'222')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'352',2514,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LU',N'LUX',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Luxemburgo',null,0,N'223')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'853',2515,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MO',N'MAC',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Macao',null,0,N'224')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2516,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MK',N'MKD',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Macedonia del Norte',null,0,N'225')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'261',2517,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MG',N'MDG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Madagascar',null,0,N'226')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'265',2518,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MW',N'MWI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Malaui',null,0,N'227')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'60',2519,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MY',N'MYS',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Malasia',null,0,N'228')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'960',2520,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MV',N'MDV',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Maldivas',null,0,N'229')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'223',2521,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ML',N'MLI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Mali',null,0,N'230')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'356',2522,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MT',N'MLT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Malta',null,0,N'231')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'692',2523,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MH',N'MHL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Marshall',null,0,N'232')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'596',2524,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MQ',N'MTQ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Martinica',null,0,N'233')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'222',2525,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MR',N'MRT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Mauritania',null,0,N'234')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'230',2526,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MU',N'MUS',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Mauricio',null,0,N'235')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',2527,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'YT',N'MYT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Mayotte',null,0,N'236')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'52',2528,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MX',N'MEX',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'México',null,0,N'237')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2529,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'FM',N'FSM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Estados Federados de Micronesia',null,0,N'238')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2530,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MD',N'MDA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República de Moldavia',null,0,N'239')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'377',2531,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MC',N'MCO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Mónaco',null,0,N'240')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'976',2532,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MN',N'MNG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Mongolia',null,0,N'241')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1664',2533,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MS',N'MSR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Montserrat',null,0,N'242')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',2534,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MA',N'MAR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Marruecos',null,0,N'243')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'258',2535,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MZ',N'MOZ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Mozambique',null,0,N'244')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'95',2536,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MM',N'MMR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Myanmar',null,0,N'245')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'264',2537,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NA',N'NAM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Namibia',null,0,N'246')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'674',2538,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NR',N'NRU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Nauru',null,0,N'247')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'977',2539,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NP',N'NPL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Nepal',null,0,N'248')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'31',2540,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NL',N'NLD',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Países Bajos',null,0,N'249')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'599',2541,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AN',N'ANT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Antillas Neerlandesas',null,0,N'250')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'687',2542,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NC',N'NCL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Nueva Caledonia',null,0,N'251')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'64',2543,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NZ',N'NZL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Nueva Zelanda',null,0,N'252')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'505',2544,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NI',N'NIC',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Nicaragua',null,0,N'253')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'227',2545,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NE',N'NER',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Niger',null,0,N'254')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'234',2546,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NG',N'NGA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Nigeria',null,0,N'255')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'683',2547,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NU',N'NIU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Niue',null,0,N'256')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'6723',2548,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NF',N'NFK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Isla Norfolk',null,0,N'257')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1670',2549,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'MP',N'MNP',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Marianas del Norte',null,0,N'258')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',2550,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'NO',N'NOR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Noruega',null,0,N'259')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'968',2551,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'OM',N'OMN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Omán',null,0,N'260')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'92',2552,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PK',N'PAK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Pakistán',null,0,N'261')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'680',2553,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PW',N'PLW',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Palau',null,0,N'262')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2554,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PS',N'PSE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Territorios Palestinos',null,0,N'263')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'507',2555,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PA',N'PAN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Panamá',null,0,N'264')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'675',2556,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PG',N'PNG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Papúa Nueva Guinea',null,0,N'265')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'595',2557,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PY',N'PRY',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Paraguay',null,0,N'266')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'51',2558,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PE',N'PER',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Peru',null,0,N'267')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'63',2559,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PH',N'PHL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Filipinas',null,0,N'268')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2560,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PN',N'PCN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Pitcairn',null,0,N'269')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'48',2561,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PL',N'POL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Polonia',null,0,N'270')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'351',2562,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PT',N'PRT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Portugal',null,0,N'271')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1787',2563,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PR',N'PRI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Puerto Rico',null,0,N'272')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'974',2564,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'QA',N'QAT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Qatar',null,0,N'273')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'262',2565,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'RE',N'REU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Reunion',null,0,N'274')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'40',2566,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'RO',N'ROU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Rumanía',null,0,N'275')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'7',2567,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'RU',N'RUS',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Federación de Rusia',null,0,N'276')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'250',2568,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'RW',N'RWA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Ruanda',null,0,N'277')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'290',2569,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SH',N'SHN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Santa Elena',null,0,N'278')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1869',2570,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'KN',N'KNA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'San Cristóbal y Nieves',null,0,N'279')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1758',2571,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LC',N'LCA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Santa Lucia',null,0,N'280')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'508',2572,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'PM',N'SPM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'San Pedro y Miquelón',null,0,N'281')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1784',2573,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'VC',N'VCT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'San Vicente y las Granadinas',null,0,N'282')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'685',2574,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'WS',N'WSM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Samoa',null,0,N'283')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'378',2575,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SM',N'SMR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'San Marino',null,0,N'284')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'239',2576,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ST',N'STP',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Santo Tomé y Príncipe',null,0,N'285')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'966',2577,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SA',N'SAU',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Arabia Saudí',null,0,N'286')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'221',2578,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SN',N'SEN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Senegal',null,0,N'287')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2579,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'YU',N'YUG',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Serbia',null,0,N'288')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'248',2580,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SC',N'SYC',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Seychelles',null,0,N'289')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'232',2581,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SL',N'SLE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Sierra Leone',null,0,N'290')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'65',2582,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SG',N'SGP',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Singapur',null,0,N'291')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'421',2583,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SK',N'SVK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Eslovaquia',null,0,N'292')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'386',2584,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SI',N'SVN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Eslovenia',null,0,N'293')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'677',2585,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SB',N'SLB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Salomón',null,0,N'294')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'252',2586,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SO',N'SOM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Somalia',null,0,N'295')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'27',2587,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ZA',N'ZAF',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Sudáfrica',null,0,N'296')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2588,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'GS',N'SGS',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Georgias del Sur y Sandwich del Sur',null,0,N'297')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'34',2589,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ES',N'ESP',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'España',null,0,N'298')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'94',2590,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'LK',N'LKA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Sri Lanka',null,0,N'299')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'249',2591,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SD',N'SDN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Sudán',null,0,N'300')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'597',2592,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SR',N'SUR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Suriname',null,0,N'301')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'47',2593,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SJ',N'SJM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Svalbard y Jan Mayen',null,0,N'302')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'268',2594,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SZ',N'SWZ',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Swaziland',null,0,N'303')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'46',2595,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SE',N'SWE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Suecia',null,0,N'304')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'41',2596,'2021-07-13T08:43:50.493',N'Creater',1,1,1,N'CH',N'CHE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Suiza',null,2,N'305')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'963',2597,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'SY',N'SYR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Árabe Siria',null,0,N'306')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'886',2598,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TW',N'TWN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Taiwan',null,0,N'307')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'992',2599,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TJ',N'TJK',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Tayikistán',null,0,N'308')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'255',2600,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TZ',N'TZA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'República Unida de Tanzania',null,0,N'309')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'66',2601,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TH',N'THA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Tailandia',null,0,N'310')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'670',2602,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TL',N'TLS',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Timor Oriental',null,0,N'311')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'228',2603,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TG',N'TGO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Togo',null,0,N'312')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'690',2604,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TK',N'TKL',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Tokelau',null,0,N'313')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'676',2605,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TO',N'TON',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Tonga',null,0,N'314')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1868',2606,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TT',N'TTO',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Trinidad and Tobago',null,0,N'315')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'216',2607,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TN',N'TUN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Túnez',null,0,N'316')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'90',2608,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TR',N'TUR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Turquía',null,0,N'317')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'993',2609,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TM',N'TKM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Turkmenistán',null,0,N'318')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1649',2610,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TC',N'TCA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Turcas y Caicos',null,0,N'319')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'688',2611,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'TV',N'TUV',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Tuvalu',null,0,N'320')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'256',2612,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'UG',N'UGA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Uganda',null,0,N'321')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'380',2613,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'UA',N'UKR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Ucrania',null,0,N'322')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'971',2614,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'AE',N'ARE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Emiratos Árabes Unidos',null,0,N'323')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'44',2615,'2021-07-13T08:43:50.493',N'Creater',1,1,1,N'GB',N'GBR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Reino Unido',null,3,N'324')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1',2616,'2021-07-13T08:43:50.493',N'Creater',1,1,1,N'US',N'USA',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Estados Unidos',null,6,N'325')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'0',2617,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'UM',N'UMI',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas menores alejadas de los Estados Unidos',null,0,N'326')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'598',2618,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'UY',N'URY',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Uruguay',null,0,N'327')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'998',2619,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'UZ',N'UZB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Uzbekistán',null,0,N'328')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'678',2620,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'VU',N'VUT',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Vanuatu',null,0,N'329')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'58',2621,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'VE',N'VEN',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Venezuela',null,0,N'330')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'84',2622,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'VN',N'VNM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Vietnam',null,0,N'331')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1284',2623,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'VG',N'VGB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Vírgenes Británicas',null,0,N'332')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'1340',2624,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'VI',N'VIR',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Islas Vìrgenes de EE.UU',null,0,N'333')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'681',2625,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'WF',N'WLF',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Wallis y Futuna',null,0,N'334')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'212',2626,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'EH',N'ESH',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Sáhara Occidental',null,0,N'335')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'967',2627,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'YE',N'YEM',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Yemen',null,0,N'336')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'260',2628,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ZM',N'ZMB',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Zambia',null,0,N'337')
INSERT [LU].[Country]([CallingCode],[CountryId],[CreateDate],[CreateUser],[Favorite],[IsActive],[IsGreat],[Iso2Code],[Iso3Code],[Language],[LegacyId],[ModifyDate],[ModifyUser],[Name],[Rating],[SortOrder],[Value])
VALUES(N'263',2629,'2021-07-13T08:43:50.493',N'Creater',0,1,1,N'ZW',N'ZWE',N'es',null,'2021-07-13T08:43:50.493',N'Modifier',N'Zimbabue',null,0,N'338')
SET IDENTITY_INSERT [LU].[Country] OFF


SET IDENTITY_INSERT [LU].[Currency] ON
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2014-06-24T11:32:42.097',N'Setup',1,1,N'de','2019-02-06T21:36:02.517',N'Setup',N'€',19,0,N'EUR')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2014-06-24T11:32:42.097',N'Setup',1,1,N'en','2019-02-06T21:36:02.517',N'Setup',N'€',20,0,N'EUR')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2014-06-24T11:32:42.097',N'Setup',0,1,N'de','2019-02-06T21:36:15.843',N'Setup',N'$',21,1,N'USD')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2014-06-24T11:32:42.097',N'Setup',0,1,N'en','2019-02-06T21:36:15.843',N'Setup',N'$',22,1,N'USD')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2014-06-24T11:32:42.097',N'Setup',0,1,N'de','2019-02-06T21:36:11.813',N'Setup',N'£',23,2,N'GBP')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2014-06-24T11:32:42.097',N'Setup',0,1,N'en','2019-02-06T21:36:11.813',N'Setup',N'£',24,2,N'GBP')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2017-08-28T11:32:42.097',N'Setup',1,1,N'hu','2019-02-06T21:36:02.517',N'Setup',N'€',25,0,N'EUR')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2017-08-28T11:32:42.097',N'Setup',0,1,N'hu','2019-02-06T21:36:15.843',N'Setup',N'$',26,1,N'USD')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2017-08-28T11:32:42.097',N'Setup',0,1,N'hu','2019-02-06T21:36:11.813',N'Setup',N'£',27,2,N'GBP')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2019-02-06T21:36:02.293',N'Setup',1,1,N'fr','2019-02-06T21:36:02.293',N'Setup',N'€',28,0,N'EUR')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2019-02-06T21:36:11.593',N'Setup',0,1,N'fr','2019-02-06T21:36:11.593',N'Setup',N'£',29,2,N'GBP')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2019-02-06T21:36:15.627',N'Setup',0,1,N'fr','2019-02-06T21:36:15.627',N'Setup',N'$',30,1,N'USD')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.323',N'Creater',1,1,N'es','2021-07-13T08:43:50.323',N'Modifier',N'€',31,0,N'EUR')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.323',N'Creater',0,1,N'es','2021-07-13T08:43:50.323',N'Modifier',N'$',32,1,N'USD')
INSERT [LU].[Currency]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[ProjectCategoriyId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.323',N'Creater',0,1,N'es','2021-07-13T08:43:50.323',N'Modifier',N'£',33,2,N'GBP')
SET IDENTITY_INSERT [LU].[Currency] OFF

SET IDENTITY_INSERT [LU].[InvoicingType] ON
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,1,1,1,1,1,N'en','2021-11-10T15:18:31.173',N'migration_20211004153000',N'Lump sum',0,N'LumpSum')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,2,1,1,1,1,N'de','2021-11-10T15:18:31.173',N'migration_20211004153000',N'Pauschal',0,N'LumpSum')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,3,1,0,0,0,N'en','2019-02-06T21:52:42.757',N'Setup',N'T&M basis',1,N'TMbasis')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,4,1,0,0,0,N'de','2019-02-06T21:52:42.757',N'Setup',N'nach Aufwand',1,N'TMbasis')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,5,1,0,0,0,N'en','2019-02-06T21:48:30.403',N'Setup',N'By status',2,N'ByStatus')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,6,1,0,0,0,N'de','2019-02-06T21:48:30.400',N'Setup',N'Festlegung nach Sachstand',2,N'ByStatus')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,7,1,0,0,0,N'en','2019-02-06T21:52:25.493',N'Setup',N'Goodwill',2,N'Goodwill')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,8,1,0,0,0,N'de','2019-02-06T21:52:25.493',N'Setup',N'Kulanz',2,N'Goodwill')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,9,1,1,1,1,N'hu','2021-11-10T15:18:31.173',N'migration_20211004153000',N'##hu: InvoicingType_LumpSum',0,N'LumpSum')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,10,1,0,0,0,N'hu','2019-02-06T21:52:42.757',N'Setup',N'##hu: InvoicingType_TMbasis',1,N'TMbasis')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,11,1,0,0,0,N'hu','2019-02-06T21:48:30.403',N'Setup',N'##hu: InvoicingType_ByStatus',2,N'ByStatus')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.947',N'Migration_20170523101900',0,12,1,0,0,0,N'hu','2019-02-06T21:52:25.493',N'Setup',N'##hu: InvoicingType_Goodwill',2,N'Goodwill')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2019-02-06T21:48:30.163',N'Setup',0,13,1,0,0,0,N'fr','2019-02-06T21:48:30.163',N'Setup',N'Par statut',2,N'ByStatus')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2019-02-06T21:52:16.237',N'Setup',0,14,1,0,0,0,N'fr','2019-02-06T21:52:25.493',N'Setup',N'A volonté',2,N'Goodwill')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2019-02-06T21:52:18.683',N'Setup',0,15,1,1,1,1,N'fr','2021-11-10T15:18:31.173',N'migration_20211004153000',N'Montant forfaitaire',0,N'LumpSum')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2019-02-06T21:52:42.543',N'Setup',0,16,1,0,0,0,N'fr','2019-02-06T21:52:42.543',N'Setup',N'Base de T&M',1,N'TMbasis')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.340',N'Creater',0,17,1,0,0,0,N'es','2021-07-13T08:43:50.340',N'Modifier',N'Goodwill',2,N'Goodwill')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.340',N'Creater',0,18,1,1,1,1,N'es','2021-11-10T15:18:31.173',N'migration_20211004153000',N'Suma agrupada',0,N'LumpSum')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.340',N'Creater',0,19,1,0,0,0,N'es','2021-07-13T08:43:50.340',N'Modifier',N'Por Estado',2,N'ByStatus')
INSERT [LU].[InvoicingType]([CreateDate],[CreateUser],[Favorite],[InvoicingTypeId],[IsActive],[IsCostLumpSum],[IsMaterialLumpSum],[IsTimeLumpSum],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.340',N'Creater',0,20,1,0,0,0,N'es','2021-07-13T08:43:50.340',N'Modifier',N'Base de T&M',1,N'TMbasis')
SET IDENTITY_INSERT [LU].[InvoicingType] OFF


SET IDENTITY_INSERT [LU].[Language] ON
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'de',0,1,1,13,N'de','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Deutsch',0,N'de')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'de',0,1,1,14,N'en','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'German',0,N'de')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'de',0,1,1,15,N'hu','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Német',0,N'de')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'en-GB',0,1,1,16,N'de','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Englisch',0,N'en')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'en-GB',0,1,1,17,N'en','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'English',0,N'en')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'en-GB',0,1,1,18,N'hu','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Angol',0,N'en')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'hu',0,1,0,19,N'de','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Ungarisch',0,N'hu')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'hu',0,1,0,20,N'en','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Hungarian',0,N'hu')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'hu',0,1,0,21,N'hu','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Magyar',0,N'hu')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'de',0,1,1,22,N'fr','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Allemand',0,N'de')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'en-GB',0,1,1,23,N'fr','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Anglais',0,N'en')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'hu',0,1,0,24,N'fr','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Hongrois',0,N'hu')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'fr',0,1,1,25,N'de','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Französisch',0,N'fr')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'fr',0,1,1,26,N'en','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'French',0,N'fr')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'fr',0,1,1,27,N'fr','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Français',0,N'fr')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.877',N'Migration_20170523101900',N'fr',0,1,1,28,N'hu','2018-05-24T15:17:45.877',N'Migration_20170523101900',N'Francia',0,N'fr')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'es',0,1,1,29,N'en','2021-07-13T08:43:50.270',N'Modifier',N'Spanish',0,N'es')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'es',0,1,1,30,N'de','2021-07-13T08:43:50.270',N'Modifier',N'Spanisch',0,N'es')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'es',0,1,1,31,N'fr','2021-07-13T08:43:50.270',N'Modifier',N'Espagnol',0,N'es')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'es',0,1,1,32,N'es','2021-07-13T08:43:50.270',N'Modifier',N'Español',0,N'es')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'es',0,1,1,33,N'hu','2021-07-13T08:43:50.270',N'Modifier',N'Spanyol',0,N'es')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'en-GB',0,1,1,34,N'es','2021-07-13T08:43:50.270',N'Modifier',N'Inglés',0,N'en')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'de',0,1,1,35,N'es','2021-07-13T08:43:50.270',N'Modifier',N'Alemán',0,N'de')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'hu',0,1,0,36,N'es','2021-07-13T08:43:50.270',N'Modifier',N'Húngaro',0,N'hu')
INSERT [LU].[Language]([CreateDate],[CreateUser],[DefaultLocale],[Favorite],[IsActive],[IsSystemLanguage],[LangId],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.270',N'Creater',N'fr',0,1,1,37,N'es','2021-07-13T08:43:50.270',N'Modifier',N'Francés',0,N'fr')
SET IDENTITY_INSERT [LU].[Language] OFF


SET IDENTITY_INSERT [LU].[PaymentCondition] ON
INSERT [LU].[PaymentCondition]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentConditionId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.940',N'Migration_20170523101900',0,1,N'en','2019-02-07T09:02:13.617',N'Setup',N'Payable on receipt without discount',1,0,N'1')
INSERT [LU].[PaymentCondition]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentConditionId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.940',N'Migration_20170523101900',0,1,N'de','2019-02-07T09:02:13.617',N'Setup',N'Zahlbar nach Erhalt ohne Abzug',2,0,N'1')
INSERT [LU].[PaymentCondition]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentConditionId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.940',N'Migration_20170523101900',0,1,N'hu','2019-02-07T09:02:13.620',N'Setup',N'##hu: PaymentCondition_1',3,0,N'1')
INSERT [LU].[PaymentCondition]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentConditionId],[SortOrder],[Value])
VALUES('2019-02-07T09:02:13.393',N'Setup',0,1,N'fr','2019-02-07T09:02:13.393',N'Setup',N'Payable à la réception sans escompte',4,0,N'1')
INSERT [LU].[PaymentCondition]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentConditionId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.390',N'Creater',0,1,N'es','2021-07-13T08:43:50.390',N'Modifier',N'A pagar en el momento de la recepción sin descuento',5,0,N'1')
SET IDENTITY_INSERT [LU].[PaymentCondition] OFF


SET IDENTITY_INSERT [LU].[PaymentInterval] ON
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.933',N'Migration_20170523101900',0,1,N'en','2019-02-07T09:01:09.927',N'Setup',N'Immediately',1,0,N'1')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.933',N'Migration_20170523101900',0,1,N'de','2019-02-07T09:01:09.927',N'Setup',N'Sofort',2,0,N'1')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.933',N'Migration_20170523101900',0,1,N'en','2019-02-07T09:01:30.733',N'Setup',N'Quarterly',3,1,N'2')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.933',N'Migration_20170523101900',0,1,N'de','2019-02-07T09:01:30.733',N'Setup',N'Vierteljährlich',4,1,N'2')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.933',N'Migration_20170523101900',0,1,N'hu','2019-02-07T09:01:09.927',N'Setup',N'##hu: PaymentInterval_1',5,0,N'1')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.933',N'Migration_20170523101900',0,1,N'hu','2019-02-07T09:01:30.733',N'Setup',N'##hu: PaymentInterval_2',6,1,N'2')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2019-02-07T09:01:09.697',N'Setup',0,1,N'fr','2019-02-07T09:01:09.697',N'Setup',N'Immédiatement',7,0,N'1')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2019-02-07T09:01:30.517',N'Setup',0,1,N'fr','2019-02-07T09:01:30.517',N'Setup',N'Trimestriel',8,1,N'2')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.377',N'Creater',0,1,N'es','2021-07-13T08:43:50.377',N'Modifier',N'Trimestralmente',9,1,N'2')
INSERT [LU].[PaymentInterval]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentIntervalId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.377',N'Creater',0,1,N'es','2021-07-13T08:43:50.377',N'Modifier',N'Inmediatamente',10,0,N'1')
SET IDENTITY_INSERT [LU].[PaymentInterval] OFF


SET IDENTITY_INSERT [LU].[PaymentType] ON
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.927',N'Migration_20170523101900',0,1,N'en','2019-02-07T09:00:36.177',N'Setup',N'Bill',1,0,N'1')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.927',N'Migration_20170523101900',0,1,N'de','2019-02-07T09:00:36.177',N'Setup',N'Rechnung',2,0,N'1')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.927',N'Migration_20170523101900',0,1,N'en','2019-02-07T09:00:46.747',N'Setup',N'Debit',3,1,N'2')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.927',N'Migration_20170523101900',0,1,N'de','2019-02-07T09:00:46.747',N'Setup',N'Abbuchung',4,1,N'2')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.927',N'Migration_20170523101900',0,1,N'hu','2019-02-07T09:00:36.177',N'Setup',N'##hu: PaymentType_1',5,0,N'1')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2018-05-24T15:17:45.927',N'Migration_20170523101900',0,1,N'hu','2019-02-07T09:00:46.750',N'Setup',N'##hu: PaymentType_2',6,1,N'2')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2019-02-07T09:00:35.950',N'Setup',0,1,N'fr','2019-02-07T09:00:35.950',N'Setup',N'Facture',7,0,N'1')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2019-02-07T09:00:46.530',N'Setup',0,1,N'fr','2019-02-07T09:00:46.530',N'Setup',N'Débit',8,1,N'2')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.383',N'Creater',0,1,N'es','2021-07-13T08:43:50.383',N'Modifier',N'Débito',9,1,N'2')
INSERT [LU].[PaymentType]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[PaymentTypeId],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.383',N'Creater',0,1,N'es','2021-07-13T08:43:50.383',N'Modifier',N'Factura',10,0,N'1')
SET IDENTITY_INSERT [LU].[PaymentType] OFF


SET IDENTITY_INSERT [LU].[Region] ON
INSERT [LU].[Region]([CountryKey],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[RegionId],[SortOrder],[Value])
VALUES(null,'2017-01-23T14:37:54.040',N'Migration_20161221100000',0,1,N'de','2019-02-07T08:56:55.977',N'Setup',N'Sonstige',7,0,N'100')
INSERT [LU].[Region]([CountryKey],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[RegionId],[SortOrder],[Value])
VALUES(null,'2017-01-23T14:37:54.040',N'Migration_20161221100000',0,1,N'en','2019-02-07T08:56:55.977',N'Setup',N'Other',8,0,N'100')
INSERT [LU].[Region]([CountryKey],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[RegionId],[SortOrder],[Value])
VALUES(null,'2017-03-20T15:54:16',N'LookupManager',0,1,N'hu','2019-02-07T08:56:55.977',N'Setup',N'##hu: Region_100',9,0,N'100')
INSERT [LU].[Region]([CountryKey],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[RegionId],[SortOrder],[Value])
VALUES(null,'2019-02-07T08:56:55.753',N'Setup',0,1,N'fr','2019-02-07T08:56:55.753',N'Setup',N'Autre',10,0,N'100')
INSERT [LU].[Region]([CountryKey],[CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[RegionId],[SortOrder],[Value])
VALUES(null,'2021-07-13T08:43:50.400',N'Creater',0,1,N'es','2021-07-13T08:43:50.400',N'Modifier',N'Otro',11,0,N'100')
SET IDENTITY_INSERT [LU].[Region] OFF


SET IDENTITY_INSERT [LU].[TimeUnit] ON
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Jahre',10,65,1,N'Year')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Years',10,66,1,N'Year')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Monate',30,67,12,N'Month')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Months',30,68,12,N'Month')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Wochen',40,69,52,N'Week')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Weeks',40,70,52,N'Week')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Tage',50,71,null,N'Day')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Days',50,72,null,N'Day')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',1,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Stunden',60,73,null,N'Hour')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',1,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Hours',60,74,null,N'Hour')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Minuten',70,75,null,N'Minute')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Minutes',70,76,null,N'Minute')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Sekunden',80,77,null,N'Second')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Seconds',80,78,null,N'Second')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Millisekunden',90,79,null,N'Millisecond')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Millisecond',90,80,null,N'Millisecond')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'de','2017-05-19T08:18:24.670',N'Setup',N'Quartal',20,81,4,N'Quarter')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'en','2017-05-19T08:18:24.670',N'Setup',N'Quarter',20,82,4,N'Quarter')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Év',10,83,1,N'Year')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Hónap',30,84,12,N'Month')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Hét',40,85,52,N'Week')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Nap',50,86,null,N'Day')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',1,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Óra',60,87,null,N'Hour')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Perc',70,88,null,N'Minute')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Másodperc',80,89,null,N'Second')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Milliszekundum',90,90,null,N'Millisecond')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2017-05-19T08:18:24.650',N'Setup',0,1,N'hu','2017-05-19T08:18:24.670',N'Setup',N'Negyed',20,91,4,N'Quarter')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.727',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.727',N'LookupManager',N'An',10,92,1,N'Year')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.733',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.733',N'LookupManager',N'Mois',30,93,12,N'Month')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.737',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.737',N'LookupManager',N'Semaine',40,94,52,N'Week')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.740',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.740',N'LookupManager',N'Jour',50,95,null,N'Day')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.743',N'LookupManager',1,1,N'fr','2018-05-24T15:25:40.743',N'LookupManager',N'Heure',60,96,null,N'Hour')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.747',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.747',N'LookupManager',N'Minute',70,97,null,N'Minute')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.750',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.750',N'LookupManager',N'Seconde',80,98,null,N'Second')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.753',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.753',N'LookupManager',N'Milliseconde',90,99,null,N'Millisecond')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2018-05-24T15:25:40.757',N'LookupManager',0,1,N'fr','2018-05-24T15:25:40.757',N'LookupManager',N'Trimestre',20,100,4,N'Quarter')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Años',10,101,1,N'Year')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Meses',30,102,12,N'Month')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Semanas',40,103,52,N'Week')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Dias',50,104,null,N'Day')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',1,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Horas',60,105,null,N'Hour')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Minutos',70,106,null,N'Minute')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Segundos',80,107,null,N'Second')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Milisegundos',90,108,null,N'Millisecond')
INSERT [LU].[TimeUnit]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[TimeUnitId],[TimeUnitsPerYear],[Value])
VALUES('2021-07-13T08:43:50.450',N'Creater',0,1,N'es','2021-07-13T08:43:50.450',N'Modifier',N'Trimestre',20,109,4,N'Quarter')
SET IDENTITY_INSERT [LU].[TimeUnit] OFF

SET IDENTITY_INSERT [LU].[UserStatus] ON
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'de','2019-02-06T22:13:35.967',N'Setup',N'Verfügbar',0,1,N'Available')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'en','2019-02-06T22:13:35.967',N'Setup',N'Available',0,2,N'Available')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'de','2019-02-06T22:13:34.797',N'Setup',N'Beschäftigt',0,3,N'Away')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'en','2019-02-06T22:13:34.797',N'Setup',N'Away',0,4,N'Away')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'de','2019-02-06T22:13:33.953',N'Setup',N'Abwesend',0,5,N'DnD')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'en','2019-02-06T22:13:33.953',N'Setup',N'Do Not Disturb',0,6,N'DnD')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'hu','2019-02-06T22:13:35.967',N'Setup',N'##hu: UserStatus_Available',0,7,N'Available')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'hu','2019-02-06T22:13:34.797',N'Setup',N'##hu: UserStatus_Away',0,8,N'Away')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2018-05-24T15:17:45.883',N'Migration_20170523101900',0,1,N'hu','2019-02-06T22:13:33.953',N'Setup',N'##hu: UserStatus_DnD',0,9,N'DnD')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2019-02-06T22:13:07.117',N'Setup',0,1,N'fr','2019-02-06T22:13:35.967',N'Setup',N'Disponible',0,10,N'Available')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2019-02-06T22:13:12.810',N'Setup',0,1,N'fr','2019-02-06T22:13:34.797',N'Setup',N'Absent',0,11,N'Away')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2019-02-06T22:13:33.737',N'Setup',0,1,N'fr','2019-02-06T22:13:33.737',N'Setup',N'Ne pas déranger',0,12,N'DnD')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2021-07-13T08:43:50.507',N'Creater',0,1,N'es','2021-07-13T08:43:50.507',N'Modifier',N'Disponible',0,13,N'Available')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2021-07-13T08:43:50.507',N'Creater',0,1,N'es','2021-07-13T08:43:50.507',N'Modifier',N'No disponible',0,14,N'Away')
INSERT [LU].[UserStatus]([CreateDate],[CreateUser],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[SortOrder],[UserStatusId],[Value])
VALUES('2021-07-13T08:43:50.507',N'Creater',0,1,N'es','2021-07-13T08:43:50.507',N'Modifier',N'No molestar',0,15,N'DnD')
SET IDENTITY_INSERT [LU].[UserStatus] OFF


SET IDENTITY_INSERT [LU].[DocumentCategory] ON
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2019-12-12T15:11:22.367',N'Setup',1,0,1,N'de','2021-10-10T14:22:05.083',N'default.1',N'Dokument',0,1,0,N'Document')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2019-12-12T15:11:22.367',N'Setup',2,0,1,N'en','2021-10-10T14:22:05.083',N'default.1',N'Document',0,1,0,N'Document')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2019-12-12T15:11:22.367',N'Setup',3,0,1,N'fr','2021-10-10T14:22:05.083',N'default.1',N'Document',0,1,0,N'Document')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2019-12-12T15:11:22.367',N'Setup',4,0,1,N'hu','2019-12-12T15:11:22.367',N'Setup',N'Dokumentáció',0,0,0,N'Document')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-07-13T08:43:50.460',N'Setup',5,0,1,N'es','2021-10-10T14:22:05.083',N'default.1',N'Documento',0,1,0,N'Document')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:02.897',N'default.1',6,0,1,N'de','2021-10-10T14:22:03.727',N'default.1',N'Links',0,1,0,N'Links')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:02.927',N'default.1',7,0,1,N'en','2021-10-10T14:22:03.727',N'default.1',N'Links',0,1,0,N'Links')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:02.930',N'default.1',8,0,1,N'es','2021-10-10T14:22:03.727',N'default.1',N'Liens',0,1,0,N'Links')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:02.933',N'default.1',9,0,1,N'fr','2021-10-10T14:22:03.727',N'default.1',N'Enlaces',0,1,0,N'Links')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:32.263',N'default.1',10,0,1,N'de','2021-10-10T14:22:02.517',N'default.1',N'Einladungen',0,1,0,N'Invitations')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:32.270',N'default.1',11,0,1,N'en','2021-10-10T14:22:02.517',N'default.1',N'Invitations',0,1,0,N'Invitations')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:32.273',N'default.1',12,0,1,N'es','2021-10-10T14:22:02.517',N'default.1',N'Invitaciones',0,1,0,N'Invitations')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:32.277',N'default.1',13,0,1,N'fr','2021-10-10T14:22:02.517',N'default.1',N'Invitations',0,1,0,N'Invitations')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:59.847',N'default.1',14,0,1,N'de','2021-10-10T14:22:01.737',N'default.1',N'Referenzberichte',0,1,0,N'Refreports')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:59.850',N'default.1',15,0,1,N'en','2021-10-10T14:22:01.737',N'default.1',N'Reference reports',0,1,0,N'Refreports')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:59.853',N'default.1',16,0,1,N'es','2021-10-10T14:22:01.740',N'default.1',N'Informes de referencia',0,1,0,N'Refreports')
INSERT [LU].[DocumentCategory]([CreateDate],[CreateUser],[DocumentCategoryId],[Favorite],[IsActive],[Language],[ModifyDate],[ModifyUser],[Name],[OfflineRelevant],[SalesRelated],[SortOrder],[Value])
VALUES('2021-10-10T14:21:59.857',N'default.1',17,0,1,N'fr','2021-10-10T14:22:01.740',N'default.1',N'Rapports de référence',0,1,0,N'Refreports')
SET IDENTITY_INSERT [LU].[DocumentCategory] OFF
");
			Database.ExecuteNonQuery(@"
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-09-28T12:50:17.953',N'System',null,0,'2021-09-28T12:50:17.953',N'System',N'Crm.Project.Model.Potential','427b74a1-5a20-ec11-98ac-001a7dda7115',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-09-28T12:50:18.037',N'System',null,0,'2021-09-28T12:50:18.037',N'System',N'Crm.Project.Model.Relationships.PotentialContactRelationship','de7b74a1-5a20-ec11-98ac-001a7dda7115',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-09-28T12:50:18.090',N'System',null,0,'2021-09-28T12:50:18.090',N'System',N'Crm.Article.Model.ProductFamily','e07b74a1-5a20-ec11-98ac-001a7dda7115',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-09-28T12:50:18.137',N'System',null,0,'2021-09-28T12:50:18.137',N'System',N'Crm.Service.Model.Notes.ServiceOrderDispatchCompletedNote','e27b74a1-5a20-ec11-98ac-001a7dda7115',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2020-02-24T15:19:14.957',N'System',null,0,'2020-02-24T15:19:14.957',N'System',N'Crm.PerDiem.Model.PerDiemReport','0847e803-1957-ea11-9860-04d9f57ec1f7',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2020-03-30T14:15:04.153',N'System',null,0,'2020-03-30T14:15:04.153',N'System',N'Crm.Service.Model.Notes.ServiceCaseCreatedNote','28dd1bd9-9072-ea11-97f2-2c56dc77b601',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2020-06-03T09:47:53.347',N'System',null,0,'2020-06-03T09:47:53.347',N'System',N'Integration.Exchange.Model.ExchangeSyncConfig','9581da4a-7fa5-ea11-97f8-2c56dc77b601',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-12-15T09:43:49.617',N'System',null,0,'2021-12-15T09:43:49.617',N'System',N'Crm.MarketInsight.Model.MarketInsight','ac53ae80-8b5d-ec11-a73a-2cdb077b2ad1',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-12-15T09:43:49.707',N'System',null,0,'2021-12-15T09:43:49.707',N'System',N'Crm.MarketInsight.Model.Relationships.MarketInsightContactRelationship','b453ae80-8b5d-ec11-a73a-2cdb077b2ad1',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-12-15T09:43:49.763',N'System',null,0,'2021-12-15T09:43:49.763',N'System',N'Crm.Project.Model.DocumentEntry','b653ae80-8b5d-ec11-a73a-2cdb077b2ad1',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-07-07T13:21:53.477',N'System',null,0,'2021-07-07T13:21:53.477',N'System',N'Crm.ErpExtension.Model.CreditNote','603dfd4a-26df-eb11-9d20-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-07-07T13:21:53.497',N'System',null,0,'2021-07-07T13:21:53.497',N'System',N'Crm.ErpExtension.Model.DeliveryNote','613dfd4a-26df-eb11-9d20-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-07-07T13:21:53.497',N'System',null,0,'2021-07-07T13:21:53.497',N'System',N'Crm.ErpExtension.Model.Invoice','623dfd4a-26df-eb11-9d20-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-07-07T13:21:53.497',N'System',null,0,'2021-07-07T13:21:53.497',N'System',N'Crm.ErpExtension.Model.MasterContract','633dfd4a-26df-eb11-9d20-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-07-07T13:21:53.497',N'System',null,0,'2021-07-07T13:21:53.497',N'System',N'Crm.ErpExtension.Model.Quote','643dfd4a-26df-eb11-9d20-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-07-07T13:21:53.497',N'System',null,0,'2021-07-07T13:21:53.497',N'System',N'Crm.ErpExtension.Model.SalesOrder','653dfd4a-26df-eb11-9d20-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2021-07-07T13:21:53.500',N'System',null,0,'2021-07-07T13:21:53.500',N'System',N'Crm.ErpExtension.Model.ErpTurnover','663dfd4a-26df-eb11-9d20-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2023-03-06T10:54:00.200',N'System',null,0,'2023-03-06T10:54:00.200',N'System',N'Crm.Campaigns.Model.CampaignPerson','fbe5d832-0dbc-ed11-9d74-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2023-03-06T10:54:00.260',N'System',null,0,'2023-03-06T10:54:00.260',N'System',N'Crm.Campaigns.Model.CampaignCompany','01e6d832-0dbc-ed11-9d74-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2023-03-06T10:54:00.990',N'System',null,0,'2023-03-06T10:54:00.990',N'System',N'Crm.Article.Model.Relationships.ArticleCompanyRelationship','05e6d832-0dbc-ed11-9d74-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2023-03-06T11:00:41.740',N'System',null,0,'2023-03-06T11:00:41.740',N'System',N'Main.Flow.Model.FlowRule','5a372f22-0ebc-ed11-9d74-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2023-03-06T11:00:41.790',N'System',null,0,'2023-03-06T11:00:41.790',N'System',N'Crm.Model.UserSubscription','5c372f22-0ebc-ed11-9d74-3c7c3f1d6fc4',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2022-02-17T11:55:09.037',N'System',null,0,'2022-02-17T11:55:09.037',N'System',N'Crm.Library.Model.RecentPage','12ddf76f-e88f-ec11-8a49-806d970ef16d',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2022-02-17T11:55:09.200',N'System',null,0,'2022-02-17T11:55:09.200',N'System',N'Crm.PerDiem.Germany.Model.PerDiemAllowanceEntry','26ddf76f-e88f-ec11-8a49-806d970ef16d',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2022-02-17T11:55:09.390',N'System',null,0,'2022-02-17T11:55:09.390',N'System',N'Crm.Service.Model.ServiceCaseTemplate','28ddf76f-e88f-ec11-8a49-806d970ef16d',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2022-02-17T11:55:09.933',N'System',null,0,'2022-02-17T11:55:09.933',N'System',N'Crm.Model.CompanyBranch','39ddf76f-e88f-ec11-8a49-806d970ef16d',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2022-02-17T11:55:10.040',N'System',null,0,'2022-02-17T11:55:10.040',N'System',N'Crm.Project.Model.PotentialContactRelationship','3cddf76f-e88f-ec11-8a49-806d970ef16d',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Address','1314639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Relationships.BusinessRelationship','1414639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Bravo','1514639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Email','1614639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Fax','1714639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Phone','1814639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Website','1914639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Company','1a14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.333',N'System',null,0,'2018-05-24T15:17:50.333',N'System',N'Crm.Model.Folder','1b14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.Person','1c14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.DocumentAttribute','1d14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.FileResource','1e14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.LinkResource','1f14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.Message','2014639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.Notes.EmailNote','2114639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.Notes.TaskCompletedNote','2214639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Model.UserNote','2314639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Library.Model.Station','2414639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.337',N'System',null,0,'2018-05-24T15:17:50.337',N'System',N'Crm.Library.Model.User','2514639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.340',N'System',null,0,'2018-05-24T15:17:50.340',N'System',N'Crm.Model.Tag','2614639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.340',N'System',null,0,'2018-05-24T15:17:50.340',N'System',N'Crm.Model.Task','2714639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.353',N'System',null,0,'2018-05-24T15:17:50.353',N'System',N'Crm.Campaigns.Model.Campaign','2814639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.353',N'System',null,0,'2018-05-24T15:17:50.353',N'System',N'Crm.Campaigns.Model.CampaignParticipant','2914639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.353',N'System',null,0,'2018-05-24T15:17:50.353',N'System',N'Crm.Campaigns.Model.TaskTemplate','2a14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.357',N'System',null,0,'2018-05-24T15:17:50.357',N'System',N'Crm.Article.Model.Relationships.ArticleRelationship','2b14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.360',N'System',null,0,'2018-05-24T15:17:50.360',N'System',N'Crm.Article.Model.Article','2c14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.360',N'System',null,0,'2018-05-24T15:17:50.360',N'System',N'Crm.Configurator.Model.ConfigurationBase','2d14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.360',N'System',null,0,'2018-05-24T15:17:50.360',N'System',N'Crm.Configurator.Model.ConfigurationRule','2e14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.360',N'System',null,0,'2018-05-24T15:17:50.360',N'System',N'Crm.Configurator.Model.Variable','2f14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.363',N'System',null,0,'2018-05-24T15:17:50.363',N'System',N'Crm.DynamicForms.Model.DynamicFormLanguage','3014639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.720',N'System',null,0,'2018-05-24T15:17:50.720',N'System',N'Crm.Order.Model.Notes.BaseOrderCreatedNote','3114639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.720',N'System',null,0,'2018-05-24T15:17:50.720',N'System',N'Crm.Order.Model.Notes.BaseOrderStatusChangedNote','3214639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.720',N'System',null,0,'2018-05-24T15:17:50.720',N'System',N'Crm.Order.Model.Offer','3314639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.720',N'System',null,0,'2018-05-24T15:17:50.720',N'System',N'Crm.Order.Model.Order','3414639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.720',N'System',null,0,'2018-05-24T15:17:50.720',N'System',N'Crm.Order.Model.OrderItem','3514639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.720',N'System',null,0,'2018-05-24T15:17:50.720',N'System',N'Crm.Order.Model.CalculationPosition','3614639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.723',N'System',null,0,'2018-05-24T15:17:50.723',N'System',N'Crm.Project.Model.Notes.ProjectCreatedNote','3714639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.723',N'System',null,0,'2018-05-24T15:17:50.723',N'System',N'Crm.Project.Model.Notes.ProjectLostNote','3814639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.723',N'System',null,0,'2018-05-24T15:17:50.723',N'System',N'Crm.Project.Model.Notes.ProjectStatusChangedNote','3914639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.723',N'System',null,0,'2018-05-24T15:17:50.723',N'System',N'Crm.Project.Model.Relationships.ProjectContactRelationship','3a14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.723',N'System',null,0,'2018-05-24T15:17:50.723',N'System',N'Crm.Project.Model.Project','3b14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.867',N'System',null,0,'2018-05-24T15:17:50.867',N'System',N'Crm.Service.Model.Relationships.InstallationAddressRelationship','3c14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.867',N'System',null,0,'2018-05-24T15:17:50.867',N'System',N'Crm.Service.Model.Relationships.ServiceContractAddressRelationship','3d14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.867',N'System',null,0,'2018-05-24T15:17:50.867',N'System',N'Crm.Service.Model.Relationships.ServiceContractInstallationRelationship','3e14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.867',N'System',null,0,'2018-05-24T15:17:50.867',N'System',N'Crm.Service.Model.Notes.OrderStatusChangedNote','3f14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.867',N'System',null,0,'2018-05-24T15:17:50.867',N'System',N'Crm.Service.Model.Notes.ServiceCaseStatusChangedNote','4014639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.867',N'System',null,0,'2018-05-24T15:17:50.867',N'System',N'Crm.Service.Model.Notes.ServiceContractStatusChangedNote','4114639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.Notes.ServiceOrderHeadCreatedNote','4214639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.ServiceOrderDispatch','4314639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.Expense','4414639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.ExpenseReport','4514639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.Installation','4614639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.InstallationPos','4714639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.InstallationPosSerial','4814639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.Location','4914639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.MaintenancePlan','4a14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.RdsPpStructure','4b14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.ReplenishmentOrder','4c14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.ReplenishmentOrderItem','4d14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.ServiceCase','4e14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.ServiceContract','4f14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.870',N'System',null,0,'2018-05-24T15:17:50.870',N'System',N'Crm.Service.Model.ServiceObject','5014639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.ServiceOrderHead','5114639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.ServiceOrderMaterial','5214639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.ServiceOrderMaterialSerial','5314639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.ServiceOrderTime','5414639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.ServiceOrderTimePosting','5514639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.Store','5614639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.TimeEntryReport','5714639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.873',N'System',null,0,'2018-05-24T15:17:50.873',N'System',N'Crm.Service.Model.UserTimeEntry','5814639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.880',N'System',null,0,'2018-05-24T15:17:50.880',N'System',N'Crm.VisitReport.Model.Relationships.ContactPersonRelationship','5914639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.880',N'System',null,0,'2018-05-24T15:17:50.880',N'System',N'Crm.VisitReport.Model.Notes.VisitReportClosedNote','5a14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.880',N'System',null,0,'2018-05-24T15:17:50.880',N'System',N'Crm.VisitReport.Model.Notes.VisitReportTopicNote','5b14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.880',N'System',null,0,'2018-05-24T15:17:50.880',N'System',N'Crm.VisitReport.Model.Visit','5c14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.880',N'System',null,0,'2018-05-24T15:17:50.880',N'System',N'Crm.VisitReport.Model.VisitReport','5d14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.880',N'System',null,0,'2018-05-24T15:17:50.880',N'System',N'Crm.VisitReport.Model.VisitTopic','5e14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.933',N'System',null,0,'2018-05-24T15:17:50.933',N'System',N'Sms.Checklists.Model.ChecklistInstallationTypeRelationship','6114639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.933',N'System',null,0,'2018-05-24T15:17:50.933',N'System',N'Sms.Checklists.Model.ServiceOrderChecklist','6214639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.937',N'System',null,0,'2018-05-24T15:17:50.937',N'System',N'Sms.Einsatzplanung.Connector.Model.SchedulerConfig','6314639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.937',N'System',null,0,'2018-05-24T15:17:50.937',N'System',N'Sms.Einsatzplanung.Connector.Model.SchedulerIcon','6414639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.937',N'System',null,0,'2018-05-24T15:17:50.937',N'System',N'Sms.Einsatzplanung.Connector.Model.Scheduler','6514639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.937',N'System',null,0,'2018-05-24T15:17:50.937',N'System',N'Sms.Einsatzplanung.Connector.Model.RplServiceOrderDispatch','6614639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.937',N'System',null,0,'2018-05-24T15:17:50.937',N'System',N'Sms.Einsatzplanung.Connector.Model.RplTimePosting','6714639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.940',N'System',null,0,'2018-05-24T15:17:50.940',N'System',N'Einsatzplanung.Absence.Model.AbsenceDispatch','6814639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.940',N'System',null,0,'2018-05-24T15:17:50.940',N'System',N'Einsatzplanung.Absence.Model.GeneratedAbsenceDispatch','6914639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.940',N'System',null,0,'2018-05-24T15:17:50.940',N'System',N'Einsatzplanung.InforTimes.Model.InforTimesDispatch','6a14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.940',N'System',null,0,'2018-05-24T15:17:50.940',N'System',N'Einsatzplanung.Team.Model.TeamDispatch','6b14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.940',N'System',null,0,'2018-05-24T15:17:50.940',N'System',N'Einsatzplanung.Core.Model.Profile','6c14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.967',N'System',null,0,'2018-05-24T15:17:50.967',N'System',N'Sms.Einsatzplanung.Team.Model.TeamDispatchUser','6d14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-05-24T15:17:50.970',N'System',null,0,'2018-05-24T15:17:50.970',N'System',N'Sms.TimeManagement.Model.TimeManagementEvent','6e14639c-655f-e811-82d5-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-10-04T08:48:17.917',N'System',null,0,'2018-10-04T08:48:17.917',N'System',N'LMobile.Unicore.User','2b3f933c-b2c7-e811-82e6-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-10-04T08:48:18.657',N'System',null,0,'2018-10-04T08:48:18.657',N'System',N'LMobile.Unicore.Permission','2c3f933c-b2c7-e811-82e6-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-10-04T08:48:18.683',N'System',null,0,'2018-10-04T08:48:18.683',N'System',N'LMobile.Unicore.PermissionSchema','2d3f933c-b2c7-e811-82e6-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-10-04T08:48:18.703',N'System',null,0,'2018-10-04T08:48:18.703',N'System',N'LMobile.Unicore.PermissionSchemaCircle','2e3f933c-b2c7-e811-82e6-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-10-04T08:48:18.723',N'System',null,0,'2018-10-04T08:48:18.723',N'System',N'LMobile.Unicore.PermissionSchemaRole','2f3f933c-b2c7-e811-82e6-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2020-01-07T17:51:08.690',N'System',null,0,'2020-01-07T17:51:08.690',N'System',N'Sms.Checklists.Model.ServiceCaseChecklist','35e34948-7631-ea11-bc56-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2020-01-07T17:51:08.743',N'System',null,0,'2020-01-07T17:51:08.743',N'System',N'Crm.AttributeForms.Model.AttributeForm','36e34948-7631-ea11-bc56-901b0e84e2be',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-06-06T16:16:31.993',N'System',null,0,'2018-06-06T16:16:31.993',N'System',N'Crm.DynamicForms.Model.DynamicForm','72ead4f8-a469-e811-99d9-901b0ea6d4a0',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2018-06-08T10:14:50.407',N'System',null,0,'2018-06-08T10:14:50.407',N'System',N'Crm.Library.Model.Usergroup','0cea89c6-046b-e811-99da-901b0ea6d4a0',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2019-07-22T12:41:20.380',N'System',null,0,'2019-07-22T12:41:20.380',N'System',N'Crm.PerDiem.Model.UserExpense','2c3afb00-7eac-e911-9a3b-901b0ea6d4a0',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2019-07-22T12:41:20.420',N'System',null,0,'2019-07-22T12:41:20.420',N'System',N'Crm.PerDiem.Model.ExpenseReport','2d3afb00-7eac-e911-9a3b-901b0ea6d4a0',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2019-07-22T12:41:20.450',N'System',null,0,'2019-07-22T12:41:20.450',N'System',N'Crm.PerDiem.Model.UserTimeEntry','2e3afb00-7eac-e911-9a3b-901b0ea6d4a0',1)
INSERT [dbo].[EntityType]([CreatedAt],[CreatedBy],[DeletedAt],[IsDeleted],[ModifiedAt],[ModifiedBy],[Name],[UId],[Version])
VALUES('2019-07-22T12:41:20.490',N'System',null,0,'2019-07-22T12:41:20.490',N'System',N'Crm.PerDiem.Model.TimeEntryReport','2f3afb00-7eac-e911-9a3b-901b0ea6d4a0',1)
");
		}
	}
}
