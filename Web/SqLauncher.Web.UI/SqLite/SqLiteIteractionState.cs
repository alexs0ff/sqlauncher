// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLiteIteractionState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 03 03 13:50
//   * Modified at: 2012  03 03  13:59
// / ******************************************************************************/ 

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.UI.SqLite
{
    /// <summary>
    ///   The iteraction state of sqlite model.
    /// </summary>
    public class SqLiteIteractionState : BindableModelObject, IIteractionState
    {
        /// <summary>
        ///   Gets or sets the flag of physical view.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual bool PhysicalView { get; set; }

        /// <summary>
        /// Gets or sets model entity script generator.
        /// </summary>
        [Dependency]
        public ERDEntityGeneratorBase EntityGenerator { get; set; }

        /// <summary>
        /// Gets or sets model relation script generator.
        /// </summary>
        [Dependency]
        public EntityRelationGeneratorBase RelationGenerator { get; set; }

        /// <summary>
        /// Gets or sets the assotiated ASCII painter.
        /// </summary>
        [Dependency]
        public ERDEntityASCIIPainterBase EntityASCIIPainter { get; set; }
    }
}