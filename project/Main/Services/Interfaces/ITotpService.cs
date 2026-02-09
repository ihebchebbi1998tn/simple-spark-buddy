namespace Main.Services.Interfaces
{
	using System;

	using Crm.Library.AutoFac;

	public interface ITotpService : IDependency
	{
		string GenerateNewAuthorizationKey();
		string ComposeAuthorizationUri(string authorizationKey, string email);
		bool ValidateToken(string authorizationKey, string token);
		string EncryptAuthorizationKey(Guid userId, string plainToken);
		string DecryptAuthorizationKey(Guid userId, string cipheredToken);
		void RemoveHash(Guid userId);
		void ClearHashStore();
	}
}
