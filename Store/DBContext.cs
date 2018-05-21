using GithubCommentBot.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubCommentBot.Store
{
    public class GithubBotContext : DbContext
    {
        public DbSet<BotUser> BotUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DB/GithubBotDB.db");
        }
    }
}
