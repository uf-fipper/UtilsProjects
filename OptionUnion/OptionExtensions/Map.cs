namespace OptionUnion.OptionExtensions;

public static partial class OptionExtension
{
    /// <summary>
    /// alias for MapOr
    /// </summary>
    public static U Map<T, U>(this Option<T> option, U @default, Func<T, U> func) =>
        option.MapOr(@default, func);

    /// <summary>
    /// alias for MapOrElse
    /// </summary>
    public static U Map<T, U>(this Option<T> option, Func<U> fe, Func<T, U> ft) =>
        option.MapOrElse(fe, ft);
}
