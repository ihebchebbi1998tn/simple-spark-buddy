namespace Main.Model
{
	using Crm.Library.BaseModel;
	using System;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;

	using Main.Model.Lookups;

	public class UserSkill : EntityBase<Guid>, ISoftDelete
	{
		public virtual string Username { get; set; }
		public virtual User User { get; set; }
		public virtual string SkillKey { get; set; }
		public virtual Skill Skill
		{
			get { return SkillKey != null ? LookupManager.Get<Skill>(SkillKey) : null; }
		}
		public virtual DateTime? ValidFrom { get; set; }
		public virtual DateTime? ValidTo { get; set; }
		public virtual int? DaysToNotifyBeforeExpiration { get; set; }
	}
}
