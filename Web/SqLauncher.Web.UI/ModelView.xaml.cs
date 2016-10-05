// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelView.xaml.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  02 15  23:17
// / ******************************************************************************/ 

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    public partial class ModelView : IModelView
    {
        //private const int NestedCanvasOffset = 2;
        /// <summary>
        ///   The offset for nested canvas.
        /// </summary>
        /// <summary>
        ///   The layout z axis manager.
        /// </summary>
        private readonly LayoutZManager _layoutZManager = new LayoutZManager();

        public ModelView()
        {
            InitializeComponent();
            layoutBorder.Width = ModelViewState.DefaultWidth;
            LayoutRoot.Width = ModelViewState.DefaultWidth;
            layoutBorder.Height = ModelViewState.DefaultHeight;
            LayoutRoot.Height = ModelViewState.DefaultHeight;
            LayoutRoot.MouseMove += CanvasMouseMove;
            LayoutRoot.MouseLeftButtonDown +=
                ( sender, args ) => RiseMouseButtonDown( MouseButton.Left, args );

            LayoutRoot.MouseLeftButtonUp += ( sender, args ) => RiseMouseButtonUp( MouseButton.Left, args );
        }

        #region Model Mouse events 

        /// <summary>
        ///   Occurs when mouse button up.
        /// </summary>
        public event EventHandler<MouseButtonUpEventArgs> MouseButtonUp;

        /// <summary>
        ///   The invocator for MouseButtonUp event.
        /// </summary>
        /// <param name = "button">The button.</param>
        /// <param name = "args">The mouse event args.</param>
        private void RiseMouseButtonUp( MouseButton button, MouseButtonEventArgs args )
        {
            EventHandler<MouseButtonUpEventArgs> handler = MouseButtonUp;

            if ( handler != null ){
                handler( this, new MouseButtonUpEventArgs( button, args.ClickCount, args.GetPosition( LayoutRoot ) ) );
            }
        }

        /// <summary>
        ///   The invocator for MouseButtonDown event.
        /// </summary>
        /// <param name = "mouseButton">The button.</param>
        /// <param name = "args">The mouse event args.</param>
        private void RiseMouseButtonDown( MouseButton mouseButton, MouseButtonEventArgs args )
        {
            var handler = MouseButtonDown;

            if ( handler != null ){
                handler( this,
                         new MouseButtonDownEventArgs( mouseButton, args.ClickCount, args.GetPosition( LayoutRoot ) ) );
            }
        }

        /// <summary>
        ///   Occurs when mouse button down.
        /// </summary>
        public event EventHandler<MouseButtonDownEventArgs> MouseButtonDown;

        /// <summary>
        ///   Occurs when mouse move on canvas.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void CanvasMouseMove( object sender, MouseEventArgs e )
        {
            Point position = e.GetPosition( LayoutRoot );

            RiseModelMouseMove( position );
        }

        /// <summary>
        ///   Occurs when mouse move on model view canvas.
        /// </summary>
        public event EventHandler<ScaledMouseMoveEventArgs> ModelMouseMove;

        /// <summary>
        ///   The invocator of ModelMouseMove event.
        /// </summary>
        /// <param name = "position">The mouse position.</param>
        private void RiseModelMouseMove( Point position )
        {
            EventHandler<ScaledMouseMoveEventArgs> handler = ModelMouseMove;
            if ( handler != null ){
                handler( this,
                         new ScaledMouseMoveEventArgs( scaleTransform.ScaleX,
                                                       scaleTransform.ScaleY, position ) );
            }
        }

        #endregion Model Mouse events

        #region Selection 

        /// <summary>
        ///   The maximum of Z indexes for selection.
        /// </summary>
        private const int SelectionMaxZIndex = 100;

        /// <summary>
        ///   Shows the selection.
        /// </summary>
        public void ShowSelection()
        {
            selectionRectangle.Visibility = Visibility.Visible;
            Canvas.SetZIndex( selectionRectangle, SelectionMaxZIndex );
        }

        /// <summary>
        ///   The current width of the model work space.
        /// </summary>
        public double CurrentWidth
        {
            get { return LayoutRoot.Width; }
        }

        /// <summary>
        ///   The current нeight of the model work space.
        /// </summary>
        public double CurrentHeight
        {
            get { return LayoutRoot.Height; }
        }

        /// <summary>
        ///   Sets the selection by the begin and the end points.
        /// </summary>
        /// <param name = "start">The satrt point.</param>
        /// <param name = "end">The end point.</param>
        public void SetSelection( Point start, Point end )
        {
            var x = start.X < end.X ? start.X : end.X;
            var y = start.Y < end.Y ? start.Y : end.Y;

            var width = Math.Abs( start.X - end.X );

            var height = Math.Abs( start.Y - end.Y );

            Canvas.SetLeft( selectionRectangle, x );
            Canvas.SetTop( selectionRectangle, y );

            selectionRectangle.Height = height;
            selectionRectangle.Width = width;
        }

        /// <summary>
        ///   Hides the selection on model view.
        /// </summary>
        public void HideSelection()
        {
            selectionRectangle.Visibility = Visibility.Collapsed;
            selectionRectangle.Width = 0;
            selectionRectangle.Height = 0;
            selectionRectangle.ClearValue( Canvas.ZIndexProperty );
        }

        #endregion Selection

        /// <summary>
        ///   Returns a model canvas element.
        /// </summary>
        /// <returns>The canvas instance.</returns>
        public Canvas GetMainGanvas()
        {
            return LayoutRoot;
        }

        /// <summary>
        ///   Appends to canvas the entity form.
        /// </summary>
        /// <param name = "form">The entity form.</param>
        public void AddChild( IForm form )
        {
            LayoutRoot.Children.Add( (UserControl) form );
            form.Initialize();

            if ( form is EntityForm ){
                _layoutZManager.Register( (EntityForm) form );
            } //if
            else if ( form is RelationForm ){
                _layoutZManager.Register( (RelationForm) form );
            } //if
        }

        /// <summary>
        ///   Removes from canvas the entity form.
        /// </summary>
        /// <param name = "form">The form to remove.</param>
        public void RemoveChild( IForm form )
        {
            if ( form is EntityForm ){
                _layoutZManager.Unregister( (EntityForm) form );
            } //if
            else if ( form is RelationForm ){
                _layoutZManager.Unregister( (RelationForm) form );
            } //if

            LayoutRoot.Children.Remove( (UserControl) form );
            form.Uninitialize();
        }

        /// <summary>
        ///   The zoom of data model.
        /// </summary>
        public double Zoom
        {
            get { return scaleTransform.ScaleX*100; }
            set
            {
                scaleTransform.ScaleX = value/100;
                scaleTransform.ScaleY = value/100;
                layoutBorder.Width = DataEntity.Width*scaleTransform.ScaleX;
                layoutBorder.Height = DataEntity.Height*scaleTransform.ScaleY;
            }
        }

        /// <summary>
        ///   The data entity
        /// </summary>
        public IModelViewState DataEntity
        {
            get { return DataContext as IModelViewState; }
            set
            {
                if ( DataContext != null && DataContext is ModelViewState ){
                    var state = (ModelViewState) DataContext;
                    state.PropertyChanged -= ModelViewStatePropertyChanged;
                } //if

                DataContext = value;
                ( (ModelViewState) DataContext ).PropertyChanged += ModelViewStatePropertyChanged;
                UpdateLayoutWidth();
                UpdateLayoutHeight();
            }
        }

        /// <summary>
        ///   Occurs when property changed on data context.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void ModelViewStatePropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            switch ( e.PropertyName ){
                case "Width":
                    UpdateLayoutWidth();

                    break;
                case "Height":
                    UpdateLayoutHeight();
                    break;
            } //switch
        }

        /// <summary>
        /// Updates the layout Height property.
        /// </summary>
        private void UpdateLayoutHeight()
        {
            LayoutRoot.Height = DataEntity.Height;
            layoutBorder.Height = DataEntity.Height*scaleTransform.ScaleY;
        }

        /// <summary>
        /// Updates the layout Width property.
        /// </summary>
        private void UpdateLayoutWidth()
        {
            LayoutRoot.Width = DataEntity.Width;
            layoutBorder.Width = DataEntity.Width*scaleTransform.ScaleX;
        }

        /// <summary>
        ///   Captures the mouse events.
        /// </summary>
        public void MouseCapture()
        {
            LayoutRoot.CaptureMouse();
        }

        /// <summary>
        ///   Releases the mouse.
        /// </summary>
        public void MouseRelease()
        {
            LayoutRoot.ReleaseMouseCapture();
        }
    }
}