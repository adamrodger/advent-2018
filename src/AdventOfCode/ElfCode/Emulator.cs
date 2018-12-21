namespace AdventOfCode.ElfCode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Emulator for ElfCode assembly language
    /// </summary>
    public class Emulator
    {
        private readonly Dictionary<string, Action<int[], int, int, int>> operations = new Dictionary<string, Action<int[], int, int, int>>
        {
            ["addr"] = (reg, a, b, c) => reg[c] = reg[a] + reg[b],
            ["addi"] = (reg, a, b, c) => reg[c] = reg[a] + b,
            ["mulr"] = (reg, a, b, c) => reg[c] = reg[a] * reg[b],
            ["muli"] = (reg, a, b, c) => reg[c] = reg[a] * b,
            ["banr"] = (reg, a, b, c) => reg[c] = reg[a] & reg[b],
            ["bani"] = (reg, a, b, c) => reg[c] = reg[a] & b,
            ["borr"] = (reg, a, b, c) => reg[c] = reg[a] | reg[b],
            ["bori"] = (reg, a, b, c) => reg[c] = reg[a] | b,
            ["setr"] = (reg, a, _, c) => reg[c] = reg[a],
            ["seti"] = (reg, a, _, c) => reg[c] = a,
            ["gtir"] = (reg, a, b, c) => reg[c] = a > reg[b] ? 1 : 0,
            ["gtri"] = (reg, a, b, c) => reg[c] = reg[a] > b ? 1 : 0,
            ["gtrr"] = (reg, a, b, c) => reg[c] = reg[a] > reg[b] ? 1 : 0,
            ["eqir"] = (reg, a, b, c) => reg[c] = a == reg[b] ? 1 : 0,
            ["eqri"] = (reg, a, b, c) => reg[c] = reg[a] == b ? 1 : 0,
            ["eqrr"] = (reg, a, b, c) => reg[c] = reg[a] == reg[b] ? 1 : 0
        };

        /// <summary>
        /// Event raised after every instruction completes
        /// </summary>
        public event EventHandler<InstructionEventArgs> OnInstructionComplete;

        /// <summary>
        /// Current state of the registers
        /// </summary>
        public int[] Registers { get; }

        /// <summary>
        /// Register containing the instruction pointer
        /// </summary>
        public int InstructionRegister { get; }

        /// <summary>
        /// Number of executed cycles
        /// </summary>
        public int Cycles { get; private set; }

        /// <summary>
        /// Program instructions
        /// </summary>
        public IList<Instruction> Program { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Emulator"/> class.
        /// </summary>
        /// <param name="input">Input program</param>
        /// <param name="registers">Number of registers (default 6)</param>
        /// <param name="debug">Print debugging information (default false)</param>
        public Emulator(string[] input, int registers = 6, bool debug = false)
        {
            this.InstructionRegister = input[0].Numbers()[0];
            this.Program = input.Skip(1).Select((s, i) => new Instruction(i, s)).ToList();

            this.Registers = new int[registers];
            this.Cycles = 0;

            if (debug)
            {
                this.OnInstructionComplete += (_, __) => this.PrintDebug();
            }
        }

        /// <summary>
        /// Execute the loaded program
        /// </summary>
        public void Execute()
        {
            while (this.Registers[this.InstructionRegister] >= 0 && this.Registers[this.InstructionRegister] < this.Program.Count)
            {
                // get the current instruction
                var instruction = this.Program[this.Registers[this.InstructionRegister]];

                // execute it
                this.operations[instruction.Operation](this.Registers, instruction.A, instruction.B, instruction.C);

                // notify the cycle is complete
                this.Cycles++;
                var args = InstructionEventArgs.Default;
                this.OnInstructionComplete?.Invoke(this, args);

                if (args.StopExecution)
                {
                    return;
                }

                // move to the next instruction
                this.Registers[this.InstructionRegister] = this.Registers[this.InstructionRegister] + 1;
            }
        }

        /// <summary>
        /// Print the current state and which instruction has just executed
        /// </summary>
        public void PrintDebug()
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            var builder = new StringBuilder();

            builder.AppendLine($"Cycles: {this.Cycles}");
            builder.AppendLine($"State: {string.Join("\t\t\t", this.Registers)}\n");

            foreach (Instruction instruction in this.Program)
            {
                builder.Append(instruction);

                if (instruction.Index == this.Registers[this.InstructionRegister])
                {
                    builder.Append("  <<<<<<");
                }

                builder.AppendLine();
            }

            // have to tweak this number to get it to align properly depending on output window size
            foreach (int blank in Enumerable.Range(1, 0))
            {
                builder.AppendLine();
            }

            Debug.WriteLine(builder.ToString());
        }
    }
}
