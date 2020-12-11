using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day8\input.txt", "Day8", 8)]
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
                var (opcode, value) = Computer.Instructions[i];
                if (opcode == OpCodeD8.Jump)
                {
                    Computer.Instructions[i] = (OpCodeD8.NoOperator, value);
                }
                else if (opcode == OpCodeD8.NoOperator)
                {
                    Computer.Instructions[i] = (OpCodeD8.Jump, value);
                }
                else { continue; }
                Computer.StepUntilFinished();

                if (Computer.CurrentState == ComputerStateD8.Terminated)
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
        public (OpCodeD8 opcode, int value)[] OriginalInstructions;
        public (OpCodeD8 opcode, int value)[] Instructions;
        public int InstructionCount;
        public bool[] Visited;

        /// <summary>
        /// The next instruction to be executed.
        /// </summary>
        public int InstructionPointer;

        public int Accumulator;
        public ComputerStateD8 CurrentState;

        Dictionary<string, OpCodeD8> OpMap = new Dictionary<string, OpCodeD8>()
        {
            { "acc", OpCodeD8.Accumulator },
            { "jmp", OpCodeD8.Jump },
            { "nop", OpCodeD8.NoOperator },
        };

        public ComputerD8(string[] instructions)
        {
            Instructions = new (OpCodeD8, int)[instructions.Length];
            var i = 0;
            foreach (var inst in instructions)
            {
                var split = inst.Split(' ');

                Instructions[i] = (OpMap[split[0]], int.Parse(split[1]));
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
            if (Visited[InstructionPointer]) { CurrentState = ComputerStateD8.LoopDetected; return; }
            if (CurrentState != ComputerStateD8.Normal) { return; }

            var instModifier = 1;

            var (opcode, value) = Instructions[InstructionPointer];
            switch (opcode)
            {
                case OpCodeD8.Accumulator:
                    Accumulator += value;
                    break;
                case OpCodeD8.Jump:
                    instModifier = value;
                    break;
                case OpCodeD8.NoOperator:
                    break;
            }

            Visited[InstructionPointer] = true;
            InstructionPointer += instModifier;

            CurrentState = InstructionPointer >= InstructionCount ? 
                ComputerStateD8.Terminated : 
                ComputerStateD8.Normal;
        }

        public void StepUntilFinished()
        {
            do { Step(); } 
            while (CurrentState == ComputerStateD8.Normal);
        }

        public void Reset()
        {
            InstructionPointer = 0;
            Accumulator = 0;
            Visited = new bool[InstructionCount];
            Instructions = OriginalInstructions.ToArray();
            CurrentState = ComputerStateD8.Normal;
        }
    }

    public enum ComputerStateD8
    {
        Normal,
        LoopDetected,
        Terminated,
    }

    public enum OpCodeD8
    {
        NoOperator = 0,
        Accumulator = 1,
        Jump = 2,
    }
}
