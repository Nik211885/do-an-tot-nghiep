using System.Reflection;

namespace Infrastructure.Options;

internal class KeyOptionsAttribute(string key)
    : Attribute
{
    public string Key { get; } = key;
}

internal static class AttributeKeyOptionExtension
{
    public static string GetKeyOptions(this object obj)
    {
        var objType = obj.GetType();
        var attributeKeyOption = objType.GetCustomAttribute<KeyOptionsAttribute>();
        return attributeKeyOption?.Key
            ?? throw new Exception($"Not config key for {objType.Name}");
    }
    // Key and type for option
    public static Dictionary<string, Type> GetTypeOptions(this Assembly assembly)
    {
        var options = new Dictionary<string, Type>();
        foreach (var type in assembly.GetTypes())
        {
            var attr = type.GetCustomAttribute<KeyOptionsAttribute>();
            if (attr is not null)
            {
                options.Add(attr.Key, type);
            }
        }
        return options;
    }
}
