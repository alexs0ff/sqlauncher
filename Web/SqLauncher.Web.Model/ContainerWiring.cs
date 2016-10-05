// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ContainerWiring.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 21 1:58 PM
//   * Modified at: 2011  08 22  12:06 PM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Practices.Unity;

using SqLauncher.Web.Model.Interception;

namespace SqLauncher.Web.Model
{
    /// <summary>
    /// Represents type interseption and dpendency injection  engine.
    /// </summary>
    public class ContainerWiring
    {
        #region Fabric 

        /// <summary>
        /// The created instances.
        /// </summary>
        private static readonly ICollection<ContainerWiring> _instances = new Collection<ContainerWiring>();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        private ContainerWiring()
        {
            _container = new UnityContainer();
        }

        /// <summary>
        /// Creates and setups a new instance for model obejct wiring.
        /// </summary>
        /// <param name="databaseModelInterception">The database model interception settings.</param>
        public static ContainerWiring SetUp(IDatabaseModelInterception databaseModelInterception)
        {
            var wiring = new ContainerWiring();
            
            databaseModelInterception.Initialize( wiring._container );
            _instances.Add( wiring );
            return wiring;
        }

        /// <summary>
        /// Gets the Container Wiring instance by owned IUnityContater instance.
        /// </summary>
        /// <param name="container">The owned unity container.</param>
        /// <returns>The finded instance or null.</returns>
        internal static ContainerWiring GetInstanceByContainer(IUnityContainer container)
        {
            return _instances.FirstOrDefault( instance => ReferenceEquals( instance.Container, container ) );
        }

        #endregion Fabric

        #region interception

        private IUnityContainer _container;

        #endregion

        #region Public Methods/Properties

        public IUnityContainer Container
        {
            get { return _container; }
        }

        public void TearDown()
        {
            if ( _container != null ){
                _container.Dispose();
            }
            _instances.Remove( this );
            _container = null;
        }

        #endregion

        #region Value change event 

        /// <summary>
        /// Suspending counter of value changing event.
        /// </summary>
        private int _valueChangedSuspendCounter;

        /// <summary>
        /// Starts suspendings of rising valuse change events.
        /// </summary>
        public void BeginSuspendValueChangeEvent()
        {
            _valueChangedSuspendCounter++;
        }

        /// <summary>
        /// End suspendings of rising valuse change events.
        /// </summary>
        public void EndSuspendValueChangeEvent()
        {
            if (_valueChangedSuspendCounter > 0){
                _valueChangedSuspendCounter--;
            }
        }

        /// <summary>
        /// Occurs when a value of interseption objects has been changed.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Rises a value changed event.
        /// </summary>
        /// <param name="e"></param>
        public void RiseValueChanged( ValueChangedEventArgs e )
        {
            if (_valueChangedSuspendCounter == 0)
            {
                EventHandler<ValueChangedEventArgs> handler = ValueChanged;
                if ( handler != null ){
                    handler( this, e );
                }
            }
        }

        #endregion Value change event

        #region Creating instance 

        /// <summary>
        /// Creates an instance for intercepting.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <returns>The created instance</returns>
        public T CreateInstance<T>()
        {
            BeginSuspendValueChangeEvent();
            var instance = _container.Resolve<T>();

            var bindableModel = instance as BindableModelObject;

            if ( bindableModel!=null ){
                bindableModel.Wiring = this;
            }

            EndSuspendValueChangeEvent();
            return instance;
        }

        #endregion Creating instance
    }
}