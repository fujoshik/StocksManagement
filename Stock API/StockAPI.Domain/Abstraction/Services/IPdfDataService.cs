using StockAPI.Infrastructure.Models.PdfData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Abstraction.Services
{
    public interface IPdfDataService
    {
        Task GeneratePdf(PdfData pdfData);
        Task<string> GetMostPopularStockTicker(string beginningDate, string endDate);

    }
}
