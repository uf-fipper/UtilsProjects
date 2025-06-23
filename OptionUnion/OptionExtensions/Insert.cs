using System.Runtime.CompilerServices;

namespace OptionUnion.OptionExtensions;

public static partial class OptionExtension
{
    public static T Insert<T>(this in Option<T> option, T value)
    {
        Unsafe.AsRef(in option) = Option.Some(value);
        return value;
    }

    public static T GetOrInsert<T>(this in Option<T> option, T value)
    {
        if (option.IsSome())
            return option.Unwrap();

        Unsafe.AsRef(in option) = Option.Some(value);
        return value;
    }

    public static T? GetOrInsertDefault<T>(this in Option<T?> option) =>
        GetOrInsert(option, default);

    public static T GetOrInsertWith<T>(this in Option<T> option, Func<T> func)
    {
        if (option.IsSome())
            return option.Unwrap();

        var value = func();
        Unsafe.AsRef(in option) = Option.Some(value);
        return value;
    }
}
