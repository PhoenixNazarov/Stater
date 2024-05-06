using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class AutomatonEffect
    {
        /// <summary>
        /// Имя автомата
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип автомата (имя диаграммы с автоматом).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Название события.
        /// </summary>
        public string Event { get; set; }

        public Synchronism Synchronism { get; set; }

    }
}
