// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 08 19:27
//   * Modified at: 2011  11 01  22:44
// / ******************************************************************************/ 

using System;
using System.Linq;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   Represents the view propeties for displaing the model view.
    /// </summary>
    public class ModelViewState : BindableModelObject, IModelViewState
    {
        public ModelViewState()
        {
            SetDefaults();
            EntityViewStates = new EntityViewStateCollection();
            EntityRelationStates = new RelationViewStateCollection();
        }

        private DataModel _dataModel;

        /// <summary>
        ///   Gets or sets the handled data model.
        /// </summary>
        [Dependency]
        public DataModel DataModel
        {
            get { return _dataModel; }
            set
            {
                _dataModel = value;
                EntityRelationStates.DataModel = value;
                EntityViewStates.DataModel = value;
            }
        }

        /// <summary>
        ///   The height of the model view.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual double Height { get; set; }

        /// <summary>
        ///   The width of the model view.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual double Width { get; set; }

        /// <summary>
        ///   Sets the default values for visual state.
        /// </summary>
        public void SetDefaults()
        {
            Height = DefaultHeight;
            Width = DefaultWidth;
        }

        /// <summary>
        ///   The default width of layout root.
        /// </summary>
        public const int DefaultWidth = 1000;

        /// <summary>
        ///   The default height of layoutRoot.
        /// </summary>
        public const int DefaultHeight = 900;

        /// <summary>
        ///   Gets entity view states.
        /// </summary>
        public EntityViewStateCollection EntityViewStates { get; private set; }

        /// <summary>
        ///   Gets the relation view states.
        /// </summary>
        public RelationViewStateCollection EntityRelationStates { get; private set; }

        /// <summary>
        /// Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            DataModel = CreateInstance<DataModel>();
        }

        /// <summary>
        ///   Clones the ModelViewState.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IModelViewState Clone()
        {
            var copy = (ModelViewState) CreateInstance<IModelViewState>();

            copy.Height = Height;
            copy.Width = Width;
            copy.DataModel = DataModel.Clone();

            foreach ( var entityRelation in DataModel.Relations ){
                var relationViewStateCopy = EntityRelationStates[entityRelation.InnerId].Clone();
                relationViewStateCopy.Relation =
                    copy.DataModel.Relations.FirstOrDefault( rel => rel.ClonedBy == entityRelation.InnerId );
                copy.EntityRelationStates.Add( relationViewStateCopy );
            } //foreach

            foreach ( var entity in DataModel.Entities ){
                var entityViewState = EntityViewStates[entity.InnerId].Clone();
                entityViewState.Entity =
                    copy.DataModel.Entities.FirstOrDefault( ent => ent.ClonedBy == entity.InnerId );
                copy.EntityViewStates.Add( entityViewState );
            } //foreach

            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}