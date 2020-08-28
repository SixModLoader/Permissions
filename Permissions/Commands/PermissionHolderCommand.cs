using System;
using System.Collections.Generic;
using CommandSystem;

namespace Permissions.Commands
{
    public class PermissionHolderCommand<T> : ParentCommand
    {
        public PermissionHolderCommand(Dictionary<string, T> dictionary)
        {
            Dictionary = dictionary;
            LoadGeneratedCommands();
        }

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new AddCommand(this));
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Please specify a valid subcommand!";
            return false;
        }

        public override string Command => typeof(T).Name.ToLower() + "s";
        public override string[] Aliases => new string[0];
        public override string Description => "Manage" + Command;
        public Dictionary<string, T> Dictionary { get; }

        public class AddCommand : ICommand
        {
            public AddCommand(PermissionHolderCommand<T> parent)
            {
                Parent = parent;
            }

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                Parent.Dictionary[arguments.At(0)] = Activator.CreateInstance<T>();
                PermissionsMod.Instance.Storage.Save();

                response = "Done :)";
                return true;
            }

            public string Command => "add";
            public string[] Aliases => new string[0];
            public string Description => "Adds " + typeof(T).Name;
            public PermissionHolderCommand<T> Parent { get; }
        }
    }
}