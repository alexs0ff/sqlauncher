// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EditButtonBehavior.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 24 11:48 PM
//   * Modified at: 2011  10 24  10:33 PM
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

using SqLauncher.Web.UI.Common;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   Behevior of apperance of edit button.
    /// </summary>
    public class EditButtonBehavior : Behavior<EntityForm>
    {
        private readonly ToggleButton _editButton = new ToggleButton();

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            if ( AssociatedObject == null ){
                return;
            }

            _editButton.Style = (Style) AssociatedObject.Resources.MergedDictionaries[0]["EntityEditButtonStyle"];

            var canvas = ControlHelper.FindParent<Canvas>( AssociatedObject );
            canvas.Children.Add( _editButton );
            Canvas.SetLeft( _editButton, 0 );
            Canvas.SetTop( _editButton, 0 );
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
        }
    }
}