using System.Collections.Generic;

namespace StaterV.Commands
{
    /// <summary>
    /// Класс для будущей возможности записи команд в макрос.
    /// </summary>
    public class CommandMacro : Command
    {
        public CommandMacro(CommandParams aParams) : base(aParams) {}

        private List<Command> subCommands = new List<Command>();

        public List<Command> SubCommands
        {
            get { return subCommands; }
            set { subCommands = value; }
        }

        public void AddCommand(Command c)
        {
            SubCommands.Add(c);
        }

        public void DeleteCommand(Command c)
        {
            SubCommands.Remove(c);
        }

        public override void Execute()
        {
            //Положить команду в стек. 

            foreach (var command in SubCommands)
            {
                command.Execute();
            }
        }

        public override void Unexecute()
        {
            //throw new NotImplementedException();
        }
    }
}
