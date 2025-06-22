namespace ResultUnion;

public interface IResult
{
    public bool IsOk();

    public bool IsErr();
}

public interface IResult<T, E> : IResult
{
    public bool IsOkAnd(Func<T, bool> func);

    public T Unwrap();

    public T UnwrapOr(T @default);

    public T? UnwrapOrDefault();

    public T UnwrapOrElse(Func<E, T> func);

    public Result<U, E> And<U>(Result<U, E> res);

    public Result<U, E> AndThen<U>(Func<T, Result<U, E>> func);

    public Result<T, E> Inspect(Action<T> func);

    public T Expect(string message);

    public bool IsErrAnd(Func<E, bool> func);

    public E UnwrapErr();

    public Result<T, U> Or<U>(Result<T, U> res);

    public Result<T, U> OrElse<U>(Func<E, Result<T, U>> func);

    public Result<T, E> InspectErr(Action<E> func);

    public E ExpectErr(string message);

    public Result<U, E> Map<U>(Func<T, U> func);

    public U MapOr<U>(U @default, Func<T, U> func);

    public U MapOrElse<U>(Func<E, U> fe, Func<T, U> ft);

    public Result<T, U> MapErr<U>(Func<E, U> func);
}

public interface IOk : IResult;

public interface IOk<out T> : IOk
{
    public T Data { get; }
}

public interface IOk<out T, E> : IOk<T>;

public interface IErr : IResult;

public interface IErr<out E> : IErr
{
    public E Error { get; }
}

public interface IErr<T, out E> : IErr<E>;
