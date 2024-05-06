using System;
namespace SpinVeriff.LTLAPParser
{
	public class APParser
	{
        public APParser(LTLConverter _owner, int _index)
		{
			state = States.state0;
            owner = _owner;
            index = _index;
            exprParser = new ExprParser(this);
        }

        #region ~~~~~~Declarations~~~~~~

        private readonly LTLConverter owner;
	    public LTLConverter Owner
	    {
            get { return owner; }
	    }
        public string Proposition { get; set; }
        private readonly int index;
        private string curFSM;

        public States State
        {
            get { return state; }
        }

        #endregion
        
        /// <summary>
		/// </summary>
		public void Reject()
		{
		}
		
		/// <summary>
		/// </summary>
		public void WriteDefine()
		{
            Proposition = "#define p" + index + " (";
        }
		
		/// <summary>
		/// </summary>
		public void SaveMachineType()
		{
            owner.SaveCurFSM();
            curFSM = owner.curMachine.Name;
        }
		
		/// <summary>
		/// </summary>
		public void WriteToken()
		{
            Proposition += owner.curToken;
        }
		
		/// <summary>
		/// </summary>
		public void WriteFunctionProposition()
		{
            Proposition += "functionCall == " + curFSM + "_" + owner.curToken + ")";
        }
		
		/// <summary>
		/// </summary>
		public void WriteStateProposition()
		{
            Proposition += "state == " + curFSM + "_" + owner.curToken + ")";
        }
		
		/// <summary>
		/// </summary>
		public void WriteEventProposition()
		{
            Proposition += "curEvent == " + owner.curToken + ")";
        }
		
		/// <summary>
		/// </summary>
		public void WriteEndForkProp()
		{
            Proposition += ".forkMachine == " + owner.curToken + ")";
        }
		
		/// <summary>
		/// </summary>
		public void WriteEndPropNested()
		{
            Proposition += ".nestedMachine == " + owner.curToken + ")";
        }
#region ~~~~~~Generated functions~~~~~~
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
			reject,
		}
		public ExprParser exprParser { get; set; }
		private States state;
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
				default:
					state = States.reject;
					Reject();
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
				default:
					state = States.reject;
					Reject();
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
				default:
					state = States.reject;
					Reject();
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
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.FunctionFound:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.StateFound:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Fork:
				switch (_event)
				{
				case Events.machine_type:
					state = States.Fork;
					break;
				case Events.machine_name:
					state = States.ForkMachine;
					WriteEndForkProp();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.ForkMachine:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.NameWithArrow:
				switch (_event)
				{
				case Events.machine_type:
					state = States.NameWithArrow;
					break;
				case Events.machine_name:
					state = States.NestedMachine;
					WriteEndPropNested();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.NestedMachine:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.FirstPartArrow:
				switch (_event)
				{
				case Events.see_operator:
					state = States.NameWithArrow;
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Vert:
				switch (_event)
				{
				case Events.vertical_line:
					state = States.Fork;
					break;
				default:
					state = States.reject;
					Reject();
					break;
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
				case "machine_type":
					state = States.MachineType;
					WriteDefine();
					SaveMachineType();
					break;
				default:
					state = States.reject;
					Reject();
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
				default:
					state = States.reject;
					Reject();
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
				default:
					state = States.reject;
					Reject();
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
					exprParser.ProcessEvent(_event);
					break;
				}
				break;
			case States.EventFound:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.FunctionFound:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.StateFound:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Fork:
				switch (_event)
				{
				case "machine_type":
					state = States.Fork;
					break;
				case "machine_name":
					state = States.ForkMachine;
					WriteEndForkProp();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.ForkMachine:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.NameWithArrow:
				switch (_event)
				{
				case "machine_type":
					state = States.NameWithArrow;
					break;
				case "machine_name":
					state = States.NestedMachine;
					WriteEndPropNested();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.NestedMachine:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.FirstPartArrow:
				switch (_event)
				{
				case "see_operator":
					state = States.NameWithArrow;
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Vert:
				switch (_event)
				{
				case "vertical_line":
					state = States.Fork;
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
