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

    [TestMethod]
    public void TestInsert()
    {
        var someInt = Some(100);
        var noneInt = None<int>();
        var someIntInsert = someInt.Insert(200);
        var noneIntInsert = noneInt.Insert(200);

        Assert.AreEqual(200, someIntInsert);
        Assert.AreEqual(200, noneIntInsert);
        Assert.AreEqual(200, someInt.Unwrap());
        Assert.AreEqual(200, noneInt.Unwrap());
    }

    [TestMethod]
    public void TestGetOrInsert()
    {
        var someInt = Some(100);
        var noneInt = None<int>();
        var someIntValue = someInt.GetOrInsert(200);
        var noneIntValue = noneInt.GetOrInsert(200);

        Assert.AreEqual(100, someIntValue);
        Assert.AreEqual(200, noneIntValue);
        Assert.AreEqual(100, someInt.Unwrap());
        Assert.AreEqual(200, noneInt.Unwrap());
    }

    [TestMethod]
    public void TestGetOrInsertDefault()
    {
        var someInt = Some(100);
        var noneInt = None<int>();
        var someIntValue = someInt.GetOrInsertDefault();
        var noneIntValue = noneInt.GetOrInsertDefault();

        Assert.AreEqual(100, someIntValue);
        Assert.AreEqual(0, noneIntValue);
        Assert.AreEqual(100, someInt.Unwrap());
        Assert.AreEqual(0, noneInt.Unwrap());
    }

    [TestMethod]
    public void TestGetOrInsertWith()
    {
        var someInt = Some(100);
        var noneInt = None<int>();
        var someIntGetOrInsertWith = someInt.GetOrInsertWith(() => 200);
        var noneIntGetOrInsertWith = noneInt.GetOrInsertWith(() => 200);

        Assert.AreEqual(100, someIntGetOrInsertWith);
        Assert.AreEqual(200, noneIntGetOrInsertWith);
        Assert.AreEqual(100, someInt.Unwrap());
        Assert.AreEqual(200, noneInt.Unwrap());
    }
}
