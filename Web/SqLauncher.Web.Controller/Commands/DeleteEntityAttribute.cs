// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DeleteEntityAttribute.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 13 8:41 PM
//   * Modified at: 2011  09 13  8:51 PM
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Deletes an antity attribute.
    /// </summary>
    public class DeleteEntityAttribute : ICommand
    {
        /// <summary>
        ///   The holder of entity attribute.
        /// </summary>
        public ERDEntity ERDEntity { get; set; }

        /// <summary>
        /// </summary>
        public EntityAttribute EntityAttribute { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            ERDEntity.Attributes.Remove( EntityAttribute );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            ERDEntity.Attributes.Add( EntityAttribute );
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
    }
}