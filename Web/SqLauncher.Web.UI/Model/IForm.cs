// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IForm.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 13 8:56 PM
//   * Modified at: 2011  09 12  8:18 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents base interface of the all model view forms (EntityForm, Relation From etc)
    /// </summary>
    public interface IForm
    {
        /// <summary>
        /// Initializes instance of the form.
        /// Invokes when from has been added into the model view.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Uninitializes the instance of the form.
        /// Invokes when from has been removed from the model view.
        /// </summary>
        void Uninitialize();

        /// <summary>
        ///   Occurs when the form will be selected or unselected.
        /// </summary>
        event EventHandler<SelectionStateChangedEventArgs> SelectionStateChanged;

        /// <summary>
        ///   The flag which indicates that a form has been selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        ///   Occurs when from has been loaded.
        /// </summary>
        event EventHandler<ElementLoadedEventArgs> ElementLoaded;

        /// <summary>
        /// Gets the information about loaded event has been rised.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        ///   Returns the left position of the entity form.
        /// </summary>
        /// <returns>The position.</returns>
        double GetLeft();

        /// <summary>
        ///   Sets the top position of the entity form on view model.
        /// </summary>
        /// <param name = "y">The top position</param>
        void SetTop( double y );

        /// <summary>
        ///   Sets the left position of the entity form on view model.
        /// </summary>
        /// <param name = "x">The left position</param>
        void SetLeft( double x );

        /// <summary>
        ///   Returns the Z index of the entity form.
        /// </summary>
        /// <returns></returns>
        int GetZIndex();

        /// <summary>
        ///   Returns the top position of the entity form.
        /// </summary>
        /// <returns>The top value.</returns>
        double GetTop();

        /// <summary>
        /// Checks what form has the point.
        /// </summary>
        /// <param name="pointToCheck">Point to check.</param>
        /// <returns>True if it belongs to otherwise false</returns>
        bool HitTest( Point pointToCheck );
    }
}