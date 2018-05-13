using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubCommentBot.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace GithubCommentBot.Bot
{
    public class GithubBot : IGithubBot
    {
        public GithubBot()
        {
            var apiToken = Environment.GetEnvironmentVariable("API_TOKEN");
            _githubToTelegramUsers = new Dictionary<string, string>();
            _prUsers = new Dictionary<long, List<string>>();
            _telegramClient = new TelegramBotClient(apiToken);
            _telegramUsersWithInvite = new List<string>();
        }

        public async void Start()
        {
            var me = await _telegramClient.GetMeAsync();
            Console.WriteLine($"Hello! My name is {me.FirstName}");
            _telegramClient.StartReceiving();
            _telegramClient.OnMessage += BotOnMessageReceived;
        }

        public void Stop()
        {
            _telegramClient.StopReceiving();
            _telegramClient.OnMessage -= BotOnMessageReceived;
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"{e.Message.Text}");
            if (e.Message.Text == "/start")
            {
                if (!_githubToTelegramUsers.ContainsValue(e.Message.Chat.Username))
                {
                    _telegramUsersWithInvite.Add(e.Message.Chat.Username);
                    await _telegramClient.SendTextMessageAsync(e.Message.Chat.Id, "What is you name in github?");
                }
                else
                {
                    await _telegramClient.SendTextMessageAsync(e.Message.Chat.Id, "You already registered");
                }
            }
            else
            {
                if (_telegramUsersWithInvite.Contains(e.Message.Chat.Username))
                {
                    _githubToTelegramUsers.Add(e.Message.Text, e.Message.Chat.Username);
                    await _telegramClient.SendTextMessageAsync(e.Message.Chat.Id, $"You registered with githubname: {e.Message.Text}");
                }
            }
        }

        public async Task AddHook(PrWebHook comment)
        {
            List<string> users;
            if (_prUsers.ContainsKey(comment.PullRequest.Id))
            {
                users = _prUsers[comment.PullRequest.Id];
            }
            else
            {
                users = new List<string>();
                _prUsers.Add(comment.PullRequest.Id, users);
            }

            if (!users.Contains(comment.PullRequest.User.Login))
            {
                users.Add(comment.PullRequest.User.Login);
            }

            if (!users.Contains(comment.Comment.User.Login))
            {
                users.Add(comment.PullRequest.User.Login);
            }

            var telegramUsers = users
                .Where(_ => _githubToTelegramUsers.ContainsKey(_))
                .Select(_ => _githubToTelegramUsers[_])
                .ToArray();

            foreach (var telegramUser in telegramUsers)
            {
                var message = $"{comment.PullRequest.Body}\r\n{comment.Comment.User.Login} : {comment.Comment.Body}";
                await _telegramClient.SendTextMessageAsync(new ChatId(telegramUser), message);
            }
        }

        private readonly List<string> _telegramUsersWithInvite;
        private readonly Dictionary<string, string> _githubToTelegramUsers;
        private readonly Dictionary<long, List<string>> _prUsers;
        private readonly TelegramBotClient _telegramClient;
    }
}