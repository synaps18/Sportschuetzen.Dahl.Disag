using Sportschuetzen.Dahl.Disag.Rm3.Enum;

namespace Sportschuetzen.Dahl.Disag.Rm3.Extensions;

public static class EDisagFlagExtension
{
    public static EDisagFlag FromString(this EDisagFlag flag, string str)
    {
        return EDisagFlag.G;
    }

    public static T ParseJson<T>(this T t, string s) where T : new()
    {
        return t;
    }
}