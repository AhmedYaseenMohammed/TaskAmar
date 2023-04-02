using Microsoft.EntityFrameworkCore;
using testTask.Models;

namespace testTask.Data
{
    public class DbData:DbContext
    {
        public DbData(DbContextOptions<DbData> options) : base(options) { }
        public DbSet<Information> informations { get; set; }
        public DbSet<CoursesOfPersonal> coursesOfPersonals { get; set; }
        public DbSet<PromotionsOfPersonal> promotionsOfPersonals { get; set; }
        public DbSet<Users> users { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            //string? currentUserId = _httpContextAccessor?.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).InsertDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).IsDeleted = false;

                    //((BaseEntity)entity.Entity).InsertByUserId = currentUserId!;
                }

                else
                {
                    // Only update the UpdateDate property in the Modified state
                    ((BaseEntity)entity.Entity).UpdateDate = DateTime.UtcNow;
                }
            }
        }
    }
}
