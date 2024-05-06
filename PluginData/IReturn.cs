using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    /// <summary>
    /// Данные, которые возвращает плагин. Если плагин не хочет возвращать какие-то элементы, 
    /// то соответствующие свойства делаем null.
    /// </summary>
    public class IReturn
    {
        public IReturn()
        {
            AffectedMachines = new List<StateMachine>();
            AffectedStates = new List<State>();
            AffectedTransitions = new List<Transition>();
            ChangedMachines = new List<StateMachine>();
        }
        /// <summary>
        /// Если не null, то выводится MessageBox с сообщением Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Автоматы, которые надо подсветить.
        /// </summary>
        public List<StateMachine> AffectedMachines { get; set; }

        /// <summary>
        /// Состояния, которые надо подсветить.
        /// </summary>
        public List<State> AffectedStates { get; set;}

        /// <summary>
        /// Переходы, которые надо подсветить.
        /// </summary>
        public List<Transition> AffectedTransitions { get; set; }

        /// <summary>
        /// Изменившиеся автоматы.
        /// </summary>
        public List<StateMachine> ChangedMachines { get; set; }
    }
}
