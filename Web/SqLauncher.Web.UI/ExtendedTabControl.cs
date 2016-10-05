// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ExtendedTabControl.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 03 21:20
//   * Modified at: 2011  11 03  21:25
// / ******************************************************************************/ 

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SqLauncher.Web.UI
{
    public class ExtendedTabControl : TabControl
    {
        private Button _tabAddItemButton;

        private RepeatButton _tabLeftButton;

        private TabPanel _tabPanelBottom;

        private RepeatButton _tabRightButton;

        private ScrollViewer _tabScrollViewer;

        public ExtendedTabControl()
        {
            DefaultStyleKey = typeof ( ExtendedTabControl );
            SelectionChanged += OnSelectionChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _tabLeftButton = GetTemplateChild("TabLeftButtonBottom") as RepeatButton;
            _tabRightButton = GetTemplateChild("TabRightButtonBottom") as RepeatButton;
            _tabScrollViewer = GetTemplateChild("TabScrollViewerBottom") as ScrollViewer;
            _tabPanelBottom = GetTemplateChild("TabPanelBottom") as TabPanel;
            _tabAddItemButton = GetTemplateChild( "TabAddItemButton" ) as Button;

            if ( _tabLeftButton != null ){
                _tabLeftButton.Click += tabLeftButton_Click;
            }

            if ( _tabRightButton != null ){
                _tabRightButton.Click += tabRightButton_Click;
            }

            if ( _tabAddItemButton != null ){
                _tabAddItemButton.Click += TabAddItemButtonClick;
            }
        }

        private void OnSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            TabItem si = e.AddedItems.Cast<TabItem>().FirstOrDefault();
            if ( si != null ){
                SelectedItem = si.DataContext;
                ScrollToSelectedItem();
            }
            else{
                SelectedItem = null;
            }
        }

        #region Add item functionality

        /// <summary>
        /// Occurs when new user clicks on add new item button.
        /// </summary>
        public event EventHandler AddNewItem;

        /// <summary>
        /// The invocator for AddNewItem event.
        /// </summary>
        private void RiseAddNewItem()
        {
            EventHandler handler = AddNewItem;
            if ( handler != null ){
                handler( this, new EventArgs() );
            }
        }

        private void TabAddItemButtonClick( object sender, RoutedEventArgs e )
        {
            RiseAddNewItem();
        }

        #endregion

        #region Scrollable tabs

        /// <summary>
        ///   Tab Bottom Left Button style
        /// </summary>
        public static readonly DependencyProperty TabLeftButtonBottomStyleProperty = DependencyProperty.Register(
            "TabLeftButtonBottomStyle",
            typeof ( Style ),
            typeof ( ExtendedTabControl ),
            new PropertyMetadata( null ) );

        /// <summary>
        ///   Tab Bottom Left Button style
        /// </summary>
        public static readonly DependencyProperty TabRightButtonBottomStyleProperty = DependencyProperty.Register(
            "TabRightButtonBottomStyle",
            typeof ( Style ),
            typeof ( ExtendedTabControl ),
            new PropertyMetadata( null ) );

        /// <summary>
        ///   Gets or sets the Tab Bottom Left Button style.
        /// </summary>
        /// <value>The left button style style.</value>
        [Description("Gets or sets the tab Bottom left button style")]
        [Category( "ScrollButton" )]
        public Style TabLeftButtonBottomStyle
        {
            get { return (Style)GetValue(TabLeftButtonBottomStyleProperty); }
            set { SetValue(TabLeftButtonBottomStyleProperty, value); }
        }

        /// <summary>
        ///   Gets or sets the Tab Bottom Right Button style.
        /// </summary>
        /// <value>The left button style style.</value>
        [Description("Gets or sets the tab Bottom right button style")]
        [Category( "ScrollButton" )]
        public Style TabRightButtonBottomStyle
        {
            get { return (Style)GetValue(TabRightButtonBottomStyleProperty); }
            set { SetValue(TabRightButtonBottomStyleProperty, value); }
        }

        //It is the function that is called when the control is resized. I need to update scroll and visibility of buttons
        protected override Size ArrangeOverride( Size finalSize )
        {
            Size size = base.ArrangeOverride( finalSize );
            UpdateScrollButtonsAvailability();
            //this.ScrollToSelectedItem(); //If the selected item is hidden - be it so
            return size;
        }

        private void tabRightButton_Click( object sender, RoutedEventArgs e )
        {
            if ( null != _tabScrollViewer && null != _tabPanelBottom ){
                //25 pixels to right
                double currentHorizontalOffset = Math.Min( _tabScrollViewer.HorizontalOffset + 25,
                                                           _tabScrollViewer.ScrollableWidth );

                _tabScrollViewer.ScrollToHorizontalOffset( currentHorizontalOffset );
                UpdateScrollButtonsAvailability( currentHorizontalOffset );
            }
        }

        private void tabLeftButton_Click( object sender, RoutedEventArgs e )
        {
            if ( null != _tabScrollViewer ){
                //25 pixels to left
                double currentHorizontalOffset = Math.Max( _tabScrollViewer.HorizontalOffset - 25, 0 );

                _tabScrollViewer.ScrollToHorizontalOffset( currentHorizontalOffset );
                UpdateScrollButtonsAvailability( currentHorizontalOffset );
            }
        }

        /// <summary>
        ///   Change visibility and avalability of buttons if it is necessary
        /// </summary>
        /// <param name = "horizontalOffset">the real offset instead of outdated one from the scroll viewer</param>
        private void UpdateScrollButtonsAvailability( double? horizontalOffset = null,
                                                      double? extraScrollableWidth = null )
        {
            if ( _tabScrollViewer == null ){
                return;
            }

            double hOffset = horizontalOffset ?? _tabScrollViewer.HorizontalOffset;
            hOffset = Math.Max( hOffset, 0 );

            double scrWidth = _tabScrollViewer.ScrollableWidth - ( extraScrollableWidth ?? 0.0 );
            scrWidth = Math.Max( scrWidth, 0 );

            if ( _tabLeftButton != null ){
                _tabLeftButton.Visibility = scrWidth == 0 ? Visibility.Collapsed : Visibility.Visible;

                _tabLeftButton.IsEnabled = hOffset > 0;
            }
            if ( _tabRightButton != null ){
                double ho = _tabScrollViewer.HorizontalOffset;
                double w1 = _tabScrollViewer.ViewportWidth;
                double w2 = _tabScrollViewer.ScrollableWidth;
                double w3 = _tabScrollViewer.ExtentWidth;

                _tabRightButton.Visibility = scrWidth == 0 ? Visibility.Collapsed : Visibility.Visible;

                _tabRightButton.IsEnabled = hOffset < scrWidth;
            }
        }

        /// <summary>
        ///   Scrolls to a selected tab
        /// </summary>
        private void ScrollToSelectedItem()
        {
            var si = base.SelectedItem as TabItem;
            if ( si == null || si.ActualWidth == 0 || _tabScrollViewer == null ){
                return;
            }

            double leftItemsWidth = Items.Cast<TabItem>().TakeWhile( ti => ti != si ).Sum( ti => ti.ActualWidth );

            //If left size + tab size is not visible and situated somwhere at the right area
            if ( leftItemsWidth + si.ActualWidth >
                 _tabScrollViewer.HorizontalOffset + _tabScrollViewer.ViewportWidth ){
                double currentHorizontalOffset = ( leftItemsWidth + si.ActualWidth ) -
                                                 ( _tabScrollViewer.HorizontalOffset +
                                                   _tabScrollViewer.ViewportWidth );
                double hMargin = _tabPanelBottom.Margin.Left + _tabPanelBottom.Margin.Right;
                currentHorizontalOffset += _tabScrollViewer.HorizontalOffset + hMargin;
                    //Probably 6 = left margin + right margin


                _tabScrollViewer.ScrollToHorizontalOffset( currentHorizontalOffset );
                UpdateScrollButtonsAvailability( currentHorizontalOffset );
            }
                //if selected item somewhere at the left
            else if ( leftItemsWidth < _tabScrollViewer.HorizontalOffset ){
                double currentHorizontalOffset = leftItemsWidth;

                _tabScrollViewer.ScrollToHorizontalOffset( currentHorizontalOffset );
                UpdateScrollButtonsAvailability( currentHorizontalOffset );
            }
        }

        #endregion

        #region Tabs with databinding and templates

        public new static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register( "ItemTemplate", typeof ( DataTemplate ), typeof ( ExtendedTabControl ),
                                         new PropertyMetadata(
                                             ( sender, e ) => ( (ExtendedTabControl) sender ).InitTabs() ) );

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register( "ContentTemplate", typeof ( DataTemplate ), typeof ( ExtendedTabControl ),
                                         new PropertyMetadata(
                                             ( sender, e ) => ( (ExtendedTabControl) sender ).InitTabs() ) );

        public new static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register( "ItemsSource", typeof ( IEnumerable ), typeof ( ExtendedTabControl ),
                                         new PropertyMetadata( OnItemsSourceChanged ) );

        public new static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register( "SelectedItem", typeof ( object ), typeof ( ExtendedTabControl ),
                                         new PropertyMetadata( OnSelectedItemChanged ) );

        /// <summary>
        ///   Gets or sets a DataTemplate for a TabItem header
        /// </summary>
        public new DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue( ItemTemplateProperty ); }
            set { SetValue( ItemTemplateProperty, value ); }
        }

        /// <summary>
        ///   Gets or sets a DataTemplate for a TabItem content
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate) GetValue( ContentTemplateProperty ); }
            set { SetValue( ContentTemplateProperty, value ); }
        }

        /// <summary>
        ///   Gets or sets a collection used to generate the collection of TabItems
        /// </summary>
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue( ItemsSourceProperty ); }
            set { SetValue( ItemsSourceProperty, value ); }
        }

        /// <summary>
        ///   Gets or sets the first item in the current selection or returns null if the selection is empty
        /// </summary>
        public new object SelectedItem
        {
            get { return GetValue( SelectedItemProperty ); }
            set { SetValue( SelectedItemProperty, value ); }
        }

        private static void OnItemsSourceChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
        {
            var control = (ExtendedTabControl) sender;
            var incc = e.OldValue as INotifyCollectionChanged;
            if ( incc != null ){
                incc.CollectionChanged -= control.ItemsSourceCollectionChanged;
            }

            control.InitTabs();

            incc = e.NewValue as INotifyCollectionChanged;
            if ( incc != null ){
                incc.CollectionChanged += control.ItemsSourceCollectionChanged;
            }
        }

        private static void OnSelectedItemChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
        {
            var control = (TabControl) sender;
                //Base class, not extended, because we must change the original SelectedItem property

            if ( e.NewValue == null ){
                control.SelectedItem = null;
            }
            else{
                TabItem tab = control.Items.Cast<TabItem>().FirstOrDefault( ti => ti.DataContext == e.NewValue );
                if ( tab != null && control.SelectedItem != tab ){
                    control.SelectedItem = tab;
                }
            }
        }

        /// <summary>
        ///   Create the collection of TabItems from the collection of clr-objects
        /// </summary>
        private void InitTabs()
        {
            Items.Clear();
            if ( ItemsSource == null ){
                return;
            }

            foreach ( object item in ItemsSource ){
                TabItem newitem = CreateTabItem( item );
                Items.Add( newitem );
            }
        }

        /// <summary>
        ///   Creates the TabItem object from a clr-object
        /// </summary>
        /// <param name = "dataContext">The clr-object which will be set as the DataContext of the TabItem</param>
        /// <returns>The TabItem object</returns>
        private TabItem CreateTabItem( object dataContext )
        {
            var newitem = new ExtendedTabItem();
            
            var hca = new Binding( "HorizontalContentAlignment" ){Source = this};
            BindingOperations.SetBinding( newitem, HorizontalContentAlignmentProperty, hca );

            var vca = new Binding( "VerticalContentAlignment" ){Source = this};
            BindingOperations.SetBinding( newitem, VerticalContentAlignmentProperty, vca );

            if ( ContentTemplate != null ){
                newitem.Content = ContentTemplate.LoadContent();
            }
            else{
                newitem.Content = dataContext;
            }

            if ( ItemTemplate != null ){
                newitem.Header = ItemTemplate.LoadContent();
            }
            else{
                newitem.Header = dataContext;
            }

            newitem.DataContext = dataContext;

            return newitem;
        }

        /// <summary>
        ///   Handles the CollectionChanged event of the ItemsSource property
        /// </summary>
        private void ItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if ( e.Action == NotifyCollectionChangedAction.Add ){
                if ( e.NewItems != null && e.NewStartingIndex > -1 ){
                    foreach ( object item in e.NewItems.OfType<object>().Reverse() ){
                        TabItem newitem = CreateTabItem( item );
                        Items.Insert( e.NewStartingIndex, newitem );
                        newitem.Loaded += OnNewItemLoaded; //this line must be after the insert, I don't know why  
                    }
                }
            }
            else if ( e.Action == NotifyCollectionChangedAction.Remove ){
                if ( e.OldStartingIndex > -1 ){
                    Items.RemoveAt( e.OldStartingIndex );

                    //Update buttons of scroll
                    UpdateScrollButtonsAvailability();
                }
            }
            else if ( e.Action == NotifyCollectionChangedAction.Replace ){
                //I don't know how this action can be called. I would rather ignore it.
            }
            else if ( e.Action == NotifyCollectionChangedAction.Reset ){
                InitTabs();
            }
        }

        private void OnNewItemLoaded( object sender, RoutedEventArgs e )
        {
            var ti = (TabItem) sender;

            if ( ti.IsSelected ){
                ScrollToSelectedItem();
            }
            else //-1 is dirty hack in case if the new item is added, but hasn't received the width
            {
                UpdateScrollButtonsAvailability( null, ti.ActualWidth - 1 );
            }
        }

        #endregion

        /// <summary>
        /// Finds the visual content of tabs by assigned data.
        /// </summary>
        /// <param name="dataContextValue">The data context</param>
        /// <returns>The found visual content or Null.</returns>
        public object GetContentByDataContext(object dataContextValue)
        {
            object result = null;

            var foundedItem = Items.FirstOrDefault( item => ReferenceEquals( ( (TabItem) item ).DataContext, dataContextValue ) );

            if ( foundedItem!=null ){
                result = ( (TabItem) foundedItem ).Content;

            } //if

            return result;
        }
    }
}