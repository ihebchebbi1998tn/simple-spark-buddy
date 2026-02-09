namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;

	using Main.Rest.Model;

	[RestTypeFor(DomainType = typeof(ServiceCaseTemplate))]
	public class ServiceCaseTemplateRest : RestEntityWithExtensionValues
	{
		public string CategoryKey { get; set; }
		public string Name { get; set; }
		public string PriorityKey { get; set; }
		public Guid? UserGroupKey { get; set; }
		[NavigationProperty(nameof(UserGroupKey))] public UsergroupRest UserGroup { get; set; }
		public string ResponsibleUser { get; set; }
		[NavigationProperty(nameof(ResponsibleUser))] public UserRest ResponsibleUserUser { get; set; }
		[RestrictedField] public string[] RequiredSkillKeys { get; set; }
		[RestrictedField] public string[] RequiredAssetKeys { get; set; }

	}
}
