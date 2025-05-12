// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  J O B   -   C O M P I L E   J O B
//      Type of job used to compile a script from source code
//      to compiled QB bytecode
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Type of job used to compile a script from source code to compiled QB bytecode. (<c>*.q</c> to <c>*.qb</c>)
    /// </summary>
    public class QBCCompileJob : QBCBaseJob {
        /// <summary>
        ///  Construct a new instance of <see cref="QBCCompileJob"/> given a source file to read.
        /// </summary>
        /// <param name="qFile">
        ///  The source <c>*.q</c> file to parse the code from.
        /// </param>
        public QBCCompileJob(string qFile) : base() {
            HandleCtor(qFile, null);   
        }

        /// <summary>
        ///  Construct a new instance of <see cref="QBCCompileJob"/> given a source file to read and various job options.
        /// </summary>
        /// <param name="qFile">
        ///  The source <c>*.q</c> file to parse the code from.
        /// </param>
        /// <param name="options">
        ///  Options to use with the compile job.
        /// </param>
        public QBCCompileJob(string qFile, QBCJobOptions options) : base() {
            HandleCtor(qFile, options);
        }

        /// <summary>
        ///  Handles constructor data.
        /// </summary>
        /// <param name="qFile">
        ///  The source <c>*.q</c> file to parse the code from.
        /// </param>
        /// <param name="options">
        ///  Options to use with the compile job.
        /// </param>
        /// <exception cref="FileNotFoundException"/>
        private void HandleCtor(string qFile, QBCJobOptions options) {
            // Assign file path and check it
            FileName = qFile;

            // File does not exist
            if (!File.Exists(FileName)) {
                throw new FileNotFoundException($"Input QBC source *.q file not found: {FileName}");
            }

            // Read all text from the file
            Input = File.ReadAllText(FileName);

            // Assign to just file name
            FileName = Path.GetFileName(FileName);

            // No output yet; we'll handle it later
            Output = null;

            // Assign options
            Options = options;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  The source code input string.
        /// </summary>
        public new string Input { get; private set; }

        /// <summary>
        ///  The raw byte output from the compile job.
        /// </summary>
        public new byte[] Output { get; private set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }
}
