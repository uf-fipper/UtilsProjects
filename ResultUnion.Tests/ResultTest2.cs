using ResultUnion.ResultExtensions;

namespace ResultUnion.Tests;

public class ExceptionBase : Exception;

public class SubException1 : ExceptionBase;

public class SubException2 : ExceptionBase;

[TestClass]
public class ResultTest2
{
    private static int ThrowMethod(Exception? e)
    {
        if (e == null)
            return 100;
        throw e;
    }

    private static void ThrowVoidMethod(Exception? e)
    {
        if (e == null)
            return;
        throw e;
    }

    [TestMethod]
    public void TestCatchRun()
    {
        var result1 = ResultExtension.CatchRun(() => 1);
        Assert.AreEqual(result1.Unwrap(), 1);

        var result2 = ResultExtension.CatchRun(() => ThrowMethod(new SubException1()));
        Assert.IsInstanceOfType<SubException1>(result2.UnwrapErr());

        var result3 = ResultExtension.CatchRun<int, ExceptionBase>(
            () => ThrowMethod(new SubException2())
        );
        Assert.IsInstanceOfType<SubException2>(result3.UnwrapErr());

        Assert.ThrowsException<SubException2>(
            () =>
                ResultExtension.CatchRun<int, SubException1>(() => ThrowMethod(new SubException2()))
        );

        Assert.ThrowsException<ExceptionBase>(
            () =>
                ResultExtension.CatchRun<int, SubException1>(() => ThrowMethod(new ExceptionBase()))
        );
    }

    [TestMethod]
    public void TestCatchRunVoid()
    {
        var result1 = ResultExtension.CatchRun(() => { });
        Assert.IsNull(result1);

        var result2 = ResultExtension.CatchRun(() => ThrowVoidMethod(new SubException1()));
        Assert.IsTrue(result2 is SubException1);

        var result3 = ResultExtension.CatchRun<ExceptionBase>(
            () => ThrowVoidMethod(new SubException2())
        );
        Assert.IsInstanceOfType<SubException2>(result3);

        Assert.ThrowsException<SubException2>(
            () =>
                ResultExtension.CatchRun<SubException1>(() => ThrowVoidMethod(new SubException2()))
        );

        Assert.ThrowsException<ExceptionBase>(
            () =>
                ResultExtension.CatchRun<SubException1>(() => ThrowVoidMethod(new ExceptionBase()))
        );
    }

    [TestMethod]
    public void TestThrowIfException()
    {
        var okResult = Ok<int, Exception>(100);
        var errResult = Err<int, Exception>(new Exception("Error"));

        Assert.AreEqual(100, okResult.ThrowIfException());
        Assert.ThrowsException<Exception>(() => errResult.ThrowIfException());
    }
}
