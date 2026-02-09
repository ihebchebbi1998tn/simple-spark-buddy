# API
## Introduction
The API is designed around the [REST](https://en.wikipedia.org/wiki/Representational_state_transfer) principles using the [oData](https://en.wikipedia.org/wiki/Open_Data_Protocol) protocol. HTTP response codes and messages are used to indicate API errors.
To access the API, just go to:

```
subdomain.yourdomain.com/virtualpath/api
```

This document does not aim to describe all oData specifics but is a starting point and describes the limitations and special features of our implementation.

## Supported Formats
The meta data model is provided in XML. Everything else is provided in JSON.

## HTTP Request Methods
- `GET` is used for retrieving resources
- `POST` is used for creating resources
- `PUT` is used for updating resources
- `PATCH` is used for updating resources with partial data, it is **not** supported
- `DELETE` is used for deleting resources

## Tools
There are several tools for different platforms to help you during API development. Since cURL is commonly used and other programs can also import requests in cURL syntax, this document will use cURL to demonstrate requests.

- [cURL](https://curl.haxx.se/windows)
- [Postman](https://www.getpostman.com)
  - also availabe as [Google Chrome extension](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop) (now deprecated)
  - import cURL requests using the button _Import_ and then _Paste Raw Text_
- [RESTClient](https://addons.mozilla.org/firefox/addon/restclient)
  - for Firefox
  - import cURL requests using the button _Paste_ in the section _Curl_ near the bottom
- [Fiddler](https://www.telerik.com/fiddler) is a powerful web debugger

## Authentication
There are no special permissions or roles to access the API. It can be accessed by any valid user. Authorization is done on entity level. However it is recommended to create a user that has only the required API permissions if you want to implement a recurring job like an import or an export.
To authenticate there are two supported methods: the regular user credentials and the user token.

Using cookies would also work, but there currently is no defined way to retrieve the cookie.

### Basic Authentication
You can either set the login as clear text  or use the basic authentication header (which is also clear text, Base64 encoded).

```
curl -X GET "/api/Crm_Company" -H "Authorization: Basic ZGVmYXVsdEBleGFtcGxlLmNvbTpkZWZhdWx0"
curl -X GET "/api/Crm_Company" -u "default@example.com:default"
```

### Token Authentication
Simply add your token as query parameter `token` to your request.

```
curl -X GET "/api/Crm_Company?$top=1&token=9f7041aa60e44b3"
```

## User Agent
Please set a meaningful `User-Agent` HTTP header for all requests. Currently this is not required, but it may be in the future. Setting this header helps when analysing problems.

```
curl -X GET "/api/Crm_Company?token=9f7041aa60e44b3" -H "User-Agent: Paul's Test Client for Demo L-mobile v0.1-alpha"
```

## Metadata
The service document, describing the endpoints, can be accessed by URI `/api/`. There you will find the link to the metadata model, which is `/api/$metadata`. The meta data model describes the entity sets, types, relationships, default values and restrictions.

### Metadata Annotations
Type properties reflect database and business rule restrictions where possible. Some properties are marked as `Nullable="false"` and/or have a maximum allowed length. Furthermore they may be annotated with additional information. Please note that the restrictions (e.g. nullability, maximum length) are auto generated and may not be complete.

- `Lmobile.DefaultValue` descibes the default value for a property, you can use this for initialization of your entities
- `InsertRestrictions`, `UpdateRestrictions` and `DeleteRestrictions` are used to mark properties that are read only. Properties with these annotations will be ignored in write operations. Some read only properties can still be written using _Direct Save_ (see further below). Please note, that some properties may still be read only, even though they are not annotated.
- `InsertRestrictions`, `UpdateRestrictions` and `DeleteRestrictions` are used to mark types as restricted regarding the operations that are allowed on them. Trying to execute a restricted operation will yield an error. Some restricted types can still be written using _Direct Save_ (see further below).
- `Lmobile.Required` marks a property that is required by a business rule. As some business rules are more complex, there may be properties that are required, but are not marked with the annotation.

There are entity sets with restrictions regarding the queries that they support. These entity sets have one or many of the following restriction annotations:

- `Org.OData.Capabilities.V1.FilterRestrictions`
- `Org.OData.Capabilities.V1.SortRestrictions`
- `Org.OData.Capabilities.V1.CountRestrictions`
- `Org.OData.Capabilities.V1.NavigationRestrictions`
- `Org.OData.Capabilities.V1.ExpandRestrictions`

### Extension Values
In the meta data model you will find complex types called extension values. These are our way of extending data models from plugins. After they are defined in a plugin they will also be available in the meta data model of the API. Additionally to their extensible nature they are open types, which means you can add extra data that is not defined in the meta data model.

## API Explorer
We provide an API explorer to play around with the models and operations. You can find it at:
```
subdomain.yourdomain.com/virtualpath/api/swagger
```

## Reading
Reading is always done with HTTP method `GET`. Entities can be read as a list or as a single entity.

For clarity the following examples only contain the URI.

### Single Entity
Entities can be directly accessed by their key:

```
/api/Crm_Company/01234567-89AB-CDEF-0123-456789ABCDEF
/api/Crm_Company(01234567-89AB-CDEF-0123-456789ABCDEF)
```

You can also access (navigation) properties using this syntax:

```
/api/Crm_Company(01234567-89AB-CDEF-0123-456789ABCDEF)/StandardAddress
```

### Collection of Entities
A collection can be read by using the path of the entity set:

```
/api/Crm_Company
```

To access navigational properties, you add an `$expand` clause:

```
/api/Crm_Company?$expand=StandardAddress
```

To select only certain fields, use the `$select` clause:

```
/api/Crm_Company?$select=Id,Name
```

You can also mix `$expand` and `$select`:

```
/api/Crm_Company?$select=Id,Name,Addresses&$expand=Addresses($select=City)
```

For limiting the amount of data you can use `$top` and `$skip`:

```
/api/Crm_Company?$skip=0&$top=10
```

Use `orderby` to sort the results:

```
/api/Crm_Company?$expand=Addresses($select=CreateDate;$orderby=CreateDate)&$orderby=Name desc&$select=Name
```

### Filtering
Filtering is done by using `$filter`. There are a lot of possible operators, so your will only see a few examples here. You can read more about `$filter` in the [url conventions](http://docs.oasis-open.org/odata/odata/v4.01/cs01/part2-url-conventions/odata-v4.01-cs01-part2-url-conventions.html#sec_SystemQueryOptionfilter). Please note, that not all options are supported.

A basic filter using `contains` on a string:

```
/api/Crm_Company?$filter=contains(Name, 'GmbH')
```

Filtering a collection can be done using `any` or `all`:

```
/api/Crm_Company?$filter=Tags/any(t: t eq 'Infor')
```

As _Tags_ are a collection of strings, the same result can be achieved using `in`:

```
/api/Crm_Company?$filter='Infor' in Tags
```

Getting company name and street that have an address in Sulzbach:

```
/api/Crm_Company?$select=Name,Addresses&$expand=Addresses($select=Street,City)&$filter=Addresses/any(a: a/City eq 'Sulzbach')
```

Filtering by enums can be done in two ways:

```
/api/Crm_Company?$filter=Visibility eq '2'
/api/Crm_Company?$filter=Visibility eq 'Everybody'
```

### Counting
To just get a plain number of results, you use `/$count`:

```
/api/Crm_Company/$count?$filter=contains(Name,'e')
```

You can also include the count into the result set, called inline count. Note that for any kind of counting, `$skip` and `$top` are ignored. This means, the following query returns the total number of results after applying the filter, and one entity because of `$top=1`

```
/api/Crm_Company?$filter=contains(Name,'e')&$top=1&$count=true
Result:
{
    "@odata.context": "/api/$metadata#Crm_Company",
    "@odata.count": 10,
    "value": [{ Id: [...] } ]
}
```

## Writing
Writing is done using HTTP methods `POST`, `PUT` and `DELETE`. Writing operations are always done to single entities. When writing related entities you should always use batch requests (see below). Not all entities are writable and for some not all properties are writable.

### Direct Save
The default way of writing is using internal services called `SyncServices` which, depending on the entity type, do additional validation, complex writing (like splitting entities or collections), only allow writing some special properties or even do not allow writing at all. You can skip the sync service and write directly to the database if you set the header `DirectSave` to `true` and also use a user that has the permission `WebAPI::DirectSave`. 

Although validation will still take place when using this header, it should never be your default way of writing entities.

### Special Entities
#### User
Writing a `User` is currently only possible using `PUT` and will only edit the **current** user (the one sending the request). Using _Direct Save_ can circumvent this restriction. Creating a new user is currently not supported by the API.

### Creating / Updating
Entities should always be sent as a whole, including the Id property. If properties are left out they will be set to their default value and sometimes the request will fail because the entity cannot be deserialized. Example: the value `null` is never valid for an `Id`.

Write operations are checked by business rules for validity. These rules for example check required fields and also field lengths and should give a clear error message about what's wrong.

For creating, either set the `Id` to a new unique value or a proper default value if you want to let the server decide.

```
curl -X POST /api/Crm_Address -H "Content-Type: application/json" --data-binary "@data.txt"
data.txt:
{
    "Id": "00000000-0000-0000-0000-000000000000",
    "Name1": "L-mobile",
    "Name2": "solutions",
    "Name3": "GmbH & Co. KG",
    "Street": "Im Horben 7",
    "ZipCode": "71560",
    "City": "Sulzbach an der Murr",
    "Country": "Germany",
    "CountryKey": "100",
    "RegionKey": "100",
    "POBox": null,
    "ZipCodePOBox": null,
    "AddressTypeKey": "1",
    "CompanyId": "01234567-89AB-CDEF-0123-456789ABCDEF",
    "Latitude": null,
    "Longitude": null,
    "LegacyId": null,
    "IsCompanyStandardAddress": false,
    "CreateDate": "0001-01-01T00:00:00Z",
    "ModifyDate": "0001-01-01T00:00:00Z",
    "CreateUser": null,
    "ModifyUser": null,
    "IsActive": true,
    "ExtensionValues": {
        "Distance": 1
    }
}
```

The server will respond with the created entity.

For updates, change to `PUT` and reference the entity you want to update: `/api/Crm_Address(01234567-89AB-CDEF-0123-456789ABCDEF)`

#### Internal Properties
The following properties will always be ignored and are overwritten by the server:

- `CreateDate` (only for `POST`)
- `ModifyDate`
- `CreateUser` (only for `POST`)
- `ModifyUser`
- `IsActive` is used for soft deletion. You can disable soft deletion (`SoftDelete=false`) which makes this property unused.

You can set the value of these properties to something meaningful if you use them for sorting or the likes, but be prepared that they might have changed the next time you are requesting data.

#### Return Representation
It is recommended to set an additional header in case you do not need the returned entity: `Prefer` with value `return=minimal`. The server will then return an empty body instead of the created or updated entity. For `POST` it will return the following header: `Location` with an URL pointing to the entity.

### Deleting
For deleting just send a `DELETE` request to the URL of the entity you want to delete.

## Batch
A batch request contains multiple sub requests that will be handled in one database transaction. It is advised to use batch requests for writing operations that have any kind of relations (e.g. creating a company and an address). You can also use batch requests for reading.

A simple batch request that reads a company and all its addresses (authentication and user agent stripped for clarity):

```
curl -X POST "/api/$batch" -H "Content-Type: multipart/mixed;boundary=batch_123test" --data-binary "@data.txt"
data.txt:
--batch_123test
Content-Type: application/http
Content-Transfer-Encoding: binary

GET Crm_Company(01234567-89AB-CDEF-0123-456789ABCDEF) HTTP/1.1

--batch_123test
Content-Type: application/http
Content-Transfer-Encoding: binary

GET Crm_Address?$filter=CompanyId eq 01234567-89AB-CDEF-0123-456789ABCDEF HTTP/1.1

--batch_123test--
```

The line breaks are important. You can read more about the format in the [protocol specifications](http://docs.oasis-open.org/odata/odata/v4.01/cs01/part1-protocol/odata-v4.01-cs01-part1-protocol.html#sec_MultipartBatchFormat)

You can also read and write in one request. The sub request will be handled in their given order and the whole request is handled in one database transaction. In Appendix A you can find an example.

Writing in batch request is done in change sets. On the server the whole batch will be processed in a single database transaction which will contain sub transactions for each unique change set in the batch request. That means when a change set fails its transaction will be rolled back and no changes take place from that change set.

### Continue on Error
By default batch execution is stopped when a request fails. If you want the server to still continue execution, you can set the header `Prefer` to `odata.continue-on-error=true`.

## Return Codes
- `200 OK` successful `GET` or `PUT`
- `201 CREATED` successful `POST`
- `204 NO CONTENT` successful `POST` or `PUT` with header `Prefer: return=minimal`, successful `DELETE`
- `400 BAD REQUST` failed `POST` or `PUT`
- `403 FORBIDDEN` the user is not allowed to access (read/write) this entity type
- `404 NOT FOUND` failed `GET`, `DELETE` or `PUT` for unkown `Id`
- `409 CONFLICT` failed `POST` if entity with this id already exists
- `500 INTERNAL SERVER ERROR` failed unsupported request or API error
- `501 NOT IMPLEMENTED` failed `PATCH`

> Note that there can also be other return codes. Also note, that a batch request returns `200 OK` even though not all sub requests were successful.

Failed requests will be logged on the server for further analysis.

## Client Development
You should not implement this yourself as there are already great libraries out there that can consume the meta data model and generate classes and handle requests and responses. You can check this as a starting point: https://www.odata.org/libraries

## Further Reading
- [https://www.odata.org/](https://www.odata.org/)
- [https://docs.microsoft.com/de-de/odata/](https://docs.microsoft.com/de-de/odata/)

## Known Limitations
- [Aggregation Extension](http://docs.oasis-open.org/odata/odata-data-aggregation-ext/v4.0/odata-data-aggregation-ext-v4.0.html) (groupby, distinct etc.) are not supported yet
- `PATCH` is not supported as multiple copies of the entity can exist in our eco system, we do not want to mix data from different sources in one entity
- [ETag](https://de.wikipedia.org/wiki/HTTP_ETag) is not supported
- when accessing single entities by key, nested property access is not supported: `/api/Crm_Company(95c72f4b-fc89-e511-a145-005056c00008)/StandardAddress($select=Name1))` use a $filter instead
- filtering and sorting in expanded collections is supported, but is done only _in memory_, meaning it is not translated to SQL, please keep that in mind when handling large data and make separate requests if possible
- not all entities are writable and also not all properties are writable, there are some indicators that reflect the limitations (see annotations above), but they may not be complete
  - you can use the `DirectSave` header to loosen that restriction in a lot of cases
- insert and update operations are single entities only, navigation properties are ignored
- self referencing models (like Company - ParentCompany / Subsidiaries) support $expand only on the outer level
- not all extension values are open types, especially extensions of lookups are not open types

## Appendix A
The following request creates a new address entity, a new email entity referencing the address entity and then selects the newest email from the database.

```
curl -X POST "/api/$batch" -H "Content-Type: multipart/mixed;boundary=batch_123test" --data-binary "@data.txt"
```

### Request
data.txt:

```
--batch_123test
Content-Type: multipart/mixed; boundary=changeset_456test
​
--changeset_456test
Content-Type: application/http
Content-Transfer-Encoding: binary
Content-ID: 1
​
POST Crm_Address HTTP/1.1
Content-Type: application/json
Prefer: return=minimal
​
{
    "Id": "01234567-89AB-CDEF-0123-456789ABCDEF",
    "Name1": "L-mobile",
    "Name2": "solutions",
    "Name3": "GmbH & Co. KG",
    "Street": "Im Horben 7",
    "ZipCode": "71560",
    "City": "Sulzbach an der Murr",
    "Country": "Germany",
    "CountryKey": "100",
    "RegionKey": "100",
    "POBox": null,
    "ZipCodePOBox": null,
    "AddressTypeKey": "1",
    "CompanyId": "FEDCBA98-7654-3210-FEDC-BA9876543210",
    "Latitude": null,
    "Longitude": null,
    "LegacyId": null,
    "IsCompanyStandardAddress": false,
    "CreateDate": "0001-01-01T00:00:00Z",
    "ModifyDate": "0001-01-01T00:00:00Z",
    "CreateUser": null,
    "ModifyUser": null,
    "IsActive": true,
    "ExtensionValues": {
        "Distance": 1
    }
}
--changeset_456test
Content-Type: application/http
Content-Transfer-Encoding: binary
Content-ID: 2
​
POST Crm_Email HTTP/1.1
Content-Type: application/json
Prefer: return=minimal
​
{
    "Id": "00000000-0000-0000-0000-000000000000",
    "ContactId": "FEDCBA98-7654-3210-FEDC-BA9876543210",
    "AddressId": "01234567-89AB-CDEF-0123-456789ABCDEF",
    "LegacyId": null,
    "TypeKey": "EmailWork",
    "Data": "info@l-mobile.com",
    "Comment": null,
    "CreateDate": "0001-01-01T00:00:00Z",
    "ModifyDate": "0001-01-01T00:00:00Z",
    "CreateUser": null,
    "ModifyUser": null,
    "IsActive": true
}
--changeset_456test--
--batch_123test
Content-Type: application/http
Content-Transfer-Encoding: binary
​
GET Crm_Email?$top=1&$orderby=CreateDate desc HTTP/1.1
​
--batch_123test--
```

### Response

```
--batchresponse_bf89bd68-c313-47a7-9c8d-ee9e16cbe0a3
Content-Type: multipart/mixed; boundary=changesetresponse_7ee37648-38d0-48ac-84cd-05f6017e2bbf
​
--changesetresponse_7ee37648-38d0-48ac-84cd-05f6017e2bbf
Content-Type: application/http
Content-Transfer-Encoding: binary
Content-ID: 1
​
HTTP/1.1 204 No Content
Location: /api/Crm_Address(01234567-89ab-cdef-0123-456789abcdef)
OData-EntityId: 01234567-89ab-cdef-0123-456789abcdef
​
​
--changesetresponse_7ee37648-38d0-48ac-84cd-05f6017e2bbf
Content-Type: application/http
Content-Transfer-Encoding: binary
Content-ID: 2
​
HTTP/1.1 204 No Content
Location: /api/Crm_Email(5ea64ddf-5e7a-42ab-aed0-aa080148bb6e)
OData-EntityId: 5ea64ddf-5e7a-42ab-aed0-aa080148bb6e
​
​
--changesetresponse_7ee37648-38d0-48ac-84cd-05f6017e2bbf--
--batchresponse_bf89bd68-c313-47a7-9c8d-ee9e16cbe0a3
Content-Type: application/http
Content-Transfer-Encoding: binary
​
HTTP/1.1 200 OK
Content-Type: application/json; odata.metadata=minimal; odata.streaming=true
OData-Version: 4.0
​
{
    "@odata.context": "/api/$metadata#Crm_Email",
    "value": [{
        "Id": "5ea64ddf-5e7a-42ab-aed0-aa080148bb6e",
        "ContactId": "FEDCBA98-7654-3210-FEDC-BA9876543210",
        "AddressId": "01234567-89ab-cdef-0123-456789abcdef",
        "LegacyId": null,
        "TypeKey": "EmailWork",
        "Data": "info@l-mobile.com",
        "Comment": null,
        "CreateDate": "2019-03-06T19:56:52.6290042+01:00",
        "ModifyDate": "2019-03-06T19:56:52.6290042+01:00",
        "CreateUser": "default.1",
        "ModifyUser": "default.1",
        "IsActive": true
    }]
}
--batchresponse_bf89bd68-c313-47a7-9c8d-ee9e16cbe0a3--
```
