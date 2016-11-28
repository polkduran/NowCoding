using System.IO;
using Microsoft.EntityFrameworkCore;
using Model;
using Xamarin.Forms;

namespace Client
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var fileName = "Pets.db";
            var dbFullPath = Path.Combine(App.DbFolder, fileName);
            PetsDbContext.DataBasePath = dbFullPath;
            using (var db = new PetsDbContext())
            {
                db.Database.Migrate();
            }
        }
    }
}
