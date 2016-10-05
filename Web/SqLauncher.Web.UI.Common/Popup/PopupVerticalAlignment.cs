// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   PopupVerticalAlignment.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 03 9:18 PM
//   * Modified at: 2011  10 03  9:20 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Common.Popup
{
    public enum PopupVerticalAlignment
    {
        // the top side of the popup is aligned with the top side of the placement target
        Top,
        // the bottom side of the popup is aligned with the center of the placement target
        BottomCenter,
        // the center of the popup is aligned with the center of the placement target
        Center,
        // the top side of the popup is aligned with the center of the placement target
        TopCenter,
        // the bottom side of the popup is aligned with the bottom side of the placement target
        Bottom
    }
}