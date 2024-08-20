using System.ComponentModel;
using System.Reflection;

namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            if (fieldInfo == null)
                return null;

            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            return descriptionAttribute?.Description ?? value.ToString();
        }
    }
}
