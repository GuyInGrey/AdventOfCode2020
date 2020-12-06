using System;

namespace GuyInGrey_AoC2020
{
    public class PuzzleAttribute : Attribute
    {
        public string DataFilePath { get; }
        public string Name { get; }

        public PuzzleAttribute(string filePath, string name)
        {
            DataFilePath = filePath;
            Name = name;
        }
    }
}
