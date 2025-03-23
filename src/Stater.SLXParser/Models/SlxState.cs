using Stater.SLXParser.Utils;

namespace Stater.SLXParser.Models;

public class SlxState : BaseNode
{
    public string LabelString { get; set; }
    public DoublePoint Position { get; set; }
    public int FontSize { get; set; }
    public bool Visible { get; set; }
    public int Subviewer { get; set; }
    public string Type { get; set; }
    public string Decomposition { get; set; }
    public int ExecutionOrder { get; set; }

    public static ActiveStateOutput ActiveStateOutput = new ActiveStateOutput();
    public List<SlxState> ChildrenState = new List<SlxState>();
    public List<SlxTransition> ChildrenTransition = new List<SlxTransition>();
}

public class ActiveStateOutput
{
    public bool UseCustomName { get; set; }
    public string CustomName { get; set; }
    public bool UseCustomEnumTypeName { get; set; }
    public string EnumTypeName { get; set; }
}