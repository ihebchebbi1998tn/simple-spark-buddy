namespace Sms.Scheduler.Controllers.ActionRoleProvider
{
	using Crm;
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model;
	using Crm.Model.Lookups;
	using Crm.PerDiem.Model;
	using Crm.PerDiem.Model.Lookups;
	using Crm.Service;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;

	using LMobile.Unicore;

	using Main;
	using Main.Model;
	using Main.Model.Lookups;

	using Sms.Scheduler.Model;

	using User = Crm.Library.Model.User;

	public class SchedulerActionRoleProvider : RoleCollectorBase
	{
		public SchedulerActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			//This roleset  includes the least amount of permissions, readonly
			var readOnlyRoles = new[] { SchedulerPlugin.Roles.ServicePlanner, SchedulerPlugin.Roles.Disponent, SchedulerPlugin.Roles.SchedulerViewer };
			//This roleset is able to interact with the Planning board
			var disponentRoles = new[] { SchedulerPlugin.Roles.ServicePlanner, SchedulerPlugin.Roles.Disponent };
			//This roleset has the highest amount of permissions
			var plannerRoles = new[] { SchedulerPlugin.Roles.ServicePlanner };

			Add(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Index, readOnlyRoles);
			Add(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, readOnlyRoles);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Index);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.Login, SchedulerPlugin.PermissionName.SchedulerClient);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApi, nameof(Profile));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApi, nameof(ProfileResource));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApi, nameof(ServiceOrderDispatch));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, ServicePlugin.PermissionGroup.Dispatch, ServicePlugin.PermissionName.SeeAllUsersDispatches);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(ServiceOrderHead));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(ServiceOrderStatus));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(UserSkill));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(UserAsset));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(Skill));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(Asset));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(ServiceOrderTimePosting));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(ServiceOrderExpensePosting));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(TimeEntryType));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(ArticleDowntimeReason));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(ServiceOrderDispatchStatus));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(UserTimeEntry));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(Absence));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(DispatchPersonAssignment));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(DispatchArticleAssignment));
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View, PermissionGroup.WebApiRead, nameof(Tag));
			Add(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Read, readOnlyRoles);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Read, SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.View);
			Add(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Edit, disponentRoles);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Edit, SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Read);
			Add(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Delete, disponentRoles);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Delete, SchedulerPlugin.PermissionGroup.Scheduler, PermissionName.Edit);
			Add(SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.AddProfile, plannerRoles);
			Add(SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.EditProfile, plannerRoles);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.EditProfile, SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.AddProfile);
			Add(SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.DeleteProfile, plannerRoles);
			AddImport(SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.DeleteProfile, SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.EditProfile);
			Add(SchedulerPlugin.PermissionGroup.Scheduler, SchedulerPlugin.PermissionName.CanSeeOtherUsersProfile, plannerRoles);
			Add(PermissionGroup.Login, SchedulerPlugin.PermissionName.SchedulerClient, disponentRoles);
			AddImport(PermissionGroup.Login, SchedulerPlugin.PermissionName.SchedulerClient, PermissionGroup.WebApi, nameof(User));
			AddImport(PermissionGroup.Login, SchedulerPlugin.PermissionName.SchedulerClient, PermissionGroup.WebApiRead, nameof(Site));
			AddImport(PermissionGroup.Login, SchedulerPlugin.PermissionName.SchedulerClient, PermissionGroup.WebApiRead, nameof(Domain));

			Add(PermissionGroup.WebApi, SchedulerPlugin.PermissionName.SchedulerClient, disponentRoles);
			Add(PermissionGroup.WebApi, nameof(DispatchPersonAssignment), disponentRoles);
			Add(PermissionGroup.WebApi, nameof(DispatchArticleAssignment), disponentRoles);
			Add(PermissionGroup.WebApi, nameof(Absence), disponentRoles);

			Add(PermissionGroup.WebApi, nameof(UserUserGroup), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(UserSubscription), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(UserSkill), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(UserAsset), readOnlyRoles);

			Add(MainPlugin.PermissionGroup.UserAccount, CrmPlugin.PermissionName.ExportDropboxAddressAsVCard, readOnlyRoles);
			Add(MainPlugin.PermissionGroup.UserAccount, PermissionName.Settings, readOnlyRoles);
			Add(MainPlugin.PermissionGroup.UserAccount, MainPlugin.PermissionName.UpdateStatus, readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(RecentPage), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Site), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleDowntime), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleDowntimeReason), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Device), readOnlyRoles);

			Add(PermissionGroup.WebApi, nameof(Stock), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Store), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Station), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Location), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Article), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Usergroup), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ServiceOrderTime), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Address), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(FileResource), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(DocumentAttribute), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleUserRelationship), readOnlyRoles);

			Add(PermissionGroup.WebApi, nameof(ArticleType), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleDescription), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Language), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup01), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup02), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup03), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup04), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup05), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Currency), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(ArticleRelationshipType), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(QuantityUnit), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(VATLevel), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Region), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(CompanyType), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(Country), readOnlyRoles);
			Add(PermissionGroup.WebApi, nameof(DrivingLicenceCategory), readOnlyRoles);
		}
	}
}
