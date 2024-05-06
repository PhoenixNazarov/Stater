using System;
using System.Linq;
using PluginData;

namespace FBDK.FBParser
{
	public class ABasic : IStateMachine
	{
		public ABasic()
		{
			state = States.state0;
		}

        public AParser Owner { get; set; }
	    private PluginData.State curState;
	    private PluginData.Transition curTransition;
		
		/// <summary>
		/// </summary>
		private void TestComment()
		{
            Owner.Agent.TestInitComment();
		}
		
		/// <summary>
		/// </summary>
		private void CreateState()
		{
            curState = new State();
            curState.ID = new UID(cnt);
		}
		
		/// <summary>
		/// </summary>
		private void CreateTransition()
		{
            curTransition = new Transition();
            curTransition.ID = new UID(cnt);
		}
		
		/// <summary>
		/// </summary>
		private void SetName()
		{
		    curState.Name = Owner.Agent.CurValue;
		}
		
		/// <summary>
		/// </summary>
		private void SetStateComment()
		{
            //curState.
		}
		
		/// <summary>
		/// </summary>
		private void SetInitialState()
		{
            curState.Type = State.StateType.Start;
		    Owner.Fsm.StartState = curState;
		}
		
		/// <summary>
		/// </summary>
		private void SetSource()
		{
		    foreach (var s in Owner.Fsm.States.Where(s => s.Name == Owner.Agent.CurValue))
		    {
		        curTransition.Start = s;
                s.Outgoing.Add(curTransition);
		    }
		}

	    /// <summary>
		/// </summary>
		private void SetDst()
		{
	        foreach (var s in Owner.Fsm.States.Where(s => s.Name == Owner.Agent.CurValue))
	        {
	            curTransition.End = s;
                s.Incoming.Add(curTransition);
	        }
		}
		
		/// <summary>
		/// </summary>
		private void SetCondition()
		{
		    Event e;
		    string guard;
            Agent.ParseCondition(Owner.Agent.CurValue, out e, out guard);
		    curTransition.Guard = guard;
		}


		
		/// <summary>
		/// </summary>
		private void ApplyState()
		{
            Owner.Fsm.States.Add(curState);
		    if (curState.Type == State.StateType.Start)
		    {
		        Owner.Fsm.StartState = curState;
		    }
		}
		
		/// <summary>
		/// </summary>
		private void ApplyTransition()
		{
            Owner.Fsm.Transitions.Add(curTransition);
		}
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			InsideBasicFB,
			InsideECC,
			InsideECState,
			InsideAction,
			InsideECTransition,
		}
		private States state;
		private byte cnt = 0;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.BasicFB:
					state = States.InsideBasicFB;
					break;
				}
				break;
			case States.InsideBasicFB:
				switch (_event)
				{
				case Events.ECC:
					state = States.InsideECC;
					break;
				}
				break;
			case States.InsideECC:
				switch (_event)
				{
				case Events.ECC:
					state = States.InsideBasicFB;
					break;
				case Events.ECState:
					state = States.InsideECState;
					cnt++;

					CreateState();
					break;
				case Events.ECTransition:
					state = States.InsideECTransition;
					cnt++;

					CreateTransition();
					break;
				}
				break;
			case States.InsideECState:
				switch (_event)
				{
				case Events.ECState:
					state = States.InsideECC;
					ApplyState();
					break;
				case Events.Name:
					state = States.InsideECState;
					SetName();
					break;
				case Events.Comment:
					state = States.InsideECState;
					TestComment();
					SetStateComment();
					break;
				case Events.InitialState:
					state = States.InsideECState;
					SetInitialState();
					break;
				case Events.ECAction:
					state = States.InsideAction;
					break;
				}
				break;
			case States.InsideAction:
				switch (_event)
				{
				case Events.ECAction:
					state = States.InsideECState;
					break;
				case Events.Algorithm:
					state = States.InsideAction;
					break;
				case Events.Output:
					state = States.InsideAction;
					break;
				}
				break;
			case States.InsideECTransition:
				switch (_event)
				{
				case Events.ECTransition:
					state = States.InsideECC;
					ApplyTransition();
					break;
				case Events.Source:
					state = States.InsideECTransition;
					SetSource();
					break;
				case Events.Destination:
					state = States.InsideECTransition;
					SetDst();
					break;
				case Events.Condition:
					state = States.InsideECTransition;
					SetCondition();
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
				case "BasicFB":
					state = States.InsideBasicFB;
					break;
				}
				break;
			case States.InsideBasicFB:
				switch (_event)
				{
				case "ECC":
					state = States.InsideECC;
					break;
				}
				break;
			case States.InsideECC:
				switch (_event)
				{
				case "ECC":
					state = States.InsideBasicFB;
					break;
				case "ECState":
					state = States.InsideECState;
					cnt++;

					CreateState();
					break;
				case "ECTransition":
					state = States.InsideECTransition;
					cnt++;

					CreateTransition();
					break;
				}
				break;
			case States.InsideECState:
				switch (_event)
				{
				case "ECState":
					state = States.InsideECC;
					ApplyState();
					break;
				case "Name":
					state = States.InsideECState;
					SetName();
					break;
				case "Comment":
					state = States.InsideECState;
					TestComment();
					SetStateComment();
					break;
				case "InitialState":
					state = States.InsideECState;
					SetInitialState();
					break;
				case "ECAction":
					state = States.InsideAction;
					break;
				}
				break;
			case States.InsideAction:
				switch (_event)
				{
				case "ECAction":
					state = States.InsideECState;
					break;
				case "Algorithm":
					state = States.InsideAction;
					break;
				case "Output":
					state = States.InsideAction;
					break;
				}
				break;
			case States.InsideECTransition:
				switch (_event)
				{
				case "ECTransition":
					state = States.InsideECC;
					ApplyTransition();
					break;
				case "Source":
					state = States.InsideECTransition;
					SetSource();
					break;
				case "Destination":
					state = States.InsideECTransition;
					SetDst();
					break;
				case "Condition":
					state = States.InsideECTransition;
					SetCondition();
					break;
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
