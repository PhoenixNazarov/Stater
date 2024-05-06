using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using StaterV.Attributes;
using StaterV.Windows;

namespace StaterV.Project
{
    [Serializable]
    public struct MachineObject
    {
        public string Name;
        public string Type;
    }

    [Serializable]
    public struct ProjectInfo
    {
        public string Name { get; set; }

        [property:XmlIgnore]
        public string Location { get; set; }

        //public List<DiagramInfo> Diagrams { get; set; }
        [XmlArray("Diagrams")]
        public List<DiagramInfo> Diagrams 
        {
            get { return diagrams; }
            set { diagrams = value; }
        }

        private List<DiagramInfo> diagrams;

        [property: XmlIgnore]
        public Dictionary<AutomatonExecution, AutomatonExecution> MachineObjects { get; private set; }

        [property: XmlIgnore]
        public List<StateMachine.StateMachine> Machines { get; private set; }

        public void AddMachineObjects(List<AutomatonExecution> objects)
        {
            if (MachineObjects == null)
            {
                MachineObjects = new Dictionary<AutomatonExecution, AutomatonExecution>();
            }

            foreach(var execution in objects)
            {
                MachineObjects.Add(execution, execution);
            }
        }

        public void RemoveMachineObjects(List<AutomatonExecution> objects)
        {
            foreach (var execution in objects)
            {
                MachineObjects.Remove(execution);
            }
        }

        public string GetWorkFolder()
        {
            var path = Location;

            Location = Location.Trim('\\');

            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            path += Name;// +"\\";
            return path;                
        }

        public void AddDiagram(string name, DiagramType type)
        {
            if (Diagrams == null)
            {
                Diagrams = new List<DiagramInfo>();
            }

            foreach (var diagram in Diagrams)
            {
                if (diagram.Name == name)
                {
                    throw (new DataException("Diagram already exists"));
                }
            }
            var newDiagram = new DiagramInfo();
            newDiagram.Name = name;
            newDiagram.Type = type;
            newDiagram.Location = GetWorkFolder();
            //newDiagram.Location = "";
            //newDiagram.Owner = this.Clone();
            Diagrams.Add(newDiagram);

            if (type == DiagramType.StateMachine)
            {
                StateMachine.StateMachine m = new StateMachine.StateMachine();
                m.Name = name;
                //AddMachine(m);
                AddMachine(LoadOneMachine(newDiagram));
            }
        }

        private void AddMachine(StateMachine.StateMachine m)
        {
            if (Machines == null)
            {
                Machines = new List<StateMachine.StateMachine>();
            }
            Machines.Add(m);
        }

        public void RemoveDiagram(string name)
        {
            //var dgr = Diagrams.Find()
            Diagrams.RemoveAt(Diagrams.FindIndex(0, p => p.Name == name));
            //Diagrams.re
        }

        #region Work with files
        public void SaveProject(string path)
        {
        }

        public void SaveProject()
        { 
            //Путь = <Location>/<Name>
            var path = GetWorkFolder() + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += Name + FilesInfo.ProjectExtension;
            //Сериализовать через xml.
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectInfo));
            TextWriter wr = new StreamWriter(path);
            serializer.Serialize(wr, this);
            wr.Close();

            //Сериализовать хранилище событий через xml.
            /*
            serializer = new XmlSerializer((typeof(StaterV.Attributes.EventStorage)));
            string esPath = GetWorkFolder() + "\\" + "EventStorage.stu";
            wr = new StreamWriter(esPath);
            serializer.Serialize(wr, Attributes.EventStorage.GetInstance());
            wr.Close();
             * */

            /*
            BinaryFormatter formatter = new BinaryFormatter();
            string esPath = GetWorkFolder() + "\\" + FilesInfo.EventStorageFile;
            Stream writer = File.Create(esPath);
            formatter.Serialize(writer, EventStorage.GetInstance());
            writer.Close();
            */
        }

        public void LoadProject(string path)
        {
            //Десериализовать
            XmlSerializer s = new XmlSerializer(typeof(ProjectInfo));
            TextReader r = new StreamReader(path);
            this = (ProjectInfo)s.Deserialize(r);
            r.Close();

            //Выделить Location
            var ind = path.LastIndexOf("\\" + Name + "\\");
            //var indL = path.LastIndexOf("\\" + Name + FilesInfo.ProjectExtension);
            var L = Path.GetDirectoryName(path);
            var indL = L.LastIndexOf("\\");
            var L2 = L.Substring(0, indL);
            Location = path.Substring(0, ind);

            for (int i = 0; i < diagrams.Count; i++)
            {
                var dgr = diagrams[i];
                dgr.SetLocation(Location);
                dgr.Owner = Clone();
                dgr.Location = GetWorkFolder();
                diagrams[i] = dgr;
            }
            LoadMachines();
        }

        private void LoadMachines()
        {
            Machines = new List<StateMachine.StateMachine>();
            Machines.Clear();
            //Загрузить все окна и извлечь из каждого автомат.
            foreach (var dgr in Diagrams)
            {
                if (dgr.Type != DiagramType.StateMachine)
                {
                    continue;
                }
                Machines.Add(LoadOneMachine(dgr));
            }
        }

        private StateMachine.StateMachine LoadOneMachine(DiagramInfo dgr)
        {
            var wnd = WindowBase.LoadFromXML(dgr.GetPath());
            var res = wnd.ExportStateMachine();
            res.Name = dgr.Name;
            LoadMachineExecutions(res);
            return res;
        }

        private void LoadMachineExecutions(StateMachine.StateMachine _machine)
        {
            if (MachineObjects == null)
            {
                MachineObjects = new Dictionary<AutomatonExecution, AutomatonExecution>();
            }

            foreach (var state in _machine.States)
            {
                foreach (var execution in state.TheAttributes.EntryExecutions)
                {
                    MachineObjects.Add(execution, execution);
                }
            }
        }

        #endregion

        public ProjectInfo Clone()
        {
            ProjectInfo res = (ProjectInfo)MemberwiseClone();
            return res;
        }
    }
}
