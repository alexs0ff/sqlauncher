// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ICommand.cs
//   * Project: SqLauncher.Web.Controller
//   * Description:
//   * Created at 2011 09 05 3:07 PM
//   * Modified at: 2011  09 05  3:07 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    ///   The command pattern.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///   Executes the command.
        /// </summary>
        void Do();

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        void Undo();

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        bool Done { get; set; }
    }
}