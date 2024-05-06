using System;
using System.Collections.Generic;
using System.Text;
using StaterV.StateMachine;
using TextProcessor;

namespace StaterV.Variables
{
    public class EditVariablesLogic
    {
        public EditVariablesLogic()
        {
            wrapper.Logic = this;
        }

        private EditVariablesWrapper wrapper = new EditVariablesWrapper();

        public List<Variable> Variables { get; set; }

        private string CreateVariablesString()
        {
            StringBuilder sb = new StringBuilder();

            if (Variables != null)
            {
                foreach (var v in Variables)
                {
                    sb.AppendLine(v.ToString());
                    //sb.Append("\n");
                }
            }
            return sb.ToString();
        }

        private void ParseInput()
        {
            var lines = wrapper.Variables.Split('\n');
            Variables = new List<Variable>(lines.Length);
            foreach (var l in lines)
            {
                if (l.Trim() != "")
                {
                    var v = ParseLine(l);
                    Variables.Add(v);
                    
                }
            }
        }

        /// <summary>
        /// Разбирает строку описания переменной.
        /// </summary>
        /// <param name="line">
        /// Допустимые варианты:
        /// type name; Пример: int8 x;
        /// type name = val; Пример: int16 y = 5;
        /// type name[count]; Пример: bool flags[5];
        /// Слова и скобки могут быть окружены любым количеством пробельных символов.
        /// </param>
        /// <returns></returns>
        public static Variable ParseLine(string line)
        {
            Variable res;
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.ParseText(line);

            int iType = 0;
            int iName = 1;
            int iExternal = 0;
            int iParam = 0;
            int iVolatile = 0;
            int iOpenSB = 2;
            int iIndex = 3;
            int iAssign = 2;
            int iVal = 3;

            bool external = false, @volatile = false, param = false;

            if (tokens[iExternal].Type == TokenType.Word)
            {
                if (tokens[iExternal].Value.Trim() == "external")
                {
                    external = true;
                    ++iParam;
                    ++iVolatile;
                    ++iType;
                    ++iName;
                    ++iOpenSB;
                    ++iIndex;
                    ++iAssign;
                    ++iVal;
                }
            }

            if (tokens[iParam].Type == TokenType.Word)
            {
                if (tokens[iParam].Value.Trim() == "param")
                {
                    param = true;
                    ++iVolatile;
                    ++iType;
                    ++iName;
                    ++iOpenSB;
                    ++iIndex;
                    ++iAssign;
                    ++iVal;
                }
            }

            if (tokens[iVolatile].Type == TokenType.Word)
            {
                if (tokens[iVolatile].Value.Trim() == "volatile")
                {
                    @volatile = true;
                    ++iType;
                    ++iName;
                    ++iOpenSB;
                    ++iIndex;
                    ++iAssign;
                    ++iVal;
                }
            }

            if (tokens.Count < 3)
            {
                throw new ArgumentException("Wrong declaration");
            }

            if (tokens[iOpenSB].Type == TokenType.OpenSqBracket)
            {
                res = new StateMachine.Array();
            }
            else
            {
                res = new SingleVariable();
            }

            res.External = external;
            res.Volatile = @volatile;
            res.Param = param;

            if (tokens[iType].Type == TokenType.Word)
            {
                res.SetType(tokens[iType].Value);
            }
            else
            {
                throw new ArgumentException("Wrong declaration");
            }

            if (tokens[iName].Type == TokenType.Word)
            {
                res.Name = tokens[iName].Value;
            }
            else
            {
                throw new ArgumentException("Wrong declaration");
            }

            if (tokens[iOpenSB].Type == TokenType.OpenSqBracket)  
            {
                if (tokens[iIndex].Type == TokenType.IntegerNumber)
                {
                    ((StateMachine.Array) res).NElements = int.Parse(tokens[iIndex].Value);
                }
                else
                {
                    throw new ArgumentException("Wrong declaration");
                }
            }
            else
            {
                if (tokens[iAssign].Type == TokenType.Assign)
                {
                    if ((tokens[iVal].Type == TokenType.IntegerNumber) ||
                        (tokens[iVal].Type == TokenType.Word)) //Тут может быть не только число, но и true/false.
                    {
                        ((SingleVariable) res).Value = tokens[iVal].Value;
                    }
                    else
                    {
                        throw new ArgumentException("Wrong declaration");
                    }
                }
                else if (tokens[iAssign].Type == TokenType.Semicolon)
                {
                    if (res.Type == Variable.TypeList.Bool)
                    {
                        ((SingleVariable)res).Value = "false";
                    }
                    else
                    {
                        ((SingleVariable)res).Value = "0";
                    }
                }
            }
            return res;
        }

        public ResultDialog Start()
        {
            wrapper.Variables = CreateVariablesString();

            var res = LaunchForm();

            return res;
        }

        private ResultDialog LaunchForm()
        {
            var res = wrapper.Start();

            if (res == ResultDialog.OK)
            {
                try
                {
                    ParseInput();
                }
                catch (Exception e)
                {
                    var r = wrapper.ReportError(e.ToString());
                    if (r == ResultDialog.Cancel)
                    {
                        return LaunchForm();
                    }
                }
            }

            return res;
        }
    }
}
