using System;
namespace StateFlow.SFDParser
{
	public class AState : IStateMachine
	{
		public AState(Parser ow)
		{
            owner = ow;
            Reset();
		}

        private Parser owner;
        private string curText;
        public int StatemachineID { get; private set; }
		
		/// <summary>
		/// </summary>
		private void SetID()
		{
            owner.CurState.ID.Value = long.Parse(owner.Owner.CurText);
		}
		
		/// <summary>
		/// </summary>
		private void SetName()
		{
            owner.CurState.Name = curText;
		}
		
		/// <summary>
		/// </summary>
		private void AddToken()
		{
            curText += owner.Owner.CurText + " ";
		}
		
		/// <summary>
		/// </summary>
		private void ClearText()
		{
            curText = "";
		}
		
		/// <summary>
		/// </summary>
		private void SetFSMID()
		{
            StatemachineID = int.Parse(owner.Owner.CurText);
		}
#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			state0,
			SName,
			SText,
			state5,
			state10,
		}
		private States state;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.labelString:
					state = States.SName;
					break;
				case Events.id:
					state = States.state5;
					break;
				case Events.chart:
					state = States.state10;
					break;
				}
				break;
			case States.SName:
				switch (_event)
				{
				case Events.quote:
					state = States.SText;
					ClearText();
					break;
				}
				break;
			case States.SText:
				switch (_event)
				{
				case Events.quote:
					state = States.state0;
					SetName();
					break;
				default:
					state = States.SText;
					AddToken();
					break;
				}
				break;
			case States.state5:
				switch (_event)
				{
				case Events.number:
					state = States.state0;
					SetID();
					break;
				}
				break;
			case States.state10:
				switch (_event)
				{
				case Events.number:
					state = States.state0;
					SetFSMID();
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
				case "labelString":
					state = States.SName;
					break;
				case "id":
					state = States.state5;
					break;
				case "chart":
					state = States.state10;
					break;
				}
				break;
			case States.SName:
				switch (_event)
				{
				case "quote":
					state = States.SText;
					ClearText();
					break;
				}
				break;
			case States.SText:
				switch (_event)
				{
				case "quote":
					state = States.state0;
					SetName();
					break;
				default:
					state = States.SText;
					AddToken();
					break;
				}
				break;
			case States.state5:
				switch (_event)
				{
				case "number":
					state = States.state0;
					SetID();
					break;
				}
				break;
			case States.state10:
				switch (_event)
				{
				case "number":
					state = States.state0;
					SetFSMID();
					break;
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~

        public void Reset()
        {
            state = States.state0;
            ClearText();
            StatemachineID = -1;
        }
	}
}
