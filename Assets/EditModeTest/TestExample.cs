using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class TestExample
{

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Debug.Log("OneTimeSetUp");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Debug.Log("OneTimeTearDown");
    }

    [SetUp]
    public void SetUp()
    {
        Debug.Log("SetUp");
    }

    [TearDown]
    public void TearDown()
    {
        Debug.Log("TearDown");
    }

    [Test]
    [Author("Cliff Lee CL")]
    [Category("TestCategory")]
    public void TestCategoryAndAuthor()
    {

    }

    [Test]
    [TestCase(12, 3)]
    [TestCase(5, 8)]
    public void TestTestCase(int x, int y)
    {
        Assert.That(x, Is.GreaterThan(4));
        Assert.That(y, Is.LessThanOrEqualTo(8));
    }

    [Test]
    [TestCase(12, 3, ExpectedResult = 15)]
    [TestCase(5, 8, ExpectedResult = 13)]
    public int TestTestCaseWithExpectedResult(int x, int y)
    {
        return x + y;
    }

    [Test]
    public void TestCombinatorial([Values(1, 2, 3)] int x, [Values("+", "-")] string y)
    {

    }

    [Test]
    public void TestRandom([Random(1, 10, 4)] int x, [Values("+", "-")] string y)
    {

    }

    [Test]
    public void TestRandom([Random(5)] int x)
    {
        Assert.That(x, Is.GreaterThan(0));
    }

    [Test]
    public void TestRange([NUnit.Framework.Range(0.0f, 1.0f, 0.25f)] float x, [Values("+", "-")] string y)
    {

    }

    /* This won't let the test pass.
    [Test]
    [Ignore("This is ignored.")]
    public void TestIgnore()
    {

    }
    */
}
