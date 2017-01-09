namespace CsDoc.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a documented member.
    /// </summary>
    public class MemberDocInfo
    {
        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the summary section of the documentation.
        /// </summary>
        public string Summary { get; internal set; } = Options.DefaultText;

        /// <summary>
        /// Get the remarks section of the documentation.
        /// </summary>
        public string Remarks { get; internal set; } = Options.DefaultText;

        /// <summary>
        /// Gets a list of modifiers for this member.
        /// </summary>
        public string[] Modifies { get; internal set; }

        /// <summary>
        /// Get a list of examples from the documentation.
        /// </summary>
        /// <seealso cref="ExampleDocInfo"/>
        public List<ExampleDocInfo> Examples { get; private set; } = new List<ExampleDocInfo>();

        /// <summary>
        /// Gets a list of exceptions from the documentation.
        /// </summary>
        /// <seealso cref="ExceptionDocInfo"/>
        public List<ExceptionDocInfo> Exceptions { get; private set; } = new List<ExceptionDocInfo>();

        /// <summary>
        /// Gets a list of documented generic type parameters.
        /// </summary>
        public List<TypeDocInfo> TypeParameters { get; private set; } = new List<TypeDocInfo>();
    }
}