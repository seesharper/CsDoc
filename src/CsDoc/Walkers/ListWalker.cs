namespace CsDoc.Walkers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Model;
    using Rendering;

    public class ListWalker : CSharpSyntaxWalker
    {
        private ListItemDocInfo listItemDocInfo;
        private readonly ContentWalker contentWalker;

        public ListWalker(IRenderer renderer) : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            contentWalker = new ContentWalker(renderer);
        }

        public void ApplyDocumentation(XmlElementSyntax element, ListItemDocInfo listItemDocInfo)
        {
            this.listItemDocInfo = listItemDocInfo;
            Visit(element);
        }

        public override void VisitXmlElement(XmlElementSyntax node)
        {
            if (node.StartTag.Name.LocalName.Text == "term")
            {
                listItemDocInfo.Term = contentWalker.Render(node.Content);
            }
            if (node.StartTag.Name.LocalName.Text == "description")
            {
                listItemDocInfo.Description = contentWalker.Render(node.Content);
            }

            base.VisitXmlElement(node);
        }
    }
}