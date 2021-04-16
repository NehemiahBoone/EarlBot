﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlBot.Modules
{
    public class GeneralModule : ModuleBase
    {
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
        }

        [Command("info")]
        public async Task Userinfo(SocketGuildUser user = null)
        {
            if(user == null)
            {
                var builder = new EmbedBuilder()
                    .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                    .WithColor(new Color(33, 176, 252))
                    .AddField("Id", Context.User.Discriminator)
                    .AddField("Created at", Context.User.CreatedAt.ToString("MMMM dd, yyyy"))
                    .WithCurrentTimestamp();
                var embed = builder.Build();
                await Context.Channel.SendMessageAsync(null, false, embed);
            }
            else
            {
                var builder = new EmbedBuilder()
                    .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .WithColor(new Color(33, 176, 252))
                    .AddField("Id", user.Discriminator)
                    .AddField("Created at", user.CreatedAt.ToString("MMMM dd, yyyy"))
                    .WithCurrentTimestamp();
                var embed = builder.Build();
                await Context.Channel.SendMessageAsync(null, false, embed);
            }
        }

        

        [Command("server")]
        public async Task Server()
        {
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithTitle($"{Context.Guild.Name} (server info)")
                .WithColor(new Color(33, 176, 252))
                .AddField("Created at", Context.Guild.CreatedAt.ToString("MMMM dd, yyyy"))
                .AddField("Member Count: ", (Context.Guild as SocketGuild).MemberCount)
                .AddField("Online Users: ", (Context.Guild as SocketGuild).Users.Where(m => m.Status != UserStatus.Offline).Count());
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}
