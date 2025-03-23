using Stater.SLXParser.Utils;

namespace Stater.SLXParser.Models;

public class Chart : BaseNode
{
    public string Name { get; set; }
    public DoublePoint WindowPosition { get; set; }
    public DoublePoint ViewLimits { get; set; }
    public float ZoomFactor { get; set; }
    public Color StateColor { get; set; }
    public Color StateLabelColor { get; set; }
    public Color TransitionColor { get; set; }
    public Color TransitionLabelColor { get; set; }
    public Color JunctionColor { get; set; }
    public Color ChartColor { get; set; }
    public int ViewObj { get; set; }
    public bool Visible { get; set; }

    public List<SlxState> ChildrenState = new List<SlxState>();
    public List<global::Stater.SLXParser.Models.Data> ChildrenData = new List<global::Stater.SLXParser.Models.Data>();
}