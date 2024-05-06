using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Commands
{
    public class EditEventParams : CommandParams
    {
        public Project.ProjectManager Project { get; set; }
        public Attributes.Event OldEvent { get; set; }
        public Attributes.Event NewEvent { get; set; }
    }
}
