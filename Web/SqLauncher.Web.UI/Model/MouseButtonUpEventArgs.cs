// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   MouseButtonUpEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 03 9:42 PM
//   * Modified at: 2011  10 03  10:04 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args for mouse up event.
    /// </summary>
    public class MouseButtonUpEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.MouseButtonUpEventArgs" /> class.
        /// </summary>
        public MouseButtonUpEventArgs( MouseButton button, int clickCount, Point position )
        {
            Position = position;
            Button = button;
            ClickCount = clickCount;
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
}