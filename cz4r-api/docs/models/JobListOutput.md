# Org.OpenAPITools.Model.JobListOutput
The full result of a job-list search: every matching job/assignment row plus the (possibly defaulted) search parameters that produced it. Shared by both the HTML page and the JSON API.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Admin** | **bool** |  | 
**Assigned** | **bool** |  | 
**Completed** | **bool** |  | 
**Count** | **int** |  | 
**Jobs** | [**List&lt;JobData&gt;**](JobData.md) |  | 
**Order** | **Order** |  | 
**Params** | [**SearchParams**](SearchParams.md) |  | 
**Started** | **bool** |  | 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

