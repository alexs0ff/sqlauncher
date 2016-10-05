// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   MouseButtonDownEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 12 8:02 PM
//   * Modified at: 2011  09 12  8:03 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args of the mouse button down event.
    /// </summary>
    public class MouseButtonDownEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.MouseButtonDownEventArgs" /> class.
        /// </summary>
        public MouseButtonDownEventArgs(MouseButton button, int clickCount, Point position)
        {
            Button = button;
            ClickCount = clickCount;
            Position = position;
        }

        /// <summary>
        ///   The pressed button
        /// </summary>
        public MouseButton Button { get; private set; }

        /// <summary>
        ///   The count of clicks.
        /// </summary>
        public int ClickCount { get; private set; }

        /// <summary>
        ///   The position of mouse event.
        /// </summary>
        public Point Position { get; private set; }
    }

    /// <summary>
    ///   The mouse buttons.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        ///   The left button
        /// </summary>
        Left,

        /// <summary>
        ///   The right button.
        /// </summary>
        Right
    }
}