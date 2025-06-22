global using static ResultUnion.Result;

namespace ResultUnion.Tests;

[TestClass]
public class ResultTest1
{
    private static Result<int, string> OkResult => Ok<int, string>(100);
    private static Result<int, string> ErrResult => Err<int, string>("Error");

    public static bool IsOk<T, E>(Result<T, E> result) => result.IsOk() && !result.IsErr();

    public static bool IsErr<T, E>(Result<T, E> result) => result.IsErr() && !result.IsOk();

    [TestMethod]
    public void TestIsOk()
    {
        Assert.IsTrue(IsOk(OkResult));

        Assert.IsFalse(IsOk(ErrResult));
    }

    [TestMethod]
    public void TestIsErr()
    {
        Assert.IsTrue(IsErr(ErrResult));

        Assert.IsFalse(IsErr(OkResult));
    }

    [TestMethod]
    public void TestIsOkAnd()
    {
        Assert.IsTrue(OkResult.IsOkAnd(x => x == 100));
        Assert.IsFalse(OkResult.IsOkAnd(x => x == 200));

        Assert.IsFalse(ErrResult.IsOkAnd(x => x == 100));
        Assert.IsFalse(ErrResult.IsOkAnd(x => x == 100));
    }

    [TestMethod]
    public void TestUnwrap()
    {
        Assert.AreEqual(100, OkResult.Unwrap());
        Assert.ThrowsException<InvalidCastException>(() => ErrResult.Unwrap());
    }

    [TestMethod]
    public void TestUnwrapOr()
    {
        Assert.AreEqual(100, OkResult.UnwrapOr(200));
        Assert.AreEqual(200, ErrResult.UnwrapOr(200));
    }

    [TestMethod]
    public void TestUnwrapOrDefault()
    {
        Assert.AreEqual(100, OkResult.UnwrapOrDefault());
        Assert.AreEqual(0, ErrResult.UnwrapOrDefault());
    }

    [TestMethod]
    public void TestUnwrapOrElse()
    {
        Assert.AreEqual(100, OkResult.UnwrapOrElse(err => 200));
        Assert.AreEqual(200, ErrResult.UnwrapOrElse(err => 200));
    }

    [TestMethod]
    public void TestAnd()
    {
        var ok = OkResult.And(Ok<string, string>("100"));
        var err = ErrResult.And(Ok<string, string>("100"));

        Assert.IsTrue(ok.IsOk());
        Assert.AreEqual("100", ok.Unwrap());

        Assert.IsTrue(err.IsErr());
        Assert.AreEqual("Error", err.UnwrapErr());
    }

    [TestMethod]
    public void TestAndThen()
    {
        var ok = OkResult.AndThen(x => Ok<string, string>(x.ToString()));
        var err = ErrResult.AndThen(x => Ok<string, string>(x.ToString()));

        Assert.IsTrue(ok.IsOk());
        Assert.AreEqual("100", ok.Unwrap());

        Assert.IsTrue(err.IsErr());
        Assert.AreEqual("Error", err.UnwrapErr());
    }

    [TestMethod]
    public void TestInspect()
    {
        bool flag = false;
        OkResult.Inspect(x => flag = x == 100);
        Assert.IsTrue(flag);

        flag = false;
        ErrResult.Inspect(x => flag = x == 100);
        Assert.IsFalse(flag);
    }

    [TestMethod]
    public void TestExpect()
    {
        Assert.AreEqual(100, OkResult.Expect("Should not throw"));
        Assert.ThrowsException<InvalidOperationException>(() => ErrResult.Expect("Should throw"));
    }

    [TestMethod]
    public void TestIsErrAnd()
    {
        Assert.IsTrue(ErrResult.IsErrAnd(x => true));
        Assert.IsFalse(ErrResult.IsErrAnd(x => false));
        Assert.IsFalse(OkResult.IsErrAnd(x => true));
    }

    [TestMethod]
    public void TestUnwrapErr()
    {
        Assert.AreEqual("Error", ErrResult.UnwrapErr());
        Assert.ThrowsException<InvalidCastException>(() => OkResult.UnwrapErr());
    }

    [TestMethod]
    public void TestOr()
    {
        var ok = OkResult.Or(Err<int, int>(-1));
        var err = ErrResult.Or(Ok<int, int>(-1));

        Assert.IsTrue(ok.IsOk());
        Assert.AreEqual(100, ok.Unwrap());

        Assert.IsTrue(err.IsOk());
        Assert.AreEqual(-1, err.Unwrap());
    }

    [TestMethod]
    public void TestOrElse()
    {
        var ok = OkResult.OrElse(err => Err<int, int>(err.Length));
        var err = ErrResult.OrElse(err => Ok<int, int>(err.Length));

        Assert.IsTrue(ok.IsOk());
        Assert.AreEqual(100, ok.Unwrap());

        Assert.IsTrue(err.IsOk());
        Assert.AreEqual("Error".Length, err.Unwrap());
    }

    [TestMethod]
    public void TestInspectErr()
    {
        bool flag = false;
        ErrResult.InspectErr(x => flag = true);
        Assert.IsTrue(flag);

        flag = false;
        OkResult.InspectErr(x => flag = true);
        Assert.IsFalse(flag);
    }

    [TestMethod]
    public void TestExpectErr()
    {
        Assert.AreEqual("Error", ErrResult.ExpectErr("Should not throw"));
        Assert.ThrowsException<InvalidOperationException>(() => OkResult.ExpectErr("Should throw"));
    }

    [TestMethod]
    public void TestMap()
    {
        var ok = OkResult.Map(x => x.ToString());
        var err = ErrResult.Map(x => x.ToString());

        Assert.IsTrue(ok.IsOk());
        Assert.AreEqual("100", ok.Unwrap());

        Assert.IsTrue(err.IsErr());
        Assert.AreEqual("Error", err.UnwrapErr());
    }

    [TestMethod]
    public void TestMapOr()
    {
        var ok = OkResult.MapOr("Default", x => x.ToString());
        var err = ErrResult.MapOr("Default", x => x.ToString());

        Assert.AreEqual("100", ok);
        Assert.AreEqual("Default", err);
    }

    [TestMethod]
    public void TestMapOrElse()
    {
        var ok = OkResult.MapOrElse(err => err + "Default", x => x.ToString());
        var err = ErrResult.MapOrElse(err => err + "Default", x => x.ToString());

        Assert.AreEqual("100", ok);
        Assert.AreEqual("ErrorDefault", err);
    }

    [TestMethod]
    public void TestMapErr()
    {
        var ok = OkResult.MapErr(err => err + "Default");
        var err = ErrResult.MapErr(err => err + "Default");

        Assert.IsTrue(ok.IsOk());
        Assert.AreEqual(100, ok.Unwrap());

        Assert.IsTrue(err.IsErr());
        Assert.AreEqual("ErrorDefault", err.UnwrapErr());
    }
}
