using CsvHelper;
using CsvHelper.TypeConversion;
using FinancialChat.Application.Entities.StockData;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Services
{
    public class CSVParseService : ICSVParseService
    {
        private readonly ILogger<CSVParseService> _logger;

        public CSVParseService(ILogger<CSVParseService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<StockData> ParseCsv(byte[] data)
        {
            try
            {
                _logger.LogInformation("Parsing csv");
                _logger.LogDebug($"Creating memory stream, data size: {data.Length} bytes");
                using MemoryStream stream = new MemoryStream(data);
                
                _logger.LogDebug($"Creating Stream reader");
                using var reader = new StreamReader(stream);

                _logger.LogDebug($"Creating CSV reader");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                _logger.LogDebug($"Parsing Records");
                var records = csv.GetRecords<StockData>();
                return records.ToList();
            }
            catch (HeaderValidationException ex)
            {
                throw new ApplicationException("CSV file header is invalid.", ex);
            }
            catch (TypeConverterException ex)
            {
                throw new ApplicationException("CSV file contains invalid data format.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading CSV file", ex);
            }
        }
    }
}
