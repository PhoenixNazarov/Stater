using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SLXParser.Data;

namespace SLXParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string path = "./BR_GATES_HDL.slx";

            var parser = new Parser(path);
            var stateflow = parser.Parse();

            Console.WriteLine(stateflow.Machine.Chart.ChildrenState);
            Console.WriteLine(stateflow.Machine.Chart.ChildrenState.Count);
        }
    }
}