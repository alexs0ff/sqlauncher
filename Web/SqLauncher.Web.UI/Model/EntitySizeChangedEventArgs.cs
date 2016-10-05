// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntitySizeChangedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 26 9:20 PM
//   * Modified at: 2011  09 26  9:24 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The evnet args of size changed event.
    /// </summary>
    public class EntitySizeChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.EntitySizeChangedEventArgs" /> class.
        /// </summary>
        public EntitySizeChangedEventArgs( Rect oldSize, Rect newSize )
        {
            OldSize = oldSize;
            NewSize = newSize;
        }

        /// <summary>
        ///   The old size.
        /// </summary>
        public Rect OldSize { get; private set; }

        /// <summary>
        ///   The new size.
        /// </summary>
        public Rect NewSize { get; private set; }
    }
}