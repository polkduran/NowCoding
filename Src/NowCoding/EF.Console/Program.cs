using EF.Model;
using EF.Model.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = "mydb.db";
            File.Delete(file);
            Func<SqliteConnectionStringBuilder> getCon = () => new SqliteConnectionStringBuilder { ["Filename"] = file };

            var con = getCon();
            con.Mode = SqliteOpenMode.ReadWriteCreate;
            using(var db = new EfDataContext(con.ConnectionString))
            {
                db.Database.Migrate();
            }

            var con1 = getCon();
            con1.Mode = SqliteOpenMode.ReadOnly;
            using (var db1 = new EfDataContext(con1.ConnectionString))
            {
                // db1.Database.BeginTransaction();
                var dogs = db1.Dogs.ToList();

                var con2 = getCon();
                con2.Mode = SqliteOpenMode.ReadWrite;

                using(var db2 = new EfDataContext(con2.ConnectionString))
                {
                    db2.Database.BeginTransaction();
                    var dog1 = new Dog { Name = "Minus" };
                    db2.Dogs.Add(dog1);
                    db2.SaveChanges();
                    db2.Database.CommitTransaction();
                }
            }
        }
    }
}
