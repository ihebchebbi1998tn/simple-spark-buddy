namespace Main.Services
{
	using System;

	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;

	using log4net;

	using Main.Extensions;
	using Main.Model;
	using Main.Services.Interfaces;

	using Medallion.Threading;

	using Microsoft.Extensions.Caching.Distributed;

	using NHibernate;

	public class NumberingService : INumberingService
	{
		private readonly ISessionProvider sessionProvider;
		private readonly ILog logger;
		private readonly IDistributedLockProvider lockProvider;
		private readonly IDistributedCache cache;
		private readonly IInterceptor interceptor;

		public NumberingService(ISessionProvider sessionProvider, ILog logger, IDistributedLockProvider lockProvider, IDistributedCache cache, IInterceptor interceptor)
		{
			this.sessionProvider = sessionProvider;
			this.logger = logger;
			this.lockProvider = lockProvider;
			this.cache = cache;
			this.interceptor = interceptor;
		}
		protected virtual NumberingSequence GetNumberingSequence(string sequenceName)
		{
			var lockKey = $"NumberingService.GetNumberingSequence.{sequenceName}";
			using var _ = lockProvider.GetLock(lockKey);
			using var newSession = sessionProvider.GetSession().SessionFactory.WithOptions().Interceptor(interceptor).OpenSession();
			try
			{
				var selectQuery = newSession.CreateQuery($"FROM {typeof(NumberingSequence).FullName} s WHERE {nameof(NumberingSequence.SequenceName)} = :sequenceName");
				selectQuery.SetString("sequenceName", sequenceName);
				var result = selectQuery.UniqueResult<NumberingSequence>();

				if (result != null)
				{
					result.LastNumber += 1;
					var updateQuery = newSession.CreateSQLQuery("UPDATE [dbo].[NumberingSequence] SET [LastNumber] = :lastNumber WHERE [SequenceName] = :sequenceName");
					updateQuery.SetString("sequenceName", sequenceName);
					updateQuery.SetInt64("lastNumber", result.LastNumber);
					updateQuery.ExecuteUpdate();
				}
				else
				{
					logger.WarnFormat("Unable to retrieve results for sequence: {0}. NumberingSequence may not be available in dbo.NumberingSequence", sequenceName.RemoveLineBreaks());
				}

				return result;
			}
			catch (Exception ex)
			{
				logger.ErrorFormat("GetNumberingSequence failed: {0}", ex.Message);
				throw;
			}
		}
		protected virtual bool HasNumberingSequence(string sequenceName)
		{
			var session = sessionProvider.GetSession();
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					var selectQuery = session.CreateQuery($"FROM {typeof(NumberingSequence).FullName} s WHERE {nameof(NumberingSequence.SequenceName)} = :sequenceName");
					selectQuery.SetString("sequenceName", sequenceName);
					var result = selectQuery.UniqueResult<NumberingSequence>();
					transaction.Commit();
					if (result != null) return true;
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					logger.ErrorFormat("HasNumberingSequence failed: {0}", ex.Message);
					throw;
				}
			}

			return false;
		}
		public virtual string GetNextFormattedNumber(string sequenceName)
		{
			var numberingSequence = GetNumberingSequence(sequenceName);
			if (numberingSequence == null) return null;
			return numberingSequence.ToString();
		}
		public virtual long? GetNextHighValue(string sequenceName)
		{
			var numberingSequence = GetNumberingSequence(sequenceName);
			if (numberingSequence == null || numberingSequence.MaxLow == null) return null;
			return numberingSequence.LastNumber;
		}
		public virtual bool NumberingSequenceExists(string sequenceName)
		{
			var key = $"NumberingSequence.{sequenceName}";
			var str = cache.GetString(key);
			if (str is null || bool.TryParse(str, out var exists) == false)
			{
				exists = HasNumberingSequence(sequenceName);
				cache.SetString(key, exists.ToString(), new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });
			}
			return exists;
		}
	}
}
