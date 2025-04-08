using Stater.SLXParser;

namespace Stater.Plugin;

public class SLXPlugin: ButtonFilePlugin
{
    public override PluginOutput Start(PluginInput pluginInput, string file)
    {
        var parser = new Parser(file);
        var stateflow = parser.Parse();
        var pluginStateflow = new Translator().Convert(stateflow);
        var result = PluginOutput.From("Ok");
        result.ChangedStateMachines.Add(pluginStateflow);
        return result;
    }
    
    public override string Name => "SLXParser";
}