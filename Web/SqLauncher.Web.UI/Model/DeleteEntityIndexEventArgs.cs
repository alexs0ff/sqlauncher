// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeleteEntityIndexEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 25 18:42
//   * Modified at: 2012  02 25  18:44
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Reprsesents the event args of delete entity index event.
    /// </summary>
    public class DeleteEntityIndexEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.DeleteEntityIndexEventArgs" /> class.
        /// </summary>
        public DeleteEntityIndexEventArgs(EntityIndex index)
        {
            Index = index;
        }

        /// <summary>
        ///   The entity index to delete.
        /// </summary>
        public EntityIndex Index { get; private set; }
    }
}