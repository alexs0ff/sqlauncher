// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   Cardinality.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 30 11:24 PM
//   * Modified at: 2011  10 16  12:27 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel.DataAnnotations;

using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents the cardinality of one data table with restrict to another data table.
    /// </summary>
    public class Cardinality : BindableModelObject,IDeepClonable<Cardinality>
    {
        /// <summary>
        ///   The parent table. May be between 0 and 1.
        /// </summary>
        [PerformValidation]
        [NotifyPropertyChanged, Range(0,1,ErrorMessage = "1 or 0")]
        public virtual int ParentFrom { get; set; }

        /// <summary>
        ///   The child table. May be between 0 and 1.
        /// </summary>
        [NotifyPropertyChanged]
        [PerformValidation]
        [Range(0, 1, ErrorMessage = "1 or 0")]
        public virtual int ChildFrom { get; set; }

        /// <summary>
        /// The child table. May be between 0 and N.
        /// </summary>
        private string _childTo = "N";

        /// <summary>
        ///  Gets or sets The child table. May be between 0 and N.
        /// </summary>
        [NotifyPropertyChanged]
        [PerformValidation]
        [RegularExpression(@"^(\d+)|(N)$", ErrorMessage = "from 0 to N")]
        public virtual string ChildTo
        {
            get { return _childTo; }
            set { _childTo = value; }
        }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public Cardinality Clone()
        {
            var copy = CreateInstance<Cardinality>();

            copy.ChildFrom = ChildFrom;
            copy.ChildTo = ChildTo;
            copy.ParentFrom = ParentFrom;

            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}