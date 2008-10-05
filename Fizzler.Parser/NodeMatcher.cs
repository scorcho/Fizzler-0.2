using System;
using System.Collections.Generic;
using Fizzler.Parser.ChunkHandling;
using Fizzler.Parser.Extensions;
using HtmlAgilityPack;

namespace Fizzler.Parser
{
	public class NodeMatcher
	{
		public bool IsMatch(HtmlNode node, Chunk chunk, Chunk previousChunk)
		{
			bool match = false;

			if (node.NodeType != HtmlNodeType.Element)
				return false;

			if (chunk.ChunkType == ChunkType.Star)
			{
				if (previousChunk != null)
				{
					// are any parent nodes affected by the previous chunk?
					var parent = node.ParentNode;

					while (parent != null)
					{
						match = IsMatch(parent, previousChunk, null);

						if (match)
						{
							break;
						}

						parent = parent.ParentNode;
					}
				}
				else
				{
					match = true;
				}
			}

			if (chunk.ChunkType == ChunkType.TagName)
			{
				if (node.Name == chunk.Body)
				{
					if (previousChunk != null)
					{
						match = IsMatch(node.ParentNode, previousChunk, null);
					}
					else
					{
						match = true;
					}
				}
			}

			if (chunk.ChunkType == ChunkType.Id)
			{
				if (node.Attributes["id"] != null)
				{
					string idValue = node.Attributes["id"].Value;
					string[] chunkParts = chunk.Body.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

					// if length is greater than one, we could have an id selector with element
					if (chunkParts.Length > 1)
					{
						if (node.Name == chunkParts[0] && chunkParts[1] == idValue)
						{
							if (previousChunk != null)
							{
								match = IsMatch(node.ParentNode, previousChunk, null);
							}
							else
							{
								match = true;
							}
						}
					}
					else
					{
						if (chunkParts[0] == idValue)
						{
							if (previousChunk != null)
							{
								match = IsMatch(node.ParentNode, previousChunk, null);
							}
							else
							{
								match = true;
							}
						}
					}
				}
			}

			if (chunk.ChunkType == ChunkType.Class)
				match = MatchClass(node, chunk, previousChunk);

			return match;
		}

		private bool MatchClass(HtmlNode node, Chunk chunk, Chunk previousChunk)
		{
			bool match = false;
		
			if (node.Attributes["class"] != null)
			{
				List<string> idValues = new List<string>(node.Attributes["class"].Value.Split(" ".ToCharArray()));
				List<string> chunkParts = new List<string>(chunk.Body.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

				// if length is greater than one, we could have an id selector with element
				if (chunkParts.Count > 1)
				{
					if(chunkParts.ContainsAll(idValues))
					{
						match = true;
					}
					else if (node.Name == chunkParts[0] && idValues.Contains(chunkParts[1]))
					{
						if (previousChunk != null)
						{
							match = IsMatch(node.ParentNode, previousChunk, null);
						}
						else
						{
							match = true;
						}
					}
				}
				else
				{
					if (idValues.Contains(chunkParts[0]))
					{
						if (previousChunk != null)
						{
							// are any parent nodes affected by the previous chunk?
							var parent = node.ParentNode;

							while (parent != null)
							{
								match = IsMatch(parent, previousChunk, null);

								if (match)
								{
									break;
								}

								parent = parent.ParentNode;
							}
						}
						else
						{
							match = true;
						}
					}
				}
			}

			return match;
		}
	}
}