using System.Data;
using System.Windows.Forms;

namespace StaterV.Project
{
    /// <summary>
    /// Специализация для Windows Forms.
    /// </summary>
    class ProjectWindowDotNet : ProjectWindow
    {
        private TreeView theControl;
        private ContextMenuStrip projectMenu;
        private ContextMenuStrip diagramMenu;

        public override void SetControl(object control)
        {
            var cType = control.GetType();
            if (cType == typeof (Control) || cType.IsSubclassOf(typeof (Control)))
            {
                theControl = control as TreeView;
            }
        }

        public override void SetProjectMenu(object _projectMenu)
        {
            var cType = _projectMenu.GetType();
            var mType = _projectMenu.GetType();
            if (cType == mType || cType.IsSubclassOf(mType))
            {
                projectMenu = _projectMenu as ContextMenuStrip;
            }
            var projectNode = theControl.Nodes[ProjectKey];
            if (projectNode != null)
            {
                projectNode.ContextMenuStrip = projectMenu;
            }
        }

        public override void SetDiagramMenu(object _diagramMenu)
        {
            var cType = _diagramMenu.GetType();
            var mType = _diagramMenu.GetType();
            if (cType == mType || cType.IsSubclassOf(mType))
            {
                diagramMenu = _diagramMenu as ContextMenuStrip;
            }
            var projectNode = theControl.Nodes[ProjectKey];
            if (projectNode != null)
            {
                for (int i = 0; i < projectNode.Nodes.Count; i++)
                {
                    projectNode.Nodes[i].ContextMenuStrip = diagramMenu;
                }
            }
        }

        private const string ProjectKey = "project";

        public override void SetProjectName(string name)
        {
            var projectNode = theControl.Nodes[ProjectKey];
            if (projectNode == null)
            {
                projectNode = theControl.Nodes.Add(ProjectKey, name);
            }
            else
            {
                //theControl.Nodes[ProjectKey].Text = name;
                projectNode.Text = name;
            }
            if (projectMenu != null)
            {
                //theControl.Nodes[ProjectKey].ContextMenuStrip = projectMenu;
// ReSharper disable PossibleNullReferenceException
                //В этом месте не может быть NullReferenceException
                projectNode.ContextMenuStrip = projectMenu;
// ReSharper restore PossibleNullReferenceException
            }
        }

        public override void AddDiagram(string name, DiagramType dType)
        {
            var projectNode = theControl.Nodes[ProjectKey];
            if (projectNode == null)
            {
                throw (new DataException("No project present"));
            }

            for (int i = 0; i < projectNode.Nodes.Count; i++)
            {
                if (projectNode.Nodes[i].Text == name)
                {
                    throw (new DataException("The diagram is already exists!"));
                }
            }

            var diagramNode = projectNode.Nodes.Add(name, name);
            if (diagramMenu != null)
            {
                diagramNode.ContextMenuStrip = diagramMenu;
            }
        }

        public override bool RemoveDiagram(string name)
        {
            var diagramNode = theControl.Nodes.Find(name, true);
            if (diagramNode.Length != 1)
            {
                return false;
            }
            theControl.Nodes.Remove(diagramNode[0]);
            return true;
        }

        public override object GetActiveNode()
        {
            return theControl.SelectedNode;
        }

        public override void ClearNodes()
        {
            theControl.Nodes.Clear();
        }

        public override void NodeDoubleClick(object node)
        {
            //Мы не знаем, что это за узел.
            var projectNode = theControl.Nodes[ProjectKey];

            TreeNode convertedNode = node as TreeNode;
            //Пробуем найти узел в списке диаграмм.
            foreach (var dgr in projectNode.Nodes)
            {
                if (node == dgr)
                { 
                    //Диаграмма найдена!
                    //Открыть диаграмму.
                    string path = Owner.GetDiagramInfo(convertedNode.Text);
                    Form1 form = MainForm as Form1;
                    string name = path.Substring(path.LastIndexOf('\\') + 1);
                    form.LoadDiagram(path, name, convertedNode.Text);
                }
            }
        }
    }
}
