using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMultyThread
{
    class OldFileParser
    {
        public OldFileParser()
        {}

        private string[] codeLines;

        public void LoadText(string[] text)
        {
            codeLines = new string[text.Length];
            text.CopyTo(codeLines, 0);
        }

        public void Go()
        {
            if (codeLines == null)
            {
                return;
            }
            List<string> newFile = new List<string>();
            for (int i = 0; i < codeLines.Length; i++)
            {
                
            }
        }
    }
}
