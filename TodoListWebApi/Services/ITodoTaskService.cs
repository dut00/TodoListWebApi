using TodoListWebApi.DTOs;
using TodoListWebApi.Entities;

namespace TodoListWebApi.Services;

public interface ITodoTaskService
{
    IEnumerable<TodoTask> GetAll();

    TodoTask? Get(uint id);
    TodoTask? Add(TodoTaskDTO todoTaskDTO);
    TodoTask? Update(uint id, TodoTaskDTO todoTaskDTO);
    void Delete(uint id);

    public IEnumerable<TodoTask> GetMyTasks();
}