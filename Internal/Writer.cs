// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  W R I T E R   C L A S S
//      Helper class for writing binary data to a file
//  
//      Class back ported from C# 12 to C# 7.3
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.IO;
using System.Text;
using System.Linq;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Helper class for writing binary data to a file.
    /// </summary>
    public class Writer {
        /// <summary>
        ///  Construct a new instance of <see cref="Writer"/>.
        /// </summary>
        public Writer() {
            Data = new MemoryStream();
            LittleEndian = false;
        }

        /// <summary>
        ///  Construct a new instance of <see cref="Writer"/> given the default endianness.
        /// </summary>
        /// <param name="littleEndian">
        ///  If true, tells the Writer instance to write data in little endian format.
        /// </param>
        public Writer(bool littleEndian) {
            Data = new MemoryStream();
            LittleEndian = littleEndian;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Raw form binary data written thus far.
        /// </summary>
        public MemoryStream Data { get; private set; }

        /// <summary>
        ///  If set to true, data is written in little endian byte order.
        /// </summary>
        public bool LittleEndian { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Inverts the endianness of the <see cref="Writer"/> instance.
        /// </summary>
        public void InvertEndian() {
            LittleEndian = !LittleEndian;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Where are we in writing the file? Returns the offset.
        /// </summary>
        /// <returns>
        ///  The 64 bit signed integer representing the current offset in the file.
        /// </returns>
        public long Tell() {
            return Data.Position;
        }

        /// <summary>
        ///  Manually set the read position in the file.
        /// </summary>
        /// <param name="offset">
        ///  Location to seek to in the file.
        /// </param>
        public void Seek(long offset) {
            Data.Seek(offset, SeekOrigin.Begin);
        }

        /// <summary>
        ///  Ensure that 'n' byte(s) have been allocated to the underlying <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="len">
        ///  Number of bytes to ensure that have been allocated.
        /// </param>
        public void EnsureBytes(long len) {
            long newLen = Tell() + len;
            if (newLen > Data.Length) {
                long toAllocate = newLen - Data.Length;
                Data.Write(new byte[toAllocate], 0, (int)toAllocate);
            }
        }

        /// <summary>
        ///  Pad the next 'n' bytes with a specific byte of padding.
        /// </summary>
        /// <param name="len">
        ///  Number of bytes to pad.
        /// </param>
        /// <param name="padWith">
        ///  Byte to pad the data with. Default is 0x00.
        /// </param>
        public void Pad(long len, byte padWith = 0x00) {
            for (long i = 0; i < len; i++) {
                UInt8(padWith);
            }
        }

        /// <summary>
        ///  Pad the nearest 'n' bytes with the given byte of padding.
        /// </summary>
        /// <param name="snap">
        ///  Nearest 'n' bytes to pad with the padding byte.
        /// </param>
        /// <param name="padWith">
        ///  Byte to pad the data with. Default is 0x00.
        /// </param>
        /// <returns>
        ///  Amount of bytes that were padded, if any.
        /// </returns>
        public long PadToNearest(long snap, byte padWith = 0x00) {
            long extra = Tell() % snap;
            long toPad = 0;
            if (extra > 0) {
                toPad = snap - extra;
                Pad(toPad, padWith);
            }
            return toPad;
        }

        /// <summary>
        ///  Converts the underlying instance of <see cref="MemoryStream"/> to a byte array.
        /// </summary>
        /// <returns>
        ///  Array of bytes containing a representation of the data written.
        /// </returns>
        public byte[] ToArray() {
            return Data.ToArray();
        }

        /// <summary>
        ///  Returns the contents of the underlying <see cref="MemoryStream"/> as a byte array. This is an alias for <see cref="ToArray"/>.
        /// </summary>
        /// <returns>
        ///  Array of bytes containing a representation of the data written.
        /// </returns>
        public byte[] GetBytes() {
            return ToArray();
        }

        /// <summary>
        ///  Writes all bytes in this instance of <see cref="Writer"/> to the disk at the given file path.
        /// </summary>
        /// <param name="filePath">
        ///  Path to write the bytes to.
        /// </param>
        public void WriteAllBytes(string filePath) {
            File.WriteAllBytes(filePath, ToArray());
        }

        /// <summary>
        ///  Gets the size of this <see cref="Writer"/> instance as a 32 bit unsigned integer.
        /// </summary>
        /// <returns>
        ///  32 bit unsigned integer containing the length of the stored buffer stream.
        /// </returns>
        public uint GetSize() {
            return (uint) ToArray().Length;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Write an 8 bit unsigned integer to the file and advance the position forward by 1 byte.
        /// </summary>
        /// <param name="value">
        ///  8 bit unsigned integer to be written.
        /// </param>
        public void UInt8(byte value) {
            Data.WriteByte(value);
        }

        /// <summary>
        ///  Write an 8 bit signed integer to the file and advance the position forward by 1 byte.
        /// </summary>
        /// <param name="value">
        ///  8 bit signed integer to be written.
        public void Int8(sbyte value) {
            UInt8((byte) value);
        }

        // - - - - - - - - - - - - - -

        /// <summary>
        ///  Write a 16 bit unsigned integer to the file and advance the position forward by 2 bytes.
        /// </summary>
        /// <param name="value">
        ///  16 bit unsigned integer to be written.
        /// </param>
        public void UInt16(ushort value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///  Write a 16 bit signed integer to the file and advance the position forward by 2 bytes.
        /// </summary>
        /// <param name="value">
        ///  16 bit signed integer to be written.
        /// </param>
        public void Int16(short value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        // - - - - - - - - - - - - - -

        /// <summary>
        ///  Write a 32 bit unsigned integer to the file and advance the position forward by 4 bytes.
        /// </summary>
        /// <param name="value">
        ///  32 bit unsigned integer to be written.
        /// </param>
        public void UInt32(uint value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///  Write a Neversoft QBKey (as a string that gets converted to hash if necessary) and advance the position forward by 4 bytes.
        /// </summary>
        /// <param name="key">
        ///  String key to write. If "0x" is not contained in the string, it will be converted to a hash.
        /// </param>
        public void QBKey(string key) {
            // Is the key a hash?
            // We've got some tests we'd like to perform just in case!
            bool isKeyHash = false;

            // For the input to be a hash, it must be 10 characters long.
            // It should start with 0x, and then everything after it
            // should be a valid hexadecimal number.

            // If it's not, then treat it like it's not already a hash.
            if (key.Length == 10 && key.StartsWith("0x")) {
                isKeyHash = true;
                for (var i = 2; i < key.Length; i++) {
                    if (!"0123456789ABCDEF".Contains(key[i])) {
                        isKeyHash = false;
                        break;
                    }
                }
            }

            // If it's a hash, then convert it to a number (using Convert.ToUInt32).
            // Otherwise, we'll hash it, then get the number (QB.Hexify).
            if (isKeyHash) {
                UInt32(Convert.ToUInt32(key, 16));
            } else {
                UInt32(QB.Hexify(key));
            }
        }

        /// <summary>
        ///  Write a Neversoft QBKey (as a UInt32 number) to the file and advance the position forward by 4 bytes.
        /// </summary>
        /// <param name="keyHex">
        ///  Raw UInt32 QBKey hash to write.
        /// </param>
        public void QBKey(uint keyHex) {
            UInt32(keyHex);
        }

        /// <summary>
        ///  Write a 32 bit signed integer to the file and advance the position forward by 4 bytes.
        /// </summary>
        /// <param name="value">
        ///  32 bit signed integer to be written.
        /// </param>
        public void Int32(int value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        // - - - - - - - - - - - - - -

        /// <summary>
        ///  Write a 64 bit unsigned integer to the file and advance the position forward by 8 bytes.
        /// </summary>
        /// <param name="value">
        ///  64 bit unsigned integer to be written.
        /// </param>
        public void UInt64(ulong value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///  Write a 64 bit signed integer to the file and advance the position forward by 8 bytes.
        /// </summary>
        /// <param name="value">
        ///  64 bit signed integer to be written.
        /// </param>
        public void Int64(long value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        // - - - - - - - - - - - - - -

        /// <summary>
        ///  Write a 32 bit single precision floating point number to the file and advance the position forward by 4 bytes.
        /// </summary>
        /// <param name="value">
        ///  32 bit single precision floating point number to be written.
        /// </param>
        public void Float(float value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///  Write a 32 bit single precision floating point number to the file and advance the position forward by 4 bytes.
        ///  This is an alias for <see cref="Float"/>.
        /// </summary>
        /// <param name="value">
        ///  32 bit single precision floating point number to be written.
        /// </param>
        public void Float32(float value) {
            Float(value);
        }

        /// <summary>
        ///  Write a 64 bit double precision floating point number to the file and advance the position forward by 8 bytes.
        /// </summary>
        /// <param name="value">
        ///  64 bit double precision floating point number to be written.
        /// </param>
        public void Double(double value) {
            WriteEndian(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///  Write a 64 bit double precision floating point number to the file and advance the position forward by 8 bytes.
        ///  This is an alias for <see cref="Double"/>.
        /// </summary>
        /// <param name="value">
        ///  64 bit double precision floating point number to be written.
        /// </param>
        public void Float64(double value) {
            Double(value);
        }

        // - - - - - - - - - - - - - -

        /// <summary>
        ///  Write an ASCII string to the file and advance the position forward by the number of characters in the string.
        /// </summary>
        /// <param name="value">
        ///  ASCII string of text to be written.
        /// </param>
        public void String(string value) {
            byte[] data = Encoding.ASCII.GetBytes(value);
            Data.Write(data, 0, data.Length);
        }

        /// <summary>
        ///  Write a string to the file as 2 values: First the length of the string as a UInt32, followed by the actual
        ///  text string itself. Advance the position forward by 4 bytes + the number of characters in the string.
        /// </summary>
        /// <param name="value">
        ///  String of text to be written.
        /// </param>
        public void NumString(string value) {
            UInt32((uint)value.Length);
            String(value);
        }

        /// <summary>
        ///  Writes an ASCII string to the buffer. Advance the position forward by the number of characters in the
        ///  string, optionally plus 1 if the terminaton character is requested to be written.
        /// </summary>
        /// <param name="str">
        ///  The string of text to be written.
        /// </param>
        /// <param name="term">
        ///  If true, writes a terminating 0x00 byte at the end of the string.
        /// </param>
        public void ASCIIString(string str, bool term = false) {
            for (int i = 0; i < str.Length; i++) {
                UInt8((byte)str[i]);
            }
            if (term) {
                UInt8(0x00);
            }
        }

        /// <summary>
        ///  Writes a wide ASCII string to the buffer. Advance the position forward by the number of characters in the
        ///  string times 2, optionally plus 2 if the terminaton character is requested to be written.
        /// </summary>
        /// <param name="str">
        ///  The string of text to be written.
        /// </param>
        /// <param name="term">
        ///  If true, writes a terminating 0x00 unsigned short at the end of the string.
        /// </param>
        public void ASCIIWString(string str, bool term = false) {
            for (int i = 0; i < str.Length; i++) {
                UInt16(str[i]);
            }
            if (term) {
                UInt16(0);
            }
        }

        // - - - - - - - - - - - - - -

        /// <summary>
        ///  Combine a series of bytes into the current data stream.
        /// </summary>
        /// <param name="bytes">
        ///  Raw array of bytes to append into the stream.
        /// </param>
        public void Combine(byte[] bytes) {
            Data.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        ///  Combine the bytes of another instance of <see cref="Writer"/> into the current data stream.
        /// </summary>
        /// <param name="writer">
        ///  Instance of <see cref="Writer"/> to append the bytes from.
        /// </param>
        public void Combine(Writer writer) {
            byte[] bytes = writer.ToArray();
            Data.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        ///  Combine the contents of another <see cref="Stream"/> into the current data stream.
        /// </summary>
        /// <param name="stream">
        ///  <see cref="Stream"/> to append the bytes from.
        /// </param>
        public void Combine(Stream stream) {
            stream.CopyTo(Data);
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  In an endian aware sense, writes a set of bytes to the underlying stream in <see cref="Data"/>.
        /// </summary>
        /// <param name="bytes">
        ///  Set of bytes to write into the stream.
        /// </param>
        private void WriteEndian(byte[] bytes) {
            if (!LittleEndian) {
                Array.Reverse(bytes);
            }
            Data.Write(bytes, 0, bytes.Length);
        }
    }
}
