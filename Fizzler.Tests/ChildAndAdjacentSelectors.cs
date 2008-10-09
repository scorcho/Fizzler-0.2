using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fizzler.Tests
{
	[TestClass]
	public class ChildAndAdjacentSelectors : SelectorBaseTest
	{	
		[TestMethod]
		public void Child_With_Pre_And_Post_Space()
		{
			Assert.AreEqual(2, Parser.Parse("div > p").Count);
		}

		[TestMethod]
		public void Child_With_Post_Space()
		{
			Assert.AreEqual(2, Parser.Parse("div> p").Count);
		}

		[TestMethod]
		public void Child_With_Pre_Space()
		{
			Assert.AreEqual(2, Parser.Parse("div >p").Count);
		}

		[TestMethod]
		public void Child_With_No_Space()
		{
			Assert.AreEqual(2, Parser.Parse("div>p").Count);
		}

		[TestMethod]
		public void Child_With_Class()
		{
			Assert.AreEqual(1, Parser.Parse("div > p.ohyeah").Count);
		}

		[TestMethod]
		public void All_Children()
		{
			// match <a href="">hi</a><span>test</span> so that's 2
			Assert.AreEqual(2, Parser.Parse("p > *").Count);
		}

		[TestMethod]
		public void All_GrandChildren()
		{
			// match <a href="">hi</a><span>test</span> so that's 3
			// *any* second level children under any div
			Assert.AreEqual(3, Parser.Parse("div > * > *").Count);
		}

		[TestMethod]
		public void Adjacent_With_Pre_And_Post_Space()
		{
			Assert.AreEqual(2, Parser.Parse("a + span").Count, "Adjacent not supported yet.");
		}

		[TestMethod]
		public void Adjacent_With_Post_Space()
		{
			Assert.AreEqual(2, Parser.Parse("a+ span").Count, "Adjacent not supported yet.");
		}

		[TestMethod]
		public void Adjacent_With_Pre_Space()
		{
			Assert.AreEqual(2, Parser.Parse("a +span").Count, "Adjacent not supported yet.");
		}

		[TestMethod]
		public void Adjacent_With_No_Space()
		{
			Assert.AreEqual(2, Parser.Parse("a+span").Count, "Adjacent not supported yet.");
		}

		[TestMethod]
		public void Comma_Child_And_Adjacent()
		{
			Assert.AreEqual(4, Parser.Parse("a + span, div > p").Count, "Adjacent not supported yet.");
		}
	}	
}