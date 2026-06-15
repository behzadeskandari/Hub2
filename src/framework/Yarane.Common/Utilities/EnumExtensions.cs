using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Yarane.Common.Utilities
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }
        public static string ToName(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static string GetEnumDisplayName(Enum enumValue)
        {
            Type enumType = enumValue.GetType();
            string enumName = Enum.GetName(enumType, enumValue);

            MemberInfo memberInfo = enumType.GetField(enumName);
            if (memberInfo != null)
            {
                DisplayAttribute displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    return displayAttribute.Name;
                }
            }

            return enumName;
        }

    }
}
