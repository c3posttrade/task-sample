# Overview
You have been tasked with creating a dotnet web api to return tasks for an external web app to consume. You have full reign for how this project will structured, but it is expected that your approach will follow your own best practice guidlines. 

## Overview
The underlying data for this service will be sourced from [Todo List API](https://documenter.getpostman.com/view/8858534/SW7dX7JG#4d4e14a4-3f2d-4115-91fd-27f2db6227a0) provided by Postman. All enpoints and responses can be found in the link, the task contract is defined below: 

```
{
    "completed": true,
    "_id": "61275fac8924ac0017e24359",
    "description": "dddd",
    "owner": "6008cc3e5ab46f0017b8f64e",
    "createdAt": "2021-08-26T09:32:28.325Z",
    "updatedAt": "2021-09-08T06:01:20.092Z",
    "__v": 0
},
```

# Requirements
The below requirements have been supplied to you by the business and architectural team

## Storage
Each Task will need to be able to be persisited to a data store. Your architecture team have not yet decided on the underlying storage so the requirement is to store the data in memory with the ability to switch this out easily when required.
