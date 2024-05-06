using System;
namespace SpinVeriff.LTLAPParser
{
	public class VarParser
	{
		public VarParser(ExprParser ow)
		{
			state = States.state0;
		    owner = ow;
		}

	    private ExprParser owner;
		
		/// <summary>
		/// </summary>
		public void WriteToken()
		{
		    owner.Owner.Proposition += owner.Owner.Owner.curToken;
		}
		
		/// <summary>
		/// </summary>
		public void CloseBracket()
		{
		    owner.Owner.Proposition += ")";
		}
		
		/// <summary>
		/// </summary>
		public void WriteSpace()
		{
		    owner.Owner.Proposition += " ";
		}
		
		/// <summary>
		/// </summary>
		public void WriteVOCEnd()
		{
            owner.Owner.Proposition += ")";
        }
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			Variable,
			Array,
			MachineType,
			MachineName,
			Dot,
			OpenSqBracket,
			state13,
			state14,
			ConstantInBrackets,
			EndArray,
			Constant,
		}
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
					WriteSpace();
					break;
				case Events.constant:
					state = States.Constant;
					WriteSpace();
					WriteToken();
					WriteVOCEnd();
					break;
				}
				break;
			case States.Variable:
				switch (_event)
				{
				}
				break;
			case States.Array:
				switch (_event)
				{
				case Events.open_sq_bracket:
					state = States.OpenSqBracket;
					WriteToken();
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
					state = States.Dot;
					WriteToken();
					break;
				}
				break;
			case States.Dot:
				switch (_event)
				{
				case Events.variable:
					state = States.Variable;
					WriteToken();
					CloseBracket();
					break;
				case Events.array:
					state = States.Array;
					WriteToken();
					break;
				}
				break;
			case States.OpenSqBracket:
				switch (_event)
				{
				case Events.constant:
					state = States.ConstantInBrackets;
					WriteToken();
					break;
				}
				break;
			case States.state13:
				switch (_event)
				{
				case Events.state_name:
					if (ap.count == 0)
					{
						state = States.state14;
					}
					break;
				}
				break;
			case States.state14:
				switch (_event)
				{
				}
				break;
			case States.ConstantInBrackets:
				switch (_event)
				{
				case Events.close_sq_bracket:
					state = States.EndArray;
					WriteToken();
					CloseBracket();
					break;
				}
				break;
			case States.EndArray:
				switch (_event)
				{
				}
				break;
			case States.Constant:
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
				case "machine_type":
					state = States.MachineType;
					WriteSpace();
					break;
				case "constant":
					state = States.Constant;
					WriteSpace();
					WriteToken();
					WriteVOCEnd();
					break;
				}
				break;
			case States.Variable:
				switch (_event)
				{
				}
				break;
			case States.Array:
				switch (_event)
				{
				case "open_sq_bracket":
					state = States.OpenSqBracket;
					WriteToken();
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
					state = States.Dot;
					WriteToken();
					break;
				}
				break;
			case States.Dot:
				switch (_event)
				{
				case "variable":
					state = States.Variable;
					WriteToken();
					CloseBracket();
					break;
				case "array":
					state = States.Array;
					WriteToken();
					break;
				}
				break;
			case States.OpenSqBracket:
				switch (_event)
				{
				case "constant":
					state = States.ConstantInBrackets;
					WriteToken();
					break;
				}
				break;
			case States.state13:
				switch (_event)
				{
				case "state_name":
					if (ap.count == 0)
					{
						state = States.state14;
					}
					break;
				}
				break;
			case States.state14:
				switch (_event)
				{
				}
				break;
			case States.ConstantInBrackets:
				switch (_event)
				{
				case "close_sq_bracket":
					state = States.EndArray;
					WriteToken();
					CloseBracket();
					break;
				}
				break;
			case States.EndArray:
				switch (_event)
				{
				}
				break;
			case States.Constant:
				switch (_event)
				{
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
