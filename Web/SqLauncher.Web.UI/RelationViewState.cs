// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RelationViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 10 10 9:10 PM
//   * Modified at: 2011  10 10  9:14 PM
// / ******************************************************************************/ 

using System;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    /// The view state for form.
    /// </summary>
    public class RelationViewState : ViewStateBase, IRelationViewState
    {
        /// <summary>
        /// The data relation.
        /// </summary>
        [Dependency]
        [SelfPropertyChanged]
        public virtual EntityRelation Relation { get; set; }

        /// <summary>
        ///   The start point of the line.
        /// </summary>
        public RectConnector StartPoint { get; set; }

        /// <summary>
        ///   The destination point of the line.
        /// </summary>
        public RectConnector DestinationPoint { get; set; }

        /// <summary>
        /// Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected override void OnNeedRebuildModelObject()
        {
            Relation = CreateInstance<EntityRelation>();
        }

        /// <summary>
        ///   Clones the entity.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public IRelationViewState Clone()
        {
            var copy = (RelationViewState) CreateInstance<IRelationViewState>();

            copy.ClonedBy = InnerId;
            copy.DestinationPoint = DestinationPoint;
            copy.StartPoint = StartPoint;
            return copy;
        }

        /// <summary>
        /// The cloned object id.
        /// </summary>
        public Guid ClonedBy { get; set; }
    }
}