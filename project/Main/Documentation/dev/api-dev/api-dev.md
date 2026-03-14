# API Development

## Client

The (material) client currently uses two different mechanisms to build it's model.

For offline mode it uses the REST models together with the relations found in JavaScript (usually `OfflineModel.js`). For online mode it uses only `/api/$metadata` which must contain all relations (navigation properties).

## Attributes

### ExplicitEntitySet

Use this attribute to mark REST types that have no sync controller but you still want to access them with an oData entity set.

### ExplicitExpand

Use this attribute to mark properties that need to explicitly be expanded to be transferred via oData. Usually this is used for collections and complex types (or other navigation properties) as it has direct impact on the resulting SQL query and thus the amount of data going over the wire. If you add an entity type to a rest model (e.g. an address to a company) and it is not marked with this attribute, the resulting SQL will always load the address, but the transferred JSON will omit the address if there is no expand in the query.

> this attribute will add a `PreCondition` to the mapping used by Automapper, be sure to not use a `PreCondition` in your mapping of the property, as it will not be applied

## RestrictedField

This attribute marks properties that cannot be projected into the SQL database. This mean there is a complex auto map or other logic in between. Marking these properties results in the oData meta data model reflecting these restrictions, so that the client can know about them. 
In the meta model the properties will be marked as NotFilterable, NotCountable, NotSortable, NotExpandable, NotNavigable
> `[Database(Ignore=true)]` has the same effect regarding the restrictions

### NotMapped

This will mark the property to be ignored, resulting it not being listed in the meta data model. Automapper will also ignore these properties.
> `[JsonIgnore]` has the same effect regarding the property being ignored

### NotReceived

This marks the property as read only.

### RestrictedType

Marks restricted types regarding the operations that are allowed on them.

## Extending the API

There are multiple interfaces which let you implement services that can extend the API easily.

### Configuring the model

To get full access during the building phase of the model you can implement `IModelConfigurator`. Use this to register special types or operations (functions or actions). When you want to add a special controller, you can use `ODataControllerEx` as a base class. In case you are handling entities (which you almost always are), you should also implement `IEntityApiController` to get authorization checks for the entity type (meaning checking if the user has permission to access this type).

#### Configuring and filtering properties

If attributes are not enough for you, or you need to override or extend defaults, you can implement `IPropertyConfigurator` for dynamic filtering and configuration of properties.

#### Configuring types

Services implementing `ITypeConfigurator` are called a bit later (almost as last step), when all convention based exploring and registering is already done.

#### Annotations

To change default annotations or add your own, implement `IModelAnnotator`.

### Manipulating queries

Sometimes you need to do extend the filtering or ordering of the query. Then you can implement `IODataQueryFunction` to manipulate the NHibernate query.

### Manipulating results

As `ExtensionValues` are open complex types, you can add additional data to to them when sending the result to the client. To do so implement `IODataDynamicExtensionValueSerializer`.

### Manipulating data manipulating requests

You can hook into any write operations (POST, UPDATE, DELETE) by implementing `IODataWriteFunction`. It is called after the default write operation is done.

### Manipulating keys

You can change the key of the current operation is using by implementing a `IODataAlternativeKeyProvider`. This is currently used by the OData integration to allow retrieving and writing data without knowing the L-mobile FIELD key, but only the external key.
