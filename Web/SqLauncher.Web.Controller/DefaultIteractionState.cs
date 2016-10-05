// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DefaultIteractionState.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 03 03 14:03
//   * Modified at: 2012  03 03  14:03
// / ******************************************************************************/ 

using SqLauncher.Web.Model;
using SqLauncher.Web.UI;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The default iteraction state.
    /// </summary>
    public class DefaultIteractionState : IIteractionState
    {
        /// <summary>
        ///   Gets or sets the flag of physical view.
        /// </summary>
        public bool PhysicalView { get; set; }

        /// <summary>
        /// Gets or sets model entity script generator.
        /// </summary>
        public ERDEntityGeneratorBase EntityGenerator { get; set; }

        /// <summary>
        /// Gets or sets model relation script generator.
        /// </summary>
        public EntityRelationGeneratorBase RelationGenerator { get; set; }

        /// <summary>
        /// Gets or sets the assotiated ASCII painter.
        /// </summary>
        public ERDEntityASCIIPainterBase EntityASCIIPainter { get; set; }
    }
}