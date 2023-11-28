using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IBlacklistService
    {
        bool IsEmailBlacklisted(string email);
    }

    public class BlacklistService : IBlacklistService
    {
        private readonly HashSet<string> _blacklistedEmails;

        public BlacklistService()
        {
            _blacklistedEmails = LoadBlacklistedEmailsFromConfig();
        }

        public bool IsEmailBlacklisted(string email)
        {
            return _blacklistedEmails.Contains(email.ToLowerInvariant());
        }

        private HashSet<string> LoadBlacklistedEmailsFromDatabase()
        {

            using (var dbContext = new YourDbContext())
            {
                var blacklistedEmails = dbContext.Blacklist.Select(b => b.Email).ToList();
                return new HashSet<string>(blacklistedEmails);
            }
           
        }
        private HashSet<string> LoadBlacklistedEmailsFromConfig()
        {

            var blacklistedEmails = ConfigurationManager.AppSettings["BlacklistedEmails"];
            var emailList = blacklistedEmails?.Split(',').Select(e => e.Trim()).ToList() ?? new List<string>();
            return new HashSet<string>(emailList);
        }
        private HashSet<string> LoadBlacklistedEmails()
        {
            
            return new HashSet<string>
        {
            "blocked@example.com",
            "spam@example.com"
        };
        }

}
