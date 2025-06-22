namespace OptionUnion;

public abstract partial record Option<T> : IOption<T>
{
    private Option() { }

    internal sealed record Some(T Data) : Option<T>
    {
        public override string ToString()
        {
            return $"Some({Data})";
        }
    }

    internal sealed record None : Option<T>
    {
        public static readonly None Instance = new();

        public override string ToString()
        {
            return "None";
        }
    }
}

public partial record Option<T>
{
    public bool IsSome() => this is Some;

    internal Some AsSome() => (Some)this;

    public bool IsNone() => this is None;

    internal None AsNone() => (None)this;

    public bool IsSomeAnd(Func<T, bool> func) => this is Some some && func(some.Data);

    public T Unwrap() => AsSome().Data;

    public T UnwrapOr(T @default)
    {
        return this switch
        {
            Some some => some.Data,
            _ => @default,
        };
    }

    public T? UnwrapOrDefault()
    {
        return this switch
        {
            Some some => some.Data,
            _ => default,
        };
    }

    public T UnwrapOrElse(Func<T> func)
    {
        return this switch
        {
            Some some => some.Data,
            _ => func(),
        };
    }

    public Option<U> And<U>(Option<U> x)
    {
        return this switch
        {
            Some => x,
            _ => Option.None<U>(),
        };
    }

    public Option<U> AndThen<U>(Func<T, Option<U>> func)
    {
        return this is Some some ? func(some.Data) : Option.None<U>();
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
        return this switch
        {
            Some some => Option.Some(func(some.Data)),
            _ => Option.None<U>(),
        };
    }

    public U MapOr<U>(U @default, Func<T, U> func)
    {
        return this switch
        {
            Some some => func(some.Data),
            _ => @default,
        };
    }

    public U MapOrElse<U>(Func<U> fe, Func<T, U> ft)
    {
        return this switch
        {
            Some some => ft(some.Data),
            _ => fe(),
        };
    }

    public Option<T> Inspect(Action<T> func)
    {
        if (this is Some some)
            func(some.Data);
        return this;
    }

    public T Expect(string message)
    {
        if (this is Some some)
            return some.Data;
        throw new InvalidOperationException(message);
    }
}

public static class Option
{
    public static Option<T> Some<T>(T data) => new Option<T>.Some(data);

    public static Option<T> None<T>() => Option<T>.None.Instance;

    public static Option<object> None() => Option<object>.None.Instance;
}
