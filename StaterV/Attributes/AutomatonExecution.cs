using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Attributes
{
    /// <summary>
    /// Класс описывающий запуск автомата в новом потоке.
    /// </summary>
    [Serializable]
    public class AutomatonExecution
    {
        /// <summary>
        /// Имя автомата
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип автомата (имя диаграммы с автоматом).
        /// </summary>
        public string Type { get; set; }
    }
}
