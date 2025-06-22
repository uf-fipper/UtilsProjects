namespace OptionUnion.OptionExtensions;

public static partial class OptionExtension
{
    public static T? ToNullable<T>(this Option<T> option)
        where T : struct
    {
        return option.MapOr(null, x => (T?)x);
    }
}
