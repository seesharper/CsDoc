namespace CsDoc.Walkers
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Model;
    using Rendering;

    public class DocumentationWalker<TDocInfo> : CSharpSyntaxWalker where TDocInfo : MemberDocInfo
    {
        protected TDocInfo docInfo;
        private readonly ContentWalker contentWalker;

        public DocumentationWalker(IRenderer renderer) : base(SyntaxWalkerDepth.Node)
        {
            contentWalker = new ContentWalker(renderer);
        }

        public void ApplyDocumentation(SyntaxNode documentationTrivia, TDocInfo docInfo)
        {
            this.docInfo = docInfo;
            Visit(documentationTrivia);
        }

        public override void VisitXmlElement(XmlElementSyntax node)
        {
            if (node.StartTag.Name.LocalName.Text == "summary")
            {
                docInfo.Summary = contentWalker.Render(node.Content);
            }

            if (node.StartTag.Name.LocalName.Text == "remarks")
            {
                docInfo.Remarks = contentWalker.Render(node.Content);
            }

            if (node.StartTag.Name.LocalName.Text == "typeparam")
            {
                var parameterName = node.Find<XmlNameAttributeSyntax>(n => n.Name.LocalName.Text == "name").FirstOrDefault()?.Identifier.ToString();
                var paramDocInfo = docInfo.TypeParameters.FirstOrDefault(p => p.Name == parameterName);
                if (paramDocInfo != null)
                {
                    paramDocInfo.Description = contentWalker.Render(node.Content);
                }               
            }


            if (node.StartTag.Name.LocalName.Text == "example")
            {
                var codeTag =
                    node.Find<XmlElementSyntax>(e => e.StartTag.Name.LocalName.Text == "code").FirstOrDefault();
                string code = null;
                string description = null;
                if (codeTag != null)
                {
                    code = contentWalker.Render(codeTag.Content);
                    description = contentWalker.Render(node.Content.Except(new[] { codeTag }));
                }
                else
                {
                    description = contentWalker.Render(node.Content);
                }

                var exampleDoc = new ExampleDocInfo();
                exampleDoc.Description = description.Trim();
                exampleDoc.Code = code;
                docInfo.Examples.Add(exampleDoc);
            }


            if (node.StartTag.Name.LocalName.Text == "exception")
            {
                var crefAttribute = node.StartTag.Find<XmlCrefAttributeSyntax>(t => true).FirstOrDefault();
                var exceptionType = crefAttribute?.Cref.ToString();
                var descrption = contentWalker.Render(node.Content).Trim();
                var exceptionDocInfo = new ExceptionDocInfo() { ThrownWhen = descrption, Type = exceptionType };
                docInfo.Exceptions.Add(exceptionDocInfo);
            }

            base.VisitXmlElement(node);
        }
    }
}