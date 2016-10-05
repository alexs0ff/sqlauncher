// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IModelView.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 06 9:56 PM
//   * Modified at: 2011  09 11  7:21 PM
// / ******************************************************************************/ 

using System;
using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents a Model View interface
    /// </summary>
    public interface IModelView
    {
        /// <summary>
        ///   The data entity
        /// </summary>
        IModelViewState DataEntity { get; set; }
        
        /// <summary>
        ///   Appends to canvas the entity form.
        /// </summary>
        /// <param name = "form">The new form.</param>
        void AddChild( IForm form );

        /// <summary>
        ///   Removes from canvas the entity form.
        /// </summary>
        /// <param name = "form">The entity form to remove.</param>
        void RemoveChild( IForm form );

        /// <summary>
        /// The zoom of data model.
        /// </summary>
        double Zoom { get; set; }

        /// <summary>
        /// Shows the selection.
        /// </summary>
        void ShowSelection();

        /// <summary>
        /// Sets the selection by the begin and the end points.
        /// </summary>
        /// <param name="start">The satrt point.</param>
        /// <param name="end">The end point.</param>
        void SetSelection(Point start, Point end);

        /// <summary>
        /// Hides the selection on model view.
        /// </summary>
        void HideSelection();

        /// <summary>
        /// Captures the mouse events.
        /// </summary>
        void MouseCapture();

        /// <summary>
        /// Releases the mouse.
        /// </summary>
        void MouseRelease();

        /// <summary>
        /// The current width of the model work space.
        /// </summary>
        double CurrentWidth { get; }

        /// <summary>
        /// The current íeight of the model work space.
        /// </summary>
        double CurrentHeight { get; }

        /// <summary>
        ///   Occurs when mouse move on model view canvas.
        /// </summary>
        event EventHandler<ScaledMouseMoveEventArgs> ModelMouseMove;

        /// <summary>
        /// Occurs when mouse button up.
        /// </summary>
        event EventHandler<MouseButtonUpEventArgs> MouseButtonUp;

        /// <summary>
        /// Occurs when mouse button down.
        /// </summary>
        event EventHandler<MouseButtonDownEventArgs> MouseButtonDown;
    }
}