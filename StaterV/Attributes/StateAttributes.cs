using System;
using System.Collections.Generic;
using StaterV.Widgets;

namespace StaterV.Attributes
{
    /// <summary>
    /// Атрибуты автомата, которые будут использоваться для кодогенерации и верификации.
    /// </summary>
    [Serializable]
    public class StateAttributes : Attributes
    {
        public StateAttributes() : base()
        {
            EntryActions = new List<Action>();    
            ExitActions = new List<Action>();
            EntryEffects = new List<AutomatonEffect>();
            //ExitEffects = new List<AutomatonEffect>();
            EntryExecutions = new List<AutomatonExecution>();
            NestedMachines = new List<NestedMachine>();
            Comment = "";
        }
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

        /// <summary>
        /// Комментарии.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Тип состояния: обычное, стартовое или конечное.
        /// </summary>
        public State.StateType Type { get; set; }


        public override object Clone()
        {
            var res = new StateAttributes();
            //res = (StateAttributes) base.MemberwiseClone();
            res = (StateAttributes) MemberwiseClone();

            if (EntryExecutions != null)
            {
                res.EntryExecutions = new List<AutomatonExecution>(EntryExecutions);
            }
            /*
            res.EntryActions = EntryActions;
            res.EntryEffects = EntryEffects;
            res.ExitActions = ExitActions;
            res.ExitEffects = ExitEffects;
            res.Comment = Comment;
             * */
            return res;
        }
    }
}
