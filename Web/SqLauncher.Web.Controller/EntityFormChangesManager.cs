// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFormChangesManager.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 05 7:02 PM
//   * Modified at: 2011  09 26  9:51 PM
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Controller.Commands;
using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The manager of all Entity form changes and creating a command of and/redo handling.
    /// </summary>
    public class EntityFormChangesManager : IDisposable
    {
        /// <summary>
        ///   The handled entity form.
        /// </summary>
        private IEntityForm _entityForm;

        /// <summary>
        ///   The model controller.
        /// </summary>
        private readonly ModelController _modelController;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.EntityFormChangesManager" /> class.
        /// </summary>
        public EntityFormChangesManager( IEntityForm entityForm, ModelController modelController )
        {
            _entityForm = entityForm;
            _modelController = modelController;
            _entityForm.PositionChanged += PositionChanged;
            _entityForm.AddEntityAttribute += AddEntityAttribute;
            _entityForm.DeleteEntityAttribute += DeleteEntityAttribute;
            _entityForm.EntitySizeChanged += EntitySizeChanged;

            _entityForm.AddEntityIndex += AddEntityIndex;
            _entityForm.DeleteEntityIndex += DeleteEntityIndex;

            _entityForm.AddIndexAttribute += AddIndexAttribute;
            _entityForm.DeleteIndexAttribute += DeleteIndexAttribute;
            _entityForm.EntityAttributeReordering += EntityAttributeReordering;
        }

        /// <summary>
        /// Occurs when user want to reorder item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void EntityAttributeReordering(object sender, EntityAttributeReorderingEventArgs e)
        {
            var command = new ReorderEntityAttribute();
            command.OldIndex = e.CurrentIndex;
            command.NewIndex = e.NewIndex;
            command.Entity = _entityForm.DataEntity.Entity;
            _modelController.ExecCommand( command );
        }
        

        /// <summary>
        /// Occurs when user wish to delete an index attribute.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void DeleteIndexAttribute(object sender, DeleteIndexAttributeEventArgs e)
        {
            var command = new DeleteIndexAttribute();
            command.EntityIndex = e.EntityIndex;
            command.IndexAttribute = e.IndexAttribute;
            _modelController.ExecCommand(command);
        }

        /// <summary>
        /// Occurs when user wish to add an index attribute.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void AddIndexAttribute(object sender, AddIndexAttributeEventArgs e)
        {
            var command = new AddNewIndexAttribute();
            command.EntityAttribute = e.EntityAttribute;
            command.EntityIndex = e.EntityIndex;
            command.IndexAttribute = _modelController.ModelContainerWiring.CreateInstance<IndexAttribute>();
            _modelController.ExecCommand(command);
        }

        /// <summary>
        /// Occurs when user wish to delete an antity index.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void DeleteEntityIndex(object sender, DeleteEntityIndexEventArgs e)
        {
            var command = new DeleteEntityIndex();
            command.ERDEntity = _entityForm.DataEntity.Entity;
            command.Index = e.Index;
            _modelController.ExecCommand(command);
        }

        /// <summary>
        /// Occurs whe user wish to add an entity attribute.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void AddEntityIndex(object sender, AddEntityIndexEventArgs e)
        {
            var command = new AddNewEntityIndex();
            command.ERDEntity = _entityForm.DataEntity.Entity;
            command.Index = _modelController.ModelContainerWiring.CreateInstance<EntityIndex>();
            _modelController.ExecCommand(command);
        }

        /// <summary>
        ///   Occurs when entity form size changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void EntitySizeChanged( object sender, EntitySizeChangedEventArgs e )
        {
            var command = new ERDEntitySizeChange();
            command.Done = true;
            command.EntityFormId = _entityForm.DataEntity.Entity.InnerId;
            command.NewSize = e.NewSize;
            command.OldSize = e.OldSize;
            command.ViewManager = _modelController.ModelViewManager;
            _modelController.ExecCommand( command );
        }

        /// <summary>
        ///   Occurs when we need to remove an entity attribute.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void DeleteEntityAttribute( object sender, DeleteEntityAttributeEventArgs e )
        {
            var command = new DeleteEntityAttribute();
            command.ERDEntity = _entityForm.DataEntity.Entity;
            command.EntityAttribute = e.EntityAttribute;
            _modelController.ExecCommand( command );
        }

        /// <summary>
        ///   Occurs when we need to add an entity attribute.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void AddEntityAttribute( object sender, AddEntityAttributeEventArgs e )
        {
            var command = new AddNewEntityAttribute();
            command.ERDEntity = _entityForm.DataEntity.Entity;
            command.EntityAttribute = _modelController.ModelContainerWiring.CreateInstance<EntityAttribute>();
            _modelController.ExecCommand( command );
        }

        /// <summary>
        ///   Occurs when position of entity form has been changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void PositionChanged( object sender, PositionChangedEventArgs e )
        {
            var command = new EntityFormChangePosition();
            command.Done = true;
            command.ViewManager = _modelController.ModelViewManager;
            command.EntityFormId = ((IEntityForm) sender).DataEntity.Entity.InnerId;
            command.NewPosition = e.NewPosition;
            command.OldPosition = e.OldPosition;
            _modelController.ExecCommand( command );
        }

        #region Implementation of IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _entityForm.PositionChanged -= PositionChanged;
            _entityForm.AddEntityAttribute -= AddEntityAttribute;
            _entityForm.DeleteEntityAttribute -= DeleteEntityAttribute;
            _entityForm.EntitySizeChanged -= EntitySizeChanged;

            _entityForm.AddEntityIndex -= AddEntityIndex;
            _entityForm.DeleteEntityIndex -= DeleteEntityIndex;
            _entityForm.AddIndexAttribute -= AddIndexAttribute;
            _entityForm.DeleteIndexAttribute -= DeleteIndexAttribute;
            _entityForm.EntityAttributeReordering -= EntityAttributeReordering;
        }

        #endregion
    }
}