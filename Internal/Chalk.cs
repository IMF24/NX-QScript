// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  C H A L K
//      Used for colored console text
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.Collections.Generic;
using System.Linq;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Helper class with methods to print colored text to the Windows console.
    /// </summary>
    internal static class Chalk {
        /// <summary>
        ///  Writes a string to the console in the default text color. Alias for <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="message">
        ///  Text to be written to the console.
        /// </param>
        public static void Write(string message) {
            Console.WriteLine(message);
        }

        /// <summary>
        ///  Writes a string to the console in the default text color. Alias for <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="message">
        ///  Text to be written to the console.
        /// </param>
        public static void Log(string message) {
            // Another alias for Console.WriteLine
            Console.WriteLine(message);
        }

        /// <summary>
        ///  Send an error by printing a string to the console in red text.
        /// </summary>
        /// <param name="message">
        ///  Text to be printed.
        /// </param>
        /// <param name="light">
        ///  Optional: If set, the shade of red will appear lighter than normal.
        /// </param>
        public static void Error(string message, bool light = false) {
            Console.ForegroundColor = (light) ? ConsoleColor.Red : ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        ///  Warn the user by printing a string to the console in yellow text.
        /// </summary>
        /// <param name="message">
        ///  Text to be printed.
        /// </param>
        /// <param name="dark">
        ///  Optional: If set, the shade of yellow will appear darker than normal.
        /// </param>
        public static void Warn(string message, bool dark = false) {
            Console.ForegroundColor = (dark) ? ConsoleColor.DarkYellow : ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        ///  Send a success message to the user by printing a string to the console in green text.
        /// </summary>
        /// <param name="message">
        ///  Text to be printed.
        /// </param>
        public static void Success(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        ///  Over time, prints a gradually changing in color string to the console.
        /// </summary>
        /// <param name="message">
        ///  Main text to be printed.
        /// </param>
        /// <param name="current">
        ///  Current value in the iteration.
        /// </param>
        /// <param name="max">
        ///  Max value of the iteration.
        /// </param>
        /// <returns>
        ///  The percentage of the current value towards the max value.
        /// </returns>
        public static decimal Progress(string message, int current, int max) {
            // Get percent between current and max
            decimal percent = (decimal) current / max;

            // Change color based on percent
            ConsoleColor colorToUse;
            if (percent >= 0.75M) {
                colorToUse = ConsoleColor.Green;
            } else if (percent >= 0.5M) {
                colorToUse = ConsoleColor.Yellow;
            } else if (percent >= 0.25M) {
                colorToUse = ConsoleColor.DarkYellow;
            } else {
                colorToUse = ConsoleColor.DarkRed;
            }
            Console.ForegroundColor = colorToUse;

            // Round percentage multiplied by 100
            int visiblePercent = (int) (percent * 100);
            string visiblePercentString = $"[{current} / {max} - {visiblePercent}%] {message}";
            Console.WriteLine(visiblePercentString);

            // Reset console draw color
            Console.ForegroundColor = ConsoleColor.Gray;

            // Return the percent value
            return percent;
        }

        /// <summary>
        ///  Over time, in the same line of the console, prints a faked bar to the console that will fill up over time.
        /// </summary>
        /// <param name="current">
        ///  Current value in the iteration.
        /// </param>
        /// <param name="max">
        ///  Max value of the iteration.
        /// </param>
        /// <param name="barFillLength">
        ///  Number of characters in the terminal to fill up with the progress bar.
        /// </param>
        /// <param name="colorChanging">
        ///  If true, the bar will change color based on the percentage of the current value towards the max value. Default is true.
        /// </param>
        /// <param name="fillChar">
        ///  The character to fill the progress bar with. Default is the equal sign ('=').
        /// </param>
        /// <returns>
        ///  The percentage of the current value towards the max value.
        /// </returns>
        public static decimal ProgressBar(int current, int max, int barFillLength = 50, bool colorChanging = true, char fillChar = '=') {
            // Get percent between current and max
            decimal percent = (decimal) current / max;

            // Change color based on percent if desired
            // Otherwise, just draw it in gray
            ConsoleColor colorToUse;
            if (colorChanging) {
                if (percent >= 0.75M) {
                    colorToUse = ConsoleColor.Green;
                } else if (percent >= 0.5M) {
                    colorToUse = ConsoleColor.Yellow;
                } else if (percent >= 0.25M) {
                    colorToUse = ConsoleColor.DarkYellow;
                } else {
                    colorToUse = ConsoleColor.DarkRed;
                }
            } else {
                colorToUse = ConsoleColor.Gray;
            }

            // Set console draw color
            Console.ForegroundColor = colorToUse;

            // Draw the progress bar (shrinked for demonstration purposes)
            // Example: 1 / 10 [=         ] Total: 10.0%

            // Note: When this method is called, it needs to overwrite the
            // previous line to give us a clean progress bar
            Console.Write("\r");

            // Calculate the length of the bar
            int barFill = (int) (percent * barFillLength);

            // Draw the progress bar (draw thousands with commas)
            Console.Write($"{current:N0} / {max:N0} [");
            for (var i = 0; i < barFill; i++) {
                Console.Write(fillChar);
            }
            for (var i = 0; i < barFillLength - barFill; i++) {
                Console.Write(' ');
            }
            Console.Write($"] Total: {percent * 100:0.0}%");

            // Reset console draw color
            Console.ForegroundColor = ConsoleColor.Gray;

            // Return the percent value
            return percent;
        }

        /// <summary>
        ///  For CLI use: Prints a required argument for a command in light red text.
        /// </summary>
        /// <param name="message">
        ///  Text to be printed.
        /// </param>
        public static void RequiredArg(string message) {
            Error(message, true);
        }

        /// <summary>
        ///  For CLI use: Prints an optional argument for a command in light blue text.
        /// </summary>
        /// <param name="message">
        ///  Text to be printed.
        /// </param>
        public static void OptionalArg(string message) {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  A S K   P R O M P T   M E T H O D S
        //      Helper methods for asking the user for input via
        //      the console
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Ask the user to provide an input for a specific prompt and returns the result. This is a fancier
        ///  alias for <see cref="Console.ReadLine"/>, but with some styling on the prompted text.
        /// </summary>
        /// <param name="message">
        ///  The prompt to display to the user.
        /// </param>
        /// <param name="readPathMode">
        ///  If true, the user's input will be treated as a file path and will be trimmed of any quotes or spaces.
        /// </param>
        /// <returns>
        ///  Any string of text representing the user's input. An empty string is returned if the user provides no input
        ///  or <see cref="Console.ReadLine"/> returns null.
        /// </returns>
        public static string Ask(string message, bool readPathMode = false) {
            // User prompt format:
            // ? {PROMPT} >> {USER_INPUT}
            // - ? in blue
            // - {PROMPT} in gray
            // - >> in light yellow
            // - {USER_INPUT} in gray

            // Display the prompt
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("? ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" >> ");
            Console.ForegroundColor = ConsoleColor.Gray;

            // Ask for user input and return it
            string givenInput = Console.ReadLine() ?? "";

            // If the user is providing a path, trim quotes around it
            return (readPathMode) ? givenInput.Trim('"').Trim('\'') : givenInput;
        }

        /// <summary>
        ///  Ask the user to provide a numeric input for a specific prompt and returns the result as a 32 bit signed integer.
        /// </summary>
        /// <param name="message">
        ///  The prompt to display to the user.
        /// </param>
        /// <returns>
        ///  32 bit signed integer representing the user's input. If the user provides invalid input or <see cref="int.TryParse"/>
        ///  cannot interpret the input, -1 is returned.
        /// </returns>
        public static int AskInt(string message) {
            // Ask the user for input
            string userInput = Ask(message);

            // Try to parse the input as an integer
            if (int.TryParse(userInput, out int result)) {
                return result;
            }

            // If the input is invalid, return -1
            return -1;
        }

        /// <summary>
        ///  Ask the user to provide a numeric input for a specific prompt and returns the result as a decimal.
        /// </summary>
        /// <param name="message">
        ///  The prompt to display to the user.
        /// </param>
        /// <returns>
        ///  128 bit decimal representing the user's input. If the user provides invalid input or <see cref="decimal.TryParse"/>
        ///  cannot interpret the input, -1 is returned.
        /// </returns>
        public static decimal AskDecimal(string message) {
            // Ask for input
            string userInput = Ask(message);

            // Try to parse the input as a decimal
            if (decimal.TryParse(userInput, out decimal result)) {
                return result;
            }

            // If the input is invalid, return -1
            return -1;
        }

        /// <summary>
        ///  Ask the user for a sequence of inputs for a list of prompts and return the results.
        /// </summary>
        /// <param name="prompts">
        ///  Any enumerable collection of strings representing the prompts to display to the user.
        /// </param>
        /// <returns>
        ///  Array of strings representing the user's input for each prompt. An array of empty strings is returned if the user
        ///  gave no valid input to any of the provided prompts.
        /// </returns>
        public static string[] AskMultiple(IEnumerable<string> prompts) {
            // Ask the user for input
            string[] results = new string[prompts.Count()];
            int i = 0;
            foreach (string prompt in prompts) {
                results[i] = Ask(prompt);
                i++;
            }
            return results;
        }

        /// <summary>
        ///  Ask the user for a Yes/No style prompt (Y/N) and return the result.
        /// </summary>
        /// <param name="message">
        ///  Message to display to the user.
        /// </param>
        /// <param name="yesVal">
        ///  Minimum required sequence of characters to interpret as a "Yes" response. Default is "Y".
        /// </param>
        /// <param name="caseMustMatch">
        ///  If true, the user's input must match the value of <paramref name="yesVal"/> exactly. Default is false.
        /// </param>
        /// <returns>
        ///  True if (case insensitively) the user's input contains the value of <paramref name="yesVal"/>, false otherwise.
        /// </returns>
        public static bool AskYesNo(string message, string yesVal = "Y", bool caseMustMatch = false) {
            // Display the prompt
            string userInput = Ask(message);

            // Null or empty string, return false
            if (string.IsNullOrEmpty(userInput)) {
                return false;
            }

            // Alter return value based on case sensitivity
            return (caseMustMatch) ? userInput == yesVal : userInput.ToLower().Contains(yesVal.ToLower());
        }
    }
}
