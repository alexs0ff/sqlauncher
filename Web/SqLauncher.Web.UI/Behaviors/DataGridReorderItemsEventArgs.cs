// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DataGridReorderItemsEventArgs.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 03 06 21:35
//   * Modified at: 2012  03 06  21:42
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   The event args for items reordering event.
    /// </summary>
    public class DataGridReorderItemsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public DataGridReorderItemsEventArgs( object reorderingItem, int oldIndex, int newIndex )
        {
            ReorderingItem = reorderingItem;
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }

        /// <summary>
        ///   The item to reordering operation.
        /// </summary>
        public object ReorderingItem { get; private set; }

        /// <summary>
        ///   Gets the old index of item.
        /// </summary>
        public int OldIndex { get; private set; }

        /// <summary>
        ///   Gets the new index.
        /// </summary>
        public int NewIndex { get; private set; }
    }
}