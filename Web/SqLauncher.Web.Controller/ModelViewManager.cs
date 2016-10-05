// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelViewManager.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  01 17  22:30
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using SqLauncher.Web.Controller.Carriers;
using SqLauncher.Web.Controller.Commands;
using SqLauncher.Web.Controller.PlaceHandlers;
using SqLauncher.Web.UI.Common.Measure;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   Represents a manager of model view.
    /// </summary>
    public class ModelViewManager
    {
        /// <summary>
        ///   The main modelView.
        /// </summary>
        private readonly IModelView _modelView;

        /// <summary>
        ///   The controller.
        /// </summary>
        private readonly ModelController _controller;

        /// <summary>
        ///   Gets the controller.
        /// </summary>
        internal ModelController Controller
        {
            get { return _controller; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.ModelViewManager" /> class.
        /// </summary>
        public ModelViewManager( IModelView modelView, ModelController controller )
        {
            _modelView = modelView;
            _controller = controller;

            _modelView.MouseButtonDown += ModelViewMouseButtonDown;
            _modelView.MouseButtonUp += ModelViewMouseButtonUp;
            _modelView.ModelMouseMove += ModelViewModelMouseMove;
        }

        #region Placing 

        /// <summary>
        ///   The place handler of new Entity Forms.
        /// </summary>
        private IPlaceHandler _formPlaceHandler;

        /// <summary>
        ///   Assigns a new place for the enity form.
        /// </summary>
        /// <param name = "newForm">The form instance.</param>
        public void AssignPlace( IEntityForm newForm )
        {
            StopPlaceHandler();

            _formPlaceHandler = new EntityFromPlaceHandler{EntityForm = newForm, ModelViewManager = this};
            _formPlaceHandler.StartAssignNewPlace();
        }

        /// <summary>
        ///   Stops the current place handler.
        /// </summary>
        private void StopPlaceHandler()
        {
            if ( _formPlaceHandler != null ){
                _formPlaceHandler.Stop();
            }
        }

        /// <summary>
        ///   Assigns a new place fo the relation.
        /// </summary>
        /// <param name = "newRelation">The new relation.</param>
        public void AssignPlace( IRelationForm newRelation )
        {
            StopPlaceHandler();
            _formPlaceHandler = new RelationFromPlaceHandler
                                {ModelViewManager = this, RelationForm = newRelation};
            _formPlaceHandler.StartAssignNewPlace();
        }

        /// <summary>
        /// Stops the current place handler.
        /// </summary>
        public void StopCurrentOperation()
        {
            StopPlaceHandler();
            UnselectAllForms();
        }

        #endregion Placing

        #region Entities

        /// <summary>
        ///   The registred entities.
        /// </summary>
        private readonly IDictionary<Guid, EntityFormChangeNotifierContainer> _registerdEntities =
            new Dictionary<Guid, EntityFormChangeNotifierContainer>();

        /// <summary>
        ///   Gets the registred entity forms.
        /// </summary>
        public IEnumerable<IEntityForm> RegistredEntityForms
        {
            get { return from entity in _registerdEntities select entity.Value.EntityForm; }
        }

        /// <summary>
        ///   The registred relations.
        /// </summary>
        private readonly IDictionary<Guid, IRelationForm> _registredRelations = new Dictionary<Guid, IRelationForm>();

        /// <summary>
        ///   Gets the registred relation froms.
        /// </summary>
        private IEnumerable<IRelationForm> RegistredRelationForms
        {
            get { return from registredRelation in _registredRelations select registredRelation.Value; }
        }

        /// <summary>
        ///   Gets registred forms.
        /// </summary>
        private IEnumerable<IForm> RegistredForms
        {
            get
            {
                var registredEntityForms = from entityForm in RegistredEntityForms select (IForm) entityForm;
                var registredRelationForms = from relationForm in RegistredRelationForms select (IForm) relationForm;

                return registredEntityForms.Union( registredRelationForms );
            }
        }

        /// <summary>
        ///   Adds entity from on a specified place.
        /// </summary>
        /// <param name = "form">The form.</param>
        public void Add( IEntityForm form )
        {
            form.ElementLoaded += EntityFormLoaded;

            ModelView.DataEntity.EntityViewStates.SafeAdd( form.DataEntity );
            ModelView.AddChild( form );
            form.ForceResize( new Rect( form.DataEntity.Location.X, form.DataEntity.Location.Y, form.DataEntity.Width,
                                        form.DataEntity.Height ) );
            RegisterNewEntityForm(form);
        }

        /// <summary>
        ///   Removes the specified entity form.
        /// </summary>
        /// <param name = "form">The form to remove.</param>
        public void Remove( IEntityForm form )
        {
            ModelView.DataEntity.EntityViewStates.Remove( form.DataEntity );
            form.ElementLoaded -= EntityFormLoaded;
            ModelView.RemoveChild( form );
            UnregisterEntityForm( form );
            RemoveDependedRelations( form );
        }

        /// <summary>
        ///   Removes the specified entity form.
        /// </summary>
        /// <param name = "viewState">The view state to remove.</param>
        public void Remove( IEntityViewState viewState )
        {
            if ( _registerdEntities.ContainsKey( viewState.Entity.InnerId ) ){
                var form = _registerdEntities[viewState.Entity.InnerId].EntityForm;
                Remove( form );
            } //if
        }

        /// <summary>
        ///   Occurs when EntityForm loaded.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The routed event args.</param>
        private void EntityFormLoaded( object sender, ElementLoadedEventArgs e )
        {
            var form = (IEntityForm) sender;
            form.ElementLoaded -= EntityFormLoaded;
            form.IsSelected = false;
        }

        /// <summary>
        ///   Unregistereng the entity form.
        /// </summary>
        /// <param name = "form">The form.</param>
        private void UnregisterEntityForm( IEntityForm form )
        {
            var notifierContainer = _registerdEntities[form.DataEntity.Entity.InnerId];
            form.PositionChanging -= RegistredEntityFormPositionChanging;
            form.SelectionStateChanged -= RegistredEntityFormSelectionStateChanged;
            var propertyChanged = form.DataEntity as INotifyPropertyChanged;

            if (propertyChanged!=null)
            {
                propertyChanged.PropertyChanged -= RegisteredFromViewStatePropertyChanged;
            } //if

            notifierContainer.EntityForm = null;
            notifierContainer.EntityFormChangesManager.Dispose();
            _registerdEntities.Remove( form.DataEntity.Entity.InnerId );
        }

        /// <summary>
        /// Moves an entity form to new position.
        /// </summary>
        /// <param name="entityFormId">The entity form id.</param>
        /// <param name="newPosition">The new position point.</param>
        public void Move(Guid entityFormId, Point newPosition)
        {
            _registerdEntities[entityFormId].EntityForm.SetLeft( newPosition.X );
            _registerdEntities[entityFormId].EntityForm.SetTop( newPosition.Y );
        }

        /// <summary>
        /// Changes the size of an entity.
        /// </summary>
        /// <param name="entityFormId">The entity form id.</param>
        /// <param name="size">The new size of entity form.</param>
        public void SetFormSize(Guid entityFormId, Rect size)
        {
            _registerdEntities[entityFormId].EntityForm.ForceResize( size );
        }

        #region Registred Entity form view state property changed 

        /// <summary>
        /// Occurs when registred form propertu changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void RegisteredFromViewStatePropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            ProcessPropertyChanged( (IEntityViewState) sender, e.PropertyName );
        }

        /// <summary>
        /// Processes all changes of registred entity view state.
        /// </summary>
        /// <param name="entityViewState">The entity view state.</param>
        /// <param name="propertyName">The property name that has been changed.</param>
        private void ProcessPropertyChanged( IEntityViewState entityViewState, string propertyName )
        {
            switch ( propertyName ){
                case "IsEditing":// entityViewState.IsEditing
                    OnEntityViewStateIsEditingChanged( entityViewState, entityViewState.IsEditing );
                    break;
            } //switch
        }

        /// <summary>
        /// Occurs when property IsEditing entity view state has been changed.
        /// </summary>
        /// <param name="entityViewState">The shanged object.</param>
        /// <param name="value">The new value.</param>
        private void OnEntityViewStateIsEditingChanged( IEntityViewState entityViewState, bool value )
        {
            SetVisibilityToDependedRelations( entityViewState.Entity.InnerId, !value );
        }

        #endregion Registred Entity form view state property changed
        
        /// <summary>
        ///   Registereng the entity form.
        /// </summary>
        /// <param name = "form">The form.</param>
        private void RegisterNewEntityForm( IEntityForm form )
        {
            //subscribe on position change events
            form.PositionChanging += RegistredEntityFormPositionChanging;
            form.SelectionStateChanged += RegistredEntityFormSelectionStateChanged;
            var propertyChanged = form.DataEntity as INotifyPropertyChanged;

            if (propertyChanged != null)
            {
                propertyChanged.PropertyChanged += RegisteredFromViewStatePropertyChanged;
            } //if

            var entityFormChangesManager = new EntityFormChangesManager( form, _controller );

            //registering the form and listeners
            _registerdEntities.Add( form.DataEntity.Entity.InnerId,
                                    new EntityFormChangeNotifierContainer( form, entityFormChangesManager ) );
        }

        /// <summary>
        ///   Adds a new relation.
        /// </summary>
        /// <param name = "newForm">The relaction instance.</param>
        public void Add( IRelationForm newForm )
        {
            ModelView.AddChild( newForm );
            ModelView.DataEntity.EntityRelationStates.SafeAdd( newForm.DataEntity );
            _registredRelations.Add( newForm.DataEntity.Relation.InnerId, newForm );
            newForm.ElementLoaded += RelationFormLoaded;
        }

        /// <summary>
        ///   Occurs when relation form has been loaded.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RelationFormLoaded( object sender, ElementLoadedEventArgs e )
        {
            var relation = (IRelationForm) sender;
            relation.ElementLoaded -= RelationFormLoaded;
        }

        /// <summary>
        /// Removes the relation view state.
        /// </summary>
        /// <param name="relationViewState"></param>
        public void Remove(IRelationViewState relationViewState)
        {
            if ( _registredRelations.ContainsKey( relationViewState.Relation.InnerId ) ){
                Remove( _registredRelations[relationViewState.Relation.InnerId] );
            } //if
        }

        /// <summary>
        ///   Removes the specified relation form.
        /// </summary>
        /// <param name = "form">The form to remove.</param>
        public void Remove( IRelationForm form )
        {
            form.IsSelected = false;

            ModelView.DataEntity.EntityRelationStates.Remove( form.DataEntity );
            ModelView.RemoveChild( form );

            _registredRelations.Remove( form.DataEntity.Relation.InnerId );
            form.ElementLoaded -= RelationFormLoaded;
        }

        #endregion Entities

        #region Selection 

        /// <summary>
        ///   Removes selected items.
        /// </summary>
        public void RemoveSelectedItems()
        {
            var selectedEntityForms = SelectedEntityForms.ToList();

            var selectedRelationForms = SelectedRelationForms.ToList();

            if ( selectedEntityForms.Count > 0 ){
                var command = new DeleteERDEntityForms();

                command.Controller = Controller;
                command.EntityViewStates = SelectedEntityViewStates.ToList();

                var dependedRelations = new Collection<IRelationViewState>();

                foreach ( var entityForm in command.EntityViewStates ){
                    foreach ( var dependedRelation in GetDependedRelationForms( entityForm.Entity.InnerId ) ){
                        dependedRelations.Add( dependedRelation.DataEntity );
                    } //foreach
                } //foreach

                command.RelationViewStates = dependedRelations;

                Controller.ExecCommand( command );
            } //if selectedEntityForms

            if ( selectedRelationForms.Count > 0 ){
                var command = new DeleteRelationForms();
                command.Controller = Controller;
                command.RelationViewStates = (from selectedRelationForm in selectedRelationForms
                                             select selectedRelationForm.DataEntity).ToList();

                Controller.ExecCommand( command );
            } //if
        }

        /// <summary>
        ///   Unselects all registred entity form.
        /// </summary>
        public void UnselectAllEntityForms()
        {
            UnselectAllEntityForms( null );
        }

        /// <summary>
        ///   Unselects all registred entity form.
        /// </summary>
        public void UnselectAllEntityForms( IEntityForm entityFormToSkip )
        {
            foreach ( var selectedForm in SelectedEntityForms ){
                if ( selectedForm != entityFormToSkip ){
                    selectedForm.IsSelected = false;
                }
            }
        }

        /// <summary>
        ///   Gets all selected entity forms at current time.
        /// </summary>
        private IEnumerable<IEntityForm> SelectedEntityForms
        {
            get
            {
                return from container in _registerdEntities
                       where container.Value.EntityForm.IsSelected
                       select container.Value.EntityForm;
            }
        }

        /// <summary>
        ///   Gets all selected entity forms at current time.
        /// </summary>
        private IEnumerable<IRelationForm> SelectedRelationForms
        {
            get
            {
                return from registredRelation in _registredRelations
                       where registredRelation.Value.IsSelected
                       select registredRelation.Value;
            }
        }

        /// <summary>
        ///   Unselects all registred relation forms.
        /// </summary>
        private void UnselectAllRelationForms()
        {
            foreach ( var selectedRelationForm in SelectedRelationForms ){
                selectedRelationForm.IsSelected = false;
            } //foreach
        }

        /// <summary>
        ///   Unselectes all forms.
        /// </summary>
        private void UnselectAllForms()
        {
            UnselectAllRelationForms();
            UnselectAllEntityForms();
        }

        /// <summary>
        ///   Occurs when selection state changed of a registred entity form.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RegistredEntityFormSelectionStateChanged( object sender, SelectionStateChangedEventArgs e )
        {
            if ( e.IsSelected ){
                RiseEntityFormSelected( (IEntityForm) sender );
                _selectedEntityFormCounter++;
                _provider.HasSelectedEntities = true;
            }
            else{
                _selectedEntityFormCounter--;
                if ( _selectedEntityFormCounter ==0 ){
                    _provider.HasSelectedEntities = false;    
                } //if
            } //else

        }

        /// <summary>
        /// The counter of selected entity form.
        /// </summary>
        private int _selectedEntityFormCounter;

        /// <summary>
        ///   Occurs when a entity form has been selected.
        /// </summary>
        public event EventHandler<FormSelectedEventArgs> EntityFormSelected;

        /// <summary>
        ///   The invocator of FormSelected event.
        /// </summary>
        /// <param name = "selectedForm"></param>
        private void RiseEntityFormSelected( IForm selectedForm )
        {
            EventHandler<FormSelectedEventArgs> handler = EntityFormSelected;
            if ( handler != null ){
                handler( this, new FormSelectedEventArgs( selectedForm ) );
            }
        }

        /// <summary>
        ///   The current carrier.
        /// </summary>
        private Carrier _currentCarrier;

        /// <summary>
        ///   Occurs when mouse move on model view canvas.
        /// </summary>
        private void ModelViewModelMouseMove( object sender, ScaledMouseMoveEventArgs e )
        {
            if ( _currentCarrier != null ){
                _currentCarrier.Move( e.Position );
            } //if
        }

        /// <summary>
        ///   Occurs when mouse button up.
        /// </summary>
        private void ModelViewMouseButtonUp( object sender, MouseButtonUpEventArgs e )
        {
            if ( _currentCarrier != null ){
                _currentCarrier.Stop();
                _currentCarrier = null;
            } //if
        }

        /// <summary>
        ///   Occurs when mouse button down.
        /// </summary>
        private void ModelViewMouseButtonDown( object sender, MouseButtonDownEventArgs e )
        {
            var formsUnderPointer = ( from entity in RegistredForms
                                      where entity.HitTest( e.Position )
                                      select entity ).ToList();


            bool finded = formsUnderPointer.Count > 0;

            if ( !finded ){
                _currentCarrier = new ModelSelectionCarrier( ModelView, RegistredEntityForms );
                _currentCarrier.Start( e.Position );
                UnselectAllForms();
            } //if
            else{
                var selectedForms = SelectedEntityForms.ToList();

                if ( selectedForms.Count == 1 &&
                     !formsUnderPointer.Any( form => ReferenceEquals( form, selectedForms[0] ) ) ){
                    selectedForms[0].IsSelected = false;
                }

                UnselectAllRelationForms();

                SelectFormWithMaxZIndex( formsUnderPointer );

                _currentCarrier = new EntityFormCarrier( SelectedEntityForms, ModelView, _controller );
                _currentCarrier.Start( e.Position );
            } //else
        }

        /// <summary>
        ///   Selects the topmost entity forms.
        /// </summary>
        /// <param name = "formsUnderPointer">The entity forms list.</param>
        private void SelectFormWithMaxZIndex( IEnumerable<IForm> formsUnderPointer )
        {
            var forms =
                ( from form in formsUnderPointer orderby form.GetZIndex() descending select form ).
                    Take( 1 ).ToList();

            if ( forms.Count > 0 ){
                forms[0].IsSelected = true;

                var entityForm = forms[0] as IEntityForm;

                if ( entityForm != null && entityForm.DataEntity.IsEditing ){
                    UnselectAllEntityForms();
                } //if

            } //if
            
        }

        #endregion Selection

        #region Clipboard 

        /// <summary>
        /// The selected entity view states.
        /// </summary>
        public IEnumerable<IEntityViewState> SelectedEntityViewStates
        {
            get { return from selectedForm in SelectedEntityForms select selectedForm.DataEntity; }
        }

        /// <summary>
        ///   Copies into clipboard all selected items.
        /// </summary>
        public void CopySelectedItems()
        {
            var entities = SelectedEntityViewStates.ToList();

            if ( entities.Count > 0 ){
                ClipboardManager.Manager.SetEntities( entities );

                Controller.IteractionProvider.Global.CanInsert = true;
            } //if
        }

        /// <summary>
        ///   Insertes stored items into model.
        /// </summary>
        public void PasteItemsFromClipboard()
        {
            var entities = ClipboardManager.Manager.GetStoredEntities();

            if ( entities.Count > 0 ){
                var command = new PasteItemsIntoModel();
                command.Controller = Controller;
                command.Entities = entities;
                Controller.ExecCommand(command);    
            } //if
        }

        /// <summary>
        /// Cutttes selected items to clipboard.
        /// </summary>
        public void CutItemToClipboard()
        {
            var entities = SelectedEntityViewStates.ToList();
            if ( entities.Count > 0 ){
                var command = new CutItemsToClipboard();

                command.Entities = entities;
                command.Controller = Controller;
                Controller.ExecCommand(command);

                Controller.IteractionProvider.Global.CanInsert = true;    
            } //if
        }

        /// <summary>
        ///   Duplicates selected items and move into clipboard.
        /// </summary>
        public void DublicateSelectedItems()
        {
            CopySelectedItems();
            PasteItemsFromClipboard();
        }

        #endregion Clipboard

        #region Relation Forms handling

        /// <summary>
        ///   Removes all depended relation of entity form.
        /// </summary>
        /// <param name = "form">The entity form.</param>
        private void RemoveDependedRelations( IEntityForm form )
        {
            var forms = GetDependedRelationForms( form.DataEntity.Entity.InnerId ).ToList();

            foreach ( var relationForm in forms ){
                Remove( relationForm );
            } //foreach
        }

        /// <summary>
        ///   Sets visibility state to related relation forms.
        /// </summary>
        /// <param name = "formId">The parent or child form Id.</param>
        /// <param name = "isVisible">The visibility value.</param>
        public void SetVisibilityToDependedRelations( Guid formId, bool isVisible )
        {
            foreach ( var relation in GetDependedRelationForms( formId ) ){
                var secondFormId = formId == relation.DataEntity.Relation.Parent.InnerId
                                       ? relation.DataEntity.Relation.Child.InnerId
                                       : relation.DataEntity.Relation.Parent.InnerId;
                if ( !isVisible ){
                    relation.IsVisible = false;
                } //if
                else if (! _registerdEntities[secondFormId].EntityForm.DataEntity.IsEditing ){
                    relation.IsVisible = true;    
                } //if
            }
        }

        /// <summary>
        ///   Occurs when position at canvas of a EntityForm has been changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void RegistredEntityFormPositionChanging( object sender, PositionChangingEventArgs e )
        {
            DependentRelationsLayoutUpdate( ( (IEntityForm) sender ).DataEntity );
        }

        /// <summary>
        ///   Updates the layout of relation form by debendent EntityForm id.
        /// </summary>
        /// <param name = "entityViewState">The entity view state what dependet on it.</param>
        public void DependentRelationsLayoutUpdate( IEntityViewState entityViewState )
        {
            foreach (var relationForm in GetDependedRelationForms(entityViewState.Entity.InnerId))
            {
                DependentRelationLayoutUpdate( relationForm );
            }
        }

        /// <summary>
        /// Relation layout update.
        /// </summary>
        /// <param name="relationViewState">The rlation view state.</param>
        public void RelationLayoutUpdate(IRelationViewState relationViewState)
        {
            DependentRelationLayoutUpdate( _registredRelations[relationViewState.Relation.InnerId] );
        }

        /// <summary>
        ///   Updates layot of relation.
        /// </summary>
        /// <param name = "relationForm">T h erelation form that we will update.</param>
        private void DependentRelationLayoutUpdate( IRelationForm relationForm )
        {
            if ( relationForm == null || relationForm.DataEntity.Relation.Child == null ||
                 relationForm.DataEntity.Relation.Parent == null ){
                return;
            } //if

            if ( !_registerdEntities.ContainsKey( relationForm.DataEntity.Relation.Child.InnerId ) ||
                 !_registerdEntities.ContainsKey( relationForm.DataEntity.Relation.Parent.InnerId ) ){
                return;
            } //if

            var childForm = _registerdEntities[relationForm.DataEntity.Relation.Child.InnerId].EntityForm;
            var parentForm = _registerdEntities[relationForm.DataEntity.Relation.Parent.InnerId].EntityForm;

            var line = GetLineBetweenForms( childForm, parentForm );

            relationForm.StartPoint = line.End;
            relationForm.DestinationPoint = line.Head;

            //search sibling connectors
            ProcessSibling( parentForm, childForm, line );
        }

        /// <summary>
        /// The distance between side connector`s.
        /// </summary>
        private const int ConnectorDistance = 16;

        /// <summary>
        /// Recalculates all sibling connector positions.
        /// </summary>
        /// <param name="parentForm">The parent form.</param>
        /// <param name="childForm">The child form.</param>
        /// <param name="line">The current line.</param>
        private void ProcessSibling( IEntityForm parentForm, IEntityForm childForm, LineDescriptor line )
        {
            var childsSibling = GetSiblingRelationForms( childForm, line.End );
            var parentsSibling = GetSiblingRelationForms( parentForm, line.Head );

            if ( childsSibling.Count>1 ){
                var rangedChilds = RangeSibling(childsSibling, line.End.RectSide, childForm);
                CalculateConnectorPositions( childForm,line.End, rangedChilds );
            } //if

            if ( parentsSibling.Count>1 ){
                var rangedParents = RangeSibling(parentsSibling, line.Head.RectSide, parentForm);
                CalculateConnectorPositions( parentForm, line.Head, rangedParents );
            } //if
        }

        /// <summary>
        /// Ranges siblings relation forms by opposite entity forms.
        /// </summary>
        /// <param name="relationForms">The rlation form list.</param>
        /// <param name="rectSide">The rect side.</param>
        /// <param name="entityForm">The entity form.</param>
        /// <returns>The ranged list of relation forms.</returns>
        private ICollection<IRelationForm> RangeSibling(IEnumerable<IRelationForm> relationForms, RectSide rectSide, IEntityForm entityForm)
        {
            var orderByTop = false;
            if (rectSide == RectSide.Left || rectSide == RectSide.Right)
            {
                orderByTop = true;

            } //if
           
            return
                (from relationFormAndEntityFormPair in
                     (from relationForm in relationForms
                      select new KeyValuePair<IEntityForm, IRelationForm>(
                          relationForm.DataEntity.Relation.Parent.InnerId == entityForm.DataEntity.Entity.InnerId
                            ? _registerdEntities[relationForm.DataEntity.Relation.Child.InnerId].EntityForm
                            : _registerdEntities[relationForm.DataEntity.Relation.Parent.InnerId].EntityForm,relationForm)
                          )
                     orderby orderByTop ? relationFormAndEntityFormPair.Key.GetTop() : relationFormAndEntityFormPair.Key.GetLeft()
                     select relationFormAndEntityFormPair.Value ).ToList();
             
        }

        /// <summary>
        /// Calculates the new connector positions.
        /// </summary>
        /// <param name="entityForm">The entity form.</param>
        /// <param name="currentRectConnector">The current rect connector.</param>
        /// <param name="rangedForms">The ranged form list.</param>
        private static void CalculateConnectorPositions(IEntityForm entityForm, RectConnector currentRectConnector,
                                                         ICollection<IRelationForm> rangedForms)
        {
            int leftBound = 0 - rangedForms.Count/2;
            int rightBound = 0 + rangedForms.Count/2;
            var enumerator = rangedForms.GetEnumerator();
            var isEven = rangedForms.Count%2 == 0;
            var middlePoint = GetMiddleSidePoint( entityForm, currentRectConnector.RectSide );

            if ( !isEven ){
                rightBound++;
            } //if

            for ( int i = leftBound; i < rightBound; i++ ){
                enumerator.MoveNext();
                var currentRelationForm = enumerator.Current;

                var offsetX = isEven ? ( i*ConnectorDistance + ConnectorDistance/2 ) : i*ConnectorDistance;
                var offsetY = offsetX;

                if ( currentRectConnector.RectSide == RectSide.Left || currentRectConnector.RectSide == RectSide.Right ){
                    offsetX = 0;
                } //if
                else{
                    offsetY = 0;
                } //else

                if (currentRelationForm.DataEntity.Relation.Parent.InnerId == entityForm.DataEntity.Entity.InnerId && currentRelationForm.DestinationPoint.RectSide == currentRectConnector.RectSide)
                {
                    currentRelationForm.DestinationPoint =
                        new RectConnector(currentRelationForm.DestinationPoint.RectSide,
                                           new Point(middlePoint.X + offsetX,
                                                      middlePoint.Y + offsetY));
                } //if
                else if (currentRelationForm.DataEntity.Relation.Child.InnerId == entityForm.DataEntity.Entity.InnerId && currentRelationForm.StartPoint.RectSide == currentRectConnector.RectSide)
                {
                    currentRelationForm.StartPoint =
                       new RectConnector(currentRelationForm.StartPoint.RectSide,
                                          new Point(middlePoint.X + offsetX,
                                                     middlePoint.Y + offsetY));
                } //else
                
            } //for
        }

        /// <summary>
        /// Gets the sibling relation forms.
        /// </summary>
        /// <param name="entityForm">The entity form.</param>
        /// <param name="rectConnector">The rect connector.</param>
        /// <returns></returns>
        private IList<IRelationForm> GetSiblingRelationForms(IEntityForm entityForm, RectConnector rectConnector)
        {
            return ( from entityRelation in entityForm.DataEntity.Entity.Relations
                     where
                         ( 
                           _registredRelations[entityRelation.InnerId].StartPoint.RectSide == rectConnector.RectSide )
                         ||
                         (
                           _registredRelations[entityRelation.InnerId].DestinationPoint.RectSide ==
                           rectConnector.RectSide )
                     select _registredRelations[entityRelation.InnerId] ).ToList();
        }

        /// <summary>
        ///   Calculetes a middle of the line coordionats.
        /// </summary>
        /// <param name = "head"></param>
        /// <param name = "end"></param>
        /// <returns>Middle point.</returns>
        private static Point CalcMiddlePoint( Point head, Point end )
        {
            return new Point( ( head.X + end.X )/2, ( head.Y + end.Y )/2 );
        }

        /// <summary>
        ///   Gets a shortest line between two entity Forms.
        /// </summary>
        /// <param name = "childForm">The child form</param>
        /// <param name = "parentForm">The parent form.</param>
        private LineDescriptor GetLineBetweenForms( IEntityForm childForm, IEntityForm parentForm )
        {
            var childConnectors = GetEntityConnectors( childForm );
            var parentConnectors = GetEntityConnectors( parentForm );

            return GetShortestConnector( childConnectors, parentConnectors );
        }

        /// <summary>
        ///   Calculates a shortest line.
        /// </summary>
        /// <param name = "childConnectors">The child connectors.</param>
        /// <param name = "parentConnectors">The parent connectors.</param>
        /// <returns>The shortest line descriptor.</returns>
        public LineDescriptor GetShortestConnector( RectConnector[] childConnectors, RectConnector[] parentConnectors )
        {
            var result = new Collection<LineDescriptor>();

            for ( int i = 0; i < 4; i++ ){
                for ( int j = 0; j < 4; j++ ){
                    result.Add( new LineDescriptor( childConnectors[i], parentConnectors[j] ) );
                }
            }

            var line = ( from l in result orderby l.Lenght select l ).Take( 1 ).ToArray();
            return line[0];
        }

        /// <summary>
        ///   Gets the sides connectors of the entity form.
        /// </summary>
        /// <param name = "form">The form.</param>
        /// <returns>4 connectors</returns>
        public RectConnector[] GetEntityConnectors(IEntityForm form)
        {
            var result = new RectConnector[4];

            result[0] = new RectConnector( RectSide.Top, GetMiddleSidePoint( form, RectSide.Top )
                );
            result[1] = new RectConnector( RectSide.Right, GetMiddleSidePoint( form, RectSide.Right )
                );

            result[2] = new RectConnector( RectSide.Bottom, GetMiddleSidePoint( form, RectSide.Bottom )
                );

            result[3] = new RectConnector( RectSide.Left, GetMiddleSidePoint( form, RectSide.Left )
                );

            return result;
        }


        /// <summary>
        /// Gets the middle point of specific side.
        /// </summary>
        /// <param name="form">The entity form.</param>
        /// <param name="side">The side.</param>
        /// <returns>The point.</returns>
        private static Point GetMiddleSidePoint(IEntityForm form,RectSide side)
        {
            double leftX = form.GetLeft();
            double leftY = form.GetTop();


            switch (side)
            {
                case RectSide.Left:
                    return CalcMiddlePoint( new Point( leftX, leftY + form.CurrentHeight ),
                                     new Point( leftX, leftY ) );
                case RectSide.Top:
                    return CalcMiddlePoint( new Point( leftX, leftY ),
                                            new Point( leftX + form.CurrentWidth, leftY ) );
                case RectSide.Right:
                    return CalcMiddlePoint( new Point( leftX + form.CurrentWidth, leftY ),
                                     new Point( leftX + form.CurrentWidth,
                                                leftY + form.CurrentHeight ) );
                case RectSide.Bottom:
                    return CalcMiddlePoint(
                        new Point( leftX + form.CurrentWidth, leftY + form.CurrentHeight ),
                        new Point( leftX, leftY + form.CurrentHeight ) );
            } //switch

            return new Point();
        }

        /// <summary>
        ///   Gets the points connectors of the Entity Form.
        /// </summary>
        /// <param name = "point">The points</param>
        /// <returns>Created connectors.</returns>
        public RectConnector[] GetPointConnectors( Point point )
        {
            const double pointDistance = 2.0;

            RectConnector[] result = new RectConnector[4];

            result[0] = new RectConnector( RectSide.Left, new Point( point.X - pointDistance, point.Y ) );
            result[1] = new RectConnector( RectSide.Top, new Point( point.X, point.Y - pointDistance ) );
            result[2] = new RectConnector( RectSide.Right, new Point( point.X + pointDistance, point.Y ) );
            result[3] = new RectConnector( RectSide.Bottom, new Point( point.X, point.Y + pointDistance ) );

            return result;
        }

        /// <summary>
        ///   Returns all depended relation form.
        /// </summary>
        /// <param name = "entityFormId">The entity form.</param>
        /// <returns>The list of relation forms.</returns>
        private IEnumerable<IRelationForm> GetDependedRelationForms( Guid entityFormId )
        {
            return from registredRelation in _registredRelations
                   where registredRelation.Value.DataEntity.Relation.IsRelatedToForm( entityFormId )
                   select registredRelation.Value;
        }

        /// <summary>
        /// Updates position of all registerd relations.
        /// </summary>
        public void UpdatePositionOfAllRelations()
        {
            foreach ( var registredEntityForm in RegistredEntityForms ){
                DependentRelationsLayoutUpdate( registredEntityForm.DataEntity );
            } //foreach
        }

        #endregion Relation Forms handling

        #region View state 

        /// <summary>
        ///   The assotiated iteraction provider.
        /// </summary>
        private UserIteractionProvider _provider;

        /// <summary>
        ///   Sets the iteraction provider.
        /// </summary>
        /// <param name = "provider">The provider.</param>
        public void SetUserIteractionPropvider( UserIteractionProvider provider )
        {
            _provider = provider;

            var modelHeightBinding = new Binding( "Height" );
            modelHeightBinding.Source = ModelView.DataEntity;
            modelHeightBinding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding( _provider.ModelSize.Height, MeasureProxy.ValueProperty, modelHeightBinding );

            var modelWidthBinding = new Binding( "Width" );
            modelWidthBinding.Source = ModelView.DataEntity;
            modelWidthBinding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding( _provider.ModelSize.Width, MeasureProxy.ValueProperty, modelWidthBinding );
        }

        /// <summary>
        ///   Sets the fill brush for all selected entity forms.
        /// </summary>
        /// <param name = "brush"></param>
        public void SetBrushForSelectedEntityForms( Brush brush )
        {
            foreach ( IEntityForm selectedForm in SelectedEntityForms ){
                
                if (!_cashedBrushes.ContainsKey(selectedForm.DataEntity.Entity.InnerId)){
                    //save for new changes.
                    _cashedBrushes.Add( selectedForm.DataEntity.Entity.InnerId, selectedForm.DataEntity.BackgroundBrush );
                } //if

                selectedForm.DataEntity.BackgroundBrush = brush;
            } //foreach
        }

        /// <summary>
        /// Register change command for all cashed entity form backgroound changes.
        /// </summary>
        public void RegisterEntityFormsBackgroundBrushChange()
        {
            var command = new ChangeEntityFormBackgroundBrush();
            command.ModelViewManager = this;

            foreach ( var cashedBrush in _cashedBrushes ){
                command.NewBrushes.Add( cashedBrush.Key,_registerdEntities[cashedBrush.Key].EntityForm.DataEntity.BackgroundBrush );
                command.OldBrushes.Add( cashedBrush.Key, cashedBrush.Value );
            } //foreach

            command.Done = true;
            Controller.ExecCommand( command );
            _cashedBrushes.Clear();
        }

        /// <summary>
        /// The cashed brushes.
        /// It`s need for background brushes changes watching.
        /// </summary>
        private readonly Dictionary<Guid,Brush> _cashedBrushes = new Dictionary<Guid, Brush>();

        #endregion View state

        /// <summary>
        ///   The main modelView.
        /// </summary>
        public IModelView ModelView
        {
            get { return _modelView; }
        }
    }
}