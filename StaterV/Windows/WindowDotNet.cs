using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using StaterV.Attributes;
using StaterV.Commands;
using StaterV.StateMachine;
using StaterV.Widgets;

namespace StaterV.Windows
{
    [Serializable]
    public class WindowDotNet : WindowBase
    {
        #region Drawing functions
        public override void DrawRectangle(Colors color, float x, float y, float width, float height)
        {
            var curPen = PenSelector(color);
            var dc = TheControl.CreateGraphics();
            ModifyCoords(ref x, ref y);
            ModifyShift(ref width);
            ModifyShift(ref height);
            dc.DrawRectangle(curPen, x, y, width, height);
            dc.Dispose();
        }

        public override void DrawRoundedRectangle(Colors color, float x, float y, float radius, float width, float height)
        {
            var curPen = PenSelector(color);
            var dc = TheControl.CreateGraphics();
            ModifyCoords(ref x, ref y);
            ModifyShift(ref radius);
            ModifyShift(ref width);
            ModifyShift(ref height);

            //Левая сторона.
            dc.DrawLine(curPen, x, y + radius, x, y + height - radius);

            //Правая сторона.
            dc.DrawLine(curPen, x + width, y + radius, x + width, y + height - radius);

            //Верхняя сторона.
            dc.DrawLine(curPen, x + radius, y, x + width - radius, y);

            //Нижняя сторона.
            dc.DrawLine(curPen, x + radius, y + height, x + width - radius, y + height);

            //Левый верхний угол.
            dc.DrawArc(curPen, x, y, radius*2, radius*2, 180, 90);

            //Правый верхний угол.
            dc.DrawArc(curPen, x + width - radius * 2, y, radius * 2, radius * 2, 270, 90);

            //Левый нижний угол.
            dc.DrawArc(curPen, x, y + height - radius*2, radius * 2, radius * 2, 90, 90);

            //Правый нижный угол.
            dc.DrawArc(curPen, x + width - radius * 2, y + height - radius * 2, radius * 2, radius * 2, 0, 90);
        }

        public override void DrawLine(Colors color, float x1, float y1, float x2, float y2)
        {
            var curPen = PenSelector(color);
            var dc = TheControl.CreateGraphics();
            ModifyCoords(ref x1, ref y1);
            ModifyCoords(ref x2, ref y2);
            dc.DrawLine(curPen, x1, y1, x2, y2);
            dc.Dispose();
        }

        public override void DrawEllipse(Colors color, float x, float y, float width, float height)
        {
            var curPen = PenSelector(color);
            var dc = TheControl.CreateGraphics();
            ModifyCoords(ref x, ref y);
            ModifyShift(ref width);
            ModifyShift(ref height);
            dc.DrawEllipse(curPen, x, y, width, height);
            dc.Dispose();
        }

        public override void FillEllipse(Colors color, float x, float y, float width, float height)
        {
            var curBrash = BrushSelector(color);
            var dc = TheControl.CreateGraphics();
            ModifyCoords(ref x, ref y);
            ModifyShift(ref width);
            ModifyShift(ref height);
            dc.FillEllipse(curBrash, x, y, width, height);
            dc.Dispose();
        }

        public override void DrawText(Colors color, float x, float y, float size, string text)
        {
            var dc = TheControl.CreateGraphics();
            Font f = new Font("Courier new", size);
            ModifyCoords(ref x, ref y);
            ModifyShift(ref size);
            var p = new Point((int)x, (int)y);
            TextRenderer.DrawText(dc, text, f, p, ColorSelect(color));
            dc.Dispose();
        }

        public override Utility.Size MeasureText(float size, string text)
        {
            var res = new Utility.Size();

            Font f = new Font("Courier new", size);
            var dc = TheControl.CreateGraphics();

            var m = TextRenderer.MeasureText(dc, text, f);
            dc.Dispose();

            res.width = m.Width;
            res.height = m.Height;

            return res;
        }

        public override void Clear()
        {
            var dc = TheControl.CreateGraphics();
            dc.Clear(ColorSelect(Colors.Background));
        }
        #endregion

        List<string> GetMachineTypes()
        {
            List<string> res = new List<string>();
            if (MyProject != null)
            {
                if (MyProject.Info.Machines != null)
                    foreach (var machine in MyProject.Info.Machines)
                    {
                        res.Add(machine.Name);
                    }
            } 
            return res;
        }

        protected override void AddVariable(Variable v)
        {
            ((StatemachineData)TheWindowData).Variables.Add(v);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Окна~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #region Forms
        [field: NonSerialized]
        private StateAttributesForm stateAttributesForm = new StateAttributesForm();

        public Form1 Form { get; set; }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Функции вызова окон~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public override bool ShowStatePropertiesForm(State aState)
        {
            stateAttributesForm.TheAttributes = aState.TheAttributes.Clone() as StateAttributes;
            stateAttributesForm.MachineTypes = GetMachineTypes();
            stateAttributesForm.Window = this;
            if (MyProject != null)
            {
                stateAttributesForm.Machines = MyProject.ExportStateMachines();
                if (MyProject.Info.MachineObjects != null)
                {
                    stateAttributesForm.AllExecutions = new List<AutomatonExecution>(MyProject.Info.MachineObjects.Keys);
                }
                else
                {
                    stateAttributesForm.AllExecutions = new List<AutomatonExecution>();
                }
            }
            else
            {
                stateAttributesForm.AllExecutions = new List<AutomatonExecution>();
                stateAttributesForm.Machines = new List<StateMachine.StateMachine>();
            }


            var res = stateAttributesForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                //aState.TheAttributes = stateAttributesForm.TheAttributes;
                //Выполнить команду ChangeStateCommand.
                var p = new Commands.ChangeStateParams();
                p.Window = this;
                p.TheWidget = aState;
                p.NewAttributes = stateAttributesForm.TheAttributes;
                p.Project = MyProject;
                curCommand = new Commands.ChangeStateCommand(p);
                curParams = p;
                curCommand.Execute();
            }
            return (res == DialogResult.OK);
        }

        public override bool ShowTransitionPropertiesForm(Transition aTransition)
        {
            transitionAttrsLogic.Window = this; //TODO: Move this statement to the base.
            transitionAttrsLogic.Machines = MyProject.ExportStateMachines();
            transitionAttrsLogic.TheAttributes = aTransition.TheAttributes.Clone() as TransitionAttributes;
            var res = transitionAttrsLogic.Start();

            //transitionAttributesForm.TheAttributes = aTransition.TheAttributes.Clone() as TransitionAttributes;
            //var res = transitionAttributesForm.ShowDialog();
            //if (res == DialogResult.OK)
            if (res == ResultDialog.OK)
            {
                //aTransition.TheAttributes = transitionAttributesForm.TheAttributes;
                var p = new Commands.ChangeTransitionParams();
                p.Window = this;
                p.TheWidget = aTransition;
                //p.NewAttributes = transitionAttributesForm.TheAttributes.Clone() as TransitionAttributes;
                if (transitionAttrsLogic.TheAttributes != null)
                {
                    p.NewAttributes = transitionAttrsLogic.TheAttributes.Clone() as TransitionAttributes;
                }
                curCommand = new Commands.ChangeTransitionCommand(p);
                curParams = p;
                curCommand.Execute();
                return true;
            }
            return false;
        }

        public override bool ShowVariablesForm()
        {
            editVariablesLogic.Variables = new List<Variable>(((StatemachineData)TheWindowData).Variables);
            var res = editVariablesLogic.Start();
            if (res == ResultDialog.OK)
            {
                // Выполнить команду.
                ChangeVariablesParams p = new ChangeVariablesParams {NewVariables = editVariablesLogic.Variables};
                p.Window = this;
                ChangeVariablesCommand cmd = new ChangeVariablesCommand(p);
                cmd.Execute();
            }
            return false;
        }

        public override void ShowFSMContextMenu()
        {
            Form.ShowFSMMenu();
        }

        #endregion
    }
}