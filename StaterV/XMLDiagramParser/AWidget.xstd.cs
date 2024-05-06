namespace XMLDiagramParser
{
	public abstract class AWidget
	{
		public enum States
		{
			state0,
			Attributes,
		}
		public enum Transitions
		{
			transition1,
			transition14,
			transition15,
			transition16,
		}
		private States state;
		public AAttributes AttributesParser { get; set; }
		public AWidget()
		{
			state = States.state0;
		}
		/// <summary>
		///
		/// </summary>
		public abstract void SetWidgetId();
		/// <summary>
		///
		/// </summary>
		public abstract void ConstructWidget();
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.attributes:
					state = States.Attributes;
					break;
				case Events.id:
					state = States.state0;
					SetWidgetId();
					break;
				case Events.type:
					state = States.state0;
					ConstructWidget();
					break;
				}
				break;
			case States.Attributes:
				switch (_event)
				{
				case Events.attributes:
					state = States.state0;
					break;
				default:
					AttributesParser.ProcessEvent(_event);
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
				case "attributes":
					state = States.Attributes;
					break;
				case "id":
					state = States.state0;
					SetWidgetId();
					break;
				case "type":
					state = States.state0;
					ConstructWidget();
					break;
				}
				break;
			case States.Attributes:
				switch (_event)
				{
				case "attributes":
					state = States.state0;
					break;
				default:
					AttributesParser.ProcessEventStr(_event);
					break;
				}
				break;
			}
		}
	}
}
