using System;
namespace SpinVeriff.LTLAPParser
{
	public class ExprParser
	{
		public ExprParser(APParser ap)
		{
			state = States.state0;
		    owner = ap;
            varParser = new VarParser(this);
		}

	    private readonly APParser owner;
        public APParser Owner
        {
            get { return owner; }
        }

		
		/// <summary>
		/// </summary>
		public void Reject()
		{
		}
		
		/// <summary>
		/// </summary>
		public void WriteSpace()
		{
            Owner.Proposition += " ";
        }
		
		/// <summary>
		/// </summary>
		public void WriteToken()
		{
            Owner.Proposition += Owner.Owner.curToken;
        }
		
		/// <summary>
		/// </summary>
		public void WriteVOCEnd()
		{
            Owner.Proposition += ")";
        }
		
		/// <summary>
		/// </summary>
		public void WriteNestedMachine()
		{
            Owner.Proposition += ".nestedMachine == " + Owner.Owner.curToken + ")";
        }
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			Variable,
			VarAndOp,
			SArray,
			SQBracket,
			Index,
			EArray,
			IMT,
			IMN,
			IMND,
			reject,
		}
		public VarParser varParser { get; set; }
		private States state;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.variable:
					state = States.Variable;
					WriteToken();
					break;
				case Events.array:
					state = States.SArray;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Variable:
				switch (_event)
				{
				case Events.see_operator:
					state = States.VarAndOp;
					WriteSpace();
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.VarAndOp:
				switch (_event)
				{
				default:
					varParser.ProcessEvent(_event);
					break;
				}
				break;
			case States.SArray:
				switch (_event)
				{
				case Events.open_sq_bracket:
					state = States.SQBracket;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.SQBracket:
				switch (_event)
				{
				case Events.machine_type:
					state = States.IMT;
					WriteNestedMachine();
					break;
				case Events.constant:
					state = States.Index;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Index:
				switch (_event)
				{
				case Events.close_sq_bracket:
					state = States.EArray;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.EArray:
				switch (_event)
				{
				case Events.see_operator:
					state = States.VarAndOp;
					WriteSpace();
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.IMT:
				switch (_event)
				{
				case Events.machine_name:
					state = States.IMN;
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.IMN:
				switch (_event)
				{
				case Events.dot:
					state = States.IMND;
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.IMND:
				switch (_event)
				{
				case Events.variable:
					state = States.Index;
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
				case "variable":
					state = States.Variable;
					WriteToken();
					break;
				case "array":
					state = States.SArray;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Variable:
				switch (_event)
				{
				case "see_operator":
					state = States.VarAndOp;
					WriteSpace();
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.VarAndOp:
				switch (_event)
				{
				default:
					varParser.ProcessEvent(_event);
					break;
				}
				break;
			case States.SArray:
				switch (_event)
				{
				case "open_sq_bracket":
					state = States.SQBracket;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.SQBracket:
				switch (_event)
				{
				case "machine_type":
					state = States.IMT;
					WriteNestedMachine();
					break;
				case "constant":
					state = States.Index;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.Index:
				switch (_event)
				{
				case "close_sq_bracket":
					state = States.EArray;
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.EArray:
				switch (_event)
				{
				case "see_operator":
					state = States.VarAndOp;
					WriteSpace();
					WriteToken();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.IMT:
				switch (_event)
				{
				case "machine_name":
					state = States.IMN;
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.IMN:
				switch (_event)
				{
				case "dot":
					state = States.IMND;
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.IMND:
				switch (_event)
				{
				case "variable":
					state = States.Index;
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
