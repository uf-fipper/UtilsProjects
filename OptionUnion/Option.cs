namespace OptionUnion;

public readonly partial record struct Option<T> : IOption<T>
{
    internal readonly object Inner = None.Instance;

    internal Option(Some inner)
    {
        Inner = inner;
    }

    internal Option(None inner)
    {
        Inner = inner;
    }

    internal readonly record struct Some(T Data)
    {
        public override string ToString()
        {
            return $"Some({Data})";
        }
    }

    internal readonly record struct None
    {
        public static readonly None Instance = new();

        public override string ToString()
        {
            return "None";
        }
    }

    public override string ToString() => Inner.ToString()!;
}

public partial record struct Option<T>
{
    public bool IsSome() => Inner is Some;

    internal Some AsSome() => (Some)Inner;

    public bool IsNone() => Inner is None;

    internal None AsNone() => (None)Inner;

    public bool IsSomeAnd(Func<T, bool> func) => Inner is Some some && func(some.Data);

    public T Unwrap() => AsSome().Data;

    public T UnwrapOr(T @default)
    {
        return Inner switch
        {
            Some some => some.Data,
            _ => @default,
        };
    }

    public T? UnwrapOrDefault()
    {
        return Inner switch
        {
            Some some => some.Data,
            _ => default,
        };
    }

    public T UnwrapOrElse(Func<T> func)
    {
        return Inner switch
        {
            Some some => some.Data,
            _ => func(),
        };
    }

    public Option<U> And<U>(Option<U> x)
    {
        return Inner switch
        {
            Some => x,
            _ => Option.None<U>(),
        };
    }

    public Option<U> AndThen<U>(Func<T, Option<U>> func)
    {
        return Inner is Some some ? func(some.Data) : Option.None<U>();
    }

    public bool IsNoneOr(Func<T, bool> func)
    {
        return IsNone() || func(Unwrap());
    }

    public Option<T> Or(Option<T> x)
    {
        return IsSome() ? this : x;
    }

    public Option<T> OrElse(Func<Option<T>> func)
    {
        return IsSome() ? this : func();
    }

    public Option<U> Map<U>(Func<T, U> func)
    {
        return Inner switch
        {
            Some some => Option.Some(func(some.Data)),
            _ => Option.None<U>(),
        };
    }

    public U MapOr<U>(U @default, Func<T, U> func)
    {
        return Inner switch
        {
            Some some => func(some.Data),
            _ => @default,
        };
    }

    public U MapOrElse<U>(Func<U> fe, Func<T, U> ft)
    {
        return Inner switch
        {
            Some some => ft(some.Data),
            _ => fe(),
        };
    }

    public Option<T> Inspect(Action<T> func)
    {
        if (Inner is Some some)
            func(some.Data);
        return this;
    }

    public T Expect(string message)
    {
        if (Inner is Some some)
            return some.Data;
        throw new InvalidOperationException(message);
    }

    public Option<T> Filter(Func<T, bool> predicate)
    {
        return Inner switch
        {
            Some some when predicate(some.Data) => this,
            _ => Option.None<T>(),
        };
    }

    public Option<T> Xor(Option<T> other)
    {
        return (IsSome(), other.IsSome()) switch
        {
            (true, false) => this,
            (false, true) => other,
            _ => Option.None<T>(),
        };
    }
}

public static class Option
{
    public static Option<T> Some<T>(T data) => new(new Option<T>.Some(data));

    public static Option<T> None<T>() => new(Option<T>.None.Instance);

    public static Option<object> None() => new(Option<object>.None.Instance);
}
