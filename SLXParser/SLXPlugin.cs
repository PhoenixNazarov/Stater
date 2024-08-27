using System.Windows.Forms;
using PluginData;

namespace SLXParser
{
    public class SLXPlugin : IndependentPlugin
    {
        public override IReturn Start(IParams param)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Function block files|*.slx";
            ofd.FilterIndex = 0;
            var dialogResult = ofd.ShowDialog();
            var result = new IReturn();
            if (dialogResult != DialogResult.OK) return result;
            
            var parser = new Parser(ofd.SafeFileName);
            var stateflow = parser.Parse();
            var pluginStateflow = new Translator().Convert(stateflow);
            result.ChangedMachines.Add(pluginStateflow);

            return result;
        }

        public override bool NeedParams
        {
            get { return false; }
        }
    }
}