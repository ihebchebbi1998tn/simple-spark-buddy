namespace Main.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class UserSkillConfiguration : EntityConfiguration<UserSkill>
	{
		public UserSkillConfiguration(IEntityConfigurationHolder<UserSkill> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
		public override void Initialize()
		{
			Property(x => x.Skill, c =>
			{
				c.Filterable();
			});
			Property(x => x.ValidTo, c =>
			{
				c.Sortable();
				c.Filterable(f => f.Definition(new DateFilterDefinition { AllowFutureDates = true, AllowPastDates = true }));
			});
		}
	}
}
