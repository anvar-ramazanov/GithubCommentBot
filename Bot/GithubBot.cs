using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubCommentBot.Models;
using GithubCommentBot.Store;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace GithubCommentBot.Bot
{
    public class GithubBot : IGithubBot
    {
        public GithubBot(ILogger<GithubBot> logger, IStore store)
        {
            _logger = logger;
            var apiToken = Environment.GetEnvironmentVariable("API_TOKEN");
            _logger.LogInformation($"TelegramBot api: {apiToken}");
            _prUsers = new Dictionary<long, List<string>>();
            _telegramClient = new TelegramBotClient(apiToken);
            _telegramUsersWithInvite = new List<string>();
            _store = store;
        }

        public async void Start()
        {
            var me = await _telegramClient.GetMeAsync();
            _logger.LogInformation($"Hello! My name is {me.FirstName}");
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
            _logger.LogInformation($"{e.Message.Text}");
            if (e.Message.Text == "/start")
            {
                if (!_store.IsRegistered(e.Message.Chat.Id))
                {
                    _telegramUsersWithInvite.Add(e.Message.Chat.Username);
                    await SendMessage(e.Message.Chat.Id, "What is you name in github?");
                }
                else
                {
                    await SendMessage(e.Message.Chat.Id, "You already registered");
                }
            }
            else
            {
                if (_telegramUsersWithInvite.Contains(e.Message.Chat.Username))
                {
                    var githubName = e.Message.Text;
                    await _store.AddUser(new BotUser()
                    {
                        GithubName = githubName,
                        ChatId = e.Message.Chat.Id,
                        TelegramName = e.Message.Chat.Username,
                    });
                    await SendMessage(e.Message.Chat.Id, $"You registered with githubname: {e.Message.Text}");
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

            var telegramChatIds = users
                .Where(_ => _store.HaveUser(_))
                .Select(_ => _store.GetUser(_).ChatId)
                .Distinct()
                .ToArray();

            foreach (var telegramChatId in telegramChatIds)
            {
                var userName = _store.HaveUser(comment.Comment.User.Login)
                    ? _store.GetUser(comment.Comment.User.Login).TelegramName
                    : comment.Comment.User.Login;

                var actionString = comment.Action == "created"
                    ? "Added new coment"
                    : "Edit comment";

                var message = $"{actionString}\r\nRepo: {comment.Repository?.Name}\r\nPR: {comment.PullRequest.Title}\r\n{userName}: {comment.Comment.Body}\r\n{comment.Comment.Links.Self}";
                await SendMessage(telegramChatId, message);
            }
        }

        private async Task SendMessage(long chatId, string text)
        {
            await _telegramClient.SendTextMessageAsync(chatId, text);
            _logger.LogInformation($"Send message to '{chatId}' Text = '{text}'");
        }

        private readonly List<string> _telegramUsersWithInvite;
        private readonly IStore _store;
        private readonly Dictionary<long, List<string>> _prUsers;
        private readonly TelegramBotClient _telegramClient;
        private ILogger<GithubBot> _logger;
    }
}