using System;

namespace FBDropshipper.Common.Extensions
{
    public static class EnumExtension
    {
        public static int ToInt<T>(this T source) where T : IConvertible//enum
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return (int)(IConvertible)source;
        }
    }
}