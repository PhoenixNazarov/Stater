using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FBDK.FBParser;
using PluginData;

namespace FBDK
{
    public class FBDKMain : IndependentPlugin
    {
        public override IReturn Start(IParams param)
        {
            IReturn res = new IReturn();

            //Show dialog menu to find file.
            ShowDialog();

            if (agent != null)
            {
                if (agent.FSM != null)
                {
                    res.ChangedMachines.Add(agent.FSM);
                }
            }

            res.Message = "OK";
            return res;
        }

        public override bool NeedParams
        {
            get { return false; }
        }

        private Agent agent;

        private void ShowDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Function block files|*.fbt";
            ofd.FilterIndex = 0;
            var res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                agent = new Agent();
                agent.FileName = ofd.SafeFileName;
                agent.Parse(ofd.FileName);
            }
            
        }

        private static string GetName(string file)
        {
            int i = file.LastIndexOf('\\');
            const int ExtensionSize = 4;
            return file.Substring(i, file.Length - i - ExtensionSize);
        }
    }
}
