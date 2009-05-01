using NUnit.Framework;

namespace Fizzler.Tests
{
	[TestFixture]
	public class HumanReadableSelectorGeneratorTests
	{
		[Test]
		public void All_Nodes()
		{
			Run("*", "Select all elements.");
		}

		[Test]
		public void Tag()
		{
			Run("p", "Select all elements with the <p> tag.");
		}

		[Test]
		public void Descendant()
		{
			Run("p a", "Select all elements with the <p> tag which have descendants with the <a> tag.");
		}

		[Test]
		public void Three_Levels_Of_Descendant()
		{
			Run("p a img", "Select all elements with the <p> tag which have descendants with the <a> tag, which in turn have descendants with the <img> tag.");
		}

		[Test]
		public void Attribute()
		{
			Run("a[href]", "Select all elements with the <a> tag which have a href attribute.");
		}

		[Test]
		public void Adjacent()
		{
			Run("a + span", "Select all elements with the <a> tag which is immediately preceeded by a sibling node with the <span> tag.");
		}

		[Test]
		public void Id()
		{
			Run("#nodeId", "Select all elements with an id of 'nodeId'.");
		}

		[Test]
		public void SelectorGroup()
		{
			Run("a, span", "Select all elements with the <a> tag, then combined with previous, select all elements with the <span> tag.");
		}

		private static void Run(string selector, string message)
		{
		    var generator = new HumanReadableSelectorGenerator();
			Parser.Parse(selector, generator);
			Assert.AreEqual(message, generator.Text);
		}
	}
}