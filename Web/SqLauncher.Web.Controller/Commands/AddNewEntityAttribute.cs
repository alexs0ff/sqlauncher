// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   AddNewEntityAttribute.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 12 2:05 PM
//   * Modified at: 2011  09 12  2:11 PM
// / ******************************************************************************/ 

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   Adds new Entity attribute to ERDEntity.
    /// </summary>
    public class AddNewEntityAttribute : ICommand
    {
        /// <summary>
        ///   The erd entity.
        /// </summary>
        public ERDEntity ERDEntity { get; set; }

        /// <summary>
        ///   The added entity attribute.
        /// </summary>
        public EntityAttribute EntityAttribute { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            ERDEntity.Attributes.Add( EntityAttribute );
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            //TODO: make deep clearing index attributes.
            ERDEntity.Attributes.Remove( EntityAttribute );
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }
       
    }
}