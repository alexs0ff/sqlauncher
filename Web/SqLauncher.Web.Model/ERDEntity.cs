// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ERDEntity.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 12 03 22:18
//   * Modified at: 2012  02 24  20:25
// / ******************************************************************************/ 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The erd entity.
    /// </summary>
    public class ERDEntity : BindableModelObject, ICollection<EntityAttribute>, IDeepClonable<ERDEntity>
    {
        public ERDEntity()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Attributes = new ObservableCollection<EntityAttribute>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Indexes = new ObservableCollection<EntityIndex>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Relations = new ObservableCollection<EntityRelation>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        ///   The name of a entity.
        /// </summary>
        private ItemName _caption;

        /// <summary>
        ///   The name of a entity.
        /// </summary>
        [Dependency]
        [SelfPropertyChangedAttribute]
        public virtual ItemName Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        /// <summary>
        ///   The flag which indicates that by the entity generating DDL.
        /// </summary>
        [NotifyPropertyChanged]
        [SelfPropertyChangedAttribute]
        public virtual bool Generated { get; set; }

        /// <summary>
        /// Gets or sets the notes of current entity.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Notes { get; set; }

        /// <summary>
        ///   The collection of the entity attributes.
        /// </summary>
        [SelfPropertyChangedAttribute]
        public virtual ICollection<EntityAttribute> Attributes { get; set; }

        /// <summary>
        ///   The assotiated relations.
        /// </summary>
        private ICollection<EntityRelation> _relations = new ObservableCollection<EntityRelation>();

        /// <summary>
        ///   The assotiated relations.
        /// </summary>
        [SelfPropertyChangedAttribute]
        public virtual ICollection<EntityRelation> Relations
        {
            get { return _relations; }
            set { _relations = value; }
        }

        /// <summary>
        ///   The assotiated child relations.
        /// </summary>
        /// <returns>The child relations</returns>
        public IEnumerable<EntityRelation> ChildRelations
        {
            get
            {
                return from entityRelation in Relations
                       where entityRelation.Child != null && entityRelation.Child.InnerId == InnerId
                       select entityRelation;
            }
        }

        /// <summary>
        ///   The assotiated parent relations.
        /// </summary>
        /// <returns>The parent relations</returns>
        public IEnumerable<EntityRelation> ParentRelations
        {
            get
            {
                return from entityRelation in Relations
                       where entityRelation.Parent != null && entityRelation.Parent.InnerId == InnerId
                       select entityRelation;
            }
        }

        /// <summary>
        ///   Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            Caption = CreateInstance<ItemName>();
            Attributes = new ObservableCollection<EntityAttribute>();
            Indexes = new ObservableCollection<EntityIndex>();
            Relations = new ObservableCollection<EntityRelation>();
        }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public ERDEntity Clone()
        {
            var copy = CreateInstance<ERDEntity>();
            copy.Caption = Caption.Clone();
            copy.Generated = Generated;
            copy.Notes = Notes;

            foreach (
                var entityAttribute in
                    Attributes.Where(entityAttribute => entityAttribute.Key != AttributeKeyType.IsForeignKey && entityAttribute.Key != AttributeKeyType.IsPrimaryForeignKey))
            {
                copy.Attributes.Add( entityAttribute.Clone() );
            }

            foreach ( var entityIndex in Indexes ){
                var indexCopy = entityIndex.Clone();

                indexCopy.Parent = copy;

                foreach ( var indexAttribute in entityIndex.Attributes ){
                    var attributeCopy = indexAttribute.Clone();
                    attributeCopy.Attribute =
                        copy.First( attribute => attribute.ClonedBy == indexAttribute.Attribute.InnerId );
                    indexCopy.Attributes.Add( attributeCopy );
                } //foreach

                copy.Indexes.Add( indexCopy );
            } //foreach

            return copy;
        }

        /// <summary>
        ///   The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }

        #region Indexes
 
        [SelfPropertyChangedAttribute]
        public virtual ICollection<EntityIndex> Indexes { get; set; }

        #endregion Indexes

        #region Indexator

        /// <summary>
        ///   The indexator of entity attributes.
        /// </summary>
        /// <param name = "name"></param>
        /// <returns></returns>
        public EntityAttribute this[ string name ]
        {
            get
            {
                return
                    FirstOrDefault( name );
            }
            set
            {
                var attribute = FirstOrDefault( name );
                if ( attribute != null ){
                    Attributes.Remove( attribute );
                }
                Attributes.Add( value );
            }
        }

        private EntityAttribute FirstOrDefault( string name )
        {
            return Attributes.FirstOrDefault(
                attribute => attribute.Caption.Title != null && attribute.Caption.Title.OrdinalIgnoreCaseEqual( name ) );
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<EntityAttribute> GetEnumerator()
        {
            return Attributes.GetEnumerator();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///   An <see cref = "T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<EntityAttribute>

        /// <summary>
        ///   Adds an item to the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name = "item">The object to add to the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        public void Add( EntityAttribute item )
        {
            Attributes.Add( item );
        }

        /// <summary>
        ///   Removes all items from the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only. </exception>
        public void Clear()
        {
            Attributes.Clear();
        }

        /// <summary>
        ///   Determines whether the <see cref = "T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <returns>
        ///   true if <paramref name = "item" /> is found in the <see cref = "T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        /// <param name = "item">The object to locate in the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        public bool Contains( EntityAttribute item )
        {
            return Attributes.Contains( item );
        }

        /// <summary>
        ///   Copies the elements of the <see cref = "T:System.Collections.Generic.ICollection`1" /> to an <see
        ///    cref = "T:System.Array" />, starting at a particular <see cref = "T:System.Array" /> index.
        /// </summary>
        /// <param name = "array">The one-dimensional <see cref = "T:System.Array" /> that is the destination of the elements copied from <see
        ///    cref = "T:System.Collections.Generic.ICollection`1" />. The <see cref = "T:System.Array" /> must have zero-based indexing.</param>
        /// <param name = "arrayIndex">The zero-based index in <paramref name = "array" /> at which copying begins.</param>
        /// <exception cref = "T:System.ArgumentNullException"><paramref name = "array" /> is null.</exception>
        /// <exception cref = "T:System.ArgumentOutOfRangeException"><paramref name = "arrayIndex" /> is less than 0.</exception>
        /// <exception cref = "T:System.ArgumentException"><paramref name = "array" /> is multidimensional.-or-The number of elements in the source <see
        ///    cref = "T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref
        ///    name = "arrayIndex" /> to the end of the destination <paramref name = "array" />.-or-Type <paramref name = "T" /> cannot be cast automatically to the type of the destination <paramref
        ///    name = "array" />.</exception>
        public void CopyTo( EntityAttribute[] array, int arrayIndex )
        {
            Attributes.CopyTo( array, arrayIndex );
        }

        /// <summary>
        ///   Removes the first occurrence of a specific object from the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///   true if <paramref name = "item" /> was successfully removed from the <see
        ///    cref = "T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref
        ///    name = "item" /> is not found in the original <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <param name = "item">The object to remove from the <see cref = "T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref = "T:System.NotSupportedException">The <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        public bool Remove( EntityAttribute item )
        {
            return Attributes.Remove( item );
        }

        /// <summary>
        ///   Gets the number of elements contained in the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///   The number of elements contained in the <see cref = "T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public int Count
        {
            get { return Attributes.Count; }
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>
        ///   true if the <see cref = "T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return Attributes.IsReadOnly; }
        }

        #endregion
    }
}