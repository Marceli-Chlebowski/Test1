using System.Collections;

namespace ExampleTest1.Models.DTOs;

public class NewAnimalDTO
{
    
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public int OwnerId { get; set; }
    public string AnimalClass { get; set; }
    public IEnumerable Procedures { get; set; }
}