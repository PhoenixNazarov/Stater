using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginData;
using StaterV.PluginManager;
using StaterV.Project;
using StaterV.Widgets;
using StaterV.Windows;
using StaterV.XMLDiagramParser;
using TD.SandDock;
using StaterV.MachineProperties;
using StateFlow;
//using StaterV.MachineProperties;

namespace StaterV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            StartupProject = "";
            BugFSM = false;
            BugOnlyFiles = false;
            InitializeComponent();
            SetUpExtensions();
            WindowFactory.Instance.SetWindowVariant(WindowVariant.WindowsForms);
        }

        #region File extensions

        private void SetUpExtensions()
        {
            saveFileDialogProject.AddExtension = true;
            saveFileDialogProject.DefaultExt = FilesInfo.ProjectExtension;

            saveFileDialogDiagram.AddExtension = true;
            saveFileDialogDiagram.DefaultExt = FilesInfo.DiagramExtension;

            //openFileDialog1.
        }
        #endregion

        private List<TD.SandDock.TabbedDocument> docList = new List<TD.SandDock.TabbedDocument>();
        private WindowBase window2 = new WindowDotNet();

        private List<WindowDotNet> windows = new List<WindowDotNet>();

        private Project.ProjectManager pm = new ProjectManager();
        private Project.ProjectWindowDotNet projectWindow = new Project.ProjectWindowDotNet();

        private Project.NewProjectForm newProjectForm = new NewProjectForm();
        private Project.NewDiagramForm newDiagramForm = new NewDiagramForm();

        public string WorkFolder
        {
            get { return pm.Info.GetWorkFolder(); }
        }

        private void CreateProjectWindow()
        {
            projectWindow = new Project.ProjectWindowDotNet();
            projectWindow.MainForm = this;
            projectWindow.SetControl(projectView1);
            projectWindow.SetProjectMenu(PMProjectMenuStrip);
            projectWindow.SetDiagramMenu(PMDiagramMenuStrip);
        }

        public string StartupProject { get; set; }
        public bool BugFSM { get; set; }
        public bool BugOnlyFiles { get; set; }
        public string BugTemplates { get; set; }

        private bool bugReady = false;

        private void LoadForm()
        {
            if (("" == StartupProject))
            {
                DefaultLoadingForm();
            }
            else
            {
                LoadStartupPr();
            }
            //Добавить кнопки плагинов.
            var pluginManager = PluginManager.PluginManager.Instance;
            foreach (var buttonPlugin in pluginManager.ButtonPluginList)
            {
                //Добавить кнопку на тулбар.
                ToolStripButton newButton = new ToolStripButton();
                newButton.Text = buttonPlugin.ToString();
                newButton.CheckOnClick = true;
                newButton.Click += new EventHandler(newButton_Click);
                toolStrip1.Items.Add(newButton);
                bplugs[newButton] = buttonPlugin;
                //toolStrip1.
            }

            foreach (var plugin in pluginManager.IndependentPluginList)
            {
                //Добавим кнопку на тулбар.
                ToolStripButton newButton = new ToolStripButton();
                newButton.Text = plugin.ToString();
                newButton.CheckOnClick = true;
                newButton.Click += new EventHandler(IPlugin_Click);
                toolStrip1.Items.Add(newButton);
                iPlugs[newButton] = plugin;
            }

            if (bugReady)
            {
                //Verify.
                StartVeriff();
            }
        }

        private void StartVeriff()
        {
            ButtonPlugin bp = null;
            //Find
            foreach (var buttonPlugin in bplugs)
            {
                if (buttonPlugin.Value.ToString().IndexOf("SpinVeriff") != -1)
                {
                    bp = buttonPlugin.Value;
                    break;
                }
            }

            if (bp != null)
            {
                PluginParams p = new PluginParams(pm);
                bp.SilentStart(p);
            }
            Close();
        }

        private void DefaultLoadingForm()
        {
            CreateNewDocument();
            tabbedDocument1.Close();
            tabbedDocument2.Close();

            CreateProjectWindow();
            pm.Window = projectWindow;

            testButton.CheckOnClick = true;
        }

        private void LoadStartupPr()
        {
            DefaultLoadingForm();
            //TODO: load project!
            if (File.Exists(StartupProject))
            {
                OpenProjectOnLoad();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadForm();
        }

        private Dictionary<ToolStripButton, IndependentPlugin> iPlugs = new Dictionary<ToolStripButton, IndependentPlugin>();
        /// <summary>
        /// New (independend) plugin starter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IPlugin_Click(object sender, EventArgs e)
        {
            var button = sender as ToolStripButton;
            if (button == null)
            {
                return;
            }

            var plugin = iPlugs[button];
            if (plugin != null)
            {
                try
                {
                    IReturn res;
                    if (!plugin.NeedParams)
                    {
                        res = plugin.Start(null);
                    }

                    else
                    {
                        SaveAll();

                        IParams param = pm.CreateParams();
                        res = plugin.Start(param);
                    }

                    if (res.Message != null)
                    {
                        MessageBox.Show(this, res.Message, "Plugin result");
                        ApplyFSM(res);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Не удалось запустить плагин: " + ex.Message, "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ;
                }
            }
        }

        private void ApplyFSM(IReturn res)
        {
            foreach (var machine in res.ChangedMachines)
            {
                //check cur fsm
                bool found = false;
                foreach (var windowDotNet in windows)
                {
                    if (windowDotNet.DiagramName == machine.Type)
                    {
                        found = true;
                        break;
                    }
                }
                //if changed fsm is new
                if (!found)
                {
                    //add fsm to project
                    var w = CreateNewDocument();
                    w.MyProject = pm;
                    w.ImportMachineData(machine);
                    w.FileName = pm.Info.GetWorkFolder() + "\\" + machine.Type + FilesInfo.DiagramExtension;
                    w.GiveDefaultCoords();
                    w.Form = this;
                    windows.Add(w);
                    w.SaveToXML();
                    pm.AddDiagram(w.DiagramName, DiagramType.StateMachine);
                }
            }
        }

        /// <summary>
        /// Old (dependent) plugin starter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newButton_Click(object sender, EventArgs e)
        {
            SaveAll();
            foreach (var item in toolStrip1.Items)
            {
                if (item.GetType() == typeof(ToolStripButton))
                {
                    var btn = (ToolStripButton) item;
                    try
                    {
                        if (btn.Checked)
                        {
                            btn.Checked = false;
                            var plugin = bplugs[btn];
                            if (pm == null)
                            {
                                return;
                            }
                            //List<StateMachine.StateMachine> machines = pm.ExportStateMachines();
                            /*
                        var machines = new List<StateMachine.StateMachine>();
                        foreach (var w in windows)
                        {
                            var m = w.ExportStateMachine();
                            //m.FileName = pm.GetLocation() + "\\" + m.FileName;
                            m.FileName = pm.Info.GetWorkFolder() + "\\" + m.FileName;
                            machines.Add(m);
                        }
                        /*
                        var w = GetActiveWindow();
                        //var machine = new StateMachine.StateMachine();
                        var machines = w.ExportStateMachine();
                        //machines.Name = "A1";
                        //machines.FileName = "A1.cs";
                         * */
                            PluginParams pluginParams = new PluginParams(pm);
                            var res = plugin.Start(pluginParams);
                            MessageBox.Show(res.ToString());
                        }
                    }
                    catch (KeyNotFoundException )
                    {}
                    catch (Exception exception)
                    {
                        MessageBox.Show(this, "Не удалось запустить плагин: " + exception.Message, "Ошибка", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        Console.WriteLine(exception);
                    }
                }
            }
        }

        private Dictionary<ToolStripButton, ButtonPlugin> bplugs = new Dictionary<ToolStripButton, ButtonPlugin>();

        public void ReDraw()
        {
            /*
            WindowDotNet window = new WindowDotNet();
            window.TheControl = tabPage1;
            Widget st = new State(window);

            st.Draw();
             * */
            //window.Clear();
            //window.DrawAll();

            var w = GetActiveWindow();
            if (w != null)
            {
                w.Clear();
                w.DrawAll();
            }
        }

        WindowDotNet window = new WindowDotNet();

        /// <summary>
        /// Определяет активный tabPage.
        /// </summary>
        /// <returns>Активный tabPage</returns>
        private Control GetActivePage()
        {
            return null;
        }

        private WindowDotNet CreateNewDocument()
        {
            var name = "Doc" + docList.Count;
            var w = CreateWindow(name);
            CreateNewDocument(name, w);
            return w;
            //return CreateNewDocument();
        }

        /*
        private WindowDotNet CreateNewDocument(string docName)
        {
            return CreateNewDocument(docName, DiagramType.StateMachine);
        }
         * */

        //private WindowDotNet CreateNewDocument(string docName, DiagramType type)
        private void CreateNewDocument(string docName, WindowBase w)
        {
            TD.SandDock.TabbedDocument doc = new TabbedDocument();

            doc.BackColor = System.Drawing.SystemColors.Window;
            doc.FloatingSize = new System.Drawing.Size(550, 400);
            doc.Guid = System.Guid.NewGuid();
            doc.Location = new System.Drawing.Point(1, 22);
            doc.Name = docName;
            doc.Size = new System.Drawing.Size(703, 235);
            doc.TabIndex = 2;
            doc.Text = docName;
            //doc.BindingContext = tabbedDocument1.BindingContext;
            //doc.DockSituation = DockSituation.Document;
            doc.BindingContext = documentContainer1.BindingContext;

            doc.Enter += tabbedDocument2_Enter;

            if (documentContainer1.Controls.Count == 0)
            {
                documentContainer1.Manager = sandDockManager1;
            }
            doc.Manager = sandDockManager1;

            docList.Add(doc);

            //doc.OpenDocument(WindowOpenMethod.OnScreen);
            documentContainer1.Controls.Add(doc);
            doc.OpenDocument(WindowOpenMethod.OnScreenActivate);

            documentContainer1.Visible = true;

            doc.Click += tabDoc_Click;
            doc.MouseDown += doc_MouseDown;
            doc.MouseUp += doc_MouseUp;
            doc.Invalidated += new InvalidateEventHandler(doc_Invalidated);
            doc.Enter += new EventHandler(doc_Enter);
            doc.Closing += new DockControlClosingEventHandler(doc_Closing);
            doc.DoubleClick += doc_DoubleClick;
            doc.KeyPress += Form1_KeyPress;
            doc.KeyDown += Form1_KeyDown;

            w.TheControl = doc;
            //return CreateWindow(docName);
            if (doc.Name == "Doc0")
            {
                return;
            }
            CloseAuxillaryDoc();
        }

        private WindowDotNet CreateWindow(string docName)
        {
            WindowDotNet newWindow = new WindowDotNet();
            newWindow.DiagramName = docName;
            newWindow.TheWindowData = new StatemachineData();
            newWindow.Form = this;
            windows.Add(newWindow);
            return newWindow;
        }

        void doc_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var w = GetActiveWindow();
                //this.GetMou
                if (w != null)
                {
                    //var p = Control.MousePosition;
                    var p = System.Windows.Forms.Cursor.Position;
                    var pp = w.TheControl.PointToClient(p);
                    w.FrontEndMouseDoubleClick(pp.X, pp.Y);
                }

            }
            catch (NotImplementedException ex)
            {
                MessageBox.Show(this, ex.Message, "Not implemented", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void doc_Closing(object sender, DockControlClosingEventArgs e)
        {
            docList.Remove(documentContainer1.ActiveControl as TabbedDocument);
            var w = GetActiveWindow();
            windows.Remove(w);

            if (documentContainer1.Controls.Count == 1)
            {
                //TODO: Исправить баг: "Все портится, когда закрыты все документы" и убрать это.
                CreateNewDocument();
            }

        }

        private void CloseAuxillaryDoc()
        {
            if (documentContainer1.Controls.Count == 1)
            {
                return;
            }

            WindowDotNet wnd = null;
            foreach (var w in windows)
            {
                if (w.DiagramName == "Doc0")
                {
                    wnd = w;
                    break;
                }
            }

            windows.Remove(wnd);

            TabbedDocument td = null;
            foreach (var tabbedDocument in documentContainer1.Controls)
            {
                if (((TabbedDocument)tabbedDocument).Name == "Doc0")
                {
                    td = tabbedDocument as TabbedDocument;
                    break;
                }
            }
            docList.Remove(td);
            if (td != null)
            {
                td.Close();
            }
        }


        private void testButton_Click(object sender, EventArgs e)
        {
            CreateNewDocument();

            testButton.Checked = false;

            //tabControl2.
            //window.TheControl = tabPage1;
            //window.AddWidget(new State(window));
        }

        void doc_Enter(object sender, EventArgs e)
        {
            ReDraw();
        }

        void doc_Invalidated(object sender, InvalidateEventArgs e)
        {
            ReDraw();
        }

        private void tabDoc_Click(object sender, EventArgs e)
        {
            var w = GetActiveWindow();
        }

        private void doc_MouseDown(object sender, MouseEventArgs e)
        {
            var w = GetActiveWindow();
            if (w != null)
            {
                w.FrontEndMouseDown(e.X, e.Y);
            }
            ReDraw();
        }

        private void doc_MouseUp(object sender, MouseEventArgs e)
        {
            var w = GetActiveWindow();

            if (w != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    w.FrontEndMouseUp(e.X, e.Y);
                }
                if (e.Button == MouseButtons.Right)
                {
                    w.FrontEndRMBClick(e.X, e.Y);
                }
            } 
            ReDraw();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            ReDraw();
        }

        /// <summary>
        /// Определяет окно активного документа.
        /// </summary>
        /// <returns>Окно активного документа.</returns>
        WindowDotNet GetActiveWindow()
        {
            foreach (var w in windows)
            {
                if (w.TheControl == documentContainer1.ActiveControl)
                {
                    return w;
                }
            }
            return null;
        }

        /*
        public void LoadDiagram(string path, string name)
        {
            var w = CreateNewDocument();
            ContinueLoadDiagram(path, name, w);
        }
         * */

        public void LoadDiagram(string path, string name, string diagramName)
        {
            LoadDiagramXML(path, name);
            return;
        }

        /*
        private void ContinueLoadDiagram(string path, string name, WindowDotNet w)
        {
            w.LoadFromFile(path);
            w.SafeFileName = name;
            w.FileName = path;
            w.TheControl.Text = w.SafeFileName;
            w.MyProject = projectWindow.Owner;
            ReDraw();
        }
         */

        private void LoadDiagramXML(string path, string safeName)
        {
            //var agent = new XMLAgent();
            //var w = agent.Parse(path) as WindowDotNet;
            var w = WindowBase.LoadFromXML(path);
            CreateNewDocument(safeName, w);
            if (w != null)
            {
                if (!w.CoordsAreSet)
                {
                    w.GiveDefaultCoords(); 
                }
                w.MyProject = projectWindow.Owner;
                ((WindowDotNet) w).Form = this;
                windows.Add(w as WindowDotNet);
            }
            ReDraw();
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            //window.StateButtonClick();
            var w = GetActiveWindow();
            if (w != null)
            {
                w.StateButtonClick();
            }
            return;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var w = GetActiveWindow();
            w.TransitionButtonClick();
            //window.TransitionButtonClick();
            return;
        }

        public List<Widget> ActiveWidgets = new List<Widget>();

        public void ShowEditWidgetMenu(Control ctrl, float x, float y)
        {
            editWidgetMenu.Show(ctrl, (int)x, (int)y);
        }

        private void tabPage_MouseClick(object sender, MouseEventArgs e)
        {
            /*
            var mouse = new Utility.Dot();
            mouse.x = e.X;
            mouse.y = e.Y;
            window.FrontEndMouseClick(mouse);
             * */
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //Отправляем в window.
                window.FrontEndRMBClick(e.X, e.Y);
                //editWidgetMenu.Show(tabControl1, e.X, e.Y);
            }
            return;
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            return;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Открываем диалоговое окно с выбором файла.
            var w = GetActiveWindow();

            if (w != null)
            {
                DialogResult res = saveFileDialogDiagram.ShowDialog();
                if (res == DialogResult.OK)
                {
                    //window.SaveToFile(saveFileDialog1.FileName);
                    //Сохраняем туда.
                    w.FileName = saveFileDialogDiagram.FileName;
                    w.SafeFileName = w.FileName.Substring(w.FileName.LastIndexOf('\\') + 1);
                    w.SaveToFile(saveFileDialogDiagram.FileName);
                    w.TheControl.Text = w.SafeFileName;
                }
                
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void tabPage1_MouseDown(object sender, MouseEventArgs e)
        {
            var w = GetActiveWindow();
            if (w != null)
            {
                w.FrontEndMouseDown(e.X, e.Y);
            }
            //window.FrontEndMouseDown(e.X, e.Y);
        }

        private void tabPage1_MouseUp(object sender, MouseEventArgs e)
        {
            var w = GetActiveWindow();
            if (w != null)
            {
                w.FrontEndMouseUp(e.X, e.Y);
            }

            //window.FrontEndMouseUp(e.X, e.Y);
            ReDraw();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Undo
            var w = GetActiveWindow();
            if (w != null)
            {
                w.Undo();
            }
            //window.Undo();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void tabbedDocument2_Enter(object sender, EventArgs e)
        {
            var w = GetActiveWindow();
            if (w == null)
            {
                return;
            }
            var data = w.TheWindowData as StatemachineData;
            if (data != null)
            {
                autoRejectToolStripMenuItem.Checked = data.AutoReject;
            }
        }

        private void diagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Вывести окно, в котором будет указан тип диаграммы и название.
            //Создать документ с диаграммой.
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewProject();
        }

        private void CreateNewProject()
        {
            //Вывести окно, в котором будет название проекта.
            var res = newProjectForm.ShowDialog(this);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                //Создать проект.
                if (pm == null)
                {
                    pm = new ProjectManager();
                }
                pm.SetProjectName(newProjectForm.ProjectName, newProjectForm.ProjectLocation);
                DoSaveProject();
            }
        }

        private void diagramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                LoadDiagramXML(openFileDialog1.FileName, openFileDialog1.SafeFileName);
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newDiagramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newDiagramForm.DefaultLocation = pm.GetLocation();
            var res = newDiagramForm.ShowDialog(this);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                var w = CreateWindow(newDiagramForm.TheDiagramInfo.Name);
                CreateNewDocument(newDiagramForm.TheDiagramInfo.Name, w);

                //Сохранить диаграмму.
                w.FileName = pm.Info.GetWorkFolder() + "\\" + 
                    newDiagramForm.TheDiagramInfo.ShowName + FilesInfo.DiagramExtension;
                w.SafeFileName = newDiagramForm.TheDiagramInfo.ShowName + 
                    FilesInfo.DiagramExtension;
                //w.DiagramName = 
                //w.SaveToFile();
                w.SaveToXML();
                w.TheControl.Text = w.SafeFileName;
                w.MyProject = projectWindow.Owner;
                windows.Add(w);

                pm.AddDiagram(newDiagramForm.TheDiagramInfo.Name, newDiagramForm.TheDiagramInfo.Type);

                projectWindow.SetDiagramMenu(PMDiagramMenuStrip);
            }
        }

        private void existingDiargramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Вызвать openFileDialog.
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                //Добавить диаграмму.

                try
                {
                    var diagramName = ProjectManager.ExtractDiagramName(openFileDialog1.SafeFileName);
                    pm.AddDiagram(diagramName, DiagramType.StateMachine);

                    //Открыть XML-диаграмму.
                    //*
                    var agent = new XMLAgent();
                    var w = agent.Parse(openFileDialog1.FileName) as WindowDotNet;
                    var posAgent = new PositionsAgent();
                    CreateNewDocument(openFileDialog1.SafeFileName, w);
                    if (w != null)
                    {
                        w.GiveDefaultCoords();
                        posAgent.LoadPositions(w);
                        w.MyProject = projectWindow.Owner;
                        windows.Add(w);
                    }
                    // */ 

                    //Открыть диаграмму.
                    /*
                    var w = CreateNewDocument();
                    w.LoadFromFile(openFileDialog1.FileName);
                    w.SafeFileName = openFileDialog1.SafeFileName;
                    w.FileName = openFileDialog1.FileName;
                    w.TheControl.Text = w.SafeFileName;
                    //*/

                    projectWindow.SetDiagramMenu(PMDiagramMenuStrip);
                }
                catch (DataException ex)
                {
                    MessageBox.Show(this, ex.Message, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }
        }

        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Сохранить проект.
        }

        private void SaveProjectAs()
        { 
            //Проверить, что открыт проект.
            //Если проет закрыт, выйти.
            //Открыть saveFileDialog.
            var dlgRes = saveFileDialogProject.ShowDialog();

            //Если файл выбран, то 
            if (dlgRes == DialogResult.OK)
            {
                var path = saveFileDialogProject.FileName;
                var name = path;
                //Сохранить проект.
                DoSaveProject(path, name);
            }
        }

        /// <summary>
        /// Сохраняет проект в файл.
        /// </summary>
        /// <param name="path">Пусть к файлу, куда сохранить проект.</param>
        /// <param name="fileName">Короткое имя файла, куда сохранить проект.</param>
        private void DoSaveProject(string path, string fileName)
        { 
            //Проверка: короткое имя файла должно содержаться в полном.

            pm.SetProjectName(fileName);            
        }

        private void DoSaveProject()
        {
            try
            {
                pm.SaveProject();

            }
            catch (Exception e)
            {
                MessageBox.Show("Не могу сохранить проект. " + e.Message,
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Сохраняет диаграмму в текущей открытой вкладке.
        /// </summary>
        private void DoSaveDiagram()
        {
            DoSaveDiagramToXML();
            //return;
            try
            {
                //Найти вкладку.
                //var w = GetActiveWindow();
                //Сохранить диаграмму.
                //w.SaveToFile();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DoSaveDiagramToXML()
        {
            var w = GetActiveWindow();
            try
            {
                w.SaveToXML();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAll()
        {
            try
            {
                foreach (var w in windows)
                {
                    if (w.DiagramName != "Doc0")
                    {
                        w.SaveToXML();
                    }
                }
                DoSaveProject();

            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't save diagram. \n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseProject()
        {
            if (pm == null)
            {
                return;
            }
            pm.Window.ClearNodes();
            pm = null;
            projectWindow = null;
            windows.Clear();

            TabbedDocument td = null;
            foreach (var tabbedDocument in documentContainer1.Controls)
            {
                td = tabbedDocument as TabbedDocument;
                docList.Remove(td);
                if (td != null)
                {
                    td.Close();
                }
            }
        }

        private void ImportFromStateflow()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Stateflow files|*.mdl";
            openFileDialog1.FilterIndex = 0;
            var res = dlg.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                Importer imp = new Importer(dlg.FileName, pm.Info.Location);
                imp.Import();

                foreach (var stateMachine in imp.LoadedFSMs)
                {
                    WindowDotNet wnd = new WindowDotNet();
                    wnd.MyProject = pm;
                    wnd.ImportMachineData(stateMachine);
                    wnd.SaveToXML();
                    CreateNewDocument(wnd.FileName, wnd);
                    wnd.GiveDefaultCoords();
                    wnd.MyProject = projectWindow.Owner;
                    wnd.Form = this;
                    windows.Add(wnd);
                    ReDraw();
                }
            }
        }

        private void KeyPressed(char key)
        {
            var w = GetActiveWindow();
            if (w != null) w.FrontEndKeyPressed(key);
        }

        private void KeyDowned(int key)
        {
            var w = GetActiveWindow();
            if (w != null) w.FrontEndKeyDown(key);
        }

        private void DoLoadDiagramXML()
        {
            /*
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                var agent = new XMLAgent();
                var w = agent.Parse(openFileDialog1.FileName) as WindowDotNet;
                var posAgent = new PositionsAgent();
                CreateNewDocument(openFileDialog1.SafeFileName, w);
                if (w != null)
                {
                    w.GiveDefaultCoords();
                    posAgent.LoadPositions(w);
                    w.MyProject = projectWindow.Owner;
                    windows.Add(w);
                }
                else
                {
                    MessageBox.Show(this, "Window creation error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
             */
        }

        /// <summary>
        /// Обрабатывает запрос на исключение диаграммы из проекта. Спрашивает подтверждение.
        /// </summary>
        /// <param name="node">Узел с диаграммой для удаления.</param>
        private void RequestRemoveDiagram(TreeNode node)
        {
            var res = MessageBox.Show(this, "Are you sure?", "Confirmation", MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                DoRemoveDiagram(node);
            }
        }

        /// <summary>
        /// Исключает диаграмму из проекта.
        /// </summary>
        /// <param name="node"></param>
        private void DoRemoveDiagram(TreeNode node)
        {
            pm.RemoveDiagram(node.Text);
        }

        /// <summary>
        /// Удаляет выделенные виджеты.
        /// </summary>
        private void DeleteSelectedWidgets()
        {
            //Определяем текущее окно.
            var w = GetActiveWindow();
            //Отправляем в него команду на удаление.
            w.DeleteSelectedWidgets();
        }

        /// <summary>
        /// Подаёт команду в окно автомата на редактирование переменных.
        /// </summary>
        private void OpenVariablesWindow()
        {
            var w = GetActiveWindow();
            w.ShowVariablesForm();
        }

        private void ChangeAutoreject(bool autoReject)
        {
            var w = GetActiveWindow();
            w.ChangeAutoReject(autoReject);
        }

        public void ShowFSMMenu()
        {
            machineMenu.Show(Cursor.Position);
        }

        private void RequestProperties(TreeNode node)
        {
            MachinePropertiesLogic logic = new MachinePropertiesLogic(pm.ExportMachine(node.Text));
            logic.Start();

            //Найти открытую диаграмму, если есть.
            //Применить к ней изменения.
            //Если нет, записать изменения в файл.
        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Открыть проект.
            DoOpenProject();
        }

        /// <summary>
        /// Загружает проект из файла.
        /// </summary>
        private void DoOpenProject()
        {
            CloseProject();
            openFileDialog1.Filter = "Project files|*" + FilesInfo.ProjectExtension;
            openFileDialog1.FilterIndex = 0;
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                pm = new ProjectManager();
                CreateProjectWindow();
                pm.Window = projectWindow;
                pm.LoadProject(openFileDialog1.FileName);
            }
        }

        private void OpenProjectOnLoad()
        {
            CloseProject();
            pm = new ProjectManager();
            CreateProjectWindow();
            pm.Window = projectWindow;
            pm.LoadProject(StartupProject);
            if (BugFSM)
            {
                bugReady = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSaveDiagram();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSaveProject();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void projectView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            pm.NodeDoubleClick(e.Node);
        }

        private void editWidgetMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestRemoveDiagram(projectWindow.GetActiveNode() as TreeNode);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSaveDiagramToXML();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            DoOpenProject();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            DoSaveDiagram();
        }

        private void diagramXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoLoadDiagramXML();
        }


        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DeleteSelectedWidgets();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestProperties(projectWindow.GetActiveNode() as TreeNode);
        }

        private void variablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVariablesWindow();
        }

        private void autoRejectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //autoRejectToolStripMenuItem.CheckState == CheckState.Checked
            ChangeAutoreject(autoRejectToolStripMenuItem.Checked);
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProject();
        }

        private void importFromStateflowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFromStateflow();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressed(e.KeyChar);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDowned(e.KeyValue);
        }
    }
}
