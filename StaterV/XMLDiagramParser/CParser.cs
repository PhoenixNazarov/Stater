using System;
using StaterV.Attributes;
using StaterV.Variables;
using StaterV.Windows;
using XMLDiagramParser;

namespace StaterV.XMLDiagramParser
{
    public class CParser : AParser
    {
        public CParser()
// ReSharper disable RedundantBaseConstructorCall
            : base()
// ReSharper restore RedundantBaseConstructorCall
        {
            EventParse = new CEvent();
            ((CEvent) EventParse).Owner = this;
            WidgetParse = new CWidget();
            ((CWidget) WidgetParse).Owner = this;
        }

        private WindowBase window;// = WindowFactory.Instance.CreateWindow();

        public WindowBase Window { get { return window; } }

        public XMLAgent Owner { get; set; }

        #region Parser params
        private Event curEvent;

        public Event CurEvent
        {
            get { return curEvent; }
            set { curEvent = value; }
        }
        private StatemachineData statemachineData = new StatemachineData();

        public Widgets.Widget CurWidget { get; set; }
        public Int64 CurID { get; set; }
        #endregion

        #region Automata methods
        /// <summary>
        ///
        /// </summary>
        public override void CreateDiagram()
        {
            window = WindowFactory.Instance.CreateWindow();
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetDiagramName()
        {
            window.DiagramName = Owner.CurValue;
        }

        /// <summary>
        ///Set WindowSata.Type to StatemachineType
        /// </summary>
        public override void SetStatemachineType()
        {
            window.TheWindowData = statemachineData;
            statemachineData.Type = Project.DiagramType.StateMachine;
        }

        public override void CreateVariable()
        {
            var v = EditVariablesLogic.ParseLine(Owner.CurValue);
            var data = window.TheWindowData as StatemachineData;
            data.Variables.Add(v);
        }

        public override void SetAutoReject()
        {
            //var ar = Boolean.Parse(Owner.CurValue);
            bool ar;
            var data = window.TheWindowData as StatemachineData;
            if (Boolean.TryParse(Owner.CurValue, out ar))
            {
                data.AutoReject = ar;
            }
            else
            {
                int aa;
                if (int.TryParse(Owner.CurValue, out aa))
                {
                    data.AutoReject = (aa == 0);
                }

                else
                {
                    data.AutoReject = false;
                }
            }
        }

        /*
        /// <summary>
        ///
        /// </summary>
        public override void SetEventName()
        {
            curEvent.Name = Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEventComment()
        {
            curEvent.Comment = Owner.CurValue;
        }
        */
        /// <summary>
        ///
        /// </summary>
        public void CreateEvent()
        {
            curEvent = new Event();
            statemachineData.Events.Add(curEvent);
        }
        
        #endregion
    }
}
