using System;
using CommandSystem;
using HarmonyLib;

namespace Permissions.Commands
{
    public class PermissionsCommand<T> : ParentCommand where T : PermissionHolder
    {
        public PermissionsCommand(PermissionHolderCommand<T> parent)
        {
            Parent = parent;
            LoadGeneratedCommands();
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"Please specify a valid subcommand! ({Commands.Keys.Join()})";
            return false;
        }

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new AddCommand(Parent));
            RegisterCommand(new RemoveCommand(Parent));
        }

        public override string Command => "permissions";
        public override string[] Aliases => new string[0];
        public override string Description => "Manages permissions";
        public PermissionHolderCommand<T> Parent { get; }

        public class AddCommand : ICommand
        {
            public AddCommand(PermissionHolderCommand<T> parent)
            {
                Parent = parent;
            }

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                var id = arguments.At(0);
                var permission = arguments.At(1);

                if (Parent.Dictionary.TryGetValue(id, out var permissionHolder))
                {
                    if (permissionHolder.Permissions.Contains(permission))
                    {
                        response = $"{id} already has {permission}";
                        return false;
                    }

                    permissionHolder.Permissions.Add(permission);
                    PermissionsMod.Instance.Storage.Save();

                    response = $"Added {permission} to {id}";
                    return true;
                }

                response = id + " not found";
                return false;
            }

            public string Command => "add";
            public string[] Aliases => new string[0];
            public string Description => "Adds permission";
            public PermissionHolderCommand<T> Parent { get; }
        }

        public class RemoveCommand : ICommand
        {
            public RemoveCommand(PermissionHolderCommand<T> parent)
            {
                Parent = parent;
            }

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                var id = arguments.At(0);
                var permission = arguments.At(1);

                if (Parent.Dictionary.TryGetValue(id, out var permissionHolder))
                {
                    if (!permissionHolder.Permissions.Contains(permission))
                    {
                        response = $"{id} doesn't have {permission}";
                        return false;
                    }

                    permissionHolder.Permissions.Remove(permission);
                    PermissionsMod.Instance.Storage.Save();

                    response = $"Removed {permission} from {id}";
                    return true;
                }

                response = id + " not found";
                return false;
            }

            public string Command => "remove";
            public string[] Aliases => new string[0];
            public string Description => "Removes permission";
            public PermissionHolderCommand<T> Parent { get; }
        }
    }
}