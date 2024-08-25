namespace MedUnify.AuthAPI.DbContext
{
    using MedUnify.Domain.Auth;
    using MedUnify.Domain.HealthPulse;
    using Microsoft.EntityFrameworkCore;

    public class MedUnifyDbContext : DbContext
    {
        public DbSet<OAuthClient> OAuthClients { get; set; }

        public MedUnifyDbContext(DbContextOptions<MedUnifyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}