// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   NotifyPropertyChangedAttribute.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 21 12:21 PM
//   * Modified at: 2011  09 25  2:32 PM
// / ******************************************************************************/ 

using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace SqLauncher.Web.Model.Interception
{
    /// <summary>
    /// The notify property change attribute. Used for change property insterception.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public class NotifyPropertyChangedAttribute : HandlerAttribute
    {
        /// <summary>
        /// Will rise the value changed.
        /// </summary>
        private bool _riseValueChanged = true;

        /// <summary>
        /// Will rise the value changed.
        /// </summary>
        public bool RiseValueChanged
        {
            get { return _riseValueChanged = true; }
            set { _riseValueChanged = value; }
        }

        public override ICallHandler CreateHandler( IUnityContainer container )
        {
            if (_riseValueChanged){
                var wiring = ContainerWiring.GetInstanceByContainer( container );
                return new NotifyPropertyChangedHandler( wiring );
            }
            else{
                return new NotifyPropertyChangedHandler( null );
            } //else
        }
    }
}