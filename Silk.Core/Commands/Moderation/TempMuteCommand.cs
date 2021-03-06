﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Silk.Core.Database.Models;
using Silk.Core.Services;
using Silk.Core.Utilities;
using Silk.Extensions;

namespace Silk.Core.Commands.Moderation
{
    [Category(Categories.Mod)]
    public class TempMuteCommand : BaseCommandModule
    {
        private readonly ConfigService _dbService;
        public TempMuteCommand(ConfigService dbService) => _dbService = dbService;

        [Command("Mute")]
        [RequirePermissions(Permissions.ManageRoles)]
        public async Task TempMute(CommandContext ctx, DiscordMember user, TimeSpan duration, [RemainingText] string reason = "Not Given.")
        {
            DiscordMember bot = ctx.Guild.CurrentMember;
            if (user.IsAbove(bot))
            {
                await ctx.RespondAsync($"{user.Username} is {user.Roles.Last().Position - bot.Roles.Last().Position} roles above me!").ConfigureAwait(false);
                return;
            }
            GuildConfigModel config = (await _dbService.GetConfigAsync(ctx.Guild.Id))!;

            if (config.MuteRoleId is 0)
            {
                await ctx.RespondAsync("You haven't setup the mute role :(").ConfigureAwait(false);
                return;
            }


            await user.GrantRoleAsync(ctx.Guild.GetRole(config.MuteRoleId), reason);
        }




    }
}