// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  L E X E R
//      Used to lex text into usable tokens and QB bytecode
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Main class to lex text into usable tokens and QB bytecode.
    /// </summary>
    internal class QBCLexer {
        /// <summary>
        ///  Construct a new instance of <see cref="QBCLexer"/>.
        /// </summary>
        /// <param name="job">
        ///  The instance of <see cref="QBCCompileJob"/> that will be used with this lex operation.
        /// </param>
        /// <exception cref="ArgumentNullException"/>
        public QBCLexer(QBCCompileJob job) {
            // Assign job and check it
            Job = job;

            // If job is null, it's no good!
            if (Job is null) {
                throw new ArgumentNullException($"Cannot start a lex operation with a null job.");
            }

            // Set text on this object
            Text = Job.Input;

            // Offset text is being read at
            Offset = 0;

            // Stop read flag
            StopRead = false;

            // Lexed tokens
            Tokens = new List<QBCLexerToken>();

            // String cap character
            StringCap = '\'';

            // Inside various parts of the language
            InString = false;
            InWideString = false;
            IgnoreSpaces = false;
            InLongKey = false;

            // Comment type
            CommentType = QBCCommentType.None;

            // Script depth and depth stack
            ScriptDepth = 0;
            ScriptDepthStack = new List<int>();

            // Index of the token being handled in post-lex
            TokenIndex = 0;

            // Current token being handled
            CurrentToken = "";
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  The instance of <see cref="QBCCompileJob"/> used in the lex operation.
        /// </summary>
        public QBCCompileJob Job { get; set; }

        /// <summary>
        ///  The original source code text to be lexed into tokens.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///  The current read position in the text being lexed.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///  The tokens that have been lexed from the source text.
        /// </summary>
        public List<QBCLexerToken> Tokens { get; set; }

        /// <summary>
        ///  The current token being handled.
        /// </summary>
        public string CurrentToken { get; set; }

        /// <summary>
        ///  Are we currently parsing inside of a string?
        /// </summary>
        public bool InString { get; set; }

        /// <summary>
        ///  Are we currently parsing inside of a wide string?
        /// </summary>
        public bool InWideString { get; set; }

        /// <summary>
        ///  Are we currently ignoring spaces in the lex operation?
        /// </summary>
        public bool IgnoreSpaces { get; set; }

        /// <summary>
        ///  Are we currently inside of a raw or long QBKey?
        /// </summary>
        public bool InLongKey { get; set; }

        /// <summary>
        ///  Should the read operation be stopped?
        /// </summary>
        public bool StopRead { get; set; }

        /// <summary>
        ///  Type of comment currently being parsed.
        /// </summary>
        public QBCCommentType CommentType { get; set; }

        /// <summary>
        ///  The character currently being used to be handled as the string start/end delimiter.
        /// </summary>
        public char StringCap { get; set; }

        /// <summary>
        ///  The current script depth.
        /// </summary>
        public int ScriptDepth { get; set; }

        /// <summary>
        ///  The script depth stack, used in error handling.
        /// </summary>
        public List<int> ScriptDepthStack { get; set; }

        /// <summary>
        ///  In post lexing, which token are we looking at?
        /// </summary>
        public int TokenIndex { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Fail the lexer job with a reason.
        /// </summary>
        /// <param name="reason">
        ///  Text reason to fail the job with.
        /// </param>
        public void Fail(string reason = "") {
            // Get the line & column # that the error occurred on
            (int line, int column) = Helpers.GetLineAndColumn(Text, Offset);

            // Final job fail reason
            string finalFailReason = "";

            // Construct the first part of the string
            finalFailReason += $"[Ln {line}, Col {column}, @{Offset}] QBC LEXER FAIL: {reason}\n";

            // Get the text of the line that caused the problem
            string problematicLine = Text.Split('\n')[line - 1];

            // Create a new blank line that's the same number of characters as the problematic line
            string caretLine = new string(' ', problematicLine.Length);

            // At the column number, add a caret to show where the error occurred
            caretLine = caretLine.Substring(0, column) + "^" + caretLine.Substring(column + 1);

            // Add the problematic line and caret line to the final fail reason
            finalFailReason += $"    {problematicLine}\n";
            finalFailReason += $"    {caretLine}\n";

            // Actually fail the job now and stop all reading
            Job.Fail(finalFailReason);
            StopRead = true;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Are we in the top level of our QB? In THPG and later games, this is PakQB.
        /// </summary>
        /// <returns>
        ///  True if <see cref="ScriptDepth"/> is equal to 0, false otherwise.
        /// </returns>
        public bool TopLevel() {
            return (ScriptDepth == 0);
        }

        /// <summary>
        ///  Is this string comprised wholly of integer-like characters?
        /// </summary>
        /// <param name="str">
        ///  String to verify the characters of.
        /// </param>
        /// <returns>
        ///  True if all characters in <paramref name="str"/> are comprised of integer-like numeric characters, false otherwise.
        /// </returns>
        public bool IsIntString(string str) {
            foreach (char c in str) {
                if (c == 45 || (c >= 48 && c <= 57)) {
                    continue;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Is this string comprised wholly of floating point number-like characters?
        /// </summary>
        /// <param name="str">
        ///  String to verify the characters of.
        /// </param>
        /// <returns>
        ///  True if all characters in <paramref name="str"/> are comprised of float-like numeric characters, false otherwise.
        /// </returns>
        public bool IsFloatString(string str) {
            foreach (char c in str) {
                if (c == 46 || c == 45 || (c >= 48 && c <= 57)) {
                    continue;
                }
                return false;
            }
            return true;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  T O K E N   M A N A G E M E N T
        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Adds a new token to the list of lexed tokens.
        /// </summary>
        /// <param name="type">
        ///  The token type this token is.
        /// </param>
        /// <param name="value">
        ///  The value the token should have.
        /// </param>
        /// <param name="finalize">
        ///  If true, finalizes the current token being processed. Default is false.
        /// </param>
        /// <returns>
        ///  New <see cref="QBCLexerToken"/> based off the given type and value.
        /// </returns>
        public QBCLexerToken AddToken(string type, object value, bool finalize = false) {
            // Finalize the current token?
            if (finalize) {
                FinalizeToken();
            }

            // Create a new token!
            QBCLexerToken token = new QBCLexerToken();

            // Assign type and value
            token.Type = type;
            token.Value = value;

            // Add the token
            Tokens.Add(token);

            // Return the new token
            return token;
        }

        /// <summary>
        ///  Resets the current token being parsed.
        /// </summary>
        public void ResetToken() {
            CurrentToken = "";
        }

        /// <summary>
        ///  Finalize the current token being processed.
        /// </summary>
        public void FinalizeToken() {
            // Token is empty and not inside a string
            if ((CurrentToken is null || CurrentToken == "") && !InString) {
                return;
            }

            // Remove trailing whitespace
            CurrentToken = CurrentToken.Replace("\n", "").Replace("\r", "");

            // Create token type and value
            string tokenType = "";
            object tokenValue = CurrentToken;

            // If reading string, token will be a string
            if (InString) {
                tokenType = InWideString ? "wstring" : "string";

            } else if (!IgnoreSpaces) {
                // Was this a float?
                bool wasFloat = false;

                // There's a period!
                if (CurrentToken.Contains('.') && IsFloatString(CurrentToken)) {
                    bool floatParseGood = float.TryParse(CurrentToken, out float floatVal);

                    if (floatParseGood) {
                        tokenType = "float";
                        tokenValue = floatVal;
                        wasFloat = true;
                    }
                }

                // It wasn't a float, is it an integer?
                if (!wasFloat && IsIntString(CurrentToken)) {
                    bool intParseGood = int.TryParse(CurrentToken, out int intVal);
                    if (intParseGood) {
                        tokenType = "int";
                        tokenValue = intVal;
                    }
                }
            }

            // Try assuming it's a keyword if no type has been set yet
            if (tokenType == "") {
                tokenType = "keyword";
            }

            // Is it a keyword?
            if (tokenType == "keyword") {
                // At least 2 characters? Check for local argument delimiters (<>)
                if (CurrentToken.Length > 2) {

                    // Get the starting and ending characters
                    bool localStart = CurrentToken[0] == '<';
                    bool localEnd = CurrentToken[CurrentToken.Length - 1] == '>';

                    // Angle brackets opened, but did not close
                    if (localStart && !localEnd) {
                        this.Fail("Local argument found opened, but not closed: '<' found, but not '>'.");
                        return;

                        // Angle brackets closed, but did not open
                    } else if (localEnd && !localStart) {
                        this.Fail("Local argument found closed, but not opened: '>' found, but not '<'.");
                        return;
                    }

                }

                // In a string, do not check for keywords
                // (This feels redundant... tokenType can't possibly be either)
                if (tokenType != "string" && tokenType != "wstring") {
                    // What is our current token?
                    switch (CurrentToken.ToLower()) {
                        // Start of a script
                        case "script":
                            ScriptDepth++;
                            tokenType = "scriptstart";
                            CurrentToken = "";
                            ScriptDepthStack.Add(Tokens.Count);
                            break;

                        // End of a script
                        case "endscript":
                            ScriptDepth--;
                            tokenType = "scriptend";
                            CurrentToken = "";
                            ScriptDepthStack.RemoveAt(Tokens.Count - 1);
                            break;

                        // Word only keywords
                        case "default":
                        case "if":
                        case "else":
                        case "elseif":
                        case "endif":
                        case "begin":
                        case "repeat":
                        case "break":
                        case "switch":
                        case "case":
                        case "endswitch":
                        case "qs":
                        case "random":
                        case "random2":
                        case "randomrange":
                        case "randomrange2":
                        case "randomnorepeat":
                        case "randompermute":
                        case "randomfloat":
                        case "randominteger":
                        case "return":
                        case "not":
                        case "<...>":
                            tokenType = CurrentToken.ToLower();
                            CurrentToken = "";
                            break;
                    }
                }
            }

            // This is a string, are there quotation marks within it?
            // This will get duplicated somewhere else in the lexer
            if (tokenType == "string" || tokenType == "wstring") {

                string realValue = tokenValue as string;
                string inWhatString = (tokenType == "wstring") ? "wide string" : "string";
                string shouldUse = (tokenType == "wstring") ? "''" : "\\'\\'";

                if (realValue.Contains('\"')) {
                    this.Fail($"Illegal use of quotation marks in {inWhatString}; use double apostrophes ({shouldUse}) to emulate a quotation mark. -> ({realValue})");
                    return;
                }

            }

            // Add the new token and reset the current token
            var token = AddToken(tokenType, tokenValue);
            ResetToken();
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Is the lexer allowed to continue reading and lexing text?
        /// </summary>
        /// <returns>
        ///  True if a series of read-blocking tests return false, false otherwise.
        /// </returns>
        public bool CanRead() {
            // Instructed to stop reading?
            if (StopRead) return false;

            // Text is null or has no real text
            if (Text is null || Text.Trim() == "") return false;

            // Offset has gone out of bounds of length of text
            if (Offset >= Text.Length) return false;

            // We ARE allowed to read!
            return true;
        }

        /// <summary>
        ///  Read text from the input and lex into tokens.
        /// </summary>
        public void Read() {
            // Has no job, can't read!
            if (Job is null) {
                return;
            }

            // Assign text from job? Already did this...
            Text = Job.Input;
            Offset = 0;

            // Start lexing!
            while (CanRead()) {
                ReadText();
            }

            // Post read routine
            PostRead();
        }

        /// <summary>
        ///  Reads text (in the main loop of <see cref="Read()"/>, tokenizing the entire string input into serializable tokens.
        /// </summary>
        private void ReadText() {

        }

        /// <summary>
        ///  Post read routine for the lexer, which tweaks tokens as necessary and finalizes them.
        /// </summary>
        private void PostRead() {

        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>
    ///  Which type of comment are we currently inside of?
    /// </summary>
    internal enum QBCCommentType {
        /// <summary>
        ///  Not inside a comment.
        /// </summary>
        None = 0,
        /// <summary>
        ///  Inside of a single line comment.
        /// </summary>
        Line = 1,
        /// <summary>
        ///  Inside of a multi-line block comment.
        /// </summary>
        Block = 2,
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /// <summary>
    ///  Token used in the lexer.
    /// </summary>
    internal class QBCLexerToken {
        /// <summary>
        ///  Construct a new instance of <see cref="QBCLexerToken"/>.
        /// </summary>
        public QBCLexerToken() {
            Lexer = null;
            Type = "";
            Value = 0;
        }

        /// <summary>
        ///  The original lexer that spawned this token.
        /// </summary>
        public QBCLexer Lexer { get; set; }

        /// <summary>
        ///  The type of token this is.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///  Any arbitrary object that this token has associated with it.
        /// </summary>
        public object Value { get; set; }
    }
}
