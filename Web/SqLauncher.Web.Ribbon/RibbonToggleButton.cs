// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RibbonToggleButton.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:56 AM
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls.Primitives;

namespace SqLauncher.Web.Ribbon
{
    public class RibbonToggleButton : RibbonButtonBase
    {
        public RibbonToggleButton()
        {
            // Attach an events
            this.Loaded += RibbonButton_Loaded;
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get
            {
                if ( Button != null ){
                    return ( Button as ToggleButton ).IsChecked.Value;
                }
                else{
                    return _isChecked;
                }
            }
            set
            {
                if ( Button != null ){
                    ( Button as ToggleButton ).IsChecked = value;
                }
                else{
                    _isChecked = value;
                }
            }
        }

        private void RibbonButton_Loaded( object sender, RoutedEventArgs e )
        {
            if ( Button == null ){
                Button = new ToggleButton();
                if ( Button.Style == null ){
                    Button.Style = toggleButtonStyle;
                }
                //
                ButtonLoaded();
            }

            //
            ( Button as ToggleButton ).IsChecked = _isChecked;
        }
    }
}