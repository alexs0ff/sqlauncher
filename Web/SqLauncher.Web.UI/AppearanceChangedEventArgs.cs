// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AppearanceChangedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 15 22:17
//   * Modified at: 2012  02 15  22:20
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   The event args of apperance changed event.
    /// </summary>
    internal class AppearanceChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.AppearanceChangedEventArgs" /> class.
        /// </summary>
        public AppearanceChangedEventArgs( bool appeared )
        {
            Appeared = appeared;
        }

        /// <summary>
        ///   Gets the flag indicates that the form become visible.
        /// </summary>
        public bool Appeared { get; private set; }
    }
}