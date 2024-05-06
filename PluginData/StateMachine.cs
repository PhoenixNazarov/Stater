using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class StateMachine
    {
        public StateMachine()
        {
            States = new List<State>();
            Transitions = new List<Transition>();
            Events = new List<Event>();
            EndStates = new List<State>();
            Variables = new List<Variable>();
        }

        /// <summary>
        /// Тип автомата.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Имя экземпляра автомата.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список состояний автомата.
        /// </summary>
        public List<State> States { get; set; }

        /// <summary>
        /// Список переходов автомата. Для ускорения обработки автомата.
        /// </summary>
        public List<Transition> Transitions { get; set; }

        /// <summary>
        /// Список всех событий в автомате.
        /// </summary>
        public List<Event> Events { get; set; }

        /// <summary>
        /// Начальное состояние. Для ускорения обработки автомата.
        /// </summary>
        public State StartState { get; set; }

        /// <summary>
        /// Конечные состояния. Для ускорения обработки автомата.
        /// </summary>
        public List<State> EndStates { get; set; }

        /// <summary>
        /// Список переменных автомата.
        /// </summary>
        public List<Variable> Variables { get; set; }

        /// <summary>
        /// Флаг, который показывает, будет ли автомат переходить в недопускающее состояние в случае, если пришло 
        /// неожиданное событие.
        /// </summary>
        public bool AutoReject { get; set; }

        public State GetState(UID id)
        {
            return States.FirstOrDefault(state => state.ID.Value == id.Value);
        }

        public IEnumerable<Action> GetAllActions()
        {
            Dictionary<string, Action> allActions = new Dictionary<string, Action>();

            foreach (var transition in Transitions)
            {
                AddActions(ref allActions, transition.Actions);
            }

            foreach (var state in States)
            {
                AddActions(ref allActions, state.EntryActions);
                AddActions(ref allActions, state.ExitActions);
            }

            return allActions.Values;
        }

        private void AddActions(ref Dictionary<string, Action> allActions, List<Action> actionList)
        {
            foreach (var action in actionList)
            {
                if (!allActions.ContainsKey(action.Name))
                {
                    allActions.Add(action.Name, action);
                }
            }
        }
    }
}
