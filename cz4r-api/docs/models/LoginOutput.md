# Org.OpenAPITools.Model.LoginOutput
The outcome of attempting to log in. `must_change_pw` is `true` when the credentials were valid but the account is required to change its password before a session (or, for the API, a token) can be issued. On the web this always redirects rather than returning this body; the API returns it directly.  `token` is only ever populated by the JSON API (`POST /api/v1/login`): it's the bearer token to send as `Authorization: Bearer <token>` on every subsequent `/api/v1/_*` or `/admin/api/v1/_*` request. The HTML UI doesn't use tokens at all - it authenticates via a cookie session instead, so `token` is always `None` there.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **long** |  | 
**MustChangePw** | **bool** |  | 
**Username** | **string** |  | 
**Token** | **string** |  | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

