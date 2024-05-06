using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using StaterV.Widgets;

namespace StaterV.StateMachine
{
    /// <summary>
    /// Автомат, содержащий только необходимую для его обработки информацию.
    /// </summary>
    [Serializable]
    public class StateMachine
    {
        /// <summary>
        /// Имя автомата.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Полное имя файла с автоматом.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Список состояний автомата.
        /// </summary>
        [XmlArray("StateList")]
        public List<State> States { get; set; }

        /// <summary>
        /// Список переходов автомата. Для ускорения обработки автомата.
        /// </summary>
        [XmlArray("Transitions")]
        public List<Transition> Transitions { get; set; }

        /// <summary>
        /// Список всех событий в автомате.
        /// </summary>
        [XmlArray("Events")]
        public List<Attributes.Event> Events { get; set; }

        /// <summary>
        /// Начальное состояние. Для ускорения обработки автомата.
        /// </summary>
        public State StartState { get; set; }
        
        /// <summary>
        /// Конечное состояние. Для ускорения обработки автомата.
        /// </summary>
        [XmlArray("EndStates")]
        public List<State> EndStates { get; set; }

        /// <summary>
        /// Переменные автомата.
        /// </summary>
        public List<Variable> Variables { get; set; }

        /// <summary>
        /// Флаг, который показывает, будет ли автомат переходить в недопускающее состояние в случае, если пришло 
        /// неожиданное событие.
        /// </summary>
        public bool AutoReject { get; set; }

        public string Epsilon { get; set; }

        public void ConvertEpsilon()
        {
            var names = CollectAllNames();
            Epsilon = "epsilon";
            while (names.Contains(Epsilon))
            {
                Epsilon = "_" + Epsilon;
            }
            foreach (var @event in Events.Where(@event => @event.Name == "*"))
            {
                @event.Name = Epsilon;
            }

            foreach (var transition in Transitions)
            {
                if (transition.TheAttributes.TheEvent.Name == "*")
                {
                    transition.TheAttributes.TheEvent.Name = Epsilon;
                }
            }
        }

        private HashSet<string> CollectAllNames()
        {
            HashSet<string> res = new HashSet<string>();
            res.Add(Name);
            foreach (var variable in Variables)
            {
                res.Add(variable.Name);
            }
            foreach (var transition in Transitions)
            {
                res.Add(transition.TheAttributes.TheEvent.Name);
            }
            foreach (var state in States)
            {
                res.Add(state.TheAttributes.Name);
            }
            return res;
        }
    }
}
