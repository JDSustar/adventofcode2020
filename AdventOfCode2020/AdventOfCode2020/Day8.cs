using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2020
{
    public class Day8
    {
        static readonly Regex InstructionValueRegex = new Regex(@"((?:\+|-)\d+)");

        public class GameConsole
        {
            public List<Instruction> ProgramInstructions = new List<Instruction>();
            public int currentInstructionIndex = 0;
            public int Accumulator = 0;

            public GameConsole(string instructionSetString)
            {
                string[] instructions = instructionSetString.Split(Environment.NewLine);

                foreach (var i in instructions)
                {
                    var it = (Instruction.InstructionType) Enum.Parse(typeof(Instruction.InstructionType), i.Substring(0, 3), true);
                    ProgramInstructions.Add(new Instruction(it, int.Parse(InstructionValueRegex.Match(i).Groups[0].Value)));
                }
            }

            public class Instruction
            {
                public enum InstructionType
                {
                    acc, jmp, nop
                }

                public InstructionType Type;
                public int InstructionValue;

                public Instruction(InstructionType instructionType, int instructionValue)
                {
                    this.Type = instructionType;
                    this.InstructionValue = instructionValue;
                }
            }

            public void RunUntilLoop()
            {
                List<int> visitedInstructionIndexes = new List<int>();

                while (!visitedInstructionIndexes.Contains(currentInstructionIndex) && currentInstructionIndex < ProgramInstructions.Count)
                {
                    visitedInstructionIndexes.Add(currentInstructionIndex);
                    var currentInstruction = ProgramInstructions[currentInstructionIndex];

                    switch (currentInstruction.Type)
                    {
                        case Instruction.InstructionType.acc:
                            Accumulator += currentInstruction.InstructionValue;
                            currentInstructionIndex++;
                            break;
                        case Instruction.InstructionType.jmp:
                            currentInstructionIndex += currentInstruction.InstructionValue;
                            break;
                        case Instruction.InstructionType.nop:
                            currentInstructionIndex++;
                            break;
                    }
                }
            }
        }

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day8.txt")
        {
            string allText = File.ReadAllText(fileLocation);

            var gc1 = new GameConsole(allText);

            gc1.RunUntilLoop();



            var gc2 = new GameConsole(allText);
            gc2.RunUntilLoop();
            var currentChangeIndex = 0;

            while (gc2.currentInstructionIndex < gc2.ProgramInstructions.Count)
            {
                gc2 = new GameConsole(allText);

                if (gc2.ProgramInstructions[currentChangeIndex].Type == GameConsole.Instruction.InstructionType.jmp)
                {
                    gc2.ProgramInstructions[currentChangeIndex].Type = GameConsole.Instruction.InstructionType.nop;
                    gc2.RunUntilLoop();
                    currentChangeIndex++;
                }
                else if (gc2.ProgramInstructions[currentChangeIndex].Type == GameConsole.Instruction.InstructionType.nop)
                {
                    gc2.ProgramInstructions[currentChangeIndex].Type = GameConsole.Instruction.InstructionType.jmp;
                    gc2.RunUntilLoop();
                    currentChangeIndex++;
                }
                else
                {
                    currentChangeIndex++;
                }
            }

            Logger.LogMessage(LogLevel.ANSWER, "8A: " + gc1.Accumulator);
            Logger.LogMessage(LogLevel.ANSWER, "8B: " + gc2.Accumulator);
        }
    }
}
