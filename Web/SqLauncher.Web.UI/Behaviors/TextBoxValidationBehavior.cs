// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   TextBoxValidationBehavior.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:
//   * Created at 2011 10 24 8:54 PM
//   * Modified at: 2011  10 24  10:30 PM
// / ******************************************************************************/ 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

using SqLauncher.Web.Model.Validation;

namespace SqLauncher.Web.UI.Behaviors
{
    /// <summary>
    ///   Represents the validation behavior for TextBox control.
    ///   It will be validate only the xaml place bindings, thats has been initialized before the Loaded event.
    /// </summary>
    public class TextBoxValidationBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObjectLoaded;
            AssociatedObject.DataContextChanged += TextBoxDataContextChanged;
            AssociatedObject.TextChanged += TextChanged;
        }

        /// <summary>
        ///   Occurs when AssociatedObject has been loaded.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The event args.</param>
        private void AssociatedObjectLoaded( object sender, RoutedEventArgs e )
        {
            AssociatedObject.Loaded -= AssociatedObjectLoaded;

            if ( AssociatedObject.DataContext != null ){
                InitValidation();
            } //if
            
        }

        /// <summary>
        ///   Occurs when text box text has been changed.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void TextChanged( object sender, TextChangedEventArgs e )
        {

        }

        /// <summary>
        ///   Occurs when the textbox data context has been changed.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void TextBoxDataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            InitValidation();
        }

        /// <summary>
        ///   Initialize the validation engine.
        /// </summary>
        private void InitValidation()
        {
            _currentRestrictions.Clear();

            var binding = AssociatedObject.GetBindingExpression( TextBox.TextProperty );

            if ( binding != null ){
                var pi = GetPropertyByPath( AssociatedObject.DataContext, binding.ParentBinding.Path.Path );

                if ( pi != null ){
                    foreach ( var customAttribute in  pi.GetCustomAttributes( true ) ){
                        if ( customAttribute.GetType().IsSubclassOf( typeof ( RestrictDataAttribute ) ) ){
                            _currentRestrictions.Add( (RestrictDataAttribute) customAttribute );
                        } //if
                    } //foreach
                } //if
            } //if
        }

        /// <summary>
        ///   The current restrict attribute.
        /// </summary>
        private readonly ICollection<RestrictDataAttribute> _currentRestrictions =
            new Collection<RestrictDataAttribute>();

        public static PropertyInfo GetPropertyByPath( object obj, string propertyPath )
        {
            if ( obj == null ){
                return null;
            } // if

            Type type = obj.GetType();

            PropertyInfo propertyInfo = null;
            foreach ( var part in propertyPath.Split( new[]{'.'} ) ){
                // On subsequent iterations use the type of the property
                if ( propertyInfo != null ){
                    type = propertyInfo.PropertyType;
                } // if

                // Get the property at this part
                propertyInfo = type.GetProperty( part );

                // Not found
                if ( propertyInfo == null ){
                    return null;
                } // if

                // Can't navigate into indexer
                if ( propertyInfo.GetIndexParameters().Length > 0 ){
                    return null;
                } // if
            } // foreach

            return propertyInfo;
        }


        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
            AssociatedObject.DataContextChanged -= TextBoxDataContextChanged;
            AssociatedObject.TextChanged -= TextChanged;
        }
    }
}