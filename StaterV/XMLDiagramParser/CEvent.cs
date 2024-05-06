using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLDiagramParser;

namespace StaterV.XMLDiagramParser
{
    public class CEvent : AEvent
    {
        public CParser Owner { get; set; }

        public override void CreateEvent()
        {
            Owner.CreateEvent();
        }

        public override void SetEventName()
        {
            Owner.CurEvent.Name = Owner.Owner.CurValue;            
        }

        public override void SetEventComment()
        {
            Owner.CurEvent.Comment = Owner.Owner.CurValue;            
        }
    }
}
