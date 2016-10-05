// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DatabaseVersion.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 01 21:06
//   * Modified at: 2011  11 01  21:11
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents the version of the database.
    /// </summary>
    public class DatabaseVersion : BindableModelObject, IDeepClonable<DatabaseVersion>
    {
        /// <summary>
        /// Initizlizes the new instance of the DatabaseVersion object.
        /// </summary>
        /// <param name="modelViewState">The assotiated model view state.</param>
        public DatabaseVersion( IModelViewState modelViewState )
        {
            _modelViewState = modelViewState;
        }

        /// <summary>
        ///   Gets or sets the number version.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual int Number { get; set; }

        /// <summary>
        ///   Gets or sets the date of virsion creation.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        ///   Gets or sets the name of version.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual string Name { get; set; }

        /// <summary>
        ///   Gets or sets the lock state.
        /// TODO: implement locked version state.
        /// </summary>
        [NotifyPropertyChanged(RiseValueChanged = false)]
        public virtual bool Locked { get; set; }

        /// <summary>
        /// The model view state.
        /// </summary>
        private IModelViewState _modelViewState;

        /// <summary>
        /// Gets the model view state.
        /// </summary>
        public IModelViewState ModelViewState
        {
            get { return _modelViewState; }
        }

        /// <summary>
        /// Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            _modelViewState = CreateInstance<IModelViewState>();
        }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public DatabaseVersion Clone()
        {
            var copy = CreateInstance<DatabaseVersion>();

            copy.Number = Number;
            copy.Name = Name;
            copy.CreateDate = CreateDate;
            copy._modelViewState = _modelViewState.Clone();
            copy.ClonedBy = InnerId;
            
            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}