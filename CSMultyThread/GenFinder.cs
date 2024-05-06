namespace CSMultyThread
{
	public class GenFinder
	{
		public GenFinder()
		{
			state = States.UserIncludes;
		}

        public CodeGenerator Owner;

        /// <summary>
		/// </summary>
		public void WriteCurLine()
		{
            Owner.WriteCurLine();
        }
		/// <summary>
		/// </summary>
		public void WriteBuffer()
		{
            Owner.WriteBuffer();
        }
		/// <summary>
		/// </summary>
		public void InsertNamespace()
		{
            Owner.InsertNamespace();
        }
		/// <summary>
		/// </summary>
		public void WriteGeneratedFunctions()
		{
            Owner.WriteGeneratedFunctions();
		}
		/// <summary>
		/// </summary>
		public void RegisterAction()
		{
            Owner.RegisterAction();
		}
		/// <summary>
		/// </summary>
		public void WriteRemainActions()
		{
            Owner.WriteRemainActions();
		}
		/// <summary>
		/// </summary>
		public void CheckBrackets()
		{
		}
		/// <summary>
		/// </summary>
		public void WriteFunctions()
		{
		}

        /// <summary>
        /// 
        /// </summary>
        private void WriteNewFile()
        {
            Owner.WriteNewFile();
        }

#region ~~~~~~Generated functions~~~~~~
		public enum States
		{
			UserIncludes,
			NamespaceFound,
			UserDefinitions,
			GenFunctions,
			RemainUserLines,
			CheckBrackets,
			ImmediateClass,
			UserDefinitions2,
			Error,
			ClassFound,
			VoidFile,
		}
		private States state;
		public void ProcessEvent(Events _event)
		{
			switch (state)
			{
			case States.UserIncludes:
				switch (_event)
				{
				case Events.next_line:
					state = States.UserIncludes;
					WriteCurLine();
					break;
				case Events._namespace:
					state = States.NamespaceFound;
					WriteCurLine();
					WriteBuffer();
					break;
				case Events._class:
					state = States.ImmediateClass;
					InsertNamespace();
					WriteCurLine();
					WriteBuffer();
					break;
				case Events.eof:
					state = States.VoidFile;
					WriteNewFile();
					break;
				}
				break;
			case States.NamespaceFound:
				switch (_event)
				{
				case Events._class:
					state = States.ImmediateClass;
					WriteCurLine();
					WriteBuffer();
					break;
				case Events.next_line:
					state = States.UserDefinitions;
					WriteCurLine();
					break;
				}
				break;
			case States.UserDefinitions:
				switch (_event)
				{
				case Events.next_line:
					state = States.UserDefinitions;
					WriteCurLine();
					break;
				case Events.gen_functions:
					state = States.Error;
					break;
				case Events._class:
					state = States.ClassFound;
					WriteCurLine();
					WriteBuffer();
					break;
				}
				break;
			case States.GenFunctions:
				switch (_event)
				{
				case Events.next_line:
					state = States.GenFunctions;
					break;
				case Events.gen_functions:
					state = States.RemainUserLines;
					WriteGeneratedFunctions();
					break;
				}
				break;
			case States.RemainUserLines:
				switch (_event)
				{
				case Events.next_line:
					state = States.RemainUserLines;
					WriteCurLine();
					break;
				case Events.action_found:
					state = States.RemainUserLines;
					RegisterAction();
					WriteCurLine();
					break;
				case Events.eof:
					state = States.CheckBrackets;
					WriteRemainActions();
					CheckBrackets();
					WriteBuffer();
					break;
				}
				break;
			case States.CheckBrackets:
				switch (_event)
				{
				}
				break;
			case States.ImmediateClass:
				switch (_event)
				{
				case Events.eof:
					state = States.CheckBrackets;
					WriteGeneratedFunctions();
					WriteRemainActions();
					WriteBuffer();
					break;
				case Events.next_line:
					state = States.UserDefinitions2;
					WriteCurLine();
					break;
				case Events.action_found:
					state = States.UserDefinitions2;
					RegisterAction();
					WriteCurLine();
					break;
				}
				break;
			case States.UserDefinitions2:
				switch (_event)
				{
				case Events.gen_functions:
					state = States.GenFunctions;
					WriteFunctions();
					WriteBuffer();
					break;
				case Events.eof:
					state = States.CheckBrackets;
					WriteGeneratedFunctions();
					WriteRemainActions();
					WriteBuffer();
					break;
				case Events.next_line:
					state = States.UserDefinitions2;
					WriteCurLine();
					break;
				case Events.action_found:
					state = States.UserDefinitions2;
					RegisterAction();
					WriteCurLine();
					break;
				}
				break;
			case States.Error:
				switch (_event)
				{
				}
				break;
			case States.ClassFound:
				switch (_event)
				{
				case Events.next_line:
					state = States.UserDefinitions2;
					WriteCurLine();
					break;
				case Events.action_found:
					state = States.UserDefinitions2;
					RegisterAction();
					WriteCurLine();
					break;
				}
				break;
			case States.VoidFile:
				switch (_event)
				{
				}
				break;
			}
		}
		public void ProcessEventStr(string _event)
		{
			switch (state)
			{
			case States.UserIncludes:
				switch (_event)
				{
				case "next_line":
					state = States.UserIncludes;
					WriteCurLine();
					break;
				case "_namespace":
					state = States.NamespaceFound;
					WriteCurLine();
					WriteBuffer();
					break;
				case "_class":
					state = States.ImmediateClass;
					InsertNamespace();
					WriteCurLine();
					WriteBuffer();
					break;
				case "eof":
					state = States.VoidFile;
					WriteNewFile();
					break;
				}
				break;
			case States.NamespaceFound:
				switch (_event)
				{
				case "_class":
					state = States.ImmediateClass;
					WriteCurLine();
					WriteBuffer();
					break;
				case "next_line":
					state = States.UserDefinitions;
					WriteCurLine();
					break;
				}
				break;
			case States.UserDefinitions:
				switch (_event)
				{
				case "next_line":
					state = States.UserDefinitions;
					WriteCurLine();
					break;
				case "gen_functions":
					state = States.Error;
					break;
				case "_class":
					state = States.ClassFound;
					WriteCurLine();
					WriteBuffer();
					break;
				}
				break;
			case States.GenFunctions:
				switch (_event)
				{
				case "next_line":
					state = States.GenFunctions;
					break;
				case "gen_functions":
					state = States.RemainUserLines;
					WriteGeneratedFunctions();
					break;
				}
				break;
			case States.RemainUserLines:
				switch (_event)
				{
				case "next_line":
					state = States.RemainUserLines;
					WriteCurLine();
					break;
				case "action_found":
					state = States.RemainUserLines;
					RegisterAction();
					WriteCurLine();
					break;
				case "eof":
					state = States.CheckBrackets;
					WriteRemainActions();
					CheckBrackets();
					WriteBuffer();
					break;
				}
				break;
			case States.CheckBrackets:
				switch (_event)
				{
				}
				break;
			case States.ImmediateClass:
				switch (_event)
				{
				case "eof":
					state = States.CheckBrackets;
					WriteGeneratedFunctions();
					WriteRemainActions();
					WriteBuffer();
					break;
				case "next_line":
					state = States.UserDefinitions2;
					WriteCurLine();
					break;
				case "action_found":
					state = States.UserDefinitions2;
					RegisterAction();
					WriteCurLine();
					break;
				}
				break;
			case States.UserDefinitions2:
				switch (_event)
				{
				case "gen_functions":
					state = States.GenFunctions;
					WriteFunctions();
					WriteBuffer();
					break;
				case "eof":
					state = States.CheckBrackets;
					WriteGeneratedFunctions();
					WriteRemainActions();
					WriteBuffer();
					break;
				case "next_line":
					state = States.UserDefinitions2;
					WriteCurLine();
					break;
				case "action_found":
					state = States.UserDefinitions2;
					RegisterAction();
					WriteCurLine();
					break;
				}
				break;
			case States.Error:
				switch (_event)
				{
				}
				break;
			case States.ClassFound:
				switch (_event)
				{
				case "next_line":
					state = States.UserDefinitions2;
					WriteCurLine();
					break;
				case "action_found":
					state = States.UserDefinitions2;
					RegisterAction();
					WriteCurLine();
					break;
				}
				break;
			case States.VoidFile:
				switch (_event)
				{
				}
				break;
			}
		}
#endregion //~Generated functions~~~~~~
	}
}
