namespace LTLAPParser
{
	public abstract class APParser
	{
		public enum States
		{
			state0,
			MachineType,
			MachineName,
			NameWithDot,
			EventFound,
			FunctionFound,
			StateFound,
			Fork,
			ForkMachine,
			NameWithArrow,
			NestedMachine,
			FirstPartArrow,
			Vert,
		}
		public enum Transitions
		{
			transition1,
			transition3,
			transition11,
			transition15,
			transition57,
			transition60,
			transition45,
			transition61,
			transition62,
			transition63,
			transition64,
			transition57_1,
			transition62_1,
			transition54,
		}
		private States state;
		public ExprParser exprParser { get; set; }
		public APParser()
		{
			state = States.state0;
		}
		/// <summary>
		///
		/// </summary>
		public abstract void WriteDefine();
		/// <summary>
		///
		/// </summary>
		public abstract void SaveMachineType();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteToken();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteToken();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteFunctionProposition();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteStateProposition();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteEventProposition();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteEndForkProp();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteEndPropNested();
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.machine_type:
					state = States.MachineType;
					WriteDefine();
					SaveMachineType();
					break;
				}
				break;
			case States.MachineType:
				switch (_event)
				{
				case Events.machine_name:
					state = States.MachineName;
					WriteToken();
					break;
				}
				break;
			case States.MachineName:
				switch (_event)
				{
				case Events.dot:
					state = States.NameWithDot;
					WriteToken();
					break;
				case Events.see_minus:
					state = States.FirstPartArrow;
					break;
				case Events.vertical_line:
					state = States.Vert;
					break;
				}
				break;
			case States.NameWithDot:
				switch (_event)
				{
				case Events.function_name:
					state = States.FunctionFound;
					WriteFunctionProposition();
					break;
				case Events.state_name:
					state = States.StateFound;
					WriteStateProposition();
					break;
				case Events.event_name:
					state = States.EventFound;
					WriteEventProposition();
					break;
				default:
					exprParser.ProcessEvent(_event);
					break;
				}
				break;
			case States.EventFound:
				switch (_event)
				{
				}
				break;
			case States.FunctionFound:
				switch (_event)
				{
				}
				break;
			case States.StateFound:
				switch (_event)
				{
				}
				break;
			case States.Fork:
				switch (_event)
				{
				case Events.machine_name:
					state = States.ForkMachine;
					WriteEndForkProp();
					break;
				case Events.machine_type:
					state = States.Fork;
					break;
				}
				break;
			case States.ForkMachine:
				switch (_event)
				{
				}
				break;
			case States.NameWithArrow:
				switch (_event)
				{
				case Events.machine_name:
					state = States.NestedMachine;
					WriteEndPropNested();
					break;
				case Events.machine_type:
					state = States.NameWithArrow;
					break;
				}
				break;
			case States.NestedMachine:
				switch (_event)
				{
				}
				break;
			case States.FirstPartArrow:
				switch (_event)
				{
				case Events.see_operator:
					state = States.NameWithArrow;
					break;
				}
				break;
			case States.Vert:
				switch (_event)
				{
				case Events.vertical_line:
					state = States.Fork;
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
				case "machine_type":
					state = States.MachineType;
					WriteDefine();
					SaveMachineType();
					break;
				}
				break;
			case States.MachineType:
				switch (_event)
				{
				case "machine_name":
					state = States.MachineName;
					WriteToken();
					break;
				}
				break;
			case States.MachineName:
				switch (_event)
				{
				case "dot":
					state = States.NameWithDot;
					WriteToken();
					break;
				case "see_minus":
					state = States.FirstPartArrow;
					break;
				case "vertical_line":
					state = States.Vert;
					break;
				}
				break;
			case States.NameWithDot:
				switch (_event)
				{
				case "function_name":
					state = States.FunctionFound;
					WriteFunctionProposition();
					break;
				case "state_name":
					state = States.StateFound;
					WriteStateProposition();
					break;
				case "event_name":
					state = States.EventFound;
					WriteEventProposition();
					break;
				default:
					exprParser.ProcessEventStr(_event);
					break;
				}
				break;
			case States.EventFound:
				switch (_event)
				{
				}
				break;
			case States.FunctionFound:
				switch (_event)
				{
				}
				break;
			case States.StateFound:
				switch (_event)
				{
				}
				break;
			case States.Fork:
				switch (_event)
				{
				case "machine_name":
					state = States.ForkMachine;
					WriteEndForkProp();
					break;
				case "machine_type":
					state = States.Fork;
					break;
				}
				break;
			case States.ForkMachine:
				switch (_event)
				{
				}
				break;
			case States.NameWithArrow:
				switch (_event)
				{
				case "machine_name":
					state = States.NestedMachine;
					WriteEndPropNested();
					break;
				case "machine_type":
					state = States.NameWithArrow;
					break;
				}
				break;
			case States.NestedMachine:
				switch (_event)
				{
				}
				break;
			case States.FirstPartArrow:
				switch (_event)
				{
				case "see_operator":
					state = States.NameWithArrow;
					break;
				}
				break;
			case States.Vert:
				switch (_event)
				{
				case "vertical_line":
					state = States.Fork;
					break;
				}
				break;
			}
		}
	}
}
