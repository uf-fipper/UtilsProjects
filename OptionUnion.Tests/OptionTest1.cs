global using static OptionUnion.Option;

namespace OptionUnion.Tests;

[TestClass]
public class OptionTest1
{
    private static Option<int> SomeInt => Some(100);
    private static Option<int> NoneInt => None<int>();

    private static Option<string> SomeString => Some("string");
    private static Option<string> NoneString => None<string>();

    public static bool IsSome<T>(Option<T> option) => option.IsSome() && !option.IsNone();

    public static bool IsNone<T>(Option<T> option) => option.IsNone() && !option.IsSome();

    [TestMethod]
    public void TestIsSome()
    {
        Assert.IsTrue(IsSome(SomeInt));
        Assert.IsTrue(IsSome(SomeString));

        Assert.IsTrue(IsNone(NoneInt));
        Assert.IsTrue(IsNone(NoneString));
    }

    [TestMethod]
    public void TestIsNone()
    {
        Assert.IsTrue(IsNone(NoneInt));
        Assert.IsTrue(IsNone(NoneString));

        Assert.IsTrue(IsSome(SomeInt));
        Assert.IsTrue(IsSome(SomeString));
    }

    [TestMethod]
    public void TestIsSomeAnd()
    {
        Assert.IsTrue(SomeInt.IsSomeAnd(x => x == 100));
        Assert.IsFalse(SomeInt.IsSomeAnd(x => x == 200));

        Assert.IsFalse(NoneInt.IsSomeAnd(x => x == 100));
        Assert.IsFalse(NoneInt.IsSomeAnd(x => x == 200));
    }

    [TestMethod]
    public void TestUnwrap()
    {
        Assert.AreEqual(100, SomeInt.Unwrap());
        Assert.ThrowsException<InvalidCastException>(() => NoneInt.Unwrap());
    }

    [TestMethod]
    public void TestUnwrapOr()
    {
        Assert.AreEqual(100, SomeInt.UnwrapOr(200));
        Assert.AreEqual(200, NoneInt.UnwrapOr(200));
    }

    [TestMethod]
    public void TestUnwrapOrDefault()
    {
        Assert.AreEqual(100, SomeInt.UnwrapOrDefault());
        Assert.AreEqual(0, NoneInt.UnwrapOrDefault());
    }

    [TestMethod]
    public void TestUnwrapOrElse()
    {
        Assert.AreEqual(100, SomeInt.UnwrapOrElse(() => 200));
        Assert.AreEqual(200, NoneInt.UnwrapOrElse(() => 200));
    }

    [TestMethod]
    public void TestAnd()
    {
        var someOption = Some(200);
        var noneOption = None<int>();

        Assert.IsTrue(SomeInt.And(someOption).IsSome());
        Assert.AreEqual(200, SomeInt.And(someOption).Unwrap());

        Assert.IsTrue(NoneInt.And(someOption).IsNone());
        Assert.IsTrue(SomeInt.And(noneOption).IsNone());
    }

    [TestMethod]
    public void TestAndThen()
    {
        var result = SomeInt.AndThen(x => Some(x + 50));
        Assert.IsTrue(result.IsSome());
        Assert.AreEqual(150, result.Unwrap());

        var noneResult = NoneInt.AndThen(x => Some(x + 50));
        Assert.IsTrue(noneResult.IsNone());
    }

    [TestMethod]
    public void TestIsNoneOr()
    {
        Assert.IsTrue(SomeInt.IsNoneOr(x => x == 100));
        Assert.IsFalse(SomeInt.IsNoneOr(x => x != 100));

        Assert.IsTrue(NoneInt.IsNoneOr(x => x == 100));
        Assert.IsTrue(NoneInt.IsNoneOr(x => x != 100));
    }

    [TestMethod]
    public void TestOr()
    {
        var someOption = Some(200);
        var noneOption = None<int>();

        Assert.IsTrue(SomeInt.Or(someOption).IsSome());
        Assert.AreEqual(100, SomeInt.Or(someOption).Unwrap());

        Assert.IsTrue(NoneInt.Or(someOption).IsSome());
        Assert.AreEqual(200, NoneInt.Or(someOption).Unwrap());

        Assert.IsTrue(SomeInt.Or(noneOption).IsSome());
        Assert.AreEqual(100, SomeInt.Or(noneOption).Unwrap());

        Assert.IsTrue(NoneInt.Or(noneOption).IsNone());
    }

    [TestMethod]
    public void TestOrElse()
    {
        var someOption = Some(200);
        var noneOption = None<int>();

        Assert.IsTrue(SomeInt.OrElse(() => someOption).IsSome());
        Assert.AreEqual(100, SomeInt.OrElse(() => someOption).Unwrap());

        Assert.IsTrue(NoneInt.OrElse(() => someOption).IsSome());
        Assert.AreEqual(200, NoneInt.OrElse(() => someOption).Unwrap());

        Assert.IsTrue(SomeInt.OrElse(() => noneOption).IsSome());
        Assert.AreEqual(100, SomeInt.OrElse(() => noneOption).Unwrap());

        Assert.IsTrue(NoneInt.OrElse(() => noneOption).IsNone());
    }

    [TestMethod]
    public void TestMap()
    {
        var mappedSome = SomeInt.Map(x => x.ToString());
        Assert.IsTrue(mappedSome.IsSome());
        Assert.AreEqual("100", mappedSome.Unwrap());

        var mappedNone = NoneInt.Map(x => x.ToString());
        Assert.IsTrue(mappedNone.IsNone());
    }

    [TestMethod]
    public void TestMapOr()
    {
        var mappedSome = SomeInt.MapOr("default", x => x.ToString());
        Assert.AreEqual("100", mappedSome);

        var mappedNone = NoneInt.MapOr("default", x => x.ToString());
        Assert.AreEqual("default", mappedNone);
    }

    [TestMethod]
    public void TestMapOrElse()
    {
        var mappedSome = SomeInt.MapOrElse(() => "default", x => x.ToString());
        Assert.AreEqual("100", mappedSome);

        var mappedNone = NoneInt.MapOrElse(() => "default", x => x.ToString());
        Assert.AreEqual("default", mappedNone);
    }

    [TestMethod]
    public void TestInspect()
    {
        bool flag = false;
        SomeInt.Inspect(x => flag = true);
        Assert.IsTrue(flag);

        flag = false;
        NoneInt.Inspect(x => flag = true);
        Assert.IsFalse(flag);
    }

    [TestMethod]
    public void TestExpect()
    {
        Assert.AreEqual(100, SomeInt.Expect("Should not throw"));
        Assert.ThrowsException<InvalidOperationException>(() => NoneInt.Expect("Should throw"));
    }
}
