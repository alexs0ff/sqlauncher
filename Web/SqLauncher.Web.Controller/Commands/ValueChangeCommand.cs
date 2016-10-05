using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.Controller.Commands
{
    /// <summary>
    /// Changes value of an DataModle object property.
    /// </summary>
    public class ValueChangeCommand: ICommand
    {
        /// <summary>
        ///   The property info of changed property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        ///   The changed object.
        /// </summary>
        public Object Target { get; set; }

        /// <summary>
        ///   The old value.
        /// </summary>
        public Object OldValue { get; set; }

        /// <summary>
        ///   The new value.
        /// </summary>
        public Object NewValue { get; set; }

        /// <summary>
        ///   Executes the command.
        /// </summary>
        public void Do()
        {
            Controller.ModelContainerWiring.BeginSuspendValueChangeEvent();

            try
            {
                PropertyInfo.SetValue(Target, NewValue, null);
            }
            finally{
                Controller.ModelContainerWiring.EndSuspendValueChangeEvent();
            }
        }

        /// <summary>
        ///   Undoes the current command.
        /// </summary>
        public void Undo()
        {
            Controller.ModelContainerWiring.BeginSuspendValueChangeEvent();

            try
            {
                PropertyInfo.SetValue( Target, OldValue, null );
            }
            finally
            {
                Controller.ModelContainerWiring.EndSuspendValueChangeEvent();
            }
        }

        /// <summary>
        ///   Indicates that the command has already been made.
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        ///   The current model controller.
        /// </summary>
        public ModelController Controller { get; set; }
    }
}
