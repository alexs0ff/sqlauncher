// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 21 19:22
//   * Modified at: 2012  01 11  22:16
// / ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Media;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;
using SqLauncher.Web.UI.Common;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   Represents the view propeties for displaing the ERD entity.
    /// </summary>
    public class EntityViewState : ViewStateBase, IEntityViewState
    {
        /// <summary>
        ///   The key of default background brush.
        /// </summary>
        public const string DefaultBrushKey = "EntityFormDefaultBrush";

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.UI.EntityViewState" /> class.
        /// </summary>
        public EntityViewState()
        {
            SetDefaults();
        }

        /// <summary>
        ///   Sets the default values for visual state.
        /// </summary>
        public void SetDefaults()
        {
            var reader = new XamlStyleReader();
            BackgroundBrush = reader.GetBrush( DefaultBrushKey );
            Width = double.NaN;
            Height = double.NaN;
        }

        /// <summary>
        ///   The handled erd entity object.
        /// </summary>
        [Dependency]
        [SelfPropertyChangedAttribute]
        public virtual ERDEntity Entity { get; set; }

        /// <summary>
        ///   The background brush.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual Brush BackgroundBrush { get; set; }

        /// <summary>
        ///   The current location.
        ///   When you set this property physic location of the bounded form don`t change!
        ///   This property only data bound purposes.
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        ///   The inner id.
        /// </summary>
        public new Guid InnerId
        {
            get { return Entity.InnerId; }
        }

        /// <summary>
        ///   Gets or sets editing state.
        ///   True when the form has edit state.
        /// </summary>
        [NotifyPropertyChanged( RiseValueChanged = false )]
        public virtual bool IsEditing { get; set; }

        /// <summary>
        ///   Gets or sets the width of the entity view.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        ///   Gets or sets the height of the entity.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            Entity = CreateInstance<ERDEntity>();
        }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IEntityViewState Clone()
        {
            var copy = (EntityViewState) CreateInstance<IEntityViewState>();

            copy.Entity = Entity.Clone();
            copy.IsEditing = IsEditing;
            copy.Location = Location;
            copy.BackgroundBrush = BackgroundBrush;
            return copy;
        }

        /// <summary>
        ///   The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}