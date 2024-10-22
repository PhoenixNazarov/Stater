using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using PluginData;
using StaterV.Attributes;
using StaterV.Exceptions;
using StaterV.Project;
using StaterV.Utility;
using StaterV.Variables;
using StaterV.Widgets;
using StaterV.XMLDiagramParser;
using StaterV.Commands;
using AutomatonEffect=PluginData.AutomatonEffect;
using AutomatonExecution=PluginData.AutomatonExecution;
using Event=StaterV.Attributes.Event;
using NestedMachine=PluginData.NestedMachine;
using State=StaterV.Widgets.State;
using Transition=StaterV.Widgets.Transition;

namespace StaterV.Windows
{
    [Serializable]
    public class Action
    {
        public Action()
        {
            Widgets = new List<Widget>();
        }

        public Action(ActionType type)
        {
            Widgets = new List<Widget>();
            Type = type;
        }

        public enum ActionType
        {
            StartCreateState,
            StartMoveState,
            StartCreateTransition,
            StateSelected,
        }

        public ActionType Type { get; private set; }
        public List<Widget> Widgets { get; private set; }
    }

    [Serializable]
    public abstract class WindowBase
    {
        public WindowBase()
        {
            DoneCommands = new Stack<Command>();
            UndoneCommands = new Stack<Command>();
            CoordsAreSet = false;
            Scale = 1;
            HorizontalShift = 0;
            VerticalShift = 0;
        }

        #region Protected Fields
        [field: NonSerialized]
        protected Control theControl;

        [field: NonSerialized]
        protected Project.ProjectManager myProject;

        [field: NonSerialized]
        protected Pen backgroundPen = Pens.White;
        [field: NonSerialized]
        protected Brush background = Brushes.White;
        
        [field: NonSerialized]
        protected Pen foregroundPen = Pens.Black;

        [field: NonSerialized]
        protected Brush foreground = Brushes.Black;

        [field: NonSerialized]
        protected Pen activePen = Pens.Red;

        [field: NonSerialized]
        protected Pen showInfoPen = Pens.DarkBlue;

        [field: NonSerialized]
        protected List<Action> actionList = new List<Action>(3);

        [field: NonSerialized]
        protected Command curCommand;
        [field: NonSerialized]
        protected CommandParams curParams;

        #endregion

        public enum Colors
        {
            Background,
            Foreground,
            Active,
            ShowInfo,
        }

        #region Properties
        public ProjectManager MyProject
        {
            get { return myProject; }
            set { myProject = value; }
        }

        public Control TheControl
        {
            get { return theControl; }
            set { theControl = value; }
        }

        public List<Action> ActionList
        {
            get { return actionList; }
            set { actionList = value; }
        }

        public List<Widget> Widgets
        {
            get { return widgets; }
        }

        public Dot StartMoveDot
        {
            get { return startMoveDot; }
            protected set { startMoveDot = value; }
        }

        //public Project.DiagramType TheDiagramType { get; set; }
        public WindowData TheWindowData { get; set; }

        #endregion

        #region Common GUI functions
        protected Brush BrushSelector(Colors color)
        {
            Brush curBrush;
            switch (color)
            {
                case Colors.Background:
                    curBrush = background;
                    break;
                case Colors.Foreground:
                    curBrush = foreground;
                    break;
                default:
                    curBrush = foreground;
                    break;
            }
            return curBrush;
        }

        protected Pen PenSelector(Colors color)
        {
            Pen curPen;
            switch (color)
            {
                case Colors.Background:
                    curPen = backgroundPen;
                    break;
                case Colors.Foreground:
                    curPen = foregroundPen;
                    break;
                case Colors.Active:
                    curPen = activePen;
                    break;
                case Colors.ShowInfo:
                    curPen = showInfoPen;
                    break;
                default:
                    curPen = foregroundPen;
                    break;
            }
            return curPen;
        }

        protected static Color ColorSelect(Colors color)
        {
            Color res;
            switch (color)
            {
                case Colors.Background:
                    res = Color.White;
                    break;
                case Colors.Foreground:
                    res = Color.Black;
                    break;
                case Colors.Active:
                    res = Color.Red;
                    break;
                default:
                    res = Color.Black;
                    break;
            }
            return res;
        }
        #endregion

        public void ClearActions()
        {
            ActionList.Clear();

            //Деактивировать все виджеты.
            deactivator.Notify();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Функции рисования~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #region Drawing functions
        public abstract void DrawRectangle(Colors color, float x, float y, float width, float height);
        public abstract void DrawRoundedRectangle(Colors color, float x, float y, float radius, float width, float height);
        public abstract void DrawLine(Colors color, float x1, float y1, float x2, float y2);
        public abstract void DrawEllipse(Colors color, float x, float y, float width, float height);
        public abstract void FillEllipse(Colors color, float x, float y, float width, float height);
        public abstract void DrawText(Colors color, float x, float y, float size, string text);
        public abstract Utility.Size MeasureText(float size, string text);
        public abstract void Clear();

        public float Scale { get; protected set; }
        public float HorizontalShift { get; protected set; }
        public float VerticalShift { get; protected set; }

        protected void ModifyCoords(ref float x, ref float y)
        {
            x = x*Scale + HorizontalShift;
            y = y*Scale + VerticalShift;
        }
        protected void ModifyShift(ref float shift)
        {
            shift *= Scale;
        }

        protected void ReverseModifyCoords(ref float x, ref float y)
        {
            x = (x - HorizontalShift)/Scale;
            y = (y - VerticalShift)/Scale;
        }
        protected void ReverseModifyShift(ref float shift)
        {
            shift /= Scale;
        }
        #endregion

        public Int64 GetUniqueID()
        {
            List<Int64> listID = new List<Int64>();
            foreach (var widget in Widgets)
            {
                listID.Add(widget.ID);
            }

            if (listID.Count == 0)
            {
                return 0;
            }

            Int64 res = listID[0];
            for (int i = 0; i < listID.Count; i++)
            {
                if (res < listID[i])
                {
                    res = listID[i];
                }
            }
            ++res;
            return res;
        }


        private List<string> GetMachineTypes()
        {
            List<string> res = new List<string>();
            //MyProject.Info.Machines
            return res;
        }

        #region FormWrappers

        [field: NonSerialized]
        protected TransitionAttrsLogic transitionAttrsLogic = new TransitionAttrsLogic();
        protected EditVariablesLogic editVariablesLogic = new EditVariablesLogic();
        #endregion


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Функции вызова окон~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #region Window functions
        public abstract bool ShowStatePropertiesForm(State aState);
        public abstract bool ShowTransitionPropertiesForm(Transition aTransition);
        public abstract bool ShowVariablesForm();
        public abstract void ShowFSMContextMenu();
        #endregion

        public void SaveToFile(string path)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(List<Widget>));
            //System.Runtime.Serialization.
            BinaryFormatter serializer = new BinaryFormatter();
            Stream writer = File.Create(path);
            serializer.Serialize(writer, Widgets);
            writer.Close();

            string additionPath = path + ".data";
            serializer = new BinaryFormatter();
            writer = File.Create(additionPath);
            serializer.Serialize(writer, TheWindowData);
            writer.Close();
        }

        public void SaveToFile()
        {
            SaveToFile(FileName);
        }

        public void SaveToXML(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(path, settings);
            writer.WriteStartDocument();

            writer.WriteStartElement("diagram");
            
            WriteName(writer);
            WriteData(writer);
            WriteWidgets(writer);
            
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            SaveCoords(path);
        }

        public void SaveToXML()
        {
            SaveToXML(FileName);
        }

        private void SaveCoords(string path)
        {
            string fileName = path + ".positions";
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            //settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlWriter writer = XmlWriter.Create(fileName, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("diagram");
            SaveWidgetsPositions(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }

        private void SaveWidgetsPositions(XmlWriter writer)
        {
            foreach (var widget in Widgets)
            {
                widget.SaveCoords(writer);
            }
        }

        private void WriteName(XmlWriter writer)
        {
            writer.WriteElementString("name", DiagramName);
        }
        private void WriteData(XmlWriter writer)
        {
            writer.WriteStartElement("data");

            //string type = TheWindowData.Type.ToString();
            //writer.WriteAttributeString("type", type);

            switch (TheWindowData.Type)
            {
                case DiagramType.StateMachine:
                    WriteStateMachineData(writer);
                    break;
                case DiagramType.Connections:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            writer.WriteEndElement();
        }

        private void WriteStateMachineData(XmlWriter writer)
        {
            StatemachineData data = TheWindowData as StatemachineData;

            writer.WriteStartElement("Statemachine");

            foreach (var @event in data.Events)
            {
                writer.WriteStartElement("event");
                writer.WriteAttributeString("name", @event.Name);
                writer.WriteAttributeString("comment", @event.Comment);
                writer.WriteEndElement();
            }

            WriteVariables(writer);

            writer.WriteElementString("autoreject", data.AutoReject.ToString());

            writer.WriteEndElement();
        }

        private void WriteVariables(XmlWriter writer)
        {
            StatemachineData data = TheWindowData as StatemachineData;

            foreach (var v in data.Variables)
            {
                writer.WriteStartElement("variable");
                writer.WriteAttributeString("decl", v.ToString());
                writer.WriteEndElement();
                //writer.WriteAttributeString("type", v.Type.ToString());
                //writer.WriteAttributeString("name", v.Name);
            }
        }

        private void WriteWidgets(XmlWriter writer)
        {
            foreach (var widget in Widgets)
            {
                widget.ToXML(writer);
            }
        }

        public static WindowBase LoadFromXML(string path)
        {
            var agent = new XMLAgent();
            //var wnd = agent.Parse(path + ".xml");
            var wnd = agent.Parse(path);
            var posAgent = new PositionsAgent();
            posAgent.LoadPositions(wnd);
            return wnd;
        }

        /*
        public void LoadFromFile(string path)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(List<Widget>));
            //StreamReader sr = new StreamReader(path);
            if (File.Exists(path))
            {
                BinaryFormatter deserializer = new BinaryFormatter();
                Stream reader = File.OpenRead(path);
                widgets = (List<Widget>) deserializer.Deserialize(reader);
                reader.Close();

                foreach (var widget in Widgets)
                {
                    widget.Window = this;
                    deactivator.Attach(widget);
                }

                string additionPath = path + ".data";
                if (File.Exists(additionPath))
                {
                    reader = File.OpenRead(additionPath);
                    TheWindowData = (WindowData) deserializer.Deserialize(reader);
                    reader.Close();
                }
            }
        }
         * */

        public void GiveDefaultCoords()
        {
            List<Shape> shapeList = new List<Shape>();
            foreach (var w in Widgets)
            {
                var shape = w as Shape;
                if (shape != null)
                {
                    shape.DetermineWidth();
                    shapeList.Add(shape);
                }
            }
            
            //Найти максимум по ширине и по высоте.
            float maxWidth = 0, maxHeight = 0;
            foreach (var shape in shapeList)
            {
                if (maxHeight < shape.Height)
                {
                    maxHeight = shape.Height;
                }

                if (maxWidth < shape.Width)
                {
                    maxWidth = shape.Width;
                }
            }

            //Расположить фигуры квадратом.
            //Найти ближайший квадратный корень от количества.
            int sRow = (int) (Math.Sqrt(shapeList.Count)) + 1;
            //Присвоить координаты
            CalcCoords(sRow, maxWidth, maxHeight);
            CoordsAreSet = true;
        }

        public bool CoordsAreSet { get; set; }

        private const float IndentX = 50;
        private const float IndentY = 50;
        private void CalcCoords(int sRow, float maxWidth, float maxHeight)
        {
            int col = 0, row = 0;
            for (int i = 0; i < Widgets.Count; i++)
            {
                var shape = Widgets[i] as Shape;
                if (shape != null)
                {
                    shape.X = IndentX + col * maxWidth * 2;
                    shape.Y = IndentY + row * maxHeight * 2;

                    ++col;
                    if (col >= sRow)
                    {
                        col = 0;
                        ++row;
                    }
                }
            }
        }

        [field: NonSerialized]
        protected Notifier deactivator = new Notifier();

        protected List<Widget> widgets = new List<Widget>();
        public void AddWidget(Widget w)
        {
            Widgets.Add(w);

            //Подписать виджет на принудительную деактивацию.
            deactivator.Attach(w);
        }

        public void RemoveWidget(Widget w)
        {
            deactivator.Detach(w);
            Widgets.Remove(w);
        }

        public void GetMultiArrowNumber(Arrow arrow, out int num, out int amount)
        {
            amount = 0;
            num = 0;
            Type aType = arrow.GetType();
            foreach (var widget in Widgets)
            {
                Type wType = widget.GetType();
                if (wType == aType || wType.IsSubclassOf(aType))
                {
                    Arrow a = (Arrow) widget;
                    if ((a.Start == arrow.Start && a.End == arrow.End) ||
                        (a.Start == arrow.End && a.End == arrow.Start))
                    {
                        amount++;
                    }
                    if (a == arrow)
                    {
                        num = amount;
                    }
                }
            }
        }

        public void DrawAll()
        {
            foreach (var w in Widgets)
            {
                w.Draw();
            }
        }

        public void ReDraw()
        {
            Clear();
            DrawAll();
        }

        public enum States
        {
            Regular,
            CreateWidget,
        }

        public enum Events
        {
            LeftButtonClick,

        }

        [field: NonSerialized]
        public List<Widget> ActiveWidgets = new List<Widget>();

        public string DiagramName { get; set; }
        public string FileName { get; set; }
        public string SafeFileName { get; set; }

        public void StateButtonClick()
        {
            curParams = new CreateWidgetParams();
            curParams.Type = WidgetType.State;
            curParams.Window = this;
            ClearActions();
            var newAction = new Action(Action.ActionType.StartCreateState);
            ActionList.Add(newAction);
        }

        public void TransitionButtonClick()
        {
            curParams = new CreateWidgetParams();
            curParams.Type = WidgetType.Transition;
            curParams.Window = this;
            ClearActions();
            var newAction = new Action(Action.ActionType.StartCreateTransition);
            ActionList.Add(newAction);
        }

        public void FrontEndKeyPressed(char key)
        {
            switch (key)
            {
                case '+':
                    Scale += 0.1f;
                    ReDraw();
                    break;
                case '-':
                    if (Scale > 0.1)
                    {
                        Scale -= 0.1f;
                    }
                    ReDraw();
                    break;
                case 'p':
                    Scale = 1;
                    ReDraw();
                    break;
            }
        }

        public void FrontEndKeyDown(int key)
        {
            switch (key)
            {
                case 40: //down
                    VerticalShift -= 50;
                    ReDraw();
                    break;
                case 38: //up
                    VerticalShift += 50;
                    ReDraw();
                    break; 
                case 37: //left
                    HorizontalShift += 50;
                    ReDraw();
                    break;
                case 39: //right
                    HorizontalShift -= 50;
                    ReDraw();
                    break;
            }
        }

        public void FrontEndMouseClick(Dot e)
        {
            //ReverseModifyCoords(ref e.x, ref e.y);
            if (ActionList.Count == 0)
            {
                foreach (var widget in Widgets)
                {
                    if (widget.IntersectsWith(e.x, e.y))
                    {
                        widget.TheMode = Widget.Mode.Active;
                    }
                    else
                    {
                        widget.TheMode = Widget.Mode.Common;
                    }
                }
                return;
            }
            switch (ActionList[0].Type)
            {
                    #region CreateState
                case Action.ActionType.StartCreateState:
                    //Завершить создание состояния.
                    curParams.X = e.x;
                    curParams.Y = e.y;
                    curCommand = new CreateStateCommand((CreateWidgetParams)curParams);
                    curCommand.Execute();
                    ClearActions();
                    //ReDraw();
                    break;
                    #endregion
                    #region CreateTransition
                case Action.ActionType.StartCreateTransition:
                    switch (ActionList.Count)
                    {
                        case 1:
                            //Выделить состояние, если в него попал курсор.
                            foreach (var widget in Widgets)
                            {
                                if (widget.GetType() == typeof(State))
                                {
                                    if (widget.IntersectsWith(e.x, e.y))
                                    {
                                        widget.TheMode = Widget.Mode.Active;
                                        //Виджет выбран.
                                        Action newAction = new Action(Action.ActionType.StateSelected);
                                        newAction.Widgets.Add(widget);
                                        ActionList.Add(newAction);
                                        curParams.Type = WidgetType.Transition;
                                    }
                                }
                            }
                            break;
                        case 2:
                            //Выделить состояние, если в него попал курсор.
                            foreach (var widget in Widgets)
                            {
                                if (widget.GetType() == typeof(State))
                                {
                                    if (widget.IntersectsWith(e.x, e.y))
                                    {
                                        widget.TheMode = Widget.Mode.Active;
                                        //Виджет выбран.
                                        Action newAction = new Action(Action.ActionType.StateSelected);
                                        newAction.Widgets.Add(widget);
                                        ActionList.Add(newAction);
                                        //curParams.TheWidget
                                        curCommand = new CreateArrowCommand((CreateWidgetParams)curParams);
                                        curCommand.Execute();
                                        ClearActions();
                                        //ReDraw();
                                        return;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    //Invalidate();
                    break;
                    #endregion
                default:
                    foreach (var widget in Widgets)
                    {
                        if (widget.IntersectsWith(e.x, e.y))
                        {
                            widget.TheMode = Widget.Mode.Active;
                        }
                        else
                        {
                            widget.TheMode = Widget.Mode.Common;
                        }
                    }
                    break;
            }
        }

        [field: NonSerialized]
        protected Utility.Dot startMoveDot;

        private bool IsWidgetSelected(float x, float y, out Widget w)
        {
            foreach (var widget in Widgets)
            {
                if (widget.IntersectsWith(x, y))
                {
                    w = widget;
                    return true;
                }
            }
            w = null;
            return false;
        }

        public void FrontEndMouseDown(float x, float y)
        {
            ReverseModifyCoords(ref x, ref y);
            startMoveDot.x = x;
            startMoveDot.y = y;
            if (ActionList.Count == 0)
            {
                //Смотрим, есть ли выделенные виджеты, в которые попал курсор.
                var newAction = new Action(Action.ActionType.StartMoveState);
                var selected = false;
                foreach (var widget in Widgets)
                {
                    if (widget.TheMode == Widget.Mode.Active)
                    {
                        if (widget.IntersectsWith(x, y))
                        {
                            selected = true;
                        }
                        newAction.Widgets.Add(widget);
                    }
                }

                if (selected)
                {
                    ActionList.Add(newAction);
                }
                else
                {
                    //Deselect
                    foreach (var widget in Widgets)
                    {
                        if (widget.TheMode == Widget.Mode.Active)
                        {
                            widget.TheMode = Widget.Mode.Common;
                        }
                    }
                }

            }
        }

        public void FrontEndRMBClick(float x, float y)
        {
            ReverseModifyCoords(ref x, ref y);
            Widget w;
            if (IsWidgetSelected(x, y, out w))
            {
                //Вызвать меню.
                w.TheMode = Widget.Mode.ShowInfo;
            }
            else
            {
                //Вызвать меню автомата.
                if (TheWindowData.GetType() == typeof (StatemachineData))
                {
                    ShowFSMContextMenu();
                }
            }
        }

        public void FrontEndMouseUp(float x, float y)
        {
            ReverseModifyCoords(ref x, ref y);
            if ((Math.Abs(x - StartMoveDot.x) < MathSupport.DeltaEqual) &&
                (Math.Abs(y - StartMoveDot.y) < MathSupport.DeltaEqual))
            {
                //return;
                var mouse = new Dot();
                mouse.x = x;
                mouse.y = y;
                FrontEndMouseClick(mouse);
                return;
            }

            if (ActionList.Count > 0)
            {
                if (ActionList[0].Type == Action.ActionType.StartMoveState)
                {
                    var dX = x - StartMoveDot.x;
                    var dY = y - StartMoveDot.y;
                    //Создаем команду.
                    curParams = new MoveWidgetParams();
                    curParams.X = StartMoveDot.x;
                    curParams.Y = StartMoveDot.y;
                    curParams.Window = this;

                    curParams.Width = dX;
                    curParams.Height = dY;

                    foreach (var widget in ActionList[0].Widgets)
                    {
                        curParams.TheWidget = widget;
                        curCommand = new MoveWidgetCommand(curParams);
                        curCommand.Execute();
                    }
                }
            }
            //TheControl.Invalidate();
            ClearActions();
        }

        public void FrontEndMouseDoubleClick(float x, float y)
        {
            ReverseModifyCoords(ref x, ref y);
            Widget w;
            if (IsWidgetSelected(x, y, out w))
            {
                //Вывести окно об изменении свойств.
                w.ShowPropertiesForm();
                //Если свойства изменились, то 
                //Выполнить команду "Изменение свойств".
            }
        }

        private Widget GetIntersected(float x, float y)
        {
            ReverseModifyCoords(ref x, ref y);
            foreach (var item in widgets)
            {
                if (item.IntersectsWith(x, y))
                {
                    return item;
                }
            }
            return null;
        }

        [field: NonSerialized]
        private Stack<Command> doneCommands;
        public Stack<Command> DoneCommands
        {
            get { return doneCommands; }
            set { doneCommands = value; }
        }

        [field: NonSerialized]
        private Stack<Command> undoneCommands;
        public Stack<Command> UndoneCommands
        {
            get { return undoneCommands; }
            set { undoneCommands = value; }
        }

        public void Undo()
        {
            //
            if (DoneCommands.Count == 0)
            {
                return;
            }
            var command = DoneCommands.Pop();
            try
            {
                command.Unexecute();
                UndoneCommands.Push(command);
            }
            catch (NotImplementedException exception)
            {
                MessageBox.Show(null, exception.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } 
        }

        public void ChangeAutoReject(bool autoReject)
        {
            var wd = TheWindowData as StatemachineData;
            if (wd == null)
            {
                return;
            }

            wd.AutoReject = autoReject;
        }

        public void DeleteSelectedWidgets()
        {
            List<Widget> selected = new List<Widget>();
            CompositeParams p = new CompositeParams();
            p.Window = this;
            foreach (var widget in Widgets)
            {
                if (widget.TheMode == Widget.Mode.Active)
                {
                    selected.Add(widget);
                    p.Commands.Add(widget.PrepareDeleteCommand());
                }
            }

            CompositeCommand cmd = new CompositeCommand(p);
            cmd.Execute();
        }

        /// <summary>
        /// Создает конечный автомат из элементов окна. Если встречаются виджеты, не имеющие отношения к 
        /// конечному автомату, выбрасывается исключение.
        /// </summary>
        /// <returns></returns>
        public StateMachine.StateMachine ExportStateMachine()
        {
            var machine = new StateMachine.StateMachine();
            Dictionary<string, State> states = new Dictionary<string,State>();
            Dictionary<string, Transition> transitions = new Dictionary<string,Transition>();
            Dictionary<string, Event> events = new Dictionary<string,Event>();

            ESMVariables(ref machine);

            foreach (var w in Widgets)
            {
                if (w.GetType() == typeof(State))
                {
                    State s = (State)w;
                    while (states.ContainsKey(s.TheAttributes.Name))
                    {
                        s.TheAttributes.Name += "_";
                    }
                    states.Add(s.TheAttributes.Name, s);
                }
                else if (w.GetType() == typeof(Transition))
                {
                    Transition t = (Transition)w;
                    //transitions.Add(t.TheAttributes.Name, t);
                    TryAddTransition(transitions, t);

                    //Добавить событие.
                    if (t.TheAttributes.TheEvent.Name == null)
                    {
                        continue;
                    }

                    TryAddEvent(events, t.TheAttributes.TheEvent);
                    /*
                    if (!events.ContainsKey(t.TheAttributes.TheEvent.Name))
                    {
                        events.Add(t.TheAttributes.TheEvent.Name, t.TheAttributes.TheEvent);
                    }
                     * */
                }
            }

            StatemachineData data = TheWindowData as StatemachineData;
            if (data != null)
            {
                foreach (var evt in data.Events)
                {
                    TryAddEvent(events, evt);
                }

                machine.AutoReject = data.AutoReject;
            }

            machine.States = new List<State>(states.Count);
            machine.EndStates = new List<State>();
            foreach (var pair in states)
            {
                machine.States.Add(pair.Value);
                if (pair.Value.TheAttributes.Type == State.StateType.Start)
                {
                    machine.StartState = pair.Value;
                }

                if (pair.Value.TheAttributes.Type == State.StateType.End)
                {
                    machine.EndStates.Add(pair.Value);
                }
            }

            machine.Transitions = new List<Transition>(transitions.Count);
            foreach (var pair in transitions)
            {
                machine.Transitions.Add(pair.Value);
            }

            machine.Events = new List<Event>(events.Count);
            foreach (var pair in events)
            {
                machine.Events.Add(pair.Value);
            }

            machine.Name = DiagramName;
            machine.FileName = SafeFileName + ".cs";

            return machine;
        }
        private void ESMVariables(ref StateMachine.StateMachine machine)
        {
            var wd = TheWindowData as StatemachineData;
            machine.Variables = new List<StateMachine.Variable>(wd.Variables);
        }

        public void ImportMachineData(PluginData.StateMachine fsm)
        {
            TheWindowData = new StatemachineData();
            DiagramName = fsm.Type;
            SafeFileName = DiagramName + FilesInfo.DiagramExtension;
            FileName = MyProject.Info.GetWorkFolder() + "\\" + SafeFileName;

            ImportStates(fsm.States);
            ImportEvents(fsm.Events);
            ImportTransitions(fsm.Transitions);
            ImportVariables(fsm.Variables);

        }

        private void ImportVariables(List<Variable> vars)
        {
            foreach (var variable in vars)
            {
                StateMachine.Variable.TypeList t;
                string defaultValue = "0";

                switch (variable.Type)
                {
                    case Variable.TypeList.Bool:
                        t = StateMachine.Variable.TypeList.Bool;
                        defaultValue = "false";
                        break;
                    case Variable.TypeList.Int8:
                        t = StateMachine.Variable.TypeList.Int8;
                        break;
                    case Variable.TypeList.Int16:
                        t = StateMachine.Variable.TypeList.Int16;
                        break;
                    case Variable.TypeList.Int32:
                        t = StateMachine.Variable.TypeList.Int32;
                        break;
                    default:
                        t = StateMachine.Variable.TypeList.Bool;
                        break;
                }
                var type = variable.GetType();
                if (type == typeof(PluginData.SingleVariable))
                {
                    var v = new StateMachine.SingleVariable { Name = variable.Name, Type = t, Value = defaultValue};
                    AddVariable(v);
                }
                else if (type == typeof(PluginData.Array))
                {
                    var v = new StateMachine.Array {Name = variable.Name, Type = t};
                    AddVariable(v);
                }
            }
        }

        protected abstract void AddVariable(StateMachine.Variable v);

        private void ImportStates(List<PluginData.State> states)
        {
            foreach (var impState in states)
            {
                State myState = new State(this);
                myState.ID = impState.ID.Value;
                myState.TheAttributes = new StateAttributes
                    {
                        Name = impState.Name,
                        Type = (State.StateType) ((int) impState.Type)
                    };

                foreach (var impAct in impState.EntryActions)
                {
                    Attributes.Action myAct = new Attributes.Action
                        {
                            Name = impAct.Name,
                            Comment = ""
                        };
                    myAct.Synchronism = (ESynchronism) ((int) impAct.Synchronism);
                    myState.TheAttributes.EntryActions.Add(myAct);
                }

                foreach (var impAct in impState.ExitActions)
                {
                    Attributes.Action myAct = new Attributes.Action
                    {
                        Name = impAct.Name,
                        Comment = ""
                    };
                    myAct.Synchronism = (ESynchronism)((int)impAct.Synchronism);
                    myState.TheAttributes.ExitActions.Add(myAct);
                }

                foreach (var impEff in impState.EntryEffects)
                {
                    Attributes.AutomatonEffect myEff = new Attributes.AutomatonEffect
                        {
                            Name = impEff.Name,
                            Type = impEff.Type,
                            Event = impEff.Event,
                            TheEffectType = Attributes.AutomatonEffect.EffectType.Manual
                        };
                    myEff.Synchronism = (ESynchronism) ((int) impEff.Synchronism);
                    myState.TheAttributes.EntryEffects.Add(myEff);
                }

                foreach (var impExe in impState.EntryExecutions)
                {
                    Attributes.AutomatonExecution myExe = new Attributes.AutomatonExecution
                    {
                        Name = impExe.Name,
                        Type = impExe.Type,
                    };
                    myState.TheAttributes.EntryExecutions.Add(myExe);
                }

                foreach (var impNest in impState.NestedMachines)
                {
                    Attributes.NestedMachine myNest = new Attributes.NestedMachine
                        {
                            Name = impNest.Name,
                            Type = impNest.Type
                        };
                    myState.TheAttributes.NestedMachines.Add(myNest);
                }

                /*
                foreach (var transition in impState.Incoming)
                {
                    myState.IncomingArrows
                }
                 */
 
                Widgets.Add(myState);
            }
        }

        private void ImportEvents(List<PluginData.Event> events)
        {
            var wd = GetMachineData();
            if (wd == null)
            {
                return;
            }
            foreach (var @event in events)
            {
                wd.Events.Add(new Event {Name = @event.Name});
            }
        }

        private void ImportTransitions(List<PluginData.Transition> transitions)
        {
            var wd = GetMachineData();
            foreach (var trans in transitions)
            {
                var nt = new Transition(this);
                nt.ID = trans.ID.Value;
                nt.TheAttributes.TheEvent = wd.Events.Find(evt => evt.Name == trans.Event.Name);
                if (nt.TheAttributes.TheEvent == null)
                {
                    nt.TheAttributes.TheEvent = Event.CreateEpsilon();
                }

                if (trans.GuardExists())
                {
                    nt.TheAttributes.Guard = trans.Guard;
                }

                if (trans.End == null)
                {
                    continue; //Мы не отображаем состояния, которые ведут в никуда.
                }
                nt.End = Widgets.Find(state => state.ID == trans.End.ID.Value) as State;
                nt.End.IncomingArrows.Add(nt);

                if (trans.Start != null)
                {
                    nt.Start = Widgets.Find(state => state.ID == trans.Start.ID.Value) as State;
                    nt.Start.OutgoingArrows.Add(nt);
                }
                else
                {
                    var s0 = (nt.End as State);
                    s0.TheAttributes.Type = State.StateType.Start;
                }

                foreach (var act in trans.Actions)
                {
                    Attributes.Action myAct = new Attributes.Action
                    {
                        Name = act.Name,
                        Comment = ""
                    };
                    myAct.Synchronism = (ESynchronism)((int)act.Synchronism);
                    nt.TheAttributes.Actions.Add(myAct);
                }

                foreach (var eff in trans.Effects)
                {
                    Attributes.AutomatonEffect myEff = new Attributes.AutomatonEffect
                        {
                            Event = eff.Event,
                            Name = eff.Name,
                            Type = eff.Type
                        };
                    myEff.Synchronism = (ESynchronism) ((int) eff.Synchronism);

                    nt.TheAttributes.Effects.Add(myEff);
                }

                nt.TheAttributes.Code = trans.Code;

                if (trans.Start != null)
                {
                    Widgets.Add(nt);
                }
            }
        }

        private StatemachineData GetMachineData()
        {
            var wd = (TheWindowData as StatemachineData);
            if (wd == null)
            {
                throw new InvalidDiagramException("Ошибка: попытка использования диаграммы автоматов вместо другой диаграммы");
            }
            return wd;
        }

        public PluginData.StateMachine ExportMachineData()
        {
            PluginData.StateMachine res = new PluginData.StateMachine();

            StateMachine.StateMachine m = ExportStateMachine();

            ExportStates(ref res, m);
            ExportEvents(ref res, m);
            ExportTransitions(ref res, m);
            ExportVariables(ref res);
            ExportAutoReject(ref res);

            return res;
        }

        private void ExportAutoReject(ref PluginData.StateMachine res)
        {
            var data = TheWindowData as StatemachineData;
            if (data != null)
            {
                res.AutoReject = data.AutoReject;
            }
        }


        private void ExportVariables(ref PluginData.StateMachine res)
        {
            res.Variables = new List<Variable>();
            var data = TheWindowData as StatemachineData;
            foreach (var variable in data.Variables)
            {
                PluginData.Variable v = null;
                if (variable.GetType() == typeof(StateMachine.SingleVariable))
                {
                    v = new PluginData.SingleVariable { Value = ((StateMachine.SingleVariable)variable).Value };
                }
                if (variable.GetType() == typeof(StateMachine.Array))
                {
                    v = new PluginData.Array {NElements = ((StateMachine.Array) variable).NElements};
                }

                v.Name = variable.Name;
                v.SetType(variable.TypeToString());

                v.Volatile = variable.Volatile;
                v.External = variable.External;
                v.Param = variable.Param;

                res.Variables.Add(v);
            }
        }

        private static void ExportTransitions(ref PluginData.StateMachine res, StateMachine.StateMachine m)
        {
            res.Transitions = new List<PluginData.Transition>();
            foreach (var wt in m.Transitions)
            {
                PluginData.Transition t = new PluginData.Transition();
                t.Name = wt.TheAttributes.Name;
                t.Start = res.GetState(new UID(wt.Start.ID));
                t.Start.Outgoing.Add(t);

                t.End = res.GetState(new UID(wt.End.ID));
                t.End.Incoming.Add(t);

                t.Event = new PluginData.Event {Name = wt.TheAttributes.TheEvent.SafeName};

                t.Actions = new List<PluginData.Action>();
                foreach (var wa in wt.TheAttributes.Actions)
                {
                    PluginData.Action a = new PluginData.Action()
                                              {
                                                  Name = wa.Name,
                                                  Synchronism = (PluginData.Synchronism) ((int) wa.Synchronism)
                                              };
                    t.Actions.Add(a);
                }
                res.Transitions.Add(t);

                t.Effects = new List<AutomatonEffect>();
                foreach (var we in wt.TheAttributes.Effects)
                {
                    PluginData.AutomatonEffect eff = new AutomatonEffect()
                                                         {
                                                             Name = we.Name,
                                                             Event = we.Event,
                                                             Synchronism =
                                                                 (PluginData.Synchronism) ((int) we.Synchronism),
                                                             Type = we.Type
                                                         };
                    t.Effects.Add(eff);
                }

                //t.Code = new List<string>(wt.TheAttributes.Code);
                t.Code = new List<string>();
                foreach (var line in wt.TheAttributes.Code.Where(line => line.Trim() != ""))
                {
                    t.Code.Add(line);
                }

                t.Guard = wt.TheAttributes.Guard;
            }
        }

        private static void ExportStates(ref PluginData.StateMachine res, StateMachine.StateMachine m)
        {
            res.States = new List<PluginData.State>();
            foreach (var ws in m.States)
            {
                PluginData.State s = new PluginData.State
                                         {
                                             Name = ws.TheAttributes.Name,
                                             ID = new PluginData.UID {Value = ws.ID},
                                             Type = (PluginData.State.StateType) ((int) ws.TheAttributes.Type),
                                             Outgoing = new List<PluginData.Transition>(),
                                             Incoming = new List<PluginData.Transition>(),
                                             EntryActions = new List<PluginData.Action>()
                                         };

                foreach (var wact in ws.TheAttributes.EntryActions)
                {
                    PluginData.Action act = new PluginData.Action();
                    act.Synchronism = (PluginData.Synchronism)((int)wact.Synchronism);
                    act.Name = wact.Name;
                    s.EntryActions.Add(act);
                }

                s.ExitActions = new List<PluginData.Action>();
                foreach (var wact in ws.TheAttributes.EntryActions)
                {
                    PluginData.Action act = new PluginData.Action();
                    act.Synchronism = (PluginData.Synchronism)((int)wact.Synchronism);
                    act.Name = wact.Name;
                    s.ExitActions.Add(act);
                }

                s.EntryEffects = new List<PluginData.AutomatonEffect>();
                foreach (var weff in ws.TheAttributes.EntryEffects)
                {
                    PluginData.AutomatonEffect eff = new PluginData.AutomatonEffect
                                                         {
                                                             Event = weff.Event,
                                                             Name = weff.Name,
                                                             Synchronism =
                                                                 (PluginData.Synchronism) ((int) weff.Synchronism),
                                                             Type = weff.Type
                                                         };
                    s.EntryEffects.Add(eff);
                }

                s.EntryExecutions = new List<AutomatonExecution>();
                foreach (var wexe in ws.TheAttributes.EntryExecutions)
                {
                    PluginData.AutomatonExecution ex = new AutomatonExecution {Name = wexe.Name, Type = wexe.Type};
                    s.EntryExecutions.Add(ex);
                }

                s.NestedMachines = new List<NestedMachine>();
                if (ws.TheAttributes.NestedMachines != null)
                {
                    foreach (var wm in ws.TheAttributes.NestedMachines)
                    {
                        PluginData.NestedMachine machine = new NestedMachine
                                                               {
                                                                   Name = wm.Name,
                                                                   Type = wm.Type
                                                               };
                        s.NestedMachines.Add(machine);
                    }
                }
                res.States.Add(s);
            }

            if (m.StartState == null)
            {
                throw new ArgumentNullException("Не задано начальное состояние автомата");
            }
            res.StartState = res.GetState(new UID(m.StartState.ID));
        }

        private static void ExportEvents(ref PluginData.StateMachine res, StateMachine.StateMachine m)
        {
            res.Events = new List<PluginData.Event>();
            foreach (var @event in m.Events)
            {
                PluginData.Event evt = new PluginData.Event { Name = @event.SafeName };
                res.Events.Add(evt);
            }
        }

        private static void TryAddTransition(IDictionary<string, Transition> transitions, Transition t)
        {
            while (transitions.ContainsKey(t.TheAttributes.Name))
            {
                t.AutoName();
            }

            transitions.Add(t.TheAttributes.Name, t);
        }

        private static void TryAddEvent(Dictionary<string, Event> events, Event evt)
        {
            if (!events.ContainsKey(evt.Name))
            {
                events.Add(evt.Name, evt);
            }

        }
    }
}