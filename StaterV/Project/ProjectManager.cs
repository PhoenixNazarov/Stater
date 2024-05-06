using System;
using System.Collections.Generic;
using PluginData;
using StaterV.Windows;

namespace StaterV.Project
{
    /// <summary>
    /// Хранит всю логику работы с проектами.
    /// </summary>
    [Serializable]
    public class ProjectManager
    {
        private ProjectWindow window;
        private ProjectInfo info;

        public ProjectInfo Info
        {
            get { return info; }
        }

        public ProjectWindow Window
        {
            get { return window; }
            set { window = value; window.Owner = this; }
        }

        public string GetLocation()
        {
            return info.Location;
        }

        public void SetProjectName(string name)
        {
            window.SetProjectName(name);
            info.Name = name;
        }

        public void SetProjectName(string name, string location)
        {
            window.SetProjectName(name);
            info.Name = name;
            info.Location = location;
        }

        public void SaveProject(string path)
        {
            info.SaveProject(path);
        }

        public void SaveProject()
        {
            info.SaveProject();
        }

        public void LoadProject(string path)
        {
            info = new ProjectInfo();
            info.LoadProject(path);
            window.SetProjectName(info.Name);

            foreach (var dgr in info.Diagrams)
            {
                window.AddDiagram(dgr.Name, dgr.Type);
            }
        }

        public void AddMachineObjects(List<Attributes.AutomatonExecution> objects)
        {
            info.AddMachineObjects(objects);
        }

        public void RemoveMachineObjects(List<Attributes.AutomatonExecution> objects)
        {
            info.RemoveMachineObjects(objects);
        }

        public void AddDiagram(string name, DiagramType type)
        {
            //Добавить диаграмму
            info.AddDiagram(name, type);
            window.AddDiagram(name, type);
        }

        public void RemoveDiagram(string name)
        {
            //Отправить запрос на удаление диаграммы в info.
            info.RemoveDiagram(name);
            //Удалить диаграмму из TreeView.
            window.RemoveDiagram(name);
        }

        public string GetDiagramInfo(string name)
        {
            //Найти информацию о диаграмме.
            foreach (var dgr in info.Diagrams)
            {
                if (dgr.Name == name)
                { 
                    //Вернуть информацию о диаграмме.
                    return dgr.GetPath();
                }
            }
            return null;
        }

        public void NodeDoubleClick(object node)
        {
            window.NodeDoubleClick(node);
        }

        public static string ExtractDiagramName(string fileName)
        {
            var ind = fileName.IndexOf(FilesInfo.DiagramExtension);
            if (ind != -1)
            {
                var res = fileName.Substring(0, ind);
                return res;
            }
            else
            {
                throw new Exceptions.DiagramNameException("Wrong file name: " + fileName);
            }
        }

        public List<StateMachine.StateMachine> ExportStateMachines()
        {
            var res = new List<StateMachine.StateMachine>();

            if (Info.Diagrams == null)
            {
                return res;
            }

            foreach (var diagram in Info.Diagrams)
            {
                if (diagram.Type == DiagramType.StateMachine)
                {
                    //Export.
                    //WindowDotNet wnd = new WindowDotNet();
                    //wnd.LoadFromFile(diagram.GetPath());
                    var wnd = WindowBase.LoadFromXML(diagram.GetPath());
                    var machine = wnd.ExportStateMachine();
                    machine.Name = diagram.Name;
                    machine.FileName = diagram.GetPath();
                    res.Add(machine);
                }
            }

            return res;
        }

        public IParams CreateParams()
        {
            IParams res = new IParams();
            res.Machines = ExportIMachines();
            res.WorkDirectory = Info.GetWorkFolder();
            return res;
        }

        private List<PluginData.StateMachine> ExportIMachines()
        {
            if (Info.Diagrams == null)
            {
                return null;
            }

            List<PluginData.StateMachine> res = new List<PluginData.StateMachine>();
            foreach (var diagram in Info.Diagrams)
            {
                /*
                var wnd = WindowBase.LoadFromXML(diagram.GetPath());
                var machine = wnd.ExportMachineData();
                machine.Name = diagram.Name;
                 * */
                res.Add(ExportMachine(diagram));
            }

            return res;
        }

        private PluginData.StateMachine ExportMachine(DiagramInfo diagram)
        {
            var wnd = WindowBase.LoadFromXML(diagram.GetPath());
            var machine = wnd.ExportMachineData();
            machine.Name = diagram.Name;
            return machine;
        }

        public PluginData.StateMachine ExportMachine(string name)
        {
            foreach (var dgr in Info.Diagrams)
            {
                if (dgr.Name == name)
                {
                    return ExportMachine(dgr);
                }
            }
            return null;
        }
    }
}
