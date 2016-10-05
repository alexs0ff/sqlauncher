// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RibbonComboBox.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:55 AM
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;

namespace SqLauncher.Web.Ribbon
{
    public class RibbonComboBox : ComboBox
    {
        public RibbonComboBox()
        {
            this.IsTabStop = false;
        }

        public void SetStyle( Style style )
        {
            if ( this.Style == null ){
                this.Style = style;
            }
        }

        private RibbonItem _ribbonItem;

        public RibbonItem RibbonItem
        {
            get { return _ribbonItem; }
            internal set { _ribbonItem = value; }
        }
    }

    public class RibbonComboBoxItem : ComboBoxItem
    {
    }
}