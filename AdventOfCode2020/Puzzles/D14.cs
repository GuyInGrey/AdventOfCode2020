using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day14\input.txt", "Day14", 14)]
    public class D14
    {
        List<Instruction> Instructions;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var input = File.ReadAllText(info.DataFilePath).Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Instructions = input.Select(s => Instruction.FromString(s)).ToList();
        }

        [Benchmark(1)]
        public object Part1()
        {
            var mem = new Dictionary<ulong, ulong>();
            Bitmask mask = null;

            foreach (var i in Instructions)
            {
                if (i.Type == InstructionType.SetMask)
                {
                    mask = (Bitmask)i.Value;
                }
                else if (i.Type == InstructionType.SetMemory)
                {
                    var a = ((ulong index, ulong value))i.Value;
                    var newValue = mask.Mask(a.value);
                    if (mem.ContainsKey(a.index)) 
                    { 
                        mem[a.index] = newValue; 
                    } 
                    else { mem.Add(a.index, newValue); }
                }
            }
            var sum = (ulong)0;
            foreach (var m in mem.Values) { sum += m; }
            return sum;
        }

        [Benchmark(2)]
        public object Part2()
        {
            var mem = new Dictionary<ulong, ulong>();
            Bitmask mask = null;

            foreach (var i in Instructions)
            {
                if (i.Type == InstructionType.SetMask)
                {
                    mask = (Bitmask)i.Value;
                }
                else if (i.Type == InstructionType.SetMemory)
                {
                    var a = ((ulong index, ulong value))i.Value;
                    var indices = mask.MaskV2(a.index);
                    foreach (var i2 in indices)
                    {
                        if (mem.ContainsKey(i2))
                        { mem[i2] = a.value; }
                        else { mem.Add(i2, a.value); }
                    }
                }
            }
            var sum = (ulong)0;
            foreach (var m in mem.Values) { sum += m; }
            return sum;
        }

        public enum InstructionType
        {
            SetMask,
            SetMemory,
        }

        public class Instruction
        {
            public InstructionType Type;
            public object Value;

            public static Instruction FromString(string s)
            {
                var toReturn = new Instruction()
                {
                    Type = s[5] == '=' ? InstructionType.SetMask : InstructionType.SetMemory,
                };

                if (toReturn.Type == InstructionType.SetMask)
                {
                    toReturn.Value = new Bitmask(s.Substring(7));
                }
                else
                {
                    var index = ulong.Parse(s.Substring(4, s.IndexOf(']') - 4));
                    var val = ulong.Parse(s.Substring(s.IndexOf('=') + 2));
                    toReturn.Value = (index, val);
                }

                return toReturn;
            }
        }

        public class Bitmask
        {
            public string Raw;
            public BitmaskMode[] Modes;

            public Bitmask(string input)
            {
                Raw = input;
                Modes = new BitmaskMode[36];
                for (var i = 0; i < 36; i++)
                {
                    Modes[i] = input[i] == 'X' ? BitmaskMode.None :
                        input[i] == '1' ? BitmaskMode.One : BitmaskMode.Zero;
                }
            }

            public ulong Mask(ulong input)
            {
                var s = Convert.ToString((long)input, 2).PadLeft(36, '0');
                var sA = s.ToCharArray();
                for (var i = 0; i < 36; i++)
                {
                    if (Modes[i] != BitmaskMode.None)
                    {
                        sA[i] = Modes[i] == BitmaskMode.One ? '1' : '0';
                    }
                }
                return Convert.ToUInt64(new string(sA).PadLeft(64, '0'), 2);
            }

            public ulong[] MaskV2(ulong input)
            {
                var s = Convert.ToString((long)input, 2).PadLeft(36, '0');
                var sA = s.ToCharArray();
                for (var i = 0; i < 36; i++)
                {
                    if (Modes[i] != BitmaskMode.Zero)
                    {
                        sA[i] = Modes[i] == BitmaskMode.One ? '1' : 'X';
                    }
                }
                s = new string(sA);

                var newList = new List<ulong>();
                RecurseMask(s, 0, ref newList);

                return newList.ToArray();
            }

            public void RecurseMask(string soFar, int lastX, ref List<ulong> final)
            {
                var index = soFar.IndexOf('X', lastX);
                if (index == -1)
                {
                    final.Add(Convert.ToUInt64(soFar.PadLeft(64, '0'), 2));
                    return;
                }

                var arr1 = soFar.ToCharArray();
                var arr2 = soFar.ToCharArray();
                arr1[index] = '0';
                arr2[index] = '1';
                RecurseMask(new string(arr1), index + 1, ref final);
                RecurseMask(new string(arr2), index + 1, ref final);
            }
        }

        public enum BitmaskMode
        {
            None,
            One,
            Zero,
        }
    }
}
