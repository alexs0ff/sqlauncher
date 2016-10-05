// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SortOrder.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 02 20 21:02
//   * Modified at: 2012  02 20  21:12
// / ******************************************************************************/ 

using System.ComponentModel;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   SortOrder is an enumeration of the possible sort ordering.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        ///   Enumeration value indicating the items are sorted in increasing order.
        /// </summary>
        [Description("asc")]
        Ascending,

        /// <summary>
        ///   Enumeration value indicating the items are sorted in decreasing order.
        /// </summary>
        [Description("desc")]
        Descending,
    }
}