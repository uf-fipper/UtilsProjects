namespace OptionUnion.OptionExtensions;

public static partial class OptionExtension
{
    /// <summary>
    /// alias for OrElse
    /// </summary>
    public static Option<T> Or<T>(this Option<T> option, Func<Option<T>> func) =>
        option.OrElse(func);
}
