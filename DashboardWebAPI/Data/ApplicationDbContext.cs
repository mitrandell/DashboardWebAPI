using DashboardWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DashboardWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BussinessDayType>(entity =>
            {
                entity.ToTable(name: "BussinessDayTypeSet");

                var workDayType = new BussinessDayType
                {
                    Id = 1,
                    Title = "Рабочий"
                };

                var weekendDayType = new BussinessDayType
                {
                    Id = 2,
                    Title = "Выходной"
                };

                entity.HasData(workDayType, weekendDayType);
            });

            builder.Entity<TaskData>(entity =>
            {
                entity.Property(x => x.CreateAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
                entity.Property(x => x.StartTaskDate).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
                entity.Property(x => x.EndTaskDate).HasColumnType("timestamp without time zone").HasDefaultValueSql("null");
                entity.Property(x => x.ExecutedTime).IsRequired(false);
            });

            builder.Entity<BussinessDay>(entity =>
            {
                entity.Property(x => x.Date).HasColumnType("timestamp without time zone");
            });

            builder.Entity<CriticalTask>(entity =>
            {
                entity.Property(x => x.CreateAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            });

            builder.Entity<DeveloperTask>(entity =>
            {
                entity.Property(x => x.CreateAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            });

            builder.Entity<ScriptNote>(entity =>
            {
                entity.Property(x => x.CreateAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            });
            
            builder.Entity<ScriptDescription>(entity =>
            {
                entity.Property(x => x.CreateAt).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            });
        }

        public DbSet<TaskData> TaskSet { get; set; }
        public DbSet<BussinessDay> BussinessDaySet { get; set; }
        public DbSet<BussinessDayType> BussinessDayTypeSet { get; set; }
        public DbSet<CriticalTask> CriticalTaskSet { get; set; }
        public DbSet<DeveloperTask> DeveloperTaskSet { get; set; }
        public DbSet<User> UserSet { get; set; }
        public DbSet<Note> NoteSet { get; set; }
        public DbSet<ScriptNote> ScriptNoteSet { get; set; }
        public DbSet<ScriptDescription> ScriptDescriptionNoteSet { get; set; }

    }
}
