namespace CsDoc.Model
{
    /// <summary>
    /// Represents a documented property.
    /// </summary>
    public class PropertyDocInfo : MemberDocInfo
    {
        /// <summary>
        /// Gets a string that represent the public property accessors.
        /// </summary>
        public string Accessors { get; internal set; }

        /// <summary>
        /// Gets the name of the property type.
        /// </summary>
        public string Type { get; internal set; }
    }
}