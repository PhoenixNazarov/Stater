using System;
using PluginData;

namespace FBDK.FBParser
{
	public class AParser : IStateMachine
	{
		public AParser()
		{
			state = States.state0;
            basic = new ABasic {Owner = this};
		}

        public Agent Agent { get; set; }

	    private PluginData.StateMachine fsm;
        public PluginData.StateMachine Fsm
        {
            get { return fsm; }
        }
		
		/// <summary>
		/// </summary>
		private void TestComment()
		{
            Agent.TestFBTypeComment();
		}
		
		/// <summary>
		/// </summary>
		private void CreateProject()
		{
		}
		
		/// <summary>
		/// </summary>
		private void CreateFSM()
		{
            fsm = new StateMachine();
		}
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			state1,
			CompositeFB,
			BasicFB,
		}
		public AComposite composite { get; set; }
		public ABasic basic { get; set; }
		private States state;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.FBType:
					state = States.state1;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				case Events.Comment:
					state = States.state1;
					TestComment();
					break;
				case Events.Name:
					state = States.state1;
					break;
				case Events.Composite:
					state = States.CompositeFB;
					CreateProject();
					break;
				case Events.Basic:
					state = States.BasicFB;
					CreateFSM();
					break;
				}
				break;
			case States.CompositeFB:
				switch (_event)
				{
				default:
					composite.ProcessEvent(_event);
					break;
				}
				break;
			case States.BasicFB:
				switch (_event)
				{
				default:
					basic.ProcessEvent(_event);
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
				case "FBType":
					state = States.state1;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				case "Comment":
					state = States.state1;
					TestComment();
					break;
				case "Name":
					state = States.state1;
					break;
				case "Composite":
					state = States.CompositeFB;
					CreateProject();
					break;
				case "Basic":
					state = States.BasicFB;
					CreateFSM();
					break;
				}
				break;
			case States.CompositeFB:
				switch (_event)
				{
				default:
					composite.ProcessEvent(_event);
					break;
				}
				break;
			case States.BasicFB:
				switch (_event)
				{
				default:
					basic.ProcessEvent(_event);
					break;
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
