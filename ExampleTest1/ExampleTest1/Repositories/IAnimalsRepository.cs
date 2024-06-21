using ExampleTest1.Models.DataModels;
using ExampleTest1.Models.DTOs;

namespace ExampleTest1.Repositories;

public interface IAnimalRepository
{
    Task<bool> CheckAnimalExists(int animalId);
    Task<AnimalDto> GetAnimal(int animalId);
    Task<bool> CheckOwnerExists(int ownerId);
    Task<bool> CheckAnimalClassExists(string animalClass);
    Task<bool> CheckProcedureExists(int procedureId);
    Task<AnimalDto> CreateAnimal(NewAnimalDTO animalDetails);
}
