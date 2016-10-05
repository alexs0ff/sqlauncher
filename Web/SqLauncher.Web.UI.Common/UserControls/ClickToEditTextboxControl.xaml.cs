// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ClickToEditTextboxControl.xaml.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2012 03 10 16:28
//   * Modified at: 2012  03 10  17:30
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SqLauncher.Web.UI.Common.UserControls
{
    /// <summary>
    ///   Represents the control, which by default renders as a TextBlock but shows a TextBox when the user clicks it.
    /// </summary>
    public partial class ClickToEditTextboxControl : UserControl
    {
        public ClickToEditTextboxControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string) GetValue( TextProperty ); }
            set { SetValue( TextProperty, value ); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register( "Text", typeof ( string ), typeof ( ClickToEditTextboxControl ),
                                         new PropertyMetadata( OnTextChanged ) );

        private static void OnTextChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var control = (ClickToEditTextboxControl) d;
            control.Text = (string) e.NewValue;
        }

        private void TextBoxNameLostFocus( object sender, RoutedEventArgs e )
        {
            ShowTextBlock( sender );
        }

        private static void ShowTextBlock( object sender )
        {
            var txtBlock = (TextBlock) ( (Grid) ( (TextBox) sender ).Parent ).Children[0];

            txtBlock.Visibility = Visibility.Visible;
            ( (TextBox) sender ).Visibility = Visibility.Collapsed;
        }

        private void TextBlockNameMouseDown( object sender, MouseButtonEventArgs e )
        {
            if ( e.ClickCount == 2 ){
                var txtBox = (TextBox) ( (Grid) ( (TextBlock) sender ).Parent ).Children[1];
                txtBox.Visibility = Visibility.Visible;
                ( (TextBlock) sender ).Visibility = Visibility.Collapsed;

                //Solution from http://stackoverflow.com/questions/1892891/how-to-set-focus-on-textbox-in-silverlight-4-out-of-browser-popup
                if ( !Application.Current.IsRunningOutOfBrowser ){
                    //System.Windows.Browser.HtmlPage.Plugin.Focus();
                } //if

                Dispatcher.BeginInvoke( () => txtBox.Focus() );
            } //if
        }

        private void TextBoxKeyDown( object sender, KeyEventArgs e )
        {
            if ( e.Key == Key.Enter ){
                ShowTextBlock( sender );
            } //if
        }
    }
}