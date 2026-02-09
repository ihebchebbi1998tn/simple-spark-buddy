# Multitenancy
With 5.0 support for multiple tenants was added to the product. There are two modes which are supported - physical tenants and logical tenants. See this blog post to understand the differences: [Thoughts on 5.0 and our upcoming API release](https://wiki.l-mobile.com/x/cAvWAQ). This chapter focuses on logical tenants (`InstancePerTenant=false`).
## Database
Currently we use only a small subset of Unicore to manage tenant / domain access of users. You can manage this access on entity level, operations like reading and writing and entity types. To keep things relatively easy we just use one operation (`RW`) and handle access only on entity type level.
### dbo.Domain
This will be the tenant itself (formerly `CRM.Site`). There is always a default domain using empty guid (00000000-0000-0000-0000-000000000000) as key.
### dbo.DomainAuthorisedDomain
Can be used for a hierarchical domain structure. Currently every domain needs an entry referencing itself here.
### dbo.User
This table is needed for Unicore filtering which user belongs to which domain and which entity types can the user access in which domain. The table references `CRM.User` via a foreign key. Currently we did not merge `CRM.User` and `dbo.User` to one table, but this will be done in an upcoming release. 
### dbo.EntityType
Here every type that needs to be authorized (meaning having it's access checked via special filters) needs an entry.
### dbo.EntityAccess
This table contains the access definition which will then be granted in `dbo.GrantedEntityAccess`. In our use case `dbo.EntityAccess` contains one entry for every entity type.
### dbo.EntityAuthData
Here our authorized entities are connected to Unicore and their owning domain is saved. Each authorized entity has a foreign key `AuthDataId` which points to `dbo.EntityAuthData`.
### dbo.GrantedEntityAccess
This table connects user, domain and access. In our case this means it determines which user can access which entity type in which domain.
## Authorization Filter
If the tenant plugin is active and running in logical tenant mode, this filter is enabled in all sessions for all authorized entites and also collections of authorized entities. It is rather complex and uses the tables described above to decide which data the current user can access.
There are five parameters which will be set for each session, This is done in `UnicoreAuthorisationFilterEnabler`:
- User: the current user's id
- Operation: always set to `RW`
- DomainType: always set to `Normal`
- NoDomainFilter: always set to `false`
- Domains: set to the common domain **and** the current domain
## Services and Interfaces
### NHibernate Mappings
The NHibernate mappings are applied dynamically, except for the filter itself. The mapping is automatically applied to types inheriting `EntityBase<Guid>`. There are also two marker interfaces to explicitly include or exclude a type: `IExplicitlyAuthorizedObject` and `INoAuthorisedObject`.
### SetEntityAuthDataEventHandler
This event handler will automatically add the required Unicore database entries for the filtering to work when new entities are inserted into the database.
#### IDomainForTypeProvider<T>
If there is an implementation of this interface where T is an authorized entity, it will be called to determine the domain of the newly created entities of type T on PreInsert, else the default implementation will be called. The returned domain does not have to be the current domain, as you might want some types to be always inserted into the common domain. For example, check `DomainForUserProvider`.
Also, the implementation can cover multiple entities in a single class like this sample:
```
public class DomainForUserAndUsergroupProvider : IDomainForTypeProvider<Crm.Library.Model.User>, IDomainForTypeProvider<Crm.Library.Model.Usergroup>
{
	public Guid GetDomain(Crm.Library.Model.User entity) => UnicoreDefaults.CommonDomainId;
	public Guid GetDomain(Crm.Library.Model.Usergroup entity) => UnicoreDefaults.CommonDomainId;
	//In case of multiple interface implementions, this is needed or we a get a compiler error.
	public Guid GetDomain(IAuthorisedObject entity) =>
		entity switch
		{
			Crm.Library.Model.User => GetDomain((Crm.Library.Model.User)entity),
			Crm.Library.Model.Usergroup => GetDomain((Crm.Library.Model.Usergroup)entity),
			_ => throw new NotImplementedException()
		};
}
```

#### IIdForEntityProvider
This provider returns the id of an entity, this is especially important for types which where explicitly marked with `IExplicitlyAuthorizedObject` and do not return an id of type Guid by default (as an `EntityBase<Guid>` would).
## Migrations
Migrations are running in this order (this is just an overview):
- add table `dbo.Domain`, a common domain with Id empty Guid will be added as default and `CRM.Site` migrated
- add table `dbo.EntityType`
- add tenant types (`*_AddEntityType.cs`)
  - these migrations are added to every plugin that defines authorized entities
- migrate `CRM.Tenant`, which will become entries in `dbo.Domain`
- add table `dbo.User`, migrate `CRM.UserTenant`
- add other required Unicore tables
- migrate column `TenantKey`, add entries to `dbo.EntityAuthData`, depending on the entries in `dbo.EntityType`
  - add a new column `AuthDataId` to the target table referencing `dbo.EntityAuthData`
  - add the entry in `dbo.EntityAuthData`
  - add entries in `dbo.GrantedEntityAccess` for the entity type for every user and every domain the user has access to

If you have custom entities in your customer plugin that need to be authorized, you must add a migration. The easiest way is to use `AddOrUpdateEntityAuthDataColumn<T>([...])` of `UnicoreMigrationHelper`.
## Imports
### User
For your user import you just have to make sure to add an entry to `CRM.User` and `dbo.User`.
### Domain / Tenants
Import to `dbo.Domain`, don't forget to add an entry to `dbo.DomainAuthorizedDomain`
### Entities
Make sure your types are registered, then you only have to add en entry to `dbo.EntityAuthData`. To do so you should output the merge action, then make an insert to `dbo.EntityAuthData` for all entites that where inserted and finally update `AuthDataId` in the merge target. 

Working example importing one note into the default domain:

	IF OBJECT_ID('tempdb..#Source') IS NOT NULL DROP TABLE #Source
	IF OBJECT_ID('tempdb..#Imported') IS NOT NULL DROP TABLE #Imported
	IF OBJECT_ID('tempdb..#AuthData') IS NOT NULL DROP TABLE #AuthData
	CREATE TABLE #Imported ([Action] NVARCHAR(100), EntityId UNIQUEIDENTIFIER, LegacyId NVARCHAR(100))
	CREATE TABLE #AuthData (AuthDataId UNIQUEIDENTIFIER, EntityId UNIQUEIDENTIFIER)

	SELECT NEWID() AS [LegacyId], 'Text' AS [Text], CONVERT(UNIQUEIDENTIFIER, '00000000-0000-0000-0000-000000000000') AS [DomainId]
	INTO #Source

	DECLARE @entityTypeId UNIQUEIDENTIFIER = (SELECT [UId] FROM [dbo].[EntityType] WHERE [Name] = 'Crm.Model.UserNote')

	BEGIN TRANSACTION
		MERGE [CRM].[Note] AS [target]
		USING #Source AS [source]
		ON [target].[LegacyId] = [source].[LegacyId]
		--WHEN MATCHED [...]
		WHEN NOT MATCHED THEN INSERT ([Text], [LegacyId]) VALUES ([source].[Text], [source].[LegacyId])
		--WHEN NOT MATCHED BY SOURCE [...]
		OUTPUT $action AS [Action], INSERTED.[NoteId] AS [EntityId], INSERTED.[LegacyId] INTO #Imported;

		INSERT INTO [dbo].[EntityAuthData] ([EntityId], [EntityTypeId], [DomainId])
		OUTPUT INSERTED.[UId] AS AuthDataId, INSERTED.EntityId INTO #AuthData
		SELECT [imported].[EntityId], @entityTypeId, [source].[DomainId]
		FROM #Imported [imported]
		JOIN #Source [source] ON [source].[LegacyId] = [imported].[LegacyId]
		WHERE [imported].[Action] = 'INSERT'

		UPDATE [target]
		SET [target].[AuthDataId] = [authData].[AuthDataId]
		FROM #AuthData [authData]
		JOIN [CRM].[Note] [target] ON [authData].EntityId = [target].NoteId
	COMMIT TRANSACTION