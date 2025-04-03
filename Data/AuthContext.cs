using Microsoft.EntityFrameworkCore;
using TodoApiNovo.Models;

namespace TodoApiNovo.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}