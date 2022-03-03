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
            var result = -1;
            var sql = @"insert into pets.pet (name, category, status, tags, created, createdby)
                        values (@name, @category, @status, @tags, current_timestamp, 'PetStore.Pet.Api');
                        select currval('pets.pet_id_seq');";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    result = await _connection.ExecuteScalarAsync<int>(sql, pet);
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

        public async Task<int> InsertPetPhoto(Guid photoId, int petId, string metaData, string url)
        {
            var result = -1;
            var sql = @"insert into pets.photo (id, petid, url, metadata, created, createdby)
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
                }

                return result;
            }
        }

        public async Task<int> DeletePet(int petId)
        {
            var result = -1;
            var sql =     @"update pets.pet set
                            Deleted = current_timestamp,
                            DeletedBy = 'PetStore.Pet.Api',
                            IsDelete = true
                            where Id = @Id";

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
                    result = await _connection.ExecuteAsync(sql, pet);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await _connection.CloseAsync();
                }

                return result;
            }
        }

        public async Task<Pet> GetPet(int petId)
        {
            var result = new Pet();
            var sql = @"select p.Id, p.Name, p.Category, p.Status, p.Tags 
                        from pets.pet p
                        where p.Id = @Id
                        and p.IsDelete = false";

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
                }

                return result;
            }
        }

        public async Task<List<Pet>> GetPetByStatus(PetStatus status)
        {
            var result = new List<Pet>();
            var sql = @"select p.id, p.Name, p.Category, p.Status, p.Tags 
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
                }

                return result;
            }
        }

    }
}
