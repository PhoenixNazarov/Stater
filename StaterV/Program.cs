using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StaterV
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            var pluginManager = PluginManager.PluginManager.Instance;
            pluginManager.LoadPlugins();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form1();
            var starter = new BatchModeStarter(args, form);
            //Application.Run(form);
            starter.Run();
        }
    }
}
