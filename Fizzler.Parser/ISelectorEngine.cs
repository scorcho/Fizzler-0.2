using System.Collections.Generic;
using Fizzler.Parser.Document;

namespace Fizzler.Parser
{
	/// <summary>
	/// SelectorEngine.
	/// </summary>
	public interface ISelectorEngine
	{
		/// <summary>
		/// Select from the IDocument which was used to initialise the engine.
		/// </summary>
		/// <remarks>Implementors should ensure that their constructor supplies something to select against.</remarks>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		IEnumerable<IDocumentNode> Select(string selectorChain);

		/// <summary>
		/// Select from the passed IDocument.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		IEnumerable<IDocumentNode> Select(IDocumentNode node, string selectorChain);
	}
}