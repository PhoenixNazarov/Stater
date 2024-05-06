namespace PositionsLoader
{
	public abstract class APosLoader
	{
		public enum States
		{
			state0,
			Diagram,
			State,
		}
		public enum Transitions
		{
			transition1,
			transition4,
			transition5,
			transition7,
			transition8,
			transition8_1,
			transition9,
			transition10,
			transition11,
		}
		private States state;
		public APosLoader()
		{
			state = States.state0;
		}
		/// <summary>
		///
		/// </summary>
		public abstract void WritePositions();
		/// <summary>
		///
		/// </summary>
		public abstract void SetHeight();
		/// <summary>
		///
		/// </summary>
		public abstract void SetWidth();
		/// <summary>
		///
		/// </summary>
		public abstract void SetY();
		/// <summary>
		///
		/// </summary>
		public abstract void SetX();
		/// <summary>
		///
		/// </summary>
		public abstract void SetCurState();
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.diagram:
					state = States.Diagram;
					break;
				}
				break;
			case States.Diagram:
				switch (_event)
				{
				case Events.diagram:
					state = States.state0;
					break;
				case Events.state:
					state = States.State;
					break;
				}
				break;
			case States.State:
				switch (_event)
				{
				case Events.state:
					state = States.Diagram;
					WritePositions();
					break;
				case Events.height:
					state = States.State;
					SetHeight();
					break;
				case Events.width:
					state = States.State;
					SetWidth();
					break;
				case Events.y:
					state = States.State;
					SetY();
					break;
				case Events.x:
					state = States.State;
					SetX();
					break;
				case Events.id:
					state = States.State;
					SetCurState();
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
				case "diagram":
					state = States.Diagram;
					break;
				}
				break;
			case States.Diagram:
				switch (_event)
				{
				case "diagram":
					state = States.state0;
					break;
				case "state":
					state = States.State;
					break;
				}
				break;
			case States.State:
				switch (_event)
				{
				case "state":
					state = States.Diagram;
					WritePositions();
					break;
				case "height":
					state = States.State;
					SetHeight();
					break;
				case "width":
					state = States.State;
					SetWidth();
					break;
				case "y":
					state = States.State;
					SetY();
					break;
				case "x":
					state = States.State;
					SetX();
					break;
				case "id":
					state = States.State;
					SetCurState();
					break;
				}
				break;
			}
		}
	}
}
