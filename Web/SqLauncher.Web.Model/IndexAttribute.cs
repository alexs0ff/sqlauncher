// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IndexAttribute.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 02 20 20:51
//   * Modified at: 2012  02 20  21:41
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.Model
{
    public class IndexAttribute : BindableModelObject, IDeepClonable<IndexAttribute>
    {
        /// <summary>
        ///   The associated entity attribute.
        /// </summary>
        public EntityAttribute Attribute { get; set; }

        /// <summary>
        ///   The sort order of this index attribute.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual SortOrder Order { get; set; }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IndexAttribute Clone()
        {
            var copy = CreateInstance<IndexAttribute>();
            copy.Order = Order;
            return copy;
        }

        /// <summary>
        ///   The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}