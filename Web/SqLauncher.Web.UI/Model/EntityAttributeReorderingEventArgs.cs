// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityAttributeReorderingEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 03 06 21:57
//   * Modified at: 2012  03 06  22:17
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args for IEntityForm reorder event.
    /// </summary>
    public class EntityAttributeReorderingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.UI.Model.EntityAttributeReorderingEventArgs"/> class.
        /// </summary>
        public EntityAttributeReorderingEventArgs( EntityAttribute attribute, int currentIndex, int newIndex )
        {
            Attribute = attribute;
            CurrentIndex = currentIndex;
            NewIndex = newIndex;
        }

        /// <summary>
        ///   Gets or sets attribute to reorder operation.
        /// </summary>
        public EntityAttribute Attribute { get; private set; }

        /// <summary>
        ///   The current index of attribute.
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        ///   Gets the new index to replace.
        /// </summary>
        public int NewIndex { get; private set; }
    }
}