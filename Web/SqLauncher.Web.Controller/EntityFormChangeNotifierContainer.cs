// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   EntityFormChangeNotifierContainer.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 08 8:13 PM
//   * Modified at: 2011  09 12  8:07 PM
// / ******************************************************************************/ 

using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.Controller
{
    /// <summary>
    ///   The container of EntityForm and his changes Listener.
    /// </summary>
    internal class EntityFormChangeNotifierContainer
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Controller.EntityFormChangeNotifierContainer" /> class.
        /// </summary>
        public EntityFormChangeNotifierContainer( IEntityForm entityForm,
                                                  EntityFormChangesManager entityFormChangesManager
            )
        {
            EntityForm = entityForm;
            EntityFormChangesManager = entityFormChangesManager;
        }

        /// <summary>
        ///   The entity form change manager.
        /// </summary>
        public EntityFormChangesManager EntityFormChangesManager { get; set; }

        /// <summary>
        ///   The entity form.
        /// </summary>
        public IEntityForm EntityForm { get; set; }
    }
}