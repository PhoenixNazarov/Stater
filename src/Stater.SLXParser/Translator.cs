using Stater.Domain.Models;
using Stater.SLXParser.Models;

namespace Stater.SLXParser;

public class Translator
{
    public StateMachine Convert(Stateflow stateflow)
    {
        var stateMachine = new StateMachine
        {
            // Type = "SLX",
            Name = stateflow.Instance.Name,
            States = ConvertChart(stateflow.Machine.Chart),
            Variables = ConvertVariable(stateflow.Machine.Chart.ChildrenData),
        };

        stateMachine.Transitions.AddRange(ConvertChartTransitions(stateflow.Machine.Chart, stateMachine.States));

        stateMachine.States[0] = stateMachine.States[0] with { Type = StateType.Start };
        stateMachine.States[^1] = stateMachine.States[^1] with { Type = StateType.End };

        return stateMachine;
    }

    private static List<State> ConvertChart(Chart chart)
    {
        var states = new List<State>();

        foreach (var state in chart.ChildrenState)
        {
            states.AddRange(ConvertState(state));
        }

        return states;
    }

    private static IEnumerable<State> ConvertState(SlxState slxSlxState)
    {
        var state = new State
        {
            Guid = slxSlxState.Id,
            Name = slxSlxState.LabelString,
            Type = StateType.Common,
            X = slxSlxState.Position.X.X,
            Y = slxSlxState.Position.X.Y,
        };

        var states = new List<State> { state };

        foreach (var slxStateChildren in slxSlxState.ChildrenState)
        {
            states.AddRange(ConvertState(slxStateChildren));
        }

        return states;
    }

    private List<Transition> ConvertChartTransitions(Chart chart, List<State> slxStateOriginList)
    {
        var transitions = new List<Transition>();

        foreach (var state in chart.ChildrenState)
        {
            transitions.AddRange(ConvertStateTransition(state, slxStateOriginList));
        }

        return transitions;
    }

    private static IEnumerable<Transition> ConvertStateTransition(SlxState slxSlxState, List<State> slxStateOriginList)
    {
        var transitions = new List<Transition>();

        foreach (var transition in slxSlxState.ChildrenTransition)
        {
            var transition_ = ConvertTransition(transition, slxStateOriginList);
            if (transition_ != null)
            {
                transitions.Add(transition_);
            }
        }

        foreach (var state in slxSlxState.ChildrenState)
        {
            transitions.AddRange(ConvertStateTransition(state, slxStateOriginList));
        }

        return transitions;
    }

    private static Transition ConvertTransition(SlxTransition slxTransition, List<State> slxStateOriginList)
    {
        var transition = new Transition
        {
            Guid = slxTransition.Id
        };

        var start = FindStateById(slxTransition.Src.SSID, slxStateOriginList);
        var end = FindStateById(slxTransition.Dst.SSID, slxStateOriginList);

        if (start == null || end == null)
        {
            return null;
        }

        transition = transition with
        {
            Start = start.Guid,
            End = end.Guid,
            Name = slxTransition.LabelString
        };

        // start.Outgoing.Add(transition);
        // end.Incoming.Add(transition);

        return transition;
    }

    private static State FindStateById(Guid id, List<State> slxStateOriginList)
    {
        return slxStateOriginList.FirstOrDefault(state => state.Guid == id);
    }

    private static List<Variable> ConvertVariable(List<Data> datas)
    {
        var result = new List<Variable>();
        foreach (var data in datas)
        {
            // var variableValue = VariableValueBuilder.fromString("");

            var variable = new Variable
            {
                Name = data.Name
            };

            // variable.ID = new UID(data.Id);
            result.Add(variable);
        }

        return result;
    }
}