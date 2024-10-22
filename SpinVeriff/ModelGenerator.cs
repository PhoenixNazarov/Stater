using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginData;
using SpinVeriff.Exceptions;
using SpinVeriff.Parameters;
using StaterV;
using StaterV.Attributes;
using StaterV.StateMachine;
using StaterV.Widgets;
using TextProcessor;
using Action=StaterV.Attributes.Action;
using Array = StaterV.StateMachine.Array;

namespace SpinVeriff
{
    class MachineVariable
    {
        public MachineVariable() { }
        public MachineVariable(StateMachine m, string name)
        {
            Machine = m;
            Name = name;
        }

        public StateMachine Machine;
        public string Name;
    }

    public class ModelGenerator
    {
        public const string StateVar            = "state";
        public const string EventVar            = "curEvent";
        public const string IDVar               = "ID";
        public const string FunctionVar         = "functionCall";
        public const string NestedMachineVar    = "nestedMachine";
        public const string ForkMachineVar      = "forkMachine";
        public const string RejectState         = "reject";
        public const string StartedVar          = "started";
        public const string FinishedVar         = "finished";

        //public const string FinishedFlag        = "Finished";

        public const string MachineArg          = "machine";
        public const string EventArg            = "evt";

        public const string ChanSuffix          = "_ch";

        private Dictionary<int, string> propositions = new Dictionary<int, string>();

        private List<string> allMachines;
        public List<string> AllMachines
        {
            get { return allMachines; }
        }

        public bool CreateModel(SpinPlugin _owner, 
                                List<StateMachine> machines, 
                                Options _options, 
                                List<AutomatonExecution> _forkList,
                                out List<string> model)
        {
            machineSystem = machines;
            owner = _owner;
            options = _options;
            forkList = _forkList;
            model = new List<string>();

            LTLConverter conv = new LTLConverter(options, machines, forkList);
            conv.UseOldParser = true;
            //var formulae = conv.Convert();
            var formulae = conv.ProcessLTL();
            propositions = conv.Propositions;

            indentLevel = 0;
            try
            {
                if (options.TextBefore != null)
                {
                    model.Add(options.TextBefore.Trim());
                    model.Add("\n");
                }
                model.AddRange(DefineValues());
                model.AddRange(DefineStructures(machineSystem));
                if (!options.OnlyFSM)
                {
                    model.AddRange(WriteRandom());
                } 
                model.AddRange(DefinePropositions());
                if (!options.OnlyFSM)
                {
                    model.AddRange(DefineChans(options.EnteredObjects));
                }
                model.AddRange(WriteMachines(machines));
                FillAllMachines(options.EnteredObjects);
                if (!options.OnlyFSM)
                {
                    model.AddRange(WriteAllVolatileChanges());
                    model.AddRange(WriteExec(options.EnteredObjects, machineSystem));
                }
                if (options.TextAfter != null)
                {
                    model.Add("\n");
                    model.Add(options.TextAfter.Trim());
                }
            }
            catch(InvalidNameException ex)
            {
                resultMessage = "Invalid name: " + ex.Message;
            }

            model.Add("\n");
            model.AddRange(WriteLTL(formulae));
            //model.Add(DoNeverClaim(formula));

            return true;
        }

        private void FillAllMachines(List<Options.ObjectName> mvars)
        {
            allMachines = new List<string>();
            foreach (var machine in mvars)
            {
                if (machine != null)
                {
                    allMachines.Add(machine.Name);
                }
            }

            foreach (var execution in forkList)
            {
                if (execution != null)
                {
                    allMachines.Add(execution.Name);
                }
            }
        }


        private IEnumerable<string> DefineChans(IEnumerable<Options.ObjectName> mvars)
        {
            List<string> res = new List<string>();

            foreach (var objectName in mvars)
            {
                res.Add(WriteChan(objectName.Name));
            }

            foreach (var execution in forkList)
            {
                res.Add(WriteChan(execution.Name));
            }

            return res;
        }

        private IEnumerable<string> WriteLTL(List<string> formulae)
        {
            List<string> res = new List<string>();
            int i = 0;
            foreach (var f in formulae)
            {
                res.Add("ltl f" + i + " {" + f + "}");
                ++i;
            }
            return res;
        }

        private IEnumerable<string> WriteProcesses(IEnumerable<Options.ObjectName> mvars, List<StateMachine> machineSystem)
        {
            List<string> res = new List<string>();

            //TODO: Зарефакторить!
            foreach (var objectName in mvars)
            {
                //res.Add(WriteChan(objectName.Name));
                //res.Add(DoIndent() + "bool " + objectName.Name + FinishedFlag + ";");
                //res.Add(WriteFinishedFlag(objectName.Name));
                res.Add("");
                res.AddRange(WriteSystemProcess(objectName.Type, objectName.Name));
                if (options.EventSource == Options.EEventSource.Fair)
                {
                    res.AddRange(WriteFairEventSource(FindFSM(objectName.Type), objectName.Name));
                }
                else
                {
                    res.AddRange(WriteEventSource(objectName.Name));
                }
            }
            res.Add("");

            foreach (var execution in forkList)
            {
                //res.Add(WriteChan(execution.Name));
                //res.Add(WriteFinishedFlag(execution.Name));
                res.Add("");
                res.AddRange(WriteSystemProcess(execution.Type, execution.Name));
                if (options.EventSource == Options.EEventSource.Fair)
                {
                    res.AddRange(WriteFairEventSource(FindFSM(execution.Type), execution.Name));
                }
                else
                {
                    res.AddRange(WriteEventSource(execution.Name));
                }
            }

            return res;
        }

        private StateMachine FindFSM(string type)
        {
            foreach (var fsm in machineSystem)
            {
                if (fsm.Name == type)
                {
                    return fsm;
                }
            }
            return null;
        }

        private string WriteChan(string name)
        {
            return DoIndent() + "chan " + name + ChanSuffix + " = [0] of {int}";
        }

        /*
        private string WriteFinishedFlag(string name)
        {
            return DoIndent() + "bool " + name + FinishedFlag + ";";
        }
         * */

        private IEnumerable<string> WriteEventSource(string name)
        {
            List<string> res = new List<string>();

            res.Add(DoIndent() + "proctype " + name + "EventSource ()");
            res.Add(DoIndent() + "{");
            ++indentLevel;

            res.Add(DoIndent() + "byte newEvt;");
            
            res.Add(DoIndent() + "do");
            ++indentLevel;
            foreach (var evt in eventList)
            {
                res.Add(DoIndent() + ":: newEvt = " + evt.Key + " -> ");
                ++indentLevel;
                res.Add(DoIndent() + "atomic");
                res.Add(DoIndent() + "{");
                ++indentLevel;
                res.Add(DoIndent() + name + ChanSuffix + " ! newEvt;");    
                res.Add(DoIndent() + "printf(\"" + name + "source sent _" + evt.Key + "_\\n\");");
                --indentLevel;
                res.Add(DoIndent() + "}");
                foreach (var change in volatileChanges)
                {
                    res.Add(DoIndent() + change.Value + ";");
                }
                --indentLevel;
            }
            --indentLevel;
            res.Add(DoIndent() + "od;");

            --indentLevel;
            res.Add(DoIndent() + "}");

            return res;
        }

        private IEnumerable<string> WriteFairEventSource(StateMachine fsm, string name)
        {
            List<string> res = new List<string>();

            res.Add(DoIndent() + "proctype " + name + "EventSource ()");
            res.Add(DoIndent() + "{");
            ++indentLevel;

            res.Add(DoIndent() + "byte newEvt;");

            res.Add(DoIndent() + "do");
            ++indentLevel;
            foreach (var state in fsm.States)
            {
                res.Add(DoIndent() + ":: (" + name + "." + StateVar + " == " +
                    CallStateForModel(fsm, state) + ") ->");

                if (state.OutgoingArrows.Count == 0)
                {
                    res.Add(DoIndent() + "skip;");
                    continue;
                }

                res.Add(DoIndent() + "if");
                ++indentLevel;
                foreach (var arr in state.OutgoingArrows)
                {
                    var trans = arr as Transition;
                    if (trans == null)
                    {
                        throw new ArgumentException("Wrong Arrow type in the diagram!", "state.OutgoingArrows: arr");
                    }
                    res.Add(DoIndent() + ":: (1) -> newEvt = " + trans.TheAttributes.TheEvent.Name + ";");
                    ++indentLevel;
                    foreach (var change in volatileChanges)
                    {
                        res.Add(DoIndent() + change.Value + ";");
                    }
                    res.Add(DoIndent() + "atomic");
                    res.Add(DoIndent() + "{");
                    ++indentLevel;
                    res.Add(DoIndent() + name + ChanSuffix + " ! newEvt;");
                    res.Add(DoIndent() + "printf(\"" + name + "source sent _" + 
                        trans.TheAttributes.TheEvent.Name + "_\\n\");");
                    --indentLevel;
                    res.Add(DoIndent() + "}");
                    --indentLevel;
                }
                --indentLevel;
                res.Add(DoIndent() + "fi;");
            }
            res.Add(DoIndent() + "od;");
            --indentLevel;
            res.Add(DoIndent() + "}");

            return res;
        }

        private IEnumerable<string> WriteRandom()
        {
            List<string> res = new List<string>();

            res.Add(DoIndent() + "inline random(var, nBits)");
            res.Add(DoIndent() + "{");
            ++indentLevel;
            res.Add(DoIndent() + "int index = 0;");
            res.Add(DoIndent() + "var = 0;");
            res.Add(DoIndent() + "do");
            ++indentLevel;
            res.Add(DoIndent() + ":: index < nBits -> ");
            ++indentLevel;
            res.Add(DoIndent() + "index = index + 1;");
            res.Add(DoIndent() + "var = var * 2;");
            res.Add(DoIndent() + "if");
            ++indentLevel;
            res.Add(DoIndent() + ":: var = var + 1;");
            res.Add(DoIndent() + ":: var = var + 0;");
            --indentLevel;
            res.Add(DoIndent() + "fi;");
            --indentLevel;
            res.Add(DoIndent() + ":: else -> break;");
            --indentLevel;
            res.Add(DoIndent() + "od;");
            --indentLevel;
            res.Add(DoIndent() + "}");
            res.Add("");

            return res;
        }

        private IEnumerable<string> WriteAllVolatileChanges()
        {
            List<string> res = new List<string>();
            foreach (var enteredObject in options.EnteredObjects)
            {
                res.AddRange(WriteVolatileChanges(enteredObject));
                res.AddRange(WriteParamChanges(enteredObject));
            }

            foreach (var fork in forkList)
            {
                Options.ObjectName f = new Options.ObjectName { Name = fork.Name, Type = fork.Type };
                res.AddRange(WriteVolatileChanges(f));
                res.AddRange(WriteParamChanges(f));
            }

            return res;
        }

        private Dictionary<string, string> paramChanges = new Dictionary<string, string>();
        private IEnumerable<string> WriteParamChanges(Options.ObjectName curObj)
        {
            List<string> res = new List<string>();
            var fsm = machineSystem.First(machine => machine.Name == curObj.Type);
            /*
            if (fsm == null)
            {
                return res;
            }
             */ 
            string funcName = curObj.Name + "ParamChange()";
            res.Add(DoIndent() + "inline " + funcName);
            res.Add(DoIndent() + "{");

            paramChanges.Add(curObj.Name, funcName);

            ++indentLevel;
            res.Add(DoIndent() + "int r;");
            res.Add(DoIndent() + "int ind = 0;");
            if (fsm != null && fsm.Variables != null)
            {
                foreach (var variable in fsm.Variables)
                {
                    if (variable.Param)
                    {
                        var sv = variable as SingleVariable;
                        if (sv != null)
                        {
                            string str = curObj.Name + "." + sv.Name;
                            res.Add(DoIndent() + "random(r, " + GetNBits(sv) + ");");

                            res.Add(DoIndent() + "atomic");
                            res.Add(DoIndent() + "{");
                            ++indentLevel;
                            res.Add(DoIndent() + "printf(\"Machine param " + str + " set to %d\\n\", r" + ");");
                            res.Add(DoIndent() + str + " = r;");
                            --indentLevel;
                            res.Add(DoIndent() + "}");
                        }

                        var av = variable as Array;
                        if (av != null)
                        {
                            res.Add(DoIndent() + "do");

                            ++indentLevel;
                            res.Add(DoIndent() + ":: ind < " + av.NElements + " ->");

                            ++indentLevel;
                            string str = curObj.Name + "." + av.Name + "[ind]";
                            res.Add(DoIndent() + "random(r, " + GetNBits(av) + ");");

                            res.Add(DoIndent() + "atomic");
                            res.Add(DoIndent() + "{");
                            ++indentLevel;
                            res.Add(DoIndent() + "printf(\"Machine param " +
                                curObj.Name + "." + av.Name + "[%d]" + " set to %d\\n\", ind, r" + ");");
                            res.Add(DoIndent() + str + " = r;");
                            --indentLevel;
                            res.Add(DoIndent() + "}");

                            res.Add(DoIndent() + "ind = ind + 1;");
                            --indentLevel;

                            res.Add(DoIndent() + ":: else -> break;");
                            --indentLevel;

                            res.Add(DoIndent() + "od;");
                        }
                    }
                }
            } 
            --indentLevel;
            
            res.Add(DoIndent() + "}");

            res.Add("");

            return res;
        }

        private Dictionary<string, string> volatileChanges = new Dictionary<string, string>();
        private IEnumerable<string> WriteVolatileChanges(Options.ObjectName curObj)
        {
            List<string> res = new List<string>();
            var fsm = machineSystem.First(machine => machine.Name == curObj.Type);
            if (fsm == null)
            {
                return res;
            }
            string funcName = curObj.Name + "VolatileChange()";
            res.Add(DoIndent() + "inline " + funcName);
            res.Add(DoIndent() + "{");

            volatileChanges.Add(curObj.Name, funcName);

            ++indentLevel;
            res.Add(DoIndent() + "int r;");
            res.Add(DoIndent() + "int ind = 0;");
            foreach (var variable in fsm.Variables)
            {
                if (variable.Volatile)
                {
                    var sv = variable as SingleVariable;
                    if (sv != null)
                    {
                        string str = curObj.Name + "." + sv.Name;

                        res.Add(DoIndent() + "atomic");
                        res.Add(DoIndent() + "{");
                        ++indentLevel;
                        res.Add(DoIndent() + "random(r, " + GetNBits(sv) + ");");
                        res.Add(DoIndent() + "printf(\"Environment changed " + str + " to %d\\n\", r" + ");");
                        res.Add(DoIndent() + str + " = r;");
                        --indentLevel;
                        res.Add(DoIndent() + "}");
                    }

                    var av = variable as Array;
                    if (av != null)
                    {
                        res.Add(DoIndent() + "do");
                        
                        ++indentLevel;
                        res.Add(DoIndent() + ":: ind < " + av.NElements + " ->");

                        ++indentLevel;
                        string str = curObj.Name + "." + av.Name + "[ind]";

                        res.Add(DoIndent() + "atomic");
                        res.Add(DoIndent() + "{");
                        ++indentLevel;
                        res.Add(DoIndent() + "random(r, " + GetNBits(av) + ");");
                        res.Add(DoIndent() + "printf(\"Environment changed " + 
                            curObj.Name + "." + av.Name + "[%d]" + " to %d\\n\", ind, r" + ");");
                        res.Add(DoIndent() + str + " = r;");
                        --indentLevel;
                        res.Add(DoIndent() + "}");

                        res.Add(DoIndent() + "ind = ind + 1;");
                        --indentLevel;

                        res.Add(DoIndent() + ":: else -> break;");
                        --indentLevel;
                        
                        res.Add(DoIndent() + "od;");
                    }
                }
            }
            --indentLevel;
            
            res.Add(DoIndent() + "}");

            res.Add("");

            return res;
        }

        private int GetNBits(Variable variable)
        {
            switch (variable.Type)
            {
                case Variable.TypeList.Bool:
                    return 1;
                case Variable.TypeList.Int8:
                    return 8;
                case Variable.TypeList.Int16:
                    return 16;
                case Variable.TypeList.Int32:
                    return 32;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<string> WriteSystemProcess(string type, string name)
        {
            List<string> res = new List<string>();

            res.Add(DoIndent() + "proctype " + name + "Proc ()");
            res.Add(DoIndent() + "{");
            ++indentLevel;

            res.Add(DoIndent() + "byte newEvt;");
            res.Add(DoIndent() + name + "." + StartedVar + "= true;");
            res.Add(DoIndent() + name + ChanSuffix + " ? newEvt;");

            res.Add(DoIndent() + name + "ParamChange();");

            res.Add(DoIndent() + "do");
            ++indentLevel;

            res.Add(DoIndent() + ":: " + name + "." + FinishedVar + " == false ->");
            ++indentLevel;
            res.Add((DoIndent() + type + "(" + name + ", newEvt);"));
            res.Add(DoIndent() + name + ChanSuffix + " ? newEvt;");
            --indentLevel;
            res.Add(DoIndent() + ":: else -> skip;");

            --indentLevel;
            res.Add(DoIndent() + "od;");

            --indentLevel;
            res.Add(DoIndent() + "}");

            return res;
        }

        private string DoNeverClaim(string formula)
        {
            var spin = new System.Diagnostics.Process();
            spin.StartInfo.FileName = SpinPlugin.Spin;
            spin.StartInfo.Arguments = "-f \"" + formula + "\"";
            spin.StartInfo.RedirectStandardOutput = true;
            spin.StartInfo.UseShellExecute = false;
            spin.Start();

            var never = spin.StandardOutput.ReadToEnd();
            /*
            StringBuilder sb = new StringBuilder();
            while (!spinOut.EndOfStream)
            {
                sb.Append(spinOut.ReadLine());
                sb.Append('\n');
            }
             * */

            spin.WaitForExit();
            spin.Close();
            return never;
        }



        private IEnumerable<string> DefinePropositions()
        {
            List<string> res = new List<string>();

            foreach (var p in propositions)
            {
                res.Add(DoIndent() + p.Value);
            }

            return res;
        }

        private IEnumerable<string> WriteExec(List<Options.ObjectName> mvars, List<StateMachine> machines)
        {
            List<string> res = new List<string>();

            foreach (var machine in mvars)
            {
                res.Add(DoIndent() + MachineStructName(machine.Type) + " " + machine.Name + ";");
            }

            foreach (var execution in forkList)
            {
                res.Add(DoIndent() + MachineStructName(execution.Type) + " " + execution.Name + ";");
            }

            res.AddRange(WriteNestedDeclarations());

            res.Add("");

            res.AddRange(WriteProcesses(options.EnteredObjects, machineSystem));

            res.Add("");

            res.Add(DoIndent() + "init");
            res.Add(DoIndent() + "{");
            ++indentLevel;

            res.AddRange(WriteStartConditions(mvars, machines));

            --indentLevel;
            res.Add(DoIndent() + "}");

            return res;
        }

        private IEnumerable<string> WriteNestedDeclarations()
        {
            List<string> res = new List<string>();

            foreach (var o in nestedObjects)
            {
                //res.Add(DoIndent() + MachineStructName(o.Type) + " " + o.Name + ";");
                res.Add(DoIndent() + MachineStructName(o.Type) + " " + o.Name + ";");
                //res.Add(DoIndent() + machine.Type + "(" + machine.Name + ");");
            }

            return res;
        }

        private IEnumerable<string> WriteStartConditions(List<Options.ObjectName> mvars, 
                                                         List<StateMachine> machines)
        {
            //allMachines = new List<string>();
            List<string> res = new List<string>();
            int count = 0;
            foreach (var machine in mvars)
            {
                res.Add(DoIndent() + machine.Name + "." + IDVar + " = " + count + ";");
                res.Add(DoIndent() + machine.Name + "." + StartedVar + " = false;");
                res.Add(DoIndent() + machine.Name + "." + FinishedVar + " = false;");
                //allMachines.Add(machine.Name);
                Options.ObjectName o = machine;
                var sm = machines.Find
                    (
                    delegate(StateMachine m)
                        {
                            return m.Name == o.Type;
                        }
                    );

                if (sm.StartState == null)
                {
                    throw new ArgumentNullException("Не задано начальное состояние автомата");
                }

                res.Add(DoIndent() + machine.Name + ".state = " + CallStateForModel(sm, sm.StartState) + ";");
                //res.Add(DoIndent() + machine.Type + "(" + machine.Name + ", 0);");
                ++count;
            }

            foreach (var execution in forkList)
            {
                res.Add(DoIndent() + execution.Name + "." + IDVar + " = " + count + ";");
                //allMachines.Add(execution.Name);

                var sm = machines.Find
                    (
                        m => m.Name == execution.Type
                    );
                res.Add(DoIndent() + execution.Name + ".state = " + CallStateForModel(sm, sm.StartState) + ";");

                ++count;
            }

            res.AddRange(WriteInitRuns(mvars, machines));

            return res;
        }

        private IEnumerable<string> WriteInitRuns(List<Options.ObjectName> mvars,
                                                  List<StateMachine> machines)
        {
            List<string> res = new List<string>();

            foreach (var machine in mvars)
            {
                res.Add(DoIndent() + "run " + machine.Name + "Proc();");
                res.Add(DoIndent() + "run " + machine.Name + "EventSource();");
            }

            foreach (var execution in forkList)
            {
                if (options.ForkSource == Options.ForkEventSource.Environment)
                {
                    res.Add(DoIndent() + "run " + execution.Name + "EventSource();");
                }
            }

            return res;
        }


        private List<MachineVariable> NameMachines()
        {
            List<MachineVariable> res = new List<MachineVariable>();
            foreach (var machine in machineSystem)
            {
                res.Add(new MachineVariable(machine, machine.Name + "1"));
            }
            return res;
        }

        private SpinPlugin owner;
        private Options options;
        private List<AutomatonExecution> forkList;

        private List<Options.ObjectName> nestedObjects = new List<Options.ObjectName>();

        /// <summary>
        /// Будем описывать автоматы при помощи встроенных в PROMELA функциий.
        /// Для этого автоматы не должны быть слишком большими.
        /// Большие автоматы - зло, мы их запретим.
        /// Вместо больших автоматов - большие системы автоматов.
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        private List<string> WriteOneMachine(StateMachine machine)
        {
            List<string> res = new List<string>();

            res.Add(DoIndent() + "inline " + machine.Name + "(" + MachineArg + ", " + EventArg + ")");
            res.Add(DoIndent() + "{");
            ++indentLevel;

            if (options.AtomicTransition)
            {
                res.Add(DoIndent() + "atomic");
                res.Add(DoIndent() + "{");
                ++indentLevel;
            }

            res.Add(DoIndent() + "byte sendEvt;");

            res.AddRange(WriteValues(machine));

            res.Add(DoIndent() + "if");
            ++indentLevel;
            res.AddRange(WriteStates(machine, machine.States));
            --indentLevel;
            res.Add(DoIndent() + "fi;");

            if (options.AtomicTransition)
            {
                --indentLevel;
                res.Add(DoIndent() + "}");
            }

            --indentLevel;
            res.Add(DoIndent() + "}");
            res.Add("");

            return res;
        }

        private IEnumerable<string> WriteValues(StateMachine machine)
        {
            List<string> res = new List<string>();
            foreach (var variable in machine.Variables)
            {
                //res.Add(DoIndent() + "printf(\"machine%d." + variable.Name + " = 
                StringBuilder sb = new StringBuilder();
                sb.Append(DoIndent());
                sb.Append("printf(\"");//machine%d.");
                sb.Append(MachineArg);
                sb.Append("%d.");
                sb.Append(variable.Name);
                sb.Append(" = ");
                var singlevar = variable as SingleVariable;
                if (singlevar != null)
                {
                    sb.Append("%d\\n\", ");
                    sb.Append(MachineArg);
                    sb.Append(".");
                    sb.Append(IDVar);
                    sb.Append(", ");

                    sb.Append(MachineArg);
                    sb.Append(".");
                    sb.Append(variable.Name);
                }

                var arr = variable as Array;
                if (arr != null)
                {
                    sb.Append("{");
                    for (int i = 0; i < arr.NElements - 1; i++)
                    {
                        sb.Append("%d, \\");
                        res.Add(sb.ToString());
                        sb = new StringBuilder();
                        ++indentLevel;
                        sb.Append(DoIndent());
                        --indentLevel;
                    }
                    if (arr.NElements > 0)
                    {
                        sb.Append("%d");
                    }
                    sb.Append("}\\n\", ");

                    sb.Append(MachineArg);
                    sb.Append(".");
                    sb.Append(IDVar);
                    sb.Append(", ");

                    for (int i = 0; i < arr.NElements; i++)
                    {
                        sb.Append(MachineArg);
                        sb.Append(".");
                        sb.Append(arr.Name);
                        sb.Append("[");
                        sb.Append(i.ToString());
                        sb.Append("]");
                        if (i < arr.NElements - 1)
                        {
                            sb.Append(", ");
                        }
                        sb.Append("\\");
                        res.Add(sb.ToString());
                        sb = new StringBuilder();
                        ++indentLevel;
                        sb.Append(DoIndent());
                        --indentLevel;
                    }
                }

                sb.Append(");");
                res.Add(sb.ToString());
            }
            return res;
        }

        private IEnumerable<string> WriteStates(StateMachine machine, List<State> list)
        {
            List<string> res = new List<string>();
            foreach (var state in list)
            {
                res.AddRange(WriteOneState(machine, state));
            }

            return res;
        }

        private IEnumerable<string> WriteOneState(StateMachine machine, State state)
        {
            List<string> res = new List<string>();
            /*
            res.Add(DoIndent() + "::(" + 
                MachineStructName(machine.Machine) + "1.state == " + 
                CallStateForModel(machine.Machine, state) + ") ->");
             */
            res.Add(DoIndent() + "::(" + MachineArg + "." + StateVar + " == " +
                CallStateForModel(machine, state) + ") ->");

            ++indentLevel;
            res.Add(DoIndent() + "printf(\"machine%d." + StateVar + " = " + machine.Name +
                "." + state.TheAttributes.Name + " \\n\", " + MachineArg + "." + IDVar + ");");

            /*
            //Выходные воздействия при входе в состояние
            res.AddRange(WriteActions(machine, state.TheAttributes.EntryActions));

            //Запуск автоматов при входе в состояние
            res.AddRange(WriteFork(state));

            res.AddRange(WriteEffects(state));
              

            //Выходные воздействия при выходе из состояния
            res.AddRange(WriteActions(machine, state.TheAttributes.ExitActions));
            */

            res.AddRange(WriteTransitions(machine, state));

            if (state.TheAttributes.Type == State.StateType.End)
            {
                res.Add(DoIndent() + "atomic");
                res.Add(DoIndent() + "{");
                ++indentLevel;
                res.Add(DoIndent() + "printf(\"machine%d finished\\n\", machine.ID);");
                res.Add(DoIndent() + "machine." + FinishedVar + " = true;");
                res.Add(DoIndent() + "machine." + StartedVar + " = false;");
                --indentLevel;
                res.Add(DoIndent() + "}");
            }
            --indentLevel;
            return res;
        }

        private IEnumerable<string> WriteEffects(State state)
        {
            List<string> res = new List<string>();

            foreach (var eff in state.TheAttributes.EntryEffects)
            {
                res.Add(DoIndent() + "sendEvt = " + eff.Event + ";");
                res.Add(DoIndent() + eff.Name + ChanSuffix + " ! sendEvt;");
            }

            return res;
        }

        private IEnumerable<string> WriteFork(State state)
        {
            List<string> res = new List<string>();

            foreach (var ex in state.TheAttributes.EntryExecutions)
            {
                res.Add(DoIndent() + "atomic");
                res.Add(DoIndent() + "{");
                ++indentLevel;
                res.Add(DoIndent() + "assert (!" + ex.Name + "." + StartedVar + "); //This machine must be inactive");
                res.Add(DoIndent() + "run " + ex.Name + "Proc();");
                --indentLevel;
                res.Add(DoIndent() + "}");
            }

            return res;
        }

        private IEnumerable<string> WriteActions(StateMachine _machine, IEnumerable<Action> actionList)
        {
            List<string> res = new List<string>();
            foreach (var action in actionList)
            {
                res.Add(DoIndent() + "atomic");
                res.Add(DoIndent() + "{");
                ++indentLevel;
                res.Add(DoIndent() + "printf(\"machine%d." + action.Name + "()\\n\", " + MachineArg + "." + 
                    IDVar + ");");
                FunctionCall call = new FunctionCall {function = action.Name, machine = _machine.Name};
                int id = -1;

                if (functionIDs.ContainsKey(call))
                {
                    id = functionIDs[call];
                }
                res.Add(DoIndent() + MachineArg + "." + FunctionVar + " = " + id + ";");
                --indentLevel;
                res.Add(DoIndent() + "}");
            }
            return res;
        }

        private IEnumerable<string> WriteTransitions(StateMachine machine, State state)
        {
            //List<Arrow> list, List<string> _machines
            List<string> res = new List<string>();
            List<Arrow> list = state.OutgoingArrows;
            res.Add(DoIndent() + "if");
            ++indentLevel;
            foreach (var arr in list)
            {
                Transition trans = arr as Transition;
                if (trans != null)
                {
                    res.AddRange(WriteOneTransition(machine, trans));
                }
            }
            res.AddRange(WriteNested(machine, state));
            --indentLevel;
            res.Add(DoIndent() + "fi;");
            return res;
        }

        private IEnumerable<string> WriteNested(StateMachine sm, State _state)
        {
            List<string> res = new List<string>();

            var _machines = _state.TheAttributes.NestedMachines;
            res.Add(DoIndent() + ":: else -> ");
            if (_machines != null)
            {
                if (_machines.Count > 0)
                {
                    ++indentLevel;
                    foreach (var machine in _machines)
                    {
                        res.Add(DoIndent() + "atomic");
                        res.Add(DoIndent() + "{");
                        ++indentLevel;
                        res.Add(DoIndent() + "printf(\"machine%d. Nested machine: " +
                            machine.Name + "\\n\", " + MachineArg + "." + IDVar + ");");
                        res.Add(DoIndent() + MachineArg + "." + NestedMachineVar + " = " + NameNestedCall(machine.Name) + ";");
                        --indentLevel;
                        res.Add(DoIndent() + "}");

                        res.Add(DoIndent() + machine.Type + "(" + machine.Name + ", " + EventArg + ");");
                        Options.ObjectName no = new Options.ObjectName();
                        no.Type = machine.Type;
                        //no.Name = EnameNested(_state, machine);
                        no.Name = machine.Name;
                        nestedObjects.Add(no);
                    }
                    --indentLevel;
                }
                else
                {
                    //res.Add(WriteSkip());
                    res.AddRange(WriteReject(sm));
                }
            }
            else
            {
                //res.Add(WriteSkip());
                res.AddRange(WriteReject(sm));
            }

            return res;
        }

        private IEnumerable<string> WriteReject(StateMachine machine)
        {
            List<string> res = new List<string>();
            if (machine.AutoReject)
            {
                ++indentLevel;
                res.Add(DoIndent() + MachineArg + "." + StateVar + " = " +
                        CallStateForModel(machine.Name, RejectState) + ";");
                --indentLevel;
            }
            else
            {
                res.Add(WriteSkip());
            }
            return res;
        }

        private string WriteSkip()
        {
            return DoIndent() + "skip;";
        }

        public static string NameNestedCall(string nested)
        {
            return nested + "_call";
        }

        private IEnumerable<string> WriteOneTransition(StateMachine machine, Transition trans)
        {
            List<string> res = new List<string>();
            //res.Add(DoIndent() + ":: " + MachineStructName(machine.Machine) + "1.state = " +
            //    CallStateForModel(machine.Machine, trans.Start as State) + ";");
            //res.Add(DoIndent() + ":: machine.state = " +
            //    CallStateForModel(machine, trans.End as State) + ";");
            //res.Add(DoIndent() + "::(" + EventArg + " == " + trans.TheAttributes.TheEvent.Name + ") ->");

            StringBuilder sb = new StringBuilder();
            sb.Append(DoIndent());
            sb.Append("::((");
            sb.Append(EventArg);
            sb.Append(" == ");
            sb.Append(trans.TheAttributes.TheEvent.Name);

            if (trans.TheAttributes.Guard.Trim() != "")
            {
                sb.Append(") && (");
                //Условие на переходе
                sb.Append(ModifyVariables(machine, trans.TheAttributes.Guard));
            }
            sb.Append(")) ->");
            res.Add(sb.ToString());


            ++indentLevel;

            res.Add(DoIndent() + MachineArg + ".lock = 1;");

            res.AddRange(WriteEvent(trans.TheAttributes.TheEvent));

            res.Add(DoIndent() + MachineArg + "." + StateVar + " = " + 
                CallStateForModel(machine, trans.End as State) + ";");


            //Epsilon-переход
            res.AddRange(WriteEpsilonTransition(machine, trans.End as State));

            var start = trans.Start as State;
            if (start == null)
            {
                throw new ArgumentException("Wrong Shape class in the Statemachine!");
            }

            //Выходные воздействия при выходе из начала перехода.
            res.AddRange(WriteActions(machine, start.TheAttributes.ExitActions));

            res.AddRange(WriteExecutedCode(machine, trans));

            res.AddRange(WriteActions(machine, trans.TheAttributes.Actions));

            State state = trans.End as State;
            if (state == null)
            {
                throw new ArgumentException("Wrong Shape class in the Statemachine!");
            }
            //Выходные воздействия при входе в состояние-конец перехода.
            res.AddRange(WriteActions(machine, state.TheAttributes.EntryActions));

            //Запуск автоматов при входе в состояние-конец перехода.
            res.AddRange(WriteFork(state));

            res.AddRange(WriteEffects(state));

            res.Add(DoIndent() + MachineArg + ".lock = 0;");

            res.Add(DoIndent() + "printf(\"machine%d." + StateVar + " = " + machine.Name +
                "." + state.TheAttributes.Name + " \\n\", " + MachineArg + "." + IDVar + ");");


            --indentLevel;
            return res;
        }

        private IEnumerable<string> WriteEpsilonTransition(StateMachine machine, State s)
        {
            List<string> res = new List<string>();
            foreach (var a in s.OutgoingArrows)
            {
                var t = a as Transition;
                if (t == null)
                {
                    continue;
                }
                if (t.TheAttributes.TheEvent.Name == machine.Epsilon)
                {
                    //
                    //res.Add(DoIndent() + machine.Name + "(" + MachineArg + ", " + machine.Epsilon + ");");
                }
            }
            return res;
        }

        private IEnumerable<string> WriteExecutedCode(StateMachine machine, Transition trans)
        {
            List<string> res = new List<string>();
            res.Add(DoIndent() + "//Code");
            foreach (var line in trans.TheAttributes.Code)
            {
                if (line.Trim() != "")
                {
                    res.Add(DoIndent() + ModifyVariables(machine, line));
                }
            }
            res.Add(DoIndent() + "//Code");
            return res;
        }

        public static string ModifyVariables(StateMachine machine, string line)
        {
            StringBuilder res = new StringBuilder();
            Tokenizer tokenizer = new Tokenizer();
            var tokens = tokenizer.ParseText(line);
            foreach (var t in tokens)
            {
                if (t.Type == TokenType.Word)
                {
                    bool find = false;
                    foreach (var @var in machine.Variables)
                    {
                        if (@var.Name == t.Value)
                        {
                            find = true;
                        }
                    }
                    if (find)
                    {
                        t.Value = "machine." + t.Value;
                    }
                }
                res.Append(t.Value);
            }
            return res.ToString();
        }

        private IEnumerable<string> WriteEvent(Event _event)
        {
            List<string> res = new List<string>();

            res.Add(DoIndent() + "atomic");
            res.Add(DoIndent() + "{");
            ++indentLevel;
            res.Add(DoIndent() + "printf(\"machine%d. event_happened: _" + _event.Name + "_ \\n\", machine.ID);");
            res.Add(DoIndent() + "machine.curEvent = " + _event.Name + ";");
            res.Add(DoIndent() + "machine.curEvent = 0;");
            --indentLevel;
            res.Add(DoIndent() + "}");

            return res;
        }

        private List<string> WriteMachines(List<StateMachine> machines)
        {
            List<string> res = new List<string>();
            foreach (var m in machines)
            {
                res.AddRange(WriteOneMachine(m));
            }

            return res;
        }

        private List<string> DefineStructures(List<StateMachine> machines)
        {
            List<string> res = new List<string>();
            foreach (var m in machines)
            {
                res.AddRange(DefineOneStructure(m));
            }

            res.AddRange(DefineDataVariables());
            return res;
        }

        private List<string> DefineOneStructure(StateMachine machine)
        {
            List<string> res = new List<string>();
            res.Add("typedef " + MachineStructName(machine));
            res.Add("{");
            indentLevel++;
            res.Add(DoIndent() + "byte state;");
            res.Add(DoIndent() + "byte " + EventVar + ";");
            res.Add(DoIndent() + "byte ID;");
            res.Add(DoIndent() + "byte " + FunctionVar + ";");
            res.Add(DoIndent() + "byte " + NestedMachineVar + ";");
            res.Add(DoIndent() + "byte " + ForkMachineVar + ";");
            res.Add(DoIndent() + "bool " + StartedVar + ";");
            res.Add(DoIndent() + "bool " + FinishedVar + ";");
            res.Add(DoIndent() + "bool lock;");

            res.AddRange(DefineVariables(machine));

            indentLevel--;
            res.Add("}");
            res.Add("");
            return res;
        }

        private List<string> DefineVariables(StateMachine machine)
        {
            List<string> res = new List<string>();

            foreach (var v in machine.Variables)
            {
                res.AddRange(DefineOneVariable(v));
            }

            return res;
        }

        private List<string> DefineOneVariable(Variable v)
        {
            List<string> res = new List<string>();

            string r = DoIndent();
            switch (v.Type)
            {
                case Variable.TypeList.Bool:
                    r += "bool";
                    break;
                case Variable.TypeList.Int8:
                    r += "byte";
                    break;
                case Variable.TypeList.Int16:
                    r = "short";
                    break;
                case Variable.TypeList.Int32:
                    r += "int";
                    break;
            }

            r += " " + v.Name;

            if (v.GetType() == typeof(SingleVariable))
            {
                var sv = v as SingleVariable;
                r += " = " + sv.Value;
            }
            else if (v.GetType() == typeof(StaterV.StateMachine.Array))
            {
                var arr = v as StaterV.StateMachine.Array;
                r += "[" + arr.NElements + "]";
            }

            r += ";";

            res.Add(r);
            return res;
        }

        private static string MachineStructName(StateMachine machine)
        {
            //return machine.Name + "Data";
            return MachineStructName(machine.Name);
        }

        public static string MachineStructName(string machine)
        {
            return machine + "Data";
        }

        private List<string> DefineDataVariables()
        {
            List<string> res = new List<string>();
            
            var objects = new List<AutomatonExecution>();
            Dictionary<string, int> counts = new Dictionary<string, int>();
            if (options.VerifySystem)
            {
                objects.AddRange(owner.Params.pm.Info.MachineObjects.Keys.ToList());
                counts = CountObjects(objects);

                if (options.FillLonelyAutomata)
                {
                    counts = FillLonelyAutomata(counts);
                }
            }

            //
            return res;
        }

        private Dictionary<string, int> FillLonelyAutomata(Dictionary<string, int> _counts)
        {
            var res = new Dictionary<string, int>(_counts);

            return res;
        }

        private Dictionary<string, int> CountObjects(List<AutomatonExecution> executions)
        {
            Dictionary<string, int> res = FillCountsWithTypes();
            foreach (var obj in executions)
            {
                if (!res.ContainsKey(obj.Type))
                {
                    res.Add(obj.Type, 0);
                }
                res[obj.Type]++;
            }
            return res;
        }

        private Dictionary<string, int>FillCountsWithTypes()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach (var machine in owner.Params.pm.Info.Machines)
            {
                res.Add(machine.Name, 0);
            }
            return res;
        }

        private List<string> DefineValues()
        {
            List<string> res = new List<string>();
            res.AddRange(DefineAllStates());
            res.AddRange(DefineAllEvents());
            res.AddRange(DefineAllFunctions());
            res.AddRange(DefineAllNested());
            return res;
        }

        private Dictionary<NestedMachineCall, int> nestedIDs;
        private int nestedCount = 0;

        private IEnumerable<string> DefineAllNested()
        {
            List<string> res = new List<string>();

            nestedIDs = new Dictionary<NestedMachineCall, int>();
            nestedCount = 1;
            foreach (var machine in machineSystem)
            {
                res.AddRange(DefineOneMachineNested(machine));
            }

            return res;
        }

        private IEnumerable<string> DefineOneMachineNested(StateMachine machine)
        {
            List<string> res = new List<string>();

            foreach (var state in machine.States)
            {
                res.AddRange(DefineOneStateNested(machine.Name, state));
            }

            return res;
        }

        private IEnumerable<string> DefineOneStateNested(string machineName, State state)
        {
            List<string> res = new List<string>();

            if (state.TheAttributes.NestedMachines != null)
            {
                foreach (var nestedMachine in state.TheAttributes.NestedMachines)
                {
                    NestedMachineCall call = new NestedMachineCall {machine = machineName, nested = nestedMachine.Name};
                    if (!nestedIDs.ContainsKey(call))
                    {
                        nestedIDs.Add(call, nestedCount);
                        res.Add(DoIndent() + "#define " + NameNestedCall(nestedMachine.Name) + " " + nestedCount);
                        ++nestedCount;
                    }
                }
            }

            return res;
        }


        private Dictionary<FunctionCall, int> functionIDs;
        private int functionCount = 0;

        /// <summary>
        /// Enumerate all functions ("Actions") in the statemachine system.
        /// </summary>
        /// <returns>Part of model with defined functions.</returns>
        private IEnumerable<string> DefineAllFunctions()
        {
            List<string> res = new List<string>();
            functionIDs = new Dictionary<FunctionCall, int>();
            functionCount = 1;
            foreach (var machine in machineSystem)
            {
                res.AddRange(DefineOneMachineFunctions(machine));
            }
            return res;
        }

        private IEnumerable<string> DefineOneMachineFunctions(StateMachine machine)
        {
            List<string> res = new List<string>();
            foreach (var state in machine.States)
            {
                res.AddRange(DefineOneStateFunctions(machine.Name, state));
            }

            foreach (var transition in machine.Transitions)
            {
                res.AddRange(DefineOneTransitionFunctions(machine.Name, transition));
                
            }
            return res;
        }

        private IEnumerable<string> DefineOneTransitionFunctions(string machineName, Transition transition)
        {
            List<string> res = new List<string>();
            res.AddRange(DefineFunctionsFromList(machineName, transition.TheAttributes.Actions));
            return res;
        }

        private IEnumerable<string> DefineOneStateFunctions(string machineName, State state)
        {
            List<string> res = new List<string>();
            res.AddRange(DefineFunctionsFromList(machineName, state.TheAttributes.EntryActions));
            res.AddRange(DefineFunctionsFromList(machineName, state.TheAttributes.ExitActions));
            return res;
        }

        private List<string> DefineFunctionsFromList(string machineName, IEnumerable<Action> actionList)
        {
            List<string> res = new List<string>();
            if (actionList == null)
            {
                return res;
            }
            foreach (var action in actionList)
            {
                FunctionCall call = new FunctionCall { function = action.Name, machine = machineName };
                if (!functionIDs.ContainsKey(call))
                {
                    res.Add(DoIndent() + "#define " + FunctionTextID(call.machine, call.function) + " " + functionCount);
                    functionIDs.Add(call, functionCount++);
                }
            }
            return res;
        }

        public static string FunctionTextID(string machine, string function)
        {
            return machine + "_" + function;
        }

        private List<List<string>> definedStates;
        private List<string> DefineAllStates()
        {
            definedStates = new List<List<string>>();
            List<string> res = new List<string>();
            foreach (var machine in machineSystem)
            {
                if (!ButtonPlugin.IsNameAcceptable(machine.Name))
                {
                    throw new InvalidNameException(machine.Name);
                }
                res.AddRange(DefineOneMachineStates(machine));
            }
            return res;
        }

        private List<string> DefineOneMachineStates(StateMachine machine)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < machine.States.Count; i++)
            {
                if (!SpinPlugin.IsNameAcceptable(machine.States[i].TheAttributes.Name))
                {
                    throw new InvalidNameException(machine.States[i].TheAttributes.Name);
                }

                res.Add(CreateStateDefinition(machine, machine.States[i], i));
            }
            if (machine.AutoReject)
            {
                res.Add("#define " + CallStateForModel(machine.Name, RejectState) + " " + machine.States.Count);
            }
            res.Add("");
            return res;
        }

        private string CreateStateDefinition(StateMachine machine, State state, int number)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append("#define ");
            sb.Append(CallStateForModel(machine, state));
            sb.Append(" ");
            sb.Append(number);
            return sb.ToString();
        }

        private static string CallStateForModel(StateMachine machine, State state)
        {
            //return machine.Name + "_" + state.TheAttributes.Name;
            return CallStateForModel(machine.Name, state.TheAttributes.Name);
        }

        public static string CallStateForModel(string machine, string state)
        {
            return machine + "_" + state;
        }

        //private Dictionary<string, int> eventList;
        private List<string> DefineAllEvents()
        {
            ExtractEvents();

            List<string> res = new List<string>();
            //eventList = new Dictionary<string, int>();

            int count = 1;
            Dictionary <string, int> newList = new Dictionary<string, int>();
            foreach (var evt in eventList)
            {
                newList[evt.Key] = count;
                res.Add("#define " + evt.Key + " " + count);
                ++count;
            }

            eventList = newList;

            /*
            foreach (var machine in machineSystem)
            {
                //res.AddRange(DefineMachineEvents(machine));
            }
            */
            res.Add("");
            return res;
        }

        private Dictionary<string, int> eventList; 
        private void ExtractEvents()
        {
            eventList = new Dictionary<string, int>();

            foreach (var machine in machineSystem)
            {
                ExtactEventsFromMachine(machine);
            }
        }

        private void ExtactEventsFromMachine(StateMachine machine)
        {
            machine.ConvertEpsilon();
            foreach (var @event in machine.Events)
            {
                if (!ButtonPlugin.IsNameAcceptable(@event.Name))
                {
                    throw new InvalidNameException("event: " + @event.Name);
                }

                if (!eventList.ContainsKey(@event.Name))
                {
                    eventList.Add(@event.Name, 0);
                }
            }
        }

        /*
        private List<string> DefineMachineEvents(StateMachine machine)
        {
            List<string> res = new List<string>();

            for (int i = 0; i < machine.Events.Count; ++i)
            {
                var evt = machine.Events[i];
                if (!SpinPlugin.IsNameAcceptable(evt.Name))
                {
                    throw new InvalidNameException("event: " + evt.Name);
                }

                if (!eventList.ContainsKey(evt.Name))
                {
                    res.Add("#define " + evt.Name + " " + i);
                    eventList.Add(evt.Name, i);
                }
            }
            return res;
        }
        */

        private int indentLevel;
        private string DoIndent()
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < indentLevel; i++)
            {
                sb.Append("\t");
            }
            return sb.ToString();
        }

        private List<StateMachine> machineSystem;

        private string resultMessage;
        public string ResultMessage
        {
            get { return resultMessage; }
        }
    }
}
