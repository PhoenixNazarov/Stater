using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StaterV.Project
{
    [Serializable]
    public struct DiagramInfo
    {
        /// <summary>
        /// Имя диаграммы, которое можно использовать в коде.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Введенное имя диаграммы. Совпадает с именем файла.
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// Путь к папке, в которой находится диаграмма.
        /// </summary>
        [property: XmlIgnore]
        public string Location { get; set; }

        /// <summary>
        /// Проект, в котором данная диаграмма.
        /// </summary>
        [property: XmlIgnore]
        public ProjectInfo Owner { get; set; }
        
        public DiagramType Type { get; set; }

        public void SetLocation(string path)
        {
            Location = path;
        }

        public void CreateName()
        {
            Name = ShowName.Trim();
            Name = Name.Replace(' ', '_');

            if (Name.Count() == 0)
            {
                Name = "Diagram";
            }

            int tmp;
            if (int.TryParse(Name.Substring(0, 1), out tmp))
            {
                Name = Name.Insert(0, "_");
            }
        }

        public string GetPath()
        {
            return Location + "\\" + Name + FilesInfo.DiagramExtension;
        }
    }
}
