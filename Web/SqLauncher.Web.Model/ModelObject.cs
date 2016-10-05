// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ModelObject.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  01 04  14:54
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   Presents the base class for each in ERD model
    /// </summary>
    public abstract class ModelObject
    {
        /// <summary>
        ///   The id to internal using.
        /// </summary>
        private Guid _innerId = Guid.NewGuid();

        /// <summary>
        ///   The id to internal using.
        /// </summary>
        public Guid InnerId
        {
            get { return _innerId; }
        }

        /// <summary>
        ///   Sets the new inner id.
        ///   It used only desiarilize purposes.
        /// </summary>
        /// <param name = "newGuid">The new guid value.</param>
        public void SetInnerId( Guid newGuid )
        {
            _innerId = newGuid;
        }
    }
}