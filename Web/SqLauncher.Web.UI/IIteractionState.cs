// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IIteractionState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 03 03 13:45
//   * Modified at: 2012  03 03  13:48
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   The interface of persistent model state.
    /// </summary>
    public interface IIteractionState
    {
        /// <summary>
        ///   Gets or sets the flag of physical view.
        /// </summary>
        bool PhysicalView { get; set; }
        
        /// <summary>
        /// Gets or sets model entity script generator.
        /// </summary>
        ERDEntityGeneratorBase EntityGenerator { get; set; }

        /// <summary>
        /// Gets or sets model relation script generator.
        /// </summary>
        EntityRelationGeneratorBase RelationGenerator { get; set; }

        /// <summary>
        /// Gets or sets the assotiated ASCII painter.
        /// </summary>
        ERDEntityASCIIPainterBase EntityASCIIPainter { get; set; }
    }
}