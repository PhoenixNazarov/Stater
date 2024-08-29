using System;
using System.Windows.Forms;
using PluginData;

namespace SLXParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Function block files|*.slx";
            ofd.FilterIndex = 0;
            var dialogResult = ofd.ShowDialog();
            var result = new IReturn();
            
            var parser = new Parser(ofd.SafeFileName);
            var stateflow = parser.Parse();
            var pluginStateflow = new Translator().Convert(stateflow);
            result.ChangedMachines.Add(pluginStateflow);
        }
    }
}