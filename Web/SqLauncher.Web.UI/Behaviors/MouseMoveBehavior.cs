// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   MouseMoveBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 24 8:12 PM
//   * Modified at: 2011  09 05  6:31 PM
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

using SqLauncher.Web.UI.Common;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   Represents a mouse move behavior of any UIElement whith Canvas parent.
    /// </summary>
    public class MouseMoveBehavior : Behavior<UIElement>
    {
        /// <summary>
        ///   The flag which indicates that this control need to move.
        /// </summary>
        private bool _isElementMoving;

        /// <summary>
        ///   The offset of this control.
        /// </summary>
        private Point _offsetOfEntityForm;

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += AssociatedObjectMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObjectMouseLeftButtonUp;
            AssociatedObject.MouseMove += AssociatedObjectMouseMove;
        }

        private void AssociatedObjectMouseMove( object sender, MouseEventArgs e )
        {
            if ( _isElementMoving ){
                var currentPosition = e.GetPosition( ControlHelper.FindParent<Canvas>( AssociatedObject ) );
                currentPosition = new Point( currentPosition.X - _offsetOfEntityForm.X,
                                             currentPosition.Y - _offsetOfEntityForm.Y
                    );

                Canvas.SetLeft( AssociatedObject, currentPosition.X );
                Canvas.SetTop( AssociatedObject, currentPosition.Y );
            }
        }

        private void AssociatedObjectMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            if (_isElementMoving){
                NewPosition = new Point( Canvas.GetLeft( AssociatedObject ), Canvas.GetTop( AssociatedObject ) );
                _isElementMoving = false;
                AssociatedObject.ReleaseMouseCapture();

                if ( NewPosition != OldPosition ){
                    OnElementMoved();
                }
            }
        }

        private void AssociatedObjectMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            _offsetOfEntityForm = e.GetPosition( AssociatedObject );
            StartObjectMoving( false );
        }

        /// <summary>
        ///   Starts e object moving action
        /// </summary>
        /// <param name = "setOffset">The falg wich indicates that method should calculate offset position.</param>
        private void StartObjectMoving( bool setOffset )
        {
            if ( setOffset ){
                var element = (FrameworkElement) AssociatedObject;
                _offsetOfEntityForm = new Point( element.ActualWidth/2, element.ActualHeight/2 );
            }

            OldPosition = new Point( Canvas.GetLeft( AssociatedObject ), Canvas.GetTop( AssociatedObject ) );

            _isElementMoving = true;
            AssociatedObject.CaptureMouse();
        }

        /// <summary>
        ///   The old position.
        /// </summary>
        public Point OldPosition { get; private set; }

        /// <summary>
        ///   The new position.
        /// </summary>
        public Point NewPosition { get; private set; }

        /// <summary>
        ///   Occurs when element has been moved.
        /// </summary>
        public event EventHandler<EventArgs> ElementMoved;

        /// <summary>
        ///   The invocator of ElementMoved event.
        /// </summary>
        public void OnElementMoved()
        {
            EventHandler<EventArgs> handler = ElementMoved;
            if ( handler != null ){
                handler( this, new EventArgs() );
            }
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= AssociatedObjectMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObjectMouseLeftButtonUp;
            AssociatedObject.MouseMove -= AssociatedObjectMouseMove;
        }
    }
}