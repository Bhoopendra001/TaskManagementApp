using System.Data.Entity;

namespace TaskManagementApp.Models
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}
