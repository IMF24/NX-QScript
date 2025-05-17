// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  N X   Q S C R I P T   -   E N T R Y   P O I N T
//      Main entry point class for QBC
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using NX_QScript.Internal;
using NX_QScript.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript {
    /// <summary>
    ///  Main access class for working with Neversoft QScript files.
    /// </summary>
    public static class QBC {
        /// <summary>
        ///  Decompile a compiled QScript file (<c>*.qb</c>) into QBC source code.
        /// </summary>
        /// <param name="qbFile">
        ///  The compiled QB file.
        /// </param>
        /// <returns>
        ///  String containing the QBC source code from the decompile job.
        /// </returns>
        public static string Decompile(string qbFile) {
            // Return dummy string for now
            return "";
        }

        /// <summary>
        ///  Decompile a compiled QScript file (<c>*.qb</c>) in an array of bytes into QBC source code.
        /// </summary>
        /// <param name="qbFileBytes">
        ///  The compiled QB file bytes in an array.
        /// </param>
        /// <returns>
        ///  String containing the QBC source code from the decompile job.
        /// </returns>
        public static string Decompile(byte[] qbFileBytes) {
            // Return dummy string for now
            return "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Decompile a compiled QScript file (<c>*.qb</c>) into a <see cref="QBFile"/> instance that can be altered in various ways.
        /// </summary>
        /// <param name="qbFile">
        ///  The QB file path to have the data read from.
        /// </param>
        /// <returns>
        ///  New instance of <see cref="QBFile"/> containing the deserialized QB file's contents.
        /// </returns>
        public static QBFile DecompileAsObjects(string qbFile) {
            return new QBFile(qbFile);
        }

        /// <summary>
        ///  Decompile a compiled QScript file (<c>*.qb</c>) in a byte array into a <see cref="QBFile"/> instance that can be altered
        ///  in various ways.
        /// </summary>
        /// <param name="qbFileBytes">
        ///  The bytes of the QB file to have the data read from.
        /// </param>
        /// <returns>
        ///  New instance of <see cref="QBFile"/> containing the deserialized QB file's contents.
        /// </returns>
        public static QBFile DecompileAsObjects(byte[] qbFileBytes) {
            return new QBFile(qbFileBytes);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Compile a source code file into an array of bytes representing the QB file.
        /// </summary>
        /// <param name="qFile">
        ///  The Q file path to be read and compiled.
        /// </param>
        /// <returns>
        ///  Array of bytes containing the serialized QB file.
        /// </returns>
        public static byte[] Compile(string qFile) {
            // Just shuts the compiler up for now
            return new byte[0];
        }

        /// <summary>
        ///  Compile a source code file or string into an array of bytes representing the QB file.
        /// </summary>
        /// <param name="qFileOrSource">
        ///  The Q file path OR the raw source code to compile.
        /// </param>
        /// <param name="inputMode">
        ///  Input mode that the <paramref name="qFileOrSource"/> should be treated as.
        /// </param>
        /// <returns>
        ///  Array of bytes containing the serialized QB file.
        /// </returns>
        public static byte[] Compile(string qFileOrSource, QBInputMode inputMode) {
            // Just shuts the compiler up for now
            return new byte[0];
        }
        
        /// <summary>
        ///  Compile a source code file or string into an array of bytes representing the QB file.
        /// </summary>
        /// <param name="qFileOrSource">
        ///  The Q file path OR the raw source code to compile.
        /// </param>
        /// <param name="inputMode">
        ///  Input mode that the <paramref name="qFileOrSource"/> should be treated as.
        /// </param>
        /// <param name="gameTarget">
        ///  
        /// </param>
        /// <returns>
        ///  Array of bytes containing the serialized QB file.
        /// </returns>
        public static byte[] Compile(string qFileOrSource, QBInputMode inputMode, QBGameTarget gameTarget) {
            // Just shuts the compiler up for now
            return new byte[0];
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Compile the given Q file into a single <see cref="QBFile"/> object containing the serialized contents.
        /// </summary>
        /// <param name="qFile">
        ///  Path to the Q file to serialize.
        /// </param>
        /// <returns>
        ///  New instance of <see cref="QBFile"/> that may be used.
        /// </returns>
        public static QBFile CompileAsObjects(string qFile) {
            // Just to shut the compiler up for now
            return new QBFile("");
        }

        /// <summary>
        ///  Compile the given Q file or source code into a single <see cref="QBFile"/> object containing the serialized contents, given the input mode.
        /// </summary>
        /// <param name="qFileOrSource">
        ///  Path to the Q file OR raw QBC source code to be serialized.
        /// </param>
        /// <param name="inputMode">
        ///  Input mode that the <paramref name="qFileOrSource"/> should be treated as.
        /// </param>
        /// <returns>
        ///  New instance of <see cref="QBFile"/> that may be used.
        /// </returns>
        public static QBFile CompileAsObjects(string qFileOrSource, QBInputMode inputMode) {
            // Just to shut the compiler up for now
            return new QBFile("");
        }
        /// <summary>
        ///  Compile the given Q file or source code into a single <see cref="QBFile"/> object containing the serialized contents, given the input mode and target game.
        /// </summary>
        /// <param name="qFileOrSource">
        ///  Path to the Q file OR raw QBC source code to be serialized.
        /// </param>
        /// <param name="inputMode">
        ///  Input mode that the <paramref name="qFileOrSource"/> should be treated as.
        /// </param>
        /// <param name="gameTarget">
        ///  The target game to compile the script for.
        /// </param>
        /// <returns>
        ///  New instance of <see cref="QBFile"/> that may be used.
        /// </returns>
        public static QBFile CompileAsObjects(string qFileOrSource, QBInputMode inputMode, QBGameTarget gameTarget) {
            // Just to shut the compiler up for now
            return new QBFile("");
        }
    }

    /// <summary>
    ///  Type of QB source input mode.
    /// </summary>
    public enum QBInputMode {
        /// <summary>
        ///  The source is a <c>*.q</c> file on the disk.
        /// </summary>
        QFile = 0,
        /// <summary>
        ///  The source is literally source code to be compiled.
        /// </summary>
        QSource = 1,
    }

    /// <summary>
    ///  Various game targets that a QB script can be compiled for.
    ///  <br/><br/>
    ///  Make sure to refer to <a href="https://github.com/IMF24/NX-QScript/tree/master/README.md#games-supported">the readme</a> for which games are supported for (de)compiling!
    /// </summary>
    public enum QBGameTarget {
        /// <summary>
        ///  The script will be compiled for Guitar Hero: World Tour (default).
        /// </summary>
        GHWT = 0,
        /// <summary>
        ///  The script will be compiled for Guitar Hero III: Legends of Rock.
        /// </summary>
        GH3 = 1,
        /// <summary>
        ///  The script will be compiled for Guitar Hero: Warriors of Rock.
        /// </summary>
        GH6 = 2,
        /// <summary>
        ///  The script will be compiled for Tony Hawk's American Wasteland.
        /// </summary>
        THAW = 3,
    }
}
