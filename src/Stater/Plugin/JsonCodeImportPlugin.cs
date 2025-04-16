using System.IO;
using Stater.Domain.Json;

namespace Stater.Plugin;

public class JsonCodeImportPlugin : ButtonFilePlugin
{
    public override PluginOutput Start(PluginInput pluginInput, string path)
    {
        if (File.Exists(path))
        {
            var content = File.ReadAllText(path);
            var result = PluginOutput.From("OK");
            result.ChangedStateMachines.Add(StateMachineJsonAdapter.FromJsonSchema(content));
            return result;
        }

        return PluginOutput.From("File not found");
    }
    
    public override string Name => "Import json code";
    public override bool Directory => false;
}