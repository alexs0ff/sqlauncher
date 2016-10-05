// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   VersionedModelViewManager.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 11 07 14:34
//   * Modified at: 2011  11 07  14:40
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Linq;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The manager to handle version views.
    /// </summary>
    public class VersionedModelViewManager
    {
        /// <summary>
        /// The controllers keyed by the data model Id.
        /// </summary>
        private readonly Dictionary<Guid, ModelController> _controllers = new Dictionary<Guid, ModelController>();

        /// <summary>
        ///   The handled model view.
        /// </summary>
        private readonly IVersionedModelView _versionedModelView;
        
        /// <summary>
        ///   The model instances container.
        /// </summary>
        private readonly ContainerWiring _modelContainerWiring;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.VersionedModelViewManager" /> class.
        /// </summary>
        public VersionedModelViewManager( IVersionedModelView versionedModelView, ContainerWiring modelContainerWiring )
        {
            _versionedModelView = versionedModelView;
            _modelContainerWiring = modelContainerWiring;
            _versionedModelView.VersionViewAdding += VersionViewAdding;
            _versionedModelView.VersionViewRemoving += VersionViewRemoving;
            _versionedModelView.EditedModelViewChanged += VersionedModelViewEditedModelViewChanged;
            _versionedModelView.VersionViewLoaded += VersionViewLoaded;
            ProcessModelViewState( versionedModelView.DataEntity );
        }

        /// <summary>
        /// Occurs when version view has been loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VersionViewLoaded(object sender, VersionViewLoadedEventArgs e)
        {
            ProcessVersionLoaded( e.DatabaseVersion );
        }

        /// <summary>
        /// Processes the loaded for view the database version.
        /// </summary>
        /// <param name="databaseVersion">The database version.</param>
        private void ProcessVersionLoaded( DatabaseVersion databaseVersion )
        {
            _controllers[databaseVersion.ModelViewState.DataModel.InnerId].OnShowing();
        }

        /// <summary>
        /// Processes the model view state.
        /// </summary>
        /// <param name="document">The database document.</param>
        private void ProcessModelViewState( DatabaseDocument document )
        {
            foreach ( var databaseVersion in document.Versions ){
                ProcessAddedModelView( databaseVersion );
            } //foreach   
        }

        /// <summary>
        /// Occurs when EditedModelViewChanged event has been rised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The evnt args.</param>
        private void VersionedModelViewEditedModelViewChanged(object sender, EventArgs e)
        {
            RiseCurrentModelViewChanged();
        }

        /// <summary>
        /// Occurs when current model view changed.
        /// </summary>
        public event EventHandler CurrentModelViewChanged;

        /// <summary>
        /// The invocator of CurrentModelViewChanged event.
        /// </summary>
        private void RiseCurrentModelViewChanged()
        {
            if (CurrentModelViewChanged!=null){
                CurrentModelViewChanged( this, new EventArgs() );
            } //if
        }

        /// <summary>
        /// Occurs when removing a version view.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void VersionViewRemoving(object sender, VersionViewRemovingEventArgs e)
        {
            ProcessRemovedModelView( e.DatabaseVersion );
        }
        /// <summary>
        /// Occurs when adding new version view.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void VersionViewAdding(object sender, VersionViewAddingEventArgs e)
        {
            AddNewModelView();
        }

        /// <summary>
        /// Processes the new model view.
        /// </summary>
        /// <param name="version">The version.</param>
        private void ProcessAddedModelView(DatabaseVersion version)
        {
            var addedModelView = _versionedModelView.AddDatabaseVersion( version );
            addedModelView.DataEntity = version.ModelViewState;

            var controller = new ModelController( _modelContainerWiring );
            var modelViewManager = new ModelViewManager( addedModelView, controller );

            controller.ModelViewManager = modelViewManager;
            _controllers.Add( version.ModelViewState.DataModel.InnerId, controller );

            controller.IteractionProvider.VersionCreateDate = version.CreateDate;
            controller.IteractionProvider.VersionNumber = version.Number;

            foreach ( var entityViewState in version.ModelViewState.EntityViewStates ){
                controller.CreateEntityFormByViewState( entityViewState );
            } //foreach

            foreach ( var entityRelationState in version.ModelViewState.EntityRelationStates ){
                controller.CreateRelationFormByViewState( entityRelationState );
            } //foreach
        }

        /// <summary>
        /// Processes the new model view.
        /// </summary>
        public void AddNewModelView()
        {
            var lastVersion =GetMaximumVersion();
            DatabaseVersion versionToAdd;


            if ( lastVersion ==null ){
                versionToAdd = _modelContainerWiring.CreateInstance<DatabaseVersion>();
                versionToAdd.Number = 1;
            }
            else{
                versionToAdd = lastVersion.Clone();
                versionToAdd.Number = lastVersion.Number + 1;
            } //else

            versionToAdd.CreateDate = DateTime.Now;
            ProcessAddedModelView(versionToAdd);
            RiseCurrentModelViewChanged();
        }

        /// <summary>
        /// Processes the removed model view.
        /// </summary>
        /// <param name="databaseVersion">The deleted database version.</param>
        private void ProcessRemovedModelView( DatabaseVersion databaseVersion )
        {
            _versionedModelView.RemoveDatabaseVersion( databaseVersion );
            _controllers.Remove( databaseVersion.ModelViewState.DataModel.InnerId );
            //TODO: create deep clearing

            RiseCurrentModelViewChanged();
        }

        /// <summary>
        /// Closed the model.
        /// </summary>
        public void Close()
        {
            _versionedModelView.VersionViewAdding -= VersionViewAdding;
            _versionedModelView.VersionViewRemoving -= VersionViewRemoving;
            _versionedModelView.EditedModelViewChanged -= VersionedModelViewEditedModelViewChanged;

            _controllers.Clear();
        }

        /// <summary>
        /// Gets the controller of the edited model.
        /// If form has not edited versions then returns null.
        /// </summary>
        public ModelController CurrentController
        {
            get
            {
                var selectedModelView = _versionedModelView.EditedModelView;

                if ( selectedModelView == null || selectedModelView.DataEntity == null ){
                    return null;
                } //if

                return _controllers[selectedModelView.DataEntity.DataModel.InnerId];
            }
        }

        /// <summary>
        /// Gets the maximum from versions.
        /// </summary>
        /// <returns>The last version or null.</returns>
        public DatabaseVersion GetMaximumVersion()
        {
            if ( _versionedModelView.DataEntity.Versions.Count>0 ){
                return
                    _versionedModelView.DataEntity.Versions.ToList()[_versionedModelView.DataEntity.Versions.Count - 1];
            } //if

            return null;
        }

        /// <summary>
        ///   The handled model view.
        /// </summary>
        public IVersionedModelView VersionedModelView
        {
            get { return _versionedModelView; }
        }

        /// <summary>
        ///   The model instances container.
        /// </summary>
        public ContainerWiring Wiring
        {
            get { return _modelContainerWiring; }
        }
    }
}