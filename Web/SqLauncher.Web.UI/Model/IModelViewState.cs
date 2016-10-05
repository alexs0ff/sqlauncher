// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IModelViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 08 19:28
//   * Modified at: 2011  11 01  22:40
// / ******************************************************************************/ 

using System;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The model view state.
    /// </summary>
    public interface IModelViewState : IDeepClonable<IModelViewState>
    {
        /// <summary>
        ///   Gets or sets the handled data model.
        /// </summary>
        DataModel DataModel { get; set; }

        /// <summary>
        ///   The inner id.
        /// </summary>
        Guid InnerId { get; }

        /// <summary>
        ///   The height of the model view.
        /// </summary>
        double Height { get; set; }

        /// <summary>
        ///   The width of the model view.
        /// </summary>
        double Width { get; set; }

        /// <summary>
        ///   Sets the default values for visual state.
        /// </summary>
        void SetDefaults();

        /// <summary>
        ///   Gets entity view states.
        /// </summary>
        EntityViewStateCollection EntityViewStates { get; }

        /// <summary>
        ///   Gets the relation view states.
        /// </summary>
        RelationViewStateCollection EntityRelationStates { get; }
    }
}