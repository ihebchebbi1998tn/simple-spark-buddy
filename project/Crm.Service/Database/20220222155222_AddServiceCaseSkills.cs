namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220222155222)]
	public class AddServiceCaseSkills : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				CREATE TABLE [SMS].[ServiceNotificationsSkill] (
					ContactKey uniqueidentifier not null,
					Skill nvarchar(10) not null,
					FOREIGN KEY (ContactKey) REFERENCES [SMS].[ServiceNotifications] (ContactKey),
				);
			");
			
			Database.ExecuteNonQuery(@"
				CREATE TABLE [SMS].[ServiceCaseTemplateSkill] (
					ServiceCaseTemplateId uniqueidentifier not null,
					Skill nvarchar(10) not null,
					FOREIGN KEY (ServiceCaseTemplateId) REFERENCES [SMS].[ServiceCaseTemplate] (ServiceCaseTemplateId),
				);
			");
		}
	}
}