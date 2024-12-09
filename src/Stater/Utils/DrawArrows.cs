using Avalonia;
using Stater.Models;

namespace Stater.Utils;

public abstract record DrawArrows
{
    public State? Start { get; set; }
    public State? End { get; set; }

}