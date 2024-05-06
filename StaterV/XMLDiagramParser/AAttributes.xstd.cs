namespace XMLDiagramParser
{
	public abstract class AAttributes
	{
		public enum States
		{
			state0,
			Name,
			Type,
			Outgoing,
			Incoming,
			Event,
			Actions,
			Nested,
			Execution,
			Effect,
			Code,
			Guard,
			EntryActions,
		}
		public enum Transitions
		{
			transition1,
			transition4,
			transition8,
			transition13,
			transition15,
			transition20,
			transition25,
			transition31,
			transition36,
			transition41,
			transition51,
			transition55,
			transition59,
			transition6,
			transition11,
			transition13_1,
			transition18,
			transition23,
			transition29,
			transition34,
			transition39,
			transition49,
			transition53,
			transition57,
			transition4_1,
			transition8_1,
			transition15_1,
			transition16,
			transition20_1,
			transition21,
			transition25_1,
			transition26,
			transition27,
			transition31_1,
			transition32,
			transition36_1,
			transition37,
			transition41_1,
			transition42,
			transition43,
			transition44,
			transition45,
			transition46,
			transition51_1,
			transition55_1,
			transition59_1,
			transition60,
			transition61,
		}
		private States state;
		public AAttributes()
		{
			state = States.state0;
		}
		/// <summary>
		///
		/// </summary>
		public abstract void CreateAction();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateNestedMachine();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateExecution();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateEffect();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateEntryAction();
		/// <summary>
		///
		/// </summary>
		public abstract void SetName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetType();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateOutTransition();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateInTransition();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEventName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEventComment();
		/// <summary>
		///
		/// </summary>
		public abstract void SetActionSynchro();
		/// <summary>
		///
		/// </summary>
		public abstract void SetActionComment();
		/// <summary>
		///
		/// </summary>
		public abstract void SetActionName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetNestedName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetNestedType();
		/// <summary>
		///
		/// </summary>
		public abstract void SetExecutionType();
		/// <summary>
		///
		/// </summary>
		public abstract void SetExecutionName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEffectType();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEffectName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEffectEvent();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEffectSynchro();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEffectDescription();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEffectET();
		/// <summary>
		///
		/// </summary>
		public abstract void SetCode();
		/// <summary>
		///
		/// </summary>
		public abstract void CreateGuard();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEntryActionName();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEntryActionComment();
		/// <summary>
		///
		/// </summary>
		public abstract void SetEntryActionSynchro();
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.state0:
				switch (_event)
				{
				case Events.name:
					state = States.Name;
					break;
				case Events.type:
					state = States.Type;
					break;
				case Events.incoming:
					state = States.Incoming;
					break;
				case Events.outgoing:
					state = States.Outgoing;
					break;
				case Events._event:
					state = States.Event;
					break;
				case Events.action:
					state = States.Actions;
					CreateAction();
					break;
				case Events.nested:
					state = States.Nested;
					CreateNestedMachine();
					break;
				case Events.execution:
					state = States.Execution;
					CreateExecution();
					break;
				case Events.effect:
					state = States.Effect;
					CreateEffect();
					break;
				case Events.code:
					state = States.Code;
					break;
				case Events.guard:
					state = States.Guard;
					break;
				case Events.EntryAction:
					state = States.EntryActions;
					CreateEntryAction();
					break;
				}
				break;
			case States.Name:
				switch (_event)
				{
				case Events.name:
					state = States.state0;
					break;
				case Events.value:
					state = States.Name;
					SetName();
					break;
				}
				break;
			case States.Type:
				switch (_event)
				{
				case Events.type:
					state = States.state0;
					break;
				case Events.value:
					state = States.Type;
					SetType();
					break;
				}
				break;
			case States.Outgoing:
				switch (_event)
				{
				case Events.outgoing:
					state = States.state0;
					break;
				case Events.id:
					state = States.Outgoing;
					CreateOutTransition();
					break;
				}
				break;
			case States.Incoming:
				switch (_event)
				{
				case Events.incoming:
					state = States.state0;
					break;
				case Events.id:
					state = States.Incoming;
					CreateInTransition();
					break;
				}
				break;
			case States.Event:
				switch (_event)
				{
				case Events._event:
					state = States.state0;
					break;
				case Events.name:
					state = States.Event;
					SetEventName();
					break;
				case Events.comment:
					state = States.Event;
					SetEventComment();
					break;
				}
				break;
			case States.Actions:
				switch (_event)
				{
				case Events.action:
					state = States.state0;
					break;
				case Events.synchro:
					state = States.Actions;
					SetActionSynchro();
					break;
				case Events.comment:
					state = States.Actions;
					SetActionComment();
					break;
				case Events.name:
					state = States.Actions;
					SetActionName();
					break;
				}
				break;
			case States.Nested:
				switch (_event)
				{
				case Events.nested:
					state = States.state0;
					break;
				case Events.name:
					state = States.Nested;
					SetNestedName();
					break;
				case Events.type:
					state = States.Nested;
					SetNestedType();
					break;
				}
				break;
			case States.Execution:
				switch (_event)
				{
				case Events.execution:
					state = States.state0;
					break;
				case Events.type:
					state = States.Execution;
					SetExecutionType();
					break;
				case Events.name:
					state = States.Execution;
					SetExecutionName();
					break;
				}
				break;
			case States.Effect:
				switch (_event)
				{
				case Events.effect:
					state = States.state0;
					break;
				case Events.type:
					state = States.Effect;
					SetEffectType();
					break;
				case Events.name:
					state = States.Effect;
					SetEffectName();
					break;
				case Events._event:
					state = States.Effect;
					SetEffectEvent();
					break;
				case Events.synchro:
					state = States.Effect;
					SetEffectSynchro();
					break;
				case Events.descritpion:
					state = States.Effect;
					SetEffectDescription();
					break;
				case Events.effect_type:
					state = States.Effect;
					SetEffectET();
					break;
				}
				break;
			case States.Code:
				switch (_event)
				{
				case Events.code:
					state = States.state0;
					break;
				case Events.value:
					state = States.Code;
					SetCode();
					break;
				}
				break;
			case States.Guard:
				switch (_event)
				{
				case Events.guard:
					state = States.state0;
					break;
				case Events.value:
					state = States.Guard;
					CreateGuard();
					break;
				}
				break;
			case States.EntryActions:
				switch (_event)
				{
				case Events.EntryAction:
					state = States.state0;
					break;
				case Events.name:
					state = States.EntryActions;
					SetEntryActionName();
					break;
				case Events.comment:
					state = States.EntryActions;
					SetEntryActionComment();
					break;
				case Events.synchro:
					state = States.EntryActions;
					SetEntryActionSynchro();
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
				case "name":
					state = States.Name;
					break;
				case "type":
					state = States.Type;
					break;
				case "incoming":
					state = States.Incoming;
					break;
				case "outgoing":
					state = States.Outgoing;
					break;
				case "event":
					state = States.Event;
					break;
				case "action":
					state = States.Actions;
					CreateAction();
					break;
				case "nested":
					state = States.Nested;
					CreateNestedMachine();
					break;
				case "execution":
					state = States.Execution;
					CreateExecution();
					break;
				case "effect":
					state = States.Effect;
					CreateEffect();
					break;
				case "code":
					state = States.Code;
					break;
				case "guard":
					state = States.Guard;
					break;
				case "EntryAction":
					state = States.EntryActions;
					CreateEntryAction();
					break;
				}
				break;
			case States.Name:
				switch (_event)
				{
				case "name":
					state = States.state0;
					break;
				case "value":
					state = States.Name;
					SetName();
					break;
				}
				break;
			case States.Type:
				switch (_event)
				{
				case "type":
					state = States.state0;
					break;
				case "value":
					state = States.Type;
					SetType();
					break;
				}
				break;
			case States.Outgoing:
				switch (_event)
				{
				case "outgoing":
					state = States.state0;
					break;
				case "id":
					state = States.Outgoing;
					CreateOutTransition();
					break;
				}
				break;
			case States.Incoming:
				switch (_event)
				{
				case "incoming":
					state = States.state0;
					break;
				case "id":
					state = States.Incoming;
					CreateInTransition();
					break;
				}
				break;
			case States.Event:
				switch (_event)
				{
				case "event":
					state = States.state0;
					break;
				case "name":
					state = States.Event;
					SetEventName();
					break;
				case "comment":
					state = States.Event;
					SetEventComment();
					break;
				}
				break;
			case States.Actions:
				switch (_event)
				{
				case "action":
					state = States.state0;
					break;
				case "synchro":
					state = States.Actions;
					SetActionSynchro();
					break;
				case "comment":
					state = States.Actions;
					SetActionComment();
					break;
				case "name":
					state = States.Actions;
					SetActionName();
					break;
				}
				break;
			case States.Nested:
				switch (_event)
				{
				case "nested":
					state = States.state0;
					break;
				case "name":
					state = States.Nested;
					SetNestedName();
					break;
				case "type":
					state = States.Nested;
					SetNestedType();
					break;
				}
				break;
			case States.Execution:
				switch (_event)
				{
				case "execution":
					state = States.state0;
					break;
				case "type":
					state = States.Execution;
					SetExecutionType();
					break;
				case "name":
					state = States.Execution;
					SetExecutionName();
					break;
				}
				break;
			case States.Effect:
				switch (_event)
				{
				case "effect":
					state = States.state0;
					break;
				case "type":
					state = States.Effect;
					SetEffectType();
					break;
				case "name":
					state = States.Effect;
					SetEffectName();
					break;
				case "event":
					state = States.Effect;
					SetEffectEvent();
					break;
				case "synchro":
					state = States.Effect;
					SetEffectSynchro();
					break;
				case "descritpion":
					state = States.Effect;
					SetEffectDescription();
					break;
				case "effect_type":
					state = States.Effect;
					SetEffectET();
					break;
				}
				break;
			case States.Code:
				switch (_event)
				{
				case "code":
					state = States.state0;
					break;
				case "value":
					state = States.Code;
					SetCode();
					break;
				}
				break;
			case States.Guard:
				switch (_event)
				{
				case "guard":
					state = States.state0;
					break;
				case "value":
					state = States.Guard;
					CreateGuard();
					break;
				}
				break;
			case States.EntryActions:
				switch (_event)
				{
				case "EntryAction":
					state = States.state0;
					break;
				case "name":
					state = States.EntryActions;
					SetEntryActionName();
					break;
				case "comment":
					state = States.EntryActions;
					SetEntryActionComment();
					break;
				case "synchro":
					state = States.EntryActions;
					SetEntryActionSynchro();
					break;
				}
				break;
			}
		}
	}
}
