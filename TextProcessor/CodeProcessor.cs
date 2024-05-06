using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessor
{
    public class CodeProcessor
    {
        public CodeProcessor()
        {
            IncludeMarkStart = null;
        }

        public enum Existance
        {
            Unknown,
            Exist,
            NotExist,
        }

        public string IncludeMarkStart { get; set; }
        public string IncludeMarkEnd { get; set; }
        public Existance IncludeExist { get; private set; }

        public string FunctionsMarkStart { get; set; }
        public string FunctionsMarkEnd { get; set; }
        public Existance FunctionsExist { get; set; }

        public List<string> AutomataClassDeclaration { get; set; }
        public Existance ClassDeclarationExist { get; set; }

        public Existance NamespaceExist { get; set; }

        public string[] Text { get; set; }

        public List<string> Actions { get; set; }

        public string LastFoundAction
        {
            get { return lastFoundAction; }
        }

        private int lineCount = 0;

        public enum CodeState
        {
            PlainLine,
            Namespace,
            TargetClass,
            GenFunctionsMarkStart,
            GenFunctionsMarkEnd,
            Action,
        }

        public CodeState ProcessLine(string line)
        {
            if (CheckNamespace(line))
            {
                return CodeState.Namespace;
            }

            if (ContainsClassDecl(line))
            {
                return CodeState.TargetClass;
            }

            if (ContainsMark(FunctionsMarkStart, line))
            {
                return CodeState.GenFunctionsMarkStart;
            }

            if (ContainsMark(FunctionsMarkEnd, line))
            {
                return CodeState.GenFunctionsMarkEnd;
            }

            if (ContainsAction(line))
            {
                return CodeState.Action;
            }

            return CodeState.PlainLine;
        }

        private string lastFoundAction;

        private bool ContainsAction(string line)
        {
            foreach (var act in Actions)
            {
                if (line.IndexOf(act, System.StringComparison.Ordinal) != -1)
                {
                    lastFoundAction = act;
                    return true;
                }
            }
            return false;
        }

        private bool ContainsMark(string mark, string line)
        {
            return (line.IndexOf(mark) != -1);
        }

        private bool ContainsClassDecl(string line)
        {
            foreach (var token in AutomataClassDeclaration)
            {
                if (line.IndexOf(token) == -1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckNamespace(string line)
        {
            if (ContainsMark("namespace", line))
            {
                NamespaceExist = Existance.Exist;
                return true;
            }

            return false;
        }

        public List<string> TextBeforeInclude()
        {
            List<string> res = new List<string>();
            //foreach (var line in Text)
            for (int i = 0; i < Text.Length; ++i)
            {
                var line = Text[i];
                lineCount = i;
                if (ContainsMark(IncludeMarkStart, line))
                {
                    IncludeExist = Existance.Exist;
                    break;
                }

                if (ContainsMark(IncludeMarkEnd, line))
                {
                    IncludeExist = Existance.NotExist; //?
                    break;
                }

                if (ContainsClassDecl(line))
                {
                    IncludeExist = Existance.NotExist;
                    ClassDeclarationExist = Existance.Exist;
                    break;
                }

                if (ContainsMark(FunctionsMarkStart, line))
                {
                    //Error!!!
                    IncludeExist = Existance.NotExist;
                    ClassDeclarationExist = Existance.NotExist;
                    FunctionsExist = Existance.Exist;
                    break;
                }

                if (ContainsMark(FunctionsMarkEnd, line))
                {
                    //Error!!!
                    IncludeExist = Existance.NotExist;
                    ClassDeclarationExist = Existance.NotExist;
                    FunctionsExist = Existance.NotExist;
                    break;
                }

                res.Add(line);

                CheckNamespace(line);
            }
            return res;
        }

        public List<string> TextBeforeClassDef()
        {
            var res = new List<string>();

            bool found = false;
            while ((lineCount < Text.Length) && (!found))
            {
                if (ContainsClassDecl(Text[lineCount]))
                {
                    ClassDeclarationExist = Existance.Exist;
                    found = true;
                }
            }

            return res;
        }
    }
}
