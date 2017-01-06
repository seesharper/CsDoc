namespace CsDoc
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    public static class SyntaxTokensExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntaxTokens"></param>
        /// <returns></returns>
        public static bool IsPublic(this IEnumerable<SyntaxToken> syntaxTokens)
        {
            return !syntaxTokens.Any(m => m.Text == "private" || m.Text == "internal" || m.Text == "protected");
        }
    }
}