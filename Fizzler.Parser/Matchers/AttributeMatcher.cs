using System;
using System.Collections.Generic;
using Fizzler.Parser.ChunkHandling;
using Fizzler.Parser.Document;

namespace Fizzler.Parser.Matchers
{
	/// <summary>
	/// Attribute matcher.
	/// </summary>
	public class AttributeMatcher
	{
		/// <summary>
		/// Check if a node matches the given attribute selector.
		/// </summary>
		/// <param name="attributeSelectorData"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool Match(AttributeSelectorData attributeSelectorData, IDocumentNode node)
		{
			if(attributeSelectorData != null)
			{
				if(attributeSelectorData.Comparison == AttributeComparator.Unknown)
				{
					return node.Attributes[attributeSelectorData.Attribute] == null;
				}
				if(attributeSelectorData.Comparison == AttributeComparator.Exact)
				{
					IAttribute attribute = node.Attributes[attributeSelectorData.Attribute];

					if(attribute == null)
						return false;

					return attribute.Value == attributeSelectorData.Value;
				}

				if(attributeSelectorData.Comparison == AttributeComparator.SpaceSeparated)
				{
					IAttribute attribute = node.Attributes[attributeSelectorData.Attribute];

					if(attribute == null)
						return false;

					List<string> strings = new List<string>(attribute.Value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

					return strings.Contains(attributeSelectorData.Value);
				}

				if(attributeSelectorData.Comparison == AttributeComparator.HyphenSeparated)
				{
					IAttribute attribute = node.Attributes[attributeSelectorData.Attribute];

					if(attribute == null)
						return false;

					List<string> strings = new List<string>(attribute.Value.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

					return strings.Contains(attributeSelectorData.Value);
				}
			}

			return true;
		}
	}
}