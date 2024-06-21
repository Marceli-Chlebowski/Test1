namespace ExampleTest1.Models.DataModels

{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    }
}