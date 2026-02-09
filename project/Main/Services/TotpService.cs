namespace Main.Services
{
	using OtpNet;

	using Main.Services.Interfaces;

	using System.Security.Cryptography;
	using System.Text;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.IO;
	using System.Text.Json;

	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.AutoFac;

	public class TotpService : ITotpService
	{
		readonly AesEncryptionService encryptionService;
		readonly UserHashService userHashService;

		public virtual string GenerateNewAuthorizationKey()
		{
			return Base32Encoding.ToString(KeyGeneration.GenerateRandomKey());
		}

		public virtual string ComposeAuthorizationUri(string authorizationKey, string email)
		{
			return new OtpUri(OtpType.Totp, authorizationKey, email, "L-mobile").ToString();
		}

		public virtual bool ValidateToken(string authorizationKey, string token)
		{
			var totp = new Totp(Base32Encoding.ToBytes(authorizationKey));
			return totp.VerifyTotp(token, out long timeStepMatched, null);
		}

		public virtual string EncryptAuthorizationKey(Guid userId, string plainToken)
		{
			byte[] hash = userHashService.GenerateHash(userId);
			userHashService.SaveHash(userId, hash);

			return encryptionService.Encrypt(plainToken, hash);
		}

		public virtual string DecryptAuthorizationKey(Guid userId, string cipheredToken)
		{
			string base64Hash = userHashService.GetHash(userId);
			if (base64Hash == null)
			{
				throw new InvalidOperationException("Hash not found for the specified userId.");
			}

			byte[] hash = Convert.FromBase64String(base64Hash);

			return encryptionService.Decrypt(cipheredToken, hash);
		}

		public virtual void RemoveHash(Guid userId)
		{
			userHashService.RemoveHash(userId);
		}

		public virtual void ClearHashStore()
		{
			userHashService.ClearHashStore();
		}

		public TotpService(UserHashService userHashService, AesEncryptionService encryptionService)
		{
			this.userHashService = userHashService;
			this.encryptionService = encryptionService;
		}

		public class UserHashService : IDependency
		{
			private readonly string filePath;
			private readonly object fileLock = new object();

			public UserHashService(IEnvironment environment)
			{
				this.filePath = Path.Combine(environment.AppDataPath.FullName, "config", "totp-hashes.json");
			}

			public virtual byte[] GenerateHash(Guid userId)
			{
				byte[] salt = GenerateSalt();

				byte[] userIdBytes = userId.ToByteArray();
				byte[] combinedBytes = new byte[salt.Length + userIdBytes.Length];
				Buffer.BlockCopy(salt, 0, combinedBytes, 0, salt.Length);
				Buffer.BlockCopy(userIdBytes, 0, combinedBytes, salt.Length, userIdBytes.Length);

				using (var pbkdf2 = new Rfc2898DeriveBytes(combinedBytes, salt, 10000, HashAlgorithmName.SHA256))
				{
					return pbkdf2.GetBytes(32);
				}
			}

			public virtual void SaveHash(Guid userId, byte[] hash)
			{
				var userHashes = LoadUserHashes();
				userHashes[userId] = Convert.ToBase64String(hash);

				string json = JsonSerializer.Serialize(userHashes);
				lock (fileLock)
				{
					File.WriteAllText(filePath, json);
				}
			}

			public virtual Dictionary<Guid, string> LoadUserHashes()
			{
				lock (fileLock)
				{
					if (!File.Exists(filePath))
					{
						return new Dictionary<Guid, string>();
					}

					string json = File.ReadAllText(filePath);
					return string.IsNullOrEmpty(json)
						? new Dictionary<Guid, string>()
						: JsonSerializer.Deserialize<Dictionary<Guid, string>>(json) ?? new Dictionary<Guid, string>();
				}
			}

			public virtual string GetHash(Guid userId)
			{
				var userHashes = LoadUserHashes();
				userHashes.TryGetValue(userId, out var hash);
				return hash;
			}

			public virtual void RemoveHash(Guid userId)
			{
				var userHashes = LoadUserHashes();
				if (userHashes.Remove(userId))
				{
					string json = JsonSerializer.Serialize(userHashes);
					lock (fileLock)
					{
						File.WriteAllText(filePath, json);
					}
				}
			}

			public virtual void ClearHashStore()
			{
				lock (fileLock)
				{
					File.WriteAllText(filePath, string.Empty);
				}
			}

			protected virtual byte[] GenerateSalt()
			{
				byte[] salt = new byte[16];
				using (var rng = RandomNumberGenerator.Create())
				{
					rng.GetBytes(salt);
				}
				return salt;
			}
		}

		public class AesEncryptionService : IDependency
		{
			public virtual string Encrypt(string plainText, byte[] encryptionKey)
			{
				using var aes = new AesCcm(encryptionKey);

				var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
				RandomNumberGenerator.Fill(nonce);
				var plaintextBytes = Encoding.UTF8.GetBytes(plainText);
				var ciphertextBytes = new byte[plaintextBytes.Length];
				var tag = new byte[AesGcm.TagByteSizes.MaxSize];

				aes.Encrypt(nonce, plaintextBytes, ciphertextBytes, tag);
				return new AesGcmCiphertext(nonce, tag, ciphertextBytes).ToString();
			}

			public virtual string Decrypt(string cipheredText, byte[] encryptionKey)
			{
				var gcmCiphertext = AesGcmCiphertext.FromBase64String(cipheredText);
				using var aes = new AesCcm(encryptionKey);
				var plaintextBytes = new byte[gcmCiphertext.CiphertextBytes.Length];

				aes.Decrypt(gcmCiphertext.Nonce, gcmCiphertext.CiphertextBytes, gcmCiphertext.Tag, plaintextBytes);
				return Encoding.UTF8.GetString(plaintextBytes);
			}

			private sealed class AesGcmCiphertext
			{
				public byte[] Nonce { get; }
				public byte[] Tag { get; }
				public byte[] CiphertextBytes { get; }

				public static AesGcmCiphertext FromBase64String(string data)
				{
					var dataBytes = Convert.FromBase64String(data);
					return new AesGcmCiphertext(
						dataBytes.Take(AesGcm.NonceByteSizes.MaxSize).ToArray(),
						dataBytes[^AesGcm.TagByteSizes.MaxSize..],
						dataBytes[AesGcm.NonceByteSizes.MaxSize..^AesGcm.TagByteSizes.MaxSize]
					);
				}

				public AesGcmCiphertext(byte[] nonce, byte[] tag, byte[] ciphertextBytes)
				{
					Nonce = nonce;
					Tag = tag;
					CiphertextBytes = ciphertextBytes;
				}

				public override string ToString()
				{
					return Convert.ToBase64String(Nonce.Concat(CiphertextBytes).Concat(Tag).ToArray());
				}
			}
		}
	}
}
