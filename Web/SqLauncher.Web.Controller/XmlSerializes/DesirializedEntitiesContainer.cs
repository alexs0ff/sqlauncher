using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.XmlSerializes
{
    /// <summary>
    /// The container of desirialized entities.
    /// </summary>
    public sealed class DesirializedEntitiesContainer
    {
        /// <summary>
        /// Gets or sets the ERD entities.
        /// </summary>
        public ICollection<ERDEntity> Entities { get; set; }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public ICollection<EntityRelation> Relations { get; set; }

        /// <summary>
        /// Gets the entity by inner id.
        /// </summary>
        /// <param name="innerId">The inner id.</param>
        /// <returns>The erd entity or null.</returns>
        public ERDEntity GetEntityById(Guid innerId)
        {
            return Entities.FirstOrDefault( en => en.InnerId == innerId );
        }

        /// <summary>
        /// Gets the relation by inner id.
        /// </summary>
        /// <param name="innerId">The inner id.</param>
        /// <returns>The relation or null.</returns>
        public EntityRelation GetRelationById(Guid innerId)
        {
            return Relations.FirstOrDefault( rl => rl.InnerId == innerId );
        }
    }
}
