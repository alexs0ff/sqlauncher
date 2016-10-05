// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ImageMouseOverBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 29 1:43 PM
//   * Modified at: 2011  08 29  2:38 PM
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   Represents growthing behavior on image mouse over.
    /// </summary>
    public class ImageMouseOverBehavior : Behavior<Image>
    {
        #region Animation 

        /// <summary>
        ///   The factor to grouwthing resize.
        /// </summary>
        public double ResizeFactor { get; set; }

        /// <summary>
        ///   The duration of the growth action.
        /// </summary>
        public Duration GrowthDuration { get; set; }

        /// <summary>
        ///   The storyboard of incrise.
        /// </summary>
        private readonly Storyboard _increaseStorybouard = new Storyboard();

        /// <summary>
        ///   The storyboard of decrise.
        /// </summary>
        private readonly Storyboard _decreaseStorybouard = new Storyboard();

        /// <summary>
        ///   Initialize the double animation.
        /// </summary>
        /// <param name = "propertyPath">The target property.</param>
        /// <param name="toValue">The target value.</param>
        /// <returns>Created animation.</returns>
        private DoubleAnimation CreateDoubleAnimation(string propertyPath,double toValue )
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.BeginTime = new TimeSpan( 0 );
            animation.Duration = GrowthDuration;
            animation.To = toValue;
            Storyboard.SetTargetProperty( animation, new PropertyPath( propertyPath ) );
            Storyboard.SetTarget( animation, AssociatedObject );

            return animation;
        }

        #endregion Animation

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            AssociatedObject.MouseEnter += MouseEnter;
            AssociatedObject.MouseLeave += MouseLeave;
            
            AssociatedObject.RenderTransform = new ScaleTransform();
            AssociatedObject.RenderTransformOrigin = new Point( 0.5, 0.5 );
            _increaseStorybouard.Children.Add(CreateDoubleAnimation("(RenderTransform).(ScaleTransform.ScaleX)", ResizeFactor));
            _increaseStorybouard.Children.Add(CreateDoubleAnimation("(RenderTransform).(ScaleTransform.ScaleY)", ResizeFactor));

            _decreaseStorybouard.Children.Add(CreateDoubleAnimation("(RenderTransform).(ScaleTransform.ScaleX)", 1.0));
            _decreaseStorybouard.Children.Add(CreateDoubleAnimation("(RenderTransform).(ScaleTransform.ScaleY)", 1.0));
        }

        

        private void MouseLeave( object sender, MouseEventArgs e )
        {
            _decreaseStorybouard.Begin();
        }

        private void MouseEnter( object sender, MouseEventArgs e )
        {
            _increaseStorybouard.Begin();
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            AssociatedObject.MouseEnter -= MouseEnter;
            AssociatedObject.MouseLeave -= MouseLeave;
            AssociatedObject.RenderTransform = null;
        }
    }
}