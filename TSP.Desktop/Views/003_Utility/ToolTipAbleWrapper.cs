using System;
using System.Windows;
using System.Windows.Controls;

namespace TSP.Desktop.Views.Utility
{
    public class ToolTipAbleWrapper
    {
        public UIElement Element { get; private set; }
        private object toolTip;

        public object ToolTip {
            get => toolTip; 
            set {
                toolTip = value;
                SetTooltip(toolTip);
            }
        }

        public ToolTipAbleWrapper(UIElement element)
        {
            this.Element = element;

            switch (element)
            {
                case TextBox textBox:
                    toolTip = textBox.ToolTip;
                    break;

                case ComboBox comboBox:
                    toolTip = comboBox.ToolTip;
                    break;

                default: throw new ArgumentException();
            }
        }

        private void SetTooltip(object tooltip)
        {
            switch (Element)
            {
                case TextBox textBox:
                    textBox.ToolTip = toolTip;
                    break;

                case ComboBox comboBox:
                    comboBox.ToolTip = toolTip;
                    break;

                default: throw new ArgumentException();
            }
        }
    }
}
