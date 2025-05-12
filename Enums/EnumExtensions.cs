using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NajdiSpolubydliciRazor.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            MemberInfo info = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First();

            var attribute = info.GetCustomAttribute<DisplayAttribute>();

            if (attribute is null) return string.Empty;

            return attribute.GetName() ?? string.Empty;
        }
    }
}
