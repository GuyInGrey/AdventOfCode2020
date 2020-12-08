using System;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{

    [Puzzle(@"PuzzleInputs\Day8\input.txt", "Day8")]
    public class D8
    {
        ComputerD8 Computer;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            Computer = new ComputerD8(File.ReadAllLines(info.DataFilePath));
        }

        [Benchmark(1)]
        public int Part1()
        {
            Computer.StepUntilFinished();
            return Computer.Accumulator;
        }

        [Benchmark(2)]
        public int Part2()
        {
            for (var i = 0; i < Computer.InstructionCount; i++)
            {
                var op = Computer.Instructions[i];
                if (op.opcode == "jmp")
                {
                    Computer.Instructions[i] = ("nop", op.value);
                }
                else if (op.opcode == "nop")
                {
                    Computer.Instructions[i] = ("jmp", op.value);
                }
                else { continue; }
                Computer.StepUntilFinished();

                if (Computer.State == ComputerStateD8.Terminated)
                {
                    return Computer.Accumulator;
                } 
                
                Computer.Reset();
            }

            return int.MinValue;
        }
    }

    public class ComputerD8
    {
        public (string opcode, int value)[] OriginalInstructions;
        public (string opcode, int value)[] Instructions;
        public int InstructionCount;
        public bool[] Visited;

        /// <summary>
        /// The next instruction to be executed.
        /// </summary>
        public int InstructionPointer;

        public int Accumulator;
        public ComputerStateD8 State;

        public ComputerD8(string[] instructions)
        {
            Instructions = new (string, int)[instructions.Length];
            var i = 0;
            foreach (var inst in instructions)
            {
                var split = inst.Split(' ');
                Instructions[i] = (split[0], int.Parse(split[1]));
                i++;
            }
            OriginalInstructions = Instructions.ToArray();
            InstructionCount = Instructions.Length;

            Visited = new bool[instructions.Length];
        }

        /// <summary>
        /// Returns true if loop detected. If there was a loop, no instruction was executed.
        /// </summary>
        public void Step()
        {
            if (Visited[InstructionPointer]) { State = ComputerStateD8.LoopDetected; return; }

            var instModifier = 1;

            var currentOp = Instructions[InstructionPointer];
            switch (currentOp.opcode)
            {
                case "acc":
                    Accumulator += currentOp.value;
                    break;
                case "jmp":
                    instModifier = currentOp.value;
                    break;
                case "nop":
                    break;
            }

            Visited[InstructionPointer] = true;
            InstructionPointer += instModifier;

            State = InstructionPointer >= InstructionCount ? 
                ComputerStateD8.Terminated : 
                ComputerStateD8.Normal;
        }

        public void StepUntilFinished()
        {
            do { Step(); } 
            while (State == ComputerStateD8.Normal);
        }

        public void Reset()
        {
            InstructionPointer = 0;
            Accumulator = 0;
            Visited = new bool[InstructionCount];
            Instructions = OriginalInstructions.ToArray();
            State = ComputerStateD8.Normal;
        }
    }

    public enum ComputerStateD8
    {
        Normal,
        LoopDetected,
        Terminated,
    }
}
