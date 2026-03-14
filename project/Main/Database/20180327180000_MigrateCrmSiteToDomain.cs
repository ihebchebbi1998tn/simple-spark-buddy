namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Unicore;

	[Migration(20180327180000)]
	public class MigrateCrmSiteToDomain : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery($@"
				UPDATE [domain]
				SET [domain].[CreatedBy] = [site].[CreateUser]
					,[domain].[CreatedAt] = [site].[CreateDate]
					,[domain].[ModifiedBy] = [site].[ModifyUser]
					,[domain].[ModifiedAt] = [site].[ModifyDate]
					,[domain].[IsDeleted] = CASE WHEN [site].[IsActive] = 1 THEN 0 ELSE 1 END
					,[domain].[Name] = [site].[SiteDisplayName]
					,[domain].[DefaultLanguageKey] = [site].[DefaultLanguage]
					,[domain].[DefaultLocale] = [site].[DefaultLocale]
					,[domain].[Host] = [site].[SiteHost]
				FROM [dbo].[Domain] [domain], [CRM].[Site] [site]
				WHERE [domain].[UId] = '{UnicoreDefaults.CommonDomainId}'");
			Database.RemoveTable("[CRM].[Site]");
		}
	}
}
