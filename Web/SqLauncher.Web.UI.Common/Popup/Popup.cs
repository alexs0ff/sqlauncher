﻿// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   Popup.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description: this popup behaviors based on http://kentb.blogspot.com/2010/07/popupstaysopen-in-silverlight.html article
//   * Thanks Kent!
//   * Created at 2011 10 03 9:18 PM
//   * Modified at: 2011  10 03  9:20 PM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SqLauncher.Web.UI.Common.Popup
{
    public static class Popup
    {
        private static readonly IList<PopupPlacement> _popupPlacements =
            new List<PopupPlacement>( new[]{
                                               PopupPlacement.Top, PopupPlacement.Bottom, PopupPlacement.Left,
                                               PopupPlacement.Right
                                           } );

        private static readonly IList<PopupHorizontalAlignment> _horizontalAlignments =
            new List<PopupHorizontalAlignment>( new[]{
                                                         PopupHorizontalAlignment.Center, PopupHorizontalAlignment.LeftCenter,
                                                         PopupHorizontalAlignment.RightCenter, PopupHorizontalAlignment.Left,
                                                         PopupHorizontalAlignment.Right
                                                     } );

        private static readonly IList<PopupVerticalAlignment> _verticalAlignments =
            new List<PopupVerticalAlignment>( new[]{
                                                       PopupVerticalAlignment.Center, PopupVerticalAlignment.TopCenter,
                                                       PopupVerticalAlignment.BottomCenter, PopupVerticalAlignment.Top,
                                                       PopupVerticalAlignment.Bottom
                                                   } );

        private static readonly IDictionary<System.Windows.Controls.Primitives.Popup, PopupWatcher> popupWatcherCache =
            new Dictionary<System.Windows.Controls.Primitives.Popup, PopupWatcher>();

        public static readonly DependencyProperty PlacementTargetProperty =
            DependencyProperty.RegisterAttached( "PlacementTarget",
                                                 typeof ( FrameworkElement ),
                                                 typeof ( Popup ),
                                                 new PropertyMetadata( OnPlacementTargetChanged ) );

        public static readonly DependencyProperty PlacementParentProperty =
            DependencyProperty.RegisterAttached( "PlacementParent",
                                                 typeof ( FrameworkElement ),
                                                 typeof ( Popup ),
                                                 new PropertyMetadata( null ) );

        public static readonly DependencyProperty PlacementChildProperty =
            DependencyProperty.RegisterAttached( "PlacementChild",
                                                 typeof ( FrameworkElement ),
                                                 typeof ( Popup ),
                                                 new PropertyMetadata( null ) );

        public static readonly DependencyProperty PreferredOrientationsProperty =
            DependencyProperty.RegisterAttached( "PreferredOrientations",
                                                 typeof ( PopupOrientationCollection ),
                                                 typeof ( Popup ),
                                                 new PropertyMetadata( null ) );

        public static readonly DependencyProperty ActualOrientationProperty =
            DependencyProperty.RegisterAttached( "ActualOrientation",
                                                 typeof ( PopupOrientation ),
                                                 typeof ( Popup ),
                                                 new PropertyMetadata( null ) );

        public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.RegisterAttached( "StaysOpen",
                                                                                                           typeof ( bool
                                                                                                               ),
                                                                                                           typeof (
                                                                                                               Popup ),
                                                                                                           new PropertyMetadata
                                                                                                               ( true,
                                                                                                                 OnStaysOpenChanged ) );

        private static readonly DependencyProperty PopupProperty = DependencyProperty.RegisterAttached( "Popup",
                                                                                                        typeof (
                                                                                                            System.
                                                                                                            Windows.
                                                                                                            Controls.
                                                                                                            Primitives.
                                                                                                            Popup ),
                                                                                                        typeof ( Popup ),
                                                                                                        new PropertyMetadata
                                                                                                            ( null ) );

        public static FrameworkElement GetPlacementTarget( System.Windows.Controls.Primitives.Popup popup )
        {
            //popup.AssertNotNull("popup");
            return popup.GetValue( PlacementTargetProperty ) as FrameworkElement;
        }

        public static void SetPlacementTarget( System.Windows.Controls.Primitives.Popup popup,
                                               FrameworkElement placementTarget )
        {
            //popup.AssertNotNull("popup");
            popup.SetValue( PlacementTargetProperty, placementTarget );
        }

        public static FrameworkElement GetPlacementChild( System.Windows.Controls.Primitives.Popup popup )
        {
            //popup.AssertNotNull("popup");
            return popup.GetValue( PlacementChildProperty ) as FrameworkElement;
        }

        public static void SetPlacementChild( System.Windows.Controls.Primitives.Popup popup,
                                              FrameworkElement placementChild )
        {
            //popup.AssertNotNull("popup");
            popup.SetValue( PlacementChildProperty, placementChild );
        }

        public static FrameworkElement GetPlacementParent( System.Windows.Controls.Primitives.Popup popup )
        {
            //popup.AssertNotNull("popup");
            return popup.GetValue( PlacementParentProperty ) as FrameworkElement;
        }

        public static void SetPlacementParent( System.Windows.Controls.Primitives.Popup popup,
                                               FrameworkElement placementParent )
        {
            //popup.AssertNotNull("popup");
            popup.SetValue( PlacementParentProperty, placementParent );
        }

        public static PopupOrientationCollection GetPreferredOrientations(
            System.Windows.Controls.Primitives.Popup popup )
        {
            //popup.AssertNotNull("popup");
            return popup.GetValue( PreferredOrientationsProperty ) as PopupOrientationCollection;
        }

        public static void SetPreferredOrientations( System.Windows.Controls.Primitives.Popup popup,
                                                     PopupOrientationCollection orientations )
        {
            //popup.AssertNotNull("popup");
            popup.SetValue( PreferredOrientationsProperty, orientations );
        }

        public static PopupOrientation GetActualOrientation( System.Windows.Controls.Primitives.Popup popup )
        {
            //popup.AssertNotNull("popup");
            return popup.GetValue( ActualOrientationProperty ) as PopupOrientation;
        }

        public static void SetActualOrientation( System.Windows.Controls.Primitives.Popup popup,
                                                 PopupOrientation orientation )
        {
            //popup.AssertNotNull("popup");
            popup.SetValue( ActualOrientationProperty, orientation );
        }

        public static bool GetStaysOpen( System.Windows.Controls.Primitives.Popup popup )
        {
            //popup.AssertNotNull("popup");
            return (bool) popup.GetValue( StaysOpenProperty );
        }

        public static void SetStaysOpen( System.Windows.Controls.Primitives.Popup popup, bool staysOpen )
        {
            //popup.AssertNotNull("popup");
            popup.SetValue( StaysOpenProperty, staysOpen );
        }

        private static System.Windows.Controls.Primitives.Popup GetPopup( DependencyObject dependencyObject )
        {
            //dependencyObject.AssertNotNull("dependencyObject");
            return dependencyObject.GetValue( PopupProperty ) as System.Windows.Controls.Primitives.Popup;
        }

        private static void SetPopup( DependencyObject dependencyObject, System.Windows.Controls.Primitives.Popup popup )
        {
            //popup.AssertNotNull("popup");
            dependencyObject.SetValue( PopupProperty, popup );
        }

        private static void OnPlacementTargetChanged( DependencyObject dependencyObject,
                                                      DependencyPropertyChangedEventArgs e )
        {
            var popup = dependencyObject as System.Windows.Controls.Primitives.Popup;
            Debug.Assert( popup != null );

            InvalidatePosition( popup );
        }

        private static void OnInvalidateOrientation( DependencyObject dependencyObject,
                                                     DependencyPropertyChangedEventArgs e )
        {
            var popup = dependencyObject as System.Windows.Controls.Primitives.Popup;
            Debug.Assert( popup != null );

            InvalidatePosition( popup );
        }

        private static void OnStaysOpenChanged( DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e )
        {
            var popup = dependencyObject as System.Windows.Controls.Primitives.Popup;
            var popupWatcher = (PopupWatcher) null;
            var staysOpen = (bool) e.NewValue;

            popupWatcherCache.TryGetValue( popup, out popupWatcher );

            if ( staysOpen ){
                if ( popupWatcher != null ){
                    popupWatcher.Detach();
                    popupWatcherCache.Remove( popup );
                }
            }
            else{
                if ( popupWatcher == null ){
                    popupWatcher = new PopupWatcher( popup );
                    popupWatcherCache[popup] = popupWatcher;
                }

                popupWatcher.Attach();
            }
        }

        private static void PositionPopup( System.Windows.Controls.Primitives.Popup popup )
        {
            var parent = popup.Parent as FrameworkElement;
            var placementParent = GetPlacementParent( popup ) ?? popup.Parent as FrameworkElement;
            var placementChild = GetPlacementChild( popup ) ?? popup.Child as FrameworkElement;

            if ( parent == null || placementParent == null || placementChild == null ){
                return;
            }

            var preferredOrientations = GetPreferredOrientations( popup ) ?? Enumerable.Empty<PopupOrientation>();

            foreach ( var preferredOrientation in preferredOrientations ){
                if ( TryPlacePopup( popup, parent, placementParent, placementChild, preferredOrientation ) ){
                    return;
                }
            }

            var fallbackOrientations = from placement in _popupPlacements
                                       from horizontalAlignment in _horizontalAlignments
                                       from verticalAlignment in _verticalAlignments
                                       where
                                           preferredOrientations.FirstOrDefault(
                                               x =>
                                               x.Placement == placement && x.HorizontalAlignment == horizontalAlignment &&
                                               x.VerticalAlignment == verticalAlignment ) == null
                                       select
                                           new PopupOrientation{
                                                                   Placement = placement,
                                                                   HorizontalAlignment = horizontalAlignment,
                                                                   VerticalAlignment = verticalAlignment
                                                               };

            foreach ( var fallbackOrientation in fallbackOrientations ){
                if ( TryPlacePopup( popup, parent, placementParent, placementChild, fallbackOrientation ) ){
                    return;
                }
            }

            // give up and just use the first preferred orientation, if any
            var orientation = GetPreferredOrientations( popup ).FirstOrDefault();

            if ( orientation != null ){
                SetActualOrientation( popup, orientation );
            }
        }

        private static bool TryPlacePopup( System.Windows.Controls.Primitives.Popup popup, FrameworkElement parent,
                                           FrameworkElement placementParent, FrameworkElement placementChild,
                                           PopupOrientation orientation )
        {
            var availableWidth = placementParent.ActualWidth;
            var availableHeight = placementParent.ActualHeight;

            SetActualOrientation( popup, orientation );
            placementChild.Measure( new Size( availableWidth, availableHeight ) );

            var requiredWidth = placementChild.DesiredSize.Width;
            var requiredHeight = placementChild.DesiredSize.Height;
            var placementTarget = GetPlacementTarget( popup ) ?? parent;
            var parentTransform = placementTarget.TransformToVisual( parent );
            var popupLocation = parentTransform.Transform( new Point( 0, 0 ) );

            switch ( orientation.Placement ){
                case PopupPlacement.Top:
                    popupLocation.Y -= requiredHeight;
                    break;
                case PopupPlacement.Bottom:
                    popupLocation.Y += placementTarget.ActualHeight;
                    break;
                case PopupPlacement.Left:
                    popupLocation.X -= requiredWidth;
                    break;
                case PopupPlacement.Right:
                    popupLocation.X += placementTarget.ActualWidth;
                    break;
            }

            if ( orientation.Placement == PopupPlacement.Top || orientation.Placement == PopupPlacement.Bottom ){
                switch ( orientation.HorizontalAlignment ){
                    case PopupHorizontalAlignment.RightCenter:
                        popupLocation.X += ( ( placementTarget.ActualWidth/2 ) - requiredWidth );
                        break;
                    case PopupHorizontalAlignment.Center:
                        popupLocation.X += ( ( placementTarget.ActualWidth - requiredWidth )/2 );
                        break;
                    case PopupHorizontalAlignment.LeftCenter:
                        popupLocation.X += ( placementTarget.ActualWidth/2 );
                        break;
                    case PopupHorizontalAlignment.Right:
                        popupLocation.X += ( placementTarget.ActualWidth - requiredWidth );
                        break;
                }
            }

            if ( orientation.Placement == PopupPlacement.Left || orientation.Placement == PopupPlacement.Right ){
                switch ( orientation.VerticalAlignment ){
                    case PopupVerticalAlignment.BottomCenter:
                        popupLocation.Y += ( ( placementTarget.ActualHeight/2 ) - requiredHeight );
                        break;
                    case PopupVerticalAlignment.Center:
                        popupLocation.Y += ( ( placementTarget.ActualHeight - requiredHeight )/2 );
                        break;
                    case PopupVerticalAlignment.TopCenter:
                        popupLocation.Y += ( placementTarget.ActualHeight/2 );
                        break;
                    case PopupVerticalAlignment.Bottom:
                        popupLocation.Y += ( placementTarget.ActualHeight - requiredHeight );
                        break;
                }
            }

            var popupLocationRelativeToPlacementParent = popupLocation;

            if ( parent != placementParent ){
                var placementParentTransform = placementTarget.TransformToVisual( placementParent );
                popupLocationRelativeToPlacementParent = placementParentTransform.Transform( popupLocation );
            }

            if ( popupLocationRelativeToPlacementParent.X < 0
                 || popupLocationRelativeToPlacementParent.Y < 0
                 || ( popupLocationRelativeToPlacementParent.X + requiredWidth ) > availableWidth
                 || ( popupLocationRelativeToPlacementParent.Y + requiredHeight ) > availableHeight ){
                // not enough room
                return false;
            }

            popup.HorizontalOffset = popupLocation.X;
            popup.VerticalOffset = popupLocation.Y;

            return true;
        }

        private static IEnumerable<PopupPlacement> GetPlacementModesInOrderOfPreference(
            PopupPlacement preferredPlacementMode )
        {
            yield return preferredPlacementMode;

            switch ( preferredPlacementMode ){
                case PopupPlacement.Top:
                    yield return PopupPlacement.Bottom;
                    yield return PopupPlacement.Left;
                    yield return PopupPlacement.Right;
                    yield break;
                case PopupPlacement.Bottom:
                    yield return PopupPlacement.Top;
                    yield return PopupPlacement.Left;
                    yield return PopupPlacement.Right;
                    yield break;
                case PopupPlacement.Left:
                    yield return PopupPlacement.Right;
                    yield return PopupPlacement.Top;
                    yield return PopupPlacement.Bottom;
                    yield break;
                case PopupPlacement.Right:
                    yield return PopupPlacement.Left;
                    yield return PopupPlacement.Top;
                    yield return PopupPlacement.Bottom;
                    yield break;
            }
        }

        private static IEnumerable<PopupHorizontalAlignment> GetHorizontalAlignmentsInOrderOfPreference(
            PopupHorizontalAlignment preferredHorizontalAlignment )
        {
            var startIndex = _horizontalAlignments.IndexOf( preferredHorizontalAlignment );
            var index = startIndex;

            do{
                yield return _horizontalAlignments[index];

                ++index;

                if ( index == _horizontalAlignments.Count ){
                    index = 0;
                }
            } while ( index != startIndex );
        }

        private static IEnumerable<PopupVerticalAlignment> GetVerticalAlignmentsInOrderOfPreference(
            PopupVerticalAlignment preferredVerticalAlignment )
        {
            var startIndex = _verticalAlignments.IndexOf( preferredVerticalAlignment );
            var index = startIndex;

            do{
                yield return _verticalAlignments[index];

                ++index;

                if ( index == _verticalAlignments.Count ){
                    index = 0;
                }
            } while ( index != startIndex );
        }

        private static void InvalidatePosition( System.Windows.Controls.Primitives.Popup popup )
        {
            if ( popup.IsOpen ){
                PositionPopup( popup );
            }

            popup.Opened -= OnPopupOpened;
            popup.Opened += OnPopupOpened;

            var child = GetPlacementChild( popup ) ?? popup.Child as FrameworkElement;

            if ( child != null ){
                SetPopup( child, popup );
                child.SizeChanged -= OnChildSizeChanged;
                child.SizeChanged += OnChildSizeChanged;
            }
        }

        private static void OnChildSizeChanged( object sender, SizeChangedEventArgs e )
        {
            var child = sender as DependencyObject;
            InvalidatePosition( GetPopup( child ) );
        }

        private static void OnPopupOpened( object sender, EventArgs e )
        {
            var popup = sender as System.Windows.Controls.Primitives.Popup;
            Debug.Assert( popup != null );

            InvalidatePosition( popup );
        }

        private static FrameworkElement FindHighestAncestor( System.Windows.Controls.Primitives.Popup popup )
        {
            var ancestor = (FrameworkElement) popup;

            while ( true ){
                var parent = VisualTreeHelper.GetParent( ancestor ) as FrameworkElement;

                if ( parent == null ){
                    return ancestor;
                }

                ancestor = parent;
            }
        }

        private sealed class PopupWatcher
        {
            private readonly System.Windows.Controls.Primitives.Popup popup;

            public PopupWatcher( System.Windows.Controls.Primitives.Popup popup )
            {
                this.popup = popup;
            }

            public void Attach()
            {
                popup.Opened += OnPopupOpened;
                popup.Closed += OnPopupClosed;
            }

            public void Detach()
            {
                popup.Opened -= OnPopupOpened;
                popup.Closed -= OnPopupClosed;
            }

            private void OnPopupOpened( object sender, EventArgs e )
            {
                var popupAncestor = FindHighestAncestor( this.popup );

                if ( popupAncestor == null ){
                    return;
                }

                popupAncestor.AddHandler( UIElement.MouseLeftButtonDownEvent,
                                          (MouseButtonEventHandler) OnMouseLeftButtonDown, true );
            }

            private void OnPopupClosed( object sender, EventArgs e )
            {
                var popupAncestor = FindHighestAncestor( this.popup );

                if ( popupAncestor == null ){
                    return;
                }

                popupAncestor.RemoveHandler( UIElement.MouseLeftButtonDownEvent,
                                             (MouseButtonEventHandler) OnMouseLeftButtonDown );
            }

            private void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
            {
                // in lieu of DependencyObject.SetCurrentValue, this is the easiest way to enact a change on the value of the Popup's IsOpen
                // property without overwriting any binding that may exist on it
                var storyboard = new Storyboard{Duration = TimeSpan.Zero};
                var objectAnimation = new ObjectAnimationUsingKeyFrames{Duration = TimeSpan.Zero};
                objectAnimation.KeyFrames.Add( new DiscreteObjectKeyFrame
                                               {KeyTime = KeyTime.FromTimeSpan( TimeSpan.Zero ), Value = false} );
                Storyboard.SetTarget( objectAnimation, this.popup );
                Storyboard.SetTargetProperty( objectAnimation, new PropertyPath( "IsOpen" ) );
                storyboard.Children.Add( objectAnimation );
                storyboard.Begin();
            }
        }
    }
}