using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Models;
using Newtonsoft.Json;
using Repository;

/*
 * Created by: Naveen David
 * Task: task-sample for C3 Post Trade Assist
 * Started: 22 Septeber
 * Completed  27 September
 */
namespace TaskAPI.Controllers
{

    public class TaskController : ApiController
    {
        private IRepository _taskRepository;

        public TaskController()
        {
            _taskRepository = new TaskSQLRepository();

        }


        [HttpGet, Route("api/Tasks")]

        public IHttpActionResult GetAll([FromUri] PagingModel pagingModel)
        {
            //Get the tasks from the repository
            (var tasks, string errorMessage) = _taskRepository.GetAll();

            //If successful
            if (tasks != null && tasks.Count > 0)
            {
                //Check to see if paging parameters of PageNumber or Pagesize have been passed in.
                if (pagingModel != null)
                {
                    //Call pagination method. Return results and any error message
                    (List<TaskModel> paginatedResults, string paginationErrorMessage) = Pagination(tasks, pagingModel);

                    //If pagination was successful
                    if (paginatedResults != null)
                    {
                        //Return data
                        return Json(JsonConvert.SerializeObject(paginatedResults));
                    }
                    else
                    {
                        //Return error code and error message
                        return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + paginationErrorMessage);
                    }
                }
                //Proceed without pagination
                else
                {
                    return Json(JsonConvert.SerializeObject(tasks));
                }
            }
            else
            {
                //No data to retrieve
                return Json(HttpStatusCode.NoContent.ToString() + ":" + (int)HttpStatusCode.NoContent + "  " + errorMessage);
            }
        }

        [HttpGet, Route("api/Tasks")]
        public IHttpActionResult GetTasksByOwner(string owner, [FromUri] PagingModel pagingModel)
        {


            if (String.IsNullOrWhiteSpace(owner))
            {
                //Return error that the guid string passed in was empty
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Owner string was empty, could not GetTasksByOwner.");
            }

            Guid OwnerResult;

            if ((String.IsNullOrWhiteSpace(owner)) || !Guid.TryParse(owner.ToLower(), out OwnerResult))
            {
                //Return error that the guid string passed in was invalid
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Owner guid not valid, could not GetTasksByOwner.");
            }

            //Call the repository
            (var tasks, string errorMessage) = _taskRepository.GetTasksByOwner(OwnerResult);

            //If successful
            if (tasks != null && tasks.Count > 0)
            {
                //Check to see if paging parameters of PageNumber or Pagesize have been passed in.
                if (pagingModel != null)
                {
                    //Call pagination method. Return results and any error message
                    (List<TaskModel> paginatedResults, string paginationErrorMessage) = Pagination(tasks, pagingModel);

                    //If pagination was successful
                    if (paginatedResults != null)
                    {
                        //Return data
                        return Json(JsonConvert.SerializeObject(paginatedResults));
                    }
                    else
                    {
                        //Return error code and error message
                        return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + paginationErrorMessage);
                    }
                }
                //Proceed without pagination
                else
                {
                    return Json(JsonConvert.SerializeObject(tasks));
                }
            }
            else
            {
                //No data to retrieve
                return Json(HttpStatusCode.NoContent.ToString() + ":" + (int)HttpStatusCode.NoContent + "  " + errorMessage);
            }

        }

        [HttpGet, Route("api/Tasks")]
        public IHttpActionResult GetTasksByCompleted(bool? completed, [FromUri] PagingModel pagingModel)
        {
            //Make sure the parameters are correct
            if (!completed.HasValue)
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Completed status was empty, could not GetTasksByCompleted.");
            }

            //Call the repository
            (var tasks, string errorMessage) = _taskRepository.GetTasksByCompleted(completed.Value);


            //If successful
            if (tasks != null && tasks.Count > 0)
            {
                //Check to see if paging parameters of PageNumber or Pagesize have been passed in.
                if (pagingModel != null)
                {
                    //Call pagination method. Return results and any error message
                    (List<TaskModel> paginatedResults, string paginationErrorMessage) = Pagination(tasks, pagingModel);

                    //If pagination was successful
                    if (paginatedResults != null)
                    {
                        //Return data
                        return Json(JsonConvert.SerializeObject(paginatedResults));
                    }
                    else
                    {
                        //Return error code and error message
                        return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + paginationErrorMessage);
                    }
                }
                //Proceed without pagination
                else
                {
                    return Json(JsonConvert.SerializeObject(tasks));
                }
            }
            else
            {
                //No data to retrieve
                return Json(HttpStatusCode.NoContent.ToString() + ":" + (int)HttpStatusCode.NoContent + "  " + errorMessage);
            }

        }

        [HttpGet, Route("api/Tasks")]
        public IHttpActionResult GetTaskById(string Id)
        {
            //See if the Id has been pased 
            if (String.IsNullOrWhiteSpace(Id))
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Id was empty, could not GetTaskById.");
            }

            Guid OwnerResult;

            //Parse Id as guid
            if ((String.IsNullOrWhiteSpace(Id)) || !Guid.TryParse(Id.ToLower(), out OwnerResult))
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Id guid not valid, could not GetTaskById.");
            }

            //Call repository to get task by Id
            (var task, string errorMessage) = GetTaskById(OwnerResult);

            if (task != null)
            {
                //Return data as json
                return Json(JsonConvert.SerializeObject(task));
            }
            else
            {
                return Json(HttpStatusCode.NoContent.ToString() + ":" + (int)HttpStatusCode.NoContent + "  " + errorMessage);
            }
        }


        [HttpPost, Route("api/Tasks")]
        public IHttpActionResult AddTask([FromBody] TaskModel task)
        {
            //Make sure data is passed in via the body
            if (task == null)
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Task passed in was null");
            }

            Guid ownerResult;

            //Make user the owner is a valid guid
            if ((String.IsNullOrWhiteSpace(task.Owner)) || !Guid.TryParse(task.Owner.ToLower(), out ownerResult))
            {

                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Owner guid not valid");
            }

            //Make sure the description exists
            if (String.IsNullOrWhiteSpace(task.Description))
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Description not valid");
            }

            //Call repository to insert data
            (bool bSuccess, string errorMessage) = _taskRepository.Insert(task.Description, ownerResult);

            //return success code and message
            if (bSuccess == true)
            {
                return Json("Task '" + task.Description + "' with the owner '" + task.Owner + "' was saved to the database");
            }
            //If error inserting into the database
            else
            {
                return Json(HttpStatusCode.NotAcceptable.ToString() + ":" + (int)HttpStatusCode.NotAcceptable + "  " + errorMessage);
            }

        }

        [HttpPut, Route("api/Tasks")]
        public IHttpActionResult UpdateTask([FromBody] TaskModel jsonTask)
        {
            //Make sure data is passed in via the body
            if (jsonTask == null)
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Task passed in was null");
            }

            //Validate Id string
            if (String.IsNullOrWhiteSpace(jsonTask.Id))
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Id guid is empty");
            }

            Guid IdResult;

            //Parse Id as guid
            if (!Guid.TryParse(jsonTask.Id, out IdResult))
            {
                return Json(HttpStatusCode.BadRequest.ToString() + ":" + (int)HttpStatusCode.BadRequest + "  " + "Id guid is not valid");
            }
            
            //Call repository to get Task by Id i.e make sure it exists before updating it.
            (TaskModel taskResult, string errorGetMessage) = GetTaskById(IdResult);

            //If the get was successful 
            if (taskResult != null)
            {
                //Update the task that we have retrieved from the database with the completed status from the body section of the json data
                taskResult.Completed = jsonTask.Completed; 
                
                //Call repository to update task
                (bool bSuccess, string errorUpdateMessage) = _taskRepository.Update(taskResult);

                //if update successful return success message
                if (bSuccess == true)
                {
                    return Json("Task '" + jsonTask.Id + "' was successfully updated with the completed status '" + jsonTask.Completed.ToString() + "'");
                }
                else
                {
                    //This response is sent when the web server, after performing server-driven content negotiation, 
                    //doesn't find any content that conforms to the criteria given by the user agent.
                    return Json(HttpStatusCode.NotAcceptable.ToString() + ":" + (int)HttpStatusCode.NotAcceptable + "  " + errorUpdateMessage);
                }
            }
            else
            {
                //This response is sent when the web server, after performing server-driven content negotiation, 
                //doesn't find any content that conforms to the criteria given by the user agent.
                return Json(HttpStatusCode.NotAcceptable.ToString() + ":" + (int)HttpStatusCode.NotAcceptable + "  " + errorGetMessage);
            }




        }

        #region Methods

        /// <summary>
        /// This code is used in a couple of places and has been abstracted out.
        /// This code will call the repository to retrieve a task by the guid Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private (TaskModel, string) GetTaskById(Guid Id)
        {

            (var task, string errorMessage) = _taskRepository.GetTaskById(Id);

            return (task, errorMessage);
        }

        /// <summary>
        /// This code has been taken from https://www.c-sharpcorner.com/article/how-to-do-paging-with-asp-net-web-api/ and modified
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="pagingModel"></param>
        /// <returns></returns>
        private (List<TaskModel>, string) Pagination(List<TaskModel> tasks, PagingModel pagingModel)
        {
            try
            {

                // Get's No of Rows Count   
                int count = tasks.Count;

                // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
                int CurrentPage = pagingModel.pageNumber;

                // Parameter is passed from Query string if it is null then it default Value will be pageSize:10  
                int PageSize = pagingModel.pageSize;

                // Display TotalCount to Records to User  
                int TotalCount = count;

                // Calculating Totalpage by Dividing (No of Records / Pagesize)  
                int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

                // Returns List of tasks after applying Paging   
                var items = tasks.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                // if CurrentPage is greater than 1 means it has previousPage  
                var previousPage = CurrentPage > 1 ? "Yes" : "No";

                // if TotalPages is greater than CurrentPage means it has nextPage  
                var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

                // Object which we are going to send in header   
                var paginationMetadata = new
                {
                    totalCount = TotalCount,
                    pageSize = PageSize,
                    currentPage = CurrentPage,
                    totalPages = TotalPages,
                    previousPage,
                    nextPage
                };

                // Setting Header  
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

                return (items, string.Empty);

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        #endregion
    }
}


