// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IVersionedModelView.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 07 14:05
//   * Modified at: 2011  11 07  14:19
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents the interface for versions view.
    /// </summary>
    public interface IVersionedModelView
    {
        /// <summary>
        ///   Occurs when need to add new version.
        /// </summary>
        event EventHandler<VersionViewAddingEventArgs> VersionViewAdding;

        /// <summary>
        /// Occurs when removing version.
        /// </summary>
        event EventHandler<VersionViewRemovingEventArgs> VersionViewRemoving;

        /// <summary>
        /// Occurs when version view loaded.
        /// </summary>
        event EventHandler<VersionViewLoadedEventArgs> VersionViewLoaded;

        /// <summary>
        /// Occurs when edited model view changed.
        /// </summary>
        event EventHandler EditedModelViewChanged;

        /// <summary>
        /// Adds new database version to view.
        /// </summary>
        /// <param name="version">The database version.</param>
        IModelView AddDatabaseVersion( DatabaseVersion version);

        /// <summary>
        /// Removes the version from the view.
        /// </summary>
        /// <param name="version">The version to remove.</param>
        /// <returns>The removed view.</returns>
        IModelView RemoveDatabaseVersion( DatabaseVersion version );

        /// <summary>
        /// Gets the current model view.
        /// </summary>
        IModelView EditedModelView { get; }

        /// <summary>
        /// The associated database document.
        /// </summary>
        DatabaseDocument DataEntity { get; set; }
    }
}