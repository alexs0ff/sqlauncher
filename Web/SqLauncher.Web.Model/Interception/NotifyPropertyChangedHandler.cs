// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   NotifyPropertyChangedHandler.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 21 11:10 AM
//   * Modified at: 2011  08 21  2:20 PM
// / ******************************************************************************/ 

using System.ComponentModel;
using System.Reflection;

using Microsoft.Practices.Unity.InterceptionExtension;

namespace SqLauncher.Web.Model.Interception
{
    /// <summary>
    /// The notifu property change handler for a called members.
    /// </summary>
    public class NotifyPropertyChangedHandler : ICallHandler
    {
        /// <summary>
        /// The main container wiring object.
        /// </summary>
        private readonly ContainerWiring _containerWiring;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.Model.Interception.NotifyPropertyChangedHandler"/> class.
        /// </summary>
        public NotifyPropertyChangedHandler(ContainerWiring containerWiring)
        {
            _containerWiring = containerWiring;
        }

        #region Implementation of ICallHandler

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
            string propertyName;
            PropertyInfo propertyInfo;
            object oldValue = null;
            bool isAttributed = GetAttributedMethodParts(input, out propertyName, out propertyInfo);

            if (isAttributed &&propertyInfo != null)
            {
                oldValue = propertyInfo.GetValue( input.Target, null );
            }

            // let the original call go through first, so we can notify *after*
            IMethodReturn result = getNext()(input, getNext);

            if (!isAttributed){
                return result;
            }

            if ( propertyInfo != null ){

                object newValue = propertyInfo.GetValue( input.Target, null );

                if ((newValue ==null || oldValue==null) || !newValue.Equals(oldValue)){
                
                    // get the field storing the delegate list that are stored by the event.
                    BindableModelObject bindableModelObject = input.Target as BindableModelObject;

                    if ( bindableModelObject != null ){
                        bindableModelObject.RisePropertyChanged( propertyName );
                    }

                    //invoke value changed.
                    if ( _containerWiring!=null ){
                        _containerWiring.RiseValueChanged(new ValueChangedEventArgs(propertyInfo, input.Target,
                                                                                              oldValue, newValue));    
                    } //if
                }
            }

            return result;
        }

        /// <summary>
        /// Returns an information about a called propety.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private bool GetAttributedMethodParts(IMethodInvocation input, out string propertyName, out PropertyInfo propertyInfo)
        {
            propertyName = string.Empty;
            propertyInfo = null;

            bool result = false;
            if (input.MethodBase.Name.StartsWith("set_"))
            {
                propertyName = input.MethodBase.Name.Substring(4);
                propertyInfo = input.Target.GetType().GetProperty(propertyName);

                // check for the special attribute
                if (propertyInfo.HasAttribute<NotifyPropertyChangedAttribute>())
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        ///   Order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get { return 2; }
            set { }
        }

        #endregion
    }
}