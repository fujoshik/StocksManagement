﻿namespace Accounts.Infrastructure.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public Guid AccountId { get; set; }
    }
}
