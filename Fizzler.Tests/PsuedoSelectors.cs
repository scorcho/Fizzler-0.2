using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class PsuedoSelectors : SelectorBaseTest
	{
		[TestMethod]public void First_Child()
		{
			Assert.AreEqual(7, Parser.Parse("*:first-child").Count);
			Assert.AreEqual(1, Parser.Parse("p:first-child").Count);
		}
		
		[TestMethod]public void Last_Child()
		{
			Assert.AreEqual(6, Parser.Parse("*:last-child").Count);
			Assert.AreEqual(2, Parser.Parse("p:last-child").Count);
		}
		
		[TestMethod]public void Only_Child()
		{
			Assert.AreEqual(2, Parser.Parse("*:only-child").Count);
			Assert.AreEqual(1, Parser.Parse("p:only-child").Count);
		}
		[TestMethod]public void Empty()
		{
			var results = Parser.Parse("*:empty");
			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("head", results[0].Name);
		}
	}
}