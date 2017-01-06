namespace CsDoc.Model
{
    /// <summary>
    /// Represents a documented type.
    /// </summary>
    public class TypeDocInfo
    {
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Get the documented description of the type.
        /// </summary>
        public string Description { get; internal set; }
    }
}