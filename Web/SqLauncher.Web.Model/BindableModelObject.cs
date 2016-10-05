// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   BindableModelObject.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 08 21 12:09
//   * Modified at: 2011  11 20  16:30
// / ******************************************************************************/ 

using System;
using System.ComponentModel;

namespace SqLauncher.Web.Model
{
    public class BindableModelObject : ModelObject, INotifyPropertyChanged
    {
        /// <summary>
        /// The instance wiring.
        /// </summary>
        private ContainerWiring _wiring;

        /// <summary>
        /// The instance wiring.
        /// </summary>
        internal ContainerWiring Wiring
        {
            get { return _wiring; }
            set
            {
                _wiring = value;
                OnNeedRebuildModelObject();
            }
        }

        /// <summary>
        /// Occurs when we need to reinitialize the all dependencies.
        /// </summary>
        protected virtual void OnNeedRebuildModelObject()
        {
            
        }

        /// <summary>
        /// Createsthe instance of type T.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The instance.</returns>
        protected T CreateInstance<T>() where T:IDeepClonable<T>
        {
            var result = default( T );

            if ( Wiring == null ){
                Type type = typeof ( T );

                if ( type.IsClass ){
                    result = Activator.CreateInstance<T>();
                } //if
            }
            else{
                result = Wiring.CreateInstance<T>();
            } //else

            result.ClonedBy = InnerId;

            return result;
        }

        /// <summary>
        /// The self property name.
        /// </summary>
        private const string SelfPropertyName = "Self";

        /// <summary>
        /// Rises the notify property changed event on Self property.
        /// </summary>
        public void RiseSelfPropertyChanged()
        {
            RisePropertyChanged( SelfPropertyName );
        }

        /// <summary>
        /// The self object properties state.
        /// This property is notifiend about all object changes.
        /// Use with SelfPropertyChanged attribute.
        /// </summary>
        public object Self 
        { 
            get { return this; }
        }

        #region INotifyPropertyChanged Implementation

        /// <summary>
        ///   Occurs when any properties are changed on this object.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   A helper method that raises the PropertyChanged event for a property.
        /// </summary>
        /// <param name = "propertyNames">The names
        ///   of the properties that changed.</param>
        protected virtual void NotifyChanged( params string[] propertyNames )
        {
            foreach ( string name in propertyNames ){
                RisePropertyChanged( name );
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        public virtual void RisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs( propertyName ));
            }
        }

        #endregion
    }
}