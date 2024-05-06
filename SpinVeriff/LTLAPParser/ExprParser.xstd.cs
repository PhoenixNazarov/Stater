namespace LTLAPParser
{
	public abstract class ExprParser
	{
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
		}
		public enum Transitions
		{
			transition1,
			transition40,
			transition41,
			transition30,
			transition24,
			transition32,
			transition33,
			transition38,
			transition29,
			transition35,
			transition37,
		}
		private States state;
		public VarParser varParser { get; set; }
		public ExprParser()
		{
			state = States.state0;
		}
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
		public abstract void WriteSpace();
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
		public abstract void WriteNestedMachine();
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
		public abstract void WriteSpace();
		/// <summary>
		///
		/// </summary>
		public abstract void WriteToken();
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
				}
				break;
			case States.Index:
				switch (_event)
				{
				case Events.close_sq_bracket:
					state = States.EArray;
					WriteToken();
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
				}
				break;
			case States.IMT:
				switch (_event)
				{
				case Events.machine_name:
					state = States.IMN;
					break;
				}
				break;
			case States.IMN:
				switch (_event)
				{
				case Events.dot:
					state = States.IMND;
					break;
				}
				break;
			case States.IMND:
				switch (_event)
				{
				case Events.variable:
					state = States.Index;
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
				case "variable":
					state = States.Variable;
					WriteToken();
					break;
				case "array":
					state = States.SArray;
					WriteToken();
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
				}
				break;
			case States.VarAndOp:
				switch (_event)
				{
				default:
					varParser.ProcessEventStr(_event);
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
				}
				break;
			case States.Index:
				switch (_event)
				{
				case "close_sq_bracket":
					state = States.EArray;
					WriteToken();
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
				}
				break;
			case States.IMT:
				switch (_event)
				{
				case "machine_name":
					state = States.IMN;
					break;
				}
				break;
			case States.IMN:
				switch (_event)
				{
				case "dot":
					state = States.IMND;
					break;
				}
				break;
			case States.IMND:
				switch (_event)
				{
				case "variable":
					state = States.Index;
					break;
				}
				break;
			}
		}
	}
}
