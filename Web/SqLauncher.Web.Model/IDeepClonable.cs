// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IDeepClonable.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 30 15:05
//   * Modified at: 2011  11 30  15:08
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The deep clone contract.
    /// </summary>
    public interface IDeepClonable<T>
    {
        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        T Clone();

        /// <summary>
        /// The cloned object id.
        /// </summary>
        Guid ClonedBy { get; set; }
    }
}