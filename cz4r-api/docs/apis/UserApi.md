# Org.OpenAPITools.Api.UserApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**AssignmentDataApi**](UserApi.md#assignmentdataapi) | **GET** /api/v1/assignment-data |  |
| [**ChangePwApi**](UserApi.md#changepwapi) | **POST** /api/v1/change-pw | Changes the current user&#39;s password |
| [**CheckinoutApi**](UserApi.md#checkinoutapi) | **POST** /api/v1/checkinout | Sign in and sign out parameters accept full ISO8601 or simple HH:MM time strings. |
| [**IndexApi**](UserApi.md#indexapi) | **GET** /api/v1/stats | Some public statistics, does not require authentication. |
| [**JoblistApi**](UserApi.md#joblistapi) | **GET** /api/v1/joblist | The workers parameter is a dash-separated list of worker ids to filter by, e.g. &#x60;workers&#x3D;1-2-3&#x60;. Date range is YYYY-MM-DD format, defaults to today through 15 days from now. Non-admins can only see their own jobs, and the workers parameter is ignored for them. Admins&#39; view defaults to show all assignments for all workers, which is the recommended default. The three booleans default to true |
| [**LoginApi**](UserApi.md#loginapi) | **POST** /api/v1/login | Issues a beater token for API authentication. |
| [**LogoutApi**](UserApi.md#logoutapi) | **POST** /api/v1/logout | Revokes the bearer token used to authenticate this very request. |
| [**RefreshTokenApi**](UserApi.md#refreshtokenapi) | **POST** /api/v1/refresh-token | Refreshes the bearer token used to authenticate this very request, returning a new token. The old token is revoked and can no longer be used. |
| [**Whoami**](UserApi.md#whoami) | **GET** /api/v1/whoami | Gets the current user&#39;s details Equivalent to the &#x60;/admin/api/v1/users/:id&#x60; endpoint, but for the currently logged-in user. |

<a id="assignmentdataapi"></a>
# **AssignmentDataApi**
> FullAssignmentData AssignmentDataApi (long id, long worker)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **long** | Job ID |  |
| **worker** | **long** | Worker ID |  |

### Return type

[**FullAssignmentData**](FullAssignmentData.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="changepwapi"></a>
# **ChangePwApi**
> ChangePwOutput ChangePwApi (ChangePwInput changePwInput)

Changes the current user's password


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **changePwInput** | [**ChangePwInput**](ChangePwInput.md) |  |  |

### Return type

[**ChangePwOutput**](ChangePwOutput.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="checkinoutapi"></a>
# **CheckinoutApi**
> CheckInOutOutput CheckinoutApi (CheckInOutInput checkInOutInput)

Sign in and sign out parameters accept full ISO8601 or simple HH:MM time strings.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **checkInOutInput** | [**CheckInOutInput**](CheckInOutInput.md) |  |  |

### Return type

[**CheckInOutOutput**](CheckInOutOutput.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="indexapi"></a>
# **IndexApi**
> HomeStats IndexApi ()

Some public statistics, does not require authentication.


### Parameters
This endpoint does not need any parameter.
### Return type

[**HomeStats**](HomeStats.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="joblistapi"></a>
# **JoblistApi**
> JobListOutput JoblistApi (DateOnly startDate = null, DateOnly endDate = null, string siteName = null, string workOrder = null, string address = null, string notes = null, Order order = null, bool assigned = null, bool started = null, bool completed = null, string workers = null)

The workers parameter is a dash-separated list of worker ids to filter by, e.g. `workers=1-2-3`. Date range is YYYY-MM-DD format, defaults to today through 15 days from now. Non-admins can only see their own jobs, and the workers parameter is ignored for them. Admins' view defaults to show all assignments for all workers, which is the recommended default. The three booleans default to true


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **startDate** | **DateOnly** |  | [optional]  |
| **endDate** | **DateOnly** |  | [optional]  |
| **siteName** | **string** |  | [optional]  |
| **workOrder** | **string** |  | [optional]  |
| **address** | **string** |  | [optional]  |
| **notes** | **string** |  | [optional]  |
| **order** | **Order** |  | [optional]  |
| **assigned** | **bool** |  | [optional]  |
| **started** | **bool** |  | [optional]  |
| **completed** | **bool** |  | [optional]  |
| **workers** | **string** |  | [optional]  |

### Return type

[**JobListOutput**](JobListOutput.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="loginapi"></a>
# **LoginApi**
> LoginOutput LoginApi (LoginForm loginForm)

Issues a beater token for API authentication.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **loginForm** | [**LoginForm**](LoginForm.md) |  |  |

### Return type

[**LoginOutput**](LoginOutput.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="logoutapi"></a>
# **LogoutApi**
> LogoutOutput LogoutApi ()

Revokes the bearer token used to authenticate this very request.


### Parameters
This endpoint does not need any parameter.
### Return type

[**LogoutOutput**](LogoutOutput.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="refreshtokenapi"></a>
# **RefreshTokenApi**
> LoginOutput RefreshTokenApi ()

Refreshes the bearer token used to authenticate this very request, returning a new token. The old token is revoked and can no longer be used.


### Parameters
This endpoint does not need any parameter.
### Return type

[**LoginOutput**](LoginOutput.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="whoami"></a>
# **Whoami**
> WorkerSummary Whoami ()

Gets the current user's details Equivalent to the `/admin/api/v1/users/:id` endpoint, but for the currently logged-in user.


### Parameters
This endpoint does not need any parameter.
### Return type

[**WorkerSummary**](WorkerSummary.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

