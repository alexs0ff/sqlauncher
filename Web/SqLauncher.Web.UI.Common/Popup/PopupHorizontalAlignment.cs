// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   PopupHorizontalAlignment.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 03 9:18 PM
//   * Modified at: 2011  10 03  9:19 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Common.Popup
{
    public enum PopupHorizontalAlignment
    {
        // the left side of the popup is aligned with the left side of the placement target
        Left,
        // the right side of the popup is aligned with the center of the placement target
        RightCenter,
        // the center of the popup is aligned with the center of the placement target
        Center,
        // the left side of the popup is aligned with the center of the placement target
        LeftCenter,
        // the right side of the popup is aligned with the right side of the placement target
        Right
    }
}