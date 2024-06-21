namespace ExampleTest1.Models.DTOs
{
    public class AnimalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AnimalClass { get; set; }
        public DateTime AdmissionDate { get; set; }
        public OwnerDto Owner { get; set; }
        public List<ProcedureDto> Procedures { get; set; } = new List<ProcedureDto>();
    }
 
    public class OwnerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
 
    public class ProcedureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}