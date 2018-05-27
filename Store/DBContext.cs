using GithubCommentBot.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;

namespace GithubCommentBot.Store
{
    public class GithubBotContext : DbContext
    {
        public DbSet<BotUser> BotUsers { get; set; }

        public GithubBotContext(ILogger<GithubBotContext> logger)
        {
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dbFilePath = Path.Combine(currentPath, "DB", "GithubBotDB.db");
            _logger.LogInformation($"DB file exist: {File.Exists(dbFilePath)}");
            var builder = new SqliteConnectionStringBuilder();
            builder.DataSource = dbFilePath;
            builder.Mode = SqliteOpenMode.ReadWrite;
            _logger.LogInformation($"ConnectionString: {builder.ConnectionString}");
            optionsBuilder.UseSqlite(builder.ConnectionString);
        }

        private readonly ILogger<GithubBotContext> _logger;
    }
}
