// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IDbTypesMapper.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 14 20:05
//   * Modified at: 2011  11 14  20:08
// / ******************************************************************************/ 

using System.Collections.Generic;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The interface for the all db types mapping.
    /// </summary>
    public interface IDbTypesMapper
    {
        /// <summary>
        ///   Gets the all registred types for specific database.
        /// </summary>
        /// <returns></returns>
        ICollection<SqlTypeBase> RegisteredTypes { get; }
    }
}