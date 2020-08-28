using System;
using System.IO;
using Permissions.Storage;
using SixModLoader.Api.Configuration;
using SixModLoader.Api.Extensions;
using SixModLoader.Events;
using SixModLoader.Mods;

namespace Permissions
{
    [Mod("Permissions")]
    public class PermissionsMod
    {
        public static PermissionsMod Instance { get; private set; }

        public PermissionsMod()
        {
            Instance = this;
            PermissionExtensions.Provider = new PermissionProvider();
        }

        [Inject]
        public ModContainer ModContainer { get; set; }

        [AutoConfiguration(ConfigurationType.Configuration)]
        public Configuration Configuration { get; set; }

        public IStorage Storage { get; private set; }

        [EventHandler(typeof(ModReloadEvent))]
        public void OnReload()
        {
            switch (Configuration.StorageMethod)
            {
                case StorageMethod.Yaml:
                    Storage = (YamlStorage) ConfigurationManager.LoadConfigurationFile(typeof(YamlStorage), YamlStorage.File = Path.Combine(ModContainer.Directory, "permissions.yml"), new YamlStorage());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}