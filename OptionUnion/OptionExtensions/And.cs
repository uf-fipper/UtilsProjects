namespace OptionUnion.OptionExtensions;

public static partial class OptionExtension
{
    /// <summary>
    /// alias for AndThen
    /// </summary>
    public static Option<U> And<T, U>(this Option<T> option, Func<T, Option<U>> func) =>
        option.AndThen(func);
}
