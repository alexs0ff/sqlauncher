using System;
using System.Windows.Media;

namespace SqLauncher.Web.Ribbon.SilverlightColorPicker
{
    public class ColorPickerEventArgs : EventArgs
    {
        private readonly Color _selectedColor;

        public Color SelectedColor
        {
            get { return _selectedColor; }
        }

        public ColorPickerEventArgs(Color c)
        {
            _selectedColor = c;
        }
    }
}
