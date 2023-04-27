using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriveBot.Modules;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace DriveBot
{
    internal class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        { // create the client and command service instances
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            // create the service provider to be used by the command service
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            // get the bot token from an environment variable or configuration file
            string token = "BOT-token";
            // register the command module and start the bot
            _client.Log += _client_Log;
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
        // handle log events from the client
        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        // register the Command module and set up a message handler
        public async Task RegisterCommandsAsync()
        {  await _commands.AddModuleAsync<Command>(_services);
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        }
        // handle incoming messages and execute commands
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }

}

