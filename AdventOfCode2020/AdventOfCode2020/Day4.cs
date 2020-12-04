using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdventOfCode2020
{
    public class Day4
    {

        public class Passport
        {
            private Regex eclrx = new Regex(@"ecl:(.*?)\s");
            private Regex pidrx = new Regex(@"pid:(.*?)\s");
            private Regex eyrrx = new Regex(@"eyr:(.*?)\s");
            private Regex hclrx = new Regex(@"hcl:(.*?)\s");
            private Regex byrrx = new Regex(@"byr:(.*?)\s");
            private Regex iyrrx = new Regex(@"iyr:(.*?)\s");
            private Regex cidrx = new Regex(@"cid:(.*?)\s");
            private Regex hgtrx = new Regex(@"hgt:(.*?)\s");

            private static List<string> _validEyeColors = new List<string>()
                {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};

        private string _fullText;
            private string _ecl;
            private string _pid;
            private string _eyr;
            private string _hcl;
            private string _byr;
            private string _iyr;
            private string _cid;
            private string _hgt;

            public Passport(string text)
            {
                _fullText = text;

                var m = eclrx.Match(text);
                if (m.Success)
                {
                    _ecl = m.Groups[1].Value;
                }

                m = pidrx.Match(text);
                if (m.Success)
                {
                    _pid = m.Groups[1].Value;
                }

                m = hclrx.Match(text);
                if (m.Success)
                {
                    _hcl = m.Groups[1].Value;
                }

                m = eyrrx.Match(text);
                if (m.Success)
                {
                    _eyr = m.Groups[1].Value;
                }

                m = byrrx.Match(text);
                if (m.Success)
                {
                    _byr = m.Groups[1].Value;
                }

                m = iyrrx.Match(text);
                if (m.Success)
                {
                    _iyr = m.Groups[1].Value;
                }

                m = cidrx.Match(text);
                if (m.Success)
                {
                    _cid = m.Groups[1].Value;
                }

                m = hgtrx.Match(text);
                if (m.Success)
                {
                    _hgt = m.Groups[1].Value;
                }
            }

            public override string ToString()
            {
                string s = "\n";

                s += "------------\nPASSPORT";
                s += "\nPID:        " + _pid;
                s += "\nISSUE YEAR: " + _iyr;
                s += "\nEXP YEAR:   " + _eyr;
                s += "\nBIRTH YEAR: " + _byr;
                s += "\nHAIR COLOR: " + _hcl;
                s += "\nEYE COLOR:  " + _ecl;
                s += "\nHEIGHT:     " + _hgt;
                s += "\nCID:        " + _cid;
                s += "\nIS VALID:   " + this.IsValidPart2();
                s += "\n------------\n\n\n";

                return s;
            }

            public bool IsValidPart1(bool cidRequired = false)
            {
                if (!cidRequired)
                {
                    return _ecl != null && _pid != null && _eyr != null && _hcl != null && _byr != null &&
                           _iyr != null && _hgt != null;
                }
                else
                {
                    return _ecl != null && _pid != null && _eyr != null && _hcl != null && _byr != null &&
                           _iyr != null && _hgt != null && _cid != null;
                }
            }

            public bool IsValidPart2()
            {
                int tryy;

                if (_byr == null || !int.TryParse(_byr, out tryy) || int.Parse(_byr) < 1920 || int.Parse(_byr) > 2002)
                {
                    Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nBYR FAILED ON RECORD: " + this._fullText + "\n---------");
                    return false;
                }

                if (_iyr == null || !int.TryParse(_iyr, out tryy) || int.Parse(_iyr) < 2010 || int.Parse(_iyr) > 2020)
                {
                    Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nIYR FAILED ON RECORD: " + this._fullText + "\n---------");
                    return false;
                }

                if (_eyr == null || !int.TryParse(_eyr, out tryy) || int.Parse(_eyr) < 2020 || int.Parse(_eyr) > 2030)
                {
                    Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nEYR FAILED ON RECORD: " + this._fullText + "\n---------");
                    return false;
                }

                if (_hgt == null)
                {
                    Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nHGT FAILED ON RECORD: " + this._fullText + "\n---------");
                    return false;
                }
                else
                {
                    string hgtUnitString = _hgt.Substring(_hgt.Length - 2);

                    if (hgtUnitString == "cm" || hgtUnitString == "in")
                    {
                        int hgtValue = int.Parse(_hgt.Substring(0, _hgt.Length - 2));

                        if (hgtUnitString == "cm")
                        {
                            if (hgtValue < 150 || hgtValue > 193)
                            {
                                Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nHGT FAILED ON RECORD: " + this._fullText + "\n---------");
                                return false;
                            }
                        }
                        else if (hgtUnitString == "in")
                        {
                            if (hgtValue < 59 || hgtValue > 76)
                            {
                                Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nHGT FAILED ON RECORD: " + this._fullText + "\n---------");
                                return false;
                            }
                        }
                        else
                        {
                            Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nHGT FAILED ON RECORD: " + this._fullText + "\n---------");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nHGT FAILED ON RECORD: " + this._fullText + "\n---------");
                        return false;
                    }
                }

                if (_hcl == null)
                {
                    Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nHCL FAILED ON RECORD: " + this._fullText + "\n---------");
                    return false;
                }
                else
                {
                    //https://stackoverflow.com/a/13035186
                    if (!Regex.Match(_hcl, "^#(?:[0-9a-fA-F]{6})$").Success)
                    {
                        Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nHCL FAILED ON RECORD: " + this._fullText + "\n---------");
                        return false;
                    }
                }

                if (_ecl == null)
                {
                    Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nECL FAILED ON RECORD: " + this._fullText + "\n---------");
                    return false;
                }
                else
                {
                    if (!_validEyeColors.Contains(_ecl))
                    {
                        Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nECL FAILED ON RECORD: " + this._fullText + "\n---------");
                        return false;
                    }
                }

                if (_pid == null || !int.TryParse(_pid, out tryy) || _pid.Length != 9)
                {
                    Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nPID FAILED ON RECORD: " + this._fullText + "\n---------");
                    return false;
                }

                Logger.LogMessage(LogLevel.DEBUG, "\n\n--------\nRECORD VALID: " + this._fullText + "\n---------");
                return true;
            }
        }

    public static void ExecuteDay(string fileLocation = "PuzzleInput/Day4.txt")
        {
            string allText = File.ReadAllText(fileLocation);

            string[] passportsText = allText.Split(Environment.NewLine + Environment.NewLine);

            List<Passport> passports = new List<Passport>();

            foreach (var p in passportsText)
            {
                passports.Add(new Passport(p + " "));
            }

            Logger.LogMessage(LogLevel.ANSWER, "4A: " + passports.Count(p => p.IsValidPart1()));
            Logger.LogMessage(LogLevel.ANSWER, "4B: " + passports.Count(p => p.IsValidPart2()));
        }
    }
}
