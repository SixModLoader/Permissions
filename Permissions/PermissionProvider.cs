using System.Linq;
using CommandSystem;
using GameCore;
using RemoteAdmin;
using SixModLoader.Api.Extensions;

namespace Permissions
{
    public class PermissionProvider : IPermissionProvider
    {
        public bool HasPermission(ICommandSender sender, string permission)
        {
            if (sender is ConsoleCommandSender || sender is ServerConsoleSender)
            {
                return true;
            }

            if (sender is PlayerCommandSender playerSender)
            {
                var player = ReferenceHub.GetHub(playerSender.Processor.gameObject);
                return player.HasPermission(permission);
            }

            return false;
        }

        public bool HasPermission(ReferenceHub player, string permission)
        {
            if (player == ReferenceHub.HostHub)
            {
                return true;
            }

            if (PermissionsMod.Instance.Storage.Users.TryGetValue(player.characterClassManager.UserId, out var user) && user.HasPermission(permission))
            {
                return true;
            }

            if (player.serverRoles.Group != null)
            {
                // TODO caching?
                var group = PermissionsMod.Instance.Storage.Groups.FirstOrDefault(x =>
                {
                    var vanilla = x.Value.Vanilla;

                    if (vanilla == "^")
                    {
                        vanilla = x.Key;
                    }

                    return vanilla == ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value == player.serverRoles.Group).Key;
                });

                if (group.Value != null && group.Value.HasPermission(permission))
                {
                    return true;
                }
            }

            return false;
        }
    }
}