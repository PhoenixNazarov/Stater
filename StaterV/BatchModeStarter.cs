using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace StaterV
{
    class BatchModeStarter
    {
        public BatchModeStarter(string[] args, Form1 form)
        {
            _form = form;
            _maxThreads = 1;
            if (args == null)
            {
                return;
            }
            if (args.Length == 0)
            {
                return;
            }

            var ext = FilesInfo.DetermineExtension(args[0]);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(args[0]);
            if (fileInfo.Attributes == FileAttributes.Directory)
            {
                if (args.Length >= 2)
                {
                    if (BugArg == args[1])
                    {
                        _bugSubdirs = true;
                        _bugDir = args[0];
                    }

                    if (BugOnlyFilesArg == args[1])
                    {
                        _bugSubdirs = true;
                        _bugDir = args[0];
                        _bugOnlyFiles = true;
                    }

                    if (args.Length >= 3)
                    {
                        _bugTemplates = args[2];

                        if (args.Length >= 4)
                        {
                            if (MTArg == args[3])
                            {
                                _maxThreads = DefaultMT;
                                if (args.Length >= 5)
                                {
                                    if (!int.TryParse(args[4], out _maxThreads))
                                    {
                                        _maxThreads = DefaultMT;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (FilesInfo.ProjectExtension == ext)
            {
                _form.StartupProject = args[0];
                //form must open this project.
            }
            else if (FilesInfo.DiagramExtension == ext)
            {
                if (args.Length == 2)
                {
                    if (BugArg == args[1])
                    {
                        //form must create project, write fsm there, make verification and close.
                    }
                }
            }
        }

        private bool _bugSubdirs = false;
        private string _bugDir = "";
        private static string _bugTemplates = "";
        private static string _overallPath = "";
        private static bool _bugOnlyFiles = false;
        private DirectoryInfo _bugDirInfo = null;
        private static int nPassedMachines = 0;
        private int count = 0;

        public const string BugArg = "bug";
        public const string BugOnlyFilesArg = "bugfiles";
        public const string ProjectName = "Auto";
        public const string ProjectFile = "Auto.stp";
        public const string DiagramName = "auto";
        public const string DiagramFile = "auto.xstd";
        public const string OverallReport = "overall.txt";
        public const string MTArg = "mt";
        public const int DefaultMT = 8;

        private readonly Form1 _form;
        private int _maxThreads;
        private static int nThreads;
        private static object lockObj = new object();

        private enum Mode
        {
            Direct,
            Reverse,
        }

        public void Run()
        {
            if (_bugSubdirs)
            {
                GoSubdirs();
            }
            else
            {
                Application.Run(_form);
            }
        }

        private void GoSubdirs()
        {
            nPassedMachines = 0;
            _bugDirInfo = new DirectoryInfo(_bugDir);
            ClearOverallReport();
            if (_maxThreads == 1)
            {
            	GoSubdirs(_bugDir);
            }
            else
            {
            	GoSubdirsMT(_bugDir);
            }

            using (StreamWriter sw = new StreamWriter(_overallPath, true))
            {
                sw.WriteLine("Summary: " + nPassedMachines + "passed verification");
            }
        }

        private void ClearOverallReport()
        {
            _overallPath = _bugDirInfo + "\\" + OverallReport;
            using (StreamWriter sw = new StreamWriter(_overallPath, false))
            {
                sw.Write("");
            }
        }

        private void GoSubdirs(string path)
        {
            var di = new DirectoryInfo(path);
            if (di.Exists)
            {
                var subdirs = di.GetDirectories();
                int limit = 1;//for debug
                foreach (var d in subdirs)
                {
                    GoSubdirs(d.FullName);
                    if (--limit <= 0)
                    {
                        //break;
                    }
                }

                var files = di.GetFiles(DiagramFile);
                if (files.Length != 0)
                {
                    bool errors = true;
                    try
                    {
                        errors = VerifyProject(di.FullName, Mode.Reverse);
                        errors &= VerifyProject(di.FullName, Mode.Direct);
                        ++count;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception in folder " + di.FullName + ": " + ex.Message);
                    }

                    if (!errors)
                    {
                        ++nPassedMachines;
                    }
                    using (StreamWriter sw = new StreamWriter(_overallPath, true))
                    {
                        sw.WriteLine("***** " + nPassedMachines + " of " + count + " passed verification " + DateTime.Now.ToShortTimeString());
                    }
                }
            }
        }

        private void GoSubdirsMT(string path)
        {
            var di = new DirectoryInfo(path);
            if (di.Exists)
            {
                var subdirs = di.GetDirectories();
                int limit = 1;//for debug
                foreach (var d in subdirs)
                {
                    GoSubdirsMT(d.FullName);
                    if (--limit <= 0)
                    {
                        //break;
                    }
                }

                var files = di.GetFiles(DiagramFile);
                if (files.Length != 0)
                {
                    bool errors = true;
                    try
                    {
                        //errors = VerifyProject(di.FullName, Mode.Reverse);
                        //errors &= VerifyProject(di.FullName, Mode.Direct);
                        errors = VerificationMT(di);
                        ++count;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception in folder " + di.FullName + ": " + ex.Message);
                    }

                    if (!errors)
                    {
                        //++nPassedMachines;
                    }
                    lock (lockObj)
                    {
                        using (StreamWriter sw = new StreamWriter(_overallPath, true))
                        {
                            sw.WriteLine("***** " + nPassedMachines + " of " + count + " passed verification " + DateTime.Now.ToShortTimeString());
                        }
                        
                    }
                }
            }            
        }

        private bool VerificationMT(DirectoryInfo di)
        {
            bool errors = true;
            int passedBegin = 0;
            //int localTh = nThreads;
            while (nThreads >= _maxThreads);

            lock (lockObj)
            {
                ++nThreads;
                passedBegin = nPassedMachines;
            }
            //ThreadStart ts = VerifyProc;
            Thread t = new Thread(VeriffMT);
            ThreadParam tp = new ThreadParam();
            tp.path = di.FullName;
            t.Start(tp);
            {
            	--nThreads;
                if (nPassedMachines > passedBegin + 1)
                {
                    nPassedMachines = passedBegin + 1;
                }
            }
            return errors;
        }

        class ThreadParam
        {
            public string path;
            public Mode mode;
        }

        private static void VeriffMT(object param)
        {
            ThreadParam tp = param as ThreadParam;
            if (tp == null)
            {
                return;
            }

            VerifyProc(tp.path, Mode.Reverse);
            VerifyProc(tp.path, Mode.Direct);
        }

        private static void VerifyProc(string path, Mode mode)
        {
            /*
            if (tp == null)
            {
                return;
            }
             * */
            bool errors = true;
            var di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                return;
            }

            var subdirs = di.GetDirectories(ProjectName);
            string prj = "";
            if (di.GetFiles(ProjectFile).Length == 0)
            {
                if (subdirs.Length == 0)
                {
                    prj = CreateProject(di, mode);
                }
                else
                {
                    prj = OverwriteProject(di, mode);
                }

                Form1 f = new Form1();
                f.StartupProject = prj;
                f.BugFSM = true;
                f.BugTemplates = _bugTemplates;
                f.BugOnlyFiles = _bugOnlyFiles;
                Application.Run(f);
                errors = AnalyzeReport(di, mode);
            }

            if (!errors)
            {
                lock (lockObj)
                {
                    ++nPassedMachines;
                }
            }
        } 

        private bool VerifyProject(string path, Mode mode)
        {
            bool errors = true;
            var di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                return true;
            }

            var subdirs = di.GetDirectories(ProjectName);
            string prj = "";
            if (di.GetFiles(ProjectFile).Length == 0)
            {
                if (subdirs.Length == 0)
                {
                    prj = CreateProject(di, mode);
                }
                else
                {
                    prj = OverwriteProject(di, mode);
                }

                Form1 f = new Form1();
                f.StartupProject = prj;
                f.BugFSM = true;
                f.BugTemplates = _bugTemplates;
                f.BugOnlyFiles = _bugOnlyFiles;
                Application.Run(f);
                errors = AnalyzeReport(di, mode);
            }
            return errors;
        }

        private static string OverwriteProject(DirectoryInfo di, Mode mode)
        {
            string prjFolder = di.FullName + "\\" + ProjectName + "\\";
            string res = prjFolder + ProjectFile;
            WriteProjectFile(res);
            WriteVeriffFile(prjFolder, mode);
            File.Copy(di.FullName + "\\" + DiagramFile, di.FullName + "\\" + ProjectName + "\\" + DiagramFile, true);
            return res;
        }

        private static string CreateProject(DirectoryInfo di, Mode mode)
        {
            di.CreateSubdirectory(ProjectName);
            //var projectDir = new DirectoryInfo(di.FullName + "\\" + ProjectName);
            //WriteProjectFile(di.FullName + "\\" + ProjectName + "\\" + ProjectFile);
            return OverwriteProject(di, mode);
        }

        private static void WriteProjectFile(string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sw.WriteLine("<ProjectInfo xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            sw.WriteLine("  <Name>Auto</Name>");
            sw.WriteLine("  <Diagrams>");
            sw.WriteLine("    <DiagramInfo>");
            sw.WriteLine("      <Name>auto</Name>");
            sw.WriteLine("      <Type>StateMachine</Type>");
            sw.WriteLine("    </DiagramInfo>");
            sw.WriteLine("  </Diagrams>");
            sw.WriteLine("</ProjectInfo>");
            sw.Close();
        }

        private static void WriteVeriffFile(string path, Mode mode)
        {
            //if (_bugTemplates == null)
            //{
            //    return;
            //}

            switch (mode)
            {
                case Mode.Direct:
                    System.IO.File.Copy(_bugTemplates + "\\sv_d.sta", path + "\\sv.sta", true);
                    break;
                case Mode.Reverse:
                    System.IO.File.Copy(_bugTemplates + "\\sv_r.sta", path + "\\sv.sta", true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        private static bool AnalyzeReport(DirectoryInfo di, Mode mode)
        {
            var report = CopyReport(mode, di);

            bool errorsFound = HasErrors(report);
            using (StreamWriter sw = new StreamWriter(_overallPath, true))
            {
                sw.WriteLine(di.Name + " " + mode.ToString() + " " + (errorsFound ? "fail (_|_)" : "success!"));
            }
            return errorsFound;
        }

        private static bool HasErrors(string report)
        {
            var text = File.ReadAllText(report);
            if (text.IndexOf("There are errors.") != -1)
            {
                return true;
            }
            if (text.IndexOf("Verification fail!") != -1)
            {
                return true;
            }
            return false;
        }

        private static string CopyReport(Mode mode, DirectoryInfo di)
        {
            string name = "";
            switch (mode)
            {
                case Mode.Direct:
                    name = "report_direct.txt";
                    break;
                case Mode.Reverse:
                    name = "report_reverse.txt";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }

            var dst = di.FullName + "\\" + ProjectName + "\\" + name;
            File.Copy(di.FullName + "\\" + ProjectName + "\\report.txt", dst, true);
            return dst;
        }
    }
}
