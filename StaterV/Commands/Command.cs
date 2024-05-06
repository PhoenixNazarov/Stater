using System.Collections.Generic;
using StaterV.Widgets;

namespace StaterV.Commands
{
    /// <summary>
    /// Базовый класс для всех команд. Реализует паттерн "Command".
    /// </summary>
    public abstract class Command
    {
        public Command(CommandParams aParams)
        {
            theParams = aParams;
        }

        protected CommandParams theParams;

        protected Widget widget;
        protected static Stack<Command> DoneCommands = new Stack<Command>();
        protected static Stack<Command> UndoneCommands = new Stack<Command>();
        public virtual void Execute()
        {
            if (!IsSubCommand)
            {
                theParams.Window.DoneCommands.Push(this);            
            }
        }

        public static Command Undo()
        {
            var res = UndoneCommands.Pop();
            res.Unexecute();
            return res;
        }

        public abstract void Unexecute();

        public bool IsSubCommand { get; set; }
    }
}
