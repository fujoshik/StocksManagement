namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
       
        public HttpClient GetAccountClient();
        public HttpClient GetSettlementClient();

    }
}
