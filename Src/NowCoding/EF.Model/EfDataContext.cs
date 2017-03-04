using EF.Model.Model;
using Microsoft.EntityFrameworkCore;

namespace EF.Model
{
    public class EfDataContext : DbContext
    {
        private string _connectionString;

        public EfDataContext()
        {
            _connectionString = "FileName=";
        }

        public EfDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Person> Persons { get; set; }
        
        public DbSet<Dog> Dogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}
