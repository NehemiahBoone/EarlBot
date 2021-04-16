﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace EarlBot.Services
{
    public class StartupService
    {
        public static IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IConfigurationRoot config)
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _config = config;
        }

        public async Task StartAsync()
        {
            try
            {
                string token = _config["discord"];
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Provide discord token");
                    return;
                }

                await _discord.LoginAsync(TokenType.Bot, token);
                await _discord.StartAsync();

                await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
