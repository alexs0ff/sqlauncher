// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DataModel.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 09 05 10:42 AM
//   * Modified at: 2011  09 05  10:50 AM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using Microsoft.Practices.Unity;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents a data model of sqlite database schema.
    /// </summary>
    public class DataModel : BindableModelObject, IDeepClonable<DataModel>
    {
        /// <summary>
        ///   The ERD entities collection.
        /// </summary>
        private readonly ICollection<ERDEntity> _entities = new ObservableCollection<ERDEntity>();

        /// <summary>
        ///   The relations of entities.
        /// </summary>
        private readonly ICollection<EntityRelation> _relations = new ObservableCollection<EntityRelation>();

        public DataModel()
        {
            var relations = (INotifyCollectionChanged) _relations;
            relations.CollectionChanged += RelationsCollectionChanged;
        }

        /// <summary>
        /// Occurs when relation has been added or removed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void RelationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ( e.NewItems!=null ){

                foreach (EntityRelation newItem in e.NewItems){
                    var watcher = new EntityRelationWatcher( newItem );
                    _entityRelationWatchers.Add( newItem.InnerId, watcher );
                } //foreach

            } //if

            if (e.OldItems != null){
                foreach ( EntityRelation oldItem in e.OldItems ){
                    var watcher = _entityRelationWatchers[oldItem.InnerId];
                    watcher.Dispose();
                    _entityRelationWatchers.Remove( oldItem.InnerId );
                } //foreach
            }
        }

        /// <summary>
        ///   The caption of attribute.
        /// </summary>
        [Dependency]
        public ItemName Caption { get; set; }

        /// <summary>
        ///   The ERD entities collection.
        /// </summary>
        public ICollection<ERDEntity> Entities
        {
            get { return _entities; }
        }

        /// <summary>
        ///   The relations of entities.
        /// </summary>
        public ICollection<EntityRelation> Relations
        {
            get { return _relations; }
        }

        /// <summary>
        /// Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            Caption = CreateInstance<ItemName>();
        }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public DataModel Clone()
        {
            var copy = CreateInstance<DataModel>();
            copy.Caption = Caption.Clone();

            var copyOfEntities = Entities.ToDictionary( entity => entity.InnerId );
            var processedEntities = new Dictionary<Guid, ERDEntity>();
            var processedRelations = new Dictionary<Guid, EntityRelation>();

            foreach ( var watcher in _entityRelationWatchers ){

                if ( watcher.Value.EntityRelation.Child != null ){
                    //process child
                    CopyWatcher( copy, processedRelations, copyOfEntities, processedEntities,
                                 watcher.Value.EntityRelation);

                } //if Child != null

                //process parent
                if ( watcher.Value.EntityRelation.Parent != null ){
                    CopyWatcher( copy, processedRelations, copyOfEntities, processedEntities,
                                 watcher.Value.EntityRelation );
                } // if Parent != null

            } //foreach

            foreach ( var copyOfEntity in copyOfEntities ){
                processedEntities.Add( copyOfEntity.Value.InnerId, copyOfEntity.Value.Clone() );
            } //foreach

            //correction of entities position.
            foreach ( var entity in Entities ){
                copy.Entities.Add( processedEntities[entity.InnerId] );
            } //foreach

            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }

        /// <summary>
        /// Creates a copy of watcher.
        /// </summary>
        /// <param name="dataModelCopy"></param>
        /// <param name="processedRelations"></param>
        /// <param name="copyOfEntities"></param>
        /// <param name="processedEntities"></param>
        /// <param name="entityRelation"></param>
        private static void CopyWatcher(DataModel dataModelCopy, IDictionary<Guid, EntityRelation> processedRelations, IDictionary<Guid, ERDEntity> copyOfEntities,
            IDictionary<Guid, ERDEntity> processedEntities,
                                            EntityRelation entityRelation)
        {

            if ( !processedRelations.ContainsKey( entityRelation.InnerId ) ){

                var clonedChild = CloneErdEntity( copyOfEntities, processedEntities,
                                                  entityRelation.Child.InnerId );
                var clonedParent = CloneErdEntity(copyOfEntities, processedEntities,
                                                   entityRelation.Parent.InnerId );

                var copyRelation = entityRelation.Clone();
                copyRelation.Child = clonedChild;
                copyRelation.Parent = clonedParent;
                processedRelations.Add( entityRelation.InnerId, copyRelation );

                dataModelCopy.Relations.Add( copyRelation );
            } //if
        }

        /// <summary>
        /// Clones the erd entity.
        /// </summary>
        /// <param name="copyOfEntities"></param>
        /// <param name="processedEntities"></param>
        /// <param name="innerId"></param>
        private static ERDEntity CloneErdEntity( IDictionary<Guid, ERDEntity> copyOfEntities, IDictionary<Guid, ERDEntity> processedEntities,
                                            Guid innerId )
        {
            if ( !processedEntities.ContainsKey( innerId ) ){
                var entity = copyOfEntities[innerId].Clone();
                processedEntities.Add( innerId, entity );
                copyOfEntities.Remove( innerId );
                return entity;
            } //else

            return processedEntities[innerId];
        }

        #region Relation watchers 

        /// <summary>
        /// The entity relation watchers.
        /// </summary>
        private readonly IDictionary<Guid, EntityRelationWatcher> _entityRelationWatchers =
            new Dictionary<Guid, EntityRelationWatcher>();

        #endregion Relation watchers
    }
}