namespace StateFlow.SFDParser
{
	public interface IStateMachine
	{
		void ProcessEvent(Events _event);
		void ProcessEvent(string _event);
	}
	
}
