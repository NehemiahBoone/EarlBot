using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlBot.Modules
{
    public class ModModule : ModuleBase
    {
        [Command("purge")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int amountToPurge)
        {
            try
            {
                var messages = await Context.Channel.GetMessagesAsync(amountToPurge + 1).FlattenAsync();
                await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

                var message = await Context.Channel.SendMessageAsync($"{messages.Count()} messages deleted.");

                await Task.Delay(5000);
                await message.DeleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
