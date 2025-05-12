using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NX_QScript.Internal {
    /// <summary>
    ///  Type used to alter the actions taken by a <see cref="QBCBaseJob"/>.
    /// </summary>
    public class QBCJobOptions {
        /// <summary>
        ///  Construct a new instance of <see cref="QBCJobOptions"/>.
        /// </summary>
        public QBCJobOptions() {
            CanDebug = false;
            WriteText = true;
            TargetGame = JobGameTarget.GHWT;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///  Is this job allowed to log debug information?
        /// </summary>
        public bool CanDebug { get; set; }

        /// <summary>
        ///  The game of choice that this job is targeting.
        /// </summary>
        public JobGameTarget TargetGame { get; set; }

        /// <summary>
        ///  Used on decompile jobs: If true, writes output text from the QB types.
        /// </summary>
        public bool WriteText { get; set; }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - -
    }

    /// <summary>
    ///  Target games that a QBC compile/decompile job can operate for.
    /// </summary>
    public enum JobGameTarget {
        /// <summary>
        ///  Job targets Guitar Hero: World Tour (default). Supports both compiling and decompiling.
        /// </summary>
        GHWT, GH4 = 0,
        /// <summary>
        ///  Job targets Guitar Hero III: Legends of Rock. Supports both compiling and decompiling.
        /// </summary>
        GH3 = 1,
        /// <summary>
        ///  Job targets Tony Hawk's American Wasteland. Supports both compiling and decompiling.
        /// </summary>
        THAW = 2,
        /// <summary>
        ///  Job targets Tony Hawk's Underground 2. Only supports decompiling.
        /// </summary>
        THUG2 = 3,
        /// <summary>
        ///  Job targets Tony Hawk's Underground 1. Only supports decompiling.
        /// </summary>
        THUG1 = 4,
        /// <summary>
        ///  Job targets Tony Hawk's Pro Skater 4. Only supports decompiling.
        /// </summary>
        THPS4 = 5,
        /// <summary>
        ///  Job targets Guitar Hero: Warriors of Rock. Supports both compiling and decompiling.
        /// </summary>
        GH6 = 6
    }
}
