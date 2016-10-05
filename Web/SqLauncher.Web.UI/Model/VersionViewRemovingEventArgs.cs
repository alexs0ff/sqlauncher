// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   VersionViewRemovingEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 07 14:20
//   * Modified at: 2011  11 07  14:21
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args for VersionViewRemoving event.
    /// </summary>
    public class VersionViewRemovingEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.VersionViewRemovingEventArgs" /> class.
        /// </summary>
        public VersionViewRemovingEventArgs(DatabaseVersion databaseVersion)
        {
            DatabaseVersion = databaseVersion;
        }
        /// <summary>
        /// The removed database version.
        /// </summary>
        public DatabaseVersion DatabaseVersion { get; private set; }
    }
}