using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessor
{
    public class Token
    {
        public Token(TokenType t)
        {
            Value = "";
            Type = t;
        }

        public Token(TokenType t, string v)
        {
            Value = v;
            Type = t;
        }

        public Token(TokenType t, char v)
        {
            Value = v.ToString();
            Type = t;
        }

        public string Value { get; set; }
        public TokenType Type { get; set; }

        public override string ToString()
        {
            return Type.ToString() + " - " + Value;
        }
    }
}
