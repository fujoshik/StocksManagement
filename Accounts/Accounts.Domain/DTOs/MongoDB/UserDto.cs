using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Accounts.Domain.DTOs.MongoDB
{
    public class UserDto
    {
        [BsonId]
        [JsonProperty("verification-code")]
        public string VerificationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal Sum { get; set; }
        public int? CurrencyCode { get; set; }
    }
}
