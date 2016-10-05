// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeleteIndexAttribute.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2012 02 26 19:21
//   * Modified at: 2012  02 26  19:47
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Removes an index attribute from Index.
    /// </summary>
    public class DeleteIndexAttribute : ICommand
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
        private EntityAttribute EntityAttribute { get; set; }

        /// <summary>
        /// The indexes property name.
        /// </summary>
        private const string IndexesPropertyName = "Indexes";

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            EntityAttribute = IndexAttribute.Attribute;
            IndexAttribute.Attribute = null;
            EntityIndex.Attributes.Remove( IndexAttribute );
            EntityIndex.Parent.RisePropertyChanged(IndexesPropertyName);

        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            IndexAttribute.Attribute = EntityAttribute;
            EntityIndex.Attributes.Add( IndexAttribute );
            EntityIndex.Parent.RisePropertyChanged(IndexesPropertyName);
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}