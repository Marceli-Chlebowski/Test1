namespace ExampleTest1.Models.DataModels;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime AdmissionDate { get; set; }
    public int OwnerId { get; set; }
    public Owner Owner { get; set; }
    public int AnimalClassId { get; set; }
    public string AnimalClass { get; set; }
    public ICollection<AnimalProcedure> AnimalProcedures { get; set; } = new List<AnimalProcedure>();
}

