namespace Permissions
{
    public class Configuration
    {
        public StorageMethod StorageMethod { get; set; } = StorageMethod.Yaml;
    }

    public enum StorageMethod
    {
        Yaml
    }
}