// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityRelationWatcher.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 12 12 21:55
//   * Modified at: 2011  12 12  22:11
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Linq;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The watcher for assotiated erd entities keys.
    /// </summary>
    public class EntityRelationWatcher
    {
        /// <summary>
        /// The name of relation type property.
        /// </summary>
        private const string RelationTypePropertyName = "Type";

        /// <summary>
        ///   The entity relation.
        /// </summary>
        private readonly EntityRelation _entityRelation;

        /// <summary>
        ///   The entity relation.
        /// </summary>
        public EntityRelation EntityRelation
        {
            get { return _entityRelation; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.EntityRelationWatcher" /> class.
        /// </summary>
        public EntityRelationWatcher( EntityRelation entityRelation )
        {
            _entityRelation = entityRelation;

            if ( entityRelation == null ){
                throw new ArgumentNullException("entityRelation", "entityRelation must be set");
            } //if

            if ( entityRelation.Child!=null && entityRelation.Parent!=null ){
                StartWatch();
            } //if
            else{
                var setChildAndParent =
                    Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        h => entityRelation.PropertyChanged += h, h => entityRelation.PropertyChanged -= h ).Where(
                            args => ( (EntityRelation)args.Sender ).Child !=null &&
                                    ( (EntityRelation)args.Sender ).Parent !=null );

                _setChildAndParentSubscription =
                    setChildAndParent.Subscribe( args =>{
                                              StartWatch();
                                              _setChildAndParentSubscription.Dispose();
                                              _setChildAndParentSubscription = null;
                                          } );
            } //else

            var updateRelationType = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => entityRelation.PropertyChanged += h, h => entityRelation.PropertyChanged -= h
                ).Where( args => args.EventArgs.PropertyName == RelationTypePropertyName );

            _updateRelationTypeSubscription = updateRelationType.Subscribe( args => UpdateChildForeignKeys() );
        }

        /// <summary>
        /// The subscription for set child and parent propeties.
        /// </summary>
        private IDisposable _setChildAndParentSubscription;

        /// <summary>
        /// The subscription of update the relation type property.
        /// </summary>
        private IDisposable _updateRelationTypeSubscription;

        /// <summary>
        /// Starts the watching for the relation.
        /// </summary>
        private void StartWatch()
        {
            _entityRelation.Child.Relations.Add(_entityRelation);
            _entityRelation.Parent.Relations.Add(_entityRelation);

            AddParentAttributesToWatch();

            var parentCollection = (INotifyCollectionChanged)_entityRelation.Parent.Attributes;
            parentCollection.CollectionChanged += ParentCollectionChanged;
        }

        /// <summary>
        /// Adds all parent keyed attributes to watch.
        /// </summary>
        private void AddParentAttributesToWatch()
        {
            foreach ( var entityAttribute in _entityRelation.Parent.Attributes ){
                entityAttribute.PropertyChanged -= ParentERDEntityAttributeChanged;
                entityAttribute.PropertyChanged += ParentERDEntityAttributeChanged;

                if ( entityAttribute.Key == AttributeKeyType.IsKey ){
                    ProcessNewParentKey( entityAttribute );
                } //if
            } //foreach
        }

        /// <summary>
        ///   Realeses all handled resources and attributes.
        /// </summary>
        public void Dispose()
        {
            if (_setChildAndParentSubscription!=null)
            {
                _setChildAndParentSubscription.Dispose();
                _setChildAndParentSubscription = null;
            } //if

            if ( _updateRelationTypeSubscription!=null ){
                _updateRelationTypeSubscription.Dispose();
                _updateRelationTypeSubscription = null;
            } //if

            var parentCollection = (INotifyCollectionChanged) _entityRelation.Parent.Attributes;
            parentCollection.CollectionChanged -= ParentCollectionChanged;

            RemoveParentAttributesFromWatch();

            _entityRelation.Child.Relations.Remove( _entityRelation );
            _entityRelation.Parent.Relations.Remove(_entityRelation);
        }

        private void RemoveParentAttributesFromWatch()
        {
            foreach ( var entityAttribute in _entityRelation.Parent.Attributes ){
                entityAttribute.PropertyChanged -= ParentERDEntityAttributeChanged;
                entityAttribute.PropertyChanged -= PrimaryKeyAttributePropertyChanged;
            } //foreach _entityRelation.Parent.Attributes

            foreach ( var keyValuePair in _childForeignKeysMap ){
                _entityRelation.ParentAttributes.Remove( keyValuePair.Key );
                _entityRelation.ChildAttributes.Remove( keyValuePair.Value );
                _entityRelation.Child.Attributes.Remove( keyValuePair.Value );
            } //foreach _childForeignKeysMap

            _childForeignKeysMap.Clear();
        }

        /// <summary>
        ///   Occurs when parent collection changed.
        /// </summary>
        private void ParentCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if ( e.NewItems != null ){
                foreach ( EntityAttribute newItem in e.NewItems ){
                    newItem.PropertyChanged -= ParentERDEntityAttributeChanged;
                    newItem.PropertyChanged += ParentERDEntityAttributeChanged;

                    if ( newItem.Key == AttributeKeyType.IsKey ){
                        ProcessNewParentKey( newItem );
                    } //if
                } //foreach    
            } //if

            if ( e.OldItems != null ){
                foreach ( EntityAttribute oldItem in e.OldItems ){
                    oldItem.PropertyChanged -= ParentERDEntityAttributeChanged;
                    ProcessRemoveParentKey( oldItem );
                } //foreach    
            } //if
        }

        /// <summary>
        ///   The key property name.
        /// </summary>
        private const string KeyProperyName = "Key";

        /// <summary>
        ///   Occurs when a parent entity attribute has been changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void ParentERDEntityAttributeChanged( object sender, PropertyChangedEventArgs e )
        {
            //we have to listening for each an Attribute of erd entity.

            if ( e.PropertyName == KeyProperyName ){
                var primaryKeyAttribute = (EntityAttribute) sender;

                //TODO: add only IsKey or IsPromaryForeignKey too?
                if ( primaryKeyAttribute.Key == AttributeKeyType.IsKey ){
                    ProcessNewParentKey( primaryKeyAttribute );
                } //if
            } //if
        }

        /// <summary>
        ///   Occurs when a parent primary key attribute has been changed.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void PrimaryKeyAttributePropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            var primaryKeyAttribute = (EntityAttribute) sender;

            if ( e.PropertyName == KeyProperyName ){
                if ( primaryKeyAttribute.Key != AttributeKeyType.IsKey ){
                    ProcessRemoveParentKey( primaryKeyAttribute );
                } //if
            } //if
            else{
                var foreignKey = GetRelatedForeignKey( primaryKeyAttribute );

                PrimaryKeyFieldsToForeignKey( foreignKey, primaryKeyAttribute );
            } //else
        }

        #region Primary key Foreign key assotiated list 

        /// <summary>
        /// Updates for foreign type for all keys within child entity.
        /// </summary>
        private void UpdateChildForeignKeys()
        {
            var foreignKeyType = GetForeignKeyType();

            //last time was AttributeKeyType.None
            if (_entityRelation.Parent!=null && foreignKeyType != AttributeKeyType.None && _childForeignKeysMap.Count == 0 ){
                //try to add PK
                AddParentAttributesToWatch();
            } //if
            else if (_entityRelation.Parent != null && foreignKeyType == AttributeKeyType.None && _childForeignKeysMap.Count != 0)
            {
                //remove PK
                RemoveParentAttributesFromWatch();

            } //else
            else{

                foreach ( var keyValuePair in _childForeignKeysMap ){
                    if ( keyValuePair.Value.Wiring != null ){
                        keyValuePair.Value.Wiring.BeginSuspendValueChangeEvent();
                    } //if

                    keyValuePair.Value.Key = foreignKeyType;

                    if ( keyValuePair.Value.Wiring != null ){
                        keyValuePair.Value.Wiring.EndSuspendValueChangeEvent();
                    } //if
                } //foreach
            } //else
        }

        /// <summary>
        ///   Processes removing a parent primary key.
        /// </summary>
        /// <param name = "primaryKeyAttribute">The primary key.</param>
        private void ProcessRemoveParentKey( EntityAttribute primaryKeyAttribute )
        {
            primaryKeyAttribute.PropertyChanged -= PrimaryKeyAttributePropertyChanged;
            primaryKeyAttribute.PropertyChanged += ParentERDEntityAttributeChanged;

            EntityAttribute foreignKey = null;

            for ( int index = 0; index < _childForeignKeysMap.Count; index++ ){
                var keyValuePair = _childForeignKeysMap[index];
                if ( ReferenceEquals( keyValuePair.Key, primaryKeyAttribute ) ){
                    foreignKey = keyValuePair.Value;
                    _childForeignKeysMap.RemoveAt( index );
                    break;
                } //if
            }

            if ( foreignKey != null ){
                _entityRelation.Child.Attributes.Remove( foreignKey );
                _entityRelation.ChildAttributes.Remove( foreignKey );
            } //if

            _entityRelation.ParentAttributes.Remove( primaryKeyAttribute );
        }

        /// <summary>
        ///   Processes a new primary key from parent erd entity.
        /// </summary>
        /// <param name = "primaryKeyAttribute">The parent primary key.</param>
        private void ProcessNewParentKey(EntityAttribute primaryKeyAttribute)
        {
            var attributeKeyType = GetForeignKeyType();
            //TODO check primaryKeyAttribute already in _childForeignKeysMap
            if ( attributeKeyType != AttributeKeyType.None ){
                //Suspends all notify property changet events.
                if ( primaryKeyAttribute.Wiring != null ){
                    primaryKeyAttribute.Wiring.BeginSuspendValueChangeEvent();
                } //if

                primaryKeyAttribute.PropertyChanged -= ParentERDEntityAttributeChanged;
                primaryKeyAttribute.PropertyChanged += PrimaryKeyAttributePropertyChanged;

                var foreignKey = primaryKeyAttribute.Clone();
                foreignKey.Key = attributeKeyType;

                PrimaryKeyFieldsToForeignKey( foreignKey, primaryKeyAttribute );

                _childForeignKeysMap.Add( new KeyValuePair<EntityAttribute, EntityAttribute>( primaryKeyAttribute,
                                                                                              foreignKey ) );

                _entityRelation.Child.Attributes.Add( foreignKey );
                _entityRelation.ChildAttributes.Add( foreignKey );

                _entityRelation.ParentAttributes.Add( primaryKeyAttribute );

                if ( primaryKeyAttribute.Wiring != null ){
                    primaryKeyAttribute.Wiring.EndSuspendValueChangeEvent();
                } //if
            } //if
        }

        /// <summary>
        /// Gets the current type for added foreign key attribute.
        /// </summary>
        /// <returns>The curent foreign key type.</returns>
        private AttributeKeyType GetForeignKeyType()
        {
            if ( _entityRelation.Type == RelationshipType.Identifying ){
                return AttributeKeyType.IsPrimaryForeignKey;
            } //if
            
            if(_entityRelation.Type == RelationshipType.NonIdentifying){
                return AttributeKeyType.IsForeignKey;
            } //if
            return AttributeKeyType.None;
        }

        /// <summary>
        ///   Exchanges value of field between foreignKey and primaryKeyAttribute.
        /// </summary>
        /// <param name = "foreignKey"></param>
        /// <param name = "primaryKeyAttribute"></param>
        private static void PrimaryKeyFieldsToForeignKey( EntityAttribute foreignKey,
                                                          EntityAttribute primaryKeyAttribute )
        {
            if ( !Equals( foreignKey.Caption.Physical, primaryKeyAttribute.Caption.Physical ) ){
                foreignKey.Caption.Physical = primaryKeyAttribute.Caption.Physical;
            } //if

            if ( !Equals( foreignKey.Caption.Title, primaryKeyAttribute.Caption.Title ) ){
                foreignKey.Caption.Title = primaryKeyAttribute.Caption.Title;
            } //if

            if ( !Equals( foreignKey.DataLenght, primaryKeyAttribute.DataLenght ) ){
                foreignKey.DataLenght = primaryKeyAttribute.DataLenght;
            } //if

            if ( !Equals( foreignKey.DbType, primaryKeyAttribute.DbType ) ){
                foreignKey.DbType = primaryKeyAttribute.DbType;
            } //if
        }

        /// <summary>
        ///   The associated list.
        ///   The key - primary key.
        ///   The value - foreign key.
        /// </summary>
        private readonly IList<KeyValuePair<EntityAttribute, EntityAttribute>> _childForeignKeysMap =
            new List<KeyValuePair<EntityAttribute, EntityAttribute>>();

        /// <summary>
        ///   Gets the associated foreign key by instance primary key.
        /// </summary>
        /// <param name = "primaryKey">The primaty key.</param>
        /// <returns>The found attribute or null.</returns>
        private EntityAttribute GetRelatedForeignKey( EntityAttribute primaryKey )
        {
            EntityAttribute result = null;

            foreach ( var keyValuePair in _childForeignKeysMap ){
                if ( ReferenceEquals( keyValuePair.Key, primaryKey ) ){
                    result = keyValuePair.Value;
                } //if
            } //foreach

            return result;
        }

        #endregion Primary key Foreign key assotiated list
    }
}