// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   Ribbon.xaml.cs
//   * Project: SqLauncher.Web.Ribbon
//   * Description:
//   * Created at 2011 09 25 10:46 AM
//   * Modified at: 2011  09 25  10:46 AM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SqLauncher.Web.Ribbon
{
    public partial class Ribbon : StackPanel
    {
        #region ctor

        public Ribbon()
        {
            InitializeComponent();

            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            //
            this.Loaded += Ribbon_Loaded;
            this.Height = 145;
        }

        #endregion

        #region Fields

        internal Style _ribbonTabItemStyle;

        public event RoutedEventHandler //OnRibbonButtonClick, 
            //OnHelpButtonClick, 
            OnMailButtonClick , OnCloseButtonClick , OnHideButtonClick;

        private RibbonPanel _ribbonPanel;

        public delegate void OnSelectionTabChangedEventHandler( object sender, SelectionChangedEventArgs e );

        public event OnSelectionTabChangedEventHandler OnSelectionTabChanged;

        private Tabs _tabs;

        #endregion

        #region Properties

        public Visibility RibbonVisibility
        {
            get { return this.Visibility; }
            set
            {
                this.Visibility = value;
                RibbonButton.Visibility = value;
            }
        }

        public Tabs Tabs
        {
            get { return _tabs; }
            set { _tabs = value; }
        }

        public RibbonPanel RibbonPanel
        {
            get { return _ribbonPanel; }
            set { _ribbonPanel = value; }
        }

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
       

        //public RibbonButton HelpButton
        //{
        //    get { return _helpButton; }
        //}

        //public HyperlinkButton HelpButton
        //{
        //    get { return _helpButton; }
        //}

        public RibbonButton MailButton
        {
            get { return _mailButton; }
        }

        public RibbonButton HideButton
        {
            get { return _hideButton; }
        }

        public RibbonButton CloseButton
        {
            get { return _closeButton; }
        }

        public Grid TabContainer
        {
            get { return tabContainer; }
        }

        //public bool ShowHelpButton
        //{
        //    get
        //    {
        //        if (_helpButton.Visibility == Visibility.Visible)
        //            return true;
        //        else return false;
        //    }
        //    set
        //    {
        //        if (value)
        //            _helpButton.Visibility = Visibility.Visible;
        //        else _helpButton.Visibility = Visibility.Collapsed;
        //    }
        //}

        public bool ShowHideButton
        {
            get
            {
                if ( _hideButton.Visibility == Visibility.Visible ){
                    return true;
                }
                else{
                    return false;
                }
            }
            set
            {
                if ( value ){
                    _hideButton.Visibility = Visibility.Visible;
                }
                else{
                    _hideButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool ShowMailButton
        {
            get
            {
                if ( _mailButton.Visibility == Visibility.Visible ){
                    return true;
                }
                else{
                    return false;
                }
            }
            set
            {
                if ( value ){
                    _mailButton.Visibility = Visibility.Visible;
                }
                else{
                    _mailButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool ShowCloseButton
        {
            get
            {
                if ( _closeButton.Visibility == Visibility.Visible ){
                    return true;
                }
                else{
                    return false;
                }
            }
            set
            {
                if ( value ){
                    _closeButton.Visibility = Visibility.Visible;
                }
                else{
                    _closeButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        //public ImageSource ImageRibbon
        //{
        //    get { return imageRibbon.Source; }
        //    set { imageRibbon.Source = value; }
        //}

        /*
        public bool ImageRibbonVisibillity
        {
            get { return puRibbonButton.IsOpen; }
            set { puRibbonButton.IsOpen = value; }
        }*/

        #endregion

        #region Events

        private void Ribbon_Loaded( object sender, RoutedEventArgs e )
        {
            // search tabControl
            foreach ( UIElement el in this.Children ){
                if ( el is Tabs ){
                    _tabs = el as Tabs;
                    // init tabControl
                    _tabs.SetValue( Canvas.ZIndexProperty, 1 );
                    _tabs.Ribbon = this;
                    _tabs.Height = this.Height - HeaderPanel.Height; //!
                    _tabs.SelectionChanged += new SelectionChangedEventHandler( _tabs_SelectionChanged );
                    if ( this.Children.Contains( _tabs ) ){
                        this.Children.Remove( _tabs );
                    }
                    if ( !tabContainer.Children.Contains( _tabs ) ){
                        tabContainer.Children.Add( _tabs );
                    }
                    _tabs.SetValue( Grid.ColumnProperty, 0 );
                    
                    break;
                }
            }
            //search about control

            // search RibbonPanel 
            foreach ( UIElement el in this.Children ){
                if ( el is RibbonPanel ){
                    RibbonPanel = el as RibbonPanel;
                    RibbonPanel.Margin = new Thickness( 50, 0, 0, 0 );
                    RibbonPanel.SetValue( Canvas.ZIndexProperty, 1 );
                    if ( this.Children.Contains( RibbonPanel ) ){
                        this.Children.Remove( RibbonPanel );
                    }
                    if ( !_topPanel.Children.Contains( RibbonPanel ) ){
                        _topPanel.Children.Insert( 0, RibbonPanel );
                    }
                    break;
                }
            }

            if ( _tabs == null ){
                throw new Exception( "Not valid ribbon format." );
            }

            if ( RibbonTabItemStyle != null ){
                _ribbonTabItemStyle = RibbonTabItemStyle;
            }

            if ( RibbonTabControlStyle != null && _tabs != null ){
                _tabs.ApplyStyle( RibbonTabControlStyle );
            }

            foreach ( var startButtonItem in StartButtonItems ){
                var commandItem = startButtonItem as  CommandItemUserControl;
                if ( commandItem!=null ){
                    commandItem.Click+=( o, args ) => startMenuPopup.IsOpen = false;
                } //if
            } //foreach

            //RibbonButton.Click += new RoutedEventHandler(RibbonButton_Click);
            HideButton.OnClick += HideButton_OnClick;
            //HelpButton.OnClick += new RoutedEventHandler(HelpButton_OnClick);            
            MailButton.OnClick += MailButton_OnClick;
            CloseButton.OnClick += CloseButton_OnClick;
        }

        private bool _tabVisible = true;

        private void HideButton_OnClick( object sender, RoutedEventArgs e )
        {
            if ( this.OnHideButtonClick != null ){
                this.OnHideButtonClick( sender, e );
            }
            else
            {
                if (_tabVisible)
                {
                    Height = 55;
                    _tabVisible = false;
                    _hideButton.ImageUrl = new BitmapImage(new Uri(ExpanderUpImageUri,
                                                      UriKind.Relative)); 
                }
                else
                {
                    RestoreRibbonSize();
                }
            }
        }

        public const string ExpanderUpImageUri = "/SqLauncher.Web.Ribbon;component/img/ExpanderUp.png";

        public const string ExpanderDownImageUri = "/SqLauncher.Web.Ribbon;component/img/ExpanderDown.png";

        private void RestoreRibbonSize()
        {
            if (! _tabVisible ){
                Height = 145;
                _tabVisible = true;
                _hideButton.ImageUrl = new BitmapImage(new Uri(ExpanderDownImageUri,
                                                      UriKind.Relative)); 
            } //if
        }

        private void CloseButton_OnClick( object sender, RoutedEventArgs e )
        {
            if ( this.OnCloseButtonClick != null ){
                this.OnCloseButtonClick( sender, e );
            }
        }

        private void MailButton_OnClick( object sender, RoutedEventArgs e )
        {
            if ( this.OnMailButtonClick != null ){
                this.OnMailButtonClick( sender, e );
            }
        }

        //void HelpButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    if (this.OnHelpButtonClick != null)
        //        this.OnHelpButtonClick(sender, e);
        //}

        private void _tabs_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if ( OnSelectionTabChanged != null ){
                OnSelectionTabChanged( sender, e );
            }
        }

        //void RibbonButton_Click(object sender, RoutedEventArgs e)
        //{            
        //    if (this.OnRibbonButtonClick != null)
        //        this.OnRibbonButtonClick(RibbonButton, e);
        //    else
        //    {
        //        UIElement _element = aboutControl;
        //        if (_ribbonAboutTemplate != null)
        //            _element = _ribbonAboutTemplate;
        //        //
        //        if (_element.Visibility == Visibility.Collapsed)
        //            _element.Visibility = Visibility.Visible;
        //        else
        //            _element.Visibility = Visibility.Collapsed;
        //    }             
        //}

        #endregion

        #region Start menu items 

        /// <summary>
        /// Gets or sets items of start button.
        /// </summary>
        public UIElementCollection StartButtonItems
        {
            get
            {
                return startMenuItemsControl.Children;
            }
        }
      
        private void RibbonButtonClick(object sender, RoutedEventArgs e)
        {
            startMenuPopup.IsOpen = true;
        }

        #endregion Start menu items

        private void TemplateTopUnselected_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RestoreRibbonSize();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RestoreRibbonSize();
        }
    }

    public class RibbonPanel : StackPanel
    {
        public FrameworkElement FindControl( string id )
        {
            FrameworkElement el = null;
            if ( this.Children.Count > 0 && this.Children[0] is RibbonButtonsGroup ){
                foreach ( FrameworkElement e in ( this.Children[0] as RibbonButtonsGroup ).Children ){
                    if ( e.Name == id ){
                        el = e;
                        break;
                    }
                }
            }
            return el;
        }
    }
}