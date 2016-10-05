// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IEntityForm.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 13 9:56 PM
//   * Modified at: 2011  10 01  12:49 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Interface of EntityForm.
    /// </summary>
    public interface IEntityForm : IForm
    {
        /// <summary>
        ///   The data context.
        /// </summary>
        IEntityViewState DataEntity { get; set; }

        /// <summary>
        ///   The actual Width
        /// </summary>
        double CurrentWidth { get; }

        /// <summary>
        ///   The Actual Height of the element.
        /// </summary>
        double CurrentHeight { get; }

        /// <summary>
        ///   Ocures when Entity From changed its location.
        /// </summary>
        event EventHandler<PositionChangedEventArgs> PositionChanged;

        /// <summary>
        ///   Occurs when the entity changing self position.
        /// </summary>
        event EventHandler<PositionChangingEventArgs> PositionChanging;

        /// <summary>
        ///   Occurs when changed visibility of ViewForm.
        /// </summary>
        event EventHandler<VisibilityChangedEventArgs> ViewFormVisibilityChanged;

        /// <summary>
        ///   Occurs when a mouse button down.
        /// </summary>
        event EventHandler<MouseButtonDownEventArgs> MouseButtonDown;

        /// <summary>
        ///   Occurs when we need to add an entity attribute.
        /// </summary>
        event EventHandler<AddEntityAttributeEventArgs> AddEntityAttribute;

        /// <summary>
        /// Occurs when we need to add an entity index.
        /// </summary>
        event EventHandler<AddEntityIndexEventArgs> AddEntityIndex;

        /// <summary>
        ///   Occurs when we need to delete an entity attribute.
        /// </summary>
        event EventHandler<DeleteEntityAttributeEventArgs> DeleteEntityAttribute;

        /// <summary>
        /// Occurs when we need to delete an entity index.
        /// </summary>
        event EventHandler<DeleteEntityIndexEventArgs> DeleteEntityIndex;

        /// <summary>
        /// Occurs when we need to add an index attribute.
        /// </summary>
        event EventHandler<AddIndexAttributeEventArgs> AddIndexAttribute;

        /// <summary>
        /// Occurs when we need to delete an index attribute.
        /// </summary>
        event EventHandler<DeleteIndexAttributeEventArgs> DeleteIndexAttribute;

        /// <summary>
        ///   Resizes the entity form without EntitySizeChanged event rise.
        /// </summary>
        /// <param name = "rect">The new size rect.</param>
        void ForceResize( Rect rect );

        /// <summary>
        ///   Occurs when size of the current entity form has been changed.
        /// </summary>
        event EventHandler<EntitySizeChangedEventArgs> EntitySizeChanged;

        /// <summary>
        /// Occurs when user want to reorder some attribute.
        /// </summary>
        event EventHandler<EntityAttributeReorderingEventArgs> EntityAttributeReordering;
    }
}