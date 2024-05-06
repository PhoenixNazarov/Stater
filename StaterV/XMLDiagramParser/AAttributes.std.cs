namespace XMLDiagramParser
{
	public abstract class AAttributes
	{
		public enum States
		{
			state0,
			Name,
			Type,
			Outgoing,
			Incoming,
			Event,
			Actions,
			Nested,
		}
		public enum Transitions
		{
			transition2,
			transition3,
			transition4,
			transition6,
			transition7,
			transition8,
			transition11,
			transition12,
			transition13,
			transition14,
			transition15,
			transition16,
			transition18,
			transition19,
			transition20,
			transition21,
			transition23,
			transition24,
			transition25,
			transition26,
			transition27,
			transition29,
			transition30,
			transition31,
		}
		private States state;
		public AAttributes()
		{
			state = States.state0;
		}
		/// <summary>
		///
		/// </summary>
		public abstract void CreateAction();
		/// <summary>
		///
		/// </summary>
		public abstract void SetName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetType();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateOutTransition();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateInTransition();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEventName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEventComment();
		/// <summary>
		///
		/// </summary>
		public abstract void SetActionSynchro();
		/// <summary>
		///
		/// </summary>
		public abstract void SetActionComment();
		/// <summary>
		///
		/// </summary>
		public abstract void SetActionName();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateNestedMachine();
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.name:
					state = States.Name;
					break;
				case Events.type:
					state = States.Type;
					break;
				case Events.incoming:
					state = States.Incoming;
					break;
				case Events.outgoing:
					state = States.Outgoing;
					break;
				case Events._event:
					state = States.Event;
					break;
				case Events.action:
					state = States.Actions;
					CreateAction();
					break;
				case Events.nested:
					state = States.Nested;
					break;
				}
				break;
			case States.Name:
				switch (_event)
				{
				case Events.name:
					state = States.state0;
					break;
				case Events.value:
					state = States.Name;
					SetName();
					break;
				}
				break;
			case States.Type:
				switch (_event)
				{
				case Events.type:
					state = States.state0;
					break;
				case Events.value:
					state = States.Type;
					SetType();
					break;
				}
				break;
			case States.Outgoing:
				switch (_event)
				{
				case Events.outgoing:
					state = States.state0;
					break;
				case Events.id:
					state = States.Outgoing;
					CreateOutTransition();
					break;
				}
				break;
			case States.Incoming:
				switch (_event)
				{
				case Events.incoming:
					state = States.state0;
					break;
				case Events.id:
					state = States.Incoming;
					CreateInTransition();
					break;
				}
				break;
			case States.Event:
				switch (_event)
				{
				case Events._event:
					state = States.state0;
					break;
				case Events.name:
					state = States.Event;
					SetEventName();
					break;
				case Events.comment:
					state = States.Event;
					SetEventComment();
					break;
				}
				break;
			case States.Actions:
				switch (_event)
				{
				case Events.action:
					state = States.state0;
					break;
				case Events.synchro:
					state = States.Actions;
					SetActionSynchro();
					break;
				case Events.comment:
					state = States.Actions;
					SetActionComment();
					break;
				case Events.name:
					state = States.Actions;
					SetActionName();
					break;
				}
				break;
			case States.Nested:
				switch (_event)
				{
				case Events.nested:
					state = States.state0;
					break;
				case Events.name:
					state = States.Nested;
					CreateNestedMachine();
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
				case "name":
					state = States.Name;
					break;
				case "type":
					state = States.Type;
					break;
				case "incoming":
					state = States.Incoming;
					break;
				case "outgoing":
					state = States.Outgoing;
					break;
				case "event":
					state = States.Event;
					break;
				case "action":
					state = States.Actions;
					CreateAction();
					break;
				case "nested":
					state = States.Nested;
					break;
				}
				break;
			case States.Name:
				switch (_event)
				{
				case "name":
					state = States.state0;
					break;
				case "value":
					state = States.Name;
					SetName();
					break;
				}
				break;
			case States.Type:
				switch (_event)
				{
				case "type":
					state = States.state0;
					break;
				case "value":
					state = States.Type;
					SetType();
					break;
				}
				break;
			case States.Outgoing:
				switch (_event)
				{
				case "outgoing":
					state = States.state0;
					break;
				case "id":
					state = States.Outgoing;
					CreateOutTransition();
					break;
				}
				break;
			case States.Incoming:
				switch (_event)
				{
				case "incoming":
					state = States.state0;
					break;
				case "id":
					state = States.Incoming;
					CreateInTransition();
					break;
				}
				break;
			case States.Event:
				switch (_event)
				{
				case "event":
					state = States.state0;
					break;
				case "name":
					state = States.Event;
					SetEventName();
					break;
				case "comment":
					state = States.Event;
					SetEventComment();
					break;
				}
				break;
			case States.Actions:
				switch (_event)
				{
				case "action":
					state = States.state0;
					break;
				case "synchro":
					state = States.Actions;
					SetActionSynchro();
					break;
				case "comment":
					state = States.Actions;
					SetActionComment();
					break;
				case "name":
					state = States.Actions;
					SetActionName();
					break;
				}
				break;
			case States.Nested:
				switch (_event)
				{
				case "nested":
					state = States.state0;
					break;
				case "name":
					state = States.Nested;
					CreateNestedMachine();
					break;
				}
				break;
			}
		}
	}
}
