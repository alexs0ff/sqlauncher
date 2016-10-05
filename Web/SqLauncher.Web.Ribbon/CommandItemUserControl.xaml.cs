// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   CommandItemUserControl.xaml.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2012 03 22 19:00
//   * Modified at: 2012  03 22  19:15
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SqLauncher.Web.Ribbon
{
    /// <summary>
    ///   The user control that represents the start button command item.
    /// </summary>
    public partial class CommandItemUserControl : UserControl
    {
        public CommandItemUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///   Gets or sets the enabled mode.
        /// </summary>
        public new bool IsEnabled
        {
            get { return button.IsEnabled; }
            set { button.IsEnabled = value; }
        }

        /// <summary>
        ///   The item caption.
        /// </summary>
        public string Title
        {
            get { return text.Text; }
            set { text.Text = value; }
        }

        /// <summary>
        ///   The item image.
        /// </summary>
        public ImageSource Image
        {
            get { return image.Source; }
            set { image.Source = value; }
        }

        private void ButtonClick( object sender, RoutedEventArgs e )
        {
            RiseClick();
        }

        /// <summary>
        ///   Occurs when user click on item
        /// </summary>
        public event EventHandler<EventArgs> Click;

        public void RiseClick()
        {
            EventHandler<EventArgs> handler = Click;
            if ( handler != null ){
                handler( this, new EventArgs() );
            }
        }
    }
}