using System.IO;
using Stater.Domain.Json;

namespace Stater.Plugin;

public class JsonCodeExportPlugin : ButtonFilePlugin
{
    public override PluginOutput Start(PluginInput pluginInput, string path)
    {
        System.IO.Directory.CreateDirectory(path);
        var jsonSchema = StateMachineJsonAdapter.ToJsonSchema(pluginInput.StateMachine);
        File.WriteAllText(Path.Combine(path, pluginInput.StateMachine.Name + "_schema.json"), jsonSchema);
        return PluginOutput.From("OK");
    }

    public override string Name => "Export json code";
    public override bool Directory => true;
}