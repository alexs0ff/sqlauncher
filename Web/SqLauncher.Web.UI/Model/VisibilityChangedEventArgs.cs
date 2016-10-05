// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   VisibilityChangedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 12 8:04 PM
//   * Modified at: 2011  09 12  8:04 PM
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents a event args of visibility changed notifications.
    /// </summary>
    public class VisibilityChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.VisibilityChangedEventArgs" /> class.
        /// </summary>
        public VisibilityChangedEventArgs( bool isVisible )
        {
            IsVisible = isVisible;
        }

        /// <summary>
        ///   The flag indicate visibility of element.
        /// </summary>
        public bool IsVisible { get; private set; }
    }
}