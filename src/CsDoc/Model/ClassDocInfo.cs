namespace CsDoc.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a documented class.
    /// </summary>
    public class ClassDocInfo : GenericMemberDocInfo
    {
        /// <summary>
        /// Gets a list of base types.
        /// </summary>
        public string[] BaseTypes { get; internal set; }
      

        /// <summary>
        /// Gets a list of public methods declared by this class.
        /// </summary>
        public List<MethodDocInfo> Methods { get; private set; } = new List<MethodDocInfo>();

        /// <summary>
        /// 
        /// </summary>
        public List<PropertyDocInfo> Properties { get; private set; } = new List<PropertyDocInfo>();
    }
}