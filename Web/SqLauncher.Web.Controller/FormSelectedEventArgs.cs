// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   FormSelectedEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 11 12:21 PM
//   * Modified at: 2011  09 11  12:23 PM
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.UI;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The event args of model form selected event.
    /// </summary>
    public class FormSelectedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.FormSelectedEventArgs" /> class.
        /// </summary>
        public FormSelectedEventArgs( IForm selectedForm )
        {
            SelectedForm = selectedForm;
        }

        /// <summary>
        ///   The selected form.
        /// </summary>
        public IForm SelectedForm { get; private set; }
    }
}