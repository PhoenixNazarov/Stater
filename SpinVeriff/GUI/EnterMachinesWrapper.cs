using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpinVeriff.GUI
{
    public abstract class EnterMachinesWrapper
    {
        public string EnteredText { get; set; }
        public OptionsLogic Owner { get; set; }

        protected void SynchronizeET(List<SpinVeriff.Options.ObjectName> objects)
        {
            StringBuilder sb = new StringBuilder();
            if (objects == null)
            {
                objects = new List<Options.ObjectName>();
            }

            foreach (var obj in objects)
            {
                sb.Append(obj.Type);// + " " + obj.Name);
                sb.Append(" ");
                sb.AppendLine(obj.Name);
            }
            EnteredText = sb.ToString();
        }

        #region Interaction with form
        public virtual void SynchronizeObjects(List<SpinVeriff.Options.ObjectName> objects)
        {
            SynchronizeET(objects);
        }
        #endregion

        #region Interaction with logic
        public abstract OptionsLogic.Result Start();
        #endregion

    }
}
