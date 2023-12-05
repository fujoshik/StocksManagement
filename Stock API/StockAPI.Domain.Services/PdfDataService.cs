using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services.AppSettings;
using StockAPI.Infrastructure.Models.PdfData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Document = iTextSharp.text.Document;

namespace StockAPI.Domain.Services
{
    public class PdfDataService:IPdfDataService
    {
        private readonly string _pdfFolderPath;
        private readonly string _pdfFileName;
        private readonly IHttpClientFactory _httpClientFactory;

        public PdfDataService (IHttpClientFactory httpClientFactory, IOptions<PdfSettings> pdfSettings)
        {
            _pdfFileName = pdfSettings.Value.PdfFileName;
            _pdfFolderPath = pdfSettings.Value.PdfFolderPath;
            _httpClientFactory = httpClientFactory;
        }

        public void GeneratePdf(PdfData pdfData)
        {
            string fileName = $"{_pdfFileName}{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string filePath = Path.Combine(_pdfFolderPath, fileName);

            using (var document = new Document())
            {
                using (var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create)))
                {
                    document.Open();

                    document.Add(new Paragraph(pdfData.Title));
                    document.Add(new Paragraph(pdfData.Content));
                    document.Close();
                }
            }
        }
    }
}
