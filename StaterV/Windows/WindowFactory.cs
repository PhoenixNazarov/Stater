using System;

namespace StaterV.Windows
{
    public enum WindowVariant
    {
        WindowsForms,
        WPF,
    }

    public class WindowFactory
    {
        private static WindowFactory theInstance = new WindowFactory();
        public static WindowFactory Instance
        {
            get
            {
                return theInstance;
            }
        }

        private WindowVariant variant = WindowVariant.WindowsForms;

        public void SetWindowVariant(WindowVariant _variant)
        {
            variant = _variant;
        }
        public WindowBase CreateWindow()
        {
            switch (variant)
            {
                case WindowVariant.WindowsForms:
                    return new WindowDotNet();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
