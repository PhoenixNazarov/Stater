namespace XMLDiagramParser
{
	public abstract class AParser
	{
		public enum States
		{
			state0,
			Diagram,
			Name,
			Data,
			StatemachineData,
			Widget,
		}
		public enum Transitions
		{
			transition3,
			transition4,
			transition5,
			transition6,
			transition8,
			transition9,
			transition11,
			transition12,
			transition14,
			transition15,
		}
		private States state;
		public AEvent StatemachineData_AEvent { get; set; }
		public AWidget Widget_AWidget { get; set; }
		public AParser()
		{
			state = States.state0;
		}
		/// <summary>
		///
		/// </summary>
		public abstract void CreateDiagram();
		/// <summary>
		///
		/// </summary>
		public abstract void SetDiagramName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetStatemachineType();
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.diagram:
					state = States.Diagram;
					CreateDiagram();
					break;
				}
				break;
			case States.Diagram:
				switch (_event)
				{
				case Events.name:
					state = States.Name;
					break;
				case Events.data:
					state = States.Data;
					break;
				case Events.widget:
					state = States.Widget;
					break;
				}
				break;
			case States.Name:
				switch (_event)
				{
				case Events.name:
					state = States.Diagram;
					break;
				case Events.value:
					state = States.Name;
					SetDiagramName();
					break;
				}
				break;
			case States.Data:
				switch (_event)
				{
				case Events.data:
					state = States.Diagram;
					break;
				case Events.Statemachine:
					state = States.StatemachineData;
					SetStatemachineType();
					break;
				}
				break;
			case States.StatemachineData:
				switch (_event)
				{
				case Events.Statemachine:
					state = States.Data;
					break;
				default:
					StatemachineData_AEvent.ProcessEvent(_event);
					break;
				}
				break;
			case States.Widget:
				switch (_event)
				{
				case Events.widget:
					state = States.Diagram;
					break;
				default:
					Widget_AWidget.ProcessEvent(_event);
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
				case "diagram":
					state = States.Diagram;
					CreateDiagram();
					break;
				}
				break;
			case States.Diagram:
				switch (_event)
				{
				case "name":
					state = States.Name;
					break;
				case "data":
					state = States.Data;
					break;
				case "widget":
					state = States.Widget;
					break;
				}
				break;
			case States.Name:
				switch (_event)
				{
				case "name":
					state = States.Diagram;
					break;
				case "value":
					state = States.Name;
					SetDiagramName();
					break;
				}
				break;
			case States.Data:
				switch (_event)
				{
				case "data":
					state = States.Diagram;
					break;
				case "Statemachine":
					state = States.StatemachineData;
					SetStatemachineType();
					break;
				}
				break;
			case States.StatemachineData:
				switch (_event)
				{
				case "Statemachine":
					state = States.Data;
					break;
				default:
					StatemachineData_AEvent.ProcessEventStr(_event);
					break;
				}
				break;
			case States.Widget:
				switch (_event)
				{
				case "widget":
					state = States.Diagram;
					break;
				default:
					Widget_AWidget.ProcessEventStr(_event);
					break;
				}
				break;
			}
		}
	}
}
