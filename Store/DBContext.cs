using GithubCommentBot.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GithubCommentBot.Store
{
    public class GithubBotContext : DbContext
    {
        public DbSet<BotUser> BotUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            optionsBuilder.UseSqlite($"Filename={Path.Combine(currentPath, "DB", "GithubBotDB.db")}");
        }
    }
}
