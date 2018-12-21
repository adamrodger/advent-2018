namespace AdventOfCode.ElfCode
{
    /// <summary>
    /// A single instruction in an ElfCode program
    /// </summary>
    public class Instruction
    {
        /// <summary>
        /// Instruction index (0-based)
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Instruction opcode
        /// </summary>
        public string Operation { get; }

        /// <summary>
        /// A value
        /// </summary>
        public int A { get; }

        /// <summary>
        /// B value
        /// </summary>
        public int B { get; }

        /// <summary>
        /// C value
        /// </summary>
        public int C { get; }

        /// <summary>
        /// Initialises a new instances of the <see cref="Instruction"/> class.
        /// </summary>
        /// <param name="index">Instruction index</param>
        /// <param name="input">Input string</param>
        public Instruction(int index, string input)
        {
            this.Index = index;

            this.Operation = input.Split(' ')[0];

            var numbers = input.Numbers();
            this.A = numbers[0];
            this.B = numbers[1];
            this.C = numbers[2];
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{this.Index,-2}: {this.Operation} {this.A} {this.B} {this.C}";
        }
    }
}
