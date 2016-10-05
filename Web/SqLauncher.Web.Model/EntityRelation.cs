// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityRelation.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 30 11:12 PM
//   * Modified at: 2011  09 25  3:08 PM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents a descriptor of a relation between tho entities.
    /// </summary>
    public class EntityRelation : BindableModelObject, IDeepClonable<EntityRelation>
    {
        /// <summary>
        ///   The name of relation.
        /// </summary>
        [Dependency]
        [SelfPropertyChangedAttribute]
        public virtual ItemName Caption { get; set; }

        #region Parent Erd entity 

        private ERDEntity _parent;

        /// <summary>
        ///   The parent Entity.
        /// </summary>
        [NotifyPropertyChanged(RiseValueChanged = false)]
        public virtual ERDEntity Parent
        {
            get { return _parent; }
            set { _parent =value ; }
        }

        /// <summary>
        /// Th parent attributes.
        /// </summary>
        private readonly ICollection<EntityAttribute> _parentAttributes = new ObservableCollection<EntityAttribute>();

        /// <summary>
        /// The parent attributes.
        /// </summary>
        public ICollection<EntityAttribute> ParentAttributes
        {
            get { return _parentAttributes; }
        }

        #endregion Parent Erd entity

        /// <summary>
        /// The child attributes.
        /// </summary>
        private readonly ICollection<EntityAttribute> _childAttributes = new ObservableCollection<EntityAttribute>();

        /// <summary>
        /// The child attributes.
        /// </summary>
        public ICollection<EntityAttribute> ChildAttributes
        {
            get { return _childAttributes; }
        }

        /// <summary>
        ///   The child entity.
        /// </summary>
        [NotifyPropertyChanged(RiseValueChanged = false)]
        public virtual ERDEntity Child { get; set; }

        /// <summary>
        ///   The Cardinality of relation.
        /// </summary>
        private Cardinality _cardinality;

        /// <summary>
        ///   The Cardinality of relation.
        /// </summary>
        [Dependency]
        [SelfPropertyChangedAttribute]
        public Cardinality Cardinality
        {
            get { return _cardinality; }
            set
            {
                if ( _cardinality!=null ){
                    _cardinality.PropertyChanged -= CardinalityPropertyChanged;
                } //if

                _cardinality = value;
                _cardinality.PropertyChanged += CardinalityPropertyChanged;
            }
        }

        /// <summary>
        /// Occurs when any of cardinality properties has been changed.
        /// We should invoks the PropertyChanged fo all change events of cardinality.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void CardinalityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RisePropertyChanged( "Cardinality" );
        }

        /// <summary>
        /// The type of relationship.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual RelationshipType Type { get; set; }

        /// <summary>
        ///   Determines whether a entity dependent on this ration.
        /// </summary>
        /// <param name = "formId">The form id.</param>
        /// <returns>True is dependent otherwise false.</returns>
        public bool IsRelatedToForm( Guid formId )
        {
            return formId == Parent.InnerId || formId == Child.InnerId;
        }

        /// <summary>
        /// Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            Caption = CreateInstance<ItemName>();
            Cardinality = CreateInstance<Cardinality>();
        }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public EntityRelation Clone()
        {
            var copy = CreateInstance<EntityRelation>();
            copy.Caption = Caption.Clone();
            copy.Cardinality = Cardinality.Clone();
            copy.Type = Type;

            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}