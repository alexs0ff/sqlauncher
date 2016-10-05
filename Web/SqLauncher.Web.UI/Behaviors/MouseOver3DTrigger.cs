// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   MouseOver3D.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 23 10:02 PM
//   * Modified at: 2011  08 23  10:06 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Interactivity;

namespace SqLauncher.Web.UI.Behaviors
{
    public class MouseOver3DTrigger : TargetedTriggerAction<FrameworkElement>
    {
        private PlaneProjection ProjectionTargetObject;

        private Storyboard SB_HoverZ;

        private bool bAnimationActivated;

        private FrameworkElement feAssociatedObject;

        private FrameworkElement feSourceObject;

        private FrameworkElement feTargetObject;

        private TimeSpan hoverDown_Duration = TimeSpan.FromSeconds( 0.9 );

        private double hoverOffset = 30;

        private TimeSpan hoverUp_duration = TimeSpan.FromSeconds( 0.5 );

        [Category( "Mouse Over 3D - Going Up" )]
        public TimeSpan HoverUp_duration
        {
            get { return hoverUp_duration; }
            set { hoverUp_duration = value; }
        }

        [Category( "Mouse Over 3D - Going Down" )]
        public TimeSpan HoverDown_Duration
        {
            get { return hoverDown_Duration; }
            set { hoverDown_Duration = value; }
        }

        [Category( "Mouse Over 3D - Going Up" )]
        public IEasingFunction HoverUp_Easing { get; set; }

        [Category( "Mouse Over 3D - Going Down" )]
        public IEasingFunction HoverDown_Easing { get; set; }

        [Category( "Mouse Over 3D - General" )]
        public double HoverOffset
        {
            get { return hoverOffset; }
            set { hoverOffset = value; }
        }

        protected override void Invoke( object parameter )
        {
            var myElement = this.AssociatedObject as FrameworkElement;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            feAssociatedObject = (FrameworkElement) this.AssociatedObject;
            feSourceObject = (FrameworkElement) this.AssociatedObject;
            
            feTargetObject = (FrameworkElement)this.TargetObject;
            ProjectionTargetObject = new PlaneProjection();
            if (feTargetObject == null)
            {
                feTargetObject = feAssociatedObject;
            }
            if (feTargetObject.Projection == null)
            {
                feTargetObject.RenderTransformOrigin = new Point(0.5, 0.5);
                var pj = new PlaneProjection();
                feTargetObject.Projection = pj;
            }

            feSourceObject.Loaded += feSourceObject_Loaded;
        }

        private void feSourceObject_Loaded( object sender, RoutedEventArgs e )
        {
            feSourceObject.Loaded -= feSourceObject_Loaded;
           
            feSourceObject.MouseEnter += feSourceObject_MouseEnter;
            feSourceObject.MouseLeave += feSourceObject_MouseLeave;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        private void feSourceObject_MouseLeave( object sender, MouseEventArgs e )
        {
            DeactivateAnimation();
        }

        private void feSourceObject_MouseEnter( object sender, MouseEventArgs e )
        {
            ActivateAnimation();
        }

        private void ActivateAnimation()
        {
            if ( bAnimationActivated == false ){
                AnimateHoverZ( HoverOffset, true );
                bAnimationActivated = true;
            }
        }

        private void DeactivateAnimation()
        {
            if ( bAnimationActivated ){
                AnimateHoverZ( 0, false );
                bAnimationActivated = false;
            }
        }

        private void AnimateHoverZ( Double Z, bool HoverUp )
        {
            if ( HoverUp ){
                playAnimation( feTargetObject, "(UIElement.Projection).(PlaneProjection.LocalOffsetZ)", HoverUp_duration,
                               Z, SB_HoverZ, HoverUp_Easing );
            }
            else{
                playAnimation( feTargetObject, "(UIElement.Projection).(PlaneProjection.LocalOffsetZ)",
                               HoverDown_Duration, Z, SB_HoverZ, HoverDown_Easing );
            }
        }

        public static void playAnimation( FrameworkElement element, string property, TimeSpan time, double value,
                                          Storyboard sb, IEasingFunction EasingFunction )
        {
            sb = new Storyboard();
            sb.Children.Clear();
            var animation = new DoubleAnimation();
            animation.Duration = time;
            animation.To = value;
            animation.EasingFunction = EasingFunction;
            Storyboard.SetTargetProperty( animation, new PropertyPath( property ) );
            Storyboard.SetTarget( animation, element );
            sb.Children.Add( animation );
            sb.Begin();
        }
    }
}