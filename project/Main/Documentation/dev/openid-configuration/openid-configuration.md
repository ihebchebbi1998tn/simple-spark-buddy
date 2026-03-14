# OpenId configuration

## Preparation
We need to have a credential to connect our application to the provider. When the credential is being created the callback url should be provided, which is *host/Account/OpenIdCallback* in our case. 
When creating the credential, the flow have to be defined: we suggest code flow.
#### What exact informations do we need?
_Authority_ - which is the endpoint to the authentication provider. 
_ClientId_  - identifier for the application, it is generated during creation
_ClientSecret_ - a password which we can use to authenticate the application against the provider
_Scopes_ - this will define what kind of informations we will receive from the provider, in our case **profile** is mandatory

## Configuration
Once the credential is created, it is easy and straight forward: in the Main plugin's appSettings there are a few entries, such as OpenId/Authority, OpenId/ClientId, OpenId/ClientSecret, ... 

## FAQ
### How does the application know which user has logged in?

When a user logs in via an identity provider, we will receive some informations about the user, including some personal informations: username, email, etc... We use the _name_  property from the received claims to identify the user, and this value should be assigned to one of the users in the CRM. The assignment is very easy, you need to put this value into the  _OpenId-Identifier_  field.

### Is it necessary to create a corresponding user in the CRM?

Yes. Roles, usergroups, stations, and other entities are CRM related, so we need the same user to be defined in the CRM as well with the proper settings.

### What happens if a user logs in with the identity provider, but it's username is not assigned to any user in the CRM?

Nothing, the user will be redirected to the login page, as we can't identify this user.