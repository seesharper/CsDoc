namespace CsDoc.Model
{
    /// <summary>
    /// Represents a documented method.
    /// </summary>
    public class MethodDocInfo : GenericMemberDocInfo
    {

        /// <summary>
        /// Gets a list of parameters for this method.
        /// </summary>
        public ParameterDocInfo[] Parameters { get; set; }

        /// <summary>
        /// Gets the return type for this method.
        /// </summary>
        public TypeDocInfo ReturnType { get; private set; } = new TypeDocInfo { Description = "N/A" };

    }
}