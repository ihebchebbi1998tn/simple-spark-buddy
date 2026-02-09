namespace Crm.Service.Model.Extensions
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;

	public static class InstallationExtensions
	{
		public static Installation Get(this IRepositoryWithTypedId<Installation, Guid> installationRepository, string installationNo)
		{
			return String.IsNullOrWhiteSpace(installationNo) ? null : installationRepository.GetAll().FirstOrDefault(i => i.InstallationNo == installationNo);
		}
	}
}