namespace CsDoc.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the complete documentation for a given set of compilation units.
    /// </summary>
    public class Documentation
    {
        /// <summary>
        /// Gets a list of classes.
        /// </summary>
        public List<ClassDocInfo> Classes { get; private set; } = new List<ClassDocInfo>();

        /// <summary>
        /// Gets a list of "top-level" methods.
        /// </summary>
        public List<MethodDocInfo> Methods { get; private set; } = new List<MethodDocInfo>();
    }
}