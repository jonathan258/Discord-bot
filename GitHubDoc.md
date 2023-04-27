# Git hub bot

Done by : Jonathan Campbell

This is a Documentation of the github bot created on 2023-04-26

Program Class

The Program class is the entry point of the bot. It initializes and runs the bot by calling the RunBotAsync method and the code for this is below stating what each line does:

internal class Program

{

static void Main(string[] args) =\> new Program().RunBotAsync().GetAwaiter().GetResult();

private DiscordSocketClient \_client;

private CommandService \_commands;

private IServiceProvider \_services;

public async Task RunBotAsync()

{

// create the client and command service instances

\_client = new DiscordSocketClient();

\_commands = new CommandService();

// create the service provider to be used by the command service

\_services = new ServiceCollection()

.AddSingleton(\_client)

.AddSingleton(\_commands)

.BuildServiceProvider();

// get the bot token from an environment variable or configuration file

string token = "your-bot-token-goes-here";

// register the command module and start the bot

\_client.Log += \_client\_Log;

await RegisterCommandsAsync();

await \_client.LoginAsync(TokenType.Bot, token);

await \_client.StartAsync();

await Task.Delay(-1);

}

// handle log events from the client

private Task \_client\_Log(LogMessage arg)

{

Console.WriteLine(arg);

return Task.CompletedTask;

}

// register the Command module and set up a message handler

public async Task RegisterCommandsAsync()

{

await \_commands.AddModuleAsync\<Command\>(\_services);

\_client.MessageReceived += HandleCommandAsync;

}

// handle incoming messages and execute commands

private async Task HandleCommandAsync(SocketMessage arg)

{

var message = arg as SocketUserMessage;

var context = new SocketCommandContext(\_client, message);

if (message.Author.IsBot) return;

int argPos = 0;

if (message.HasStringPrefix("!", ref argPos))

{

var result = await \_commands.ExecuteAsync(context, argPos, \_services);

if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);

if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);

}

}

}

DiscordSocketClient

The DiscordSocketClient is the core class of the Discord.Net library that handles the WebSocket connection to the Discord API. It provides events for handling various types of events, such as message received, user joined, user left, etc.

In the Program class, the DiscordSocketClient is created and stored in the \_client field.

private DiscordSocketClient \_client;

public async Task RunBotAsync()

{

\_client = new DiscordSocketClient();

// ...

}

CommandService

The CommandService is a class in the Discord.Net library that provides a framework for defining and executing commands in a Discord bot. It handles parsing messages and executing commands defined in a module.

In the Program class, the CommandService is created and stored in the \_commands field.

private CommandService \_commands;

public async Task RunBotAsync()

{

// ...

\_commands = new CommandService();

// ...

}

Service Provider

The IServiceProvider interface is a part of the .NET Core Dependency Injection framework. It is used to provide instances of services to objects that need them.

In the Program class, the IServiceProvider is created and stored in the \_services field.

private IServiceProvider \_services;

public async Task RunBot