using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FinancialChat.Infra.Context;

namespace OpenBanking.Infra.Context
{
    public class FinancialChatContextFactory : IDesignTimeDbContextFactory<FinancialChatContext>
    {
        public FinancialChatContext CreateDbContext(string[] args)
        {
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FinancialChat.API"))
                .AddJsonFile("appsettings.local.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FinancialChatContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            return new FinancialChatContext(optionsBuilder.Options);
        }
    }
}
