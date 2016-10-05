// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IEntityViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 01 12:36 PM
//   * Modified at: 2011  10 01  12:37 PM
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Media;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The entity view state.
    /// </summary>
    public interface IEntityViewState :IViewState, IDeepClonable<IEntityViewState>
    {
        /// <summary>
        ///   The handled erd entity.
        /// </summary>
        ERDEntity Entity { get; set; }

        /// <summary>
        /// The inner id.
        /// </summary>
        Guid InnerId { get; }

        /// <summary>
        /// Sets the default values for visual state.
        /// </summary>
        void SetDefaults();

        /// <summary>
        /// Gets or sets editing state.
        /// True when the form has edit state.
        /// </summary>
        bool IsEditing { get; set; }

        /// <summary>
        ///   The background brush.
        /// </summary>
        Brush BackgroundBrush { get; set; }

        /// <summary>
        /// The current location.
        /// When you set this property physic lacation of the bounded form don`t change!
        /// This property only data bound purposes.
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// Gets the width of the entity view.
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// Gets the height of the entity.
        /// </summary>
        double Height { get; set; }
    }
}