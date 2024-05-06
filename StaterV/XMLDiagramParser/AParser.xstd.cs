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
			Variables,
			Autoreject,
		}
		public enum Transitions
		{
			transition1,
			transition4,
			transition6,
			transition10,
			transition16,
			transition8,
			transition14,
			transition6_1,
			transition12,
			transition11,
			transition18,
			transition19,
			transition23,
			transition21,
			transition19_1,
			transition23_1,
		}
		private States state;
		public AEvent EventParse { get; set; }
		public AWidget WidgetParse { get; set; }
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
		/// <summary>
		///
		/// </summary>
		public abstract void CreateVariable();
		/// <summary>
		///
		/// </summary>
		public abstract void SetAutoReject();
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
				case Events.variable:
					state = States.Variables;
					break;
				case Events.autoreject:
					state = States.Autoreject;
					break;
				default:
					EventParse.ProcessEvent(_event);
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
					WidgetParse.ProcessEvent(_event);
					break;
				}
				break;
			case States.Variables:
				switch (_event)
				{
				case Events.variable:
					state = States.StatemachineData;
					break;
				case Events.decl:
					state = States.Variables;
					CreateVariable();
					break;
				}
				break;
			case States.Autoreject:
				switch (_event)
				{
				case Events.autoreject:
					state = States.StatemachineData;
					break;
				case Events.value:
					state = States.Autoreject;
					SetAutoReject();
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
				case "variable":
					state = States.Variables;
					break;
				case "autoreject":
					state = States.Autoreject;
					break;
				default:
					EventParse.ProcessEventStr(_event);
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
					WidgetParse.ProcessEventStr(_event);
					break;
				}
				break;
			case States.Variables:
				switch (_event)
				{
				case "variable":
					state = States.StatemachineData;
					break;
				case "decl":
					state = States.Variables;
					CreateVariable();
					break;
				}
				break;
			case States.Autoreject:
				switch (_event)
				{
				case "autoreject":
					state = States.StatemachineData;
					break;
				case "value":
					state = States.Autoreject;
					SetAutoReject();
					break;
				}
				break;
			}
		}
	}
}
