namespace Main
{
	using Crm.Library.Configuration;
	using Crm.Library.Modularization;

	[Plugin(Requires = "")]
	public class MainPlugin : Plugin
	{
		public new static string Name = "Main";

		public static class Roles
		{
			public const string LicenseManagement = "LicenseManagement";
		}

		public static class PermissionGroup
		{
			public const string BackgroundService = "BackgroundService";
			public const string Category = "Category";
			public const string Site = "Site";
			public const string TopSearch = "TopSearch";
			public const string UserAccount = "UserAccount";
			public const string DocumentArchive = "DocumentArchive";
			public const string ThirdPartySoftware = "ThirdPartySoftware";
			public const string Security = "Security";
		}

		public static class PermissionName
		{
			public const string ImportFromVcf = "ImportFromVcf";
			public const string ExportAsVcf = "ExportAsVcf";
			public const string ExportAsCsv = "ExportAsCsv";
			public const string Rss = "Rss";
			public const string Ics = "Ics";
			public const string DownloadAsPdf = "DownloadAsPdf";

			public const string SetStatus = "SetStatus";
			public const string SetStatusMultiple = "SetStatusMultiple";
			public const string Merge = "Merge";

			public const string Deactivate = "Deactivate";
			public const string Activate = "Activate";
			public const string ToggleActive = "ToggleActive";

			public const string SidebarBackgroundInfo = "SidebarBackgroundInfo";
			public const string SidebarDocumentArchive = "SidebarDocumentArchive";

			public const string PublicHeaderInfo = "PublicHeaderInfo";
			public const string PrivateHeaderInfo = "PrivateHeaderInfo";
			public const string DetailsBackgroundInfo = "DetailsBackgroundInfo";

			public const string Complete = "Complete";
			public const string Reopen = "Reopen";
			public const string Close = "Close";

			public const string UpdateStatus = "UpdateStatus";

			public const string CreateRole = "CreateRole";
			public const string DeleteRole = "DeleteRole";
			public const string EditRole = "EditRole";

			public const string AddUser = "AddUser";
			public const string CreateUser = "CreateUser";
			public const string EditUser = "EditUser";
			public const string DeleteUser = "DeleteUser";

			public const string ListPermissions = "ListPermissions";
			public const string AddPermission = "AddPermission";
			public const string RemovePermission = "RemovePermission";
			public const string ListRoles = "ListRoles";
			public const string AddRole = "AddRole";
			public const string RemoveRole = "RemoveRole";
			public const string ListUsergroups = "ListUsergroups";
			public const string AddUserGroup = "AddUserGroup";
			public const string RemoveUserGroup = "RemoveUserGroup";

			public const string UserResetGeneralToken = "UserResetGeneralToken";
			public const string RefreshUserCache = "RefreshUserCache";
			public const string CreateUserGroup = "CreateUserGroup";
			public const string AssignUserGroup = "AssignUserGroup";
			public const string DeleteUserGroup = "DeleteUserGroup";
			public const string EditUserGroup = "EditUserGroup";
			public const string SaveUserGroup = "SaveUserGroup";
			public const string AssignRole = "AssignRole";
			public const string AssignSkill = "AssignSkill";
			public const string SignalR = "SignalR";

			public const string RequestLocalStorage = "RequestLocalStorage";
			public const string RequestLocalDatabase = "RequestLocalDatabase";
			public const string SetLogLevel = "SetLogLevel";
			public const string ViewLocalStorageLog = "ViewLocalStorageLog";
			public const string ViewLocalDatabaseLog = "ViewLocalDatabaseLog";
			public const string SendJavaScriptCommand = "SendJavaScriptCommand";
			public const string UserDetail = "UserDetail";
			public const string UserGroupDetail = "UserGroupDetail";
			public const string ResetUserPassword = "ResetUserPassword";
			public const string SetUsers = "SetUsers";

			public const string Lookup = "Lookup";

			public const string AssignLicense = "AssignLicense";
			public const string RevokeLicense = "RevokeLicense";

			public const string SetMinPasswordStrength = "SetMinPasswordStrength";
			public const string SetMultiFactorAuthenticationMode = "SetMultiFactorAuthenticationMode";
			public const string ManageMultiFactorAuthentication = "ManageMultiFactorAuthentication";
			public const string ResetUserTotp = "ResetUserTotp";
		}

		public static class Settings
		{
			public static SettingDefinition<bool> AllowEditAddressesWithLegacyId => new SettingDefinition<bool>("AllowEditAddressWithLegacyId", Name);

			public static class Maintenance
			{
				public static SettingDefinition<int> PasswordResetTokenDeprecationDays => new SettingDefinition<int>("Maintenance/PasswordResetTokenDeprecationDays", Name);
				public static SettingDefinition<int> ReplicatedClientDeprecationDays => new SettingDefinition<int>("Maintenance/ReplicatedClientDeprecationDays", Name);
				public static SettingDefinition<int> PostingDeprecationDays => new SettingDefinition<int>("Maintenance/PostingDeprecationDays", Name);
				public static SettingDefinition<int> LogDeprecationDays => new SettingDefinition<int>("Maintenance/LogDeprecationDays", Name);
				public static SettingDefinition<int> ErrorLogDeprecationDays => new SettingDefinition<int>("Maintenance/ErrorLogDeprecationDays", Name);
				public static SettingDefinition<int> MessageDeprecationDays => new SettingDefinition<int>("Maintenance/MessageDeprecationDays", Name);
				public static SettingDefinition<int> FragmentationLevel1 => new SettingDefinition<int>("Maintenance/FragmentationLevel1", Name);
				public static SettingDefinition<int> FragmentationLevel2 => new SettingDefinition<int>("Maintenance/FragmentationLevel2", Name);
				public static SettingDefinition<int> AmountOfRecentPagesToKeep => new SettingDefinition<int>("Maintenance/AmountOfRecentPagesToKeep", Name);
				public static SettingDefinition<int> CommandTimeout => new SettingDefinition<int>("Maintenance/CommandTimeout", Name);
			}

			public static SettingDefinition<bool> AllowEditCompanyWithLegacyId => new SettingDefinition<bool>("AllowEditCompanyWithLegacyId", Name);

			public static class Cordova
			{
				public static SettingDefinition<string> AndroidServiceAppLink => new SettingDefinition<string>("Cordova/AndroidServiceAppLink", Name);
				public static SettingDefinition<string> AppleIosServiceAppLink => new SettingDefinition<string>("Cordova/AppleIosServiceAppLink", Name);
				public static SettingDefinition<string> Windows10ServiceAppLink => new SettingDefinition<string>("Cordova/Windows10ServiceAppLink", Name);
				public static SettingDefinition<string> AndroidSalesAppLink => new SettingDefinition<string>("Cordova/AndroidSalesAppLink", Name);
				public static SettingDefinition<string> AppleIosSalesAppLink => new SettingDefinition<string>("Cordova/AppleIosSalesAppLink", Name);
				public static SettingDefinition<string> Windows10SalesAppLink => new SettingDefinition<string>("Cordova/Windows10SalesAppLink", Name);
			}

			public static class Email
			{
				public static SettingDefinition<bool> SenderImpersonation => new SettingDefinition<bool>("Email/SenderImpersonation", Name);
				public static SettingDefinition<int> AttachmentMaxSize => new SettingDefinition<int>("Email/AttachmentMaxSize", Name);
			}

			public static class Geocoder
			{
				public static SettingDefinition<string> GeocoderService => new SettingDefinition<string>("Geocoder/GeocoderService", Name);
				public static SettingDefinition<string> GoogleMapsApiKey => new SettingDefinition<string>("Geocoder/GoogleMapsApiKey", Name);
				public static SettingDefinition<string> GoogleMapsApiVersion => new SettingDefinition<string>("Geocoder/GoogleMapsApiVersion", Name);
				public static SettingDefinition<string> MapQuestApiKey => new SettingDefinition<string>("Geocoder/MapQuestApiKey", Name);
				public static SettingDefinition<string> BingMapsApiKey => new SettingDefinition<string>("Geocoder/BingMapsApiKey", Name);
				public static SettingDefinition<string> YahooMapsApiKey => new SettingDefinition<string>("Geocoder/YahooMapsApiKey", Name);
				public static SettingDefinition<string> YahooMapsApiSecret => new SettingDefinition<string>("Geocoder/YahooMapsApiSecret", Name);
				public static SettingDefinition<int> BatchSize => new SettingDefinition<int>("Geocoder/Address/BatchSize", Name);
			}

			public static SettingDefinition<int> MinPasswordStrength => new SettingDefinition<int>("MinPasswordStrength", Name);

			public static class PasswordReset
			{
				public static SettingDefinition<int> ExpirationInMinutes => new SettingDefinition<int>("PasswordReset/ExpirationInMinutes", Name);
				public static SettingDefinition<int> MaxEmailsPerHour => new SettingDefinition<int>("PasswordReset/MaxEmailsPerHour", Name);
			}

			public static SettingDefinition<bool> AllowEditPersonWithLegacyId => new SettingDefinition<bool>("AllowEditPersonWithLegacyId", Name);

			public static class Posting
			{
				public static SettingDefinition<int> MaxRetries => new SettingDefinition<int>("Posting/MaxRetries", Name);
				public static SettingDefinition<int> RetryAfter => new SettingDefinition<int>("Posting/RetryAfter", Name);
			}

			public static class Report
			{
				public static SettingDefinition<float> HeaderHeight => new SettingDefinition<float>("Report/HeaderHeight", Name);
				public static SettingDefinition<float> HeaderMargin => new SettingDefinition<float>("Report/HeaderSpacing", Name);
				public static SettingDefinition<float> FooterHeight => new SettingDefinition<float>("Report/FooterHeight", Name);
				public static SettingDefinition<float> FooterMargin => new SettingDefinition<float>("Report/FooterSpacing", Name);
			}

			public static class System
			{
				public static SettingDefinition<string> CefToPdfPath => new SettingDefinition<string>("CefToPdfPath", Name);
				public static SettingDefinition<string> RedisConfiguration => new SettingDefinition<string>("RedisConfiguration", Name);
				public static SettingDefinition<bool> SoftDelete => new SettingDefinition<bool>("SoftDelete", Name);
				public static SettingDefinition<bool> UseActiveDirectoryAuthenticationService => new SettingDefinition<bool>("UseActiveDirectoryAuthenticationService", Name);
				public static SettingDefinition<string> ActiveDirectoryEndpoint => new SettingDefinition<string>("ActiveDirectoryEndpoint", Name);

				public static class OpenIdAuthentication
				{
					public static SettingDefinition<bool> UseOpenIdAuthentication => new SettingDefinition<bool>("UseOpenIdAuthentication", Name);
				}

				public static class Maps
				{
					public static SettingDefinition<string> MapTileLayerUrl => new SettingDefinition<string>("MapTileLayerUrl", Name);
				}

				public static SettingDefinition<int> MaxFileLengthInKb => new SettingDefinition<int>("MaxFileLengthInKb", Name);
			}

			public static class Task
			{
				public static SettingDefinition<string> AttentionTaskTypeKey => new SettingDefinition<string>("Task/AttentionTaskTypeKey", Name);
			}
		}
	}
}
