// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DatabaseDocument.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 01 21:00
//   * Modified at: 2011  11 01  21:04
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents the container for all model parts.
    /// </summary>
    public class DatabaseDocument : BindableModelObject,IDeepClonable<DatabaseDocument>
    {
        /// <summary>
        ///   Gets or sets the name of document.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual string Name { get; set; }

        /// <summary>
        /// The list of avalible versions.
        /// </summary>
        private readonly ICollection<DatabaseVersion> _versions = new ObservableCollection<DatabaseVersion>();

        /// <summary>
        /// Gets the list of avalible versions.
        /// </summary>
        public ICollection<DatabaseVersion> Versions
        {
            get { return _versions; }
        }

        /// <summary>
        /// Gets or sets the model iteraction state.
        /// </summary>
        [Dependency]
        public IIteractionState IteractionState { get; set; }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public DatabaseDocument Clone()
        {
            var copy = CreateInstance<DatabaseDocument>();

            copy.ClonedBy = InnerId;
            copy.Name = Name;

            foreach ( var databaseVersion in Versions ){
                copy.Versions.Add( databaseVersion.Clone() );
            } //foreach

            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}