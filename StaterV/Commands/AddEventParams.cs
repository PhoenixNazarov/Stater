using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Commands
{
    public class AddEventParams : CommandParams
    {
        public string Name { get; set; }
        public string Comment { get; set; }

    }
}
