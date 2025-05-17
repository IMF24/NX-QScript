// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  C O N S T A N T S
//      All constant values used in the NX_QScript namespaces
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript {
    /// <summary>
    ///  All constant values used in the <see cref="NX_QScript"/> namespaces.
    /// </summary>
    internal static class Constants {
        /// <summary>
        ///  Token that denotes the end of a QScript file.
        /// </summary>
        public const byte SCRIPT_TOKEN_ENDOFFILE = 0x00;

        /// <summary>
        ///  Token that denotes the end of a line.
        /// </summary>
        public const byte SCRIPT_TOKEN_ENDOFLINE = 0x01;

        /// <summary>
        ///  Token that denotes the end of a line, used in THAW for debug purposes.
        /// </summary>
        public const byte SCRIPT_TOKEN_ENDOFLINENUMBER = 0x02;

        /// <summary>
        ///  Token that denotes the start of a struct object.
        /// </summary>
        public const byte SCRIPT_TOKEN_STARTSTRUCT = 0x03;

        /// <summary>
        ///  Token that denotes the end of a struct object.
        /// </summary>
        public const byte SCRIPT_TOKEN_ENDSTRUCT = 0x04;
        
        /// <summary>
        ///  Token that denotes the start of an array of items.
        /// </summary>
        public const byte SCRIPT_TOKEN_STARTARRAY = 0x05;

        /// <summary>
        ///  Token that denotes the end of an array of items.
        /// </summary>
        public const byte SCRIPT_TOKEN_ENDARRAY = 0x06;

        /// <summary>
        ///  The equal sign (=) token, used in assignment.
        /// </summary>
        public const byte SCRIPT_TOKEN_EQUALS = 0x07;

        /// <summary>
        ///  The dot (.) token, used in this context as a struct value accessor.
        /// </summary>
        public const byte SCRIPT_TOKEN_DOT = 0x08;

        /// <summary>
        ///  The comma (,) token, used in n-dimensional pairs (e.g. 2D pairs, 3D vectors).
        /// </summary>
        public const byte SCRIPT_TOKEN_COMMA = 0x09;

        /// <summary>
        ///  The minus (-) token, used for arithmetic subtraction.
        /// </summary>
        public const byte SCRIPT_TOKEN_MINUS = 0x0A;

        /// <summary>
        ///  The add (+) token, used for arithmetic addition.
        /// </summary>
        public const byte SCRIPT_TOKEN_ADD = 0x0B;

        /// <summary>
        ///  The divide (/) token, used for arithmetic division.
        /// </summary>
        public const byte SCRIPT_TOKEN_DIVIDE = 0x0C;

        /// <summary>
        ///  The mulitply (*) token, used for arithmetic multiplication.
        /// </summary>
        public const byte SCRIPT_TOKEN_MULTIPLY = 0x0D;

        /// <summary>
        ///  Token that denotes an opening pair of parentheses.
        /// </summary>
        public const byte SCRIPT_TOKEN_OPENPARENTH = 0x0E;

        /// <summary>
        ///  Token that denotes closing an opened pair of parentheses.
        /// </summary>
        public const byte SCRIPT_TOKEN_CLOSEPARENTH = 0x0F;

        /// <summary>
        ///  Unknown token.
        /// </summary>
        public const byte SCRIPT_TOKEN_DEBUGINFO = 0x10;

        /// <summary>
        ///  The "same as" token, used for equality comparisons.
        /// </summary>
        public const byte SCRIPT_TOKEN_SAMEAS = 0x11;

        /// <summary>
        ///  Less than token, used for value comparisons to check if one value is smaller than the other.
        /// </summary>
        public const byte SCRIPT_TOKEN_LESSTHAN = 0x12;

        /// <summary>
        ///  Less than or equal to token, used for comparisons checking if one value is smaller or equivalent to the other.
        /// </summary>
        public const byte SCRIPT_TOKEN_LESSTHANEQUAL = 0x13;

        /// <summary>
        ///  Greater than token, used for value comparisons to check if one value is larger than the other.
        /// </summary>
        public const byte SCRIPT_TOKEN_GREATERTHAN = 0x14;

        /// <summary>
        ///  Greater than or equal to token, used for comparisons checking if one value is larger or equivalent to the other.
        /// </summary>
        public const byte SCRIPT_TOKEN_GREATERTHANEQUAL = 0x15;

        /// <summary>
        ///  The Name token following a Pointer token.
        /// </summary>
        public const byte SCRIPT_TOKEN_NAME = 0x16;

        /// <summary>
        ///  An integer value, always assumed to be an Int32.
        /// </summary>
        public const byte SCRIPT_TOKEN_INTEGER = 0x17;

        /// <summary>
        ///  A hexadecimal integer.
        /// </summary>
        public const byte SCRIPT_TOKEN_HEXINTEGER = 0x18;

        /// <summary>
        ///  An enum token.
        /// </summary>
        public const byte SCRIPT_TOKEN_ENUM = 0x19;

        /// <summary>
        ///  A floating point value, inferred as Float32.
        /// </summary>
        public const byte SCRIPT_TOKEN_FLOAT = 0x1A;

        /// <summary>
        ///  The string token ('), used for storing text.
        /// </summary>
        public const byte SCRIPT_TOKEN_STRING = 0x1B;
        public const byte SCRIPT_TOKEN_LOCALSTRING = 0x1C;
        public const byte SCRIPT_TOKEN_ARRAY = 0x1D;
        public const byte SCRIPT_TOKEN_VECTOR = 0x1E;
        public const byte SCRIPT_TOKEN_PAIR = 0x1F;

        /// <summary>
        ///  The begin keyword, used to instantiate a loop.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_BEGIN = 0x20;

        /// <summary>
        ///  The repeat keyword, which denotes the end of the loop body, optionally containing the number of times the loop should execute.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_REPEAT = 0x21;

        /// <summary>
        ///  The break keyword, used to end a loop's execution prematurely.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_BREAK = 0x22;

        /// <summary>
        ///  The script keyword, used to denote the start of a script, containing its name definiton.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_SCRIPT = 0x23;

        /// <summary>
        ///  The endscript keyword, used to denote the end of a script body.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_ENDSCRIPT = 0x24;

        /// <summary>
        ///  The if keyword seen in THUG1 compiled scripts.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_IF = 0x25;

        /// <summary>
        ///  The else keyword seen in THUG1 compiled scripts.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_ELSE = 0x26;

        /// <summary>
        ///  The elseif keyword, used to denote a condition to be checked if the previous one did not evaluate as true.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_ELSEIF = 0x27;

        /// <summary>
        ///  The endif keyword, used to denote the end of an if statement block.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_ENDIF = 0x28;

        /// <summary>
        ///  The return keyword, which gives back a value to the caller.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_RETURN = 0x29;
        public const byte SCRIPT_TOKEN_UNDEFINED = 0x2A;
        public const byte SCRIPT_TOKEN_CHECKSUM_NAME = 0x2B;

        /// <summary>
        ///  All arguments (<c>&lt;...&gt;</c>), which refers to every locally accessible argument inside of a script.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_ALLARGS = 0x2C;
        public const byte SCRIPT_TOKEN_ARG = 0x2D;
        public const byte SCRIPT_TOKEN_JUMP = 0x2E;
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOM = 0x2F;
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOM_RANGE = 0x30;
        public const byte SCRIPT_TOKEN_AT = 0x31;
        public const byte SCRIPT_TOKEN_OR = 0x32;
        public const byte SCRIPT_TOKEN_AND = 0x33;
        public const byte SCRIPT_TOKEN_XOR = 0x34;
        public const byte SCRIPT_TOKEN_SHIFT_LEFT = 0x35;
        public const byte SCRIPT_TOKEN_SHIFT_RIGHT = 0x36;
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOM2 = 0x37;
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOM_RANGE2 = 0x38;
        public const byte SCRIPT_TOKEN_KEYWORD_NOT = 0x39;
        public const byte SCRIPT_TOKEN_KEYWORD_AND = 0x3A;
        public const byte SCRIPT_TOKEN_KEYWORD_OR = 0x3B;
        public const byte SCRIPT_TOKEN_KEYWORD_SWITCH = 0x3C;
        public const byte SCRIPT_TOKEN_KEYWORD_ENDSWITCH = 0x3D;
        public const byte SCRIPT_TOKEN_KEYWORD_CASE = 0x3E;
        public const byte SCRIPT_TOKEN_KEYWORD_DEFAULT = 0x3F;
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOM_NO_REPEAT = 0x40;
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOM_PERMUTE = 0x41;
        public const byte SCRIPT_TOKEN_COLON = 0x42;
        public const byte SCRIPT_TOKEN_RUNTIME_CFUNCTION = 0x43;
        public const byte SCRIPT_TOKEN_RUNTIME_MEMBERFUNCTION = 0x44;
        public const byte SCRIPT_TOKEN_KEYWORD_USEHEAP = 0x45;
        public const byte SCRIPT_TOKEN_KEYWORD_UNKNOWN = 0x46;
        public const byte SCRIPT_TOKEN_KEYWORD_FASTIF = 0x47;
        public const byte SCRIPT_TOKEN_KEYWORD_FASTELSE = 0x48;
        public const byte SCRIPT_TOKEN_SHORTJUMP = 0x49;
        public const byte SCRIPT_TOKEN_INLINEPACKSTRUCT = 0x4A;

        /// <summary>
        ///  The global dereference token ($), used to point (or refer) to a value in global scope.
        /// </summary>
        public const byte SCRIPT_TOKEN_ARGUMENTPACK = 0x4B;

        /// <summary>
        ///  The wide string token ("").
        /// </summary>
        public const byte SCRIPT_TOKEN_WIDESTRING = 0x4C;

        /// <summary>
        ///  The not equal token (!=), used to check if a value is not equivalent to the other.
        /// </summary>
        public const byte SCRIPT_TOKEN_NOTEQUAL = 0x4D;

        /// <summary>
        ///  The QS string token (<c>qs()</c>), used for localized, translatable strings.
        /// </summary>
        public const byte SCRIPT_TOKEN_STRINGQS = 0x4E;

        /// <summary>
        ///  Token used for the RandomFloat random type.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOMFLOAT = 0x4F;

        /// <summary>
        ///  Token used for the RandomInteger random type.
        /// </summary>
        public const byte SCRIPT_TOKEN_KEYWORD_RANDOMINTEGER = 0x50;
    }
}
