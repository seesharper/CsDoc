namespace CsDoc.Walkers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Model;
    using Rendering;

    /// <summary>
    /// Visits the nodeContent of a <see cref="XmlNodeSyntax"/>.
    /// </summary>
    public class ContentWalker : CSharpSyntaxWalker
    {
        private readonly IRenderer renderer;
        private readonly StringBuilder content = new StringBuilder();

        /// <summary>
        /// INitializes a new instance of the <see cref="ContentWalker"/> class.
        /// </summary>
        /// <param name="renderer">The <see cref="IRenderer"/> that is responsible 
        /// for rendering the documentation.</param>
        public ContentWalker(IRenderer renderer) : base(SyntaxWalkerDepth.StructuredTrivia)
        {
            this.renderer = renderer;
        }

        /// <summary>
        /// Renders the <paramref name="nodeContent"/> into a string.
        /// </summary>
        /// <param name="nodeContent">The content to be rendered.</param>
        /// <returns>A string representation of the <paramref name="nodeContent"/>.</returns>
        public string Render(IEnumerable<XmlNodeSyntax> nodeContent)
        {
            content.Clear();
            foreach (var xmlNodeSyntax in nodeContent)
            {
                Visit(xmlNodeSyntax);
            }
            
            var trimmed = content.ToString().Trim();
            var replaced = Regex.Replace(trimmed, "(^ )", string.Empty, RegexOptions.Multiline);
            return replaced;
        }

        /// <summary>
        /// Called when the visitor visits a XmlTextSyntax node.
        /// </summary>
        public override void VisitXmlText(XmlTextSyntax node)
        {
            base.VisitXmlText(node);
            var textTokens = node.TextTokens.ToArray();
            foreach (var textToken in textTokens)
            {
                content.Append(textToken.Text);
            }
        }

        /// <summary>
        /// Called when the visitor visits a XmlCrefAttributeSyntax node.
        /// </summary>
        public override void VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
        {
            base.VisitXmlCrefAttribute(node);
            content.Append(renderer.RenderParamRef(node.Cref.ToString()));
        }

        /// <summary>
        /// Called when the visitor visits a XmlNameAttributeSyntax node.
        /// </summary>
        public override void VisitXmlNameAttribute(XmlNameAttributeSyntax node)
        {
            content.Append(renderer.RenderParamRef(node.Identifier.ToString()));
            base.VisitXmlNameAttribute(node);
        }


        public override void VisitXmlCDataSection(XmlCDataSectionSyntax node)
        {
            base.VisitXmlCDataSection(node);
            var textTokens = node.TextTokens.ToArray();
            foreach (var textToken in textTokens)
            {
                content.Append(textToken.Text);
            }
        }


        public override void VisitXmlElement(XmlElementSyntax node)
        {
            if (node.StartTag.Name.LocalName.Text == "list")
            {

                var listContentWalker = new ListWalker(renderer);

                var listType =
                    node.Find<XmlTextAttributeSyntax>(n => n.Name.LocalName.Text == "type")
                        .FirstOrDefault()?
                        .TextTokens.ToString();
                if (listType == null)
                {
                    listType = "bullet";
                }
                ListDocInfo listDocInfo = new ListDocInfo();
                listDocInfo.Type = listType;

                var listHeader =
                    node.Find<XmlElementSyntax>(e => e.StartTag.Name.LocalName.Text == "listheader").SingleOrDefault();
                if (listHeader != null)
                {
                    listContentWalker.ApplyDocumentation(listHeader, listDocInfo.Header);
                }

                var items = node.Find<XmlElementSyntax>(e => e.StartTag.Name.LocalName.Text == "item");

                foreach (var item in items)
                {
                    ListItemDocInfo itemDocInfo = new ListItemDocInfo();
                    listContentWalker.ApplyDocumentation(item, itemDocInfo);
                    listDocInfo.Items.Add(itemDocInfo);
                }

                renderer.RenderList(listDocInfo);

            }
            else
            {
                base.VisitXmlElement(node);
            }
            
        }
    }
}