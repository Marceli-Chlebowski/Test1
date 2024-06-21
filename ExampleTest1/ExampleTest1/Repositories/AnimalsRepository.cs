using Dapper;
using ExampleTest1.Models.DTOs;
using ExampleTest1.Repositories;
using ExampleTest1.Models.DTOs;
using ExampleTest1.Models.DataModels;
using ExampleTest1.Models.DataModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
 
namespace ExampleTest1.Repositories
{
   public class AnimalRepository : IAnimalRepository
    {
        private readonly IConfiguration _config;
 
        public AnimalRepository(IConfiguration config)
        {
            _config = config;
        }
 
        public async Task<bool> CheckAnimalExists(int id)
        {
            const string sql = "SELECT 1 FROM animals WHERE Id = @Id";
            await using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }
 
        public async Task<AnimalDto> GetAnimal(int id)
        {
            const string animalSql = @"SELECT a.Id, a.Name, a.AnimalClass, a.AdmissionDate, 
                                            o.Id, o.FirstName, o.LastName 
                                       FROM animals a 
                                       JOIN owners o ON a.OwnerId = o.Id 
                                       WHERE a.Id = @Id";
 
            const string procedureSql = @"SELECT p.Id, p.Name, p.Description, ap.Date 
                                          FROM procedures p 
                                          JOIN animal_procedures ap ON p.Id = ap.ProcedureId 
                                          WHERE ap.AnimalId = @AnimalId";
 
            await using var connection = new SqlConnection(_config.GetConnectionString("Default"));
 
            var animalData = await connection.QueryAsync<Animal, Owner, Animal>(
                animalSql,
                (animal, owner) =>
                {
                    animal.Owner = owner;
                    return animal;
                },
                new { Id = id }
            );
 
            var animal = animalData.FirstOrDefault();
            if (animal == null) return null;
 
            var procedures = await connection.QueryAsync<ProcedureDto>(procedureSql, new { AnimalId = id });
 
            return new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name,
                AnimalClass = animal.AnimalClass,
                AdmissionDate = animal.AdmissionDate,
                Owner = new OwnerDto
                {
                    Id = animal.Owner.Id,
                    FirstName = animal.Owner.FirstName,
                    LastName = animal.Owner.LastName
                },
                Procedures = procedures.ToList()
            };
        }
 
        public async Task<bool> CheckOwnerExists(int id)
        {
            const string sql = "SELECT 1 FROM owners WHERE Id = @Id";
            await using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }
 
        public async Task<bool> CheckAnimalClassExists(string animalClass)
        {
            const string sql = "SELECT 1 FROM animal_classes WHERE Name = @AnimalClass";
            await using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            return await connection.ExecuteScalarAsync<bool>(sql, new { AnimalClass = animalClass });
        }
 
        public async Task<bool> CheckProcedureExists(int id)
        {
            const string sql = "SELECT 1 FROM procedures WHERE Id = @Id";
            await using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }
 
        public async Task<AnimalDto> CreateAnimal(NewAnimalDTO newAnimalDto)
        {
            const string insertAnimalSql = "INSERT INTO animals (Name, AnimalClass, AdmissionDate, OwnerId) OUTPUT INSERTED.Id VALUES (@Name, @AnimalClass, @AdmissionDate, @OwnerId)";
            const string insertAnimalProcedureSql = "INSERT INTO animal_procedures (AnimalId, ProcedureId, Date) VALUES (@AnimalId, @ProcedureId, @Date)";
 
            await using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
 
            try
            {
                var animalId = await connection.ExecuteScalarAsync<int>(insertAnimalSql, new
                {
                    newAnimalDto.Name,
                    newAnimalDto.AnimalClass,
                    newAnimalDto.AdmissionDate,
                    newAnimalDto.OwnerId
                }, transaction);
 
                foreach (var procedure in newAnimalDto.Procedures)
                {
                    await connection.ExecuteAsync(insertAnimalProcedureSql, new
                    {
                        AnimalId = animalId,
                        ProcedureId = procedure.ProcedureId,
                        Date = procedure.Date
                    }, transaction);
                }
 
                await transaction.CommitAsync();
 
                return await GetAnimal(animalId);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}