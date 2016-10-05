// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AddIndexAttributeEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 26 19:50
//   * Modified at: 2012  02 26  19:55
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args of index attrubute adding.
    /// </summary>
    public class AddIndexAttributeEventArgs : EventArgs
    {
        /// <summary>
        ///   The entity attribute.
        /// </summary>
        public EntityAttribute EntityAttribute { get; private set; }

        /// <summary>
        /// The parent index of index attribute.
        /// </summary>
        public EntityIndex EntityIndex { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.AddIndexAttributeEventArgs" /> class.
        /// </summary>
        public AddIndexAttributeEventArgs(EntityAttribute entityAttribute, EntityIndex entityIndex)
        {
            EntityAttribute = entityAttribute;
            EntityIndex = entityIndex;
        }
    }
}