// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityIndex.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 02 20 20:34
//   * Modified at: 2012  02 24  20:01
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
    ///   The class that represents index
    /// </summary>
    public class EntityIndex : BindableModelObject, IDeepClonable<EntityIndex>
    {
        public EntityIndex()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Attributes = new ObservableCollection<IndexAttribute>();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// The parent erd entity.
        /// </summary>
        public ERDEntity Parent { get; set; }

        /// <summary>
        ///   The flag then duplicate index entries are not allowed
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsUnique { get; set; }

        /// <summary>
        ///   The caption of attribute.
        /// </summary>
        private ItemName _caption;

        /// <summary>
        ///   The caption of attribute.
        /// </summary>
        [Dependency]
        [SelfPropertyChangedAttribute]
        public virtual ItemName Caption
        {
            get { return _caption; }
            set
            {
                if ( _caption != null ){
                    _caption.PropertyChanged -= CaptionPropertyChanged;
                } //if

                _caption = value;
                _caption.PropertyChanged += CaptionPropertyChanged;
            }
        }

        /// <summary>
        ///   Occurs when any of Caption properties has been changed.
        ///   We should invoks the PropertyChanged fo all change events of caption.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void CaptionPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            RisePropertyChanged( ItemName.CaptionPropertyName );
        }

        /// <summary>
        ///   Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            Caption = CreateInstance<ItemName>();
            Attributes = new ObservableCollection<IndexAttribute>();
        }

        /// <summary>
        ///   Gets the collection of index attributes.
        /// </summary>
        [SelfPropertyChangedAttribute]
        public virtual ICollection<IndexAttribute> Attributes { get; set; }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public EntityIndex Clone()
        {
            var copy = CreateInstance<EntityIndex>();

            copy.Caption = Caption.Clone();
            copy.IsUnique = IsUnique;

            return copy;
        }

        /// <summary>
        ///   The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}