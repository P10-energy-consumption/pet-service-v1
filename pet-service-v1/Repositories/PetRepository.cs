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
            var sql = @"insert into pets.pet (name, category, status, tags, created, createdby)
                        values (@name, @category, @status, @tags, current_timestamp, 'PetStore.Pet.Api');
                        select currval('pets.pet_id_seq');";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = _connection.QuerySingleAsync<int>(sql, pet);
                    return await result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task<int> InsertPetPhoto(Guid photoId, int petId, string metaData, string url)
        {
            var sql = @"insert into pets.photo (id, petid, url, metadata, created, createdby)
                        values (@id, @petid, @url, @metaData, current_timestamp, 'PetStore.Pet.Api')";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = _connection.QuerySingleAsync<int>(sql, new { id = photoId, petId, metaData, url });
                    return await result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task<int> DeletePet(int petId)
        {
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
                    var result = _connection.QuerySingleAsync<int>(sql, new { id = petId });
                    return await result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task<int> UpdatePet(int petId, string name, PetStatus status)
        {
            var sql = @"update pets.pet set
                        Name = @Name,
                        Status = @Status,
                        Modified = current_timestamp,
                        ModifiedBy = 'PetStore.Pet.Api'
                        where Id = @Id";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = _connection.QuerySingleAsync<int>(sql, new { name, status, id = petId });
                    return await result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task<Pet> GetPet(int petId)
        {
            var sql = @"select p.Id, p.Name, p.Category, p.Status, p.Tags 
                        from pets.pet p
                        where p.Id = @Id
                        and p.IsDelete = false";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = _connection.QuerySingleAsync<Pet>(sql, new {id = petId });
                    return await result;
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public async Task<List<Pet>> GetPetByStatus(PetStatus status)
        {
            var sql = @"select p.id, p.Name, p.Category, p.Status, p.Tags 
                        from pets.pet p
                        where p.IsDelete = false
                        and p.status = @status";

            using (var _connection = _connectionFactory.CreateDBConnection())
            {
                await _connection.OpenAsync();

                try
                {
                    var result = await _connection.QueryAsync<Pet>(sql, new { status });
                    return result.ToList();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

    }
}
