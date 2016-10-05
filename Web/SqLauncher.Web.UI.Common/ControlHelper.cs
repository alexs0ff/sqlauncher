// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   ControlHelper.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 08 23 9:49 PM
//   * Modified at: 2011  10 02  11:03 AM
// / ******************************************************************************/ 

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SqLauncher.Web.UI.Common
{
    /// <summary>
    ///   Represents the helper of visual elements manupulation.
    /// </summary>
    public static class ControlHelper
    {
        /// <summary>
        ///   Retrievies any parent control with the given type
        /// </summary>
        /// <typeparam name = "T">The type of control to find.</typeparam>
        /// <param name = "control">The child control.</param>
        /// <returns>The parent or null.</returns>
        public static T FindParent<T>( UIElement control ) where T : UIElement
        {
            UIElement p = VisualTreeHelper.GetParent( control ) as UIElement;
            if ( p != null ){
                if ( p is T ){
                    return p as T;
                }
                return FindParent<T>( p );
            }
            return null;
        }

        /// <summary>
        /// Retrivies an element from specified coordinates.
        /// </summary>
        /// <typeparam name="T">The interesting type.</typeparam>
        /// <param name="element">The object to search within. </param>
        /// <param name="interestingPoint">The point to use as the determination point.</param>
        /// <returns>The found type object or null.</returns>
        public static T FindElementOnGlobalCoordinates<T>(UIElement element, Point interestingPoint) where T : UIElement
        {
            var elements = VisualTreeHelper.FindElementsInHostCoordinates( interestingPoint, element );

            if ( elements.Count() > 0 ){
                int i = 1;
                i++;
            } //if

            return elements.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        ///   Finds a Child of a given item in the visual tree.
        /// </summary>
        /// <param name = "parent">A direct parent of the queried item.</param>
        /// <typeparam name = "T">The type of the queried item.</typeparam>
        /// <param name = "childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        ///   If not matching item can be found, a null parent is being returned.</returns>
        public static T FindChild<T>( DependencyObject parent, string childName )
            where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if ( parent == null ){
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount( parent );
            for ( int i = 0; i < childrenCount; i++ ){
                var child = VisualTreeHelper.GetChild( parent, i );
                // If the child is not of the request child type child
                T childType = child as T;
                if ( childType == null ){
                    // recursively drill down the tree
                    foundChild = FindChild<T>( child, childName );

                    // If the child is found, break so we do not overwrite the found child. 
                    if ( foundChild != null ){
                        break;
                    }
                }
                else if ( !string.IsNullOrEmpty( childName ) ){
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if ( frameworkElement != null && frameworkElement.Name == childName ){
                        // if the child's name is of the request name
                        foundChild = (T) child;
                        break;
                    }
                }
                else{
                    // child element found.
                    foundChild = (T) child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Rebinds the binding on dependencyProperty.
        /// </summary>
        /// <param name="frameworkElement">The dependency object that holds the binding value.</param>
        /// <param name="dependencyProperty">The dependency property to rebind.</param>
        public static void Rebind(this FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
        {
            var bindingExpression = frameworkElement.GetBindingExpression(dependencyProperty);
            
            if ( bindingExpression!=null && bindingExpression.ParentBinding!=null ){
                frameworkElement.ClearValue( dependencyProperty );
                frameworkElement.SetBinding( dependencyProperty, bindingExpression.ParentBinding );
            } //if
        }

        /// <summary>
        /// Updates the source.
        /// </summary>
        /// <param name="frameworkElement">The dependency object that holds the binding value.</param>
        /// <param name="dependencyProperty">The dependency property to updating.</param>
        public static void UpdateSource(this FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
        {
            var bindingExpression = frameworkElement.GetBindingExpression(dependencyProperty);

            if (bindingExpression != null && bindingExpression.ParentBinding != null)
            {
                bindingExpression.UpdateSource();
            } //if
        }

        /// <summary>
        /// Updates the source.
        /// </summary>
        /// <param name="dependencyObject">The dependency object that holds the binding value.</param>
        /// <param name="dependencyProperty">The dependency property to updating.</param>
        public static void UpdateSource(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            var bindingExpression = dependencyObject.GetBindingExpression(dependencyProperty);

            if (bindingExpression != null && bindingExpression.ParentBinding != null)
            {
                bindingExpression.UpdateSource();
            } //if
        }

        /// <summary>
        /// Reads the binding expression from dependency object.
        /// </summary>
        /// <param name="dObj">The dependency object.</param>
        /// <param name="dp">The dependency property.</param>
        /// <returns>The binding expression.</returns>
        public static BindingExpression GetBindingExpression(this DependencyObject dObj, DependencyProperty dp)
        {
            return (dObj.ReadLocalValue(dp) as BindingExpression);
        }

        /// <summary>
        /// Clones a dependency object.
        /// </summary>
        /// <typeparam name="T">The dependecy object type.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The cloned object</returns>
        public static T Clone<T>(this T source) where T : DependencyObject
        {
            Type t = source.GetType();
            T no = (T)Activator.CreateInstance(t);

            Type wt = t;
            while (wt.BaseType != typeof(DependencyObject))
            {
                FieldInfo[] fi = wt.GetFields(BindingFlags.Static | BindingFlags.Public);
                for (int i = 0; i < fi.Length; i++)
                {
                    {
                        DependencyProperty dp = fi[i].GetValue(source) as DependencyProperty;
                        if (dp != null && fi[i].Name != "NameProperty")
                        {
                            DependencyObject obj = source.GetValue(dp) as DependencyObject;
                            if (obj != null)
                            {
                                object o = obj.Clone();
                                no.SetValue(dp, o);
                            }
                            else
                            {
                                if (fi[i].Name != "CountProperty" &&
                                    fi[i].Name != "GeometryTransformProperty" &&
                                    fi[i].Name != "ActualWidthProperty" &&
                                    fi[i].Name != "ActualHeightProperty" &&
                                    fi[i].Name != "MaxWidthProperty" &&
                                    fi[i].Name != "MaxHeightProperty" &&
                                    fi[i].Name != "StyleProperty")
                                {
                                    no.SetValue(dp, source.GetValue(dp));
                                }

                            }
                        }
                    }
                }
                wt = wt.BaseType;
            }

            PropertyInfo[] pis = t.GetProperties();
            for (int i = 0; i < pis.Length; i++)
            {

                if (
                    pis[i].Name != "Name" &&
                    pis[i].Name != "Parent" &&
                    pis[i].CanRead && pis[i].CanWrite &&
                    !pis[i].PropertyType.IsArray &&
                    !pis[i].PropertyType.IsSubclassOf(typeof(DependencyObject)) &&
                    pis[i].GetIndexParameters().Length == 0 &&
                    pis[i].GetValue(source, null) != null &&
                    pis[i].GetValue(source, null) == (object)default(int) &&
                    pis[i].GetValue(source, null) == (object)default(double) &&
                    pis[i].GetValue(source, null) == (object)default(float)
                    )
                    pis[i].SetValue(no, pis[i].GetValue(source, null), null);
                else if (pis[i].PropertyType.GetInterface("IList", true) != null)
                {
                    int cnt = (int)pis[i].PropertyType.InvokeMember("get_Count", BindingFlags.InvokeMethod, null, pis[i].GetValue(source, null), null);
                    for (int c = 0; c < cnt; c++)
                    {
                        object val = pis[i].PropertyType.InvokeMember("get_Item", BindingFlags.InvokeMethod, null, pis[i].GetValue(source, null), new object[] { c });

                        object nVal = val;
                        DependencyObject v = val as DependencyObject;
                        if (v != null)
                            nVal = v.Clone();

                        pis[i].PropertyType.InvokeMember("Add", BindingFlags.InvokeMethod, null, pis[i].GetValue(no, null), new[] { nVal });
                    }
                }
            }

            return no;
        }

    }
}