using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Project
{
    /// <summary>
    /// Обертка вокруг контрола.
    /// </summary>
    [Serializable]
    public abstract class ProjectWindow
    {
        public ProjectManager Owner { get; set; }

        public object MainForm { get; set; }

        public abstract void SetControl(object control);

        public abstract void SetProjectMenu(object projectMenu);

        public abstract void SetDiagramMenu(object diagramMenu);

        public abstract void SetProjectName(string name);

        public abstract void AddDiagram(string name, DiagramType dType);

        public abstract bool RemoveDiagram(string name);

        public abstract object GetActiveNode();

        public abstract void ClearNodes();

        /// <summary>
        /// Обработка двойного клика по узлу.
        /// </summary>
        /// <param name="node">Узел. Узлом может быть как проект, так и диаграмма.</param>
        public abstract void NodeDoubleClick(object node);
    }
}
