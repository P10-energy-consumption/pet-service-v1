using Microsoft.AspNetCore.Mvc;
using pet_service_v1.Models;
using pet_service_v1.Repositories.Interfaces;

namespace pet_service_v1.Controllers
{
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetRepository _petRepository;

        public PetController(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        [HttpPost("/v1/pet")]
        public async Task<IActionResult> CreatePet([FromBody] Pet pet)
        {
            var result = await _petRepository.InsertPet(pet);
            return Ok(result);
        }

        [HttpPut("/v1/pet")]
        public async Task<IActionResult> UpdatePet([FromBody] Pet pet)
        {
            var result = await _petRepository.UpdatePet(pet);
            return Ok(result);
        }

        [HttpPost("/v1/pet/{petId}/uploadImage")]
        public async Task<IActionResult> InsertPetPhoto([FromBody] PetPhoto photo)
        {
            var photoId = Guid.NewGuid();
            var filePath = "/some/path" + photoId;
            var fileUrl = "/some/url/" + photoId;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.File.CopyTo(fileStream);
            }

            var result = await _petRepository.InsertPetPhoto(photoId, photo.PetID, photo.MetaData, fileUrl);
            return Ok(result);
        }

        [HttpDelete("/v1/pet/{petId}")]
        public async Task<IActionResult> DeletePet(int petId)
        {
            var result = await _petRepository.DeletePet(petId);
            return Ok(result);
        }

        [HttpGet("/v1/pet/{petId}")]
        public async Task<IActionResult> GetPet(int petId)
        {
            var result = await _petRepository.GetPet(petId);
            return Ok(result);
        }

        [HttpGet("/v1/pet/findByStatus")]
        public async Task<IActionResult> GetPetsByStatus([FromQuery] PetStatus status)
        {
            var result = await _petRepository.GetPetByStatus(status);
            return Ok(result);
        }
    }
}