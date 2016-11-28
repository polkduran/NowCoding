namespace Model
{
    public abstract class Animal
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public abstract string Kind { get; }

        public Person Owner { get; set; }
    }
}
