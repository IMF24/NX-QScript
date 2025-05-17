// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  Q B   F I L E
//      Any sort of Neversoft QScript file representable by an
//      object, made up of various types
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Types {
    /// <summary>
    ///  Any sort of Neversoft QScript file representable by an object, made up of various types.
    /// </summary>
    public class QBFile {
        /// <summary>
        ///  Construct a new instance of <see cref="QBFile"/>.
        /// </summary>
        /// <param name="filePath">
        ///  The QB file to be (de)compiled into various QB types.
        /// </param>
        public QBFile(string filePath) {
            // Set file path
            FilePath = filePath;

            // Compile or decompile as objects
            //~ string fileExt = Path.GetExtension(FilePath);
            //~ if (fileExt == ".q") {
            //~    QBCItemCore[] items = QBC.InternalCompileAsObjects(FilePath);
            //~ } else {
            //~    QBCItemCore[] items = QBC.InternalDecompileAsObjects(FilePath);
            //~ }
            //~ Contents = items;
        }

        public QBFile(byte[] fileBytes) {

        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  The path to the file that was compiled or decompiled.
        /// </summary>
        public string FilePath { get; set; }
    }
}
