namespace Main.Extensions
{
	using System;
	using System.Threading;

	using Medallion.Threading;

	public static class DistributedLockExtensions
	{
		public static IDistributedSynchronizationHandle GetLock(this IDistributedLockProvider lockProvider, string key, int expirySeconds = 30)
		{
			IDistributedSynchronizationHandle handle;
			while ((handle = lockProvider.TryAcquireLock(key, TimeSpan.FromSeconds(expirySeconds))) is null)
			{
				//to avoid two processes racing for the same lock continually blocking each other
				Thread.Sleep(new Random().Next(75, 126));
			}
			return handle;
		}
	}
}
