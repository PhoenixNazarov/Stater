using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PluginData;
using SLXParser.Data;
using State = PluginData.State;
using SLXState = SLXParser.Data.State;
using Transition = PluginData.Transition;
using SLXTransition = SLXParser.Data.Transition;

namespace SLXParser
{
    public class Translator
    {
        public StateMachine Convert(Stateflow stateflow)
        {
            var stateMachine = new StateMachine
            {
                Type = "SLX",
                Name = stateflow.Instance.Name,
                States = ConvertChart(stateflow.Machine.Chart)
            };

            stateMachine.Transitions = ConvertChartTransitions(stateflow.Machine.Chart, stateMachine.States);

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

        private static IEnumerable<State> ConvertState(SLXState slxState)
        {
            var state = new State
            {
                ID = new UID(slxState.Id),
                Name = slxState.LabelString,
                Type = State.StateType.Common
            };

            var states = new List<State> { state };

            foreach (var slxStateChildren in slxState.ChildrenState)
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

        private static IEnumerable<Transition> ConvertStateTransition(SLXState slxState, List<State> slxStateOriginList)
        {
            var transitions = new List<Transition>();

            foreach (var transition in slxState.ChildrenTransition)
            {
                transitions.Add(ConvertTransition(transition, slxStateOriginList));
            }

            return transitions;
        }

        private static Transition ConvertTransition(SLXTransition slxTransition, List<State> slxStateOriginList)
        {
            var transition = new Transition
            {
                ID = new UID(slxTransition.Id)
            };

            var start = FindStateById(new UID(slxTransition.Src.SSID), slxStateOriginList);
            var end = FindStateById(new UID(slxTransition.Dst.SSID), slxStateOriginList);

            transition.Start = start;
            transition.End = end;

            start.Outgoing.Add(transition);
            end.Incoming.Add(transition);

            return transition;
        }

        private static State FindStateById(UID id, List<State> slxStateOriginList)
        {
            return slxStateOriginList.FirstOrDefault(state => state.ID == id);
        }
    }
}