using GithubCommentBot.Models;
using GithubCommentBot.Store;
using LinqToDB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GithubCommentBot
{
    public class StoreImpl : IStore
    {
        public StoreImpl(ILogger<StoreImpl> logger)
        {
            _logger = logger;
            _botUsers = new Dictionary<string, BotUser>();
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
            if (_botUsers.FirstOrDefault(_ => _.Value.ChatId == chatId).Value != null)
            {
                return true;
            }
            return false;
        }

        private void ReadUsersFromDB()
        {
            _logger.LogInformation("Start reading reading user from db");
            using (var db = new GithubBotDB("GithubBotDB"))
            {
                foreach (var user in db.BotUsers)
                {
                    _botUsers.Add(user.GithubName, user);
                }
            }
        }

        private async Task<Boolean> InsertUserIntoDB(BotUser botUser)
        {
            using (var db = new GithubBotDB("GithubBotDB"))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        await db.InsertAsync(botUser);
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        _logger.LogError(e, "Fail inserting uset to db");
                        return false;
                    }
                }
            }
        }

        private readonly ILogger<StoreImpl> _logger;
        private readonly Dictionary<string, BotUser> _botUsers;
    }
}
