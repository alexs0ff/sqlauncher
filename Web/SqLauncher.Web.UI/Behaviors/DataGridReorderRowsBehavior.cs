// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DataGridReorderRowsBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 03 05 20:03
//   * Modified at: 2012  03 06  21:34
// / ******************************************************************************/ 

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

using SqLauncher.Web.UI.Common;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   represents the bahavior for grid rows reordering.
    /// </summary>
    public class DataGridReorderRowsBehavior : Behavior<DataGrid>
    {
        /// <summary>
        /// Occurs when an items has been reordered.
        /// </summary>
        public event EventHandler<DataGridReorderItemsEventArgs> ItemReordering;

        /// <summary>
        /// Rises the ItemReordering event.
        /// </summary>
        /// <param name="reorderedItem"></param>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        private void RiseItemReordering(  object reorderedItem, int oldIndex, int newIndex )
        {
            EventHandler<DataGridReorderItemsEventArgs> handler = ItemReordering;
            if ( handler != null ){
                handler( this, new DataGridReorderItemsEventArgs( reorderedItem, oldIndex, newIndex ) );
            }
        }

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += DataGridMouseLeftButtonDown;
            AssociatedObject.MouseMove += DataGridMouseMove;
            AssociatedObject.MouseLeftButtonUp += DataGridMouseLeftButtonUp;
            AssociatedObject.LoadingRow += LoadingRow;
            AssociatedObject.UnloadingRow += UnloadingRow;
            AssociatedObject.BeginningEdit += DataGridBeginningEdit;
            AssociatedObject.CellEditEnding +=  DataGridCellEditEnding;
            _isDragging = false;
            _draggedPopup = new Popup();
            _draggedPopup.Child = DraggedElementPattern;
            _draggedPopup.IsHitTestVisible = false;
        }

        /// <summary>
        /// Gets or sets current dataggrid editing mode.
        /// </summary>
        private bool _isEditing;

        /// <summary>
        /// Occurs when user stops editing within grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataGridCellEditEndingEventArgs"></param>
        private void DataGridCellEditEnding( object sender, DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs )
        {
            _isEditing = false;
        }

        /// <summary>
        /// Occurs when user editing some value within grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            _isEditing = true;

            if ( _isDragging ){
                _isDragging = false;
                _draggedPopup.IsOpen = false;
                _popupIsOpened = false;
                AssociatedObject.ReleaseMouseCapture();
            } //if
        }

        /// <summary>
        /// Occurs when datagrid unloding row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.RemoveHandler(UIElement.MouseLeftButtonDownEvent,
                              new MouseButtonEventHandler(DataGridMouseLeftButtonDown));
        }

        public static readonly DependencyProperty DraggedElementPatternProperty =
            DependencyProperty.Register( "DraggedElementPattern", typeof ( UIElement ),
                                         typeof ( DataGridReorderRowsBehavior ),
                                         new PropertyMetadata( default( UIElement ) ) );

        /// <summary>
        ///   The visible pattern of dragged element.
        /// </summary>
        public UIElement DraggedElementPattern
        {
            get { return (UIElement) GetValue( DraggedElementPatternProperty ); }
            set { SetValue( DraggedElementPatternProperty, value ); }
        }

        /// <summary>
        ///   Occurs when data grid loading row.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void LoadingRow( object sender, DataGridRowEventArgs e )
        {
            e.Row.AddHandler( UIElement.MouseLeftButtonDownEvent,
                              new MouseButtonEventHandler(DataGridMouseLeftButtonDown), true);
        }

        private Popup _draggedPopup;

        /// <summary>
        ///   Gets or sets dragging mode.
        /// </summary>
        private bool _isDragging;

        private bool _popupIsOpened;

        /// <summary>
        ///   Gets or sets current item.
        /// </summary>
        private DataGridRow ReorderingRow { get; set; }

        /// <summary>
        ///   Occurs when user left mouse button up on the grid.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void DataGridMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            if (!_isDragging)
            {
                return;
            } //if

            AssociatedObject.ReleaseMouseCapture();
            _isDragging = false;
            _draggedPopup.IsOpen = false;
            _popupIsOpened = false;

            var row = GetRowUnderMousePointer( e );
            if ( row != null && !ReferenceEquals( row,ReorderingRow )){
                var list = AssociatedObject.ItemsSource as IList;

                if ( list!=null ){
                    var  oldIndex = list.IndexOf( ReorderingRow.DataContext );
                    var newIndex = list.IndexOf( row.DataContext );
                    RiseItemReordering(ReorderingRow.DataContext,oldIndex ,
                                    newIndex);
                } //if
            } //if
        }

        /// <summary>
        ///   Gets the row under mouse pointer.
        /// </summary>
        /// <param name = "e"></param>
        /// <returns>The row or null.</returns>
        private DataGridRow GetRowUnderMousePointer( MouseEventArgs e )
        {
            return ControlHelper.FindElementOnGlobalCoordinates<DataGridRow>( AssociatedObject,
                                                                              e.GetPosition(
                                                                                  Application.Current.RootVisual ) );
        }

        /// <summary>
        ///   Occurs when mouse move under datagrid.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void DataGridMouseMove( object sender, MouseEventArgs e )
        {
            if ( !_isDragging ){
                return;
            } //if

            if ( _popupIsOpened ){
                UpdatePopupPosition( e );
            } //if
            else{
                var row = GetRowUnderMousePointer( e );

                if ( !ReferenceEquals( row, ReorderingRow ) ){
                    _popupIsOpened = true;
                    _draggedPopup.IsOpen = true;
                } //if
            } //else
        }

        /// <summary>
        ///   Occurs when mouse left button down on datagrid.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void DataGridMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( _isEditing ){
                return;
            } //if

            var row = GetRowUnderMousePointer( e );
            if ( row == null ){
                return;
            } //if

            _isDragging = true;
            ReorderingRow = row;
            AssociatedObject.CaptureMouse();
            _draggedPopup.DataContext = row.DataContext;
        }

        /// <summary>
        ///   The horizontal offset from mouse pointr.
        /// </summary>
        private const int HorizontalOffset = 6;

        /// <summary>
        ///   The vertical offset from mouse pointr.
        /// </summary>
        private const int VerticalOffset = 2;

        /// <summary>
        ///   Updates the dragged popup position relative to mouse pointer.
        /// </summary>
        /// <param name = "e">The mouse event rags.</param>
        private void UpdatePopupPosition( MouseEventArgs e )
        {
            var position = e.GetPosition( Application.Current.RootVisual );
            _draggedPopup.HorizontalOffset = position.X + HorizontalOffset;
            _draggedPopup.VerticalOffset = position.Y + VerticalOffset;
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= DataGridMouseLeftButtonDown;
            AssociatedObject.MouseMove -= DataGridMouseMove;
            AssociatedObject.MouseRightButtonUp -= DataGridMouseLeftButtonUp;
            AssociatedObject.LoadingRow -= LoadingRow;
        }
    }
}