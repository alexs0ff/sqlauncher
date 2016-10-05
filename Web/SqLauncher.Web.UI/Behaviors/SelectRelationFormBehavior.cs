// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SelectRelationFormBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 01 15 19:32
//   * Modified at: 2012  01 16  19:33
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Shapes;

using SqLauncher.Web.UI.Common;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   Represensts bahavior for selection of the Relation Form.
    /// </summary>
    public class SelectRelationFormBehavior : Behavior<RelationForm>
    {
        private const string EdgeRectangleStyleName = "RelationFormSelectionRectangleStyle";

        /// <summary>
        ///   The first rectangle of selection.
        /// </summary>
        private readonly Rectangle _startRect = new Rectangle();

        /// <summary>
        ///   The second rectangle of selection.
        /// </summary>
        private readonly Rectangle _endRect = new Rectangle();

        /// <summary>
        ///   The middle of the relation form.
        /// </summary>
        private readonly Rectangle _middleRect = new Rectangle();

        /// <summary>
        ///   The main parent canvas.
        /// </summary>
        private Canvas _parrentCanvas;

        /// <summary>
        ///   The z index notifier.
        /// </summary>
        private PropertyChangeNotifier _canvasZIndexChangeNotifier;

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += RelationFromLoaded;
            _startRect.Style = (Style) AssociatedObject.Resources.MergedDictionaries[0][EdgeRectangleStyleName];
            _endRect.Style = (Style) AssociatedObject.Resources.MergedDictionaries[0][EdgeRectangleStyleName];
            _middleRect.Style = (Style)AssociatedObject.Resources.MergedDictionaries[0][EdgeRectangleStyleName];

            _middleRect.MouseLeftButtonDown += SelectionRectMouseLeftButtonDown;
            _startRect.MouseLeftButtonDown += SelectionRectMouseLeftButtonDown;
            _endRect.MouseLeftButtonDown += SelectionRectMouseLeftButtonDown;

            _canvasZIndexChangeNotifier = new PropertyChangeNotifier( AssociatedObject, "(Canvas.ZIndex)" );
            _canvasZIndexChangeNotifier.ValueChanged += EntityFormZIndexValueChanged;
        }

        /// <summary>
        /// Occurs when user clicks mouse.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void SelectionRectMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( e.ClickCount == 2 ){
                AssociatedObject.ShowEditForm();
            } //if
            
            e.Handled = true;
        }

        private void EntityFormZIndexValueChanged( object sender, EventArgs e )
        {
            ProcessZIndex();
        }

        /// <summary>
        ///   Occurs when relation form has been loaded.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void RelationFromLoaded( object sender, RoutedEventArgs e )
        {
            _parrentCanvas = ControlHelper.FindParent<Canvas>( AssociatedObject );
        }

        /// <summary>
        ///   Adds to canvas all rectangles.
        /// </summary>
        private void AttachRectangles()
        {
            if ( _parrentCanvas != null && !_parrentCanvas.Children.Contains( _startRect ) ){
                _parrentCanvas.Children.Add( _startRect );
                _parrentCanvas.Children.Add( _endRect );
                _parrentCanvas.Children.Add( _middleRect );
            } //if
        }

        /// <summary>
        ///   Removes all rectangles from parent canvas.
        /// </summary>
        private void DetachRectangles()
        {
            if ( _parrentCanvas == null ){
                return;
            } //if

            if ( _parrentCanvas.Children.Contains( _startRect ) ){
                _parrentCanvas.Children.Remove( _startRect );
                _parrentCanvas.Children.Remove( _endRect );
                _parrentCanvas.Children.Remove( _middleRect );
            } //if
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            _middleRect.MouseLeftButtonDown -= SelectionRectMouseLeftButtonDown;
            _startRect.MouseLeftButtonDown -= SelectionRectMouseLeftButtonDown;
            _endRect.MouseLeftButtonDown -= SelectionRectMouseLeftButtonDown;

            AssociatedObject.Loaded -= RelationFromLoaded;
            _canvasZIndexChangeNotifier.ValueChanged -= EntityFormZIndexValueChanged;
            _canvasZIndexChangeNotifier.Dispose();
            DetachRectangles();
        }

        private const string SelectionIsVisiblePropertyName = "SelectionIsVisible";

        public static readonly DependencyProperty SelectionIsVisibleProperty =
            DependencyProperty.Register( SelectionIsVisiblePropertyName, typeof ( bool ),
                                         typeof ( SelectRelationFormBehavior ),
                                         new PropertyMetadata( SelectionIsVisibleValueChanged ) );

        /// <summary>
        ///   Occurs when selection visibility changed.
        /// </summary>
        /// <param name = "d">The dependency object.</param>
        /// <param name = "e">The event args.</param>
        private static void SelectionIsVisibleValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var behavior = (SelectRelationFormBehavior) d;
            behavior.SelectionIsVisible = (bool) e.NewValue;

            if ( behavior.SelectionIsVisible ){
                behavior.AttachRectangles();
                behavior.UpdateRectanglesPosition();
            } //if
            else{
                behavior.DetachRectangles();
            } //else
            behavior.RiseSelectionStateChanged( behavior.SelectionIsVisible );
        }

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
        private void RiseSelectionStateChanged(bool isSelected)
        {
            EventHandler<SelectionStateChangedEventArgs> handler = SelectionStateChanged;

            if (handler != null)
            {
                handler(AssociatedObject, new SelectionStateChangedEventArgs(isSelected));
            }
        }

        /// <summary>
        ///   Recalculates the position of selection elements.
        /// </summary>
        private void UpdateRectanglesPosition()
        {
            var startConnectPoint = AssociatedObject.StartConnectPoint;
            var destinationConnectPoint = AssociatedObject.DestinationConnectPoint;
            var widthOffset = _startRect.Width/2;
            var heightOffset = _startRect.Height/2;

            Canvas.SetLeft( _startRect, startConnectPoint.X - widthOffset );
            Canvas.SetTop( _startRect, startConnectPoint.Y - heightOffset );

            Canvas.SetLeft( _endRect, destinationConnectPoint.X - widthOffset );
            Canvas.SetTop( _endRect, destinationConnectPoint.Y - heightOffset );

            Canvas.SetLeft( _middleRect, AssociatedObject.MiddlePointBetweenConnectPoints.X - widthOffset );
            Canvas.SetTop( _middleRect,
                           AssociatedObject.MiddlePointBetweenConnectPoints.Y - heightOffset );
        }

        /// <summary>
        ///   Presses the Z index of rectangles about assotiated entity form.
        /// </summary>
        private void ProcessZIndex()
        {
            var index = Canvas.GetZIndex( AssociatedObject );
            index++;
            Canvas.SetZIndex( _startRect, index );
            Canvas.SetZIndex( _endRect, index );
        }
    }
}