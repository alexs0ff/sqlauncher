// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SelectAndResizeEntityFormBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 10 10:48 PM
//   * Modified at: 2011  09 15  9:53 PM
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Shapes;

using SqLauncher.Web.UI.Common;
using SqLauncher.Web.UI.Common.Converters;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   Represents bahavior for selection a form.
    /// </summary>
    public class SelectAndResizeEntityFormBehavior : Behavior<EntityForm>
    {
        /// <summary>
        ///   The style name of rectangle.
        /// </summary>
        public const string RectangleStyleName = "SelectionRectangleStyle";

        /// <summary>
        ///   The left rectangle of selection.
        /// </summary>
        private readonly Rectangle _leftRect = new Rectangle();

        /// <summary>
        ///   The top rectangle of selection.
        /// </summary>
        private readonly Rectangle _topRect = new Rectangle();

        /// <summary>
        ///   The right rectangle of selection.
        /// </summary>
        private readonly Rectangle _rightRect = new Rectangle();

        /// <summary>
        ///   The bottom rectangle of selection.
        /// </summary>
        private readonly Rectangle _bottomRect = new Rectangle();

        /// <summary>
        ///   The main parent canvas.
        /// </summary>
        private Canvas _parrentCanvas;

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += EntityFromLoaded;

            _leftRect.Style = (Style) AssociatedObject.Resources.MergedDictionaries[0][RectangleStyleName];
            _topRect.Style = (Style) AssociatedObject.Resources.MergedDictionaries[0][RectangleStyleName];
            _rightRect.Style = (Style) AssociatedObject.Resources.MergedDictionaries[0][RectangleStyleName];
            _bottomRect.Style = (Style) AssociatedObject.Resources.MergedDictionaries[0][RectangleStyleName];
            AssociatedObject.PositionChanging += EntityFormPositionChanging;
            AssociatedObject.SizeChanged += EntityFormSizeChanged;
            BindToVisibility( _leftRect );
            BindToVisibility( _topRect );
            BindToVisibility( _rightRect );
            BindToVisibility( _bottomRect );

            SelectionIsVisible = false;
        }

        /// <summary>
        /// Ocurs when assitiated entity form size changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args,</param>
        private void EntityFormSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ProcessEntityFormPositionChanged( new Point( AssociatedObject.GetLeft(), AssociatedObject.GetTop() ) );
        }

        /// <summary>
        ///   Occurs when entity form position changing.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFormPositionChanging( object sender, PositionChangingEventArgs e )
        {
            ProcessEntityFormPositionChanged( e.Position );
        }

        /// <summary>
        ///   Occurs when EntityFrom loaded.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFromLoaded( object sender, RoutedEventArgs e )
        {
            AssociatedObject.Loaded -= EntityFromLoaded;
            _parrentCanvas = ControlHelper.FindParent<Canvas>( AssociatedObject );
            AttachRectangles();

            _leftRect.MouseLeftButtonDown += LeftRectMouseLeftButtonDown;
            _leftRect.MouseLeftButtonUp += RectMouseLeftButtonUp;
            _leftRect.MouseMove += RectMouseMove;

            _rightRect.MouseLeftButtonDown += RightRectMouseLeftButtonDown;
            _rightRect.MouseLeftButtonUp += RectMouseLeftButtonUp;
            _rightRect.MouseMove += RectMouseMove;

            _topRect.MouseLeftButtonDown += TopRectMouseLeftButtonDown;
            _topRect.MouseLeftButtonUp += RectMouseLeftButtonUp;
            _topRect.MouseMove += RectMouseMove;

            _bottomRect.MouseLeftButtonUp += RectMouseLeftButtonUp;
            _bottomRect.MouseLeftButtonDown += BottomRectMouseLeftButtonDown;
            _bottomRect.MouseMove += RectMouseMove;

            _canvasZIndexChangeNotifier = new PropertyChangeNotifier( AssociatedObject, "(Canvas.ZIndex)" );
            _canvasZIndexChangeNotifier.ValueChanged += EntityFormZIndexValueChanged;
            ProcessZIndex();
        }

        /// <summary>
        /// Adds to canvas rectangles.
        /// </summary>
        private void AttachRectangles()
        {
            if ( _parrentCanvas!=null && !_parrentCanvas.Children.Contains( _leftRect ) ){
                _parrentCanvas.Children.Add(_leftRect);
                _parrentCanvas.Children.Add(_rightRect);
                _parrentCanvas.Children.Add(_topRect);
                _parrentCanvas.Children.Add(_bottomRect);
            } //if
        }

        /// <summary>
        /// Removes from the parent canvas all rectangles.
        /// </summary>
        private void DetachRectangels()
        {
            if ( _parrentCanvas == null ){
                return;
            }

            if ( _parrentCanvas.Children.Contains( _leftRect ) ){
                _parrentCanvas.Children.Remove( _leftRect );
                _parrentCanvas.Children.Remove( _rightRect );
                _parrentCanvas.Children.Remove( _topRect );
                _parrentCanvas.Children.Remove( _bottomRect );
            } //if
        }

        /// <summary>
        ///   Resized a relation form.
        /// </summary>
        /// <param name = "rectangle"></param>
        /// <param name = "e"></param>
        private void Resize( Rectangle rectangle, MouseEventArgs e )
        {
            var mousePosition = e.GetPosition( (UIElement) rectangle.Parent );

            var newRect = GetCurrentRect();

            bool isResized = false;
            bool isChangedX = false;
            bool isChangedY = false;

            double offsetX = mousePosition.X - _initMousePosition.X;
            double offsetY = mousePosition.Y - _initMousePosition.Y;

            switch ( _currentProcessedPart ){
                case ControlParts.Left:
                    if ( _initRect.Width - offsetX > AssociatedObject.MinWidth ){
                        newRect.X = _initRect.X + offsetX;
                        isChangedX = true;
                        newRect.Width = _initRect.Width - offsetX;
                        isResized = true;
                    }
                    break;
                case ControlParts.Right:
                    if (_initRect.Width + offsetX > AssociatedObject.MinWidth){
                        newRect.Width = _initRect.Width + offsetX;
                        isResized = true;
                    }
                    break;
                case ControlParts.Top:
                    if (_initRect.Height - offsetY > AssociatedObject.MinHeight){
                        newRect.Y = _initRect.Y + offsetY;
                        isChangedY = true;
                        newRect.Height = _initRect.Height - offsetY;
                        isResized = true;
                    }
                    break;
                case ControlParts.Bottom:
                    if (_initRect.Height + offsetY > AssociatedObject.MinHeight){
                        newRect.Height = _initRect.Height + offsetY;
                        isResized = true;
                    }
                    break;
            }

            if ( isResized ){
                AssociatedObject.EntityView.Width = newRect.Width;
                AssociatedObject.EntityView.Height = newRect.Height;

                if ( isChangedX ){
                    Canvas.SetLeft( AssociatedObject, newRect.X );
                }

                if ( isChangedY ){
                    Canvas.SetTop( AssociatedObject, newRect.Y );
                }

                UpdateRectanglesPosition( false, new Point( newRect.X, newRect.Y ) );
            }
        }

        /// <summary>
        ///   Gets a current rect of view form.
        /// </summary>
        /// <returns>The calculated rect.</returns>
        private Rect GetCurrentRect()
        {
            var result = new Rect();
            result.X = Canvas.GetLeft( AssociatedObject );
            result.Y = Canvas.GetTop( AssociatedObject );

            result.Height = AssociatedObject.EntityView.ActualHeight;
            result.Width = AssociatedObject.EntityView.ActualWidth;

            return result;
        }

        /// <summary>
        ///   The mouse moving.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void RectMouseMove( object sender, MouseEventArgs e )
        {
            if ( _currentProcessedPart != ControlParts.None ){
                Resize( (Rectangle) sender, e );
            }
        }

        /// <summary>
        ///   Sets the side part for resizing.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "controlPart">The part.</param>
        /// <param name = "eventArgs"></param>
        private void SetCurrentResizePart( object sender, ControlParts controlPart, MouseButtonEventArgs eventArgs )
        {
            var element = (FrameworkElement) sender;
            element.CaptureMouse();
            _currentProcessedPart = controlPart;
            _initMousePosition = eventArgs.GetPosition( (UIElement) element.Parent );
            _initRect = GetCurrentRect();
            SetCursor( (Rectangle) sender, controlPart );
        }

        /// <summary>
        ///   The onitional offset.
        /// </summary>
        private Point _initMousePosition;

        /// <summary>
        ///   The initional rect.
        /// </summary>
        private Rect _initRect;

        /// <summary>
        ///   The bottom side.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void BottomRectMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            e.Handled = true;
            SetCurrentResizePart( sender, ControlParts.Bottom, e );
        }

        /// <summary>
        ///   The top side.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void TopRectMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            e.Handled = true;
            SetCurrentResizePart( sender, ControlParts.Top, e );
        }

        /// <summary>
        ///   The right side.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void RightRectMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            e.Handled = true;
            SetCurrentResizePart( sender, ControlParts.Right, e );
        }

        /// <summary>
        ///   The left side.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void LeftRectMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            e.Handled = true;
            SetCurrentResizePart( sender, ControlParts.Left, e );
        }

        /// <summary>
        ///   The end of resizing.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void RectMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            e.Handled = true;
            ( (FrameworkElement) sender ).ReleaseMouseCapture();
            _currentProcessedPart = ControlParts.None;
            SetCursor( (Rectangle) sender, ControlParts.None );
            
            ProcessSizeChanged();
        }

        /// <summary>
        /// Processes datat when size has been changed.
        /// </summary>
        private void ProcessSizeChanged()
        {
            var rect = GetCurrentRect();
            AssociatedObject.ViewState.Height = rect.Height;
            AssociatedObject.ViewState.Width = rect.Width;
            RiseSizeChanged();
        }

        #region Resize 

        /// <summary>
        ///   The resize parts.
        /// </summary>
        private enum ControlParts
        {
            None,

            Left,

            Right,

            Top,

            Bottom,

            LeftTop,

            LeftBottom,

            RightTop,

            RightBottom,
        }

        /// <summary>
        ///   Sets the cursor to rectangle
        /// </summary>
        /// <param name = "rect">The rectangle to set a cursor.</param>
        /// <param name = "part">The side.</param>
        private void SetCursor( Rectangle rect, ControlParts part )
        {
            switch ( part ){
                case ControlParts.None:
                    rect.Cursor = Cursors.Arrow;
                    break;

                case ControlParts.Top:
                case ControlParts.Bottom:
                    rect.Cursor = Cursors.SizeNS;
                    break;

                case ControlParts.LeftBottom:
                case ControlParts.LeftTop:
                case ControlParts.RightBottom:
                case ControlParts.RightTop:
                case ControlParts.Right:
                case ControlParts.Left:
                    rect.Cursor = Cursors.SizeWE;
                    break;
            }
        }


        /// <summary>
        ///   The selected part
        /// </summary>
        private ControlParts _currentProcessedPart = ControlParts.None;

        #endregion Resize

        /// <summary>
        ///   Occurs when entity form Canvas.ZIndex property changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntityFormZIndexValueChanged( object sender, EventArgs e )
        {
            ProcessZIndex();
        }


        private PropertyChangeNotifier _canvasZIndexChangeNotifier;

        /// <summary>
        ///   Presses the Z index of rectangles about assotiated entity form.
        /// </summary>
        private void ProcessZIndex()
        {
            var index = Canvas.GetZIndex( AssociatedObject );
            index++;
            Canvas.SetZIndex( _rightRect, index );
            Canvas.SetZIndex( _leftRect, index );
            Canvas.SetZIndex( _topRect, index );
            Canvas.SetZIndex( _bottomRect, index );
        }

        /// <summary>
        ///   Calculates position for all rectangles.
        /// </summary>
        /// <param name = "byLayoutRoot">The flag which indicates that we use actual height and actual width property of entity view.</param>
        /// <param name = "position">Th eposition fo control.</param>
        private void UpdateRectanglesPosition( bool byLayoutRoot, Point position )
        {
            double width;
            double height;

            if ( byLayoutRoot ){
                width = AssociatedObject.ActualWidth;
                height = AssociatedObject.ActualHeight;
            }
            else{
                width = AssociatedObject.EntityView.Width;
                height = AssociatedObject.EntityView.Height;
            }

            SetRectCenterPosition( _leftRect, position.X, position.Y + height/2 );
            SetRectCenterPosition( _topRect, position.X + width/2, position.Y );
            SetRectCenterPosition( _rightRect, position.X + width,
                                   position.Y + height/2 );
            SetRectCenterPosition( _bottomRect, position.X + width/2,
                                   position.Y + height );
        }

        /// <summary>
        ///   Sets a rect center position.
        /// </summary>
        /// <param name = "element">The rectangle.</param>
        /// <param name = "x">The left of center rect.</param>
        /// <param name = "y">The top of center rect.</param>
        private void SetRectCenterPosition( Rectangle element, double x, double y )
        {
            if ( element.ActualHeight > 0.0 && element.ActualWidth > 0.0 ){
                Canvas.SetLeft( element, x - ( element.ActualWidth/2 ) );
                Canvas.SetTop( element, y - ( element.ActualHeight/2 ) );
            }
        }


        /// <summary>
        ///   Performs a operation of position cahnged events.
        /// </summary>
        private void ProcessEntityFormPositionChanged( Point position )
        {
            if ( SelectionIsVisible ){
                UpdateRectanglesPosition( true, position );
            }
        }

        /// <summary>
        ///   Sets binding to SelectionIsVisible property.
        /// </summary>
        /// <param name = "element">The element.</param>
        public void BindToVisibility( Rectangle element )
        {
            var visibilityBinding = new Binding{
                                                   Converter = new BoolToVisibilityConverter(),
                                                   Mode = BindingMode.OneWay,
                                                   Path = new PropertyPath( SelectionIsVisiblePropertyName ),
                                                   Source = this
                                               };
            element.SetBinding( UIElement.VisibilityProperty, visibilityBinding );
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            if ( _parrentCanvas != null ){
                DetachRectangels();

                _canvasZIndexChangeNotifier.ValueChanged -= EntityFormZIndexValueChanged;
                _canvasZIndexChangeNotifier.Dispose();
            }
            
            AssociatedObject.Loaded -= EntityFromLoaded;
            AssociatedObject.SizeChanged -= EntityFormSizeChanged;
            AssociatedObject.PositionChanging += EntityFormPositionChanging;
        }

        /// <summary>
        ///   The property name of selection is visible.
        /// </summary>
        private const string SelectionIsVisiblePropertyName = "SelectionIsVisible";

        public static readonly DependencyProperty SelectionIsVisibleProperty =
            DependencyProperty.Register( SelectionIsVisiblePropertyName, typeof ( bool ),
                                         typeof ( SelectAndResizeEntityFormBehavior ),
                                         new PropertyMetadata( default( bool ), OnSelectionIsVisibleChanged ) );

        /// <summary>
        /// Occurs when selection is visible changed.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectionIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = (bool) e.NewValue;

            var obj = (SelectAndResizeEntityFormBehavior) d;
            obj.SelectionIsVisible = newValue;
            if ( newValue ){
                obj.AttachRectangles();
                var position = new Point( Canvas.GetLeft( obj.AssociatedObject ),
                                            Canvas.GetTop( obj.AssociatedObject ) );
                obj.UpdateRectanglesPosition( true, position );
            }
            else{
                obj.DetachRectangels();
            } //else

            obj.RiseSelectionStateChanged( newValue );
        }

        /// <summary>
        ///   Indicates what selection of form is visible.
        /// </summary>
        public bool SelectionIsVisible
        {
            get { return (bool) GetValue( SelectionIsVisibleProperty ); }
            set { SetValue( SelectionIsVisibleProperty, value ); }
        }

        /// <summary>
        ///   Occurs when selection state has been changed.
        /// </summary>
        public event EventHandler<SelectionStateChangedEventArgs> SelectionStateChanged;

        /// <summary>
        ///   The invocator of SelectionStateChanged event.
        /// </summary>
        /// <param name = "isSelected">The selected indicator.</param>
        private void RiseSelectionStateChanged( bool isSelected )
        {
            EventHandler<SelectionStateChangedEventArgs> handler = SelectionStateChanged;

            if ( handler != null ){
                handler( AssociatedObject, new SelectionStateChangedEventArgs( isSelected ) );
            }
        }

        /// <summary>
        /// Resizes the entity form without EntitySizeChanged event rise.
        /// </summary>
        /// <param name="rect">The new size rect.</param>
        public void ForceResize(Rect rect)
        {
            Canvas.SetLeft(AssociatedObject, rect.X);
            Canvas.SetTop(AssociatedObject, rect.Y);
            AssociatedObject.ViewState.Width = AssociatedObject.EntityView.Width = rect.Width;
            AssociatedObject.ViewState.Height = AssociatedObject.EntityView.Height = rect.Height;

            bool byLayoutRoot = double.IsNaN( rect.Height ) || double.IsNaN( rect.Height );

            UpdateRectanglesPosition( byLayoutRoot, new Point( rect.X, rect.Y ) );
        }

        #region Size Changed event 
        
        /// <summary>
        /// Occurs when size of the entity form has been changed.
        /// </summary>
        public event EventHandler<EntitySizeChangedEventArgs> SizeChanged;

        /// <summary>
        /// The invocator of SizeChanged event.
        /// </summary>
        private void RiseSizeChanged()
        {
            EventHandler<EntitySizeChangedEventArgs> handler = SizeChanged;
            
            if ( handler != null ){
                var eventArgs = new EntitySizeChangedEventArgs( _initRect, GetCurrentRect() );
                handler( this, eventArgs );
            }
        }

        #endregion Size Changed event
    }
}