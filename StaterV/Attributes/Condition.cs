using System;

namespace StaterV.Attributes
{
    /// <summary>
    /// Определяет условие.
    /// </summary>
    [Serializable]
    public class Condition
    {
        public string Body { get; set; }
        public string Description { get; set; }
    }
}
