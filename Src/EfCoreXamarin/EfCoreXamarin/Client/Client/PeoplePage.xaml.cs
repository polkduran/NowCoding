using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Xamarin.Forms;

namespace Client
{
    public partial class PeoplePage : ContentPage
    {
        public PeoplePage()
        {
            InitializeComponent();
            LoadPeope();
        }

        private async void AddPerson_OnClicked(object sender, EventArgs e)
        {
            var newPersonName = PersonNameEntry.Text;
            if (string.IsNullOrWhiteSpace(newPersonName))
            {
                SetInfoLabel("Name must not be empty.");
                return;
            }
            using (var db = new PetsDbContext())
            {
                var existingPerson = db.People.FirstOrDefault(x => x.Name == newPersonName);
                if (existingPerson != null)
                {
                    SetInfoLabel("A person with the same name already exists.");
                    return;
                }
                db.People.Add(new Person {Name = newPersonName});
                await db.SaveChangesAsync();
                LoadPeope();
            }  
        }

        private void SetInfoLabel(string message)
        {
            InfosLabel.Text = message;
        }

        private void LoadPeope()
        {
            using (var db = new PetsDbContext())
            {
                var people = db.People.ToList();
                PeopleListView.ItemsSource = people;
            }
        }
    }
}
