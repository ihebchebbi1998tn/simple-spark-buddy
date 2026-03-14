namespace Crm.Service.Database
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230207131000)]
	public class MigrateInstallationAdditionalContactsToNewTable : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[InstallationAdditionalContacts]"))
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("SELECT a.[ContactId], [InstallationId]");
				sb.AppendLine("FROM [SMS].[InstallationAdditionalContacts] a");
				sb.AppendLine("JOIN [CRM].[Contact] b ON a.[ContactId] = b.[ContactId]");
				sb.AppendLine("Where [ContactType] = 'Company'");
				var installationCompanies = new List<KeyValuePair<Guid, Guid>>();
				using (IDataReader reader = Database.ExecuteQuery(sb.ToString()))
				{
					while (reader.Read())
					{
						var installation = (Guid)reader["InstallationId"];
						var company = (Guid)reader["ContactId"];
						installationCompanies.Add(new KeyValuePair<Guid, Guid>(installation, company));
					}
				}

				if (installationCompanies.Count > 0)
				{
					if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[InstallationCompanyRelationshipType] WHERE Value = 'Other'") == 0)
					{
						//Insert new lookup line
						var query = new StringBuilder();

						query.AppendLine($"INSERT INTO [LU].[InstallationCompanyRelationshipType] (Name, Language, Value) VALUES ('Andere', 'de', 'Other')");
						query.AppendLine($"INSERT INTO [LU].[InstallationCompanyRelationshipType] (Name, Language, Value) VALUES ('Other', 'en', 'Other')");
						query.AppendLine($"INSERT INTO [LU].[InstallationCompanyRelationshipType] (Name, Language, Value) VALUES ('Autres', 'fr', 'Other')");
						query.AppendLine($"INSERT INTO [LU].[InstallationCompanyRelationshipType] (Name, Language, Value) VALUES ('Otro', 'es', 'Other')");
						query.AppendLine($"INSERT INTO [LU].[InstallationCompanyRelationshipType] (Name, Language, Value) VALUES ('Egyéb', 'hu', 'Other')");

						Database.ExecuteNonQuery(query.ToString());
					}

					foreach (KeyValuePair<Guid, Guid> line in installationCompanies)
					{
						Database.ExecuteNonQuery($"INSERT INTO [SMS].[InstallationCompanyRelationship] ([InstallationKey], [CompanyKey], [RelationshipType]) VALUES ('{line.Key}', '{line.Value}', 'Other')");
					}
				}

				Database.RemoveTable("[SMS].[InstallationAdditionalContacts]");
			}
		}
	}
}
