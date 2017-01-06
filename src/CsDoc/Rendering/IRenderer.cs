namespace CsDoc.Rendering
{
    using Model;

    /// <summary>
    /// Represents a class that is capable of rendering an <see cref="Documentation"/>.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Renders the given <paramref name="documentation"/> into a string.
        /// </summary>
        /// <param name="documentation">The <see cref="Documentation"/> to be rendered.</param>
        /// <returns>A string that represents the rendered <paramref name="documentation"/>.</returns>
        string RenderDocumentation(Documentation documentation);

        /// <summary>
        /// Renders a parameter reference.
        /// </summary>
        /// <param name="reference">The name of the reference to render.</param>
        /// <returns>A string that represents the rendered parameter reference.</returns>
        string RenderParamRef(string reference);

        /// <summary>
        /// Renders a list.
        /// </summary>
        /// <param name="listDocInfo">The <see cref="ListDocInfo"/> to be rendered.</param>
        /// <returns>A string that represent the rendered list.</returns>
        string RenderList(ListDocInfo listDocInfo);
    }
}