using System;

namespace StaterV.Attributes
{
    [Serializable]
    public class Attributes : ICloneable
    {
        public Attributes()
        {
            Name = "";
        }

        public string Name { get; set; }

        public virtual object Clone()
        {
            Attributes res = new Attributes();
            res.Name = Name;
            return res;
        }

        public enum ResultDialog
        {
            OK,
            Cancel,
        }
    }
}
