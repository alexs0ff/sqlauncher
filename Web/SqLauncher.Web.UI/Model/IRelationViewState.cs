// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IRelationViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 10 21:17
//   * Modified at: 2011  11 01  22:47
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents the view state for relation form.
    /// </summary>
    public interface IRelationViewState: IViewState, IDeepClonable<IRelationViewState>
    {
        /// <summary>
        ///   The data relation.
        /// </summary>
        EntityRelation Relation { get; set; }
    }
}