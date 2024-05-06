using System;
namespace StateFlow.SFDParser
{
	public class AChart : IStateMachine
	{
		public AChart(Parser ow)
		{
			state = States.state0;
            owner = ow;
		}

        private Parser owner;
        public int ID { get; private set; }
        private string curText;
		
		/// <summary>
		/// </summary>
		private void SetID()
		{
            ID = int.Parse(owner.Owner.CurText);
		}
		
		/// <summary>
		/// </summary>
		private void SetName()
		{
            owner.CurMachine.Name = curText;
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
			ID,
			SName,
			SText,
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
					state = States.ID;
					break;
				case Events.name:
					state = States.SName;
					break;
				}
				break;
			case States.ID:
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
					state = States.SText;
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
					state = States.ID;
					break;
				case "name":
					state = States.SName;
					break;
				}
				break;
			case States.ID:
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
					state = States.SText;
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
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
