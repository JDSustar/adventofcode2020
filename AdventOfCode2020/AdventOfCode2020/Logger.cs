using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public enum LogLevel
    {
        INFO = 1,
        DEBUG = 2,
        WARN = 3,
        ERROR = 4,
        ANSWER = 5
    }

    public static class Logger
    {
        
        public static LogLevel CURRENT_LOG_LEVEL = LogLevel.INFO;

        public static void LogMessage(LogLevel level, string message)
        {
            if (level >= CURRENT_LOG_LEVEL)
            {
                Console.WriteLine(message);
            }
        }
    }
}
