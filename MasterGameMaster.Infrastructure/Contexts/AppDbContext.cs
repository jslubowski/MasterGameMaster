using Microsoft.EntityFrameworkCore;

namespace MasterGameMaster.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
