// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ValueChangedEventArgs.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 09 12 3:37 PM
//   * Modified at: 2011  09 12  3:42 PM
// / ******************************************************************************/ 

using System;
using System.Reflection;

namespace SqLauncher.Web.Model.Interception
{
    /// <summary>
    ///   The event args of value changed.
    /// </summary>
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SqLauncher.Web.Model.Interception.ValueChangedEventArgs" /> class.
        /// </summary>
        public ValueChangedEventArgs( PropertyInfo propertyInfo, object target, object oldValue, object newValue )
        {
            PropertyInfo = propertyInfo;
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        ///   The property info of changed property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        ///   The changed object.
        /// </summary>
        public Object Target { get; private set; }

        /// <summary>
        ///   The old value.
        /// </summary>
        public Object OldValue { get; private set; }

        /// <summary>
        ///   The new value.
        /// </summary>
        public Object NewValue { get; private set; }
    }
}