using GithubCommentBot.Models;
using GithubCommentBot.Store;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubCommentBot
{
    public class StoreImpl : IDisposable, IStore
    {
        public StoreImpl(ILogger<StoreImpl> logger)
        {
            _logger = logger;
            _botUsers = new Dictionary<string, BotUser>();
            _connection = new SqliteConnection("Data Source=DB/GithubBotDB.db");
            _connection.Open();
            ReadUsersFromDB();
        }

        public async Task AddUser(BotUser botUser)
        {
            if (await InsertUserIntoDB(botUser))
            {
                _botUsers.Add(botUser.GithubName, botUser);
                _logger.LogInformation($"Added new user (chatid = {botUser.ChatId}, githubName={botUser.GithubName})");
            }
        }

        public BotUser GetUser(string githubName)
        {
            return _botUsers[githubName];
        }

        public Boolean HaveUser(string githubName)
        {
            if (_botUsers.ContainsKey(githubName))
            {
                return true;
            }
            return false;
        }

        public Boolean IsRegistered(long chatId)
        {
            if(_botUsers.FirstOrDefault(_ => _.Value.ChatId == chatId).Value != null)
            {
                return true;
            }
            return false;
        }

        private void ReadUsersFromDB()
        {
            _logger.LogInformation("Start reading reading user from db");
            var readCommand = _connection.CreateCommand();
            readCommand.CommandText =
            $@"SELECT ChatId, TelegramName, GithubName FROM Users";
            var reader = readCommand.ExecuteReader();
            while (reader.Read())
            {
                var user = new BotUser();
                user.ChatId = long.Parse(reader["ChatId"].ToString());
                user.TelegramName = reader["TelegramName"].ToString();
                user.GithubName = reader["GithubName"].ToString();
                _logger.LogInformation($"Readed user {user.GithubName}");
                _botUsers.Add(user.GithubName, user);
            }
        }

        private async Task<Boolean> InsertUserIntoDB(BotUser botUser)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var insertCommand = _connection.CreateCommand();
                    insertCommand.CommandText =
                    $@"INSERT INTO Users (ChatId,TelegramName, GithubName) 
                       VALUES ({botUser.ChatId}, '{botUser.TelegramName}', '{botUser.GithubName}');)";
                    await insertCommand.ExecuteNonQueryAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail inserting uset to db");
                    return false;
                }
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection = null;
        }

        private readonly ILogger<StoreImpl> _logger;
        private readonly Dictionary<string, BotUser> _botUsers;
        private SqliteConnection _connection;
    }
}
