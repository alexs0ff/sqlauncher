// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IndexedAttributeProxy.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2012 02 25 19:59
//   * Modified at: 2012  02 27  22:38
// / ******************************************************************************/ 

using System.Windows;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   Represents the proxy to show indexed collection.
    /// </summary>
    public class IndexedAttributeProxy : DependencyObject
    {
        public static readonly DependencyProperty IndexedProperty =
            DependencyProperty.Register( "Indexed", typeof ( bool ), typeof ( IndexedAttributeProxy ),
                                         new PropertyMetadata( OnIndexedChanged ) );

        private static void OnIndexedChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var proxy = (IndexedAttributeProxy) d;
            proxy.Indexed = (bool) e.NewValue;
        }

        /// <summary>
        ///   Get or sets the indexed state.
        /// </summary>
        public bool Indexed
        {
            get { return (bool) GetValue( IndexedProperty ); }
            set { SetValue( IndexedProperty, value ); }
        }

        public static readonly DependencyProperty SortOrderProperty =
            DependencyProperty.Register( "SortOrder", typeof ( SortOrder ), typeof ( IndexedAttributeProxy ),
                                         new PropertyMetadata( SortOrder.Ascending, OnSortOrderChanged ) );

        /// <summary>
        ///   Occurs when sort order peorperty has been changed.
        /// </summary>
        /// <param name = "d"></param>
        /// <param name = "e"></param>
        private static void OnSortOrderChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var proxy = (IndexedAttributeProxy) d;

            if ( proxy.IndexAttribute != null ){
                proxy.IndexAttribute.Order = (SortOrder) e.NewValue;
                //proxy.SortOrder = proxy.IndexAttribute.Order;
            } //if
        }
       
        /// <summary>
        /// The default value of sort order.
        /// </summary>
        public SortOrder SortOrder
        {
            get { return SortOrder.Ascending; }
        }

        /// <summary>
        ///   The handled entity attribute.
        /// </summary>
        public EntityAttribute Attribute { get; set; }

        /// <summary>
        ///   The assotiated IndexAttribute.
        /// </summary>
        public IndexAttribute IndexAttribute { get; set; }
    }
}