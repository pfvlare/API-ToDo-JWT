using Microsoft.EntityFrameworkCore;
using TodoApiNovo.Models;

namespace TodoApiNovo.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    }
}