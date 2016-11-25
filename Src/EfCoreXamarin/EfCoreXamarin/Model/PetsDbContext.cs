using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class PetsDbContext : DbContext
    {
        public static string DataBasePath { get; set; }

        public DbSet<Person> People { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Platypus> Platypuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DataBasePath}");
        }
    }
}
