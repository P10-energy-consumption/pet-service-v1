using Dapper;
using pet_service_v1.Database.Interfaces;
using pet_service_v1.Models;
using System.Linq;
using pet_service_v1.Repositories.Interfaces;

namespace pet_service_v1.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public PetRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> InsertPet(Pet pet)
        {
            var sql = @" /* PetStore.Pet.Api */
insert into pets.pet (id, name, category, status, tags, created, createdby)
values (@id, @name, @category, @status, @tags, current_timestamp, 'PetStore.Pet.Api');";


            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    await _connection.ExecuteAsync(sql, pet);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();  
                }

                return pet.ID;
            }
        }

        public async Task<int> InsertPetPhoto(Guid photoId, int petId, string metaData, string url)
        {
            var result = -1;
            var sql = @" /* PetStore.Pet.Api */
insert into pets.photo (id, petid, url, metadata, created, createdby)
values (@id, @petid, @url, @metaData, current_timestamp, 'PetStore.Pet.Api')";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    result = await _connection.ExecuteAsync(sql, new { id = photoId, petId, metaData, url });
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                }

                return result;
            }
        }

        public async Task<int> DeletePet(int petId)
        {
            var result = -1;
            var sql = @" /* PetStore.Pet.Api */
delete from pets.pet where id = @Id"; ;

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    result = await _connection.ExecuteAsync(sql, new { id = petId });
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                }

                return result;
            }
        }

        public async Task<int> UpdatePet(Pet pet)
        {
            var result = -1;
            var sql = @" /* PetStore.Pet.Api */
update pets.pet set
Name = @Name,
Status = @Status,
Tags = @Tags,
Category = @Category,
Modified = current_timestamp,
ModifiedBy = 'PetStore.Pet.Api'
where Id = @Id";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync(); 

                try
                {
                    result = await _connection.ExecuteAsync(sql, new { name = pet.Name, status = pet.Status, tags = pet.Tags, category = pet.Category, id = pet.ID });
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                }

                return result;
            }
        }

        public async Task<Pet> GetPet(int petId)
        {
            var result = new Pet();
            var sql = @" /* PetStore.Pet.Api */
select p.Id, p.Name, p.Category, p.Status, p.Tags 
from pets.pet p
where p.Id = @Id";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    result = await _connection.QuerySingleAsync<Pet>(sql, new { id = petId });
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                }

                return result;
            }
        }

        public async Task<List<Pet>> GetPetByStatus(PetStatus status)
        {
            var result = new List<Pet>();
            var sql = @" /* PetStore.Pet.Api */
select p.id, p.Name, p.Category, p.Status, p.Tags 
from pets.pet p
where p.IsDelete = false
and p.status = @status";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    result = (await _connection.QueryAsync<Pet>(sql, new { status })).ToList();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                }

                return result;
            }
        }

    }
}
