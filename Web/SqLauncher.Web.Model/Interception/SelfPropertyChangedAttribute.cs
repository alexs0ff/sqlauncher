// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SelfPropertyChangedAttribute.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2012 03 04 16:12
//   * Modified at: 2012  03 04  16:17
// / ******************************************************************************/ 

using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace SqLauncher.Web.Model.Interception
{
    /// <summary>
    ///   Represents the Self property changed attribute.
    ///   Required object has the Self property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SelfPropertyChangedAttribute : HandlerAttribute
    {
        /// <summary>
        ///   Derived classes implement this method. When called, it
        ///   creates a new call handler as specified in the attribute
        ///   configuration.
        /// </summary>
        /// <param name = "container">The <see cref = "T:Microsoft.Practices.Unity.IUnityContainer" /> to use when creating handlers,
        ///   if necessary.</param>
        /// <returns>
        ///   A new call handler object.
        /// </returns>
        public override ICallHandler CreateHandler( IUnityContainer container )
        {
            return new SelfPropertyChangedCallHandler();
        }
    }
}