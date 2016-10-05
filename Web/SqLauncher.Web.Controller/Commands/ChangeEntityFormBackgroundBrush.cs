// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ChangeEntityFormBackgroundBrush.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 03 02 21:39
//   * Modified at: 2012  03 02  21:46
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Changes background brush for entity forms.
    /// </summary>
    public class ChangeEntityFormBackgroundBrush : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ChangeEntityFormBackgroundBrush()
        {
            OldBrushes = new Dictionary<Guid, Brush>();
            NewBrushes = new Dictionary<Guid, Brush>();
        }

        /// <summary>
        ///   The old brushes.
        /// </summary>
        public Dictionary<Guid, Brush> OldBrushes { get; private set; }

        /// <summary>
        ///   The new brushes.
        /// </summary>
        public Dictionary<Guid, Brush> NewBrushes { get; private set; }

        /// <summary>
        ///   The model view manager.
        /// </summary>
        public ModelViewManager ModelViewManager { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            foreach ( var newBrush in NewBrushes ){
                var entityForm =
                    ModelViewManager.RegistredEntityForms.First( form => form.DataEntity.Entity.InnerId == newBrush.Key );
                entityForm.DataEntity.BackgroundBrush = newBrush.Value;
            } //foreach
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            foreach ( var oldBrush in OldBrushes ){
                var entityForm =
                    ModelViewManager.RegistredEntityForms.First( form => form.DataEntity.Entity.InnerId == oldBrush.Key );
                entityForm.DataEntity.BackgroundBrush = oldBrush.Value;
            } //foreach
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}