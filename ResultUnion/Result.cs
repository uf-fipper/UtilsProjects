namespace ResultUnion;

public abstract partial record Result<T, E> : IResult<T, E>
{
    private Result() { }

    internal record Ok(T Data) : Result<T, E>
    {
        public override string ToString()
        {
            return $"Ok({Data})";
        }
    }

    internal record Err(E Error) : Result<T, E>
    {
        public override string ToString()
        {
            return $"Err({Error})";
        }
    }
}

public abstract partial record Result<T, E>
{
    internal Ok AsOk() => (Ok)this;

    public bool IsOk() => this is Ok;

    internal Err AsErr() => (Err)this;

    public bool IsErr() => this is Err;

    public bool IsOkAnd(Func<T, bool> func) => this is Ok ok && func(ok.Data);

    public T Unwrap() => AsOk().Data;

    public T UnwrapOr(T @default)
    {
        return this switch
        {
            Ok ok => ok.Data,
            _ => @default,
        };
    }

    public T UnwrapOr(Func<E, T> func) => UnwrapOrElse(func);

    public T? UnwrapOrDefault()
    {
        return this switch
        {
            Ok ok => ok.Data,
            _ => default,
        };
    }

    public T UnwrapOrElse(Func<E, T> func)
    {
        return this switch
        {
            Ok ok => ok.Data,
            Err err => func(err.Error),
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public Result<U, E> And<U>(Result<U, E> res)
    {
        return this switch
        {
            Ok => res,
            Err err => Result.Err<U, E>(err.Error),
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public Result<U, E> And<U>(Func<T, Result<U, E>> func) => AndThen(func);

    public Result<U, E> AndThen<U>(Func<T, Result<U, E>> func)
    {
        return this switch
        {
            Ok ok => func(ok.Data),
            Err err => Result.Err<U, E>(err.Error),
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public Result<T, E> Inspect(Action<T> func)
    {
        if (IsOk())
            func(Unwrap());

        return this;
    }

    public T Expect(string message)
    {
        if (this is Ok ok)
            return ok.Data;
        throw new InvalidOperationException(message);
    }

    public bool IsErrAnd(Func<E, bool> func)
    {
        return this is Err err && func(err.Error);
    }

    public E UnwrapErr() => AsErr().Error;

    public Result<T, U> Or<U>(Result<T, U> res)
    {
        return this switch
        {
            Ok ok => Result.Ok<T, U>(ok.Data),
            Err => res,
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public Result<T, U> Or<U>(Func<E, Result<T, U>> func) => OrElse(func);

    public Result<T, U> OrElse<U>(Func<E, Result<T, U>> func)
    {
        return this switch
        {
            Ok ok => Result.Ok<T, U>(ok.Data),
            Err err => func(err.Error),
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public Result<T, E> InspectErr(Action<E> func)
    {
        if (this is Err err)
            func(err.Error);

        return this;
    }

    public E ExpectErr(string message)
    {
        if (this is Err err)
            return err.Error;
        throw new InvalidOperationException(message);
    }

    public Result<U, E> Map<U>(Func<T, U> func)
    {
        return this switch
        {
            Ok ok => Result.Ok<U, E>(func(ok.Data)),
            Err err => Result.Err<U, E>(err.Error),
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public U Map<U>(U @default, Func<T, U> func) => MapOr(@default, func);

    public U Map<U>(Func<E, U> fe, Func<T, U> ft) => MapOrElse(fe, ft);

    public U MapOr<U>(U @default, Func<T, U> func)
    {
        return this switch
        {
            Ok ok => func(ok.Data),
            Err => @default,
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public U MapOrElse<U>(Func<E, U> fe, Func<T, U> ft)
    {
        return this switch
        {
            Ok ok => ft(ok.Data),
            Err err => fe(err.Error),
            _ => throw new NotSupportedException("Not supported"),
        };
    }

    public Result<T, U> MapErr<U>(Func<E, U> func)
    {
        return this switch
        {
            Ok ok => Result.Ok<T, U>(ok.Data),
            Err err => Result.Err<T, U>(func(err.Error)),
            _ => throw new NotSupportedException("Not supported"),
        };
    }
}

public static class Result
{
    public static Result<TData, TError> Ok<TData, TError>(TData data) =>
        new Result<TData, TError>.Ok(data);

    public static Result<TData, object> Ok<TData>(TData data) => new Result<TData, object>.Ok(data);

    public static Result<TData, TError> Err<TData, TError>(TError error) =>
        new Result<TData, TError>.Err(error);

    public static Result<object, TError> Err<TError>(TError error) =>
        new Result<object, TError>.Err(error);

    public static Result<T, E> CatchRun<T, E>(Func<T> func)
        where E : Exception
    {
        try
        {
            return Ok<T, E>(func());
        }
        catch (E e)
        {
            return Err<T, E>(e);
        }
    }

    public static Result<T, Exception> CatchRun<T>(Func<T> func) => CatchRun<T, Exception>(func);

    public static T ThrowIfException<T, E>(this Result<T, E> result)
        where E : Exception
    {
        if (result.IsErr())
            throw result.UnwrapErr();
        return result.Unwrap();
    }
}
