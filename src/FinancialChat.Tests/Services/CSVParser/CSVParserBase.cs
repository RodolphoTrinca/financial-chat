using Castle.Core.Logging;
using FinancialChat.Application.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Tests.Services.CSVParser
{
    public class CSVParserBase
    {
        protected ILogger<CSVParseService> _logger;
        protected CSVParseService _service;

        public CSVParserBase()
        {
            _logger = Substitute.For<ILogger<CSVParseService>>();
            _service = new CSVParseService(_logger);
        }
    }
}
