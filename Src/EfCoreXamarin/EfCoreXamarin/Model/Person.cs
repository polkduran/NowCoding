using System.Collections.Generic;

namespace Model
{
    public class Person
    {
        public Person()
        {
            Pets = new List<Animal>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Animal> Pets { get; set; }
    }
}
