namespace CsDoc.Model
{
    /// <summary>
    /// Represent a documented parameter
    /// </summary>
    public class ParameterDocInfo
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the parameter type name.
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// Gets the default value of the parameter.
        /// </summary>
        public string DefaultValue { get; internal set; }

        /// <summary>
        /// Gets the description of the parameter.
        /// </summary>
        public string Description { get; internal set; } = Options.DefaultText;
    }
}