using System.Collections.Generic;
using System.IO;
using Stater.CodeGeneration;

namespace Stater.Plugin;

public class JsonCodeImportPlugin : ButtonFilePlugin
{
    public override PluginOutput Start(PluginInput pluginInput, string path)
    {
        if (File.Exists(path))
        {
            var content = File.ReadAllText(path);
            var result = PluginOutput.From("OK");
            result.ChangedStateMachines.Add(JsonLoader.Load(content));
            return result;
        }

        return PluginOutput.From("File not found");
    }
    
    public override string Name => "Import json code";
}