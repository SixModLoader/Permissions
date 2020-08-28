using System.Collections.Generic;

namespace Permissions.Storage
{
    public interface IStorage
    {
        Dictionary<string, User> Users { get; }
        Dictionary<string, Group> Groups { get; }
        PermissionHolder Default { get; }

        void Save();
    }
}