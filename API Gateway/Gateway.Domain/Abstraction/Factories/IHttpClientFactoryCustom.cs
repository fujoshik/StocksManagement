using Gateway.Domain.Abstraction.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Factories
{
    public interface IHttpClientFactoryCustom
    {
        IAccountClient GetAccountClient();
        IStockClient GetStockClient();
        ISettlementsClient GetSettlementsClient();
        IAnalyzerClient GetAnalyzerClient();
    }
}
