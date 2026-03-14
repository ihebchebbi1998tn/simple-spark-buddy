namespace Main.BackgroundServices
{
	using System;
	using System.Data;
	using System.Data.Common;
	using System.Linq;

	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;

	using LMobile.Unicore.NHibernate;

	using log4net;

	using Microsoft.Data.SqlClient;
	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class DatabaseCleanUpAgent : ManualSessionHandlingJobBase
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IUserService userService;
		private readonly ISessionProvider sessionProviderLocal;
		protected virtual int Timeout => appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.CommandTimeout);

		public DatabaseCleanUpAgent(ISessionProvider sessionProvider, ILog logger, IAppSettingsProvider appSettingsProvider, IUserService userService, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.sessionProviderLocal = sessionProvider;
			this.appSettingsProvider = appSettingsProvider;
			this.userService = userService;
		}

		protected override void Run(IJobExecutionContext context)
		{
			EndTransaction();

			CleanupRecentPages();
			CleanupPostings();
			CleanupLogs();
			CleanupReplicatedClients();
			CleanupMessages();
			CleanupPasswordResetTokens();
			IndexOptimize();
		}
		protected virtual void IndexOptimize()
		{
			if (receivedShutdownSignal)
			{
				return;
			}

			try
			{
				var builder = new DbConnectionStringBuilder { ConnectionString = appSettingsProvider.DbConnectionString };
				var initialCatalog = builder["Initial Catalog"] as string;

				using (var command = sessionProviderLocal.GetSession().Connection.CreateCommand())
				{
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "[dbo].[IndexOptimize]";
					command.CommandTimeout = Timeout;
					command.Parameters.Add(new SqlParameter("@Databases", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = initialCatalog });
					command.Parameters.Add(new SqlParameter("@FragmentationLow", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = null });
					command.Parameters.Add(new SqlParameter("@FragmentationMedium", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "INDEX_REORGANIZE,INDEX_REBUILD_ONLINE" });
					command.Parameters.Add(new SqlParameter("@FragmentationHigh", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "INDEX_REBUILD_ONLINE,INDEX_REBUILD_OFFLINE" });
					command.Parameters.Add(new SqlParameter("@UpdateStatistics", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "ALL" });
					command.Parameters.Add(new SqlParameter("@OnlyModifiedStatistics", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "Y" });
					command.Parameters.Add(new SqlParameter("@MinNumberOfPages", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = 1 });
					command.Parameters.Add(new SqlParameter("@FragmentationLevel1", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.FragmentationLevel1) });
					command.Parameters.Add(new SqlParameter("@FragmentationLevel2", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.FragmentationLevel2) });
					Logger.Debug("About to start Index optimizer");
					command.ExecuteNonQuery();
				}

				Logger.Info("Index Optimizer completed");
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
			}
		}
		protected virtual void CleanupMessages()
		{
			if (receivedShutdownSignal)
			{
				return;
			}

			try
			{
				BeginTransaction();
				using (var command = sessionProviderLocal.GetSession().Connection.CreateCommand())
				{
					command.Transaction = sessionProviderLocal.GetSession().GetSqlTransaction();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "[dbo].[DeleteEntityByDate]";
					command.CommandTimeout = Timeout;
					command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[CRM].[Message]" });
					command.Parameters.Add(new SqlParameter("@IdColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[MessageId]" });
					command.Parameters.Add(new SqlParameter("@DateColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[CreateDate]" });
					command.Parameters.Add(new SqlParameter("@DeprecationDays", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = appSettingsProvider.GetValue(MainPlugin.Settings.Maintenance.MessageDeprecationDays) });
					command.Parameters.Add(new SqlParameter("@BatchSize", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = 1000 });
					Logger.Debug("About to delete deprecated messages");
					command.ExecuteNonQuery();
				}
				EndTransaction();
				Logger.Info("Deleting deprecated messages completed");
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
				RollbackTransaction();
			}
		}protected virtual void CleanupPasswordResetTokens()
		{
			if (receivedShutdownSignal)
			{
				return;
			}

			try
			{
				BeginTransaction();
				using (var command = sessionProviderLocal.GetSession().Connection.CreateCommand())
				{
					command.Transaction = sessionProviderLocal.GetSession().GetSqlTransaction();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "[dbo].[DeleteEntityByDate]";
					command.CommandTimeout = Timeout;
					command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[dbo].[PasswordResetToken]" });
					command.Parameters.Add(new SqlParameter("@IdColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[Id]" });
					command.Parameters.Add(new SqlParameter("@DateColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[ExpiryDate]" });
					command.Parameters.Add(new SqlParameter("@DeprecationDays", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = appSettingsProvider.GetValue(MainPlugin.Settings.Maintenance.PasswordResetTokenDeprecationDays) });
					command.Parameters.Add(new SqlParameter("@BatchSize", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = 1000 });
					Logger.Debug("About to delete deprecated password reset tokens");
					command.ExecuteNonQuery();
				}
				EndTransaction();
				Logger.Info("Deleting deprecated password reset tokens completed");
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
				RollbackTransaction();
			}
		}
		protected virtual void CleanupReplicatedClients()
		{
			if (receivedShutdownSignal)
			{
				return;
			}

			try
			{
				var deprecationDays = appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.ReplicatedClientDeprecationDays);
				// Excluding ReplicatedClientName IS NOT NULL as they are used for OData replication purposes 
				var expiredClients = sessionProviderLocal.GetSession().CreateQuery("SELECT rc.Id FROM Main.Replication.Model.ReplicatedClient rc WHERE rc.LastSync <= GetDate() - :deprecationDays AND rc.Name IS NULL ORDER BY rc.LastSync DESC")
					.SetInt32("deprecationDays", deprecationDays)
					.List<Guid>();
				var counter = 0;
				Logger.Info($"Found {expiredClients.Count} replications older than {deprecationDays} days");
				foreach (var id in expiredClients)
				{
					if (receivedShutdownSignal)
					{
						return;
					}

					BeginTransaction();
					using (var command = sessionProviderLocal.GetSession().Connection.CreateCommand())
					{
						command.Transaction = sessionProviderLocal.GetSession().GetSqlTransaction();
						command.CommandType = CommandType.StoredProcedure;
						command.CommandText = "[dbo].[DeleteEntityByDate]";
						command.CommandTimeout = Timeout;
						command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[dbo].[ReplicatedClient]" });
						command.Parameters.Add(new SqlParameter("@IdColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[ReplicatedClientId]" });
						command.Parameters.Add(new SqlParameter("@DateColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[LastSync]" });
						command.Parameters.Add(new SqlParameter("@DeprecationDays", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.ReplicatedClientDeprecationDays) });
						command.Parameters.Add(new SqlParameter("@BatchSize", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = 1 });
						command.Parameters.Add(new SqlParameter("@AdditionalConditions", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = $" AND mt.[ReplicatedClientId] = CONVERT(uniqueidentifier, '{id}') " });
						Logger.Debug($"About to delete deprecated ReplicatedClient with Id {id}");
						command.ExecuteNonQuery();
					}

					EndTransaction();
					counter++;
					Logger.Debug($"Deleted {counter} clients of {expiredClients.Count}");
				}

				Logger.Info($"Deleting {expiredClients.Count} deprecated ReplicatedClients completed");
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
				RollbackTransaction();
			}
		}
		protected virtual void CleanupPostings()
		{
			if (receivedShutdownSignal)
			{
				return;
			}

			try
			{
				BeginTransaction();
				using (var command = sessionProviderLocal.GetSession().Connection.CreateCommand())
				{
					command.Transaction = sessionProviderLocal.GetSession().GetSqlTransaction();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "[dbo].[DeleteEntityByDate]";
					command.CommandTimeout = Timeout;
					command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[CRM].[Posting]" });
					command.Parameters.Add(new SqlParameter("@IdColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[PostingId]" });
					command.Parameters.Add(new SqlParameter("@DateColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[CreateDate]" });
					command.Parameters.Add(new SqlParameter("@DeprecationDays", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.PostingDeprecationDays) });
					command.Parameters.Add(new SqlParameter("@AdditionalConditions", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = " AND mt.[State] IN ('Processed', 'Skipped', 'Stale')" });
					command.Parameters.Add(new SqlParameter("@BatchSize", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = 1000 });
					Logger.Debug("About to delete deprecated postings");
					command.ExecuteNonQuery();
				}
				EndTransaction();
				Logger.Info("Deleting deprecated postings completed");
			}
			catch (Exception exception)
			{
				RollbackTransaction();
				Logger.Error(exception);
			}
		}
		protected virtual void CleanupLogs()
		{
			if (receivedShutdownSignal)
			{
				return;
			}

			try
			{
				BeginTransaction();
				using (var command = sessionProviderLocal.GetSession().Connection.CreateCommand())
				{
					command.Transaction = sessionProviderLocal.GetSession().GetSqlTransaction();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "[dbo].[DeleteEntityByDate]";
					command.CommandTimeout = Timeout;
					command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[CRM].[Log]" });
					command.Parameters.Add(new SqlParameter("@IdColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[Id]" });
					command.Parameters.Add(new SqlParameter("@DateColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[Date]" });
					command.Parameters.Add(new SqlParameter("@DeprecationDays", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.LogDeprecationDays) });
					command.Parameters.Add(new SqlParameter("@BatchSize", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = 10000 });
					Logger.Debug("About to delete deprecated logs");
					command.ExecuteNonQuery();
				}
				EndTransaction();
				Logger.Info("Deleting deprecated logs completed");
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
				RollbackTransaction();
			}
		}
		protected virtual void CleanupRecentPages()
		{
			foreach (var user in userService.GetUsers())
			{
				if (receivedShutdownSignal)
				{
					break;
				}

				var lastRecentPageIds = sessionProviderLocal.GetSession().CreateQuery("SELECT r.Id FROM Crm.Library.Model.RecentPage r WHERE r.Username = :userId ORDER BY r.CreateDate DESC")
					.SetString("userId", user.Id)
					.SetMaxResults(appSettingsProvider.GetValue<int>(MainPlugin.Settings.Maintenance.AmountOfRecentPagesToKeep))
					.List<Guid>();
				if (lastRecentPageIds.Any())
				{
					try
					{
						BeginTransaction();
						using (var command = sessionProviderLocal.GetSession().Connection.CreateCommand())
						{
							command.Transaction = sessionProviderLocal.GetSession().GetSqlTransaction();
							command.CommandType = CommandType.StoredProcedure;
							command.CommandText = "[dbo].[DeleteEntityByDate]";
							command.CommandTimeout = Timeout;
							command.Parameters.Add(new SqlParameter("@TableName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[CRM].[UserRecentPages]" });
							command.Parameters.Add(new SqlParameter("@IdColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[Id]" });
							command.Parameters.Add(new SqlParameter("@DateColumnName", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = "[CreateDate]" });
							command.Parameters.Add(new SqlParameter("@DeprecationDays", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = 0 });
							command.Parameters.Add(new SqlParameter("@AdditionalConditions", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = $" AND mt.[Username] = '{user.Id}' AND mt.Id NOT IN ('{string.Join("', '", lastRecentPageIds)}')" });
							command.Parameters.Add(new SqlParameter("@BatchSize", SqlDbType.Text) { Direction = ParameterDirection.Input, Value = 1000 });
							Logger.Debug($"About to delete recent pages for User {user.Id}");
							command.ExecuteNonQuery();
						}

						EndTransaction();
						Logger.Debug($"Successfully deleted recent pages for User {user.Id}");
					}
					catch (Exception exception)
					{
						Logger.Error($"Error deleting recent pages", exception);
						RollbackTransaction();
					}
				}
			}
			Logger.Info("Deleting recent pages completed");
		}
	}
}
