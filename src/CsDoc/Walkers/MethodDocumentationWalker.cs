namespace CsDoc.Walkers
{
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Model;
    using Rendering;

    public class MethodDocumentationWalker : DocumentationWalker<MethodDocInfo>
    {
        private readonly ContentWalker contentWalker;

        public MethodDocumentationWalker(IRenderer renderer) : base(renderer)
        {
            contentWalker = new ContentWalker(renderer);
        }

        public override void VisitXmlElement(XmlElementSyntax node)
        {
            if (node.StartTag.Name.LocalName.Text == "param")
            {
                var parameterName = node.Find<XmlNameAttributeSyntax>(n => n.Name.LocalName.Text == "name").FirstOrDefault()?.Identifier.ToString();
                var paramDocInfo = docInfo.Parameters.FirstOrDefault(p => p.Name == parameterName);
                if (paramDocInfo != null)
                {
                    paramDocInfo.Description = contentWalker.Render(node.Content);
                }
            }

            if (node.StartTag.Name.LocalName.Text == "returns")
            {
                docInfo.ReturnType.Description = contentWalker.Render(node.Content);
            }


                base.VisitXmlElement(node);
        }
    }
}