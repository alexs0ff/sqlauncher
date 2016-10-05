// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RadioList.xaml.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 16 3:11 PM
//   * Modified at: 2011  10 16  7:16 PM
// / ******************************************************************************/ 

using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SqLauncher.Web.UI.Common
{
    /// <summary>
    ///   The radio button list.
    /// </summary>
    public partial class RadioList : UserControl
    {
        public RadioList()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register( "ItemsSource", typeof ( IEnumerable ), typeof ( RadioList ),
                                         new PropertyMetadata( default( IEnumerable ), OnItemsSourceChanged ) );

        /// <summary>
        ///   Occurs when items sourcse rpoperty changed.
        /// </summary>
        /// <param name = "d"></param>
        /// <param name = "e"></param>
        private static void OnItemsSourceChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var userControl = (RadioList) d;

            var collection = new Collection<RadioItem>();

            foreach ( var value in (IEnumerable) e.NewValue ){
                collection.Add( new RadioItem{IsChecked = false, Value = value} );
            } //foreach

            userControl.radioList.ItemsSource = collection;
        }

        /// <summary>
        ///   Gets or sets a collection used to generate the radio button section.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue( ItemsSourceProperty ); }
            set { SetValue( ItemsSourceProperty, value ); }
        }

        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register( "GroupName", typeof ( string ), typeof ( RadioList ),
                                         new PropertyMetadata( default( string ) ) );

        /// <summary>
        ///   The radio buttons group name.
        /// </summary>
        public string GroupName
        {
            get { return (string) GetValue( GroupNameProperty ); }
            set { SetValue( GroupNameProperty, value ); }
        }

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register( "SelectedValue", typeof ( string ), typeof ( RadioList ),
                                         new PropertyMetadata( default( object ), OnSelectedValueChanged ) );

        /// <summary>
        ///   Occurs when selected value changed.
        /// </summary>
        /// <param name = "d"></param>
        /// <param name = "e"></param>
        private static void OnSelectedValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            if ( e.NewValue == null ){
                return;
            } //if

            var radioList = (RadioList) d;
            var enumerator = radioList.radioList.ItemsSource.GetEnumerator();

            for ( int i = 0; enumerator.MoveNext(); i++ ){
                var radioItem = (RadioItem) enumerator.Current;

                if ( e.NewValue.Equals( radioItem.Value ) ){
                    radioItem.IsChecked = true;

                    break;
                } //if
            } //for
        }

        /// <summary>
        ///   Gets or sets the selected name.
        /// </summary>
        public object SelectedValue
        {
            get { return GetValue( SelectedValueProperty ); }
            set { SetValue( SelectedValueProperty, value ); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register( "SelectedIndex", typeof ( int ), typeof ( RadioList ),
                                         new PropertyMetadata( default( int ), OnSelectedIndexChanged ) );

        /// <summary>
        ///   Occurs when SelectedIndexProperty changed.
        /// </summary>
        /// <param name = "d"></param>
        /// <param name = "e"></param>
        private static void OnSelectedIndexChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            if ( e.NewValue == null ){
                return;
            } //if

            var radioList = (RadioList) d;
            var enumerator = radioList.radioList.ItemsSource.GetEnumerator();

            for ( int i = 0; enumerator.MoveNext(); i++ ){
                var radioItem = (RadioItem) enumerator.Current;

                if ( i == (int) e.NewValue ){
                    radioItem.IsChecked = true;

                    break;
                } //if
            } //for
        }

        /// <summary>
        ///   Gets or sets the selected index.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int) GetValue( SelectedIndexProperty ); }
            set { SetValue( SelectedIndexProperty, value ); }
        }

        /// <summary>
        ///   Occurs when user cliks on radio button.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RadioButtonClick( object sender, RoutedEventArgs e )
        {
            var radio = (RadioButton) sender;
            int index = -1;
            object value = null;

            foreach ( object val  in ItemsSource ){
                index++;

                if ( val.Equals( ( (RadioItem) radio.DataContext ).Value ) ){
                    value = val;
                    break;
                } //if
            } //foreach

            if ( value != null ){
                SelectedValue = value;
                SelectedIndex = index;
            } //if
        }
    }
}