using System;
namespace StateFlow.SFDParser
{
	public class AVariable : IStateMachine
	{
        public AVariable(Parser ow)
		{
            owner = ow;
            Reset();
		}

        public void Reset()
        {
            state = States.state0;
        }

        private Parser owner;
        private string curText;

		
		/// <summary>
		/// </summary>
		private void SetID()
		{
            owner.CurVariableId = int.Parse(owner.Owner.CurText);
		}
		
		/// <summary>
		/// </summary>
		private void SetName()
		{
		    owner.CurVariableName = curText;
		}
		
		/// <summary>
		/// </summary>
		private void ClearText()
		{
		    curText = "";
		}
		
		/// <summary>
		/// </summary>
		private void AddToken()
		{
		    curText += owner.Owner.CurText + " ";
		}
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			state1,
			SName,
			SName1,
			Scope,
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
					state = States.state1;
					break;
				case Events.name:
					state = States.SName;
					ClearText();
					break;
				case Events.scope:
					state = States.Scope;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				case Events.number:
					state = States.state0;
					SetID();
					break;
				}
				break;
			case States.SName:
				switch (_event)
				{
				case Events.quote:
					state = States.SName1;
					break;
				}
				break;
			case States.SName1:
				switch (_event)
				{
				case Events.quote:
					state = States.state0;
					SetName();
					break;
				default:
					state = States.SName1;
					AddToken();
					break;
				}
				break;
			case States.Scope:
				switch (_event)
				{
				case Events.INPUT_DATA:
					state = States.state0;
					break;
				case Events.OUTPUT_DATA:
					state = States.state0;
					break;
				case Events.LOCAL_DATA:
					state = States.state0;
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
					state = States.state1;
					break;
				case "name":
					state = States.SName;
					ClearText();
					break;
				case "scope":
					state = States.Scope;
					break;
				}
				break;
			case States.state1:
				switch (_event)
				{
				case "number":
					state = States.state0;
					SetID();
					break;
				}
				break;
			case States.SName:
				switch (_event)
				{
				case "quote":
					state = States.SName1;
					break;
				}
				break;
			case States.SName1:
				switch (_event)
				{
				case "quote":
					state = States.state0;
					SetName();
					break;
				default:
					state = States.SName1;
					AddToken();
					break;
				}
				break;
			case States.Scope:
				switch (_event)
				{
				case "INPUT_DATA":
					state = States.state0;
					break;
				case "OUTPUT_DATA":
					state = States.state0;
					break;
				case "LOCAL_DATA":
					state = States.state0;
					break;
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
