using Accounts.Domain.Abstraction.Repositories;
using Accounts.Infrastructure.Entities;
using Accounts.Infrastructure.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services.AppSettings;
using StockAPI.Infrastructure.Enums;
using StockAPI.Infrastructure.Models.PdfData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Document = iTextSharp.text.Document;

namespace StockAPI.Domain.Services
{
    public class PdfDataService:IPdfDataService
    {
        private readonly string _pdfFolderPath;
        private readonly string _pdfFileName;
        private readonly string _connectionString;
        private readonly IHttpClientFactory _httpClientFactory;

        public PdfDataService (IHttpClientFactory httpClientFactory, IOptions<PdfSettings> pdfSettings, IOptions<ConnectionStrings> connectionString)
        {
            _pdfFileName = pdfSettings.Value.PdfFileName;
            _pdfFolderPath = pdfSettings.Value.PdfFolderPath;
            _connectionString = connectionString.Value.StocksConnection;
            _httpClientFactory = httpClientFactory;
        }

        //generate pdf file
        public async Task GeneratePdf(PdfData pdfData)
        {
            try
            {
                string fileName = $"{_pdfFileName}{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string filePath = Path.Combine(_pdfFolderPath, fileName);

                using (var document = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create)))
                    {
                        document.Open();
                        pdfData.Title = "--Summary Of The Most Popular Stocks--";
                        document.Add(new Paragraph(pdfData.Title));
                        document.Add(new Paragraph(pdfData.Content));
                        document.Close();
                    }
                }
                Log.Information("pdf file was created successfully.");
            }
            catch (Exception ex)
            {
                Log.Error("an errror occured ");
            }
        }

        //get the most popular stock ticker
        public async Task<string> GetMostPopularStockTicker(string beginningDate, string endDate)
        {
            try
            {
                string mostPopularStockTicker = null;

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new SqlCommand("USE StocksDB;\r\nSELECT TOP 1 [StockTicker], " +
                        "COUNT([StockTicker]) AS TickerCount \r\nFROM Transactions \r\n" +
                        "WHERE [DateOfTransaction] BETWEEN @StartDate AND @EndDate \r\n" +
                        "GROUP BY [StockTicker] \r\n" +
                        "ORDER BY TickerCount DESC", connection))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", beginningDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                            {
                                mostPopularStockTicker = reader["StockTicker"].ToString();
                            }
                        }
                    }
                }
                Log.Information($"most popular ticker for the time period from '{beginningDate}'" +
                    $"to {endDate} is {mostPopularStockTicker}");
                return mostPopularStockTicker;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to calculate the most popular ticker.");
                throw;
            }
        }
    }
}
