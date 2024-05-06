using System.Collections.Generic;

namespace SpinVeriff.GUI
{
    public abstract class OptionsWrapper
    {
        #region From Logic

        public OptionsLogic Owner;

        public abstract void Init();
        public abstract OptionsLogic.Result Start();
        #endregion

        #region From form
        public virtual void DoOk(string formulaLTL, string singleMachineType)
        {
            Owner.SetLTL(formulaLTL);
        }
        public abstract void DoCancel();
        public abstract void CheckVerifySystem(bool val);
        public abstract void CheckEntered(bool val);
        public abstract void EnterObjects();
        public abstract void CheckLonely(bool val);
        public abstract void CheckOnlyOne(bool val);

        #endregion

        #region To form
        public abstract void AutoCheckVerifySystem(bool val);
        public abstract void AutoCheckEntered(bool val);
        public abstract void AutoCheckLonely(bool val);
        public abstract void AutoCheckOnlyOne(bool val);
        public abstract void SetLTL(string ltl);
        #endregion

    }
}