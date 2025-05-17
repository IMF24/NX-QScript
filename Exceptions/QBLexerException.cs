using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Exceptions {
    /// <summary>
    ///  An exception thrown when a lexing operation during script compilation fails for any arbitrary reason.
    /// </summary>
    public class QBLexerException : Exception {
        /// <summary>
        ///  Construct a new instance of QBLexerException.
        /// </summary>
        public QBLexerException() : base() { }

        /// <summary>
        ///  Construct a new instance of QBLexerException with a message.
        /// </summary>
        /// <param name="message">
        ///  The message to include with the exception.
        /// </param>
        public QBLexerException(string message) : base(message) { }

        /// <summary>
        ///  Construct a new instance of QBLexerException with a message and an inner exception.
        /// </summary>
        /// <param name="message">
        ///  The message to include with the exception.
        /// </param>
        /// <param name="innerException">
        ///  The inner exception to include with the exception.
        /// </param>
        public QBLexerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
