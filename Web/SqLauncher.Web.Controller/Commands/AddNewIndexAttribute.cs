// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AddNewIndexAttribute.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 02 26 19:14
//   * Modified at: 2012  02 26  19:20
// / ******************************************************************************/ 

using System.Collections.ObjectModel;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Adds new attribute to Entity Index.
    /// </summary>
    public class AddNewIndexAttribute : ICommand
    {
        /// <summary>
        ///   The entity index.
        /// </summary>
        public EntityIndex EntityIndex { get; set; }

        /// <summary>
        ///   The added index attribute.
        /// </summary>
        public IndexAttribute IndexAttribute { get; set; }

        /// <summary>
        ///   The entity attribute of index.
        /// </summary>
        public EntityAttribute EntityAttribute { get; set; }

        /// <summary>
        /// The indexes property name.
        /// </summary>
        private const string IndexesPropertyName = "Indexes";

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            IndexAttribute.Attribute = EntityAttribute;
            EntityIndex.Attributes.Add( IndexAttribute );
            EntityIndex.Parent.RisePropertyChanged( IndexesPropertyName );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            IndexAttribute.Attribute = null;
            EntityIndex.Attributes.Remove( IndexAttribute );
            EntityIndex.Parent.RisePropertyChanged(IndexesPropertyName);
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}