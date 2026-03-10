using HashProcessor.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HashProcessor.API.Infrastructure
{
    public class MariaDbContext : DbContext
    {
        public MariaDbContext(DbContextOptions<MariaDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Hash> Hashes { get; set; }
    }
}
