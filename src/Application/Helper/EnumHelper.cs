using System.ComponentModel;

namespace Application.Helper;

public static class EnumHelper
{
    public static string GetDescriptionAttribute(this Enum @enum)
    {
        var field = @enum.GetType().GetField(@enum.ToString());
        if (field is null)
        {
            return String.Empty;
        }
        var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        if (attr is null)
        {
            return string.Empty;
        }
        var description = (DescriptionAttribute)attr;
        return description.Description ?? string.Empty;
    }
}
