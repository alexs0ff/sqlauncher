// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityAttribute.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 18 10:46 PM
//   * Modified at: 2011  08 27  10:46 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model.Interception;
using SqLauncher.Web.Model.SqLite;

namespace SqLauncher.Web.Model
{
    public class EntityAttribute : BindableModelObject, IDeepClonable<EntityAttribute>
    {
        public EntityAttribute()
        {
            _dbType = new SqLiteInteger();
        }

        /// <summary>
        ///   The caption of attribute.
        /// </summary>
        private ItemName _caption;

        /// <summary>
        ///   The caption of attribute.
        /// </summary>
        [Dependency]
        [SelfPropertyChangedAttribute]
        public ItemName Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != null)
                {
                    _caption.PropertyChanged -= CaptionPropertyChanged;
                } //if

                _caption = value;
                _caption.PropertyChanged += CaptionPropertyChanged;
            }
        }

        /// <summary>
        /// Occurs when any of Caption properties has been changed.
        /// We should invoks the PropertyChanged fo all change events of caption.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void CaptionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RisePropertyChanged(  ItemName.CaptionPropertyName );
        }

        /// <summary>
        ///   The type of a column.
        /// </summary>
        private SqlTypeBase _dbType;

        /// <summary>
        ///   The property of column type.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual SqlTypeBase DbType
        {
            get { return _dbType; }
            set
            {
                _dbType = value;

                if (_dbType==null || !_dbType.HasLenght ){
                    DataLenght = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the types mapper for current database.
        /// </summary>
        [Dependency]
        public IDbTypesMapper DbTypesMapper { get; set; }

        /// <summary>
        ///   The lenght of a type.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual int DataLenght { get; set; }

        /// <summary>
        ///   The decimal digits.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual int Decimal { get; set; }

        /// <summary>
        ///   The flag which indicates a keyed attribute.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual AttributeKeyType Key { get; set; }

        /// <summary>
        ///   The flag which indicates a not nullable attribute.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsNotNull { get; set; }

        /// <summary>
        ///   The flag which indicates a unique attribute.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsUnique { get; set; }

        /// <summary>
        /// The flag which indicates a Identity attribute.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets the default value of attribute.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Default { get; set; }

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
        public EntityAttribute Clone()
        {
            var copy = CreateInstance<EntityAttribute>();
            copy.Caption = Caption.Clone();
            copy.IsIdentity = IsIdentity;
            copy.DataLenght = DataLenght;
            copy.IsNotNull = IsNotNull;
            copy.DbType = DbType;
            copy.DbTypesMapper = DbTypesMapper;
            copy.IsUnique = IsUnique;
            copy.Key = Key;
            copy.Default = Default;

            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}