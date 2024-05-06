using System;
using System.Collections.Generic;
using System.Linq;
using SpinVeriff.LTLAPParser;
using SpinVeriff.LTLSimpleParser;
using StaterV.Attributes;
using StaterV.StateMachine;
using TextProcessor;
using Events = SpinVeriff.LTLAPParser.Events;

namespace SpinVeriff
{
    public class LTLConverter
    {
        public LTLConverter(Options _options, List<StateMachine> machines, List<AutomatonExecution> _forkList)
        {
            options = _options;
            //LTL = new List<string>(options.FormulaeLTL);
            SetLTL(options.FormulaeLTL);
            machineSystem = machines;
            forkList = _forkList;
            UseOldParser = false;
        }

        //public List<string> LTL { get; set; }
        private List<string> LTL; 
        private readonly Options options;
        private readonly List<StateMachine> machineSystem;
        private readonly List<AutomatonExecution> forkList;


        private readonly Dictionary<int, string> props = new Dictionary<int, string>();
        public Dictionary<int, string> Propositions
        {
            get { return props; }
        }

        public string curToken { get; private set; }
        public StateMachine curMachine { get; private set; }
        private int propIndex = 0;

        private void SetLTL(List<string> dirtLTL)
        {
            LTL = SetLTLExt(dirtLTL);
            /*
            LTL = new List<string>();
            foreach (var formula in dirtLTL)
            {
                if (formula[0] == '/' && formula[1] == '/')
                {
                    continue;
                }
                LTL.Add(formula);
            }
             */ 
        }

        public static List<string> SetLTLExt(List<string> dirtLTL)
        {
            var eLTL = new List<string>();
            if (dirtLTL == null)
            {
                return eLTL;
            }
            foreach (var formula in dirtLTL)
            {
                if (formula[0] == '/' && formula[1] == '/')
                {
                    continue;
                }
                eLTL.Add(formula);
            }
            return eLTL;
        }

        /// <summary>
        /// Process LTL-formula with LTLSimpleParser
        /// </summary>
        /// <param name="proposition">
        /// Convert LTL formula for spin. There are atomic propositions (AP) in the curly brackets.
        /// AP::==
        /// MachineType machineName.eventName || (event "eventName" happens)
        /// MachineType machineName.StateName || ("machineName" comes to state "StateName")
        /// MachineType machineName.Function() || ("machineName" calls function "Function")
        /// MachineType machineName->nestedMachine ||  ("machineName" uses nested statemachine "nestedMachine")
        /// MachineType machineName||forkMachine || ("machineName" starts a new thread with machine "forkMachine")
        /// MachineType machineName.variable OP value || (compare with value)
        /// static variable OP value. (compare global variable with value) .
        /// 
        /// OP ::== > || < || == || != || >= || <=
        /// 
        /// 
        /// </param>
        /// <returns></returns>
        private string ProcessOneLTL(string formula)
        {
            string res = formula;
            int pos = 0;

            while ((pos < res.Length) && (pos != -1))
            {
                pos = res.IndexOf('{');
                var clPos = res.IndexOf('}');
                if (clPos == -1)
                {
                    //Error!
                    break;
                }

                string prop = res.Substring(pos + 1, clPos - pos - 1);
                ProcessProposition(prop);
                var newRes = res.Substring(0, pos) + "p" + propIndex + res.Substring(clPos + 1);
                res = newRes;

                ++propIndex;
            }

            return res;
        }

        public bool UseOldParser { get; set; }

        public List<string> ProcessLTL()
        {
            propIndex = 0;
            List<string> res = new List<string>();
            foreach (var formula in LTL)
            {
                if (formula[0] == '/' && formula[1] == '/')
                {
                    continue;
                }
                res.Add(ProcessOneLTL(formula));
            }
            return res;
        }

        public void SaveCurFSM()
        {
            curMachine = null;
            foreach (var obj in options.EnteredObjects)
            {
                if (curToken == obj.Name.Trim())
                {
                    //curMachine = obj.Type;
                    curMachine = machineSystem.First(machine => obj.Type == machine.Name.Trim());
                    return;
                }
            }

            foreach (var ex in forkList)
            {
                if (curToken == ex.Name)
                {
                    curMachine = machineSystem.First(machine => ex.Type == machine.Name);
                    return;
                }
            }

            //Nested machines
            foreach (var machine in machineSystem)
            {
                foreach (var state in machine.States)
                {
                    foreach (var n in state.TheAttributes.NestedMachines)
                    {
                        if (n.Name == curToken)
                        {
                            curMachine = machineSystem.First(fsm => n.Type == fsm.Name);
                            return;
                        }
                    }
                }
            }
        }
        
        private string ProcessProposition(string proposition)
        {
            string res = "";
            APParser parser = new APParser(this, propIndex);
            Tokenizer tokenizer = new Tokenizer();
            var tokens = tokenizer.ParseText(proposition);
            foreach (var t in tokens)
            {
                curToken = t.Value.Trim();
                switch (t.Type)
                {
                    case TokenType.Word:
                        if (curToken == "true" || curToken == "false")
                        {
                            parser.ProcessEvent(Events.constant);
                        }
                        else if (IsMachineName(curToken))
                        {
                            parser.ProcessEvent(Events.machine_type);
                            parser.ProcessEvent(Events.machine_name);
                        }

                        else if (IsState(curToken))
                        {
                            parser.ProcessEvent(Events.state_name);
                        }

                        else if (IsVariable(curToken))
                        {
                            parser.ProcessEvent(Events.variable);
                        }

                        else if (IsArray(curToken))
                        {
                            parser.ProcessEvent(Events.array);
                        }

                        else if (IsEvent(curToken))
                        {
                            parser.ProcessEvent(Events.event_name);
                        }

                        else if (IsFunction(curToken))
                        {
                            parser.ProcessEvent(Events.function_name);
                        }

                        else if (IsNested(curToken))
                        {
                            parser.ProcessEvent(Events.nested_machine);
                        }

                        else if (IsExecution(curToken))
                        {
                            parser.ProcessEvent(Events.fork_machine);
                        }

                        break;
                    case TokenType.IntegerNumber:
                        parser.ProcessEvent(Events.constant);
                        break;
                    case TokenType.FloatingNumber:
                        parser.ProcessEvent(Events.constant);
                        break;
                    case TokenType.TextMess:
                        break;
                    case TokenType.Assign:
                        break;
                    case TokenType.Equals:
                        parser.ProcessEvent(Events.see_operator);
                        break;
                    case TokenType.Less:
                        parser.ProcessEvent(Events.see_operator);
                        break;
                    case TokenType.Greater:
                        parser.ProcessEvent(Events.see_operator);
                        break;
                    case TokenType.GreaterEqual:
                        parser.ProcessEvent(Events.see_operator);
                        break;
                    case TokenType.LessEqual:
                        parser.ProcessEvent(Events.see_operator);
                        break;
                    case TokenType.Plus:
                        break;
                    case TokenType.Minus:
                        parser.ProcessEvent(Events.see_minus);
                        break;
                    case TokenType.Semicolon:
                        break;
                    case TokenType.Dot:
                        parser.ProcessEvent(Events.dot);
                        break;
                    case TokenType.OpenSqBracket:
                        parser.ProcessEvent(Events.open_sq_bracket);
                        break;
                    case TokenType.CloseSqBracket:
                        parser.ProcessEvent(Events.close_sq_bracket);
                        break;
                    case TokenType.VerticalLine:
                        parser.ProcessEvent(Events.vertical_line);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (parser.State != APParser.States.reject)
            {
                Propositions.Add(propIndex, parser.Proposition);
            }
            return res;
        }

        private bool IsNested(string word)
        {
            if (curMachine == null)
            {
                return false;
            }
            return curMachine.States.SelectMany(state => state.TheAttributes.NestedMachines).
                              Any(nm => nm.Name == word.Trim());
        }

        private bool IsExecution(string word)
        {
            if (curMachine == null)
            {
                return false;
            }

            return
                curMachine.States.SelectMany(state => state.TheAttributes.EntryExecutions).
                          Any(ex => ex.Name == word.Trim());
        }

        private bool IsEvent(string word)
        {
            return curMachine != null && curMachine.Events.Any(evt => evt.Name == word.Trim());
        }

        private bool IsFunction(string word)
        {
            if (curMachine == null)
            {
                return false;
            }

            if (curMachine.Transitions.SelectMany
                (transition => transition.TheAttributes.Actions).Any(action => action.Name == word.Trim()))
            {
                return true;
            }

            if (curMachine.States.SelectMany
                (state => state.TheAttributes.EntryActions).Any(action => action.Name == word.Trim()))
            {
                return true;
            }

            if (curMachine.States.SelectMany
                (state => state.TheAttributes.ExitActions).Any(action => action.Name == word.Trim()))
            {
                return true;
            }
            return false;
        }

        private bool IsVariable(string word)
        {
            return curMachine != null && curMachine.Variables.OfType<SingleVariable>().Any(v => v.Name.Trim() == word.Trim());

            /*
            foreach (var variable in curMachine.Variables)
            {
                var v = variable as SingleVariable;
                if (v != null)
                {
                    if (v.Name == word)
                    {
                        return true;
                    }
                }
            }
            return false;
             
             * */
        }

        private bool IsArray(string word)
        {
            return curMachine != null && curMachine.Variables.OfType<StaterV.StateMachine.Array>().Any(a => a.Name.Trim() == word.Trim());
        }

        private bool IsState(string word)
        {
            return curMachine != null && curMachine.States.Any(state => state.TheAttributes.Name == word);
        }

        private bool IsMachineType(string word)
        {
            foreach (var machine in machineSystem.Where(machine => word.Trim() == machine.Name.Trim()))
            {
                curMachine = machine;
                return true;
            }
            return false;

            /*
            foreach (var machine in machineSystem)
            {
                if (word.Trim() == machine.Name.Trim())
                {
                    curMachine = machine;
                    return true;
                }
            }
            return false;
            // * */
        }

        private bool IsMachineName(string word)
        {
            foreach (var obj in options.EnteredObjects)
            {
                if (word.Trim() == obj.Name.Trim())
                {
                    //curMachine = obj.Type;
                    //curMachine = machineSystem.First(machine => obj.Type == machine.Name.Trim());
                    return true;
                }
            }

            if (forkList != null)
            {
                if (forkList.Any(fsm => fsm.Name == word))
                {
                    return true;
                }
                
            }
            return false;
        }

        //[] ({AParser par:eventname} || {AParser par.StateName})
        //[] ({AParser par:name} || {AParser par.Name})
        /// <summary>
        /// Convert LTL formula for spin. There are atomic propositions (AP) in the curly brackets.
        /// AP:
        /// MachineType machineName:eventName - event "eventName" happens.
        /// MachineType machineName.StateName - "machineName" comes to state "StateName".
        /// MachineType machineName.Function() - "machineName" calls function "Function".
        /// MachineType machineName->nestedMachine - "machineName" uses nested statemachine "nestedMachine".
        /// MachineType machineName||forkMachine - "machineName" starts a new thread with machine "forkMachine".
        /// static variable > value - "variable" is greater than value. (supported operators: >, <, ==, !=, >=, <=.
        /// </summary>
        /// <returns>LTL formula for spin</returns>
        private string Convert(string formula)
        {
            string res = formula;
            //1. Заменяем все, что в фигурных скобках на высказывания спина.
            int figPos = res.IndexOf("{");
            cProp = 0;
            Propositions.Clear();
            while (figPos != -1)
            {
                int pairFig = res.IndexOf("}");
                string prop = res.Substring(figPos + 1, pairFig - figPos - 1);
                //TryEvent(prop);
                //*

                //TODO: Сделать по-нормальному.
                if (!TryEvent(prop))
                {
                    if (!TryNested(prop))
                    {
                        if (!TryFunction(prop))
                        {
                            TryState(prop);
                        }
                    }
                }
                // * */
                var newRes = res.Substring(0, figPos) + "p" + cProp + res.Substring(pairFig + 1);
                res = newRes;

                figPos = res.IndexOf("{");
                ++cProp;
            }

            return res;
        }

        public List<string> Convert()
        {
            List<string> res = new List<string>();
            foreach (var formula in LTL)
            {
                res.Add(Convert(formula));
            }
            return res;
        }

        /// <summary>
        /// Try parse nested machine.
        /// </summary>
        /// <param name="prop">MachineType machineName->nestedMachine</param>
        /// <returns></returns>
        private bool TryNested(string prop)
        {
            string machineType, otherPart;
            GetMachineName(prop, out machineType, out otherPart);

            int ind = otherPart.IndexOf("->");
            if (ind == -1)
            {
                return false;
            }

            string machineName = otherPart.Substring(0, ind);
            string nestedname = otherPart.Substring(ind+2);
            string newProp = "#define p" + cProp + "\t(" + machineName + "." + ModelGenerator.NestedMachineVar +
                             " == " + ModelGenerator.NameNestedCall(nestedname) + ")";

            Propositions.Add(cProp, newProp);
            return true;
        }

        /// <summary>
        /// Try parse function.
        /// </summary>
        /// <param name="prop">MachineType machineName.Function() - "machineName" calls function "Function".</param>
        /// <returns>True if parsing is ok, false otherwise.</returns>
        private bool TryFunction(string prop)
        {
            int iBrackets = prop.IndexOf("()");
            if (iBrackets == -1)
            {
                return false;
            }
            string machineName, otherPart;
            try
            {
                GetMachineName(prop.Substring(0, iBrackets), out machineName, out otherPart);
            }
            catch (Exception)
            {
                return false;
            }

            var l = otherPart.Split('.');
            if (l.Length != 2)
            {
                return false;
            }

            //string machineName = l[0];

            string functionName = l[1];

            string newProp = "#define p" + cProp + "\t(" + l[0] + "." + ModelGenerator.FunctionVar + 
                             " == " + ModelGenerator.FunctionTextID(machineName, functionName) + ")";

            Propositions.Add(cProp, newProp);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"> Proposition like "AParser p1.state"</param>
        private void TryState(string prop)
        {
            string machineName, otherPart;
            GetMachineName(prop, out machineName, out otherPart);

            var l = otherPart.Split('.');
            if (l.Length != 2)
            {
                throw new ArgumentException(@"Wrong state proposition", prop);
            }

            string stateName = ModelGenerator.CallStateForModel(machineName, l[1]);

            string newProp = "#define p" + cProp + "\t(" + l[0] + ".state == " + stateName + ")";
            Propositions.Add(cProp, newProp);
        }

        private static void GetMachineName(string prop, out string machineName, out string otherPart)
        {
            var l = prop.Trim().Split(' ');
            if (l.Length != 2)
            {
                throw new ArgumentException(@"Wrong state proposition", prop);
            }

            machineName = l[0];
            otherPart = l[1];
        }

        int cProp = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"> Proposition like "AParser p1:eventName"</param>
        /// <returns></returns>
        private bool TryEvent(string prop)
        {
            string machineName, otherPart;
            GetMachineName(prop, out machineName, out otherPart);

            var delim = otherPart.IndexOf(":");
            if (delim == -1)
            {
                return false;
            }

            //Все, что справа от двоеточия - событие.
            //Слева - автомат.
            //string m = prop.Substring(0, delim);
            //string machine = ConvertToModelNames(m);
            string machine = otherPart.Substring(0, delim);
            string @event = otherPart.Substring(delim + 1);

            string newProp = "#define p" + cProp + "\t(" + machine + ".curEvent == " + @event + ")";
            Propositions.Add(cProp, newProp);
            return true;
        }
    }
}
