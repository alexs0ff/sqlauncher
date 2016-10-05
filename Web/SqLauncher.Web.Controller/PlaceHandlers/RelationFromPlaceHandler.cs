// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RelationFromPlaceHandler.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 11 12:59 PM
//   * Modified at: 2011  09 25  1:37 PM
// / ******************************************************************************/ 

using System.Windows;

using SqLauncher.Web.Controller.Commands;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.PlaceHandlers
{
    /// <summary>
    ///   Reprsenets the handler of new relation adding.
    /// </summary>
    internal class RelationFromPlaceHandler : IPlaceHandler
    {
        /// <summary>
        ///   The relation form.
        /// </summary>
        public IRelationForm RelationForm { get; set; }

        /// <summary>
        ///   The parent entity view.
        /// </summary>
        private IEntityForm _parentEntityForm;

        /// <summary>
        ///   The child entity view.
        /// </summary>
        private IEntityForm _childEntityForm;

        /// <summary>
        ///   The model view manager.
        /// </summary>
        public ModelViewManager ModelViewManager { get; set; }

        /// <summary>
        ///   Starts finding a new place.
        /// </summary>
        public void StartAssignNewPlace()
        {
            RelationForm.ElementLoaded += RelationFormElementLoaded;
            ModelViewManager.ModelView.AddChild( RelationForm );
        }

        /// <summary>
        ///   Occurs when the form has been loaded.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RelationFormElementLoaded( object sender, ElementLoadedEventArgs e )
        {
            ModelViewManager.EntityFormSelected += ModelViewManagerEntityFormSelected;
            ModelViewManager.ModelView.ModelMouseMove += ModelViewModelMouseMove;
            RelationForm.IsVisible = false;
        }

        /// <summary>
        ///   The model view mouse move.
        /// </summary>
        /// <param name = "sender">The sender</param>
        /// <param name = "e">The event args.</param>
        private void ModelViewModelMouseMove( object sender, MouseMoveEventArgs e )
        {
            if ( !_findParent ){
                ProcessMouseChangePosition( e.Position );
            }
        }

        /// <summary>
        ///   Updates relation line information.
        /// </summary>
        /// <param name = "position">The new position of mouse pointer.</param>
        private void ProcessMouseChangePosition( Point position )
        {
            var parentConnectors = ModelViewManager.GetEntityConnectors( _parentEntityForm );
            var mouseConnectors = ModelViewManager.GetPointConnectors( position );
            var line = ModelViewManager.GetShortestConnector( mouseConnectors, parentConnectors );
            RelationForm.StartPoint = line.End;
            RelationForm.DestinationPoint = line.Head;
        }

        /// <summary>
        ///   Occurs whe user choised an enity form.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void ModelViewManagerEntityFormSelected( object sender, FormSelectedEventArgs e )
        {
            var entityForm = e.SelectedForm as IEntityForm;
            if ( entityForm != null ){
                ProcessSelectedForm( entityForm );
            }
        }

        /// <summary>
        ///   Procesed the current selection.
        /// </summary>
        /// <param name = "entityForm">The selected entity form.</param>
        private void ProcessSelectedForm( IEntityForm entityForm )
        {
            if ( _findParent ){
                _parentEntityForm = entityForm;
                _findParent = false;
                RelationForm.IsVisible = true;
            }
            else{
                _childEntityForm = entityForm;
                var command = new AddNewEntityRelation();
                command.Controller = ModelViewManager.Controller;
                command.ChildEntityForm = _childEntityForm;
                command.ParentEntityForm = _parentEntityForm;
                command.DataModel = ModelViewManager.ModelView.DataEntity.DataModel;
                command.Done = false;
                command.EntityRelation = RelationForm.DataEntity.Relation;
                command.RelationViewState = RelationForm.DataEntity;

                Stop();

                ModelViewManager.Controller.ExecCommand( command );
            }
        }

        /// <summary>
        ///   The flag which indicates what we find a parent entity from otherwise child entity from.
        /// </summary>
        private bool _findParent = true;

        /// <summary>
        ///   Stops all operation.
        /// </summary>
        public void Stop()
        {
            CleanUp();
        }

        public void CleanUp()
        {
            if ( RelationForm != null ){
                ModelViewManager.EntityFormSelected -= ModelViewManagerEntityFormSelected;
                ModelViewManager.ModelView.ModelMouseMove -= ModelViewModelMouseMove;
                RelationForm.ElementLoaded -= RelationFormElementLoaded;
                ModelViewManager.ModelView.RemoveChild( RelationForm );

                _childEntityForm = null;
                _parentEntityForm = null;
                _findParent = true;
                RelationForm = null;
            }
        }
    }
}