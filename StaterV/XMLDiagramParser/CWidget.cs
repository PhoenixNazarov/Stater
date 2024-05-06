using System;
using XMLDiagramParser;

namespace StaterV.XMLDiagramParser
{
    class CWidget : AWidget
    {
        public CWidget()
        {
            AttributesParser = new CAttributes();
            ((CAttributes)AttributesParser).Owner = this;
        }

        public CParser Owner { get; set; }
        /// <summary>
        ///
        /// </summary>
        public override void SetWidgetId()
        {
            Owner.CurID = Int64.Parse(Owner.Owner.CurValue);
        }

        /// <summary>
        ///
        /// </summary>
        public override void ConstructWidget()
        {
            switch (Owner.Owner.CurValue)
            {
                case "State":
                    Owner.CurWidget = new Widgets.State(Owner.Window) {ID = Owner.CurID};
                    Owner.Window.AddWidget(Owner.CurWidget);
                    break;
                case "Transition":
                    break;
            }
        }
    }
}
