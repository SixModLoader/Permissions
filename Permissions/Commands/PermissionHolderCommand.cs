using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using HarmonyLib;

namespace Permissions.Commands
{
    public class PermissionHolderCommand<T> : CommandHandler, ICommand where T : PermissionHolder
    {
        public PermissionHolderCommand(Func<Dictionary<string, T>> dictionary)
        {
            _dictionary = dictionary;
            LoadGeneratedCommands();
        }

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new CreateCommand(this));
            RegisterCommand(new PermissionsCommand<T>(this));
        }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"Please specify a valid subcommand! ({Dictionary.Keys.Join()}) + ({Commands.Keys.Join()})";

            if (arguments.Array != null && arguments.Count >= 2 && this.TryGetCommand(arguments.At(1), out var command))
            {
                var args = arguments.ToList();
                args.RemoveAt(1);

                if (args.Count > 1)
                {
                    var id = args[0];
                    args.Remove(id);
                    args.Insert(1, id);
                }

                command.Execute(new ArraySegment<string>(args.ToArray()), sender, out response);
            }

            return false;
        }

        public string Command => typeof(T).Name.ToLower();
        public string[] Aliases => new string[0];
        public string Description => "Manage" + Command;

        private readonly Func<Dictionary<string, T>> _dictionary;
        public Dictionary<string, T> Dictionary => _dictionary.Invoke();

        public class CreateCommand : ICommand
        {
            public CreateCommand(PermissionHolderCommand<T> parent)
            {
                Parent = parent;
            }

            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            {
                var id = arguments.At(0);

                if (Parent.Dictionary.ContainsKey(id))
                {
                    response = id + " already exists!";
                }
                else
                {
                    Parent.Dictionary[id] = Activator.CreateInstance<T>();
                    PermissionsMod.Instance.Storage.Save();

                    response = "Created " + id;
                }

                return true;
            }

            public string Command => "create";
            public string[] Aliases => new string[0];
            public string Description => "Creates " + typeof(T).Name;
            public PermissionHolderCommand<T> Parent { get; }
        }
    }
}