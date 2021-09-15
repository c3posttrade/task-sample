# Overview
You have been tasked with creating a dotnet web api to return tasks for an external web app to consume. You have full reign for how this project will structured, but it is expected that your approach will follow your own best practice guidelines. 

```
{
    "id": "61275fac8924ac0017e24359",
    "description": "sample task",
    "completed": true,
    "owner": "6008cc3e5ab46f0017b8f64e",
    "createdAt": "2021-08-26T09:32:28.325Z",
    "updatedAt": "2021-09-08T06:01:20.092Z"
},
```

## Business Requirements
The business has defined the below requirements for the service:

1. As a user, I need to be able to create a task with a given description so that I can manage my tasks
2. As a user, I need to be able to mark a task as completed so that I know when a task is done 
3. As a user, I need to be able to retrieve tasks by owner so that I can view tasks that I need to action

## Technical Requirements

1. Storage
    - Each Task will need to be able to be persisited to a data store
    - Architecture have not yet decided on the underlying storage so data will need to be stored in a mock data store with the ability to switch this out easily when required.
2. Authentication
    - There are no requirements for authentication of the api endpoints
