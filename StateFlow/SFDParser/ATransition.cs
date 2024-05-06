using System;
using PluginData;

namespace StateFlow.SFDParser
{
	public class ATransition : IStateMachine
	{
		public ATransition(Parser ow)
		{
			state = States.state0;
		    owner = ow;
		}

	    private Parser owner;
	    private string name;
		
		/// <summary>
		/// </summary>
		private void SetID()
		{
            owner.CurTransition.ID = new UID(int.Parse(owner.Owner.CurText));
		}
		
		/// <summary>
		/// </summary>
		private void ClearText()
		{
		    name = "";
		}
		
		/// <summary>
		/// </summary>
		private void SetName()
		{
		    owner.CurTransition.Event.Name = name;
		}
		
		/// <summary>
		/// </summary>
		private void AddToken()
		{
		    name += owner.Owner.CurText;
		}
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			StateID,
			StartLabel,
			SLabel,
		}
		private States state;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.id:
					state = States.StateID;
					break;
				case Events.labelString:
					state = States.StartLabel;
					break;
				}
				break;
			case States.StateID:
				switch (_event)
				{
				case Events.number:
					state = States.state0;
					SetID();
					break;
				}
				break;
			case States.StartLabel:
				switch (_event)
				{
				case Events.quote:
					state = States.SLabel;
					ClearText();
					break;
				}
				break;
			case States.SLabel:
				switch (_event)
				{
				case Events.quote:
					state = States.state0;
					SetName();
					break;
				default:
					state = States.SLabel;
					AddToken();
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
				case "id":
					state = States.StateID;
					break;
				case "labelString":
					state = States.StartLabel;
					break;
				}
				break;
			case States.StateID:
				switch (_event)
				{
				case "number":
					state = States.state0;
					SetID();
					break;
				}
				break;
			case States.StartLabel:
				switch (_event)
				{
				case "quote":
					state = States.SLabel;
					ClearText();
					break;
				}
				break;
			case States.SLabel:
				switch (_event)
				{
				case "quote":
					state = States.state0;
					SetName();
					break;
				default:
					state = States.SLabel;
					AddToken();
					break;
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
