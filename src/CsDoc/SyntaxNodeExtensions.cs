namespace CsDoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public static class SyntaxNodeExtensions
    {
        /// <summary>
        /// Returns a list of <typeparamref name="TSyntaxNode"/> instances
        /// that matches the <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TSyntaxNode">The type of <see cref="TSyntaxNode"/>
        /// to search for.</typeparam>
        /// <param name="expression">The <see cref="Expression"/> that represents the sub tree for which to start searching.</param>
        /// <param name="predicate">The <see cref="Func{TResult}"/> used to filter the result</param>
        /// <returns>A list of <see cref="TSyntaxNode"/> instances that matches the given predicate.</returns>
        public static IEnumerable<TSyntaxNode> Find<TSyntaxNode>(this SyntaxNode expression, Func<TSyntaxNode, bool> predicate) where TSyntaxNode : SyntaxNode
        {
            var finder = new SyntaxNodeFinder<TSyntaxNode>();
            return finder.Find(expression, predicate);
        }

        /// <summary>
        /// A class used to search for <see cref="Expression"/> instances. 
        /// </summary>
        /// <typeparam name="TSyntaxNode">The type of <see cref="SyntaxNode"/> to search for.</typeparam>
        private class SyntaxNodeFinder<TSyntaxNode> : CSharpSyntaxWalker where TSyntaxNode : SyntaxNode
        {
            public SyntaxNodeFinder(SyntaxWalkerDepth depth = SyntaxWalkerDepth.StructuredTrivia) : base(depth)
            {
            }

            private readonly IList<TSyntaxNode> _result = new List<TSyntaxNode>();
            private Func<TSyntaxNode, bool> _predicate;

            /// <summary>
            /// Returns a list of <typeparamref name="TExpression"/> instances that matches the <paramref name="predicate"/>.
            /// </summary>
            /// <param name="expression">The <see cref="Expression"/> that represents the sub tree for which to start searching.</param>
            /// <param name="predicate">The <see cref="Func{T,TResult}"/> used to filter the result</param>
            /// <returns>A list of <see cref="Expression"/> instances that matches the given predicate.</returns>
            public IEnumerable<TSyntaxNode> Find(SyntaxNode expression, Func<TSyntaxNode, bool> predicate)
            {
                _result.Clear();
                _predicate = predicate;
                Visit(expression);
                return _result;
            }

            /// <summary>
            /// Visits each node of the <see cref="SyntaxNode"/> tree checks 
            /// if the current expression matches the predicate.         
            /// </summary>
            /// <param name="node">The <see cref="SyntaxNode"/> currently being visited.</param>
            /// <returns><see cref="Expression"/></returns>
            public override void Visit(SyntaxNode node)
            {
                if (IsTargetExpressionType(node) && MatchesExpressionPredicate(node))
                    _result.Add((TSyntaxNode)node);
                base.Visit(node);
            }

            private bool MatchesExpressionPredicate(SyntaxNode node)
            {
                return _predicate((TSyntaxNode)node);
            }

            private static bool IsTargetExpressionType(SyntaxNode node)
            {
                return node is TSyntaxNode;
            }
        }
    }
}