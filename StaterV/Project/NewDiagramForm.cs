using System.Windows.Forms;

namespace StaterV.Project
{
    public partial class NewDiagramForm : Form
    {
        public NewDiagramForm()
        {
            InitializeComponent();
        }

        //public string DiagramName { get; private set; }
        //public DiagramType TheDiagramType { get; private set; }

        public string DefaultLocation { get; set; }

        private DiagramInfo theDiagramInfo;
        public DiagramInfo TheDiagramInfo
        {
            get { return theDiagramInfo; }
            set { theDiagramInfo = value; }
        }

        #region Actions
        private void CreateDiagram()
        {
            //Создать диаграмму.
            theDiagramInfo.ShowName = diagramNameTextBox.Text;
            theDiagramInfo.CreateName();

            theDiagramInfo.Location = DefaultLocation;

            if (rbStateMachine.Checked)
            {
                //TheDiagramType = DiagramType.StateMachine;
                theDiagramInfo.Type = DiagramType.StateMachine;
            }
            else if (rbLinkDiagram.Checked)
            {
                theDiagramInfo.Type = DiagramType.Connections;
            }
        }
        #endregion

        #region Event handlers
        private void okButton_Click(object sender, System.EventArgs e)
        {
            CreateDiagram();
        }
        #endregion
    }
}
