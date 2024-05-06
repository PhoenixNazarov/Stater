using System;
namespace FBDK.FBParser
{
	public class AComposite : IStateMachine
	{
		public AComposite()
		{
			state = States.state0;
		}
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			state1,
		}
		private States state;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.FBType:
					state = States.state1;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				}
				break;
			}
		}
		public void ProcessEvent(string _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case "FBType":
					state = States.state1;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
