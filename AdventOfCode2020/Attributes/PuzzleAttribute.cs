using System;

namespace GuyInGrey_AoC2020
{
    public class PuzzleAttribute : Attribute
    {
        public string DataFilePath { get; }
        public string Name { get; }
        public int Priority { get; }

        public PuzzleAttribute(string filePath, string name, int priority)
        {
            DataFilePath = filePath;
            Name = name;
            Priority = priority;
        }
    }
}
