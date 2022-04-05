using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListWebApi.Entities;

namespace TodoListWebApi.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "string",
                    PasswordSalt = Convert.FromBase64String("zweU6WVl6w8="),
                    PasswordHash = Convert.FromBase64String("QA0lTPLncoID3yJx1SJaFHSYVSbtIr1T8jFpExjxbhg=") // Password: string
                },
                new User
                {
                    Id = 2,
                    Username = "admin",
                    PasswordSalt = Convert.FromBase64String("zZ8e5nU1Qno="),
                    PasswordHash = Convert.FromBase64String("HBYjp7v1R4VKMgbGf8gwPLdQbbNR/sZSujVaVOVBpCw="), // Password: password
                    Role = RoleTypes.Admin
                },
                new User
                {
                    Id = 3,
                    Username = "user1",
                    PasswordSalt = Convert.FromBase64String("WWrB/VrXmyg="),
                    PasswordHash = Convert.FromBase64String("GRDoYwzdhB/Af6Ln+yy5Eo1Mq6Kxs8vBpZx5vSc7hf4=") // Password: password
                });

            modelBuilder.Entity<TodoTask>().HasData(
                new TodoTask() {Id = 1, UserId = 1, Title = "Barber", Description = "Go to barber", DeadlineDate = DateTime.Now.AddDays(2)},
                new TodoTask() {Id = 2, UserId = 1, Title = "Grocery store", Description = "Buy milk, butter and orange juice.", DeadlineDate = DateTime.Now.AddDays(1)},
                new TodoTask() {Id = 3, UserId = 1, Title = "Call to dad", Description = "Ask him about a car." },
                new TodoTask() {Id = 4, UserId = 1, Title = "Pay bills", Description = "Lorem ipsum." },
                new TodoTask() {Id = 5, UserId = 2, Title = "Hit the gym", Description = "Lorem ipsum." },
                new TodoTask() {Id = 6, UserId = 2, Title = "Buy eggs", Description = "Lorem ipsum." },
                new TodoTask() {Id = 7, UserId = 2, Title = "Read a book", Description = "Lorem ipsum." },
                new TodoTask() {Id = 8, UserId = 3, Title = "Organize office", Description = "Lorem ipsum." },
                new TodoTask() {Id = 9, UserId = 3, Title = "Do amazing things!", Description = "Lorem ipsum." },
                new TodoTask() {Id = 10, UserId = 3, Title = "Finish the Docker workshop", Description = "Lorem ipsum." });
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
    }
}
