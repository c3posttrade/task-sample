# Sample Task API
This repository has been created to assist developer recruitment for C3 Post Trade

# Overview
You have been tasked with creating a dotnet web api to return tasks for an external web app to consume. You have full reign over how this project will structured, but it is expected that your approach will follow your own best practice guidelines. 


## Business Requirements
The business has defined the below requirements for the service:

1. As an api user, I need to be able to create a task with a given description and owner so that I can store information about a task
2. As an api user, I need to be able to mark a task as completed so that I am able to specify when a task is done 
3. As an api user, I need to be able to retrieve tasks by owner so that I can identify tasks created by a specific owner
4. As an api user, I need to be able to page tasks returned from the api so that I can process large result sets
5. As an api user, I need to be able to retrieve tasks by the completed field so that I can identify tasks that are active or in-active
7. As an api user, I need to be able to retrieve a task by its id so that I can look up information for a specific task

## Technical Requirements

1. Each task will need to be able to be persisited to a data store
2. A decision has not been made on the underlying storage, so data will need to be stored in a mock data store with the ability to switch this out easily when required
3. There are no current requirements for authentication of the api endpoints
4. A task must have a datetime to represent when the task was created/updated and all datetimes should be in UTC
5. A task must have an owner populated
6. Api endpoints must return data in Json format and must accept data in Json format
7. Api must return valid HTTP response codes

Sample Task

```
{
    "id": "61275fac8924ac0017e24359",
    "description": "sample task",
    "completed": true,
    "owner": "6008cc3e5ab46f0017b8f64e",
    "created": "2021-08-26T09:32:28.325Z",
    "updated": "2021-09-08T06:01:20.092Z"
}
```


