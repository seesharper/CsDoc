namespace CsDoc.Model
{
    /// <summary>
    /// Represents a documented example. 
    /// </summary>
    public class ExampleDocInfo
    {
        /// <summary>
        /// Gets the description of the example.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets the code section of the example.
        /// </summary>
        public string Code { get; set; }
    }
}