namespace Main.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	using Main.Model.Extension;

	public class RoleEditViewModel : CrmModelItem<PermissionSchemaRole>, ITransientDependency
	{
		private readonly IResourceManager resourceManager;
		public virtual string Description { get; set; }
		public virtual IEnumerable<KeyValuePair<string, IEnumerable<Permission>>> PermissionGroups { get; set; }
		public virtual Dictionary<string, List<string>> PermissionImports { get; set; }
		public virtual Dictionary<string, List<string>> RolePermissionTable { get; set; }
		public virtual Dictionary<Guid, string> RoleTemplates { get; set; }
		public RoleEditViewModel(IAccessControlManager accessControlManager, IResourceManager resourceManager, IPermissionManager permissionManager, PermissionSchemaRole currentRole)
		{
			this.resourceManager = resourceManager;
			PermissionGroups = GetPermissionGroups(accessControlManager.ListPermissionSchemaPermissions(UnicoreDefaults.DefaultPermissionSchema));
			PermissionImports = permissionManager.CollectedPermissionImports;
			RolePermissionTable = accessControlManager.ListPermissionSchemaRoles(UnicoreDefaults.DefaultPermissionSchema).ToDictionary(x => x.Name, x => accessControlManager.ListPermissionSchemaRolePermissions(x).Select(y => y.Name).ToList());
			RoleTemplates = accessControlManager.ListPermissionSchemaRoles(UnicoreDefaults.TemplatePermissionSchema).Where(x => !RolePermissionTable.ContainsKey(x.Name)).ToDictionary(x => x.UId, x => resourceManager.GetTranslation(x.Name) ?? x.Name);
			Description = currentRole.GetExtension<PermissionSchemaRoleExtension>().Description;
		}

		public virtual IEnumerable<KeyValuePair<string, IEnumerable<Permission>>> GetPermissionGroups(IEnumerable<Permission> permissions)
		{
			return permissions
				.GroupBy(p => p.Group)
				.Select(group => new KeyValuePair<string, IEnumerable<Permission>>(group.Key, group));
		}
		public virtual bool ComprisesPermission(PermissionSchemaRole role, Permission permission)
		{
			return ComprisesPermission(role, permission.Name);
		}
		public virtual bool ComprisesPermission(PermissionSchemaRole role, string permissionName)
		{
			return role.Name != null && RolePermissionTable.ContainsKey(role.Name) && RolePermissionTable[role.Name].Any(x => x.Equals(permissionName, StringComparison.InvariantCultureIgnoreCase));
		}
		public virtual IEnumerable<string> GetImportedPermissions(Permission permission)
		{
			if (PermissionImports.ContainsKey(permission.Name))
			{
				foreach(var permissionImport in PermissionImports[permission.Name])
				{
					yield return permissionImport;
				}
			}
		}
		public virtual IEnumerable<string> GetComprisedPermissionsImportedBy(PermissionSchemaRole role, string permissionName)
		{
			return PermissionImports.Where(x => x.Value.Contains(permissionName) && ComprisesPermission(role, x.Key)).Select(x => x.Key).Distinct();
		}
	}
}
