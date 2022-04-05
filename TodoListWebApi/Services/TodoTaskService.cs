using System.Diagnostics;
using System.Security.Claims;
using TodoListWebApi.Data;
using TodoListWebApi.DTOs;
using TodoListWebApi.Entities;

namespace TodoListWebApi.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly DataContext _context;

        public TodoTaskService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public IEnumerable<TodoTask> GetAll()
        {
            //IQueryable<TodoTask> result;
            //result = _context.TodoTasks
            //    .OrderBy(string.IsNullOrEmpty(queryParameters.SortBy) ? "id" : queryParameters.SortBy
            //                + (queryParameters.IsDescending() ? " desc" : String.Empty));                       // Linq.Dynamic.Core

            //if (queryParameters.Details)
            //{
            //    result = result.Include(c => c.IsDone);
            //}

            return _context.TodoTasks;
        }

        public TodoTask? Get(uint id)
        {
            TodoTask? tasks;
            if (_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) == nameof(RoleTypes.Admin))
                tasks = _context.TodoTasks.FirstOrDefault(t => t.Id == id);
            else
                tasks = _context.TodoTasks.FirstOrDefault(t => t.Id == id && t.UserId == GetUserId());

            return tasks;
        }

        public TodoTask? Add(TodoTaskDTO todoTaskDTO)
        {
            TodoTask? todoTask = null;
            uint userId = GetUserId();
            if (userId > 0)
            {
                todoTask = new TodoTask()
                {
                    UserId = userId,
                    Title = todoTaskDTO.Title,
                    Description = todoTaskDTO.Description,
                    DeadlineDate = todoTaskDTO.DeadlineDate,
                };

                if (!string.IsNullOrEmpty(todoTask.Title) &&
                    !string.IsNullOrEmpty(todoTask.Description))
                {
                    _context.TodoTasks.Add(todoTask);
                    _context.SaveChanges();
                }
            }

            return todoTask;
        }

        public TodoTask? Update(uint id, TodoTaskDTO todoTaskDTO)
        {
            // TODO: Admin - all, Standard user - own.

            var todoTask = new TodoTask()
            {
                Id = id,
                Title = todoTaskDTO.Title,
                Description = todoTaskDTO.Description,
                DeadlineDate = todoTaskDTO.DeadlineDate,
            };

            if (!string.IsNullOrEmpty(todoTask.Title) &&
                !string.IsNullOrEmpty(todoTask.Description))
            {
                _context.Attach(todoTask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

            return todoTask;
        }

        public void Delete(uint id)
        {
            // TODO: Admin - all, Standard user - own.

            TodoTask task = _context.TodoTasks.FirstOrDefault(task => task.Id == id);
            if (task != null)
            {
                _context.TodoTasks.Remove(task);
                _context.SaveChanges();
            }
            else
            {
                Debug.WriteLine($"The task with ID={id} doesn't exist.");
            }
        }

        public IEnumerable<TodoTask> GetMyTasks()
        {
            uint userId = GetUserId();

            return _context.TodoTasks.Where(t => t.UserId == userId);
        }

        private uint GetUserId()
        {
            uint userId = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                userId = Convert.ToUInt32(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            return userId;
        }
    }
}
