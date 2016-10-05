// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ExtendedTabItem.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 11 13:42
//   * Modified at: 2012  02 11  13:43
// / ******************************************************************************/ 

using System.Windows.Controls;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   The extended tab item.
    /// </summary>
    public class ExtendedTabItem : TabItem
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Windows.Controls.TabItem" /> class.
        /// </summary>
        public ExtendedTabItem()
        {
            DefaultStyleKey = typeof(ExtendedTabItem);
        }

        /// <summary>
        /// Builds the visual tree for the <see cref="T:System.Windows.Controls.TabItem"/> when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}