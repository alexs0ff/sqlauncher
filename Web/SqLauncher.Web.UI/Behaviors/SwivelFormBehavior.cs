// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SwivelFormBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 24 8:35 PM
//   * Modified at: 2011  08 26  9:19 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SqLauncher.Web.UI.Behaviors
{
    public class RotationData
    {
        public double FromDegrees { get; set; }

        public double MidDegrees { get; set; }

        public double ToDegrees { get; set; }

        public string RotationProperty { get; set; }

        public PlaneProjection PlaneProjection { get; set; }

        public Duration AnimationDuration { get; set; }
    }

    public enum RotationDirection
    {
        LeftToRight,

        RightToLeft,

        TopToBottom,

        BottomToTop
    }

    public class SwivelFormBehavior : Behavior<EntityForm>
    {
        #region Front element

        public static readonly DependencyProperty FrontElementNameProperty =
            DependencyProperty.Register( "FrontElementName", typeof ( string ),
                                         typeof ( SwivelFormBehavior ), new PropertyMetadata( null ) );

        [Category( "Swivel Properties" )]
        public string FrontElementName { get; set; }

        #endregion

        #region Back element

        #endregion

        #region Duration

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register( "Duration", typeof ( Duration ),
                                         typeof ( SwivelFormBehavior ), new PropertyMetadata( null ) );

        [Category( "Animation Properties" )]
        public Duration Duration { get; set; }

        #endregion

        #region Rotation Direction

        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register( "Rotation", typeof ( RotationDirection ),
                                         typeof ( SwivelFormBehavior ),
                                         new PropertyMetadata( RotationDirection.LeftToRight ) );

        [Category( "Animation Properties" )]
        public RotationDirection Rotation { get; set; }

        #endregion

        private readonly Storyboard frontToBackStoryboard = new Storyboard();

        private readonly Storyboard backToFrontStoryboard = new Storyboard();

        private bool forward = true;

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += AssociatedObjectMouseLeftButtonDown;
            frontToBackStoryboard.Completed += FrontToBackActionComplited;
            backToFrontStoryboard.Completed += BackToFrontActionComplited;
        }

        private void FrontToBackActionComplited(object sender, EventArgs args)
        {
            _isComplited = true;
            AssociatedObject.GetEditForm().RiseAppearanceChanged( true );
        }

        private void BackToFrontActionComplited( object sender, EventArgs args )
        {
            _isComplited = true;
            AssociatedObject.DataEntity.IsEditing = false;
            AssociatedObject.ProcessEntityFormPositionChanging();
            AssociatedObject.GetEditForm().DataEntity = null;
        }

        private void AssociatedObjectMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            base.OnAttached();

            if ( e.ClickCount == 2 && forward){
                Start();
            }
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= AssociatedObjectMouseLeftButtonDown;
            base.OnDetaching();
        }

        /// <summary>
        /// The flag which indication completed action.
        /// </summary>
        private bool _isComplited = true;

        /// <summary>
        /// Starts swivel action of this behavior.
        /// </summary>
        public void Start()
        {
            if ( AssociatedObject == null || !_isComplited){
                return;
            }

            FrameworkElement parent = AssociatedObject; // as FrameworkElement;

            FrameworkElement front = parent.FindName( FrontElementName ) as FrameworkElement;
            FrameworkElement back = AssociatedObject.GetEditForm();

            if ( front == null || back == null ){
                return;
            }

            if ( front.Projection == null || back.Projection == null ){
                front.Projection = new PlaneProjection();
                front.RenderTransformOrigin = new Point( .5, .5 );
                front.Visibility = Visibility.Visible;

                back.Projection = new PlaneProjection{CenterOfRotationY = .5, RotationY = 180.0};
                //, CenterOfRotationZ = this.CenterOfRotationZ };
                back.RenderTransformOrigin = new Point( .5, .5 );
                back.Visibility = Visibility.Collapsed;

                RotationData showBackRotation = null;
                RotationData hideFrontRotation = null;
                RotationData showFrontRotation = null;
                RotationData hideBackRotation = null;

                var frontPP = new PlaneProjection(); // { CenterOfRotationZ = this.CenterOfRotationZ };
                var backPP = new PlaneProjection(); // { CenterOfRotationZ = this.CenterOfRotationZ };

                switch ( Rotation ){
                    case RotationDirection.LeftToRight:
                        backPP.CenterOfRotationY = frontPP.CenterOfRotationY = 0.5;
                        showBackRotation = new RotationData{
                                                               FromDegrees = 180.0,
                                                               MidDegrees = 90.0,
                                                               ToDegrees = 0.0,
                                                               RotationProperty = "RotationY",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = this.Duration
                                                           };
                        hideFrontRotation = new RotationData{
                                                                FromDegrees = 0.0,
                                                                MidDegrees = -90.0,
                                                                ToDegrees = -180.0,
                                                                RotationProperty = "RotationY",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        showFrontRotation = new RotationData{
                                                                FromDegrees = -180.0,
                                                                MidDegrees = -90.0,
                                                                ToDegrees = 0.0,
                                                                RotationProperty = "RotationY",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        hideBackRotation = new RotationData{
                                                               FromDegrees = 0.0,
                                                               MidDegrees = 90.0,
                                                               ToDegrees = 180.0,
                                                               RotationProperty = "RotationY",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = this.Duration
                                                           };
                        break;
                    case RotationDirection.RightToLeft:
                        backPP.CenterOfRotationY = frontPP.CenterOfRotationY = 0.5;
                        showBackRotation = new RotationData{
                                                               FromDegrees = -180.0,
                                                               MidDegrees = -90.0,
                                                               ToDegrees = 0.0,
                                                               RotationProperty = "RotationY",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = this.Duration
                                                           };
                        hideFrontRotation = new RotationData{
                                                                FromDegrees = 0.0,
                                                                MidDegrees = 90.0,
                                                                ToDegrees = 180.0,
                                                                RotationProperty = "RotationY",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        showFrontRotation = new RotationData{
                                                                FromDegrees = 180.0,
                                                                MidDegrees = 90.0,
                                                                ToDegrees = 0.0,
                                                                RotationProperty = "RotationY",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        hideBackRotation = new RotationData{
                                                               FromDegrees = 0.0,
                                                               MidDegrees = -90.0,
                                                               ToDegrees = -180.0,
                                                               RotationProperty = "RotationY",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = this.Duration
                                                           };
                        break;
                    case RotationDirection.BottomToTop:
                        backPP.CenterOfRotationX = frontPP.CenterOfRotationX = 0.5;
                        showBackRotation = new RotationData{
                                                               FromDegrees = 180.0,
                                                               MidDegrees = 90.0,
                                                               ToDegrees = 0.0,
                                                               RotationProperty = "RotationX",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = this.Duration
                                                           };
                        hideFrontRotation = new RotationData{
                                                                FromDegrees = 0.0,
                                                                MidDegrees = -90.0,
                                                                ToDegrees = -180.0,
                                                                RotationProperty = "RotationX",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        showFrontRotation = new RotationData{
                                                                FromDegrees = -180.0,
                                                                MidDegrees = -90.0,
                                                                ToDegrees = 0.0,
                                                                RotationProperty = "RotationX",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        hideBackRotation = new RotationData{
                                                               FromDegrees = 0.0,
                                                               MidDegrees = 90.0,
                                                               ToDegrees = 180.0,
                                                               RotationProperty = "RotationX",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = this.Duration
                                                           };
                        break;
                    case RotationDirection.TopToBottom:
                        backPP.CenterOfRotationX = frontPP.CenterOfRotationX = 0.5;
                        showBackRotation = new RotationData{
                                                               FromDegrees = -180.0,
                                                               MidDegrees = -90.0,
                                                               ToDegrees = 0.0,
                                                               RotationProperty = "RotationX",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = this.Duration
                                                           };
                        hideFrontRotation = new RotationData{
                                                                FromDegrees = 0.0,
                                                                MidDegrees = 90.0,
                                                                ToDegrees = 180.0,
                                                                RotationProperty = "RotationX",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        showFrontRotation = new RotationData{
                                                                FromDegrees = 180.0,
                                                                MidDegrees = 90.0,
                                                                ToDegrees = 0.0,
                                                                RotationProperty = "RotationX",
                                                                PlaneProjection = frontPP,
                                                                AnimationDuration = this.Duration
                                                            };
                        hideBackRotation = new RotationData{
                                                               FromDegrees = 0.0,
                                                               MidDegrees = -90.0,
                                                               ToDegrees = -180.0,
                                                               RotationProperty = "RotationX",
                                                               PlaneProjection = backPP,
                                                               AnimationDuration = Duration
                                                           };
                        break;
                }

                front.RenderTransformOrigin = new Point( .5, .5 );
                back.RenderTransformOrigin = new Point( .5, .5 );

                front.Projection = frontPP;
                back.Projection = backPP;

                frontToBackStoryboard.Duration = Duration;
                backToFrontStoryboard.Duration = Duration;

                // Rotation
                frontToBackStoryboard.Children.Add( CreateRotationAnimation( showBackRotation ) );
                frontToBackStoryboard.Children.Add( CreateRotationAnimation( hideFrontRotation ) );
                backToFrontStoryboard.Children.Add( CreateRotationAnimation( hideBackRotation ) );
                backToFrontStoryboard.Children.Add( CreateRotationAnimation( showFrontRotation ) );


                // Visibility
                frontToBackStoryboard.Children.Add( CreateVisibilityAnimation( showBackRotation.AnimationDuration, front,
                                                                               false ) );
                frontToBackStoryboard.Children.Add( CreateVisibilityAnimation( hideFrontRotation.AnimationDuration, back,
                                                                               true ) );
                backToFrontStoryboard.Children.Add( CreateVisibilityAnimation( hideBackRotation.AnimationDuration, front,
                                                                               true ) );
                backToFrontStoryboard.Children.Add( CreateVisibilityAnimation( showFrontRotation.AnimationDuration, back,
                                                                               false ) );
            }

            if ( forward ){
                AssociatedObject.GetEditForm().DataEntity = AssociatedObject.DataEntity;
                double frontLeft = Canvas.GetLeft( AssociatedObject );
                double backLeft = frontLeft;

                if (AssociatedObject.ActialSize.Width<AssociatedObject.GetEditForm().Width){
                    backLeft = frontLeft -
                               ( ( AssociatedObject.GetEditForm().Width - AssociatedObject.ActialSize.Width )/2 );
                } //if 

                double backTop = (AssociatedObject.ActialSize.Height - AssociatedObject.GetEditForm().Height) / 2 + Canvas.GetTop(AssociatedObject);

                backLeft = backLeft < 0 ? 0 : backLeft;
                backTop = backTop < 0 ? 0 : backTop;
                Canvas.SetLeft( AssociatedObject.GetEditForm(), backLeft );
                Canvas.SetTop( AssociatedObject.GetEditForm(), backTop );
                frontToBackStoryboard.Begin();
                AssociatedObject.DataEntity.IsEditing = true;
            }
            else{
                backToFrontStoryboard.Begin();
                AssociatedObject.GetEditForm().RiseAppearanceChanged( false );
            }

            _isComplited = false;

            forward = !forward;
        }

        private static ObjectAnimationUsingKeyFrames CreateVisibilityAnimation( Duration duration,
                                                                                DependencyObject element, bool show )
        {
            var animation = new ObjectAnimationUsingKeyFrames();
            animation.BeginTime = new TimeSpan( 0 );
            animation.KeyFrames.Add( new DiscreteObjectKeyFrame{
                                                                   KeyTime = new TimeSpan( 0 ),
                                                                   Value =
                                                                       ( show
                                                                             ? Visibility.Collapsed
                                                                             : Visibility.Visible )
                                                               } );
            animation.KeyFrames.Add( new DiscreteObjectKeyFrame{
                                                                   KeyTime = new TimeSpan( duration.TimeSpan.Ticks/2 ),
                                                                   Value =
                                                                       ( show
                                                                             ? Visibility.Visible
                                                                             : Visibility.Collapsed )
                                                               } );
            Storyboard.SetTargetProperty( animation, new PropertyPath( "Visibility" ) );
            Storyboard.SetTarget( animation, element );
            return animation;
        }


        private static DoubleAnimationUsingKeyFrames CreateRotationAnimation( RotationData rd )
        {
            var animation = new DoubleAnimationUsingKeyFrames();
            animation.BeginTime = new TimeSpan( 0 );
            animation.KeyFrames.Add( new EasingDoubleKeyFrame{
                                                                 KeyTime = new TimeSpan( 0 ),
                                                                 Value = rd.FromDegrees,
                                                                 EasingFunction =
                                                                     new CubicEase{EasingMode = EasingMode.EaseIn}
                                                             } );
            animation.KeyFrames.Add( new EasingDoubleKeyFrame{
                                                                 KeyTime =
                                                                     new TimeSpan( rd.AnimationDuration.TimeSpan.Ticks/2 ),
                                                                 Value = rd.MidDegrees
                                                             } );
            animation.KeyFrames.Add( new EasingDoubleKeyFrame{
                                                                 KeyTime =
                                                                     new TimeSpan( rd.AnimationDuration.TimeSpan.Ticks ),
                                                                 Value = rd.ToDegrees,
                                                                 EasingFunction =
                                                                     new CubicEase{EasingMode = EasingMode.EaseOut}
                                                             } );
            Storyboard.SetTargetProperty( animation, new PropertyPath( rd.RotationProperty ) );
            Storyboard.SetTarget( animation, rd.PlaneProjection );
            return animation;
        }
    }
}