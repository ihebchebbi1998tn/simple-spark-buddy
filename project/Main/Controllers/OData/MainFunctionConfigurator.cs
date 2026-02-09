namespace Main.Controllers.OData
{
	using System;

	using Crm.Library.Api.Extensions;
	using Crm.Library.Api.Model;
	using Crm.Library.Model;
	using Crm.Library.Rest.Model;
	using Crm.Library.Signalr;

	using LMobile.Unicore;

	using Main.Rest.Model;

	using ODataConventionModelBuilder = Crm.Library.Api.Model.ODataConventionModelBuilder;
	using PermissionRest = Main.Rest.Model.PermissionRest;
	using User = Crm.Library.Model.User;
	using UserRest = Main.Rest.Model.UserRest;

	public class MainFunctionConfigurator : IModelConfigurator
	{
		private readonly ODataModelBuilderHelper modelHelper;

		public MainFunctionConfigurator(ODataModelBuilderHelper modelHelper)
		{
			this.modelHelper = modelHelper;
		}
		public virtual void Configure(ODataConventionModelBuilder builder)
		{
			var getLookupType = builder.Function(nameof(LookupODataController.GetLookupType)).Returns<LookupType>();
			getLookupType.Parameter<string>("FullName");
			getLookupType.Title = "gets metadata about a lookup type";

			var getLookupTypes = builder.Function(nameof(LookupODataController.GetLookupTypes)).ReturnsCollection<LookupType>();
			getLookupTypes.Title = "gets a list of all lookup types";

			var isLookupUsed = builder.Function(nameof(LookupODataController.IsLookupUsed)).Returns<bool>();
			isLookupUsed.Parameter<string>("Key");
			isLookupUsed.Parameter<string>("Type");
			isLookupUsed.Title = "checks if a lookup is used";

			builder.AddComplexType(typeof(PermissionRest));
			var getPermissions = builder.EntityType<UserRest>()
					.Collection
					.Function(nameof(AuthorizationODataController.GetPermissions))
					.ReturnsCollection<PermissionRest>();
			getPermissions.Title = "gets the permissions of the current user";

			var getRoles = builder.EntityType<UserRest>()
				.Collection
				.Function(nameof(AuthorizationODataController.GetRoles))
				.ReturnsCollection<string>();
			getRoles.Title = "gets the roles of the current user";

			var getToken = builder.EntityType<UserRest>()
					.Collection
					.Function(nameof(AuthorizationODataController.GetToken))
					.Returns<string>();
			getToken.Title = "gets the token of the current user";

			var getUser = builder.EntityType<UserRest>()
					.Collection
					.Function(nameof(AuthorizationODataController.GetUser))
					.ReturnsFromEntitySet<UserRest>(modelHelper.GetEntityTypeName(typeof(User)));
			getUser.Title = "gets the current user";

			var getTransactions = builder.EntityType<PostingRest>()
					.Collection
					.Function(nameof(PostingODataController.GetTransactions))
					.ReturnsCollection<Transaction>();
			getTransactions.Title = "gets transactions";

			var getVersion = builder.Function(nameof(VersionODataController.GetVersion)).Returns<string>();
			getVersion.Title = "gets the application version";

			var addUser = builder.EntityType<UserRest>()
				.Collection
				.Action(nameof(UserAdminODataController.AddUser))
				.Title("adds/creates the provided user")
				.ReturnsFromEntitySet<UserRest>(modelHelper.GetEntityTypeName(typeof(User)));
			addUser.EntityParameter<UserRest>(UserAdminODataController.ParameterUser).NotNullable();
			addUser.Parameter<string>(UserAdminODataController.ParameterAdName).Optional();
			addUser.Parameter<string>(UserAdminODataController.ParameterRemark).Optional();
			addUser.CollectionParameter<Guid>(UserAdminODataController.ParameterUsergroupIds).Optional();
			addUser.CollectionParameter<string>(UserAdminODataController.ParameterUsergroupNames).Optional();
			addUser.Parameter<string>(UserAdminODataController.ParameterPassword).Optional().NotNullable();
			addUser.CollectionParameter<Guid>(UserAdminODataController.ParameterRoleIds).Optional();
			addUser.CollectionParameter<string>(UserAdminODataController.ParameterRoleNames).Optional();

			var getLocalDatabaseFromLog = builder.EntityType<UserRest>()
				.Collection
				.Function(nameof(UserAdminODataController.GetLocalDatabaseFromLog))
				.Title("gets local database logged from the user")
				.Returns<string>();
			getLocalDatabaseFromLog.Parameter<string>("username").NotNullable();
			getLocalDatabaseFromLog.Parameter<string>("logDate").NotNullable();

			var getLocalStorageFromLog = builder.EntityType<UserRest>()
				.Collection
				.Function(nameof(UserAdminODataController.GetLocalStorageFromLog))
				.Title("gets localstorage logged from the user")
				.Returns<string>();
			getLocalStorageFromLog.Parameter<string>("username").NotNullable();
			getLocalStorageFromLog.Parameter<string>("logDate").NotNullable();

			var getSignalRInformation = builder.EntityType<UserRest>()
				.Collection
				.Function(nameof(UserAdminODataController.GetSignalRInformation))
				.Title("gets signalr information for the provided user")
				.ReturnsCollection<UserSignalRInformation>();
			
			var setLogLevel = builder.EntityType<UserRest>()
				.Collection
				.Action(nameof(UserAdminODataController.SetLogLevel))
				.Title("sets the client side log level for the provided user")
				.ReturnsFromEntitySet<UserRest>(modelHelper.GetEntityTypeName(typeof(User)));
			setLogLevel.EntityParameter<UserRest>(UserAdminODataController.ParameterUser).NotNullable().Required();
			setLogLevel.Parameter<JavaScriptLogLevel>(UserAdminODataController.ParameterLogLevel).NotNullable().Required();
			
			var setRoles = builder.EntityType<UserRest>()
				.Collection
				.Action(nameof(UserAdminODataController.SetRoles))
				.Title("sets the roles of the provided user")
				.ReturnsFromEntitySet<UserRest>(modelHelper.GetEntityTypeName(typeof(User)));
			setRoles.EntityParameter<UserRest>(UserAdminODataController.ParameterUser).NotNullable();
			setRoles.CollectionParameter<Guid>(UserAdminODataController.ParameterRoleIds).Optional();
			setRoles.CollectionParameter<string>(UserAdminODataController.ParameterRoleNames).Optional();

			var setUsergroups = builder.EntityType<UserRest>()
				.Collection
				.Action(nameof(UserAdminODataController.SetUsergroups))
				.Title("sets the usergroups of the provided user")
				.ReturnsFromEntitySet<UserRest>(modelHelper.GetEntityTypeName(typeof(User)));
			setUsergroups.EntityParameter<UserRest>(UserAdminODataController.ParameterUser).NotNullable();
			setUsergroups.CollectionParameter<Guid>(UserAdminODataController.ParameterUsergroupIds).Optional();
			setUsergroups.CollectionParameter<string>(UserAdminODataController.ParameterUsergroupNames).Optional();

			var setUsers = builder.EntityType<UsergroupRest>()
				.Collection
				.Action(nameof(UsergroupsODataController.SetUsers))
				.Title("sets the usergroup of the provided users")
				.ReturnsFromEntitySet<UsergroupRest>(modelHelper.GetEntityTypeName(typeof(Usergroup)));
			setUsers.CollectionParameter<string>(UsergroupsODataController.ParameterUserIds).NotNullable();
			setUsers.Parameter<Guid>(UsergroupsODataController.ParameterUserGroup).NotNullable();
			
			var addRole = builder.EntityType<PermissionSchemaRoleRest>()
				.Collection
				.Action(nameof(RoleController.AddRole))
				.Title("adds/creates the provided role")
				.ReturnsFromEntitySet<PermissionSchemaRoleRest>(modelHelper.GetEntityTypeName(typeof(PermissionSchemaRole)));
			addRole.EntityParameter<PermissionSchemaRoleRest>("Role").NotNullable();
			addRole.Parameter<string>(RoleController.ParameterSourcePermissionSchemaRoleName).Optional();

			var assignPermissions = builder.EntityType<PermissionSchemaRoleRest>()
				.Collection
				.Action(nameof(RoleController.AssignPermissions))
				.Title("assigns permissions to the provided role")
				.ReturnsFromEntitySet<PermissionSchemaRoleRest>(modelHelper.GetEntityTypeName(typeof(PermissionSchemaRole)));
			assignPermissions.Parameter<Guid>("RoleKey").NotNullable().Required();
			assignPermissions.CollectionParameter<string>("AssignedPermissions").Optional();
			assignPermissions.CollectionParameter<string>("UnassignedPermissions").Optional();

			var assignUsers = builder.EntityType<PermissionSchemaRoleRest>()
				.Collection
				.Action(nameof(RoleController.AssignUsers))
				.Title("assigns users to the provided role")
				.ReturnsFromEntitySet<PermissionSchemaRoleRest>(modelHelper.GetEntityTypeName(typeof(PermissionSchemaRole)));
			assignUsers.Parameter<Guid>("RoleKey").NotNullable().Required();
			assignUsers.CollectionParameter<string>("AssignedUsernames").Optional();
			assignUsers.CollectionParameter<string>("UnassignedUsernames").Optional();

			var getRolePermissions = builder.EntityType<PermissionSchemaRoleRest>()
				.Collection
				.Function(nameof(RoleController.GetRolePermissions))
				.Returns<RolePermissions>();
			getRolePermissions.Title = "gets the grouped permission information for the provided role";
			getRolePermissions.Parameter<Guid>("RoleKey").NotNullable().Required();

			var getTemplates = builder.EntityType<PermissionSchemaRoleRest>()
				.Collection
				.Function(nameof(RoleController.GetTemplates))
				.Title("gets the template roles")
				.ReturnsFromEntitySet<PermissionSchemaRoleRest>(modelHelper.GetEntityTypeName(typeof(PermissionSchemaRole)));

			var resetRole = builder.EntityType<PermissionSchemaRoleRest>()
				.Collection
				.Action(nameof(RoleController.ResetRole))
				.Title("resets to the to its template")
				.ReturnsFromEntitySet<PermissionSchemaRoleRest>(modelHelper.GetEntityTypeName(typeof(PermissionSchemaRole)));
			resetRole.Parameter<Guid>("RoleKey").NotNullable().Required();
			
			var getGeneratorServices = builder.EntityType<DocumentGeneratorEntry>()
				.Collection
				.Function(nameof(DocumentGeneratorEntryODataController.GetGeneratorServices))
				.ReturnsCollection<string>();
			getGeneratorServices.Parameter<string>("Filter").Optional();
			getGeneratorServices.Title = "gets the names of document generator services";
			var getFailedDocumentGeneratorEntries = builder.EntityType<DocumentGeneratorEntry>()
				.Collection
				.Function(nameof(DocumentGeneratorEntryODataController.GetFailed))
				.Returns<DocumentGeneratorEntry>();
			getFailedDocumentGeneratorEntries.Parameter<string>("GeneratorService").Required();
			getFailedDocumentGeneratorEntries.Title = "gets the failed document generator entries";
			var getPendingDocumentGeneratorEntries = builder.EntityType<DocumentGeneratorEntry>()
				.Collection
				.Function(nameof(DocumentGeneratorEntryODataController.GetPending))
				.Returns<DocumentGeneratorEntry>();
			getPendingDocumentGeneratorEntries.Parameter<string>("GeneratorService").Required();
			getPendingDocumentGeneratorEntries.Title = "gets the pending document generator entries";
			var retry = builder.Action(nameof(DocumentGeneratorODataController.RetryDocumentGeneration));
			retry.CollectionEntityParameter<DocumentGeneratorEntry>("Entries").NotNullable();
			retry.Title = "resets the status of failed document generator entries";
		}
	}
}
