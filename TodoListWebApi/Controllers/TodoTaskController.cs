using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListWebApi.DTOs;
using TodoListWebApi.Entities;
using TodoListWebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoTaskController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;

        public TodoTaskController(ITodoTaskService todoTaskService)
        {
            _todoTaskService = todoTaskService;
        }

        [HttpGet]
        public IEnumerable<TodoTask> Get()
        {
            return _todoTaskService.GetMyTasks();
        }

        [HttpGet("all")]
        [Authorize(Roles = nameof(RoleTypes.Admin))]
        public IEnumerable<TodoTask> GetAll()
        {
            return _todoTaskService.GetAll();
        }


        [HttpGet("{id}")]
        public TodoTask? Get(uint id)
        {
            return _todoTaskService.Get(id);
        }

        [HttpPost]
        public void Add(TodoTaskDTO todoTaskDto)
        {
            _todoTaskService.Add(todoTaskDto);
        }

        [HttpPut("{id}")]
        public void Update(uint id, TodoTaskDTO todoTaskDto)
        {
            _todoTaskService.Update(id, todoTaskDto);
        }

        [HttpDelete("{id}")]
        public void Delete(uint id)
        {
            _todoTaskService.Delete(id);
        }
    }
}
