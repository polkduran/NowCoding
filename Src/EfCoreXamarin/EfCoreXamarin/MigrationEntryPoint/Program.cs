using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;

namespace MigrationEntryPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            PetsDbContext.DataBasePath = "Pets.db";
            Person owner;

            using (var db = new PetsDbContext())
            {
                db.Database.Migrate();
            }

            using (var db = new PetsDbContext())
            {
                owner = db.People.AsNoTracking().FirstOrDefault();
                if (owner == null)
                {
                    owner = new Person { Name = "Osi" };
                    db.People.Add(owner);
                    db.SaveChanges();
                }
            }

            using (var db = new PetsDbContext())
            {
                Animal beany = new Dog { Name = "Beany"};//, Age = 4, FurSoftness = "not so soft" };
                // me.Pets.Add(beany);
                beany.Owner = owner;

                db.People.Attach(owner);
                // db.People.Add(me);
                db.Set<Animal>().Add(beany);
                db.SaveChanges();
            }
            */
        }
    }
}
