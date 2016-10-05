// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IController.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 25 1:09 PM
//   * Modified at: 2011  09 25  4:31 PM
// / ******************************************************************************/ 

using System.Windows.Controls;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   Represents an interface for all view controllers.
    /// </summary>
    public interface IViewController
    {
        /// <summary>
        ///   Returns a view for this controller.
        /// </summary>
        /// <returns></returns>
        UserControl GetView();

        /// <summary>
        /// The user iteraction provider.
        /// </summary>
        UserIteractionProvider IteractionProvider { get; }

        /// <summary>
        /// The method invoke when current view controller is selected for viewing.
        /// </summary>
        void OnShowing();
    }
}