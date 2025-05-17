// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  R E A D E R   C L A S S
//      Helper class for reading binary data from a file
//  
//      Class back ported from C# 12 to C# 7.3
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.IO;
using System.Text;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Helper class for reading binary data from a file.
    /// </summary>
    internal class Reader {
        /// <summary>
        ///  Construct a new instance of <see cref="Reader"/>.
        /// </summary>
        public Reader() {
            Data = new byte[0];
            LittleEndian = false;
            Offset = 0;
        }

        /// <summary>
        ///  Construct a new instance of <see cref="Reader"/> given an array of raw bytes.
        /// </summary>
        /// <param name="buffer">
        ///  Raw byte array of data to read from.
        /// </param>
        /// <param name="littleEndian">
        ///  If true, parses the file in little endian format.
        /// </param>
        /// <param name="startOffset">
        ///  Manually set the starting offset of the read cursor.
        /// </param>
        public Reader(byte[] buffer, bool littleEndian = false, int startOffset = 0) {
            Data = buffer;
            LittleEndian = littleEndian;
            Offset = startOffset;
        }

        /// <summary>
        ///  Construct a new instance of <see cref="Reader"/> given a file path.
        /// </summary>
        /// <param name="filePath">
        ///  Path to the file to retrieve data from.
        /// </param>
        /// <param name="littleEndian">
        ///  If true, parses the file in little endian format.
        /// </param>
        /// <param name="startOffset">
        ///  Manually set the starting offset of the read cursor.
        /// </param>
        public Reader(string filePath, bool littleEndian = false, int startOffset = 0) {
            Data = File.ReadAllBytes(filePath);
            LittleEndian = littleEndian;
            Offset = startOffset;
        }

        /// <summary>
        ///  Construct a new instance of <see cref="Reader"/> given a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">
        ///  Stream to read data from.
        /// </param>
        /// <param name="littleEndian">
        ///  If true, parses the file in little endian format.
        /// </param>
        /// <param name="startOffset">
        ///  Manually set the starting offset of the read cursor.
        /// </param>
        public Reader(Stream stream, bool littleEndian = false, int startOffset = 0) {
            Data = new byte[stream.Length];
            stream.Read(Data, 0, Data.Length);
            LittleEndian = littleEndian;
            Offset = startOffset;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Raw byte data of the file.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        ///  Current position of the read cursor in the file dat.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///  If true, the file is parsed in little endian byte order.
        /// </summary>
        public bool LittleEndian { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Where are we in reading the file? Returns the current offset.
        /// </summary>
        /// <returns>
        ///  The current position of the read head in the buffer.
        /// </returns>
        public int Tell() => Offset;

        /// <summary>
        ///  Manually set the position of the read cursor in the file.
        /// </summary>
        /// <param name="offset">
        ///  Position to set the read head to.
        /// </param>
        public void Seek(int offset) => Offset = offset;

        /// <summary>
        ///  Advance the read position of the data forward or backward by the given number of bytes. Positive numbers will
        ///  move the read cursor forward, negative numbers will move it backwards.
        /// </summary>
        /// <param name="offset">
        ///  Number of bytes to advance forward by.
        /// </param>
        public void Advance(int offset) => Offset += offset;

        /// <summary>
        ///  Returns true if the offset of the read head has reached or gone past the bounds of the <see cref="Data"/> array.
        /// </summary>
        /// <returns>
        ///  True if the offset is less than 0 or is greater than or equal to <see cref="Data"/>.Length, false otherwise.
        /// </returns>
        public bool ExceededBounds() => (Offset < 0 || Offset >= Data.Length);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Read an 8 bit unsigned integer and advance the position forward by 1 byte.
        /// </summary>
        /// <returns>
        ///  An 8 bit unsigned integer from the current read position.
        /// </returns>
        public byte UInt8() => Data[Offset++];

        /// <summary>
        ///  Read an 8 bit signed integer and advance the position forward by 1 byte.
        /// </summary>
        /// <returns>
        ///  An 8 bit signed integer from the current read position.
        /// </returns>
        public sbyte Int8() => (sbyte) Data[Offset++];

        /// <summary>
        ///  Read a 16 bit unsigned integer and advance the position forward by 2 bytes.
        /// </summary>
        /// <returns>
        ///  A 16 bit unsigned integer from the current read position.
        /// </returns>
        public ushort UInt16() => ReadUInt16();

        /// <summary>
        ///  Read a 16 bit signed integer and advance the position forward by 2 bytes.
        /// </summary>
        /// <returns>
        ///  A 16 bit signed integer from the current read position.
        /// </returns>
        public short Int16() => (short) ReadUInt16();

        /// <summary>
        ///  Read a 32 bit unsigned integer and advance the position forward by 4 bytes.
        /// </summary>
        /// <returns>
        ///  A 32 bit unsigned integer from the current read position.
        /// </returns>
        public uint UInt32() => ReadUInt32();

        /// <summary>
        ///  Reads a Neversoft QBKey as a UInt32, advances the position forward by 4 bytes, then attempts to decode
        ///  the key. If it cannot be decoded, the raw hex string is returned.
        /// </summary>
        /// <returns>
        ///  String of text containing either a translated QBKey, or its raw hexadecimal string.
        /// </returns>
        public string QBKey() {
            return QB.LookUpKey(UInt32());
        }

        /// <summary>
        ///  Read a 32 bit signed integer and advance the position forward by 4 bytes.
        /// </summary>
        /// <returns>
        ///  A 32 bit signed integer from the current read position.
        /// </returns>
        public int Int32() => (int) ReadUInt32();

        /// <summary>
        ///  Read a 64 bit unsigned integer and advance the position forward by 8 bytes.
        /// </summary>
        /// <returns>
        ///  A 64 bit unsigned integer from the current read position.
        /// </returns>
        public ulong UInt64() => ReadUInt64();

        /// <summary>
        ///  Read a 64 bit signed integer and advance the position forward by 8 bytes.
        /// </summary>
        /// <returns>
        ///  A 64 bit signed integer from the current read position.
        /// </returns>
        public long Int64() => (long) ReadUInt64();

        /// <summary>
        ///  Read a 32 bit single precision floating point number and advance the position forward by 4 bytes.
        /// </summary>
        /// <returns>
        ///  A 32 bit single precision floating point number from the current read position.
        /// </returns>
        public float Float() => BitConverter.ToSingle(ReadBytesEndianAware(4), 0);

        /// <summary>
        ///  Read a 32 bit single precision floating point number and advance the position forward by 4 bytes.
        ///  This is an alias for <see cref="Float"/>.
        /// </summary>
        /// <returns>
        ///  A 32 bit single precision floating point number from the current read position.
        /// </returns>
        public float Float32() => Float();

        /// <summary>
        ///  Read a 64 bit double precision floating point number and advance the position forward by 8 bytes.
        /// </summary>
        /// <returns>
        ///  A 64 bit double precision floating point number from the current read position.
        /// </returns>
        public double Double() => BitConverter.ToDouble(ReadBytesEndianAware(8), 0);

        /// <summary>
        ///  Read a 64 bit double precision floating point number and advance the position forward by 8 bytes.
        ///  This is an alias for <see cref="Double"/>.
        /// </summary>
        /// <returns>
        ///  A 64 bit double precision floating point number from the current read position.
        /// </returns>
        public double Float64() => Double();

        /// <summary>
        ///  Read a boolean value and advance the position forward by 1 byte.
        /// </summary>
        /// <returns>
        ///  True if the current byte is a non-zero value, false otherwise.
        /// </returns>
        public bool Bool() => UInt8() != 0;

        /// <summary>
        ///  Read a string of a given length from the file and advance the position forward by that many bytes.
        /// </summary>
        /// <param name="length">
        ///  Number of characters, starting at the current read position, to read.
        /// </param>
        /// <returns>
        ///  String of characters, starting from the current read position, and is read up until the given length.
        /// </returns>
        public string String(int length) {
            string value = Encoding.UTF8.GetString(Data, Offset, length);
            Advance(length);
            return value;
        }

        /// <summary>
        ///  Read an Int32 value of string length, then reads a string following it. Advances the position forward by
        ///  4 bytes plus the detected length of the string.
        /// </summary>
        /// <returns>
        ///  A string from the file after parsing the length of the string first.
        /// </returns>
        public string NumString() {
            int length = Int32();
            return String(length);
        }

        /// <summary>
        ///  Read a null-padded string. Advances the position forward by the length of the string until a null
        ///  byte was reached.
        /// </summary>
        /// <returns>
        ///  String from the file, parsed until a null byte was reached.
        /// </returns>
        public string TermString() {
            var sb = new StringBuilder();
            char chr = (char) UInt8();
            while (chr != 0x00) {
                sb.Append(chr);
                chr = (char) UInt8();
            }
            return sb.ToString();
        }

        /// <summary>
        ///  Read the next 'n' raw bytes from the buffer and advance the position forward by that many bytes.
        /// </summary>
        /// <param name="length">
        ///  Number of bytes forward to read.
        /// </param>
        /// <returns>
        ///  Array of bytes from the current read position, up until the given length.
        /// </returns>
        public byte[] Chunk(int length) {
            byte[] result = new byte[length];
            Array.Copy(Data, Offset, result, 0, length);
            Advance(length);
            return result;
        }

        /// <summary>
        ///  Read the next 'n' raw bytes from the buffer and advance the position forward by that many bytes.
        /// </summary>
        /// <param name="length">
        ///  Number of bytes forward to read.
        /// </param>
        /// <returns>
        ///  Array of bytes from the current read position, up until the given length.
        /// </returns>
        public byte[] Chunk(uint length) => Chunk((int)length);

        /// <summary>
        ///  Skip to the nearest 'n' bytes.
        /// </summary>
        /// <param name="alignment">
        ///  Nearest 'n' bytes to be snapped to.
        /// </param>
        public void SkipToNearest(int alignment) {
            int remainder = Offset % alignment;
            if (remainder != 0) {
                Seek(Offset + (alignment - remainder));
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Internally reads an unsigned short.
        /// </summary>
        /// <returns>
        ///  16 bit unsigned integer from the next 2 bytes.
        /// </returns>
        private ushort ReadUInt16() {
            byte[] bytes = ReadBytesEndianAware(2);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        ///  Internally reads an unsigned integer.
        /// </summary>
        /// <returns>
        ///  32 bit unsigned integer from the next 4 bytes.
        /// </returns>
        private uint ReadUInt32() {
            byte[] bytes = ReadBytesEndianAware(4);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        ///  Internally reads an unsigned long.
        /// </summary>
        /// <returns>
        ///  64 bit unsigned integer from the next 8 bytes.
        /// </returns>
        private ulong ReadUInt64() {
            byte[] bytes = ReadBytesEndianAware(8);
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        ///  In an endian aware sense, reads and advances forward the given number of bytes.
        /// </summary>
        /// <param name="count">
        ///  Number of bytes forward to read.
        /// </param>
        /// <returns>
        ///  Array of bytes that is as long as the given <paramref name="count"/>.
        /// </returns>
        private byte[] ReadBytesEndianAware(int count) {
            byte[] bytes = new byte[count];
            Array.Copy(Data, Offset, bytes, 0, count);
            Advance(count);

            if (BitConverter.IsLittleEndian != LittleEndian) {
                Array.Reverse(bytes);
            }

            return bytes;
        }
    }
}
