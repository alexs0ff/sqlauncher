// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AttributeKeyType.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 20 11:11
//   * Modified at: 2011  11 20  11:14
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The key type for attributes.
    /// </summary>
    public enum AttributeKeyType
    {
        /// <summary>
        ///   The default value.
        /// </summary>
        None,

        /// <summary>
        ///   Indicates the keyed attribute.
        /// </summary>
        IsKey,

        /// <summary>
        ///   Indicates the foreing key attribute.
        /// </summary>
        IsForeignKey,

        /// <summary>
        /// Indicates that primary and foreign key attribute.
        /// </summary>
        IsPrimaryForeignKey
    }
}