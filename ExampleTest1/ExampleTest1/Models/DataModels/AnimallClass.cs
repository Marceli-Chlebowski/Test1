namespace ExampleTest1.Models.DataModels;

public class AnimalClass
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
}