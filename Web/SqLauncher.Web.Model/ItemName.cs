// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ItemName.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 22 11:59 AM
//   * Modified at: 2011  08 22  12:03 PM
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Represents a class for named entities: entities, attributes etc.
    /// </summary>
    public class ItemName : BindableModelObject, IDeepClonable<ItemName>
    {
        /// <summary>
        /// The name of caption property.
        /// </summary>
        public const string CaptionPropertyName = "Caption";

        /// <summary>
        ///   The design title.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Title { get; set; }

        /// <summary>
        ///   The physical name.
        /// </summary>
        [NotifyPropertyChanged]
        public virtual string Physical { get; set; }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public ItemName Clone()
        {
            var copy = CreateInstance<ItemName>();
            copy.Title = Title;
            copy.Physical = Physical;
            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}