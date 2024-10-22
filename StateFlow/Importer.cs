using StateFlow.SFDParser;
using PluginData;
using TextProcessor;

namespace StateFlow
{
    public class Importer
    {
        public Importer(string input, string output)
        {
            InputPath = input;
            OutputFolder = output;

            parser = new Parser(this);
        }

        public string InputPath { get; set; }
        public string OutputFolder { get; set; }

        public List<StateMachine> LoadedFSMs
        {
            get { return parser.LoadedFSMs; }
        }

        private Parser parser;
        public string CurText { get; private set; }

        public void Import()
        {
            StreamReader sr = new StreamReader(InputPath);
            var file = sr.ReadToEnd();

            Tokenizer t = new Tokenizer();
            var tokenList = t.ParseText(file);

            foreach (var token in tokenList)
            {
                CurText = token.Value;
                switch (token.Type)
                {
                    case TokenType.Word:
                        //parser.ProcessEvent(Events.word);
                        parser.ProcessEvent(CurText);
                        break;
                    case TokenType.IntegerNumber:
                    case TokenType.FloatingNumber:
                        parser.ProcessEvent(Events.number);
                        break;
                    case TokenType.TextMess:
                        parser.ProcessEvent("*");
                        break;
                    case TokenType.OpenCurlBracket:
                        parser.ProcessEvent(Events.open_curl_br);
                        break;
                    case TokenType.CloseCurlBracket:
                        parser.ProcessEvent(Events.close_curl_br);
                        break;
                    case TokenType.Quote:
                        parser.ProcessEvent(Events.quote);
                        break;
                    case TokenType.Assign:
                    case TokenType.Equals:
                    case TokenType.Less:
                    case TokenType.LessEqual:
                    case TokenType.Greater:
                    case TokenType.GreaterEqual:
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Semicolon:
                    case TokenType.Dot:
                    case TokenType.OpenSqBracket:
                    case TokenType.CloseSqBracket:
                    case TokenType.VerticalLine:
                    default:
                        parser.ProcessEvent("*");
                        break;
                }
            }
        }

    }
}
