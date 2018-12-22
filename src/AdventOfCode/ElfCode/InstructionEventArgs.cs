namespace AdventOfCode.ElfCode
{
    using System;

    /// <summary>
    /// Event args raised following the completion of an instruction
    /// </summary>
    public class InstructionEventArgs : EventArgs
    {
        /// <summary>
        /// Set this flag to true in order to stop the execution of the program immediately
        /// </summary>
        public bool StopExecution { get; set; }
    }
}
