// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ClipboardManager.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 12 22 21:36
//   * Modified at: 2012  01 10  23:22
// / ******************************************************************************/ 

using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The manager of clipboard operations.
    /// </summary>
    public sealed class ClipboardManager
    {
        /// <summary>
        ///   The private constructor.
        /// </summary>
        private ClipboardManager()
        {
        }

        /// <summary>
        ///   The instance.
        /// </summary>
        private static readonly ClipboardManager _manager = new ClipboardManager();

        /// <summary>
        ///   The singleton instance.
        /// </summary>
        public static ClipboardManager Manager
        {
            get { return _manager; }
        }

        /// <summary>
        ///   The stored entities.
        /// </summary>
        private readonly ICollection<IEntityViewState> _storedEntities = new Collection<IEntityViewState>();

        /// <summary>
        ///   Copies into clipboard specific entities.
        /// </summary>
        public void SetEntities( ICollection<IEntityViewState> entities )
        {
            _storedEntities.Clear();

            foreach ( var entityViewState in entities ){
                _storedEntities.Add( entityViewState.Clone() );
            } //foreach
        }

        /// <summary>
        ///   Gets the stored entities.
        /// </summary>
        /// <returns>The entity collection.</returns>
        public ICollection<IEntityViewState> GetStoredEntities()
        {
            return (from entityViewState in _storedEntities select entityViewState.Clone()).ToList();
        }
    }
}