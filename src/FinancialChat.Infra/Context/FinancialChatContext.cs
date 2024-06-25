using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Infra.Context
{
    public class FinancialChatContext : IdentityDbContext<IdentityUser>
    {

        public FinancialChatContext(DbContextOptions<FinancialChatContext> options) : base(options)
        {
        }
    }
}