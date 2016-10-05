// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   MouseMoveEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 12 8:03 PM
//   * Modified at: 2011  09 12  8:03 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Mouse move event args of mouse move event.
    /// </summary>
    public class MouseMoveEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.MouseMoveEventArgs" /> class.
        /// </summary>
        public MouseMoveEventArgs( Point position )
        {
            Position = position;
        }

        /// <summary>
        ///   The position of mouse move event.
        /// </summary>
        public Point Position { get; private set; }
    }
}