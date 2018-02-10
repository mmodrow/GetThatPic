using System;
using System.Collections.Generic;
using System.Text;

namespace GetThatPic.Data.IO
{
    public static class Logger
    {
        public static void Log(string input)
        {
            System.Console.WriteLine(input);
        }

        public static void Error(string input)
        {
            System.Console.WriteLine("Error: " + input);
        }
    }
}
