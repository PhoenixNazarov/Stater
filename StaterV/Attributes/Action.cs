using System;

namespace StaterV.Attributes
{
    [Serializable]
    public class Action
    {
        public string Name { get; set; }
        public ESynchronism Synchronism { get; set; }
        public string Comment { get; set; }
    }
}
