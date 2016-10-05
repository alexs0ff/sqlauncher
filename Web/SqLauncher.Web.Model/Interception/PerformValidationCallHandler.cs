// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   PerformValidationCallHandler.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 10 25 9:25 PM
//   * Modified at: 2011  10 25  9:51 PM
// / ******************************************************************************/ 

using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.Practices.Unity.InterceptionExtension;

namespace SqLauncher.Web.Model.Interception
{
    /// <summary>
    /// The call handler in order to performing validation.
    /// </summary>
    public class PerformValidationCallHandler : ICallHandler
    {
        /// <summary>
        ///   The name of value parameter that passed into property set method.
        /// </summary>
        private const string ValueParameter = "value";

        /// <summary>
        ///   Implement this method to execute your handler processing.
        /// </summary>
        /// <param name = "input">Inputs to the current call to the target.</param>
        /// <param name = "getNext">Delegate to execute to get the next delegate in the handler
        ///   chain.</param>
        /// <returns>
        ///   Return value from the target.
        /// </returns>
        public IMethodReturn Invoke( IMethodInvocation input, GetNextHandlerDelegate getNext )
        {
            if ( input.MethodBase.Name.StartsWith( "set_" ) ){
                string propertyName = input.MethodBase.Name.Substring( 4 );
                Validator.ValidateProperty( input.Arguments[ValueParameter],
                                            new ValidationContext( input.Target ){MemberName = propertyName} );
            }

            return getNext()( input, getNext );
        }

        /// <summary>
        ///   Order in which the handler will be executed
        ///   Executes before <see cref = "T:SqLauncher.Web.Model.Interception.NotifyPropertyChangedHandler" />
        /// </summary>
        public int Order
        {
            get { return 1; }
            set { throw new NotImplementedException(); }
        }
    }
}