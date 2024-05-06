using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SpinVeriff.GUI
{
    public class OptionsLogic
    {
        private OptionsWrapper wrapper;
        private EnterMachinesWrapper emWrapper;
        private Options options;

        public Options GetOptions()
        {
            return options;
        }

        private const string StateFile = "sv.sta";

        public OptionsLogic()
        {
            options = new Options();
            wrapper = new OptionsFormWrapper();
            wrapper.Owner = this;
            wrapper.Init();

            emWrapper = new EnterMachinesFormWrapper();
            emWrapper.Owner = this;
        }

        public enum Result
        {
            OK,
            Cancel,
        }

        public Result Start(string wd)
        {
            TryLoadState(wd);
            var res = wrapper.Start();
            switch (res)
            {
                case Result.OK:
                    TrySaveState(wd);
                    break;
                case Result.Cancel:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return res;
        }

        public void TrySaveState(string wd)
        {
            try
            {
                string path = wd + "\\" + StateFile;
                XmlSerializer serializer = new XmlSerializer(typeof(Options));
                TextWriter sw = new StreamWriter(path);
                serializer.Serialize(sw, options);
                sw.Close();
            }
            catch (Exception e)
            {
            }
        }

        public bool TryReadOptions(string wd)
        {
            try
            {
                string path = wd + "\\" + StateFile;
                XmlSerializer serializer = new XmlSerializer(typeof (Options));
                using (TextReader tr = new StreamReader(path))
                {
                    options = (Options) serializer.Deserialize(tr);
                }
                //tr.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void TryLoadState(string wd)
        {
            try
            {
                //Загрузить
                TryReadOptions(wd);

                //Отправить в форму.
                StringBuilder formulae = new StringBuilder();
                foreach (var f in options.FormulaeLTL)
                {
                    formulae.AppendLine(f);
                }

                wrapper.SetLTL(formulae.ToString().Trim());
                emWrapper.SynchronizeObjects(options.EnteredObjects);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not load state");
            }
        }

        #region Interation with form
        public void DoOk()
        {
            
        }

        public void SetLTL(string formulae)
        {
            options.FormulaeLTL = new List<string>();
            var lines = formulae.Split('\n');
            foreach (var f in lines)
            {
                var tf = f.Trim();
                if (tf != "")
                {
                    options.FormulaeLTL.Add(tf);
                }
            }
            //options.FormulaLTL = formulae;
        }

        public void DoCancel()
        {}

        public void CheckVerifySystem(bool val)
        {
            options.SetVerifySystem(val);
            AutoCheckValues();
        }

        public void CheckEntered(bool val)
        {
            options.SetEnteredObjects(val);
            AutoCheckValues();
        }

        public void EnterObjects()
        {
            //EnterMachinesFormWrapper mf = new EnterMachinesFormWrapper();
            //var res = mf.Start();

            emWrapper.SynchronizeObjects(options.EnteredObjects);
            var res = emWrapper.Start();

            if (res == Result.OK)
            {
                ParseMachinesText(emWrapper.EnteredText);
            }
        }

        public void CheckLonely(bool val)
        {
            options.SetFillLonelyAutomatons(val);
            AutoCheckValues();
        }

        public void CheckOnlyOne(bool val)
        {
            options.SetOnlyOneMache(val);
            AutoCheckValues();
        }
        #endregion

        #region Auxiliary functions for interacting with form.
        private void AutoCheckValues()
        {
            wrapper.AutoCheckEntered(options.VerifyEnteredObjects);
            wrapper.AutoCheckLonely(options.FillLonelyAutomata);
            wrapper.AutoCheckOnlyOne(options.VerifyOnlyOneMachine);
            wrapper.AutoCheckVerifySystem(options.VerifySystem);
        }

        private bool ParseMachinesText(string machinesText)
        {
            var text = machinesText.Trim();
            var lines = text.Split('\n');
            List<Options.ObjectName> newObjects = new List<Options.ObjectName>();
            foreach (var line in lines)
            {
                var tokens = line.Split(' ');
                bool typeFound = false;
                bool nameFound = false;

                Options.ObjectName obj = new Options.ObjectName();
                foreach (var token in tokens)
                {
                    if (token.Trim() != "")
                    {
                        if (!typeFound)
                        {
                            typeFound = true;
                            obj.Type = token;
                        }
                        else if (!nameFound)
                        {
                            nameFound = true;
                            obj.Name = token;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                newObjects.Add(obj);
            }
            options.EnteredObjects = newObjects;
            return true;
        }

        #endregion
    }
}
