namespace OptionUnion;

public interface IOption
{
    public bool IsSome();

    public bool IsNone();
}

public interface IOption<T>
{
    public bool IsSomeAnd(Func<T, bool> func);

    public T Unwrap();

    public T UnwrapOr(T @default);

    public T? UnwrapOrDefault();

    public T UnwrapOrElse(Func<T> func);

    public Option<U> And<U>(Option<U> x);

    public Option<U> AndThen<U>(Func<T, Option<U>> func);

    public bool IsNoneOr(Func<T, bool> func);

    public Option<T> Or(Option<T> x);

    public Option<T> OrElse(Func<Option<T>> func);

    public Option<U> Map<U>(Func<T, U> func);

    public U MapOr<U>(U @default, Func<T, U> func);

    public U MapOrElse<U>(Func<U> fe, Func<T, U> ft);

    public Option<T> Inspect(Action<T> func);

    public T Expect(string message);
}

public interface ISome : IOption;

public interface ISome<T> : IOption<T>, ISome
{
    public T Data { get; }
}

public interface INone : IOption;

public interface INone<T> : IOption<T>, INone;
