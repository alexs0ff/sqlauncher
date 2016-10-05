// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   RestrictDataAttribute.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 10 24 9:32 PM
//   * Modified at: 2011  10 24  9:38 PM
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model.Validation
{
    /// <summary>
    ///   The base attribute to restrict data.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public abstract class RestrictDataAttribute : Attribute
    {
        /// <summary>
        ///   Gets specified type for restriction.
        /// </summary>
        /// <returns>The type for restriction engine.</returns>
        public abstract Type GetRestrictedType();

        /// <summary>
        ///   Validates some value.
        /// </summary>
        /// <param name = "value">The value to validate.</param>
        /// <returns>True if validated.</returns>
        public abstract bool Validate( object value );
    }
}