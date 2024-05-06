using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV
{
    /// <summary>
    /// Интерфейс для паттерна "Наблдатель".
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Функция оповещения о деактивации объекта.
        /// </summary>
        void Update();
    }
}
