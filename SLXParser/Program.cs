namespace SLXParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string path = "./BR_GATES_HDL.slx";

            var parser = new Parser(path);
            var stateflow = parser.Parse();
        }
    }
}