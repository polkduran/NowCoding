using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Xamarin.Forms;

namespace Client
{
    public partial class PetsPage : ContentPage
    {
        public PetsPage()
        {
            InitializeComponent();
            AnimalKindPicker.Items.Add("Dog");
            AnimalKindPicker.Items.Add("Platypus");
            LoadPets();
        }

        private async void AddPet_OnClicked(object sender, EventArgs e)
        {
            var newPetName = PetNameEntry.Text;
            if (string.IsNullOrWhiteSpace(newPetName))
            {
                SetInfoLabel("Name must not be empty.");
                return;
            }
            var ownerIndex = OwnerPicker.SelectedIndex;
            if (ownerIndex < 0)
            {
                SetInfoLabel("Select an owner.");
                return;
            }
            var owner = _people[ownerIndex];
            Animal animal;
            if (AnimalKindPicker.SelectedIndex == 0)
            {
                animal = new Dog();
            }
            else
            {
                animal = new Platypus();
            }
            animal.Name = newPetName;
            animal.Owner = owner;
            using (var db = new PetsDbContext())
            {
                db.People.Attach(owner);
                db.Set<Animal>().Add(animal);
                await db.SaveChangesAsync();
            }
            LoadPets();
        }


        private void SetInfoLabel(string message)
        {
            InfosLabel.Text = message;
        }

        private List<Person> _people;
        private void LoadPets()
        {
            using (var db = new PetsDbContext())
            {
                var pets = db.Set<Animal>().ToList();
                PetListView.ItemsSource = pets;

                _people = db.People.ToList();
                OwnerPicker.Items.Clear();
                foreach (var person in _people)
                {
                    OwnerPicker.Items.Add(person.Name);
                }

            }
        }
    }
}
