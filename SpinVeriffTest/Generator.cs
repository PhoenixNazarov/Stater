using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpinVeriff;
using StaterV.Attributes;
using StaterV.StateMachine;
using StaterV.Widgets;
using StaterV.Windows;

namespace SpinVeriffTest
{
    internal class Generator
    {
        public Generator()
        {
            ForkList = null;
        }

        public List<StateMachine> FSMSystem { get; private set; }
        public Options Options { get; private set; }
        public List<AutomatonExecution> ForkList { get; private set; }

        private WindowBase win;

        public void GenerateSimpleFSMSystem()
        {
            FSMSystem = new List<StateMachine>();
            AddA1();
            Options = new Options();
            Options.EnteredObjects = new List<Options.ObjectName>();
            Options.EnteredObjects.Add(new Options.ObjectName {Name = "a1", Type = "A1"});
            Options.FormulaeLTL = new List<string>();
            Options.FormulaeLTL.Add("<>{a1.s1}");
        }

        public void GenerateNestedSystem()
        {
            FSMSystem = new List<StateMachine>();
            AddA1();

            StateMachine A2 = CreateVoidFSM("A2");
            var s = AddState(A2, "s1");
            var t = AddTransition(A2, A2.StartState, s, new Event {Name = "e1"});
            FSMSystem.Add(A2);

            AddNested(FSMSystem[0].States[1], A2, "a2");

            Options = new Options();
            Options.EnteredObjects = new List<Options.ObjectName>
                {
                    new Options.ObjectName {Name = "a1", Type = "A1"},
                    new Options.ObjectName {Name = "a2", Type = "A2"}
                };
            Options.FormulaeLTL = new List<string>();
        }

        /// <summary>
        /// FSM A1 starts AServer and AClient.
        /// </summary>
        public void GenerateParallelSystem()
        {
            FSMSystem = new List<StateMachine>();
            AddA1();
            var s = AddState(FSMSystem[0], "s2");
            var t = AddTransition(FSMSystem[0], FSMSystem[0].States[1], s, new Event {Name = "e2"});

            var A2 = CreateVoidFSM("AServer");
            FSMSystem.Add(A2);
            s = AddState(A2, "s1");
            AddTransition(A2, A2.StartState, s, new Event {Name = "es1"});

            var A3 = CreateVoidFSM("AClient");
            FSMSystem.Add(A3);
            s = AddState(A3, "s1");
            var eff = new AutomatonEffect
                {
                    Type = "AServer",
                    Name = "server",
                    Event = "es1",
                    Synchronism = ESynchronism.Asynchronous,
                    TheEffectType = AutomatonEffect.EffectType.Manual
                };
            s.TheAttributes.EntryEffects.Add(eff);

            
            FSMSystem[0].States[1].TheAttributes.EntryExecutions.Add(
                new AutomatonExecution{Type = "ASerter", Name = "server"});
            FSMSystem[0].States[1].TheAttributes.EntryExecutions.Add(
                new AutomatonExecution{Type = "AClient", Name = "client"});
            

            Options = new Options();
            Options.EnteredObjects = new List<Options.ObjectName>
                {
                    new Options.ObjectName {Type = "A1", Name = "a1"}
                    //new Options.ObjectName {Type = "AServer", Name = "server"},
                    //new Options.ObjectName {Type = "AClient", Name = "client"}
                };
            Options.FormulaeLTL = new List<string>();
            ForkList = new List<AutomatonExecution>
                {
                    new AutomatonExecution {Type = "AServer", Name = "server"},
                    new AutomatonExecution {Type = "AClient", Name = "client"}
                };
        }

        public void GenerateFSMWithVariables    ()
        {
            FSMSystem = new List<StateMachine>();
            AddA1();

            FSMSystem[0].Variables.Add(new SingleVariable { Type = Variable.TypeList.Int8, Name = "b", Value = "5" });
            FSMSystem[0].Variables.Add(new SingleVariable { Type = Variable.TypeList.Int8, Name = "c", Value = "9" });
            
            FSMSystem[0].Variables.Add(
                new StaterV.StateMachine.Array{Type = Variable.TypeList.Int32, Name = "arr", NElements = 239});
            
            FSMSystem[0].Variables.Add(
                new SingleVariable{Type = Variable.TypeList.Bool, Name = "flag", Value = "true"});

            Options = new Options();                                                                                                                                                
            Options.EnteredObjects = new List<Options.ObjectName>
                {
                    new Options.ObjectName {Type = "A1", Name = "a1"}
                };
            Options.FormulaeLTL = new List<string>();
            Options.FormulaeLTL = new List<string>();
        }

        private StateMachine CreateVoidFSM(string name)
        {
            StateMachine fsm = new StateMachine();
            fsm.Name = name;
            fsm.FileName = name + ".xstd";
            win = new WindowDotNet();
            var startState = new State(win);
            startState.TheAttributes.Type = State.StateType.Start;
            startState.TheAttributes.Name = "state0";
            fsm.StartState = startState;
            fsm.States = new List<State>();
            fsm.States.Add(startState);
            fsm.Transitions = new List<Transition>();
            fsm.Events = new List<Event>();
            fsm.Variables = new List<Variable>();

            return fsm;
        }

        private State CreateState(string name)
        {
            State state = new State(win);
            state.TheAttributes.Name = name;
            return state;
        }

        private State AddState(StateMachine fsm, string name)
        {
            return AddState(fsm, name, State.StateType.Common);
        }

        private State AddState(StateMachine fsm, string name, State.StateType type)
        {
            var s = CreateState(name);
            fsm.States.Add(s);
            s.TheAttributes.Type = type;
            switch (type)
            {
                case State.StateType.Common:
                    break;
                case State.StateType.Start:
                    fsm.StartState = s;
                    break;
                case State.StateType.End:
                    fsm.EndStates.Add(s);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return s;
        }

        private Transition AddTransition(StateMachine fsm, State s, State t, Event evt)
        {
            Transition trans = new Transition(win);
            trans.Start = s;
            trans.End = t;
            trans.TheAttributes.TheEvent = evt;
            fsm.Events.Add(evt);
            
            return trans;
        }

        private void AddNested(State s, StateMachine nested, string nestedName)
        {
            s.TheAttributes.NestedMachines.Add(new NestedMachine{Type = nested.Name, Name = nestedName});
        }


        private void AddA1()
        {
            StateMachine A1 = CreateVoidFSM("A1");
            var s1 = CreateState("s1");
            A1.States.Add(s1);
            var t1 = AddTransition(A1, A1.StartState, s1, new Event {Name = "e1"});
            t1.TheAttributes.Actions.Add(
                new StaterV.Attributes.Action{Name = "func", Synchronism = ESynchronism.Synchronous});
            A1.Transitions.Add(t1);

            FSMSystem.Add(A1);
        }
    }
}
