// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
//  J O B   -   B A S E   J O B
//      Template base class used for QBC jobs, responsible for
//      compiling and decompiling
// = - = - = - = - = - = - = - = - = - = - = - = - = - = - = - =
// Import required namespaces.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Template base class used for QBC jobs, responsible for compiling and decompiling.
    ///  New types of jobs should derive from this.
    /// </summary>
    abstract public class QBCBaseJob {
        // This class has no constructor; we'll define it in derived classes.

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  The file name (only) of the file being processed.
        /// </summary>
        public virtual string FileName { get; set; } = null;

        /// <summary>
        ///  Any associated input data with the base job. This should be overriden in derived types to allow
        ///  for the data to be what a job expects.
        /// </summary>
        public virtual object Input { get; private set; } = null;

        /// <summary>
        ///  Any associated output data with the base job. This should be overriden in derived types to allow
        ///  for the data to be what a job expects.
        /// </summary>
        public virtual object Output { get; private set; } = null;

        /// <summary>
        ///  If true, the job has been aborted.
        /// </summary>
        public virtual bool Abort { get; internal set; } = false;

        /// <summary>
        ///  Indent level being used by this job. Used for debug and text output in decompile jobs.
        /// </summary>
        public virtual int Indent { get; internal set; }

        /// <summary>
        ///  All related options to this job.
        /// </summary>
        public virtual QBCJobOptions Options { get; internal set; } = new QBCJobOptions();

        /// <summary>
        ///  The hierarchic debug text used when <see cref="HierDebug(string)"/> is called.
        /// </summary>
        public virtual string HierDebugText { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  The default reason a job might have failed.
        /// </summary>
        private const string DefaultFailReason = "Job failed with no provided reason.";

        /// <summary>
        ///  Fail the job with a given reason.
        /// </summary>
        /// <param name="reason">
        ///  The reason that the job failed.
        /// </param>
        public virtual void Fail(string reason = null) {
            // Get the real reason
            string realReason = (reason is null) ? DefaultFailReason : reason;

            // Stop all action from the job
            Abort = true;

            // Print the error to the console
            Chalk.Error(realReason);
        }

        /// <summary>
        ///  Returns true if the current job has failed.
        /// </summary>
        /// <returns>
        ///  True if <see cref="Abort"/> is set to true, false otherwise.
        /// </returns>
        public virtual bool Failed() { return Abort; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  T E X T   I N D E N T A T I O N
        //      Used for debug and for text output
        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Gets the current indentation level.
        /// </summary>
        /// <returns>
        ///  The current indentation level being used.
        /// </returns>
        public int GetIndent() {
            return Indent;
        }

        /// <summary>
        ///  Sets the indentation level to the given value.
        /// </summary>
        /// <param name="level">
        ///  Indentation level to set.
        /// </param>
        public void SetIndent(int level) {
            Indent = level;
        }

        /// <summary>
        ///  Adds 1 indentation level.
        /// </summary>
        public void AddIndent() {
            Indent++;
        }

        /// <summary>
        ///  Subtracts 1 indentation level.
        /// </summary>
        public void SubIndent() {
            Indent--;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  D E B U G   L O G G I N G
        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Write a debug message to the console.
        /// </summary>
        /// <param name="text">
        ///  Text to write out.
        /// </param>
        public void Debug(string text) {
            if (Options.CanDebug) {
                Chalk.Write(text);
            }
        }

        /// <summary>
        ///  Writes a debug message tabbed over by the current indentation level.
        /// </summary>
        /// <param name="text">
        ///  Text to write out.
        /// </param>
        /// <returns>
        ///  The indented string with the given text.
        /// </returns>
        public string TabbedDebug(string text) {
            var indented = "".PadLeft(GetIndent() * 2, ' ');
            Debug(indented + text);

            return (indented + text);
        }

        /// <summary>
        ///  Hierarchically debug with the given text.
        /// </summary>
        /// <param name="text">
        ///  Text to write out.
        /// </param>
        public void HierDebug(string text) {
            HierDebugText += TabbedDebug(text) + '\n';
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -
        //  G A M E   T Y P E   H E L P E R S
        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Is the job targeting Guitar Hero: World Tour?
        /// </summary>
        /// <returns>
        ///  True if <see cref="QBCJobOptions.TargetGame"/> on the <see cref="Options"/> field is set to <see cref="JobGameTarget.GHWT"/>, false otherwise.
        /// </returns>
        public virtual bool IsGHWT() { return Options.TargetGame == JobGameTarget.GHWT; }

        /// <summary>
        ///  Is the job targeting Guitar Hero III: Legends of Rock?
        /// </summary>
        /// <returns>
        ///  True if <see cref="QBCJobOptions.TargetGame"/> on the <see cref="Options"/> field is set to <see cref="JobGameTarget.GH3"/>, false otherwise.
        /// </returns>
        public virtual bool IsGH3() { return Options.TargetGame == JobGameTarget.GH3; }

        /// <summary>
        ///  Is the job targeting Guitar Hero: Warriors of Rock?
        /// </summary>
        /// <returns>
        ///  True if <see cref="QBCJobOptions.TargetGame"/> on the <see cref="Options"/> field is set to <see cref="JobGameTarget.GH6"/>, false otherwise.
        /// </returns>
        public virtual bool IsGH6() { return Options.TargetGame == JobGameTarget.GH6; }

        /// <summary>
        ///  Is the job targeting Tony Hawk's American Wasteland?
        /// </summary>
        /// <returns>
        ///  True if <see cref="QBCJobOptions.TargetGame"/> on the <see cref="Options"/> field is set to <see cref="JobGameTarget.THAW"/>, false otherwise.
        /// </returns>
        public virtual bool IsTHAW() { return Options.TargetGame == JobGameTarget.THAW; }

        /// <summary>
        ///  Is the job targeting Tony Hawk's Underground 1?
        /// </summary>
        /// <returns>
        ///  True if <see cref="QBCJobOptions.TargetGame"/> on the <see cref="Options"/> field is set to <see cref="JobGameTarget.THUG1"/>, false otherwise.
        /// </returns>
        public virtual bool IsTHUG1() { return Options.TargetGame == JobGameTarget.THUG1; }

        /// <summary>
        ///  Is the job targeting Tony Hawk's Underground 2?
        /// </summary>
        /// <returns>
        ///  True if <see cref="QBCJobOptions.TargetGame"/> on the <see cref="Options"/> field is set to <see cref="JobGameTarget.THUG2"/>, false otherwise.
        /// </returns>
        public virtual bool IsTHUG2() { return Options.TargetGame == JobGameTarget.THUG2; }

        /// <summary>
        ///  Is the job targeting Tony Hawk's Pro Skater 4?
        /// </summary>
        /// <returns>
        ///  True if <see cref="QBCJobOptions.TargetGame"/> on the <see cref="Options"/> field is set to <see cref="JobGameTarget.THPS4"/>, false otherwise.
        /// </returns>
        public virtual bool IsTHPS4() { return Options.TargetGame == JobGameTarget.THPS4; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

    }
}
