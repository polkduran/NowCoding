namespace Model
{
    public class Dog : Animal
    {
        public string FurSoftness { get; set; }

        public override string Kind => "Dog";
    }
}
