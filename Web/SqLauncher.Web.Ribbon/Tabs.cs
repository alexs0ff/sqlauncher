// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   Tabs.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:56 AM
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;



namespace SqLauncher.Web.Ribbon
{
    public class Tabs : TabControl
    {
        public Tabs()
        {
            this.Margin = new Thickness( 0, 6, -66, 0 );
            this.Loaded += Tabs_Loaded;
            this.IsTabStop = false;
        }

        private void Tabs_Loaded( object sender, RoutedEventArgs e )
        {
            foreach ( TabsItem tabsitem in this.Items ){
                tabsitem.Tabs = this;
            }
        }

        internal void ApplyStyle( Style style )
        {
            if ( this.Style == null ){
                this.Style = style;
            }
        }

        private Ribbon _ribbon;

        public Ribbon Ribbon
        {
            get { return _ribbon; }
            set { _ribbon = value; }
        }

        public TabsItem this[ string name ]
        {
            get
            {
                TabsItem item = null;
                foreach ( TabsItem ti in this.Items ){
                    if ( ti.Name == name ){
                        item = ti;
                        break;
                    }
                }
                return item;
            }
        }
    }
}