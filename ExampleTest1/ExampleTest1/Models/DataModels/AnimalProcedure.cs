namespace ExampleTest1.Models.DataModels

{
    public class AnimalProcedure
    {
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; }
        public DateTime Date { get; set; }
    }
}