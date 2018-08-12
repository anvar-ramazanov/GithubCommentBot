using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GithubCommentBot.Dto;
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

        public async Task AddCommentHook(PrCommentWebHook comment)
        {
            var users = GetUsersForPr(comment.PullRequest.Id);

            if (!users.Contains(comment.PullRequest.User.Login))
            {
                _logger.LogInformation($"Add {comment.PullRequest.User.Login} to receivers for this pr");
                users.Add(comment.PullRequest.User.Login);
            }

            if (!users.Contains(comment.Comment.User.Login))
            {
                _logger.LogInformation($"Add {comment.Comment.User.Login} to receivers for this pr");
                users.Add(comment.Comment.User.Login);
            }

            var means = GetMeansUsers(comment.Comment.Body);
            _logger.LogInformation($"Founded next direct call: {string.Join(',', means)}");
            foreach (var mean in means)
            {
                if (!users.Contains(mean))
                {
                    _logger.LogInformation($"Add {mean} to receivers for this pr");
                    users.Add(mean);
                }
            }

            _logger.LogInformation($"Start founding users: {string.Join(',', users)}");

            var telegramChatIds = users
                .Where(_ => _store.HaveUser(_) && _ != comment.Comment.User.Login)
                .Select(_ => _store.GetUser(_).ChatId)
                .Distinct()
                .ToArray();

            foreach (var telegramChatId in telegramChatIds)
            {
                var userName = _store.HaveUser(comment.Comment.User.Login)
                    ? $"@{_store.GetUser(comment.Comment.User.Login).TelegramName}"
                    : comment.Comment.User.Login;

                var actionString = comment.Action == "created"
                    ? "Added new coment"
                    : "Edit comment";

                var message = $"{actionString}\r\nRepo: {comment.Repository?.Name}\r\nPR: {comment.PullRequest.Title}\r\n{userName}: {comment.Comment.Body}\r\n{comment.Comment.HtmlUrl}";
                await SendMessage(telegramChatId, message);
            }
        }

        private List<string> GetUsersForPr(long prId)
        {
            if (_prUsers.ContainsKey(prId))
            {
                return _prUsers[prId];
            }
            else
            {
                _prUsers.Add(prId, new List<string>());
                return _prUsers[prId];
            }
        }

        public async Task AddApproveHook(PrWebHook prWebHook)
        {
            var user = prWebHook?.PullRequest?.User?.Login;
            if (!string.IsNullOrEmpty(user))
            {
                var telegramChatId = _store.HaveUser(user)
                    ? _store.GetUser(user).ChatId
                    : 0;

                if (telegramChatId != 0)
                {
                    var aprover = _store.HaveUser(prWebHook.Review.User.Login)
                        ? $"@{ _store.GetUser(prWebHook.Review.User.Login).TelegramName}"
                        : prWebHook.Review.User.Login;

                    var message = $"Pull request approved  by {aprover}\r\nRepo: {prWebHook.Repository?.Name}\r\nPR: {prWebHook.PullRequest?.Title}r\n{prWebHook.PullRequest.Links.Html.Href}";
                    await SendMessage(telegramChatId, message);
                }
            }
        }

        public async Task AddRejectHook(PrWebHook prWebHook)
        {
            var user = prWebHook?.PullRequest?.User?.Login;
            if (!string.IsNullOrEmpty(user))
            {
                var telegramChatId = _store.HaveUser(user)
                    ? _store.GetUser(user).ChatId
                    : 0;

                if (telegramChatId != 0)
                {
                    var aprover = _store.HaveUser(prWebHook.Review.User.Login)
                        ? $"@{ _store.GetUser(prWebHook.Review.User.Login).TelegramName}"
                        : prWebHook.Review.User.Login;

                    var message = $"Pull request rejected by {aprover}\r\nRepo: {prWebHook.Repository?.Name}\r\nPR: {prWebHook.PullRequest?.Title}r\n{prWebHook.PullRequest.Links.Html.Href}";
                    await SendMessage(telegramChatId, message);
                }
            }
        }

        private List<String> GetMeansUsers(string content)
        {
            var users = new List<String>();
            var rgx = new Regex("@\\S+");
            foreach (Match match in rgx.Matches(content))
            {
                users.Add(match.Value.Replace("@", string.Empty));
            }
            return users;
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