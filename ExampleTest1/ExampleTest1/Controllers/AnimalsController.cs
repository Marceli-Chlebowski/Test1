using System.Transactions;
using ExampleTest1.Models.DTOs;
using ExampleTest1.Models.DataModels;
using ExampleTest1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleTest1.Controllers
{
    namespace kolokwium1.Controllers
    {
        namespace ExampleTest1.Controllers
        {
            [ApiController]
            [Route("api/animals")]
            public class AnimalsController : ControllerBase
            {
                private readonly IAnimalRepository _animalRepository;

                public AnimalsController(IAnimalRepository animalRepository)
                {
                    _animalRepository = animalRepository;
                }

                [HttpGet("{id}")]
                public async Task<IActionResult> GetAnimal(int id)
                {
                    if (!await _animalRepository.CheckAnimalExists(id))
                        return NotFound($"Animal with id = {id} doesn't exist!");

                    var animal = await _animalRepository.GetAnimal(id);
                    if (animal == null) return NotFound($"Animal with id = {id} not found!");

                    return Ok(animal);
                }

                [HttpPost]
                public async Task<IActionResult> CreateAnimal(NewAnimalDTO newAnimalDto)
                {
                    if (!await _animalRepository.CheckOwnerExists(newAnimalDto.OwnerId))
                        return NotFound($"Owner with id = {newAnimalDto.OwnerId} doesn't exist!");

                    if (!await _animalRepository.CheckAnimalClassExists(newAnimalDto.AnimalClass))
                        return NotFound($"Animal class = \"{newAnimalDto.AnimalClass}\" doesn't exist!");

                    foreach (var procedure in newAnimalDto.Procedures)
                    {
                        if (!await _animalRepository.CheckProcedureExists(procedure.ProcedureId))
                            return NotFound($"Procedure with id = {procedure.ProcedureId} doesn't exist!");
                    }

                    var animal = await _animalRepository.CreateAnimal(newAnimalDto);
                    return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
                }
            }
        }
    }
}