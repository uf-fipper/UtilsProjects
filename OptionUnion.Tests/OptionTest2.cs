using OptionUnion.OptionExtensions;

namespace OptionUnion.Tests;

[TestClass]
public class OptionTest2
{
    [TestMethod]
    public void TestToNullable()
    {
        var someInt = Some(42);
        var noneInt = None<int>();

        Assert.AreEqual(42, someInt.ToNullable());
        Assert.IsNull(noneInt.ToNullable());
    }

    [TestMethod]
    public void TestToOption()
    {
        int? nullableInt = 42;
        int? nullInt = null;

        var optionFromSome = nullableInt.ToOption();
        var optionFromNone = nullInt.ToOption();

        Assert.IsTrue(optionFromSome.IsSome());
        Assert.AreEqual(42, optionFromSome.Unwrap());

        Assert.IsTrue(optionFromNone.IsNone());
    }
}
