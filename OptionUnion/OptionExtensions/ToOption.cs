namespace OptionUnion.OptionExtensions;

public static partial class OptionExtension
{
    public static Option<T> ToOption<T>(this T? value)
        where T : struct
    {
        return value switch
        {
            null => Option.None<T>(),
            _ => Option.Some(value.Value),
        };
    }
}
