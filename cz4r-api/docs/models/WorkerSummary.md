# Org.OpenAPITools.Model.WorkerSummary
A worker's admin-visible profile, safe to expose over the JSON API - unlike [`Worker`], this deliberately excludes `hash`/`salt`.  This is the shape used both by `/admin/api/v1/users` (worker management) and, filtered to active workers, by job-assignment flows that just need to know who exists and pick some subset of them.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Address** | **string** |  | 
**Admin** | **bool** |  | 
**Deactivated** | **bool** |  | 
**Email** | **string** |  | 
**FlatRateCents** | **long** |  | 
**Id** | **long** |  | 
**MustChangePw** | **bool** |  | 
**Name** | **string** |  | 
**Phone** | **string** |  | 
**RateDriveHourlyCents** | **long** |  | 
**RateHourlyCents** | **long** |  | 
**RateMileageCents** | **long** |  | 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

