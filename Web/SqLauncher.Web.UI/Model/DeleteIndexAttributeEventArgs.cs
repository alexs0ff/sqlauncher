// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeleteIndexAttributeEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 26 19:56
//   * Modified at: 2012  02 26  20:03
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The event args for index attribute deleting.
    /// </summary>
    public class DeleteIndexAttributeEventArgs : EventArgs
    {
        /// <summary>
        ///   The index attribute that will be removed.
        /// </summary>
        public IndexAttribute IndexAttribute { get; private set; }

        /// <summary>
        ///   The parent index of index attribute.
        /// </summary>
        public EntityIndex EntityIndex { get; private set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.DeleteIndexAttributeEventArgs" /> class.
        /// </summary>
        public DeleteIndexAttributeEventArgs( IndexAttribute indexAttribute, EntityIndex entityIndex )
        {
            IndexAttribute = indexAttribute;
            EntityIndex = entityIndex;
        }
    }
}