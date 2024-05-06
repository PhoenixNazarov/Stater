using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessor
{
    public class Tokenizer
    {
        public List<Token> TokenList { get; private set; }

        public enum SymbolType
        {
            Letter, // a-z, A-Z
            Digit, // 0-9
            Space,
            Equal, // =
            //Greater, // >
            //Less, // <
            Dot, // .
            //Comma, // ,
            //Colon, // :
            Semicolon, // ;
            OpenSqBracket, // [
            CloseSqBracket, // ]
            OpenCurlBracket, // {
            CloseCurlBracket, // }
            OpenBracket, // (
            CloseBracket, // )
            Not, // !
            Other, // other
        }

        public List<Token> ParseText(string text)
        {
            TokenList = new List<Token>();
            Token curToken = null;
            for (int i = 0; i < text.Length; ++i)
            {
                var st = GetSymbolType(text[i]);
                switch (st)
                {
                    case SymbolType.Letter:
                        // TODO: extract this to a separate function.
                        if (curToken == null)
                        {
                            curToken = new Token(TokenType.Word, text[i]);
                        }
                        else
                        {
                            switch (curToken.Type)
                            {
                                case TokenType.Word:
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.IntegerNumber:
                                case TokenType.FloatingNumber:
                                    curToken.Type = TokenType.TextMess;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.TextMess:
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Assign:
                                case TokenType.Equals:
                                case TokenType.Semicolon:
                                case TokenType.Greater:
                                case TokenType.Less:
                                case TokenType.GreaterEqual:
                                case TokenType.LessEqual:
                                case TokenType.OpenSqBracket:
                                case TokenType.CloseSqBracket:
                                    //curToken.Value += text[i];
                                    TokenList.Add(curToken);
                                    curToken = new Token(TokenType.Word, text[i]);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        break;
                    case SymbolType.Digit:
                        if (curToken == null)
                        {
                            curToken = new Token(TokenType.IntegerNumber, text[i]);
                        }
                        else
                        {
                            switch (curToken.Type)
                            {
                                case TokenType.Word:
                                case TokenType.IntegerNumber:
                                case TokenType.FloatingNumber:
                                case TokenType.TextMess:
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Assign:
                                case TokenType.Equals:
                                case TokenType.Semicolon:
                                case TokenType.OpenSqBracket:
                                case TokenType.CloseSqBracket:
                                    TokenList.Add(curToken);
                                    curToken = new Token(TokenType.Word, text[i]);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        break;
                    case SymbolType.Space:
                        if (curToken != null)
                        {
                            TokenList.Add(curToken);
                            curToken = null;
                        }
                        break;
                    case SymbolType.Equal:
                        if (curToken == null)
                        {
                            curToken = new Token(TokenType.Assign, text[i]);
                        }
                        else
                        {
                            switch (curToken.Type)
                            {
                                case TokenType.TextMess:
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Assign:
                                    curToken.Type = TokenType.Equals;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Equals:
                                    curToken.Type = TokenType.TextMess;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Greater:
                                    curToken.Type = TokenType.GreaterEqual;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Less:
                                    curToken.Type = TokenType.LessEqual;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.GreaterEqual:
                                case TokenType.LessEqual:
                                    curToken.Type = TokenType.TextMess;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Semicolon:
                                case TokenType.OpenSqBracket:
                                case TokenType.CloseSqBracket:
                                case TokenType.Word:
                                case TokenType.IntegerNumber:
                                case TokenType.FloatingNumber:
                                    TokenList.Add(curToken);
                                    curToken = new Token(TokenType.Assign, "=");
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        break;
                    case SymbolType.Dot:
                        if (curToken == null)
                        {
                            TokenList.Add(new Token(TokenType.Dot, "."));
                        }
                        else
                        {
                            switch (curToken.Type)
                            {
                                case TokenType.IntegerNumber:
                                    curToken.Type = TokenType.FloatingNumber;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.FloatingNumber:
                                    curToken.Type = TokenType.TextMess;
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.TextMess:
                                    curToken.Value += text[i];
                                    break;
                                case TokenType.Assign:
                                case TokenType.Equals:
                                case TokenType.Semicolon:
                                case TokenType.OpenSqBracket:
                                case TokenType.CloseSqBracket:
                                case TokenType.Word:
                                    TokenList.Add(curToken);
                                    TokenList.Add(new Token(TokenType.Dot, "."));
                                    curToken = null;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        break;
                    case SymbolType.Semicolon:
                        TryAddCurToken(curToken); 
                        TokenList.Add(new Token(TokenType.Semicolon, ";"));
                        curToken = null;
                        break;
                    case SymbolType.OpenSqBracket:
                        TryAddCurToken(curToken); 
                        TokenList.Add(new Token(TokenType.OpenSqBracket, "["));
                        curToken = null;
                        break;
                    case SymbolType.CloseSqBracket:
                        TryAddCurToken(curToken); 
                        TokenList.Add(new Token(TokenType.CloseSqBracket, "]"));
                        curToken = null;
                        break;
                    case SymbolType.OpenCurlBracket:
                        TryAddCurToken(curToken); 
                        TokenList.Add(new Token(TokenType.OpenCurlBracket, "{"));
                        curToken = null;
                        break;
                    case SymbolType.CloseCurlBracket:
                        TryAddCurToken(curToken); 
                        TokenList.Add(new Token(TokenType.CloseCurlBracket, "}"));
                        curToken = null;
                        break;
                    case SymbolType.OpenBracket:
                        TryAddCurToken(curToken);
                        TokenList.Add(new Token(TokenType.OpenBracket, "("));
                        curToken = null;
                        break;
                    case SymbolType.CloseBracket:
                        TryAddCurToken(curToken);
                        TokenList.Add(new Token(TokenType.CloseBracket, ")"));
                        curToken = null;
                        break;
                    case SymbolType.Not:
                        TryAddCurToken(curToken);
                        TokenList.Add(new Token(TokenType.Not, "!"));
                        curToken = null;
                        break;
                    case SymbolType.Other:
                        if (text[i] == '|')
                        {
                            TryAddCurToken(curToken);
                            TokenList.Add(new Token(TokenType.VerticalLine, '|'));
                            curToken = null;
                        }
                        else if (text[i] == '-')
                        {
                            TryAddCurToken(curToken);
                            TokenList.Add(new Token(TokenType.Minus, '-'));
                            curToken = null;
                        }
                        else if (text[i] == '+')
                        {
                            TryAddCurToken(curToken);
                            TokenList.Add(new Token(TokenType.Plus, '+'));
                            curToken = null;
                        }
                        else if (text[i] == '>')
                        {
                            TryAddCurToken(curToken); 
                            curToken = new Token(TokenType.Greater, '>');
                            //TokenList.Add(new Token(TokenType.Greater, '>'));
                            //curToken = null;
                        }
                        else if (text[i] == '\"')
                        {
                            TryAddCurToken(curToken);
                            TokenList.Add(new Token(TokenType.Quote, '\"'));
                            curToken = null;
                        }
                        else if (text[i] == '_')
                        {
                            // TODO use a separate functions, same with case of letter.
                            if (curToken == null)
                            {
                                curToken = new Token(TokenType.Word, text[i]);
                            }
                            else
                            {
                                switch (curToken.Type)
                                {
                                    case TokenType.Word:
                                        curToken.Value += text[i];
                                        break;
                                    case TokenType.IntegerNumber:
                                    case TokenType.FloatingNumber:
                                        curToken.Type = TokenType.TextMess;
                                        curToken.Value += text[i];
                                        break;
                                    case TokenType.TextMess:
                                        curToken.Value += text[i];
                                        break;
                                    case TokenType.Assign:
                                    case TokenType.Equals:
                                    case TokenType.Semicolon:
                                    case TokenType.Greater:
                                    case TokenType.Less:
                                    case TokenType.GreaterEqual:
                                    case TokenType.LessEqual:
                                    case TokenType.OpenSqBracket:
                                    case TokenType.CloseSqBracket:
                                        //curToken.Value += text[i];
                                        TokenList.Add(curToken);
                                        curToken = new Token(TokenType.Word, text[i]);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                        else if (text[i] == '<')
                        {
                            TryAddCurToken(curToken);
                            curToken = new Token(TokenType.Less, '<');
                            //TokenList.Add(new Token(TokenType.Less));
                            //curToken = null;
                        }
                        else
                        {
                            if (curToken == null)
                            {
                                curToken = new Token(TokenType.TextMess, text[i]);
                            }
                            else
                            {
                                curToken.Type = TokenType.TextMess;
                                curToken.Value += text[i];
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            TryAddCurToken(curToken); 
            return TokenList;
        }

        private void TryAddCurToken(Token curToken)
        {
            if (curToken != null)
            {
                TokenList.Add(curToken);
            }
        }

        private SymbolType GetSymbolType(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
            {
                return SymbolType.Letter;
            }

            if (ch >= '0' && ch <= '9')
            {
                return SymbolType.Digit;
            }

            if (ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r')
            {
                return SymbolType.Space;
            }

            if (ch == '=')
            {
                return SymbolType.Equal;
            }

            if (ch == '.')
            {
                return SymbolType.Dot;
            }

            /*
            if (ch == '>')
            {
                return SymbolType.Greater;
            }

            if (ch == '<')
            {
                return SymbolType.Less;
            }

            if (ch == ',')
            {
                return SymbolType.Comma;
            }

            if (ch == ':')
            {
                return SymbolType.Colon;
            }
            */

            if (ch == ';')
            {
                return SymbolType.Semicolon;
            }

            if (ch == '[')
            {
                return SymbolType.OpenSqBracket;
            }

            if (ch == ']')
            {
                return SymbolType.CloseSqBracket;
            }

            if (ch == '{')
            {
                return SymbolType.OpenCurlBracket;
            }

            if (ch == '}')
            {
                return SymbolType.CloseCurlBracket;
            }

            if (ch == '(')
            {
                return SymbolType.OpenBracket;
            }

            if (ch == ')')
            {
                return SymbolType.CloseBracket;
            }

            if (ch == '!')
            {
                return SymbolType.Not;
            }

            return SymbolType.Other;
        }

        public static bool IsSmallEngLetter(char ch)
        {
            return (ch >= 'a') && (ch <= 'z');
        }

    }
}
