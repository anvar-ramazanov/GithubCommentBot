using GithubCommentBot.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubCommentBot.Store
{
    public class GithubBotContext : DbContext
    {
        public DbSet<BotUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DB/GithubBotDB.db");
        }
    }
}
