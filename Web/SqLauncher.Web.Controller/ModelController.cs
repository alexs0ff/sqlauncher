// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelController.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 01 9:10 PM
//   * Modified at: 2011  09 25  1:42 PM
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Windows.Controls;

using SqLauncher.Web.Controller.Commands;
using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;
using SqLauncher.Web.UI;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   Represents the main controller of application`s engine.
    /// </summary>
    public class ModelController:IViewController
    {
        /// <summary>
        ///   The model instances container.
        /// </summary>
        private readonly ContainerWiring _modelContainerWiring;

        /// <summary>
        ///   The model instances container.
        /// </summary>
        internal ContainerWiring ModelContainerWiring
        {
            get { return _modelContainerWiring; }
        }

        /// <summary>
        ///   Initializes the ModelController instance.
        /// </summary>
        /// <param name = "modelContainerWiring">The model container wiring.</param>
        public ModelController( ContainerWiring modelContainerWiring )
        {
            _modelContainerWiring = modelContainerWiring;
            _modelContainerWiring.ValueChanged += DataObjectValueChanged;

            _iteractionProvider = new UserIteractionProvider();
            _iteractionProvider.IteractionState = modelContainerWiring.CreateInstance<IIteractionState>();
            _iteractionProvider.ZoomAbility = true;
            _iteractionProvider.HasOpenDocuments = true;
            _iteractionProvider.ZoomPercentChanged += ZoomPercentChanged;
            _iteractionProvider.BrushChanged += BrushChanged;
        }

        /// <summary>
        /// Occurs when user changed the brush fo some forms.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BrushChanged(object sender, RibbonIteraction.BrushChangedEventArgs e)
        {
            ModelViewManager.SetBrushForSelectedEntityForms( e.Brush );
        }

        /// <summary>
        /// Occurs whe zoom percent changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void ZoomPercentChanged(object sender, System.EventArgs e)
        {
            ModelViewManager.ModelView.Zoom = _iteractionProvider.ZoomPercent;
        }

        /// <summary>
        ///   Occurs when some value changed of all objects that is intercept.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void DataObjectValueChanged( object sender, ValueChangedEventArgs e )
        {
            var command = new ValueChangeCommand{
                                                    Done = true,
                                                    NewValue = e.NewValue,
                                                    OldValue = e.OldValue,
                                                    PropertyInfo = e.PropertyInfo,
                                                    Target = e.Target,
                                                    Controller = this
                                                };
            ExecCommand( command );
        }

        #region Views 

        /// <summary>
        ///   The view manager.
        /// </summary>
        private ModelViewManager _modelViewManager;

        /// <summary>
        ///   The view manager.
        /// </summary>
        public ModelViewManager ModelViewManager
        {
            get { return _modelViewManager; }
            set
            {
                _modelViewManager = value;
                _modelViewManager.SetUserIteractionPropvider( _iteractionProvider );
            }
        }

        #endregion Views

        /// <summary>
        /// Canceles the current operation.
        /// </summary>
        public void CancelCurrentOperation()
        {
            ModelViewManager.StopCurrentOperation();
        }

        /// <summary>
        ///   Creates a new entity from and admit user to place it.
        /// </summary>
        public void CreateAndPlaceEntityForm()
        {
            var entityForm = new EntityForm();
            entityForm.DataEntity = _modelContainerWiring.CreateInstance<EntityViewState>();

            ModelViewManager.AssignPlace( entityForm );
        }

        /// <summary>
        /// Creates entity form by entity view state.
        /// </summary>
        /// <param name="entityViewState">The entity view state.</param>
        public void CreateEntityFormByViewState(IEntityViewState entityViewState)
        {
            var entityForm = new EntityForm();
            entityForm.DataEntity = entityViewState;
            AddEntityForm( entityForm );
        }

        /// <summary>
        ///   Adds the entity form.
        /// </summary>
        /// <param name = "form">The entity form.</param>
        public void AddEntityForm( IEntityForm form )
        {
            ModelViewManager.Add( form );
        }

        /// <summary>
        ///   Removes a entity form.
        /// </summary>
        /// <param name = "form">The entity from to remove.</param>
        public void RemoveEntityForm( IEntityForm form )
        {
            ModelViewManager.Remove( form );
        }

        /// <summary>
        ///   Removes a entity form.
        /// </summary>
        /// <param name = "entityViewState">The entity from view state to remove.</param>
        public void RemoveEntityForm(IEntityViewState entityViewState)
        {
            ModelViewManager.Remove( entityViewState );
        }

        /// <summary>
        ///  Creates a new relation from and admit user to place it.
        /// </summary>
        /// <param name="relationshipType">The type of created relationship</param>
        public void CreateAndPlaceRelationForm(RelationshipType relationshipType)
        {
            var relationForm = new RelationForm();

            relationForm.DataEntity = _modelContainerWiring.CreateInstance<IRelationViewState>();
            ModelContainerWiring.BeginSuspendValueChangeEvent();
            relationForm.DataEntity.Relation.Type = relationshipType;
            ModelContainerWiring.EndSuspendValueChangeEvent();
            ModelViewManager.AssignPlace( relationForm );
        }

        /// <summary>
        ///   Adds the relation form.
        /// </summary>
        /// <param name = "form">The relation form.</param>
        public void AddRelationForm( IRelationForm form )
        {
            ModelViewManager.Add( form );
        }

        /// <summary>
        /// Creates the relation form by view state.
        /// </summary>
        /// <param name="relationViewState">The relation form state.</param>
        public void CreateRelationFormByViewState(IRelationViewState relationViewState)
        {
            var relationForm = new RelationForm();
            relationForm.DataEntity = relationViewState;
            AddRelationForm( relationForm );
        }

        /// <summary>
        ///   Removes a relation form.
        /// </summary>
        /// <param name = "form">The relation from to remove.</param>
        public void RemoveRelationForm( IRelationForm form )
        {
            ModelViewManager.Remove( form );
        }

        /// <summary>
        ///   Removes a relation form by view state.
        /// </summary>
        /// <param name = "relationViewState">The relation from to remove.</param>
        public void RemoveRelationForm(IRelationViewState relationViewState)
        {
            ModelViewManager.Remove(relationViewState);
        }

        #region Commands 

        /// <summary>
        ///   Executed commands stack.
        /// </summary>
        private readonly Stack<ICommand> _commands = new Stack<ICommand>();

        /// <summary>
        ///   The redo command stack.
        /// </summary>
        private readonly Stack<ICommand> _redoCommands = new Stack<ICommand>();

        /// <summary>
        ///   Executes a command.
        /// </summary>
        /// <param name = "command">The command.</param>
        public void ExecCommand( ICommand command )
        {
            if ( !command.Done ){
                command.Do();
                command.Done = true;
            }
            _commands.Push( command );
            _iteractionProvider.CanUndo = true;
        }

        /// <summary>
        ///   Undoes the last command.
        /// </summary>
        public void Undo()
        {
            if ( _commands.Count > 0 ){
                var command = _commands.Pop();
                command.Undo();
                _redoCommands.Push( command );
                _iteractionProvider.CanRedo = true;

                if ( _commands.Count == 0 ){
                    _iteractionProvider.CanUndo = false;
                } //if
            }
        }

        /// <summary>
        ///   Redoes the undues commands.
        /// </summary>
        public void Redo()
        {
            if ( _redoCommands.Count > 0 ){
                var command = _redoCommands.Pop();
                command.Do();
                ExecCommand( command );

                if(_redoCommands.Count == 0){
                    _iteractionProvider.CanRedo = false;    
                }
            }
        }

        #endregion Commands

        /// <summary>
        ///   Returns a view for this controller.
        /// </summary>
        /// <returns>The view.</returns>
        public UserControl GetView()
        {
            return (UserControl) ModelViewManager.ModelView;
        }

        #region Iteraction provider 

        private readonly UserIteractionProvider _iteractionProvider;

        /// <summary>
        /// The user iteraction provider.
        /// </summary>
        public UserIteractionProvider IteractionProvider
        {
            get { return _iteractionProvider; }
        }

        /// <summary>
        ///  Register change command for all cashed entity form background changes.
        /// </summary>
        public void RegisterEntityFormsBackgroundBrushChange()
        {
            ModelViewManager.RegisterEntityFormsBackgroundBrushChange();
        }

        #endregion Iteraction provider

        #region sql generation

        /// <summary>
        /// The
        /// </summary>
        private SqlGenerationFormManager _sqlGenerationFormManager;

        /// <summary>
        /// Starts ddl generation.
        /// </summary>
        public void StartGeneratingSql()
        {
            if ( _sqlGenerationFormManager != null ){
                _sqlGenerationFormManager.Close();
            }

            _sqlGenerationFormManager =
                new SqlGenerationFormManager(
                    new SqlGenerationForm
                    {DataEntity = _modelContainerWiring.CreateInstance<ISqlGenerationFormViewState>()},
                    _modelContainerWiring.CreateInstance<DataModelGeneratorBase>(),
                    ModelViewManager.ModelView.DataEntity.DataModel );
            _sqlGenerationFormManager.Start();
        }

        #endregion

        /// <summary>
        /// Copies into clipboard selected items.
        /// </summary>
        public void ClipboardCopy()
        {
            ModelViewManager.CopySelectedItems();
        }

        /// <summary>
        /// Instert items from clipboard.
        /// </summary>
        public void ClipboardPaste()
        {
            ModelViewManager.PasteItemsFromClipboard();
        }

        /// <summary>
        /// Cuts selected items to clipboard.
        /// </summary>
        public void ClipboardCut()
        {
            ModelViewManager.CutItemToClipboard();
        }

        /// <summary>
        /// Duplicates selected items.
        /// </summary>
        public void DuplicateSelectedItems()
        {
            ModelViewManager.DublicateSelectedItems();
        }

        /// <summary>
        /// Removes selected items.
        /// </summary>
        public void RemoveSelectedItems()
        {
            ModelViewManager.RemoveSelectedItems();
        }

        /// <summary>
        /// The method invoke when current view controller is selected for viewing.
        /// </summary>
        public void OnShowing()
        {
            //TODO: make lasy
            ModelViewManager.UpdatePositionOfAllRelations();
        }
    }
}