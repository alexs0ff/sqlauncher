// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RibbonButton.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:48 AM
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;

namespace SqLauncher.Web.Ribbon
{
    public class RibbonButton : RibbonButtonBase
    {
        public RibbonButton()
        {
            // Attach an events
            this.Loaded += RibbonButton_Loaded;
        }

        private void RibbonButton_Loaded( object sender, RoutedEventArgs e )
        {
            if ( Button == null ){
                Button = new Button();
                if ( Button.Style == null ){
                    Button.Style = buttonStyle;
                }
                //
                ButtonLoaded();
            }
        }
    }
}