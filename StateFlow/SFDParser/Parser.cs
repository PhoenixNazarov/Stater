using System;
using System.Linq;
using PluginData;
using System.Collections.Generic;
namespace StateFlow.SFDParser
{
	public class Parser : IStateMachine
	{
		public Parser(Importer ow)
		{
			state = States.state0;
            dgr = new AChart(this);
            astate = new AState(this);
            avariable = new AVariable(this);
            atransition = new ATransition(this);
            Owner = ow;
            LoadedFSMs = new List<StateMachine>();
            machineIdList = new Dictionary<int, StateMachine>();
		}

        public List<StateMachine> LoadedFSMs { get; private set; }
        private Dictionary<int, StateMachine> machineIdList;
        public StateMachine CurMachine { get; private set; }
        public State CurState { get; private set; }
        public Variable CurVariable { get; private set; }
        public int CurVariableId { get; set; }
        public string CurVariableName { get; set; }
        public int CurArrayLength { get; set; }
        public Transition CurTransition { get; private set; }
        public Importer Owner { get; set; }
        
        /// <summary>
		/// </summary>
		private void CreateNewFSM()
		{
            CurMachine = new StateMachine();
		}
		
		/// <summary>
		/// </summary>
		private void AddFSM()
		{
            LoadedFSMs.Add(CurMachine);
            try
            {
                machineIdList.Add(dgr.ID, CurMachine);

            }
            catch (Exception)
            {
            }
        }
		
		/// <summary>
		/// </summary>
		private void CreateNewState()
		{
            CurState = new State();
		}
		
		/// <summary>
		/// </summary>
		private void AddState()
		{
            if (astate.StatemachineID != -1)
            {
                machineIdList[astate.StatemachineID].States.Add(CurState);
            }
            else
            {
                CurMachine.States.Add(CurState);
            }

		}
		
		/// <summary>
		/// </summary>
		private void AddVariable()
		{
		    if (CurArrayLength < 1)
		    {
		        //Error!
		        return;
		    }
		    if (CurArrayLength == 1)
		    {
		        CurVariable = new SingleVariable();
		    }
		    else
		    {
		        CurVariable = new PluginData.Array {NElements = CurArrayLength};
		    }
		    CurVariable.Name = CurVariableName;
            CurVariable.ID = new UID(CurVariableId);

            CurMachine.Variables.Add(CurVariable);
		}
		
		/// <summary>
		/// </summary>
		private void ResetVariable()
		{
            avariable.Reset();
		    CurVariable = null;
		    CurVariableId = -1;
		    CurVariableName = "";
		    CurArrayLength = 1;
		}
		
		/// <summary>
		/// </summary>
		private void CreateNewTransition()
		{
            CurTransition = new Transition();
		}
		
		/// <summary>
		/// </summary>
		private void SetTransBegin()
		{
		    int beginID = int.Parse(Owner.CurText);
		    foreach (var st in CurMachine.States.Where(st => st.ID.Value == beginID))
		    {
		        CurTransition.Start = st;
                CurTransition.Start.Outgoing.Add(CurTransition);
                return;
		    }
		}

	    /// <summary>
		/// </summary>
		private void SetTransEnd()
		{
            int endID = int.Parse(Owner.CurText);
            foreach (var st in CurMachine.States.Where(st => st.ID.Value == endID))
            {
                CurTransition.End = st;
                CurTransition.End.Incoming.Add(CurTransition);
                return;
            }
        }
		
		/// <summary>
		/// </summary>
		private void SetArraySize()
		{
		    CurArrayLength = int.Parse(Owner.CurText);
		}
		
		/// <summary>
		/// </summary>
		private void AddTransition()
		{
            CurMachine.Transitions.Add(CurTransition);
		}
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			state1,
			PrefixFound,
			Project,
			Project1,
			Diagram,
			Diagram1,
			NestedBrackets,
			SState,
			SState1,
			SVariable,
			SVariable1,
			STransition,
			STransition1,
			VarProps,
			SArray,
			state37,
			SSrc,
			SSrc1,
			Src_ID,
			Src_ID1,
			SDst,
			SDst1,
			Dst_ID,
			Dst_ID1,
			state58,
			SArraySize,
			SArraySize1,
			SArraySize2,
			state67,
			state70,
		}
		public AChart dgr { get; set; }
		public AState astate { get; set; }
		public AVariable avariable { get; set; }
		public ATransition atransition { get; set; }
		private States state;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.Stateflow:
					state = States.state1;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.PrefixFound;
					break;
				default:
					state = States.state0;
					break;
				}
				break;
			case States.PrefixFound:
				switch (_event)
				{
				case Events.machine:
					state = States.Project;
					break;
				case Events.chart:
					state = States.Diagram;
					CreateNewFSM();
					break;
				case Events.state:
					state = States.SState;
					break;
				case Events.data:
					state = States.SVariable;
					ResetVariable();
					break;
				case Events.transition:
					state = States.STransition;
					CreateNewTransition();
					break;
				}
				break;
			case States.Project:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.Project1;
					break;
				}
				break;
			case States.Project1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.PrefixFound;
					break;
				}
				break;
			case States.Diagram:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.Diagram1;
					break;
				}
				break;
			case States.Diagram1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.PrefixFound;
					AddFSM();
					break;
				case Events.open_curl_br:
					state = States.NestedBrackets;
					break;
				default:
					dgr.ProcessEvent(_event);
					break;
				}
				break;
			case States.NestedBrackets:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.Diagram1;
					break;
				}
				break;
			case States.SState:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.SState1;
					CreateNewState();
					break;
				}
				break;
			case States.SState1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.PrefixFound;
					AddState();
					break;
				default:
					astate.ProcessEvent(_event);
					break;
				}
				break;
			case States.SVariable:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.SVariable1;
					break;
				}
				break;
			case States.SVariable1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.PrefixFound;
					AddVariable();
					break;
				case Events.props:
					state = States.VarProps;
					break;
				default:
					avariable.ProcessEvent(_event);
					break;
				}
				break;
			case States.STransition:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.STransition1;
					break;
				}
				break;
			case States.STransition1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.PrefixFound;
					AddTransition();
					break;
				case Events.src:
					state = States.SSrc;
					break;
				case Events.dst:
					state = States.SDst;
					break;
				case Events.open_curl_br:
					state = States.state58;
					break;
				default:
					atransition.ProcessEvent(_event);
					break;
				}
				break;
			case States.VarProps:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.state37;
					break;
				}
				break;
			case States.SArray:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.SArraySize;
					break;
				}
				break;
			case States.state37:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.SVariable1;
					break;
				case Events.array:
					state = States.SArray;
					break;
				case Events.open_curl_br:
					state = States.state70;
					break;
				}
				break;
			case States.SSrc:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.SSrc1;
					break;
				}
				break;
			case States.SSrc1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.STransition1;
					break;
				case Events.id:
					state = States.Src_ID;
					break;
				}
				break;
			case States.Src_ID:
				switch (_event)
				{
				case Events.number:
					state = States.Src_ID1;
					SetTransBegin();
					break;
				}
				break;
			case States.Src_ID1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.STransition1;
					break;
				}
				break;
			case States.SDst:
				switch (_event)
				{
				case Events.open_curl_br:
					state = States.SDst1;
					break;
				}
				break;
			case States.SDst1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.STransition1;
					break;
				case Events.id:
					state = States.Dst_ID;
					break;
				}
				break;
			case States.Dst_ID:
				switch (_event)
				{
				case Events.number:
					state = States.Dst_ID1;
					SetTransEnd();
					break;
				}
				break;
			case States.Dst_ID1:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.STransition1;
					break;
				}
				break;
			case States.state58:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.STransition1;
					break;
				}
				break;
			case States.SArraySize:
				switch (_event)
				{
				case Events.size:
					state = States.SArraySize1;
					break;
				}
				break;
			case States.SArraySize1:
				switch (_event)
				{
				case Events.quote:
					state = States.SArraySize2;
					break;
				}
				break;
			case States.SArraySize2:
				switch (_event)
				{
				case Events.number:
					state = States.state67;
					SetArraySize();
					break;
				}
				break;
			case States.state67:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.state37;
					break;
				}
				break;
			case States.state70:
				switch (_event)
				{
				case Events.close_curl_br:
					state = States.state37;
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
				case "Stateflow":
					state = States.state1;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				case "open_curl_br":
					state = States.PrefixFound;
					break;
				default:
					state = States.state0;
					break;
				}
				break;
			case States.PrefixFound:
				switch (_event)
				{
				case "machine":
					state = States.Project;
					break;
				case "chart":
					state = States.Diagram;
					CreateNewFSM();
					break;
				case "state":
					state = States.SState;
					break;
				case "data":
					state = States.SVariable;
					ResetVariable();
					break;
				case "transition":
					state = States.STransition;
					CreateNewTransition();
					break;
				}
				break;
			case States.Project:
				switch (_event)
				{
				case "open_curl_br":
					state = States.Project1;
					break;
				}
				break;
			case States.Project1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.PrefixFound;
					break;
				}
				break;
			case States.Diagram:
				switch (_event)
				{
				case "open_curl_br":
					state = States.Diagram1;
					break;
				}
				break;
			case States.Diagram1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.PrefixFound;
					AddFSM();
					break;
				case "open_curl_br":
					state = States.NestedBrackets;
					break;
				default:
					dgr.ProcessEvent(_event);
					break;
				}
				break;
			case States.NestedBrackets:
				switch (_event)
				{
				case "close_curl_br":
					state = States.Diagram1;
					break;
				}
				break;
			case States.SState:
				switch (_event)
				{
				case "open_curl_br":
					state = States.SState1;
					CreateNewState();
					break;
				}
				break;
			case States.SState1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.PrefixFound;
					AddState();
					break;
				default:
					astate.ProcessEvent(_event);
					break;
				}
				break;
			case States.SVariable:
				switch (_event)
				{
				case "open_curl_br":
					state = States.SVariable1;
					break;
				}
				break;
			case States.SVariable1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.PrefixFound;
					AddVariable();
					break;
				case "props":
					state = States.VarProps;
					break;
				default:
					avariable.ProcessEvent(_event);
					break;
				}
				break;
			case States.STransition:
				switch (_event)
				{
				case "open_curl_br":
					state = States.STransition1;
					break;
				}
				break;
			case States.STransition1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.PrefixFound;
					AddTransition();
					break;
				case "src":
					state = States.SSrc;
					break;
				case "dst":
					state = States.SDst;
					break;
				case "open_curl_br":
					state = States.state58;
					break;
				default:
					atransition.ProcessEvent(_event);
					break;
				}
				break;
			case States.VarProps:
				switch (_event)
				{
				case "open_curl_br":
					state = States.state37;
					break;
				}
				break;
			case States.SArray:
				switch (_event)
				{
				case "open_curl_br":
					state = States.SArraySize;
					break;
				}
				break;
			case States.state37:
				switch (_event)
				{
				case "close_curl_br":
					state = States.SVariable1;
					break;
				case "array":
					state = States.SArray;
					break;
				case "open_curl_br":
					state = States.state70;
					break;
				}
				break;
			case States.SSrc:
				switch (_event)
				{
				case "open_curl_br":
					state = States.SSrc1;
					break;
				}
				break;
			case States.SSrc1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.STransition1;
					break;
				case "id":
					state = States.Src_ID;
					break;
				}
				break;
			case States.Src_ID:
				switch (_event)
				{
				case "number":
					state = States.Src_ID1;
					SetTransBegin();
					break;
				}
				break;
			case States.Src_ID1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.STransition1;
					break;
				}
				break;
			case States.SDst:
				switch (_event)
				{
				case "open_curl_br":
					state = States.SDst1;
					break;
				}
				break;
			case States.SDst1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.STransition1;
					break;
				case "id":
					state = States.Dst_ID;
					break;
				}
				break;
			case States.Dst_ID:
				switch (_event)
				{
				case "number":
					state = States.Dst_ID1;
					SetTransEnd();
					break;
				}
				break;
			case States.Dst_ID1:
				switch (_event)
				{
				case "close_curl_br":
					state = States.STransition1;
					break;
				}
				break;
			case States.state58:
				switch (_event)
				{
				case "close_curl_br":
					state = States.STransition1;
					break;
				}
				break;
			case States.SArraySize:
				switch (_event)
				{
				case "size":
					state = States.SArraySize1;
					break;
				}
				break;
			case States.SArraySize1:
				switch (_event)
				{
				case "quote":
					state = States.SArraySize2;
					break;
				}
				break;
			case States.SArraySize2:
				switch (_event)
				{
				case "number":
					state = States.state67;
					SetArraySize();
					break;
				}
				break;
			case States.state67:
				switch (_event)
				{
				case "close_curl_br":
					state = States.state37;
					break;
				}
				break;
			case States.state70:
				switch (_event)
				{
				case "close_curl_br":
					state = States.state37;
					break;
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
