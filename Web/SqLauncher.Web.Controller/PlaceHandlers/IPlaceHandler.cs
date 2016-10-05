// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IPlaceHandler.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 05 12:31 PM
//   * Modified at: 2011  09 05  12:41 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.Controller.PlaceHandlers
{
    /// <summary>
    ///   Represents a place handler for new added elements.
    /// </summary>
    internal interface IPlaceHandler
    {
        /// <summary>
        ///   Starts finding a new place.
        /// </summary>
        void StartAssignNewPlace();

        /// <summary>
        ///   Stops all operation.
        /// </summary>
        void Stop();
    }
}