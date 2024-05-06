using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PluginData;

namespace CSMultyThread.GUI
{
    public class OptionsLogic
    {
        public OptionsLogic()
        {
            wrapper = new OptionsWrapper();
            wrapper.Logic = this;
            Executions = new List<AutomatonExecution>();
            Options = new Options();
        }

        private const string StateFile = "csmt.sta";

        private OptionsWrapper wrapper;

        public List<AutomatonExecution> Executions { get; set; }
        public string RawText { get; set; }
        //public string Namespace { get; set; }
        public Options Options { get; set; }

        public enum Result
        {
            OK,
            Cancel,
            Fail,
        }

        public Result Start(string wd)
        {
            TryLoadState(wd);

            var res = wrapper.Start();

            if (res == Result.OK)
            {
                TrySaveState(wd);
                ParseText();
            }

            return res;
        }

        private void TrySaveState(string wd)
        {
            try
            {
                string path = wd + "\\" + StateFile;
                StreamWriter sw = new StreamWriter(path);

                sw.WriteLine(Options.Namespace);
                sw.WriteLine(RawText);

                sw.Close();

            }
            catch (Exception)
            {
            }
        }

        private void TryLoadState(string wd)
        {
            try
            {
                StreamReader sr = new StreamReader(wd + "\\" + StateFile);
                Options.Namespace = sr.ReadLine();
                RawText = sr.ReadToEnd();
                RawText = RawText.Trim();
                sr.Close();

                wrapper.Namespace = Options.Namespace;
                wrapper.RawText = RawText;
            }
            catch (Exception)
            {
            }
        }

        private void ParseText()
        {
            if (RawText == null)
            {
                return;
            }
            string[] tokens = RawText.Split('\n');

            foreach (var t in tokens)
            {
                ParseString(t);
            }
        }

        private void ParseString(string t)
        {
            string source = t.Trim();
            string[] tokens = source.Split(' ');
            AutomatonExecution ex = new AutomatonExecution();

            if (tokens.Length < 2)
            {
                return;
            }

            ex.Name = tokens[0];
            ex.Type = tokens[1];

            Executions.Add(ex);
        }
    }
}
