using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FinancialChat.Infra.Context;

namespace OpenBanking.Infra.Context
{
    public class MessagesDbContextFactory : IDesignTimeDbContextFactory<MessagesDbContext>
    {
        public MessagesDbContext CreateDbContext(string[] args)
        {
            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FinancialChat.API"))
                .AddJsonFile("appsettings.local.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MessagesDbContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            return new MessagesDbContext(optionsBuilder.Options);
        }
    }
}
