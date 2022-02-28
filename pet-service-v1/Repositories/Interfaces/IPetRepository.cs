using pet_service_v1.Models;

namespace pet_service_v1.Repositories.Interfaces
{
    public interface IPetRepository
    {
        Task<int> InsertPet(Pet pet);
        Task<int> UpdatePet(int petId, string name, PetStatus status);
        Task<int> InsertPetPhoto(Guid photoId, int petId, string metaData, string url);
        Task<int> DeletePet(int petId);
        Task<Pet> GetPet(int petId);
        Task<List<Pet>> GetPetByStatus(PetStatus status);
    }
}
