# Org.OpenAPITools.Api.AdminApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ChangeWorkerApi**](AdminApi.md#changeworkerapi) | **POST** /admin/api/v1/change-worker | Admins are not capable of removing their own admin privileges, and the API will return a 403 error if they try. |
| [**CreateJobApi**](AdminApi.md#createjobapi) | **POST** /admin/api/v1/jobs | &#x60;job_id&#x60; in the body is ignored (a new job always gets a fresh id). Responds &#x60;201 Created&#x60;. |
| [**CreateWorkerApi**](AdminApi.md#createworkerapi) | **POST** /admin/api/v1/create-worker | Responds &#x60;201 Created&#x60; with the new worker&#39;s id on success. |
| [**DeactivateApi**](AdminApi.md#deactivateapi) | **POST** /admin/api/v1/deactivate-worker | Admins are not capable of deactivating themselves, and the API will return a 403 error if they try. |
| [**DeleteJobApi**](AdminApi.md#deletejobapi) | **DELETE** /admin/api/v1/jobs/{id} | Deletes a job. The id comes from the path, not the body. |
| [**ExportDbApi**](AdminApi.md#exportdbapi) | **GET** /admin/api/v1/export-database.sql | Not JSON (it streams a raw &#x60;sqlite3 .dump&#x60; of the database), content-type: application/octet-stream |
| [**GetUserApi**](AdminApi.md#getuserapi) | **GET** /admin/api/v1/users/{id} | Gets a single worker&#39;s profile, e.g. to prefill a worker-edit form. |
| [**JobeditpageApi**](AdminApi.md#jobeditpageapi) | **GET** /admin/api/v1/jobs/{id} |  |
| [**ListUsersApi**](AdminApi.md#listusersapi) | **GET** /admin/api/v1/users | The full list of workers (active and deactivated), for worker management and for picking assignees when creating or editing a job. |
| [**LogoutUserApi**](AdminApi.md#logoutuserapi) | **POST** /admin/api/v1/logout-worker | This is intended to be used by admins to forcibly log another worker out, e.g. if they left their session open on a public computer. |
| [**ResetPwApi**](AdminApi.md#resetpwapi) | **POST** /admin/api/v1/reset-pw | Forces a user to change their password on next login. |
| [**RestoreApi**](AdminApi.md#restoreapi) | **POST** /admin/api/v1/restore-worker | Restores a deactivated worker via the API. |
| [**UpdateJobApi**](AdminApi.md#updatejobapi) | **PUT** /admin/api/v1/jobs/{id} | Updates an existing job. The id comes from the path, not the body. |
| [**WorkerdatapageApi**](AdminApi.md#workerdatapageapi) | **GET** /admin/api/v1/worker-data | Returns worker data entries for a given worker and date range. Numbers are strings because the server has specific logic for rounding and formatting them, and the client should not attempt to reformat them. TrueHoursWorked is the actual hours worked, while HoursWorked is the rounded up hours worked (minimum 1 hour). The total object is a WDEntry, but only the numerical fields and the completed field are filled in. The total completed field is true if all entries are completed, false if any entry is incomplete. |

<a id="changeworkerapi"></a>
# **ChangeWorkerApi**
> WorkerChangeOutput ChangeWorkerApi (WorkerChangeInput workerChangeInput)

Admins are not capable of removing their own admin privileges, and the API will return a 403 error if they try.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **workerChangeInput** | [**WorkerChangeInput**](WorkerChangeInput.md) |  |  |

### Return type

[**WorkerChangeOutput**](WorkerChangeOutput.md)

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

<a id="createjobapi"></a>
# **CreateJobApi**
> JobEditOutput CreateJobApi (JobEditInput jobEditInput)

`job_id` in the body is ignored (a new job always gets a fresh id). Responds `201 Created`.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **jobEditInput** | [**JobEditInput**](JobEditInput.md) |  |  |

### Return type

[**JobEditOutput**](JobEditOutput.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="createworkerapi"></a>
# **CreateWorkerApi**
> WorkerCreateOutput CreateWorkerApi (WorkerCreateInput workerCreateInput)

Responds `201 Created` with the new worker's id on success.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **workerCreateInput** | [**WorkerCreateInput**](WorkerCreateInput.md) |  |  |

### Return type

[**WorkerCreateOutput**](WorkerCreateOutput.md)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** |  |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="deactivateapi"></a>
# **DeactivateApi**
> DeactivateOutput DeactivateApi (DeactivateInput deactivateInput)

Admins are not capable of deactivating themselves, and the API will return a 403 error if they try.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **deactivateInput** | [**DeactivateInput**](DeactivateInput.md) |  |  |

### Return type

[**DeactivateOutput**](DeactivateOutput.md)

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

<a id="deletejobapi"></a>
# **DeleteJobApi**
> JobDeleteOutput DeleteJobApi (long id)

Deletes a job. The id comes from the path, not the body.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **long** | Job id |  |

### Return type

[**JobDeleteOutput**](JobDeleteOutput.md)

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

<a id="exportdbapi"></a>
# **ExportDbApi**
> void ExportDbApi ()

Not JSON (it streams a raw `sqlite3 .dump` of the database), content-type: application/octet-stream


### Parameters
This endpoint does not need any parameter.
### Return type

void (empty response body)

### Authorization

[bearer_auth](../README.md#bearer_auth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A raw &#x60;sqlite3 .dump&#x60; of the database. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="getuserapi"></a>
# **GetUserApi**
> WorkerSummary GetUserApi (long id)

Gets a single worker's profile, e.g. to prefill a worker-edit form.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **long** | Worker id |  |

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

<a id="jobeditpageapi"></a>
# **JobeditpageApi**
> JobEditPageOutput JobeditpageApi (long id)




### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **long** | Job id |  |

### Return type

[**JobEditPageOutput**](JobEditPageOutput.md)

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

<a id="listusersapi"></a>
# **ListUsersApi**
> List&lt;WorkerSummary&gt; ListUsersApi ()

The full list of workers (active and deactivated), for worker management and for picking assignees when creating or editing a job.


### Parameters
This endpoint does not need any parameter.
### Return type

[**List&lt;WorkerSummary&gt;**](WorkerSummary.md)

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

<a id="logoutuserapi"></a>
# **LogoutUserApi**
> LogoutUserOutput LogoutUserApi (LogoutForm logoutForm)

This is intended to be used by admins to forcibly log another worker out, e.g. if they left their session open on a public computer.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **logoutForm** | [**LogoutForm**](LogoutForm.md) |  |  |

### Return type

[**LogoutUserOutput**](LogoutUserOutput.md)

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

<a id="resetpwapi"></a>
# **ResetPwApi**
> ResetPwOutput ResetPwApi (ResetPwInput resetPwInput)

Forces a user to change their password on next login.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **resetPwInput** | [**ResetPwInput**](ResetPwInput.md) |  |  |

### Return type

[**ResetPwOutput**](ResetPwOutput.md)

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

<a id="restoreapi"></a>
# **RestoreApi**
> RestoreOutput RestoreApi (RestoreInput restoreInput)

Restores a deactivated worker via the API.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **restoreInput** | [**RestoreInput**](RestoreInput.md) |  |  |

### Return type

[**RestoreOutput**](RestoreOutput.md)

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

<a id="updatejobapi"></a>
# **UpdateJobApi**
> JobEditOutput UpdateJobApi (long id, JobEditInput jobEditInput)

Updates an existing job. The id comes from the path, not the body.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **long** | Job id |  |
| **jobEditInput** | [**JobEditInput**](JobEditInput.md) |  |  |

### Return type

[**JobEditOutput**](JobEditOutput.md)

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

<a id="workerdatapageapi"></a>
# **WorkerdatapageApi**
> WorkerDataOutput WorkerdatapageApi (long worker = null, DateOnly startDate = null, DateOnly endDate = null)

Returns worker data entries for a given worker and date range. Numbers are strings because the server has specific logic for rounding and formatting them, and the client should not attempt to reformat them. TrueHoursWorked is the actual hours worked, while HoursWorked is the rounded up hours worked (minimum 1 hour). The total object is a WDEntry, but only the numerical fields and the completed field are filled in. The total completed field is true if all entries are completed, false if any entry is incomplete.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **worker** | **long** |  | [optional]  |
| **startDate** | **DateOnly** |  | [optional]  |
| **endDate** | **DateOnly** |  | [optional]  |

### Return type

[**WorkerDataOutput**](WorkerDataOutput.md)

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

