using Microsoft.EntityFrameworkCore;
using Nerdklubben.Domain.Entities;


namespace Nerdklubben.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ApplicationEntity> Applications { get; set; } = null!;




    }
}
