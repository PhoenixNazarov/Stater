namespace XMLDiagramParser
{
	public abstract class AEvent
	{
		public enum States
		{
			state0,
			NewEvent,
		}
		public enum Transitions
		{
			transition1,
			transition4,
			transition4_1,
			transition5,
		}
		private States state;
		public AEvent()
		{
			state = States.state0;
		}
		/// <summary>
		///
		/// </summary>
		public abstract void CreateEvent();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEventName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEventComment();
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events._event:
					state = States.NewEvent;
					CreateEvent();
					break;
				}
				break;
			case States.NewEvent:
				switch (_event)
				{
				case Events._event:
					state = States.state0;
					break;
				case Events.name:
					state = States.NewEvent;
					SetEventName();
					break;
				case Events.comment:
					state = States.NewEvent;
					SetEventComment();
					break;
				}
				break;
			}
		}
		public void ProcessEventStr(string _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case "event":
					state = States.NewEvent;
					CreateEvent();
					break;
				}
				break;
			case States.NewEvent:
				switch (_event)
				{
				case "event":
					state = States.state0;
					break;
				case "name":
					state = States.NewEvent;
					SetEventName();
					break;
				case "comment":
					state = States.NewEvent;
					SetEventComment();
					break;
				}
				break;
			}
		}
	}
}
