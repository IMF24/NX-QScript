using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Internal {
    internal static class Helpers {
        /// <summary>
        ///  Given a full string of text and an absolute position, prints out the line and column index within a string.
        /// </summary>
        /// <param name="text">
        ///  Full string of text.
        /// </param>
        /// <param name="position">
        ///  Position to calculate the line and column from.
        /// </param>
        /// <returns>
        ///  Yields a 2-tuple: 1st item is line, 2nd item is column.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static (int Line, int Column) GetLineAndColumn(string text, int position) {
            // Input position is out of bounds
            if (position < 0 || position > text.Length) {
                return (1, 1);
            }

            // Line and column numbers are 1-based
            int line = 1;
            int column = 1;
            int currentIndex = 0;

            foreach (var lineContent in text.Split('\n')) {

                // Include newline character in length
                int lineLength = lineContent.Length + 1;

                // Calculate column position
                if (currentIndex + lineLength > position) {
                    column = position - currentIndex + 1;
                    break;
                }

                // Move to the next line
                currentIndex += lineLength;
                line++;
            }

            return (line, column);
        }
    }
}
