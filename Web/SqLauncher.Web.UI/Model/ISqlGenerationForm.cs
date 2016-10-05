// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ISqlGenerationForm.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 25 10:47
//   * Modified at: 2011  11 25  13:30
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   The sql generation form.
    /// </summary>
    public interface ISqlGenerationForm
    {
        /// <summary>
        ///   Gets a collection for generated members.
        /// </summary>
        ISqlGenerationFormViewState DataEntity { get; set; }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        void Start();

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        void CloseForm();

        /// <summary>
        ///   Occurs when user want to generate sql and save it.
        /// </summary>
        event EventHandler<SqlGeneratingEventArgs> SqlGenerating;

        /// <summary>
        /// Occurs when dialog closed.
        /// </summary>
        event EventHandler<DialogClosedEventArgs> DialogClosed;
    }
}