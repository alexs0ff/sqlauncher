// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IntRangeRestrictAttribute.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 10 24 9:39 PM
//   * Modified at: 2011  10 24  9:45 PM
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model.Validation
{
    /// <summary>
    ///   Represents the restriction attribute for range of int.
    ///   "From" great then value and less then "To"
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public class IntRangeRestrictAttribute : RestrictDataAttribute
    {
        /// <summary>
        ///   The begin of range number.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        ///   The end of range number.
        /// </summary>
        public int To { get; set; }

        /// <summary>
        ///   Gets specified type for restriction.
        /// </summary>
        /// <returns>The type for restriction engine.</returns>
        public override Type GetRestrictedType()
        {
            return typeof ( int );
        }

        /// <summary>
        ///   Validates some value.
        /// </summary>
        /// <param name = "value">The value to validate.</param>
        /// <returns>True if validated.</returns>
        public override bool Validate( object value )
        {
            if ( value == null ){
                return false;
            } //if

            if ( value.GetType() != GetRestrictedType() ){
                return false;
            } //if

            var val = (int) value;

            if ( val > From && val < To ){
                return true;
            } //if

            return false;
        }
    }
}