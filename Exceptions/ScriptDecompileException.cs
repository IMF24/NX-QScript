using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Exceptions {
    /// <summary>
    ///  An exception thrown when a decompilation operation on raw QB bytecode fails.
    /// </summary>
    public class ScriptDecompileException : Exception {
        /// <summary>
        ///  Construct a new instance of ScriptDecompileException.
        /// </summary>
        public ScriptDecompileException() : base() { }

        /// <summary>
        ///  Construct a new instance of ScriptDecompileException with a message.
        /// </summary>
        /// <param name="message">
        ///  The message to include with the exception.
        /// </param>
        public ScriptDecompileException(string message) : base(message) { }

        /// <summary>
        ///  Construct a new instance of ScriptDecompileException with a message and an inner exception.
        /// </summary>
        /// <param name="message">
        ///  The message to include with the exception.
        /// </param>
        /// <param name="innerException">
        ///  The inner exception to include with the exception.
        /// </param>
        public ScriptDecompileException(string message, Exception innerException) : base(message, innerException) { }
    }
}
