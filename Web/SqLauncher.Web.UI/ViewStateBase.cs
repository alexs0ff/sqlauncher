// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ViewStateBase.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 03 03 13:42
//   * Modified at: 2012  03 03  13:55
// / ******************************************************************************/ 

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   The state of all statable parts.
    /// </summary>
    public abstract class ViewStateBase :BindableModelObject, IViewState
    {
        /// <summary>
        ///   Gets or sets the model iteraction state.
        /// </summary>
        [Dependency]
        public IIteractionState IteractionState { get; set; }
    }
}