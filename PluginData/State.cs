using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class State
    {
        public State()
        {
            ID = new UID();
            Incoming = new List<Transition>();
            Outgoing = new List<Transition>();
            Type = StateType.Common;
            EntryActions = new List<Action>();
            EntryEffects = new List<AutomatonEffect>();
            EntryExecutions = new List<AutomatonExecution>();
            ExitActions = new List<Action>();
            NestedMachines = new List<NestedMachine>();
        }

        public UID ID { get; set; }

        public string Name { get; set; }

        public List<Transition> Incoming { get; set; }

        public List<Transition> Outgoing { get; set; }

        public enum StateType
        {
            Common,
            Start, 
            End
        }
        public StateType Type { get; set; }

        /// <summary>
        /// Воздействия автомата при входе в состояние.
        /// </summary>
        public List<Action> EntryActions { get; set; }

        /// <summary>
        /// Воздействия автомата при выходе из состояния.
        /// </summary>
        public List<Action> ExitActions { get; set; }

        /// <summary>
        /// Воздействия автомата на другие автоматы при входе в состояние.
        /// </summary>
        public List<AutomatonEffect> EntryEffects { get; set; }

        /// <summary>
        /// Воздействия автомата на другие автоматы при выходе из состояния.
        /// </summary>
        //public List<AutomatonEffect> ExitEffects { get; set; }

        /// <summary>
        /// Запуск других автоматов при входе в состояние.
        /// </summary>
        public List<AutomatonExecution> EntryExecutions { get; set; }

        /// <summary>
        /// Вложенные автоматы.
        /// </summary>
        public List<NestedMachine> NestedMachines { get; set; }

    }
}
