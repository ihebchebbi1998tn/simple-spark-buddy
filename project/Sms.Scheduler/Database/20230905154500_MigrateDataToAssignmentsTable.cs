namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Sms.Scheduler.Model;

	[Migration(20230905154500)]
	public class MigrateDataToAssignmentsTable : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[DispatchPersonAssignment]"))
			{
				var helper = new UnicoreMigrationHelper(Database);
				var dispatchPersonAssignmentTypeId = helper.AddOrGetEntityTypeId<DispatchPersonAssignment>();
				var dispatchPersonAssignmentHasAuthDataId = Database.ColumnExists("[SMS].[DispatchPersonAssignment]", "AuthDataId");

				#region [SMS].[ServiceOrderDispatch] to [SMS].[DispatchPersonAssignment]
				Database.ExecuteNonQuery(@"INSERT INTO [SMS].[DispatchPersonAssignment]
					([DispatchKey], [ResourceKey], [CreateUser], [ModifyUser], [CreateDate], [ModifyDate], [IsActive]) 
					SELECT  [DispatchId], 
					d.[Username],
					'Setup',
					'Setup',
					GETDATE(),
					GETDATE(),
					1
					FROM [SMS].[ServiceOrderDispatch] d join [CRM].[User] u on d.Username = u.Username
					WHERE d.[IsActive] = 1");

				if (dispatchPersonAssignmentHasAuthDataId && Database.ColumnExists("[SMS].[ServiceOrderDispatch]", "AuthDataId"))
				{
					Database.ExecuteNonQuery(@$"
						INSERT INTO 
							[dbo].[EntityAuthData] (EntityId, EntityTypeId, DomainId)
						SELECT
							dpa.DispatchPersonAssignmentId,
							'{dispatchPersonAssignmentTypeId}',
							ead.DomainId
						FROM 
							[SMS].[ServiceOrderDispatch] [source]
							inner join [SMS].[DispatchPersonAssignment] dpa on source.DispatchId = dpa.DispatchKey
							inner join EntityAuthData ead on source.AuthDataId = ead.UId
						WHERE
							dpa.AuthDataId is null;
						");
				}
				#endregion

				#region [RPL].[Dispatch] absences to [SMS].[Absence]
				Database.AddColumnIfNotExisting("[SMS].[Absence]",
					new Column("RplDispatchId",
						DbType.Int32,
						ColumnProperty.Null));

				if (Database.TableExists("[RPL].[Dispatch]"))
				{
					Database.ExecuteNonQuery(@"
					INSERT INTO [SMS].[Absence]
						([Description]
						,[TimeEntryTypeKey]
						,[ResponsibleUser]
						,[CreateDate]
						,[CreateUser]
						,[ModifyDate]
						,[ModifyUser]
						,[From]
						,[To]
						,[IsActive]
						,[RplDispatchId])
					SELECT 
						cast(d.[InternalInformation] as nvarchar(4000)),
						d.[AbsenceTypeKey],
						d.[ResourceKey],
						GETDATE(),
						'Setup',
						GETDATE(),
						'Setup',
						d.[Start],
						d.[Stop],
						d.[IsActive],
						d.Id
					FROM [RPL].[Dispatch] d where [Type]='Absence' and d.[IsActive] = 1
					");
				}

				if (Database.ColumnExists("[RPL].[Dispatch]", "AuthDataId") && Database.ColumnExists("[SMS].[Absence]", "AuthDataId"))
				{
					var absenceTypeId = helper.AddOrGetEntityTypeId<Absence>();

					Database.ExecuteNonQuery(@$"
						INSERT INTO 
							[dbo].[EntityAuthData] (EntityId, EntityTypeId, DomainId)
						SELECT
							ab.AbsenceId,
							'{absenceTypeId}',
							ead.DomainId
						FROM 
							[RPL].[Dispatch] [source]
							inner join [SMS].[Absence] ab on source.Id = ab.RplDispatchId
							inner join EntityAuthData ead on source.AuthDataId = ead.UId
						WHERE
							ab.AuthDataId is null;

						UPDATE
							[SMS].[Absence]
						SET
							AuthDataId = ead.UId
						FROM
							[SMS].[Absence] ab
							INNER JOIN [dbo].[EntityAuthData] ead on ab.AbsenceId = ead.EntityId
						WHERE
							ab.AuthDataId is null;
						");
				}

				Database.RemoveColumnIfExisting("[SMS].[Absence]", "RplDispatchId");

				#endregion
				#region [SMS].[Absence] to [SMS].[DispatchPersonAssignment]
				Database.ExecuteNonQuery(@"INSERT INTO [SMS].[DispatchPersonAssignment]
					([DispatchKey], [ResourceKey], [CreateUser], [ModifyUser], [CreateDate], [ModifyDate], [IsActive]) 
					SELECT  [AbsenceId], 
					[ResponsibleUser],
					'Setup',
					'Setup',
					GETDATE(),
					GETDATE(),
					1
					FROM [SMS].[Absence] te join [CRM].[User] u on te.ResponsibleUser = u.Username
					WHERE te.[IsActive] = 1");
				
					if (dispatchPersonAssignmentHasAuthDataId && Database.ColumnExists("[SMS].[Absence]", "AuthDataId"))
					{
						Database.ExecuteNonQuery(@$"
							INSERT INTO 
								[dbo].[EntityAuthData] (EntityId, EntityTypeId, DomainId)
							SELECT
								dpa.DispatchPersonAssignmentId,
								'{dispatchPersonAssignmentTypeId}',
								ead.DomainId
							FROM 
								[SMS].[Absence] [source]
								inner join [SMS].[DispatchPersonAssignment] dpa on source.[AbsenceId] = dpa.DispatchKey
								inner join EntityAuthData ead on source.AuthDataId = ead.UId
							WHERE
								dpa.AuthDataId is null;
							");
					}
				#endregion

				#region [SMS].[TimeEntry] to [SMS].[DispatchPersonAssignment]
				Database.ExecuteNonQuery(@"INSERT INTO [SMS].[DispatchPersonAssignment]
					([DispatchKey], [ResourceKey], [CreateUser], [ModifyUser], [CreateDate], [ModifyDate], [IsActive]) 
					SELECT  [TimeEntryId], 
					[ResponsibleUser],
					'Setup',
					'Setup',
					GETDATE(),
					GETDATE(),
					1
					FROM [SMS].[TimeEntry] te join [CRM].[User] u on te.ResponsibleUser = u.Username
					WHERE te.[IsActive] = 1");

				if (dispatchPersonAssignmentHasAuthDataId && Database.ColumnExists("[SMS].[TimeEntry]", "AuthDataId"))
				{
					Database.ExecuteNonQuery(@$"
						INSERT INTO 
							[dbo].[EntityAuthData] (EntityId, EntityTypeId, DomainId)
						SELECT
							dpa.DispatchPersonAssignmentId,
							'{dispatchPersonAssignmentTypeId}',
							ead.DomainId
						FROM 
							[SMS].[TimeEntry] [source]
							inner join [SMS].[DispatchPersonAssignment] dpa on source.TimeEntryId = dpa.DispatchKey
							inner join EntityAuthData ead on source.AuthDataId = ead.UId
						WHERE
							dpa.AuthDataId is null;
						");
				}
				#endregion

				#region [SMS].[ServiceOrderTimePostings] to [SMS].[DispatchPersonAssignment]
				Database.ExecuteNonQuery(@"INSERT INTO [SMS].[DispatchPersonAssignment]
					([DispatchKey], [ResourceKey], [CreateUser], [ModifyUser], [CreateDate], [ModifyDate], [IsActive]) 
					SELECT  [id], 
					[UserUsername],
					'Setup',
					'Setup',
					GETDATE(),
					GETDATE(),
					1
					FROM [SMS].[ServiceOrderTimePostings] tp join [CRM].[User] u on tp.UserUsername = u.Username
					WHERE tp.[IsActive] = 1 AND [DispatchId] IS NOT NULL");

				if (dispatchPersonAssignmentHasAuthDataId && Database.ColumnExists("[SMS].[ServiceOrderTimePostings]", "AuthDataId"))
				{
					Database.ExecuteNonQuery(@$"
						INSERT INTO 
							[dbo].[EntityAuthData] (EntityId, EntityTypeId, DomainId)
						SELECT
							dpa.DispatchPersonAssignmentId,
							'{dispatchPersonAssignmentTypeId}',
							ead.DomainId
						FROM 
							[SMS].[ServiceOrderTimePostings] [source]
							inner join [SMS].[DispatchPersonAssignment] dpa on source.id = dpa.DispatchKey
							inner join EntityAuthData ead on source.AuthDataId = ead.UId
						WHERE
							dpa.AuthDataId is null;
						");
				}
				#endregion

				#region Set [SMS].[DispatchPersonAssignment].AuthDataId
				if (dispatchPersonAssignmentHasAuthDataId)
				{
					Database.ExecuteNonQuery(@$"
						UPDATE
							[SMS].[DispatchPersonAssignment]
						SET
							AuthDataId = ead.UId
						FROM
							[SMS].[DispatchPersonAssignment] dpa
							INNER JOIN [dbo].[EntityAuthData] ead on dpa.DispatchPersonAssignmentId = ead.EntityId
						WHERE
							dpa.AuthDataId is null;
						");
				}
				#endregion

				helper.AddOrUpdateEntityAuthDataColumn<DispatchPersonAssignment>("SMS", "DispatchPersonAssignment");
			}
			if (Database.TableExists("[SMS].[DispatchArticleAssignment]"))
			{
				var helper = new UnicoreMigrationHelper(Database);
				var dispatchArticleAssignmentTypeId = helper.AddOrGetEntityTypeId<DispatchArticleAssignment>();
				var dispatchArticleAssignmentHasAuthDataId = Database.ColumnExists("[SMS].[DispatchArticleAssignment]", "AuthDataId");

				#region [CRM].[ArticleDowntime] to [SMS].[DispatchArticleAssignment]
				Database.ExecuteNonQuery(@"INSERT INTO [SMS].[DispatchArticleAssignment]
					([DispatchKey], [ResourceKey], [CreateUser], [ModifyUser], [CreateDate], [ModifyDate], [IsActive]) 
					 SELECT  [ArticleDowntimeId], 
					[ArticleKey],
					'Setup',
					'Setup',
					GETDATE(),
					GETDATE(),
					1
					FROM [CRM].[ArticleDowntime]
					WHERE [IsActive] = 1");

				if (dispatchArticleAssignmentHasAuthDataId && Database.ColumnExists("[CRM].[ArticleDowntime]", "AuthDataId"))
				{
					Database.ExecuteNonQuery(@$"
						INSERT INTO 
							[dbo].[EntityAuthData] (EntityId, EntityTypeId, DomainId)
						SELECT
							daa.DispatchArticleAssignmentId,
							'{dispatchArticleAssignmentTypeId}',
							ead.DomainId
						FROM 
							[CRM].[ArticleDowntime] [source]
							inner join [SMS].[DispatchArticleAssignment] daa on source.ArticleDowntimeId = daa.DispatchKey
							inner join EntityAuthData ead on source.AuthDataId = ead.UId
						WHERE
							daa.AuthDataId is null;
						");

					Database.ExecuteNonQuery(@$"
						UPDATE
							[SMS].[DispatchArticleAssignment]
						SET
							AuthDataId = ead.UId
						FROM
							[SMS].[DispatchArticleAssignment] daa
							INNER JOIN [dbo].[EntityAuthData] ead on daa.DispatchArticleAssignmentId = ead.EntityId
						WHERE
							daa.AuthDataId is null;
						");
				}
				#endregion

				helper.AddOrUpdateEntityAuthDataColumn<DispatchArticleAssignment>("SMS", "DispatchArticleAssignment");
			}
		}
	}
}
