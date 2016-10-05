// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AddNewEntityRelation.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 11 2:20 PM
//   * Modified at: 2011  09 25  1:22 PM
// / ******************************************************************************/ 

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Adds a new relation form.
    /// </summary>
    public class AddNewEntityRelation : ICommand
    {
        /// <summary>
        ///   The data model.
        /// </summary>
        public DataModel DataModel { get; set; }

        /// <summary>
        ///   The parent entity form.
        /// </summary>
        public IEntityForm ParentEntityForm { get; set; }

        /// <summary>
        ///   The child entity form.
        /// </summary>
        public IEntityForm ChildEntityForm { get; set; }

        /// <summary>
        ///   The added relation form.
        /// </summary>
        public IRelationViewState RelationViewState { get; set; }

        /// <summary>
        ///   The entity relation.
        /// </summary>
        public EntityRelation EntityRelation { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            EntityRelation.Child = ChildEntityForm.DataEntity.Entity;
            EntityRelation.Parent = ParentEntityForm.DataEntity.Entity;
            DataModel.Relations.Add( EntityRelation );
            RelationViewState.Relation = EntityRelation;
            Controller.CreateRelationFormByViewState( RelationViewState );
            Controller.ModelViewManager.RelationLayoutUpdate( RelationViewState );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            Controller.RemoveRelationForm( RelationViewState );
            EntityRelation.Child = null;
            EntityRelation.Parent = null;
            DataModel.Relations.Remove( EntityRelation );
            RelationViewState.Relation = null;
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        ///   The current model controller.
        /// </summary>
        public ModelController Controller { get; set; }
    }
}