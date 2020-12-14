using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    public class Day14
    {
        public class PortComputer
        {
            private Regex memoryInstructionRegex = new Regex(@"mem\[(?<address>\d+)]\s\=\s(?<value>\d+)");
            private ulong OnesMask;
            private ulong ZeroMask;
            private ulong ExesMask;
            private List<string> Instructions = new List<string>();
            public Dictionary<ulong, ulong> Memory = new Dictionary<ulong, ulong>();

            public PortComputer(string[] inputs)
            {
                Instructions = inputs.ToList();
            }

            public void Run()
            {
                foreach (var instruction in Instructions)
                {
                    if (instruction.Substring(0, 3) == "mas")
                    {
                        UpdateMask(instruction.Split(null)[2]);
                    }
                    else if (instruction.Substring(0, 3) == "mem")
                    {
                        var m = memoryInstructionRegex.Match(instruction);
                        Write(ulong.Parse(m.Groups["address"].Value), ulong.Parse(m.Groups["value"].Value));
                    }
                }
            }

            //public void Run2()
            //{
            //    foreach (var instruction in Instructions)
            //    {
            //        if (instruction.Substring(0, 4) == "mask")
            //        {
            //            UpdateMask(instruction.Split(null)[2]);
            //        }
            //        else if (instruction.Substring(0, 3) == "mem")
            //        {
            //            var m = memoryInstructionRegex.Match(instruction);
            //            Write(ulong.Parse(m.Groups["address"].Value), ulong.Parse(m.Groups["value"].Value));
            //        }
            //    }
            //}

            public void UpdateMask(string mask)
            {
                OnesMask = 0;
                ZeroMask = 0;
                ExesMask = 0;

                for (int i = 0; i < mask.Length; i++)
                {
                    if (mask[i] == '0')
                    {
                        ulong addedMask = 1;
                        addedMask <<= (mask.Length - 1 - i);
                        ZeroMask |= addedMask;
                    }
                    else if (mask[i] == '1')
                    {
                        ulong addedMask = 1;
                        addedMask <<= (mask.Length - 1 - i);
                        OnesMask |= addedMask;
                    }
                    else if (mask[i] == 'X')
                    {
                        ulong addedMask = 1;
                        addedMask <<= (mask.Length - 1 - i);
                        ExesMask |= addedMask;
                    }
                }
            }

            public void Write(ulong address, ulong value)
            {
                ulong finalValue = value & ~ZeroMask;
                finalValue |= OnesMask;

                Memory[address] = finalValue;
            }

            //public void Write2(ulong address, ulong value)
            //{
            //    address |= OnesMask;

            //    foreach (var a in GetAddressPermutations(address))
            //    {
            //        Memory[a] = value;
            //    }
            //}

            public ulong GetSumOfMemory()
            {
                ulong sum = 0;

                foreach (var m in Memory)
                {
                    sum += m.Value;
                }

                return sum;
            }
        }

        public static void ExecuteDay(string fileLocation = "PuzzleInput/Day14.txt")
        {
            string[] inputs = File.ReadAllText(fileLocation).Split(Environment.NewLine);

            PortComputer pc = new PortComputer(inputs);
            pc.Run();

            Logger.LogMessage(LogLevel.ANSWER, "13A: " + pc.GetSumOfMemory());

            Logger.LogMessage(LogLevel.ANSWER, "13B: " + 2);
        }
    }
}