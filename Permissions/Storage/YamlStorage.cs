using System;
using System.Collections.Generic;
using SixModLoader.Api.Configuration;

namespace Permissions.Storage
{
    [Serializable]
    public class YamlStorage : IStorage
    {
        public static string File { get; set; }

        public Dictionary<string, User> Users { get; private set; }

        public Dictionary<string, Group> Groups { get; private set; }

        public PermissionHolder Default { get; private set; }

        public YamlStorage()
        {
            Users = new Dictionary<string, User>
            {
                ["someone@example"] = new User
                {
                    Permissions =
                    {
                        "group.admin",
                        "essentials.bypass-slots"
                    }
                }
            };

            Groups = new Dictionary<string, Group>
            {
                ["user"] = new Group
                {
                    Permissions =
                    {
                        "essentials.vote.kick"
                    }
                },

                ["admin"] = new Group
                {
                    Vanilla = "^",
                    Permissions =
                    {
                        "essentials.*"
                    }
                }
            };

            Default = new PermissionHolder
            {
                Permissions =
                {
                    "group.user"
                }
            };
        }

        public void Save()
        {
            var yaml = ConfigurationManager.Serializer.Serialize(this);
            System.IO.File.WriteAllText(File, yaml);
        }
    }
}