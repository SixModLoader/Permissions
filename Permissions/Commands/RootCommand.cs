using System;
using CommandSystem;
using HarmonyLib;
using SixModLoader.Api.Extensions;

namespace Permissions.Commands
{
    [AutoCommandHandler(typeof(RemoteAdminCommandHandler))]
    public class RootCommand : ParentCommand
    {
        public RootCommand()
        {
            LoadGeneratedCommands();
        }

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new PermissionHolderCommand<User>(() => PermissionsMod.Instance.Storage.Users));
            RegisterCommand(new PermissionHolderCommand<Group>(() => PermissionsMod.Instance.Storage.Groups));
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"Please specify a valid subcommand! ({Commands.Keys.Join()})";
            return false;
        }

        public override string Command => "permissions";
        public override string[] Aliases => new[] { "p" };
        public override string Description => "Manage permissions";
    }
}