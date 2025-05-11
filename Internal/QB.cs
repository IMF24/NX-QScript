// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  Q B   H A N D L E R   C L A S S
//      Basic class for working with QB things, QBKeys, etc.
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Other required imports.
using System.Collections.Generic;
using System.Text;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Helper class for working with basic aspects of Neversoft's QB system.
    /// </summary>
    internal static class QB {
        /// <summary>
        ///  The CRC32 table used in generating Neversoft QBKeys.
        /// </summary>
        public static readonly uint[] CRC32_TABLE = new uint[] {
            0x00000000, 0x77073096, 0xee0e612c, 0x990951ba,
            0x076dc419, 0x706af48f, 0xe963a535, 0x9e6495a3,
            0x0edb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988,
            0x09b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91,
            0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de,
            0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7,
            0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec,
            0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5,
            0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172,
            0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b,
            0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940,
            0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
            0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116,
            0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f,
            0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924,
            0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d,
            0x76dc4190, 0x01db7106, 0x98d220bc, 0xefd5102a,
            0x71b18589, 0x06b6b51f, 0x9fbfe4a5, 0xe8b8d433,
            0x7807c9a2, 0x0f00f934, 0x9609a88e, 0xe10e9818,
            0x7f6a0dbb, 0x086d3d2d, 0x91646c97, 0xe6635c01,
            0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e,
            0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457,
            0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c,
            0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
            0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2,
            0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb,
            0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0,
            0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9,
            0x5005713c, 0x270241aa, 0xbe0b1010, 0xc90c2086,
            0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f,
            0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4,
            0x59b33d17, 0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad,
            0xedb88320, 0x9abfb3b6, 0x03b6e20c, 0x74b1d29a,
            0xead54739, 0x9dd277af, 0x04db2615, 0x73dc1683,
            0xe3630b12, 0x94643b84, 0x0d6d6a3e, 0x7a6a5aa8,
            0xe40ecf0b, 0x9309ff9d, 0x0a00ae27, 0x7d079eb1,
            0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe,
            0xf762575d, 0x806567cb, 0x196c3671, 0x6e6b06e7,
            0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc,
            0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
            0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252,
            0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b,
            0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60,
            0xdf60efc3, 0xa867df55, 0x316e8eef, 0x4669be79,
            0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236,
            0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f,
            0xc5ba3bbe, 0xb2bd0b28, 0x2bb45a92, 0x5cb36a04,
            0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
            0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x026d930a,
            0x9c0906a9, 0xeb0e363f, 0x72076785, 0x05005713,
            0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0x0cb61b38,
            0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0x0bdbdf21,
            0x86d3d2d4, 0xf1d4e242, 0x68ddb3f8, 0x1fda836e,
            0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777,
            0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c,
            0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45,
            0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2,
            0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db,
            0xaed16a4a, 0xd9d65adc, 0x40df0b66, 0x37d83bf0,
            0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
            0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6,
            0xbad03605, 0xcdd70693, 0x54de5729, 0x23d967bf,
            0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94,
            0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
        };

        /// <summary>
        ///  From a literal string, generates a Neversoft QBKey and returns it in string hexadecimal form.
        /// </summary>
        /// <param name="literalText">
        ///  Text to be converted into a key.
        /// </param>
        /// <returns>
        ///  String containing a hexadecimal string (preceded by "0x") which is the hashed QBKey.
        /// </returns>
        public static string QBKey(string literalText) {
            // Convert the string to lowercase and convert all slashes to backslashes
            string correctedText = literalText.ToLower().Replace('/', '\\');

            // Get the UTF-8 text bytes of the corrected string
            byte[] textBytes = Encoding.UTF8.GetBytes(correctedText);

            // Original CRC generated
            uint crc = 0xFFFFFFFF;

            // For each text byte, run it against the CRC32 table
            foreach (byte b in textBytes) {
                uint numA = (crc ^ b) & 0xFF;
                crc = CRC32_TABLE[numA] ^ crc >> 8 & 0x00FFFFFF;
            }

            // Create the final resulting key
            uint finalCRC = ~crc;
            long value = -finalCRC - 1;
            string result = (value & 0xFFFFFFFF).ToString("X8");

            // Pad string with 8 zeros, add "0x" to the start
            result = result.PadLeft(8, '0');
            return "0x" + result.ToUpper();
        }

        /// <summary>
        ///  From a literal srting, generates a Neversoft QS string hash (translatable string in-game).
        /// </summary>
        /// <param name="literalText">
        ///  Text to generate the QS string hash of.
        /// </param>
        /// <returns>
        ///  String containing a hexadecimal string (preceded by "0x") which is the hashed QS string.
        /// </returns>
        public static string QSString(string literalText) {
            // Get the little endian UTF-16 bytes of the string
            byte[] textBytes = Encoding.Unicode.GetBytes(literalText);

            // Original CRC generated
            uint crc = 0xFFFFFFFF;

            // For each text byte, run it against the CRC32 table
            foreach (byte b in textBytes) {
                uint numA = (crc ^ b) & 0xFF;
                crc = CRC32_TABLE[numA] ^ crc >> 8 & 0x00FFFFFF;
            }

            // Create the final resulting key
            uint finalCRC = ~crc;
            long value = -finalCRC - 1;
            string result = (value & 0xFFFFFFFF).ToString("X8");

            // Pad string with 8 zeros, add "0x" to the start
            result = result.PadLeft(8, '0');
            return "0x" + result.ToUpper();
        }

        /// <summary>
        ///  Given a string to be converted to a QBKey, converts it to a key, then returns its
        ///  actual UInt32 representation.
        /// </summary>
        /// <param name="key">
        ///  Text key to return as a UInt32.
        /// </param>
        /// <returns>
        ///  A 32 bit unsigned integer representing the QBKey.
        /// </returns>
        public static uint Hexify(string key) {
            // Convert the string to lowercase and convert all slashes to backslashes
            string correctedText = key.ToLower().Replace('/', '\\');

            // Get the UTF-8 text bytes of the corrected string
            byte[] textBytes = Encoding.UTF8.GetBytes(correctedText);

            // Original CRC generated
            uint crc = 0xFFFFFFFF;

            // For each text byte, run it against the CRC32 table
            foreach (byte b in textBytes) {
                uint numA = (crc ^ b) & 0xFF;
                crc = CRC32_TABLE[numA] ^ crc >> 8 & 0x00FFFFFF;
            }

            // Create the final resulting key
            uint finalCRC = ~crc;
            long value = -finalCRC - 1;
            uint result = (uint) (value & 0xFFFFFFFF);

            return result;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  The global database of Neversoft QBKeys and their literal translations.
        ///  Populated on program start. The database can be expanded upon or have
        ///  its items modified, but its value can never be re-assigned.
        /// </summary>
        public static Dictionary<string, string> QBKeyDatabase { get; } = new Dictionary<string, string>();

        /// <summary>
        ///  Upon boot, register all QBKeys to the global database.
        /// </summary>
        public static void RegisterKeys(bool registerKeys = true) {
            // Only run if we want to register keys!
            if (!registerKeys) return;

            // Read the keys file and split it at the newlines
            // We have this text file as a resource, so we'll use it
            string[] keyLines = Properties.Resources.QBKeysBank.Split('\n');

            //~ Chalk.Warn($"Registering QBKeys...");

            // First 10 characters SHOULD be the hash of the key.
            // If it's preceded by "0x", use the first 10 characters.
            // Otherwise, try using 8 characters.
            foreach (string keyLine in keyLines) {

                // In our bank file, each of the keys is formatted like this:
                // 
                //          This whitespace just separates the hash
                //          from the translation and is not significant
                //           |
                //           V
                // 0x1234ABCD "key_here"
                //                     ^
                //                     |  
                //                    These leading/trailing quotation marks         
                //                    can be discarded, but the key itself         
                //                    contained within is important.
                // 
                // We need to split the key from the literal translation and
                // the hexadecimal hash.
                // 
                // HOWEVER: We cannot just split at the whitespace since QBKey
                // translations can contain whitespace.
                //
                // We'll splice the string after the first 10 characters, skip
                // the delimiting whitespace, then trim off the first and last
                // quotation marks, and that's the value we'll register.

                int keyUpToIdx = (keyLine.StartsWith("0x")) ? 10 : 8;
                int keyContentFromIdx = keyUpToIdx + 1;

                string keyHash = keyLine.Substring(0, keyUpToIdx);
                string keyLiteral = keyLine.Substring(keyUpToIdx).Trim().Trim('"').Trim('\n');

                // Pad the hash left with 8 zeros
                keyHash = keyHash.PadLeft(8, '0');

                // Make sure the hash is in the correct format:
                // - Starts with "0x"
                // - The hex string is 10 characters long
                // - The alphanumeric characters are all uppercase (A-F)
                if (!keyHash.StartsWith("0x") || keyHash.Length != 10) {
                    keyHash = "0x" + keyHash;
                }
                keyHash = keyHash.ToUpper().Replace("0X", "0x");

                // Add the key to the database
                AddQBKeyToDatabase(keyHash, keyLiteral);

            }

            //~ Chalk.Success($"Registered {QBKeyDatabase.Count} key(s)!");
        }

        /// <summary>
        ///  Adds a key to the global QBKey database, given both its hex key and literal translation.
        /// </summary>
        /// <param name="hexKey">
        ///  The hexadecimal QBKey string to be used.
        /// </param>
        /// <param name="literalTranslation">
        ///  The literal translation of the key.
        /// </param>
        public static void AddQBKeyToDatabase(string hexKey, string literalTranslation) {
            // Fix up hex key and add it to the database
            string correctedKey = hexKey.ToUpper().Replace("0X", "0x");
            QBKeyDatabase[correctedKey] = literalTranslation;
        }

        /// <summary>
        ///  Adds a key to the global QBKey database, given a key's literal text string only. The hashed hex string
        ///  will be created upon calling this function.
        /// </summary>
        /// <param name="literalKey">
        ///  The key to be added to the database, whose hexadecimal hash string will automatically be created.
        /// </param>
        public static void AddQBKeyToDatabase(string literalKey) {
            // Generate the hex string for the key
            string keyHash = QBKey(literalKey);
            QBKeyDatabase[keyHash] = literalKey;
        }

        /// <summary>
        ///  Given a raw hexadecimal string, attempts to look up the key's translation in the database. If the key does not
        ///  exist, the originally provided hex string is returned back.
        /// </summary>
        /// <param name="hexString">
        ///  Raw hexadecimal string to look up.
        /// </param>
        /// <returns>
        ///  Translation of the hashed QBKey, if it exists in the database. If not, the originally provided hash is returned.
        /// </returns>
        public static string LookUpKey(string hexString) {
            // Input text exists in the database values!
            // If that's the case, just return the string back! We've already
            // looked the key up, we don't need to look it up again
            if (QBKeyDatabase.ContainsValue(hexString)) return hexString;

            // Look up the string
            string correctedHexStr = hexString.ToUpper().Replace("0X", "0x");
            return (QBKeyDatabase.TryGetValue(correctedHexStr, out string value)) ? value : correctedHexStr;
        }

        /// <summary>
        ///  Given an unsigned 32 bit integer, attempts to look up the translation in the database. If the key does not
        ///  exist, the originally provided integer is returned back as a hex string.
        /// </summary>
        /// <param name="hexInteger">
        ///  Raw 32 bit unsigned integer to look up.
        /// </param>
        /// <returns>
        ///  Translation of the key, if it exists in the database. If not, the originally provided integer is returned as a hex string.
        /// </returns>
        public static string LookUpKey(uint hexInteger) {
            // Convert the integer to something we can look up
            string hexString = "0x" + hexInteger.ToString("X8").ToUpper().Replace("0X", "").PadLeft(8, '0');
            return (QBKeyDatabase.TryGetValue(hexString, out string value)) ? value : hexString;
        }
    }
}
