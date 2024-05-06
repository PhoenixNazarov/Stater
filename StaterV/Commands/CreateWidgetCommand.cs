using System;
using StaterV.Widgets;

namespace StaterV.Commands
{
    class CreateWidgetCommand : Command
    {
        public CreateWidgetCommand(CreateWidgetParams aParams)
            : base(aParams)
        {
        }

        public override void Execute()
        {
            base.Execute();
            /*
            var state = new State(theParams.Window);
            state.X = theParams.X;
            state.Y = theParams.Y;
            theParams.Window.AddWidget(state);
             */
        }

        public override void Unexecute()
        {
            throw new NotImplementedException();
        }
    }

    class CreateStateCommand : CreateWidgetCommand
    {
        public CreateStateCommand(CreateWidgetParams aParams) : base(aParams)
        {
        }

        public override void Execute()
        {
            base.Execute();
            var state = new State(theParams.Window);
            state.X = theParams.X;
            state.Y = theParams.Y;
            state.Width = 80;
            state.Height = 60;

            //Если новое состояние пересекается с чем-нибудь, не будем его создавать.
            foreach (var w in theParams.Window.Widgets)
            {
                if (w.IntersectsWith(state.X, state.Y, state.Height, state.Width))
                {
                    return;
                }
            }
            widget = state;
            theParams.Window.AddWidget(state);
        }

        public override void Unexecute()
        {
            theParams.Window.RemoveWidget(widget);
            //throw new NotImplementedException();
        }
    }

    class CreateArrowCommand : CreateWidgetCommand
    {
        public CreateArrowCommand(CreateWidgetParams aParams)
            : base(aParams)
        {
        }

        private Shape startShape;
        private Shape endShape;

        public override void Execute()
        {
            base.Execute();
            Arrow arrow;
            switch (theParams.Type)
            {
                case WidgetType.Transition:
                    arrow = new Transition(theParams.Window);
                    break;
                default:
                    throw (new ArgumentException("Invalid type", "theParams.Type"));
            }

            startShape = (Shape)theParams.Window.ActionList[1].Widgets[0];
            endShape = (Shape)theParams.Window.ActionList[2].Widgets[0];

            startShape.OutgoingArrows.Add(arrow);
            endShape.IncomingArrows.Add(arrow);

            arrow.Start = (Shape) theParams.Window.ActionList[1].Widgets[0];
            arrow.End = (Shape) theParams.Window.ActionList[2].Widgets[0];
            theParams.Window.AddWidget(arrow);
            widget = arrow;
            /*
            var state = new State(theParams.Window);
            state.X = theParams.X;
            state.Y = theParams.Y;
            theParams.Window.AddWidget(state);
             */
        }

        public override void Unexecute()
        {
            //Удалить стрелку из начала.
            startShape.OutgoingArrows.Remove((Arrow) widget);
            //Удалить стрелку из конца.
            endShape.IncomingArrows.Remove((Arrow) widget);
            theParams.Window.RemoveWidget(widget);
        }
    }
}
