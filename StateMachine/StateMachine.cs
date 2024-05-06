using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachine
{
    public class StateMachine
    {
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
    }
}
