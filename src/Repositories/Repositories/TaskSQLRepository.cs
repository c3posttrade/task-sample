using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Repository;
using Datalayer.EntityModel;
using Models;

namespace Repository
{
    public class TaskSQLRepository : IRepository
    {
        private readonly TaskDBContext _context;

        public TaskSQLRepository()
        {
            _context = new TaskDBContext();
        }

        public TaskSQLRepository(TaskDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all tasks from the database table task table
        /// </summary>
        /// <returns></returns>
        public (List<TaskModel>, string) GetAll()
        {
            try
            {
              

                //Check if data exists in the database
                if (_context.Tasks.ToList().Count > 0)
                {
                    //Pass in the entity data and allow the Convert method to return back a TaskModel
                    return (ConvertEFDataToModel(_context.Tasks.ToList()), string.Empty);

                    
                }
                //Warning is returned rather than an error, developers may decide to use this to display on the front end if required.
                else
                {
                    return (null, "Warning: there were no tasks to retrieve");
                }

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        /// <summary>
        /// Get task from the database by guid Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public (TaskModel, string) GetTaskById(Guid Id)
        {
            try
            {

                //Select all tasks from the entity model
                var taskData = (from t in _context.Tasks
                                where t.Id == Id
                                select t).FirstOrDefault();

                //Check if a task with the supplied Id exists.
                if (taskData != null)
                {
                    //Extra error checking even though a lot of these should never be null.
                    if (taskData.Id != null && taskData.Created != null && taskData.Description != null && taskData.Owner != null && taskData.Updated != null)
                    {
                        TaskModel task = new TaskModel()
                        {
                            Id = taskData.Id.ToString(),
                            Completed = taskData.Completed,
                            Created = taskData.Created.ToString(),
                            Description = taskData.Description,
                            Owner = taskData.Owner.ToString(),
                            Updated = taskData.Updated.ToString()
                        };

                        return (task, string.Empty);
                    }
                    //If there is any data corruption in the database throw an error
                    else
                    {
                        throw new Exception("Error: when trying to GetTaskById() tasks. One or more fields were null. Please check database for missing values in the Task database table");
                    }
                }
                //Warning is returned rather than an error, developers may decide to use this to display on the front end if required.
                else
                {
                    return (null, "Warning: could not GetTaskById as there wasn't a task with the Id " + Id.ToString() + " in the database");
                }

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        /// <summary>
        /// Gets tasks from the database by the boolean completed
        /// </summary>
        /// <param name="bCompleted"></param>
        /// <returns></returns>
        public (List<TaskModel>, string) GetTasksByCompleted(bool bCompleted)
        {
            try
            {
                //Select tasks by completed from the entity model
                var lstTaskEFData = (from t in _context.Tasks
                                   where t.Completed == bCompleted
                                   select t).ToList();


                //Check if data exists in the database
                if (lstTaskEFData.Count > 0)
                {
                    //Pass in the entity data and allow the Convert method to return back a TaskModel
                    return (ConvertEFDataToModel(lstTaskEFData), string.Empty);

                }
                //Warning is returned rather than an error, developers may decide to use this to display on the front end if required.
                else
                {
                    return (null, "Warning: could not GetTaskByCompleted as there wasn't a task with the completed status of " + bCompleted.ToString());
                }

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        /// <summary>
        /// Get tasks from database by the owner guid
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public (List<TaskModel>, string) GetTasksByOwner(Guid owner)
        {
            try
            {
                //Select tasks by owner from the entity model
                var lstTaskEFData = (from t in _context.Tasks
                                   where t.Owner == owner
                                   select t).ToList();

                if (lstTaskEFData.Count > 0)
                {
                    //Pass in the entity data and allow the Convert method to return back a TaskModel
                    return (ConvertEFDataToModel(lstTaskEFData), string.Empty);

                }
                //Warning is returned rather than an error, developers may decide to use this to display on the front end if required.
                else
                {
                    return (null, "Warning: could not GetTaskByowner as there weren't tasks associated with that the owner " + owner);
                }

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        /// <summary>
        /// Insert into database bod. 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public (bool, string) Insert(string description, Guid owner)
        {
            try
            {


                Task taskData = new Task()
                {

                    Id = Guid.NewGuid(),
                    Completed = false,
                    Created = DateTime.UtcNow,
                    Description = description,
                    Owner = owner,
                    Updated = DateTime.UtcNow
                };

                _context.Tasks.Add(taskData);

                //Retrieve success from save attempt
                (int numberOfEntriesAffectted, string errorMessage) = Save();

                if (numberOfEntriesAffectted > 0)
                {
                    //return success
                    return (true, string.Empty);
                }
                else
                {
                    //return failure and erro message
                    return (false, "Could not Insert task into the database. Code failed when trying to save.");
                }

            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// Save code for saving changes to the context
        /// </summary>
        /// <returns></returns>
        private (int, string) Save()
        {

            try
            {
                return (_context.SaveChanges(), string.Empty);
            }
            catch (Exception ex)
            {
                return (-1, ex.Message);
            }
        }


        /// <summary>
        /// The Update will usually occur on the Completed and Updated fields
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public (bool, string) Update(TaskModel task)
        {
            try
            {


                if (task != null && task.Id != null && !String.IsNullOrWhiteSpace(task.Created) && !String.IsNullOrWhiteSpace(task.Updated)
                    && task.Description != null && !string.IsNullOrWhiteSpace(task.Description) && task.Owner != null)
                {

                    Task taskData = new Task();

                    Guid IdResult;

                    if (Guid.TryParse(task.Id, out IdResult))
                    {
                        taskData.Id = IdResult;
                    }
                    else
                    {
                        return (false, "Could not Update task please make sure the Id of the task is a valid guid");
                    }

                    Guid ownerResult;

                    if (Guid.TryParse(task.Owner, out ownerResult))
                    {
                        taskData.Owner = ownerResult;
                    }
                    else
                    {
                        return (false, "Could not Update task please make sure the Owner of the task is a valid guid");
                    }


                    taskData.Completed = task.Completed;
                    taskData.Created = Convert.ToDateTime(task.Created);
                    taskData.Description = task.Description;
                    taskData.Updated = DateTime.UtcNow;

                    //Update the context with the updated entity data model.
                    Task existingTask = _context.Tasks.Find(taskData.Id);
                    if (existingTask != null)
                    {
                        //Remove the existing entity data model
                        _context.Tasks.Remove(existingTask);
                    
                    }

                    _context.Tasks.Add(taskData);



                    (int numberOfEntriesAffected, string errorMessage) = Save();

                    if (numberOfEntriesAffected > 0)
                    {
                        return (true, string.Empty);
                    }
                    else
                    {
                        return (false, "Error: Could not Update task into the database. Code failed in the Save() section of the Update method.");
                    }

                }
                else
                {
                    return (false, "Error: Could not Update the task to the database, please make sure all the fields have been populated");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// This method will take a list of entity data and return back a task model
        /// </summary>
        /// <param name="lstTaskEFData"></param>
        /// <returns></returns>
        private List<TaskModel> ConvertEFDataToModel(List<Task> lstTaskEFData)
        {
            List<TaskModel> lstTask = new List<TaskModel>();

            for (int i = 0; i < lstTaskEFData.Count; i++)
            {
                Task taskEFData = lstTaskEFData[i];

                //Extra error checking even though a lot of these should never be null.
                if (taskEFData.Id != null && taskEFData.Created != null && taskEFData.Description != null && taskEFData.Owner != null && taskEFData.Updated != null)
                {

                    TaskModel task = new TaskModel()
                    {
                        Id = taskEFData.Id.ToString(),
                        Completed = taskEFData.Completed,
                        Created = taskEFData.Created.ToString(),
                        Description = taskEFData.Description,
                        Owner = taskEFData.Owner.ToString(),
                        Updated = taskEFData.Updated.ToString()
                    };

                    lstTask.Add(task);


                }
                //If there is any data corruption in the database throw an error
                else
                {
                    break;
                    throw new Exception("Error: when trying to convert entity data to model data, ConvertEFDataToModel(). Please check database for missing values in the Task database table");
                }
            }

            return lstTask;
        }
    }
}

