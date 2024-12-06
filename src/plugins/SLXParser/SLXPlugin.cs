using Avalonia.Controls;
using Stater.Plugin;

namespace SLXParser;

public class SLXPlugin : ButtonPlugin
{
    public override PluginOutput Start(PluginInput pluginInput)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Function block files|*.slx";
        ofd.FilterIndex = 0;
        var dialogResult = ofd.ShowDialog();
        var result = PluginOutput.From("OK");
        if (dialogResult != DialogResult.OK)
        {
            result = new PluginOutput(Message: "DialogResult is not OK");
            return result;
        }

        var parser = new Parser(ofd.FileName);
        var stateflow = parser.Parse();
        var pluginStateflow = new Translator().Convert(stateflow);
        Console.WriteLine(pluginStateflow);
        result.ChangedMachines.Add(pluginStateflow);
        return result;
    }
}