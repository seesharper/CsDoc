namespace CsDoc.Rendering
{
    using Model;

    public class MarkdownRenderer : IRenderer
    {
        public string RenderDocumentation(Documentation documentation)
        {
            return null;
        }

        public string RenderParamRef(string reference)
        {
            return reference;
        }

        public string RenderList(ListDocInfo listDocInfo)
        {
            return "list";
        }
    }
}