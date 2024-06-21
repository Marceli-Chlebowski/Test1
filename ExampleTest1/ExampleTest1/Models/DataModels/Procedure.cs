namespace ExampleTest1.Models.DataModels

{
    public class Procedure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AnimalProcedure> AnimalProcedures { get; set; } = new List<AnimalProcedure>();
    }
}