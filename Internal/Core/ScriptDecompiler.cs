using NX_QScript.Exceptions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Global types used in global scope of QB files (taken from NodeQBC's Constants.js):
/*
    TypeBindings: {
		'Floats': 0x00,
		'Integer': 0x01,
		'Float': 0x02,
		'String': 0x03,
		'WideString': 0x04,
		'Pair': 0x05,
		'Vector': 0x06,
		'Script': 0x07,
		'Struct': 0x0A,
		'Array': 0x0C,
		'QBKey': 0x0D,
		'Pointer': 0x1A,
		'LocalString': 0x1C,
        'RequiredValue': 0x24,
        'RequiredValue': 0x25,
        'RequiredValue': 0x26,
        'RequiredValue': 0x27,
        'RequiredValue': 0x28,
        'RequiredValue': 0x29,
        'RequiredValue': 0x2A,
        'RequiredValue': 0x2B,
        'RequiredValue': 0x2C,
	}
*/

// Script tokens list (taken from NodeQBC's Constants.js):
// (Not all of these will show up in files (and some are never used at all),
//  but this is what we go by.)
/*
    ESCRIPTTOKEN_ENDOFFILE: 0x00,
    ESCRIPTTOKEN_ENDOFLINE: 0x01,
    ESCRIPTTOKEN_ENDOFLINENUMBER: 0x02,
    ESCRIPTTOKEN_STARTSTRUCT: 0x03,
    ESCRIPTTOKEN_ENDSTRUCT: 0x04,
    ESCRIPTTOKEN_STARTARRAY: 0x05,
    ESCRIPTTOKEN_ENDARRAY: 0x06,
    ESCRIPTTOKEN_EQUALS: 0x07,
    ESCRIPTTOKEN_DOT: 0x08,
    ESCRIPTTOKEN_COMMA: 0x09,
    ESCRIPTTOKEN_MINUS: 0x0A,
    ESCRIPTTOKEN_ADD: 0x0B,
    ESCRIPTTOKEN_DIVIDE: 0x0C,
    ESCRIPTTOKEN_MULTIPLY: 0x0D,
    ESCRIPTTOKEN_OPENPARENTH: 0x0E,
    ESCRIPTTOKEN_CLOSEPARENTH: 0x0F,
    ESCRIPTTOKEN_DEBUGINFO: 0x10,
    ESCRIPTTOKEN_SAMEAS: 0x11,
    ESCRIPTTOKEN_LESSTHAN: 0x12,
    ESCRIPTTOKEN_LESSTHANEQUAL: 0x13,
    ESCRIPTTOKEN_GREATERTHAN: 0x14,
    ESCRIPTTOKEN_GREATERTHANEQUAL: 0x15,
    ESCRIPTTOKEN_NAME: 0x16,
    ESCRIPTTOKEN_INTEGER: 0x17,
    ESCRIPTTOKEN_HEXINTEGER: 0x18,
    ESCRIPTTOKEN_ENUM: 0x19,
    ESCRIPTTOKEN_FLOAT: 0x1A,
    ESCRIPTTOKEN_STRING: 0x1B,
    ESCRIPTTOKEN_LOCALSTRING: 0x1C,
    ESCRIPTTOKEN_ARRAY: 0x1D,
    ESCRIPTTOKEN_VECTOR: 0x1E,
    ESCRIPTTOKEN_PAIR: 0x1F,
    ESCRIPTTOKEN_KEYWORD_BEGIN: 0x20,
    ESCRIPTTOKEN_KEYWORD_REPEAT: 0x21,
    ESCRIPTTOKEN_KEYWORD_BREAK: 0x22,
    ESCRIPTTOKEN_KEYWORD_SCRIPT: 0x23,
    ESCRIPTTOKEN_KEYWORD_ENDSCRIPT: 0x24,
    ESCRIPTTOKEN_KEYWORD_IF: 0x25,
    ESCRIPTTOKEN_KEYWORD_ELSE: 0x26,
    ESCRIPTTOKEN_KEYWORD_ELSEIF: 0x27,
    ESCRIPTTOKEN_KEYWORD_ENDIF: 0x28,
    ESCRIPTTOKEN_KEYWORD_RETURN: 0x29,
    ESCRIPTTOKEN_UNDEFINED: 0x2A,
    ESCRIPTTOKEN_CHECKSUM_NAME: 0x2B,
    ESCRIPTTOKEN_KEYWORD_ALLARGS: 0x2C,
    ESCRIPTTOKEN_ARG: 0x2D,
    ESCRIPTTOKEN_JUMP: 0x2E,
    ESCRIPTTOKEN_KEYWORD_RANDOM: 0x2F,
    ESCRIPTTOKEN_KEYWORD_RANDOM_RANGE: 0x30,
    ESCRIPTTOKEN_AT: 0x31,
    ESCRIPTTOKEN_OR: 0x32,
    ESCRIPTTOKEN_AND: 0x33,
    ESCRIPTTOKEN_XOR: 0x34,
    ESCRIPTTOKEN_SHIFT_LEFT: 0x35,
    ESCRIPTTOKEN_SHIFT_RIGHT: 0x36,
    ESCRIPTTOKEN_KEYWORD_RANDOM2: 0x37,
    ESCRIPTTOKEN_KEYWORD_RANDOM_RANGE2: 0x38,
    ESCRIPTTOKEN_KEYWORD_NOT: 0x39,
    ESCRIPTTOKEN_KEYWORD_AND: 0x3A,
    ESCRIPTTOKEN_KEYWORD_OR: 0x3B,
    ESCRIPTTOKEN_KEYWORD_SWITCH: 0x3C,
    ESCRIPTTOKEN_KEYWORD_ENDSWITCH: 0x3D,
    ESCRIPTTOKEN_KEYWORD_CASE: 0x3E,
    ESCRIPTTOKEN_KEYWORD_DEFAULT: 0x3F,
    ESCRIPTTOKEN_KEYWORD_RANDOM_NO_REPEAT: 0x40,
    ESCRIPTTOKEN_KEYWORD_RANDOM_PERMUTE: 0x41,
    ESCRIPTTOKEN_COLON: 0x42,
    ESCRIPTTOKEN_RUNTIME_CFUNCTION: 0x43,
    ESCRIPTTOKEN_RUNTIME_MEMBERFUNCTION: 0x44,
    ESCRIPTTOKEN_KEYWORD_USEHEAP: 0x45,
    ESCRIPTTOKEN_KEYWORD_UNKNOWN: 0x46,
    ESCRIPTTOKEN_KEYWORD_FASTIF: 0x47,
    ESCRIPTTOKEN_KEYWORD_FASTELSE: 0x48,
    ESCRIPTTOKEN_SHORTJUMP: 0x49,
    ESCRIPTTOKEN_INLINEPACKSTRUCT: 0x4A,
    ESCRIPTTOKEN_ARGUMENTPACK: 0x4B,
    ESCRIPTTOKEN_WIDESTRING: 0x4C,
    ESCRIPTTOKEN_NOTEQUAL: 0x4D,
    ESCRIPTTOKEN_STRINGQS: 0x4E,
    ESCRIPTTOKEN_KEYWORD_RANDOMFLOAT: 0x4F,
    ESCRIPTTOKEN_KEYWORD_RANDOMINTEGER: 0x50
*/

namespace NX_QScript.Internal.Core {
    /// <summary>
    ///  Internal helper class to decompile a script.
    /// </summary>
    internal static class ScriptDecompiler {
        /// <summary>
        ///  Decompile the input file into a string of source code text.
        /// </summary>
        /// <param name="qbFile">
        ///  The path to the compiled QB file to turn into QBC source code.
        /// </param>
        /// <returns>
        ///  New string of decompiled text.
        /// </returns>
        public static string Decompile(string qbFile) {
            // Do some sanity checks before we start decompiling!

            // File path given to decompile doesn't exist
            if (!File.Exists(qbFile)) {
                throw new FileNotFoundException($"The system cannot find the file specified: {qbFile}");
            }

            // This isn't a QB file!
            string fileName = Path.GetFileName(qbFile).ToLower();
            if (!fileName.Contains(".qb")) {
                throw new ArgumentException($"Input file is not a *.qb file: {qbFile}");
            }

            // Create a new instance of Reader
            Reader r = new Reader(qbFile);

            // Deserialize all tokens
            return DeserializeAllTokens(r);
        }

        /// <summary>
        ///  Deserializes all tokens from the instance of <see cref="Reader"/>.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private static string DeserializeAllTokens(Reader r) {
            // Skip the header stuff
            byte[] qbFileHeader = r.Chunk(28);

            // Output string
            string qbcSourceOut = "";

            // Keep looping while the Reader object has not yet exceeded its bounds
            while (!r.ExceededBounds()) {

                // Get the QB type info (always 4 bytes long)
                byte[] qbTypeInfo = r.Chunk(4);

                // Items 0 and 3 should both be zeros
                // Item 2 holds the actual QB type!
                byte zeroA = qbTypeInfo[0];
                byte flags = qbTypeInfo[1];
                byte qbType = qbTypeInfo[2];
                byte zeroB = qbTypeInfo[3];

                // Deserialize based on the QB type byte
                string parsedTypeOutput = ParseSerializedTypeByte(r, qbType) + '\n';
                qbcSourceOut += parsedTypeOutput;

            }

            // Return output source
            return qbcSourceOut;
        }

        /// <summary>
        ///  Parse the given serialized QB type byte into a real string of decompiled text.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="qbType"></param>
        /// <returns></returns>
        private static string ParseSerializedTypeByte(Reader r, byte qbType) {
            // What is the given QB type byte?
            switch (qbType) {
                // -- INTEGER
                case 1:
                    return ParseIntByte(r);

                // -- SCRIPT
                case 7:
                    return ParseScriptByte(r);

                // -- GLOBAL POINTER
                case 26:
                    return ParsePointerByte(r);
                    
                // -- UNKNOWN BYTE
                // 
                default:
                    throw new ScriptDecompileException($"Unexpected token byte in deserialization: {qbType:X8}. Contact a developer.");
            }
        }

        /// <summary>
        ///  Parses an integer from its information.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private static string ParseIntByte(Reader r, int tabOverBy = 0) {
            // Get the variable name
            string variableName = r.QBKey();

            // Skip PAK name
            uint pakName = r.UInt32();

            // Get the value!
            int value = r.Int32();

            // Tab string
            string tabbed = "";
            for (var i = 0; i < tabOverBy; i++) {
                tabbed += "\t";
            }

            // Return string with the parsed data
            return tabbed + $"{variableName} = {value}";
        }

        /// <summary>
        ///  Parse an entire script into decompiled its source code equivalent.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private static string ParseScriptByte(Reader r) {
            // Get the script's name
            uint scriptNameKey = r.UInt32();
            string hexHash = "0x" + scriptNameKey.ToString("X8").ToUpper().Replace("0X", "").PadLeft(8, '0');

            // Try and look up the script name
            string scriptLookupTest = QB.LookUpKey(scriptNameKey);

            // If script name is a raw QBKey, surround it in raw QBKey delimiters
            string scriptName = (scriptLookupTest.ToLower() == hexHash.ToLower())
                ? scriptName = $"#\"{hexHash}\""
                : scriptName = scriptLookupTest;

            // Skip PAK name
            uint pakName = r.UInt32();

            // Some sort of value? Is this used???
            int unkValue = r.Int32();

            // Skip next item
            uint dummyNextItem = r.UInt32();

            // -- WE ARE NOW READING SCRIPT PROPERTIES -- //

            // CRC of script data (is this important?)
            uint scriptDataCRC = r.UInt32();

            // Uncompressed and compressed sizes
            uint uncompressedSize = r.UInt32();
            uint compressedSize = r.UInt32();

            // Hold the offset of the Reader instance before reading the script data;
            // we'll need this in the script translation method to adjust for the
            // offset in the overall file since we're parsing it standalone!
            int originalReadOffset = r.Tell();

            // Get the script bytes
            byte[] scriptBytes = r.Chunk(compressedSize);

            // Are the sizes mismatched?
            // If so, it's LZSS compressed!
            if (uncompressedSize != compressedSize) {
                // Make an instance of LZSS to decompress the script
                LZSS lzss = new LZSS();

                // LZSS decompress the script
                scriptBytes = lzss.Decompress(scriptBytes);
            }

            // Align Reader to nearest 4 bytes just in case
            // (Scripts are end-padded by 4 bytes)
            r.SkipToNearest(4);

            // Parse the script tokens into real text
            // (MAKE SURE we transfer over the endianness of the
            //  main Reader instance!)
            string translatedScript = TranslateScript(scriptBytes, originalReadOffset, r.LittleEndian);

            // Now we'll add our "script" and "endscript" keywords!
            return $"script {scriptName} " + translatedScript + '\n' + "endscript";
        }

        /// <summary>
        ///  Translate a set of script bytes into its decompiled equivalent
        /// </summary>
        /// <param name="scriptBytes"></param>
        /// <returns></returns>
        private static string TranslateScript(byte[] scriptBytes, int origReadPos, bool isLittleEndian = false) {
            // For instance, this bytecode...
            /*
            01 16 4A B2 DD 28 16 E6 61 6B D1 07 2D 16 E6 61
            6B D1 01 16 02 79 C8 A6 4E 60 FF 4B C8 16 BC 41
            48 17 07 2D 16 6B 56 B3 3E 16 57 A5 95 7A 07 2D
            16 38 58 74 C4 01 16 25 B4 9C 97 01 29 16 E6 61
            6B D1 07 2D 16 E6 61 6B D1 01 24
            */

            // ...needs to decompile down to:
            /*
            script print_loading_time 
                GetTrueElapsedTime starttime = <starttime>
                finalprintf qs(#"0xC84BFF60") a = <elapsedtime> t = <text>
                GetTrueStartTime
                return starttime = <starttime>
            endscript
            */

            // This example's origin: qb.pak/scripts/guitar/guitar.qb (qb.pak in GH: WOR)

            // Or, for instance, for a 2nd example, this bytecode...
            // (I've annotated some stuff below for help in writing decompile logic):
            /*
                If statement           Global value dereference ($)
                ||     Next comp.      ||   Name token (0x16)
                ||     |               ||   ||       The QBKey of the global value
                ||     |     NOT  (    ||   ||        |
                \/   |---|   \/   \/   \/   \/   |----|----|
            01 [47] [19 00] [39] [0E] [4B] [16] [25 0A FA 57] 07 16 17 29
            6E 97 0F 01 16 C9 A5 6F 5B 01 28 01 47 35 00 0E
            4B 16 1D 43 61 87 07 17 01 00 00 00 0F 01 16 36
            E3 B6 9E 4A 18 00 00 00 00 00 01 00 00 00 00 08
            00 01 0D 00 00 00 00 00 11 47 F9 B0 00 00 00 00
            01 28 01 16 DF 01 A8 BF 4A 18 00 00 00 00 01 00
            00 00 00 08 00 01 01 00 BB A4 82 96 00 00 00 00
            00 00 00 00 01 24
            */

            // ...should decompile down to:
            /*
            script SongAudioPlay 
                if NOT ($current_song_qpak = jamsession)
                    SAP_Play
                endif
                if ($quickplay_whammy_rewind_enable = 1)
                    SpawnScript \{ quickplay_whammy_rewind }
                endif
                change \{ song_is_waiting_to_start = 0 }
            endscript 
            */

            // We've already accounted for the "script [NAME]" part and the
            // and we provide the "endscript" keyword in the original caller,
            // so all we need to do is discern the contents in this bytecode.
            // Since the end-of-script token is 0x24, keep reading tokens
            // until we hit 0x24, meaning there are no tokens left as the
            // script has ended. The translation gets returned to the caller.

            // End of script byte
            const byte EndOfScriptToken = 0x24;

            // Hold final translation to be returned back
            string finalTranslation = "";

            // Create a new Reader instance separate from the main one
            // to decompile the script bytes
            Reader r = new Reader(scriptBytes);

            // Carry over endianness of the main Reader instance, SWITCHED
            // (All bytes in a script are the inverse endian)

            // Just as a heads up: Most (if not all) QB files are serialized
            // in big endian, but when it comes to script bytes, they're
            // ALL written in the INVERSE endian, so that's why we're switching
            // the endianness here. The only exception is for wide strings,
            // (0x4C), which are written in the original endian.
            r.LittleEndian = !isLittleEndian;

            // Grab the first token byte
            byte token = r.UInt8();

            // Keep parsing tokens until there are no more (or the Reader
            // instance has gone out of bounds)
            while (token != EndOfScriptToken && !r.ExceededBounds()) {
                // What token is being represented by the current byte?
                // (This switch doesn't need to account for the end-of-script
                //  token, since the while loop will break before it gets here)
                switch (token) {
                    // -- END OF FILE
                    case 0x00:
                        break;

                    // -- END OF LINE
                    // -- (The latter is a THAW StructScript thing, used internally for debugging)
                    case 0x01:
                    case 0x02:
                        finalTranslation += '\n';
                        break;

                    // -- START STRUCT
                    case 0x03:
                        finalTranslation += "{";
                        break;

                    // -- END STRUCT
                    case 0x04:
                        finalTranslation += "}";
                        break;

                    // -- START ARRAY
                    case 0x05:
                        finalTranslation += "[";
                        break;

                    // -- END ARRAY
                    case 0x06:
                        finalTranslation += "]";
                        break;

                    // -- EQUALS
                    case 0x07:
                        finalTranslation += " = ";
                        break;

                    // -- DOT
                    case 0x08:
                        finalTranslation += ".";
                        break;

                    // -- COMMA
                    // -- (Purely visual, the compiler throws this out when
                    // --  it parses it unless in a pair or vector)
                    case 0x09:
                        finalTranslation += ",";
                        break;

                    // -- MINUS
                    case 0x0A:
                        finalTranslation += " - ";
                        break;

                    // -- ADD
                    case 0x0B:
                        finalTranslation += " + ";
                        break;

                    // -- DIVIDE
                    case 0x0C:
                        finalTranslation += " / ";
                        break;

                    // -- MULTIPLY
                    case 0x0D:
                        finalTranslation += " * ";
                        break;

                    // -- OPEN PARENTHESES
                    case 0x0E:
                        finalTranslation += "(";
                        break;

                    // -- CLOSE PARENTHESES
                    case 0x0F:
                        finalTranslation += ")";
                        break;

                    // -- DEBUG INFO
                    // (Ignored, not concerned about it)

                    // -- SAME AS (EQUALITY)
                    // -- (Yes, QBC actually uses 1 equal sign both for assignment and comparison)
                    case 0x11:
                        finalTranslation += " = ";
                        break;

                    // -- LESS THAN
                    case 0x12:
                        finalTranslation += " < ";
                        break;

                    // -- LESS THAN OR EQUAL
                    case 0x13:
                        finalTranslation += " <= ";
                        break;

                    // -- GREATER THAN
                    case 0x14:
                        finalTranslation += " > ";
                        break;

                    // -- GREATER THAN OR EQUAL
                    case 0x15:
                        finalTranslation += " >= ";
                        break;

                    // -- NAME TOKEN
                    // -- (usually a script's name or CFunc; some sort of script call)
                    case 0x16:
                        // Get the name
                        string name = r.QBKey();

                        // If the name is a raw QBKey, surround it in raw QBKey delimiters
                        string nameLookupTest = QB.LookUpKey(name);
                        string nameFinal = (nameLookupTest.ToLower() == name.ToLower())
                            ? name = $"#\"{name}\""
                            : name = nameLookupTest;

                        // Append to translation
                        finalTranslation += name;
                        break;

                    // -- INTEGER
                    case 0x17:
                        // Get the integer value
                        int intValue = r.Int32();

                        // Append to translation
                        finalTranslation += intValue.ToString();
                        break;

                    // -- HEX INTEGER
                    case 0x18:
                        // Get the hex value
                        uint hexValue = r.UInt32();

                        // Append to translation
                        finalTranslation += "#\"" + "0x" + hexValue.ToString("X8").ToUpper().Replace("0X", "").PadLeft(8, '0') + '"';
                        break;

                    // -- ENUM
                    // (Unused, not concerned about it)

                    // -- FLOAT
                    case 0x1A:
                        // Get the float value
                        float floatValue = r.Float32();

                        // Append to translation
                        finalTranslation += floatValue.ToString();
                        break;

                    // -- STRING
                    case 0x1B:
                        // Get the string value
                        string strValue = r.NumString();

                        // Escape apostrophes (since normal QBC strings use ' delimiters)
                        strValue = strValue.Replace("'", "\\'");

                        // Append to translation
                        finalTranslation += $"'{strValue}'";
                        break;

                    // -- LOCAL STRING
                    // -- (This is a localized string, wrapped in qs() delimiters)
                    case 0x1C: case 0x4E:
                        // Get QBKey of the localized string
                        uint localStringKey = r.UInt32();

                        // Wrap in qs() delimiters, ensuring the hexadecimal is padded by 8 zeros
                        string localString = "qs(#\"" + "0x" + localStringKey.ToString("X8").ToUpper().Replace("0X", "").PadLeft(8, '0') + "\")";

                        // Append to translation
                        finalTranslation += localString;
                        break;

                    // -- ARRAY
                    // -- (This is an array of any type)
                    case 0x1D:

                        // TODO: Implement logic for array parsing

                        break;

                    // -- VECTOR (3D vector)
                    case 0x1E:
                        // Get the vector value
                        float x = r.Float32();
                        float y = r.Float32();
                        float z = r.Float32();

                        // Append to translation
                        finalTranslation += $"({x}, {y}, {z})";
                        break;

                    // -- PAIR (2D vector)
                    case 0x1F:
                        // Get the pair value
                        float pairX = r.Float32();
                        float pairY = r.Float32();

                        // Append to translation
                        finalTranslation += $"({pairX}, {pairY})";
                        break;

                    // -- KEYWORD BEGIN
                    // -- (Starts a loop)
                    case 0x20:
                        finalTranslation += $"begin";
                        break;

                    // -- KEYWORD REPEAT
                    // -- (Tells the loop how many times to repeat)
                    case 0x21:
                        finalTranslation += $"repeat ";
                        break;

                    // -- KEYWORD BREAK
                    // -- (Breaks out of a loop)
                    case 0x22:
                        finalTranslation += $"break";
                        break;

                    // -- KEYWORD SCRIPT
                    // -- (This is a script, so we don't need to do anything here)
                    case 0x23:
                        break;

                    // -- KEYWORD ENDSCRIPT
                    // -- (This is the end of a script, so we don't need to do anything here)
                    case 0x24:
                        break;

                    // -- IF STATEMENT, FASTIF, ELSE, FASTELSE, ELSEIF, ENDIF
                    // -- (This will account for an entire conditional block)
                    case 0x25: case 0x47: case 0x26: case 0x48: case 0x27: case 0x28:

                        // TODO: Implement logic for if statements
                        // (This probably needs to be a separate function, since there's
                        //  a lot to unpack with these)

                        break;

                    // -- KEYWORD RETURN
                    // -- (Returns something, obviously)
                    case 0x29:
                        finalTranslation += $"return ";
                        break;

                    // -- UNDEFINED
                    // -- (Something we probably don't need to worry about)
                    case 0x2A:
                        break;

                    // -- CHECKSUM NAME
                    // -- (Something used in THUG files as a debug for a QBKey)
                    // -- (Not concerned about it)
                    case 0x2B:
                        break;

                    // -- KEYWORD ALLARGS
                    // -- (<...>)
                    case 0x2C:
                        finalTranslation += "<...>";
                        break;

                    // -- ARGUMENT
                    // -- (It's a local variable, so something is going to follow after this)
                    case 0x2D:
                        // Only used by the next QBKey
                        break;

                    // -- JUMP
                    // -- (Not used for now since no known port of QBC supports gotos)
                    case 0x2E:
                        break;

                    // -- KEYWORD RANDOM
                    // -- (Random generator, standard random)
                    case 0x2F:
                        finalTranslation += "Random";
                        break;

                    // -- KEYWORD RANDOM RANGE
                    // -- (Random generator, random range)
                    case 0x30:
                        finalTranslation += "RandomRange";
                        break;

                    // -- AT (unused)
                    case 0x31:
                        break;

                    // -- OR
                    case 0x32:
                        finalTranslation += " || ";
                        break;

                    // -- AND
                    case 0x33:
                        finalTranslation += " & ";
                        break;

                    // -- XOR (unused)
                    case 0x34:
                        break;

                    // -- SHIFT LEFT
                    // -- (Left bit shift)
                    case 0x35:
                        finalTranslation += " << ";
                        break;

                    // -- SHIFT RIGHT
                    // -- (Right bit shift)
                    case 0x36:
                        finalTranslation += " >> ";
                        break;

                    // -- KEYWORD RANDOM2
                    // -- (Random generator, standard(?) random)
                    case 0x37:
                        finalTranslation += "Random2";
                        break;

                    // -- KEYWORD RANDOM RANGE2
                    // -- (Random generator, random range)
                    case 0x38:
                        finalTranslation += "RandomRange2";
                        break;

                    // -- KEYWORD NOT
                    // -- (Logical NOT)
                    case 0x39:
                        finalTranslation += "NOT";
                        break;

                    // -- KEYWORD AND
                    // -- (Logical AND)
                    case 0x3A:
                        finalTranslation += "&";
                        break;

                    // -- KEYWORD OR
                    // -- (Logical OR)
                    case 0x3B:
                        finalTranslation += "||";
                        break;

                    // -- KEYWORD SWITCH, CASE, DEFAULT, AND ENDSWITCH
                    // -- (Switch statement)
                    case 0x3C: case 0x3E: case 0x3F: case 0x3D:

                        // TODO: Implement logic for switch statements

                        break;

                    // -- KEYWORD RANDOM NO REPEAT
                    // -- (Random generator, random no repeat)
                    case 0x40:
                        finalTranslation += "RandomNoRepeat";
                        break;

                    // -- KEYWORD RANDOM PERMUTE
                    // -- (Random generator, random permute)
                    case 0x41:
                        finalTranslation += "RandomPermute";
                        break;

                    // -- COLON
                    // -- (Used as '::', used for accessing certain functions on objects)
                    case 0x42:
                        finalTranslation += "::";
                        break;

                    // -- INLINE PACKED STRUCT (\{ } type of struct)
                    // -- (Used for inline packed structs)
                    case 0x4A:
                        finalTranslation += "\\{";
                        break;

                    // -- GLOBAL POINTER (ARG. PACK)
                    // -- (Just another pointer type to a value in global scope)
                    case 0x4B:
                        finalTranslation += "$";
                        break;

                    // -- WIDE STRING
                    // -- (This is the only script type that is not in the inverse endian)
                    case 0x4C:
                        // Temporarily swap the endian again
                        r.LittleEndian = !r.LittleEndian;

                        // Get the wide string value
                        string wideStringValue = r.NumString();

                        // Invert the endian again
                        r.LittleEndian = !r.LittleEndian;

                        // Append to translation
                        finalTranslation += $"\"{wideStringValue}\"";
                        break;

                    // -- NOT EQUAL
                    case 0x4D:
                        finalTranslation += " != ";
                        break;

                    // -- RANDOM FLOAT
                    case 0x4F:
                        finalTranslation += "RandomFloat";
                        break;

                    // -- RANDOM INTEGER
                    case 0x50:
                        finalTranslation += "RandomInteger";
                        break;

                    // -- UNKNOWN BYTE
                    default:
                        Chalk.Warn($"Unknown byte found: {token:X8}; this should be investigated.");
                        break;
                }

                // At the end of the loop, grab the next token
                token = r.UInt8();

            }

            // Return the translation
            return finalTranslation;
        }

        private static string ParsePointerByte(Reader r) {
            return "$something";
        }
    }
}
