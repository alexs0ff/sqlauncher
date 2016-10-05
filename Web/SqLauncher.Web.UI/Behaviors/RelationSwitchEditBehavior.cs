// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RelationSwitchEditBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 18 10:46 PM
//   * Modified at: 2011  10 20  9:28 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

using SqLauncher.Web.UI.Common;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   The swivel bahavior for relation edit show.
    /// </summary>
    public class RelationSwitchEditBehavior : Behavior<RelationForm>
    {
        #region Front element

        public static readonly DependencyProperty FrontElementNameProperty =
            DependencyProperty.Register( "FrontElementName", typeof ( string ),
                                         typeof ( SwivelFormBehavior ), new PropertyMetadata( null ) );

        [Category( "Swivel Properties" )]
        public string FrontElementName { get; set; }

        #endregion
        
        #region Duration

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register( "Duration", typeof ( Duration ),
                                         typeof ( SwivelFormBehavior ), new PropertyMetadata( null ) );

        [Category( "Animation Properties" )]
        public Duration Duration { get; set; }

        #endregion

        private readonly Storyboard _frontToBackStoryboard = new Storyboard();

        private readonly Storyboard _backToFrontStoryboard = new Storyboard();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Interactivity.Behavior`1"/> class.
        /// </summary>
        public RelationSwitchEditBehavior()
        {
            _backToFrontStoryboard.Completed += BackToFrontStoryboardCompleted;
        }

        /// <summary>
        /// Occurs when back to front story board has been completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void BackToFrontStoryboardCompleted(object sender, EventArgs e)
        {
            RelationEdit.RiseAppearanceChanged( true );
        }

        /// <summary>
        /// Gets the relation edit form.
        /// </summary>
        private RelationFormEdit RelationEdit
        {
            get { return AssociatedObject.RelationEdit; }
        }

        /// <summary>
        ///   The front element.
        /// </summary>
        private FrameworkElement _frontView;

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObjectLoaded;
        }

        /// <summary>
        ///   Occurs when assosiated object has been loaded.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void AssociatedObjectLoaded( object sender, RoutedEventArgs e )
        {
            FrameworkElement parent = AssociatedObject; // as FrameworkElement;

            _frontView = parent.FindName( FrontElementName ) as FrameworkElement;

            if ( _frontView == null || RelationEdit == null ){
                return;
            }

            var canvas = ControlHelper.FindParent<Canvas>( _frontView );
            canvas.Children.Add( RelationEdit );
            RelationEdit.Visibility = Visibility.Collapsed;
            RelationEdit.DataContext = AssociatedObject.DataEntity;
            RelationEdit.NeedToClose += delegate { Start(); };

            Canvas.SetZIndex( RelationEdit, 50 );

            var transform = new ScaleTransform();
            transform.ScaleX = 1;
            transform.ScaleY = 1;
            transform.CenterX = 0.5;
            transform.CenterY = 0.5;

            RelationEdit.RenderTransform = transform;
            RelationEdit.RenderTransformOrigin = new Point( 0.5, 0.5 );

            //Scale
            _frontToBackStoryboard.Children.Add( CreateDoubleAnimation( RelationEdit,
                                                                        ScaleXProperty, 0.1, 1.0 ) );
            _frontToBackStoryboard.Children.Add( CreateDoubleAnimation( RelationEdit,
                                                                        ScaleYProperty, 0.1, 1.0 ) );

            _backToFrontStoryboard.Children.Add( CreateDoubleAnimation( RelationEdit,
                                                                        ScaleXProperty, 1.0, 0.1 ) );
            _backToFrontStoryboard.Children.Add( CreateDoubleAnimation( RelationEdit,
                                                                        ScaleYProperty, 1.0, 0.1 ) );

            // Visibility
            _frontToBackStoryboard.Children.Add( CreateVisibilityAnimation( Duration, RelationEdit,
                                                                            true ) );

            _backToFrontStoryboard.Children.Add( CreateVisibilityAnimation( Duration, RelationEdit,
                                                                            false ) );

            AssociatedObject.Loaded -= AssociatedObjectLoaded;

            _frontView.MouseLeftButtonDown += FrontViewMouseLeftButtonDown;
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            if ( _frontView != null ){
                _frontView.MouseLeftButtonDown -= FrontViewMouseLeftButtonDown;

                if (RelationEdit != null){
                    var canvas = ControlHelper.FindParent<Canvas>( _frontView );
                    canvas.Children.Remove( RelationEdit );
                } //if

            } //if
           
        }

        /// <summary>
        ///   Occurs when user click on edit form.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void FrontViewMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if ( e.ClickCount == 2 ){
                var position = e.GetPosition( AssociatedObject );
                StartEdit( position );
            } //if
        }

        /// <summary>
        /// Startes edit the relation form.
        /// Showes the relation form edit. 
        /// </summary>
        /// <param name="position">The position to edit form appear.</param>
        public void StartEdit(Point position)
        {
            double width = RelationEdit.Width;

            if ( double.IsNaN( width ) ){
                width = RelationEdit.ActualWidth;
            } //if

            Canvas.SetLeft(RelationEdit, position.X - (width / 2));
            Canvas.SetTop( RelationEdit, position.Y );

            Start();
        }

        /// <summary>
        ///   The flag that indicates what we switch to edit.
        /// </summary>
        private bool _forward = true;

        /// <summary>
        ///   The scale x property to resize.
        /// </summary>
        private const string ScaleXProperty = "(RenderTransform).(ScaleTransform.ScaleX)";

        /// <summary>
        ///   The scale y property to resize.
        /// </summary>
        private const string ScaleYProperty = "(RenderTransform).(ScaleTransform.ScaleY)";

        /// <summary>
        ///   Starts the switch to edit or back.
        /// </summary>
        private void Start()
        {
            if ( _forward ){
                _frontToBackStoryboard.Begin();
            } //if
            else{
                AssociatedObject.RelationEdit.RiseAppearanceChanged( false );
                _backToFrontStoryboard.Begin();
            } //else

            _forward = !_forward;
        }

        /// <summary>
        ///   Initialize the double animation.
        /// </summary>
        /// <param name = "targetObject">The target object which will apply the animation.</param>
        /// <param name = "propertyPath">The target property.</param>
        /// <param name = "fromValue">The begining value.</param>
        /// <param name = "toValue">The target value.</param>
        /// <returns>Created animation.</returns>
        private DoubleAnimation CreateDoubleAnimation( FrameworkElement targetObject, string propertyPath,
                                                       double fromValue, double toValue )
        {
            var animation = new DoubleAnimation();
            animation.BeginTime = new TimeSpan( 0 );
            animation.Duration = Duration;
            animation.From = fromValue;
            animation.To = toValue;
            Storyboard.SetTargetProperty( animation, new PropertyPath( propertyPath ) );
            Storyboard.SetTarget( animation, targetObject );

            return animation;
        }

        private static ObjectAnimationUsingKeyFrames CreateVisibilityAnimation( Duration duration,
                                                                                DependencyObject element, bool show )
        {
            var animation = new ObjectAnimationUsingKeyFrames();
            animation.BeginTime = new TimeSpan( 0 );
            if ( show ){
                animation.KeyFrames.Add( new DiscreteObjectKeyFrame{
                                                                       KeyTime = new TimeSpan( 0 ),
                                                                       Value =
                                                                           Visibility.Visible
                                                                   } );
            } //if
            else{
                animation.KeyFrames.Add( new DiscreteObjectKeyFrame{
                                                                       KeyTime = duration.TimeSpan,
                                                                       Value =
                                                                           Visibility.Collapsed
                                                                   } );
            } //else

            Storyboard.SetTargetProperty( animation, new PropertyPath( "Visibility" ) );
            Storyboard.SetTarget( animation, element );
            return animation;
        }
    }
}