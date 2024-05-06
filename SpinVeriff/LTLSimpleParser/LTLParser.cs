using System;
using System.Collections.Generic;

namespace SpinVeriff.LTLSimpleParser
{
    /// <summary>
    /// As output we form 
    /// </summary>
	public class LTLParser
	{
		public LTLParser(LTLConverter _owner, int _index)
		{
			state = States.state0;
		    owner = _owner;
		    index = _index;
		}

        public void ResetFSM()
        {
            state = States.state0;
            Proposition = "";
        }

#region ~~~~~~Declarations~~~~~~
        
        private readonly LTLConverter owner;
        public string Proposition { get; private set; }
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
		public void WriteEventProposition()
		{
            Proposition += "curEvent == " + owner.curToken + ")";
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
		public void WriteToken()
		{
            Proposition += owner.curToken;
		}
		
		/// <summary>
		/// </summary>
		public void WriteNestedMachine()
		{
		    Proposition += ".nestedMachine == " + owner.curToken + ")";
		}
		
		/// <summary>
		/// </summary>
		public void WriteSpace()
		{
		    Proposition += " ";
		}
		
		/// <summary>
		/// </summary>
		public void WriteVOCEnd()
		{
		    Proposition += ")";
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
			Static,
			MachineName,
			StaticVariableFound,
			NameWithDot,
			Variable,
			VarAndOp,
			VOC,
			SArray,
			SQBracket,
			Index,
			EArray,
			IMT,
			IMN,
			IMND,
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
				case Events.static_keyword:
					state = States.Static;
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
			case States.Static:
				switch (_event)
				{
				case Events.static_variable:
					state = States.StaticVariableFound;
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
			case States.StaticVariableFound:
				switch (_event)
				{
				case Events.see_operator:
					state = States.VarAndOp;
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
				case Events.variable:
					state = States.Variable;
					WriteToken();
					break;
				case Events.array:
					state = States.SArray;
					WriteToken();
					break;
				case Events.event_name:
					state = States.EventFound;
					WriteEventProposition();
					break;
				case Events.function_name:
					state = States.FunctionFound;
					WriteFunctionProposition();
					break;
				case Events.state_name:
					state = States.StateFound;
					WriteStateProposition();
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
				case Events.constant:
					state = States.VOC;
					WriteSpace();
					WriteToken();
					WriteVOCEnd();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.VOC:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
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
				case "static_keyword":
					state = States.Static;
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
			case States.Static:
				switch (_event)
				{
				case "static_variable":
					state = States.StaticVariableFound;
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
			case States.StaticVariableFound:
				switch (_event)
				{
				case "see_operator":
					state = States.VarAndOp;
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
				case "variable":
					state = States.Variable;
					WriteToken();
					break;
				case "array":
					state = States.SArray;
					WriteToken();
					break;
				case "event_name":
					state = States.EventFound;
					WriteEventProposition();
					break;
				case "function_name":
					state = States.FunctionFound;
					WriteFunctionProposition();
					break;
				case "state_name":
					state = States.StateFound;
					WriteStateProposition();
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
				case "constant":
					state = States.VOC;
					WriteSpace();
					WriteToken();
					WriteVOCEnd();
					break;
				default:
					state = States.reject;
					Reject();
					break;
				}
				break;
			case States.VOC:
				switch (_event)
				{
				default:
					state = States.reject;
					Reject();
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
