namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using LMobile.Unicore;

	[Migration(20190807173600)]
	public class AddUnicoreDocumentTables : Migration
	{
		public override void Up()
		{
			var useFileStream = (int)Database.ExecuteScalar("SELECT CASE WHEN CAST(SERVERPROPERTY('Edition') AS VARCHAR(64)) LIKE '%Azure%' THEN 1 ELSE 0 END") == 0;
			var helper = new UnicoreMigrationHelper(Database);

			if (!Database.TableExists("dbo.BinaryContent"))
			{
				Database.ExecuteNonQuery($@"
CREATE TABLE [dbo].[BinaryContent](
	[UId] [uniqueidentifier] NOT NULL,
	[Version] [bigint] NOT NULL,
	[ContentType] [nvarchar](256) NULL,
	[FileName] [nvarchar](256) NULL,
	[Length] [bigint] NULL,
	[HashCode] [nvarchar](256) NULL,
	[ReferenceCount] [bigint] NULL,
	[RowId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Data] [varbinary](max){(useFileStream ? " FILESTREAM" : string.Empty)} NULL,
 CONSTRAINT [PK_BinaryContent] PRIMARY KEY CLUSTERED 
(
	[UId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]{(useFileStream ? " FILESTREAM_ON [Files]" : string.Empty)},
UNIQUE NONCLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]{(useFileStream ? " FILESTREAM_ON [Files]" : string.Empty)}

ALTER TABLE [dbo].[BinaryContent] ADD  DEFAULT (newsequentialid()) FOR [RowId]
");
			}

			if (!Database.TableExists("dbo.DocumentContainer"))
			{
				Database.ExecuteNonQuery(@"
CREATE TABLE [dbo].[DocumentContainer](
	[UId] [uniqueidentifier] NOT NULL,
	[Version] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](256) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[EntityReferenceEntityId] [uniqueidentifier] NULL,
	[EntityReferenceEntityTypeId] [uniqueidentifier] NULL,
	[DocumentCount] [bigint] NOT NULL,
 CONSTRAINT [PK_DocumentContainer] PRIMARY KEY CLUSTERED 
(
	[UId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
");
			}

			if (!Database.TableExists("dbo.Document"))
			{
				Database.ExecuteNonQuery(@"
CREATE TABLE [dbo].[Document](
	[UId] [uniqueidentifier] NOT NULL,
	[Version] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](256) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedAt] [datetime2](7) NULL,
	[DomainId] [uniqueidentifier] NOT NULL,
	[EntityTypeId] [uniqueidentifier] NOT NULL,
	[AuthDataId] [uniqueidentifier] NULL,
	[Type] [nvarchar](256) NULL,
	[Description] [nvarchar](max) NULL,
	[ContentType] [nvarchar](256) NULL,
	[FileName] [nvarchar](256) NULL,
	[ContentId] [uniqueidentifier] NOT NULL,
	[ReferenceCount] [bigint] NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[UId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_AuthDataId] FOREIGN KEY([AuthDataId])
REFERENCES [dbo].[EntityAuthData] ([UId])

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_AuthDataId]

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_ContentId] FOREIGN KEY([ContentId])
REFERENCES [dbo].[BinaryContent] ([UId])

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_ContentId]

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_DomainId] FOREIGN KEY([DomainId])
REFERENCES [dbo].[Domain] ([UId])

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_DomainId]

ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_EntityTypeId] FOREIGN KEY([EntityTypeId])
REFERENCES [dbo].[EntityType] ([UId])

ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_EntityTypeId]
");
			}

			if (!Database.TableExists("dbo.DocumentThumbnail"))
			{
				Database.ExecuteNonQuery(@"
CREATE TABLE [dbo].[DocumentThumbnail](
	[UId] [uniqueidentifier] NOT NULL,
	[Version] [bigint] NOT NULL,
	[DocumentId] [uniqueidentifier] NOT NULL,
	[ContentId] [uniqueidentifier] NOT NULL,
	[Width] [bigint] NULL,
	[Height] [bigint] NULL,
 CONSTRAINT [PK_DocumentThumbnail] PRIMARY KEY CLUSTERED 
(
	[UId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[DocumentThumbnail]  WITH CHECK ADD  CONSTRAINT [FK_DocumentThumbnail_ContentId] FOREIGN KEY([ContentId])
REFERENCES [dbo].[BinaryContent] ([UId])

ALTER TABLE [dbo].[DocumentThumbnail] CHECK CONSTRAINT [FK_DocumentThumbnail_ContentId]

ALTER TABLE [dbo].[DocumentThumbnail]  WITH CHECK ADD  CONSTRAINT [FK_DocumentThumbnail_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([UId])

ALTER TABLE [dbo].[DocumentThumbnail] CHECK CONSTRAINT [FK_DocumentThumbnail_DocumentId]
");
			}

			if (!Database.TableExists("dbo.DocumentContainerElement"))
			{
				Database.ExecuteNonQuery(@"
CREATE TABLE [dbo].[DocumentContainerElement](
	[DocumentContainerId] [uniqueidentifier] NOT NULL,
	[DocumentId] [uniqueidentifier] NOT NULL
 ) ON [PRIMARY]

ALTER TABLE [dbo].[DocumentContainerElement]  WITH CHECK ADD  CONSTRAINT [FK_DocumentContainerElement_DocumentContainerId] FOREIGN KEY([DocumentContainerId])
REFERENCES [dbo].[DocumentContainer] ([UId])

ALTER TABLE [dbo].[DocumentContainerElement] CHECK CONSTRAINT [FK_DocumentContainerElement_DocumentContainerId]

ALTER TABLE [dbo].[DocumentContainerElement]  WITH CHECK ADD  CONSTRAINT [FK_DocumentContainerElement_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([UId])

ALTER TABLE [dbo].[DocumentContainerElement] CHECK CONSTRAINT [FK_DocumentContainerElement_DocumentId]
");
			}

			if (!Database.TableExists("dbo.DocumentProperty"))
			{
				Database.ExecuteNonQuery(@"
CREATE TABLE [dbo].[DocumentProperty](
	[DocumentId] [uniqueidentifier] NOT NULL,
	[PropertyKey] [nvarchar](256) NOT NULL,
	[PropertyValue] [nvarchar](4000) NULL,
 CONSTRAINT [PK_DocumentProperty] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC,
	[PropertyKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[DocumentProperty]  WITH CHECK ADD  CONSTRAINT [FK_DocumentProperty_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([UId])

ALTER TABLE [dbo].[DocumentProperty] CHECK CONSTRAINT [FK_DocumentProperty_DocumentId]
");
			}

			if (!Database.IndexExists("[dbo].[BinaryContent]", "IX_BinaryContent_Unique"))
			{
				Database.ExecuteNonQuery(@"
CREATE UNIQUE NONCLUSTERED INDEX [IX_BinaryContent_Unique] ON [dbo].[BinaryContent]
(
	[ContentType] ASC,
	[FileName] ASC,
	[Length] ASC,
	[HashCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
");
			}

			if (!Database.IndexExists("[dbo].[DocumentThumbnail]", "IX_DocumentThumbnail_Size"))
			{
				Database.ExecuteNonQuery(@"
CREATE NONCLUSTERED INDEX [IX_DocumentThumbnail_Size] ON [dbo].[DocumentThumbnail]
(
	[DocumentId] ASC,
	[Width] ASC,
	[Height] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
");
			}

			if (!Database.IndexExists("[dbo].[DocumentContainerElement]", "IX_DocumentContainerElement_DocumentContainer"))
			{
				Database.ExecuteNonQuery(@"
CREATE NONCLUSTERED INDEX [IX_DocumentContainerElement_DocumentContainer] ON [dbo].[DocumentContainerElement]
(
	[DocumentContainerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
");
			}

			if (!Database.IndexExists("[dbo].[DocumentProperty]", "IX_DocumentProperty_PropertyKeyValue"))
			{
				Database.ExecuteNonQuery(@"
CREATE NONCLUSTERED INDEX [IX_DocumentProperty_PropertyKeyValue] ON [dbo].[DocumentProperty]
(
	[PropertyKey] ASC,
	[PropertyValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
");
			}

			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Document>("dbo", "Document");
		}
	}
}
