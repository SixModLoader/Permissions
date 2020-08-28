using System.Collections.Generic;

namespace Permissions
{
    public class PermissionHolder
    {
        public List<string> Permissions { get; } = new List<string>();

        public bool HasPermission(string permission)
        {
            var a = permission.Split('.');

            foreach (var node in Permissions)
            {
                var b = node.Split('.');

                if (b.Length == 2 && b[0] == "group")
                {
                    var group = PermissionsMod.Instance.Storage.Groups[b[1]];
                    if (group.HasPermission(permission))
                    {
                        return true;
                    }

                    continue;
                }

                if (a.Length > b.Length)
                    continue;

                var yes = false;

                for (var i = 0; i < b.Length; i++)
                {
                    if (a.Length <= i)
                    {
                        yes = false;
                        break;
                    }

                    if (b[i] == "*")
                    {
                        return true;
                    }

                    if (b[i] == a[i])
                    {
                        yes = true;
                    }
                    else
                    {
                        yes = false;
                        break;
                    }
                }

                if (yes)
                    return true;
            }

            return false;
        }
    }

    public class User : PermissionHolder
    {
    }

    public class Group : PermissionHolder
    {
        // Vanilla group id, use ^ for same id
        public string Vanilla { get; set; }
    }
}