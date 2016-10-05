// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 03 04 22:08
//   * Modified at: 2012  03 04  22:17
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The iteraction view state interface.
    /// </summary>
    public interface IViewState
    {
        /// <summary>
        ///   Gets or sets the model iteraction state.
        /// </summary>
        IIteractionState IteractionState { get; set; }
    }
}