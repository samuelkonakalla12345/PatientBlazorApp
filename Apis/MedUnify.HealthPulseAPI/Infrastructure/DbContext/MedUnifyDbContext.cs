namespace MedUnify.HealthPulseAPI.DbContext
{
    using MedUnify.Domain.Auth;
    using MedUnify.Domain.HealthPulse;
    using Microsoft.EntityFrameworkCore;

    public class MedUnifyDbContext : DbContext
    {
        public MedUnifyDbContext(DbContextOptions<MedUnifyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<ProgressNote> ProgressNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Patient entity
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId);

                entity.HasIndex(e => new { e.FirstName, e.MobileNumber, e.OrganizationId })
                      .HasDatabaseName("IX_Patients_FirstName_MobileNumber_OrganizationId");

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.State).HasMaxLength(100);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.MobileNumber).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configure Visit entity
            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasKey(e => e.VisitId);

                entity.HasOne(e => e.Patient)
                      .WithMany()
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.VisitDate).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configure ProgressNote entity
            modelBuilder.Entity<ProgressNote>(entity =>
            {
                entity.HasKey(e => e.ProgressNoteId);

                entity.HasOne(e => e.Visit)
                      .WithMany()
                      .HasForeignKey(e => e.VisitId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.SectionName).HasMaxLength(255);
                entity.Property(e => e.SectionText).HasColumnType("nvarchar(max)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}