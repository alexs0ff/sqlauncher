// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DialogClosedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 25 13:26
//   * Modified at: 2011  11 25  13:28
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args for DialogClosed event.
    /// </summary>
    public class DialogClosedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.DialogClosedEventArgs" /> class.
        /// </summary>
        public DialogClosedEventArgs( DialogResult result )
        {
            Result = result;
        }

        /// <summary>
        ///   The cloded result.
        /// </summary>
        public DialogResult Result { get; private set; }
    }
}