namespace Main.ViewModels
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel.ViewModels;
	using Crm.Library.Model;
	using Crm.Library.Validation;

	using StackExchange.Profiling;

	public class CrmModel : BaseCrmModel
	{
		// Members
		public readonly Dictionary<Type, object> modelItems = new Dictionary<Type, object>();

		// Properties
		public string CacheManifest { get; set; }
		public SiteViewModel Site { get; set; }
		public User User { get; set; }

		public List<RuleViolation> RuleViolations { get; protected set; }

		// Methods
		public CrmModel AddRuleViolations(IEnumerable<RuleViolation> ruleViolations)
		{
			RuleViolations.AddRange(ruleViolations);
			return this;
		}

		public IEnumerable<object> GetModelItems()
		{
			return modelItems.Values;
		}

		public virtual void Synchronize(CrmModel otherModel)
		{
			using (MiniProfiler.Current.Step("CrmModel.Synchronize"))
			{
				otherModel.AuthorizationManager = AuthorizationManager;
				otherModel.DisableCordova = DisableCordova;
				otherModel.Site = Site;
				otherModel.User = User;
				otherModel.RuleViolations.AddRange(RuleViolations);
			}
		}

		public virtual TOtherModel SynchronizedModel<TOtherModel>(TOtherModel otherModel)
			where TOtherModel : CrmModel
		{
			Synchronize(otherModel);
			return otherModel;
		}

		// Constructor
		public CrmModel()
		{
			RuleViolations = new List<RuleViolation>();
		}
	}
}
