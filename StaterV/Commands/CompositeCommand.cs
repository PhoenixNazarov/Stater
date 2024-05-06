namespace StaterV.Commands
{
    public class CompositeCommand : Command
    {
        public CompositeCommand(CommandParams aParams) : base(aParams)
        {
            theParams = (CompositeParams) aParams;
        }

        private new readonly CompositeParams theParams;

        public override void Execute()
        {
            base.Execute();
            foreach (var cmd in theParams.Commands)
            {
                cmd.IsSubCommand = true;
                cmd.Execute();
            }
        }

        public override void Unexecute()
        {
            for (int i = theParams.Commands.Count - 1; i >= 0; i--)
            {
                theParams.Commands[i].Unexecute();
            }
        }
    }
}
