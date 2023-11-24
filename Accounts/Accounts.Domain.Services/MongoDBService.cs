using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.MongoDB;
using Accounts.Domain.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Accounts.Domain.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<UserDto> _userCollection;

        public MongoDBService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionURI);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _userCollection = database.GetCollection<UserDto>(settings.Value.CollectionName);
        }

        public async Task CreateUserAsync(UserDto user)
        {
            await _userCollection.InsertOneAsync(user);
        }

        public async Task<UserDto> GetUserByCodeAsync(string code)
        {
            var users = await _userCollection.FindAsync(x => x.VerificationCode == code);

            return users.FirstOrDefault();
        }
    }
}
