namespace Stater.Plugin;

public abstract class ButtonFilePlugin : IPlugin
{
    public abstract PluginOutput Start(PluginInput pluginInput, string path);
    public abstract bool Directory { get; }
    
    public virtual string Name => "ButtonFilePlugin";
}