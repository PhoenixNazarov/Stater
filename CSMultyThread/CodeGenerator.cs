using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PluginData;
using Action=PluginData.Action;
using TextProcessor;

namespace CSMultyThread
{
    public class CodeGenerator
    {
        public CodeGenerator(IParams p)
        {
            param = p;
        }

        private readonly IParams param;
        public List<AutomatonExecution> AddedExecutions { get; set; }
        //public string Namespace { get; set; }
        public Options Options { get; set; }
        private Indent indent;

        #region Generator variables

        private StreamWriter sw;
        private List<string> newCodeBuffer;
        private List<string> newCode; 
        #endregion

        private const string statesEnumName = "States";
        //private const string transitionsEnumName = "Transitions";
        private const string eventsEnumName = "Events";
        private const string stateVariableName = "state";
        private const string eventArg = "_event";
        private const string rejectState = "reject";
        private const string rejectFunction = "Reject";


        public void Generate()
        {
            if ((param.WorkDirectory == null) || (param.Machines == null))
            {
                return;
            }

            Prepare();

            WriteInterface();

            if (Options.ThreadManager)
            {
                WriteThreadManager();
                WriteEventQueue();                
            }
            WriteEvents();
            WriteMachines();
        }

        private List<string> CreateStartNamespace()
        {
            var res = new List<string>();
            res.Add(indent.Do() + "namespace " + Options.Namespace);
            res.Add("{");
            ++indent;
            return res;
        }

        private void WriteStartNamespace()
        {
            if (Options.Namespace.Trim() != "")
            {
                /*
                sw.WriteLine(indent.Do() + "namespace " + Namespace);
                sw.WriteLine("{");
                ++indent;
                 * */
                var ns = CreateStartNamespace();
                foreach (var n in ns)
                {
                    sw.WriteLine(n);
                }
            }
        }

        private void WriteEndNamespace()
        {
            if (Options.Namespace.Trim() != "")
            {
                --indent;
                sw.WriteLine("}");
            }
        }

        private void WriteEventQueue()
        {
            sw = new StreamWriter(param.WorkDirectory + "\\" + "EventQueue.cs");
            indent = new Indent();

            sw.WriteLine(indent.Do() + "using System;");
            sw.WriteLine(indent.Do() + "using System.Collections.Generic;");
            sw.WriteLine(indent.Do() + "using System.Threading;");
            sw.WriteLine(indent.Do() + "");

            WriteStartNamespace();

            sw.WriteLine(indent.Do() + "class EventQueue");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "public EventQueue(Thread m, AutoResetEvent ar)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "machineThread = m;");
            sw.WriteLine(indent.Do() + "events = new Queue<string>();");
            sw.WriteLine(indent.Do() + "machineAr = ar;");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "private Thread machineThread;");
            sw.WriteLine(indent.Do() + "private Queue<string> events;");
            sw.WriteLine(indent.Do() + "private static object qLock = new object();");
            sw.WriteLine(indent.Do() + "private AutoResetEvent machineAr;");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "public void PushEvent(string evt)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "lock (this)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "events.Enqueue(evt);");
            sw.WriteLine(indent.Do() + "machineAr.Set();");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "public bool IsEmpty()");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "lock (this)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "return events.Count == 0;");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "public string PeekEvent()");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "lock (this)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "string res = events.Peek();");
            sw.WriteLine(indent.Do() + "return res;");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "public string PopEvent()");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "lock (this)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "string res = events.Peek();");
            sw.WriteLine(indent.Do() + "events.Dequeue();");
            sw.WriteLine(indent.Do() + "return res;");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");

            WriteEndNamespace();

            sw.Close();
        }

        private void WriteInterface()
        {
            sw = new StreamWriter(param.WorkDirectory + "\\" + "IStateMachine.cs");
            indent = new Indent();
            WriteStartNamespace();
            sw.WriteLine(indent.Do() + "public interface IStateMachine");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "void ProcessEvent(Events _event);");
            sw.WriteLine(indent.Do() + "void ProcessEvent(string _event);");

            if (Options.ThreadManager)
            {
                sw.WriteLine(indent.Do() + "");
                sw.WriteLine(indent.Do() + "ThreadManager Manager { get; set; }");
            } 
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "");

            --indent;
            WriteEndNamespace();
            sw.Close();
        }

        private void WriteThreadManager()
        {
            sw = new StreamWriter(param.WorkDirectory + "\\" + "ThreadManager.cs");
            indent = new Indent();

            sw.WriteLine(indent.Do() + "using System;");
            sw.WriteLine(indent.Do() + "using System.Collections.Generic;");
            sw.WriteLine(indent.Do() + "using System.Threading;");
            sw.WriteLine(indent.Do() + "using System.Collections.Concurrent;");
            WriteStartNamespace();

            sw.WriteLine(indent.Do() + "public class ThreadManager");
            sw.WriteLine(indent.Do() + "{");
            ++indent;

            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "class AParam");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "//public Thread thread;");
            sw.WriteLine(indent.Do() + "public AutoResetEvent ar;");
            sw.WriteLine(indent.Do() + "public EventQueue events;");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "/// <summary>");
            sw.WriteLine(indent.Do() + "/// Создаем два потока классов AStarter.");
            sw.WriteLine(indent.Do() + "/// </summary>");
            sw.WriteLine(indent.Do() + "public void StartWork()");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "Thread st1 = new Thread(st1Thread);");
            sw.WriteLine(indent.Do() + "AutoResetEvent sta1Ar = new AutoResetEvent(false);");
            sw.WriteLine(indent.Do() + "st1Events = new EventQueue(st1, sta1Ar);");
            sw.WriteLine(indent.Do() + "AParam st1p = new AParam { ar = sta1Ar, events = st1Events };");
            sw.WriteLine(indent.Do() + "st1.Start(st1p);");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "while (true)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "var s = Console.In.ReadLine();");
            sw.WriteLine(indent.Do() + "st1Events.PushEvent(s);");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "");
            sw.WriteLine(indent.Do() + "private EventQueue st1Events;");
            sw.WriteLine(indent.Do() + "public void st1Thread(Object param)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "AParam aparam = param as AParam;");
            sw.WriteLine(indent.Do() + "AStarter st1 = new AStarter();");
            sw.WriteLine(indent.Do() + "while (true)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "if (aparam.events.IsEmpty())");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "//Уснуть.");
            sw.WriteLine(indent.Do() + "Console.Out.WriteLine(\"Going to sleep!\");");
            sw.WriteLine(indent.Do() + "aparam.ar.WaitOne();");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            sw.WriteLine(indent.Do() + "string evt = aparam.events.PopEvent();");
            sw.WriteLine(indent.Do() + "Console.Out.WriteLine(\"event handled: {0}!\", evt);");
            sw.WriteLine(indent.Do() + "switch (evt)");
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "case \"e1\":");
            sw.WriteLine(indent.Do() + "Console.Out.WriteLine(\"Next state!\");");
            sw.WriteLine(indent.Do() + "break;");
            sw.WriteLine(indent.Do() + "default:");
            sw.WriteLine(indent.Do() + "Console.Out.WriteLine(\"Fail!\");");
            sw.WriteLine(indent.Do() + "break;");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");
            --indent;
            sw.WriteLine(indent.Do() + "}");

            --indent;
            sw.WriteLine("}");
            WriteEndNamespace();
            sw.Close();
        }

        private void WriteMachines()
        {
            foreach (var machine in param.Machines)
            {
                WriteOneMachine(machine);
            }
        }

        private string [] ReadMachine(StateMachine machine)
        {
            StreamReader sr = new StreamReader(param.WorkDirectory + "\\" + machine.Name + ".cs");
            string text = sr.ReadToEnd();
            sr.Close();
            text = text.Replace("\r\n", "\n");
            text = text.Replace("\n\r", "\n");
            text = text.Replace('\r', '\n');
            var res = text.Split('\n');

            //Выкинем лишнюю пустую строку, оставшуюся после Split.
            if (res[res.Length - 1].Trim() == "")
            {
                var res2 = new string[res.Length - 1];
                for (int i = 0; i < res.Length - 1; i++)
                {
                    res2[i] = res[i];
                }
                res = res2;
            }
            return res;
        }

        CodeProcessor processor = new CodeProcessor();

        private void WriteOneMachine(StateMachine machine)
        {
            string path = param.WorkDirectory + "\\" + machine.Name + ".cs";
            string[] oldFile = null;
            bool newFile = true;

            newCode = new List<string>();
            newCodeBuffer = new List<string>();

            if (File.Exists(path))
            {
                newFile = false;
            }

            if (Options.OverWrite)
            {
                newFile = true;
            }

            if (newFile)
            {
                sw = new StreamWriter(path);
            }
            else
            {
                oldFile = ReadMachine(machine);
                sw = new StreamWriter(path);
            }

            processor = new CodeProcessor();

            processor.IncludeMarkStart      = "#region ~~~~~~Generated usings~~~~~~";
            processor.IncludeMarkEnd        = "#endregion //~Generated usings~~~~~~";

            processor.FunctionsMarkStart    = "#region ~~~~~~Generated functions~~~~~~";
            processor.FunctionsMarkEnd      = "#endregion //~Generated functions~~~~~~";

            //processor.DefinitionsMarkStart  = "#region ~~~~~~Generated definitions~~~~~~";
            //processor.DefinitionsMarkEnd    = "#endregion //~Generated definitions~~~~~~";

            processor.AutomataClassDeclaration = new List<string>{"class", machine.Name};

            if (newFile)
            {
                WriteNewFile(machine);
            }

            else
            {
                WriteToOldFile(oldFile, machine);
            }

            //processor.Text = new string[oldFile.Length];
            //oldFile.CopyTo(processor.Text, 0);

            //var saved = processor.TextBeforeInclude();
            

            sw.Close();
        }

        private string curCodeLine;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">Содержимое файла</param>
        /// <param name="machine"></param>
        private void WriteToOldFile(string [] file, StateMachine machine)
        {
            processor.AutomataClassDeclaration = new List<string>();
            processor.AutomataClassDeclaration.Add("class");
            processor.AutomataClassDeclaration.Add(machine.Name);
            CreateActionSet(machine);
            processor.Actions = ExportActionSet();

            processor.Text = new string[file.Length];
            file.CopyTo(processor.Text, 0);

            GenFinder gf = new GenFinder();
            gf.Owner = this;

            foundActions = new HashSet<string>();

            curMachine = machine;

            for (int i = 0; i < file.Length; i++)
            {
                //Обработаем строку.
                curCodeLine = file[i];
                var state = processor.ProcessLine(curCodeLine);
                switch (state)
                {
                    case CodeProcessor.CodeState.PlainLine:
                        gf.ProcessEvent(Events.next_line);
                        break;
                    case CodeProcessor.CodeState.Namespace:
                        gf.ProcessEvent(Events._namespace);
                        break;
                    case CodeProcessor.CodeState.TargetClass:
                        gf.ProcessEvent(Events._class);
                        break;
                    case CodeProcessor.CodeState.GenFunctionsMarkStart:
                        gf.ProcessEvent(Events.gen_functions);
                        break;
                    case CodeProcessor.CodeState.GenFunctionsMarkEnd:
                        gf.ProcessEvent(Events.gen_functions);
                        break;
                    case CodeProcessor.CodeState.Action:
                        gf.ProcessEvent(Events.action_found);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            gf.ProcessEvent(Events.eof);

            WriteBuffer();

            foreach (var line in newCode)
            {
                sw.WriteLine(line);
            }
        }

        public void WriteCurLine()
        {
            newCodeBuffer.Add(curCodeLine);
        }

        public void WriteBuffer()
        {
            newCode.AddRange(newCodeBuffer);
            newCodeBuffer.Clear();
        }

        public void InsertNamespace()
        {
            var ns = CreateStartNamespace();
            newCode.AddRange(ns);
        }

        private StateMachine curMachine;
        public void WriteGeneratedFunctions()
        {
            indent = new Indent(2);
            newCodeBuffer.Add(processor.FunctionsMarkStart + "");

            if (Options.ThreadManager)
            {
                newCodeBuffer.AddRange(WriteManager());
            }

            //Состояния.
            newCodeBuffer.Add(indent.Do() + "public enum " + statesEnumName + "");
            newCodeBuffer.Add(indent.Do() + "{");
            ++indent;
            foreach (var state in curMachine.States)
            {
                newCodeBuffer.Add(indent.Do() + state.Name + ",");
            }
            //reject
            if (curMachine.AutoReject)
            {
                newCodeBuffer.Add(indent.Do() + rejectState + ",");
            }
            --indent;
            newCodeBuffer.Add(indent.Do() + "}");

            newCodeBuffer.AddRange(AddNestedMachines(curMachine));

            //Переменная, в которой хранится состояние.
            newCodeBuffer.Add(indent.Do() + "private " + statesEnumName + " state;");

            newCodeBuffer.AddRange(AddVariables());
            
            //Функция обработки событий.
            newCodeBuffer.AddRange(AddStateProcessor(curMachine, false));
            newCodeBuffer.AddRange(AddStateProcessor(curMachine, true));

            newCodeBuffer.Add(processor.FunctionsMarkEnd + "");
            
        }

        private void WriteVariables()
        {
            var list = AddVariables();
            foreach (var line in list)
            {
                sw.WriteLine(line);
            }
        }

        private List<string> AddVariables()
        {
            var res = new List<string>();
            foreach (var variable in curMachine.Variables)
            {
                StringBuilder sb = new StringBuilder("private ");
                switch (variable.Type)
                {
                    case Variable.TypeList.Bool:
                        sb.Append("bool ");
                        break;
                    case Variable.TypeList.Int8:
                        sb.Append("byte ");
                        break;
                    case Variable.TypeList.Int16:
                        sb.Append("Int16 ");
                        break;
                    case Variable.TypeList.Int32:
                        sb.Append("Int32 ");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (variable.GetType() == typeof(PluginData.SingleVariable))
                {
                    sb.Append(variable.Name);
                    sb.Append(" = ");
                    sb.Append(((SingleVariable) variable).Value);
                    sb.Append(";");
                }
                if (variable.GetType() == typeof(PluginData.Array))
                {
                    //int [] s = new int[4];
                    sb.Append("[] ");
                    sb.Append(variable.Name);
                    sb.Append(" = new ");
                    sb.Append(variable.Type);
                    sb.Append("[");
                    sb.Append(((PluginData.Array) variable).NElements);
                    sb.Append("];");
                }

                //newCodeBuffer.Add(indent.Do() + sb);
                res.Add(indent.Do() + sb);

                res.AddRange(AddVolatile(variable));
            }
            return res;
        }

        private IEnumerable<string> AddVolatile(Variable variable)
        {
            List<string> res = new List<string>();
            StringBuilder sb;
            if (variable.Volatile)
            {
                string vname = NameProperty(variable);
                sb = new StringBuilder(indent.Do());
                sb.Append("public ");
                sb.Append(variable.Type.ToString());
                //res.Add(indent.Do() + "public " + variable.Type + " ");// + " " + vname);
                if (variable.GetType() == typeof(PluginData.Array))
                {
                    sb.Append(" [] ");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append(vname);

                res.Add(sb.ToString());
                res.Add(indent.Do() + "{");
                ++indent;
                res.Add(indent.Do() + "get { return " + variable.Name + "; }");
                --indent;
                res.Add(indent.Do() + "}");
            }
            return res;
        }

        private string NameProperty(Variable variable)
        {
            string res = "";
            if (Tokenizer.IsSmallEngLetter(variable.Name[0]))
            {
                string tmp = new string(variable.Name[0], 1);

                res = tmp.ToUpper() + variable.Name.Substring(1);
            }
            else
            {
                res = "Get" + variable.Name;
            }
            return res;
        }

        private HashSet<string> foundActions = new HashSet<string>();
        public void RegisterAction()
        {
            foundActions.Add(processor.LastFoundAction);
        }

        public void WriteRemainActions()
        {
            List<Action> actions = new List<Action>(curMachine.GetAllActions());

            foreach (var action in actions)
            {
                if (!foundActions.Contains(action.Name))
                {
                    newCode.AddRange(AddAction(action));
                }
            }
            //Найти все оставшиеся выходные воздействия.

            //Вписать их.
        }

        public void WriteAction(Action action)
        {
            var lines = AddAction(action);

            foreach (var line in lines)
            {
                sw.WriteLine(line);
            }
        }

        public List<string> AddAction(Action action)
        {
            List<string> res = new List<string>();

            res.Add(indent.Do() + "");
            res.Add(indent.Do() + "/// <summary>");
            //res.Add(indent.Do() + "///" + action.Comment);
            res.Add(indent.Do() + "/// </summary>");
            res.Add(indent.Do() + "private void " + action.Name + "()");
            res.Add(indent.Do() + "{");
            res.Add(indent.Do() + "}");

            return res;
        }

        public void WriteFunctions()
        {
            
        }

        public void WriteNewFile()
        {
            WriteNewFile(curMachine);
        }

        private void WriteNewFile(StateMachine machine)
        {
            indent = new Indent();

            sw.WriteLine(indent.Do() + "using System;");

            WriteStartNamespace();

            sw.WriteLine(indent.Do() + "public class " + machine.Name + " : IStateMachine");
            sw.WriteLine(indent.Do() + "{");
            ++indent;

            curMachine = machine;


            WriteConstructor(machine);

            //Объявления выходных воздействий.
            WriteActionsDeclarations(machine);

            sw.WriteLine(processor.FunctionsMarkStart);

            //sw.Write(WriteManager());
            if (Options.ThreadManager)
            {
                var tm = WriteManager();
                foreach (var line in tm)
                {
                    sw.WriteLine(line);
                }
            }

            //Состояния.
            sw.WriteLine(indent.Do() + "public enum " + statesEnumName);
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            foreach (var state in machine.States)
            {
                sw.WriteLine(indent.Do() + state.Name + ",");
            }
            //reject
            if (curMachine.AutoReject)
            {
                sw.WriteLine(indent.Do() + rejectState + ",");
            }
            --indent;
            sw.WriteLine(indent.Do() + "}");

            WriteNestedMachines(machine);

            //Переменная, в которой хранится состояние.
            sw.WriteLine(indent.Do() + "private {0} state;", statesEnumName);
            //Функция обработки событий.

            WriteVariables();

            if (Options.WithEvents)
            {
                WriteStateProcessor(machine, false);
            }

            if (Options.WithStrings)
            {
                WriteStateProcessor(machine, true);
            }

            sw.WriteLine(processor.FunctionsMarkEnd);

            --indent;
            sw.WriteLine(indent.Do() + "}");

            WriteEndNamespace();
        }

        private void WriteStateProcessor(StateMachine machine, bool withStrings)
        {
            var sp = AddStateProcessor(machine, withStrings);
            foreach (var line in sp)
            {
                sw.WriteLine(line);
            }
        }

        private List<string> AddStateProcessor(StateMachine machine, bool withStrings)
        {
            var res = new List<string>();
            if (withStrings)
            {
                res.Add(string.Format(indent.Do() + "public void ProcessEvent(string {0})",
                    eventArg));
            }
            else
            {
                res.Add(string.Format(indent.Do() + "public void ProcessEvent(Events {0})",
                             eventArg));
            }
            res.Add(indent.Do() + "{");
            ++indent;

            //switch по состояниям.
            res.Add(string.Format(indent.Do() + "switch ({0})", stateVariableName));
            res.Add(indent.Do() + "{");
            foreach (var state in machine.States)
            {
                res.Add(string.Format(indent.Do() + "case {0}.{1}:",
                    statesEnumName, state.Name));
                ++indent;
                res.AddRange(WriteEventSwitch(state, withStrings));
                res.Add(indent.Do() + "break;");
                --indent;
            }
            res.Add(indent.Do() + "}");

            --indent;
            res.Add(indent.Do() + "}");
            return res;
        }

        private List<string> WriteEventSwitch(State state, bool withStrings)
        {
            List<string> res = new List<string>();
            //string processFunction = (withStrings) ? "ProcessEventStr" : "ProcessEvent";
            string processFunction = "ProcessEvent";

            res.Add(string.Format(indent.Do() + "switch ({0})", eventArg));
            res.Add(indent.Do() + "{");

            //Каждый элемент содержит список переходов по одному событию.
            Dictionary<string, List<PluginData.Transition> > sets = new Dictionary<string, List<Transition> >();
            List<Transition> anyEventList = new List<Transition>();
            foreach (var trans in state.Outgoing)
            {
                if (trans.Event.Name == "*")
                {
                    anyEventList.Add(trans);
                }
                else if (sets.ContainsKey(trans.Event.Name))
                {
                    sets[trans.Event.Name].Add(trans);
                }
                else
                {
                    sets.Add(trans.Event.Name, new List<Transition> {trans});
                }
            }

            if (!CheckConsistency(sets))
            {
                throw (new ArgumentException("Inconsistent conditions"));
            }

            foreach (var tl in sets)
            {
                if (withStrings)
                {
                    res.Add(string.Format(indent.Do() + "case \"{0}\":", tl.Key));
                }
                else
                {
                    res.Add(string.Format(indent.Do() + "case {0}.{1}:", eventsEnumName, tl.Key));
                }
                ++indent;

                res.AddRange(WriteTransitionList(state, tl.Key, tl.Value, withStrings));
            }


            if (anyEventList.Count != 0)
            {
                res.Add(indent.Do() + "default:");
                ++indent;
                res.AddRange(WriteTransitionList(state, "*", anyEventList, withStrings));
            }
            else
            {
                //Вложенный автомат.
                bool nested = false;
                if (state.NestedMachines != null)
                {
                    if (state.NestedMachines.Count > 0)
                    {
                        nested = true;
                        res.Add(indent.Do() + "default:");
                        ++indent;
                        foreach (var machine in state.NestedMachines)
                        {
                            //sw.WriteLine(indent.Do() + "{0}_{1}.{2}({3});", 
                            //    state.TheAttributes.Name, machine, processFunction, eventArg);
                            res.Add(string.Format(indent.Do() + "{0}.{1}({2});",
                                machine.Name, processFunction, eventArg));
                        }
                        res.Add(indent.Do() + "break;");
                        --indent;
                    }
                }

                if (!nested)
                {
                    if (curMachine.AutoReject)
                    {
                        res.Add(indent.Do() + "default:");
                        ++indent;
                        res.Add(string.Format(indent.Do() + "{0} = {1}.{2};",
                                     stateVariableName, statesEnumName, rejectState));
                        res.Add(indent.Do() + rejectFunction + "();");
                        res.Add(indent.Do() + "break;");
                        --indent;
                    }
                }

            }
            res.Add(indent.Do() + "}");
            return res;
        }

        private IEnumerable<string> WriteTransitionList(State state, string evt, List<Transition> tl, bool withStrings)
        {
            List<string> res = new List<string>();

            //Упорядочить так, чтобы в конце был переход без условий. 
            var transList = new List<Transition>(tl);
            var guarded = SortTransitions(ref transList);

            int count = 0;
            foreach (var trans in transList)
            {
                if (trans.GuardExists())
                {
                    string str = "";
                    if (count > 0)
                    {
                        str = "else ";
                    }
                    res.Add(indent.Do() + str + "if (" + trans.Guard + ")");
                    res.Add(indent.Do() + "{");
                    ++indent;
                    ++count;
                }
                else
                {
                    if (guarded)
                    {
                        res.Add(indent.Do() + "else");
                        res.Add(indent.Do() + "{");
                        ++indent;
                    }
                }


                //Сделать переход.
                State st = (State)trans.End;
                res.Add(string.Format(indent.Do() + "{0} = {1}.{2};",
                             stateVariableName, statesEnumName, st.Name));

                //Выходные воздействия при выходе из состояния.
                res.AddRange(WriteActions(state.ExitActions));

                //Код на переходе
                res.AddRange(WriteCode(trans.Code));

                //Выходные воздействия на переходе.
                res.AddRange(WriteActions(trans.Actions));

                State newState = trans.End;
                if (newState != null)
                {
                    //Выходные воздействия при входе в состояние.
                    res.AddRange(WriteActions(newState.EntryActions));

                    //Запуск автоматов при входе в состояние.
                    WriteExecutions(newState);
                }

                if (trans.GuardExists())
                {
                    --indent;
                    res.Add(indent.Do() + "}");
                }
                else
                {
                    if (guarded)
                    {
                        --indent;
                        res.Add(indent.Do() + "}");
                    }
                }
                //++count;
            }

            res.Add(indent.Do() + "break;");
            --indent;

            return res;
        }


        private bool SortTransitions(ref List<Transition> list)
        {
            int nNoGuard = 0;
            bool nGuard = false;
            int lim = list.Count;
            for (int i = 0; i < lim; ++i)
            {
                if (list[i].Guard.Trim() == "")
                {
                    ++nNoGuard;
                    --lim;
                    if (nNoGuard > 1)
                    {
                        throw new ArgumentException("Нельзя иметь два безусловных перехода под одному событию.");
                    }

                    //swap
                    var tmp = list[list.Count - 1];
                    list[list.Count - 1] = list[i];
                    list[i] = tmp;
                }
                else
                {
                    nGuard = true;
                }
            }
            return nGuard;
        }

        private IEnumerable<string> WriteExecutions(State state)
        {
            List<string> res = new List<string>();

            if (state.EntryExecutions == null)
            {
                return res;
            }

            foreach (var ex in state.EntryExecutions)
            {
                res.Add(indent.Do() + "Manager.StartNewThreadFSM(" + ex.Type + ", " + ex.Name + ");");
            }
            return res;
        }

        private IEnumerable<string> WriteManager()
        {
            List<string> res = new List<string>();
            res.Add(indent.Do() + "public ThreadManager Manager { get; set; }");
            return res;
        }

        private bool CheckConsistency(Dictionary<string, List<Transition>> transitionSets)
        {
            return true;
        }

        private IEnumerable<string> WriteCode(List<string> code)
        {
            List<string> res = new List<string>();
            if (code == null)
            {
                return res;
            }

            foreach (var line in code)
            {
                res.Add(indent.Do() + line);
            }

            return res;
        }

        Dictionary<string, bool> writtenActions = new Dictionary<string, bool>();

        private List<string> WriteActions(List<Action> actions)
        {
            List<string> res = new List<string>();
            if (actions == null)
            {
                return res;
            }

            foreach (var act in actions)
            {
                res.Add(indent.Do() + act.Name + "();");
            }
            return res;
        }

        private HashSet<Action> curActionSet = new HashSet<Action>();

        private void CreateActionSet(StateMachine machine)
        {
            //Создаем список выходных воздействий.
            curActionSet = new HashSet<Action>();

            //reject
            if (machine.AutoReject)
            {
                var reject = new Action {Name = rejectFunction, Synchronism = Synchronism.Synchronous};
                curActionSet.Add(reject);
            }

            foreach (var state in machine.States)
            {
                PutActionsToSet(ref curActionSet, state.EntryActions);
                PutActionsToSet(ref curActionSet, state.ExitActions);

                foreach (var trans in state.Outgoing)
                {
                    PutActionsToSet(ref curActionSet, trans.Actions);
                }
            }
        }

        private List<string> ExportActionSet()
        {
            List<string> res = new List<string>();
            foreach (var action in curActionSet)
            {
                res.Add(action.Name);
            }
            return res;
        }

        private void PutActionsToSet(ref HashSet<Action> actionSet, List<Action> list)
        {
            foreach (var action in list)
            {
                actionSet.Add(action);
            }
        }

        private void WriteActionsDeclarations(StateMachine machine)
        {
            CreateActionSet(machine);
            //Печатаем.
            foreach (var action in curActionSet)
            {
                WriteAction(action);
                /*
                sw.WriteLine(indent.Do() + "/// <summary>");
                //sw.WriteLine(indent.Do() + "///" + action.Comment);
                sw.WriteLine(indent.Do() + "/// </summary>");
                sw.WriteLine(indent.Do() + "public void " + action.Name + "()");
                sw.WriteLine(indent.Do() + "{");
                sw.WriteLine(indent.Do() + "}");
                 * */
            }
        }

        private void WriteConstructor(StateMachine machine)
        {
            sw.WriteLine(indent.Do() + "public {0}()", machine.Name);
            sw.WriteLine(indent.Do() + "{");
            ++indent;
            sw.WriteLine(indent.Do() + "{0} = {1}.{2};",
                stateVariableName, statesEnumName,
                machine.StartState.Name);
            --indent;
            sw.WriteLine(indent.Do() + "}");
        }

        private void WriteNestedMachines(StateMachine machine)
        {
            var nm = AddNestedMachines(machine);
            foreach (var line in nm)
            {
                sw.WriteLine(line);
            }
        }

        private List<string> AddNestedMachines(StateMachine machine)
        {
            List<string> res = new List<string>();
            foreach (var state in machine.States)
            {
                if (state.NestedMachines == null)
                {
                    continue;
                }

                foreach (var nestedMachine in state.NestedMachines)
                {
                    res.Add(string.Format(indent.Do() + "public {0} {1} {{ get; set; }}", 
                        nestedMachine.Type, nestedMachine.Name));
                }
            }
            return res;
        }

        private void Prepare()
        {
        }

        private void WriteEvents()
        {
            sw = new StreamWriter(param.WorkDirectory + "\\Events.cs");
            indent = new Indent();

            WriteStartNamespace();

            sw.WriteLine(indent.Do() + "public enum Events");
            sw.WriteLine(indent.Do() + "{");
            ++indent;

            var evtList = new Dictionary<string, Event>();
            foreach (var machine in param.Machines)
            {
                if (machine.Events == null)
                {
                    continue;
                }

                foreach (var anEvent in machine.Events)
                {
                    if (anEvent.Name != "*")
                    {
                        evtList[anEvent.Name] = anEvent;                        
                    }
                }
            }
            foreach (KeyValuePair<string, Event> @event in evtList)
            {
                sw.WriteLine(indent.Do() + @event.Value.Name + ",");
            }
            //sw.WriteLine(indent.Do() + StaterV.Attributes.Event.CreateEpsilon().SafeName + ",");
            --indent;
            sw.WriteLine(indent.Do() + "}");

            WriteEndNamespace();

            sw.Close();
        }
    }
}
