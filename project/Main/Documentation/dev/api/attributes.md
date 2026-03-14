# API
## Attributes
### ExplicitEntitySet
Use this attribute to mark REST types that have no sync controller but you still want to access them with an oData entity set.
### ExplicitExpand
Use this attribute to mark properties that need to explicitly be expanded to be transferred via oData. Usually this is used for collections and complex types (or other navigation properties) as it has direct impact on the resulting SQL query and thus the amount of data going over the wire. If you add an entity type to a rest model (e.g. an address to a company) and it is not marked with this attribute, the resulting SQL will always load the address, but the transferred JSON will omit the address if there is no expand in the query.
### RestrictedField
This attribute marks properties that cannot be projected into the SQL database. This mean there is a complex auto map or other logic in between. Marking these properties results in the oData meta data model reflecting these restrictions, so that the client can know about them. 
In the meta model the properties will be marked as NotFilterable, NotCountable, NotSortable, NotExpandable, NotNavigable

> `[Database(Ignore=true)]` has the same effect regarding the restrictions
### NotMapped
This will mark the property to be ignored, resulting it not being listed in the meta data model.
> `[JsonIgnore]` has the same effect regarding the property being ignored
## Configuration
If attributes are not enough for you, or you need to override or extend defaults, you can implement or an `IPropertyConfigurator` for dynamic filtering and configuration.