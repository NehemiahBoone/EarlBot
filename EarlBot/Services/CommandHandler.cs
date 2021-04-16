using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace EarlBot.Services
{
    public class CommandHandler
    {
        public static IServiceProvider _provider;
        public static DiscordSocketClient _discord;
        public static CommandService _commands;
        public static IConfigurationRoot _config;
        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _config = config;

            _discord.Ready += OnReady;
            _discord.MessageReceived += OnMessageReceived;
        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }
        private async Task OnMessageReceived(SocketMessage msgReceived)
        {
            try
            {
                var msg = msgReceived as SocketUserMessage;

                if (msg.Author.IsBot) { return; }

                SocketCommandContext context = new SocketCommandContext(_discord, msg);

                int pos = 0;
                if (msg.HasStringPrefix(_config["prefix"], ref pos) || msg.HasMentionPrefix(_discord.CurrentUser, ref pos))
                {
                    var result = await _commands.ExecuteAsync(context, pos, _provider);

                    if (!result.IsSuccess)
                    {
                        var reason = result.Error;

                        await context.Channel.SendFileAsync($"Error: \n {reason}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            
        }
    }
}
