using System;
using System.Collections.Generic;
using System.Linq;

namespace Fizzler
{
    /// <summary>
    /// A selector generator implementation for an arbitrary document/node system.
    /// </summary>
    public class SelectorGenerator<TNode> : ISelectorGenerator
    {
        private readonly IEqualityComparer<TNode> _equalityComparer;
        private readonly Stack<Selector<TNode>> _selectors;

        /// <summary>
        /// Initializes a new instance of this object with an instance
        /// of <see cref="INodeOps{TNode}"/> and the default equality
        /// comparer that is used for determining if two nodes are equal.
        /// </summary>
        public SelectorGenerator(INodeOps<TNode> ops) : this(ops, null) {}

        /// <summary>
        /// Initializes a new instance of this object with an instance
        /// of <see cref="INodeOps{TNode}"/> and an equality comparer
        /// used for determining if two nodes are equal.
        /// </summary>
        public SelectorGenerator(INodeOps<TNode> ops, IEqualityComparer<TNode> equalityComparer)
        {
            if(ops == null) throw new ArgumentNullException("ops");
            Ops = ops;
            _equalityComparer = equalityComparer ?? EqualityComparer<TNode>.Default;
            _selectors = new Stack<Selector<TNode>>();
        }

        /// <summary>
        /// Gets the selector implementation.
        /// </summary>
        /// <remarks>
        /// If the generation is not complete, this property returns the 
        /// last generated selector.
        /// </remarks>
        public Selector<TNode> Selector { get; private set; }

        /// <summary>
        /// Gets the <see cref="INodeOps{TNode}"/> instance that this object
        /// was initialized with.
        /// </summary>
        public INodeOps<TNode> Ops { get; private set; }

        /// <summary>
        /// Returns the collection of selector implementations representing 
        /// a group.
        /// </summary>
        /// <remarks>
        /// If the generation is not complete, this method return the 
        /// selectors generated so far in a group.
        /// </remarks>
        public IEnumerable<Selector<TNode>> GetSelectors()
        {
            var selectors = _selectors;
            var top = Selector;
            return top == null 
                 ? selectors.Select(s => s) 
                 : selectors.Concat(Enumerable.Repeat(top, 1));
        }

        /// <summary>
        /// Adds a generated selector.
        /// </summary>
        protected void Add(Selector<TNode> selector)
        {
            if(selector == null) throw new ArgumentNullException("selector");
            
            var top = Selector;
            Selector = top == null ? selector : (nodes => selector(top(nodes)));
        }

        /// <summary>
        /// Delimits the initialization of a generation.
        /// </summary>
        public virtual void OnInit()
        {
            _selectors.Clear();
            Selector = null;
        }

        /// <summary>
        /// Delimits a selector generation in a group of selectors.
        /// </summary>
        public virtual void OnSelector()
        {
            if (Selector != null)
                _selectors.Push(Selector);
            Selector = null;
        }

        /// <summary>
        /// Delimits the closing/conclusion of a generation.
        /// </summary>
        public virtual void OnClose()
        {
            var sum = GetSelectors().Aggregate((a, b) => (nodes => a(nodes).Concat(b(nodes))));
            var normalize = Ops.Descendant();
            Selector = nodes => sum(normalize(nodes)).Distinct(_equalityComparer);
            _selectors.Clear();
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#Id-selectors">ID selector</a>,
        /// which represents an element instance that has an identifier that 
        /// matches the identifier in the ID selector.
        /// </summary>
        public virtual void Id(string id)
        {
            Add(Ops.Id(id));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#class-html">class selector</a>,
        /// which is an alternative <see cref="ISelectorGenerator.AttributeIncludes"/> when 
        /// representing the <c>class</c> attribute. 
        /// </summary>
        public virtual void Class(string clazz)
        {
            Add(Ops.Class(clazz));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#type-selectors">type selector</a>,
        /// which represents an instance of the element type in the document tree. 
        /// </summary>
        public virtual void Type(string type)
        {
            Add(Ops.Type(type));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#universal-selector">universal selector</a>,
        /// any single element in the document tree in any namespace 
        /// (including those without a namespace) if no default namespace 
        /// has been specified for selectors. 
        /// </summary>
        public virtual void Universal()
        {
            Add(Ops.Universal());
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>
        /// whatever the values of the attribute.
        /// </summary>
        public virtual void AttributeExists(string name)
        {
            Add(Ops.AttributeExists(name));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>
        /// and whose value is exactly <paramref name="value"/>.
        /// </summary>
        public virtual void AttributeExact(string name, string value)
        {
            Add(Ops.AttributeExact(name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>
        /// and whose value is a whitespace-separated list of words, one of 
        /// which is exactly <paramref name="value"/>.
        /// </summary>
        public virtual void AttributeIncludes(string name, string value)
        {
            Add(Ops.AttributeIncludes(name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>,
        /// its value either being exactly <paramref name="value"/> or beginning 
        /// with <paramref name="value"/> immediately followed by "-" (U+002D).
        /// </summary>
        public virtual void AttributeDashMatch(string name, string value)
        {
            Add(Ops.AttributeDashMatch(name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value begins with the prefix <paramref name="value"/>.
        /// </summary>
        public void AttributePrefixMatch(string name, string value)
        {
            Add(Ops.AttributePrefixMatch(name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value ends with the suffix <paramref name="value"/>.
        /// </summary>
        public void AttributeSuffixMatch(string name, string value)
        {
            Add(Ops.AttributeSuffixMatch(name, value));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that is the first child of some other element.
        /// </summary>
        public virtual void FirstChild()
        {
            Add(Ops.FirstChild());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that is the last child of some other element.
        /// </summary>
        public virtual void LastChild()
        {
            Add(Ops.LastChild());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that is the N-th child of some other element.
        /// </summary>
        public virtual void NthChild(int position)
        {
            Add(Ops.NthChild(position));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an elementthat has a parent element and whose parent 
        /// element has no other element children.
        /// </summary>
        public virtual void OnlyChild()
        {
            Add(Ops.OnlyChild());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that has no children at all.
        /// </summary>
        public virtual void Empty()
        {
            Add(Ops.Empty());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents a childhood relationship between two elements.
        /// </summary>
        public virtual void Child()
        {
            Add(Ops.Child());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents a relationship between two elements where one element is an 
        /// arbitrary descendant of some ancestor element.
        /// </summary>
        public virtual void Descendant()
        {
            Add(Ops.Descendant());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents elements that share the same parent in the document tree and 
        /// where the first element immediately precedes the second element.
        /// </summary>
        public virtual void Adjacent()
        {
            Add(Ops.Adjacent());
        }
    }
}