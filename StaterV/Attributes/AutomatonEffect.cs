using System;

namespace StaterV.Attributes
{
    /// <summary>
    /// Класс, описывающий воздействие одного автомата на другой.
    /// Воздействовать можно передав событие автоматы.
    /// </summary>
    [Serializable]
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

        public ESynchronism Synchronism { get; set; }

        public string Description { get; set; }

        public enum EffectType
        {
            Automatic, //User chooses a statemachine and an event from existing variants.
            Manual, //User manually writes both of statemachine and event.
        }

        public EffectType TheEffectType { get; set; }
    }
}
