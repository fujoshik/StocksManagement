using Analyzer.API.Analyzer.Domain.DTOs;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IService
    {
        Task<UserData> GetInfoFromAccount(int id);
        //Task<string> GetUserById(int id);
        Task<UserData> GetInfoFromSettlement(string id);
    }
}
