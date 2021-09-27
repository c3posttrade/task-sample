using System;
using System.Collections.Generic;

using Models;


namespace Repository
{

    public interface IRepository
    {
        (List<TaskModel>, string) GetAll();
        (TaskModel, string) GetTaskById(Guid Id);
        (List<TaskModel>, string) GetTasksByOwner(Guid Owner);
        (List<TaskModel>, string) GetTasksByCompleted(bool bCompleted);
        (bool, string) Insert(string description, Guid owner);

       
        (bool, string) Update(TaskModel task);


    }
}
