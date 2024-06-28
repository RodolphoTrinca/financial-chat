using FinancialChat.Application.Entities.Chat;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Infra.Context
{
    public class MessagesDbContext : DbContext
    {
        public DbSet<MessagesData> Messages { get; init; }

        public MessagesDbContext(DbContextOptions<MessagesDbContext> options) : base(options) { }
    }
}
