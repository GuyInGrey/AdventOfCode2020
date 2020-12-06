using System;
using System.Linq;
using System.Reflection;

namespace GuyInGrey_AoC2020
{
    public static class Extensions
    {
        public static T GetAttribute<T>(this MemberInfo t) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(t, typeof(T));

        public static string Repeat(this string text, int count)
        {
            if (!String.IsNullOrEmpty(text))
            {
                return String.Concat(Enumerable.Repeat(text, count));
            }
            return "";
        }
    }
}
