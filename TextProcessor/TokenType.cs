namespace TextProcessor
{
    public enum TokenType
    {
        Word, //asdf
        IntegerNumber, //239
        FloatingNumber, //123.4
        TextMess, //Кусок текста, например: 12w2, ====, <<<<< и т.д.
        Assign, // =
        Equals, // ==
        Less, // <
        LessEqual, // <=
        Greater, // >
        GreaterEqual, // >=
        Plus, // +
        Minus, // -
        Semicolon, // ;
        Dot, // .
        OpenSqBracket, // [
        CloseSqBracket, // ]
        OpenCurlBracket, // {
        CloseCurlBracket, // }
        OpenBracket, // (
        CloseBracket, // )
        VerticalLine, // |
        Not, // !
        Quote, // "
    }

}