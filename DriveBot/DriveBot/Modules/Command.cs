using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;


namespace DriveBot.Modules
{
    internal class Command : ModuleBase<SocketCommandContext>
    {
        [Command("green")]
        public async Task Green()
        {
            await ReplyAsync("Go!");
        }

        [Command("yellow")]
        public async Task Yellow()
        {
            await ReplyAsync("Slow Down!");
        }

        [Command("red")]
        public async Task Red()
        {
            await ReplyAsync("Stop!");
        }

     



    }
}
