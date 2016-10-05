// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeleteEntityAttributeEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 13 8:18 PM
//   * Modified at: 2011  09 13  8:21 PM
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Reprsesents the event args of delete entity event.
    /// </summary>
    public class DeleteEntityAttributeEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.Model.DeleteEntityAttributeEventArgs" /> class.
        /// </summary>
        public DeleteEntityAttributeEventArgs( EntityAttribute entityAttribute )
        {
            EntityAttribute = entityAttribute;
        }

        /// <summary>
        ///   The entity attribute to delete.
        /// </summary>
        public EntityAttribute EntityAttribute { get; private set; }
    }
}