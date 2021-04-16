using Discord.Commands;
using Discord.WebSocket;
using EarlBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EarlBot
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("_configYAML.yml");
            Configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {
            try
            {
                var startup = new Startup(args);
                await startup.RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        public async Task RunAsync()
        {
            try
            {
                var services = new ServiceCollection();
                ConfigureServices(services);

                var provider = services.BuildServiceProvider();
                provider.GetRequiredService<CommandHandler>();

                await provider.GetRequiredService<StartupService>().StartAsync();
                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
             {
                LogLevel = Discord.LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false
             }))
            .AddSingleton<CommandHandler>()
            .AddSingleton<StartupService>()
            .AddSingleton(Configuration);
        }

    }
}
