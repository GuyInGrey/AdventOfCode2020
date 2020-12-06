using System;
using System.Reflection;

namespace GuyInGrey_AoC2020
{
    public static class Extensions
    {
        public static T GetAttribute<T>(this MemberInfo t) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(t, typeof(T));
    }
}
