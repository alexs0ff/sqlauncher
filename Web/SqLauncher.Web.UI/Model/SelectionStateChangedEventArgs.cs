// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SelectionStateChangedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 12 8:05 PM
//   * Modified at: 2011  09 12  8:06 PM
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents event args for a form selection change event.
    /// </summary>
    public class SelectionStateChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.SelectionStateChangedEventArgs" /> class.
        /// </summary>
        public SelectionStateChangedEventArgs( bool isSelected )
        {
            IsSelected = isSelected;
        }

        /// <summary>
        ///   Indicates what form is selected at current time.
        /// </summary>
        public bool IsSelected { get; private set; }
    }
}