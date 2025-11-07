using NUnit.Framework;
using Allure.NUnit.Attributes;
using Allure.NUnit;

namespace YourProject.Tests;

[AllureNUnit]
[AllureSuite("Calculator")]
public class SampleTests
{
    [Test]
    [Category("smoke")]
    [AllureSubSuite("Add")]
    [AllureDescription("Simple addition test")]
    public void Add_Works()
    {
        Assert.That(4.Equals(2 + 2));
	}
	
    [Test]
    [Category("smoke")]
    [AllureSubSuite("Add")]
    [AllureDescription("Simple minus test")]
    public void Minus_Works()
    {
        Assert.That(4.Equals(6 - 2));
	}
	
    [Test]
    [Category("regression")]
    [AllureSubSuite("Divide")]
    public void Divide_ByZero_Throws()
    {
        int a = 1;
		int b = 0;                    // nicht konstante 0
		Assert.Throws<DivideByZeroException>(() => { var _ = a / b; });
	}
}
