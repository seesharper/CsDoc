namespace CsDoc.Model
{
    using System;

    /// <summary>
    /// Represents a documented exception.
    /// </summary>
    public class ExceptionDocInfo
    {
        /// <summary>
        /// Gets the <see cref="Exception"/> type.
        /// </summary>
        public string Type { get; internal set; }

        /// <summary>
        /// Gets the text that describes when the exception is thrown.
        /// </summary>
        public string ThrownWhen { get; internal set; }
    }
}