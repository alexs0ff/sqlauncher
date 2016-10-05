// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SelectableItemsControl.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description: http://blog.geeky.cc/post/2009/09/11/Silverlight-DataTemplateSelector.aspx
//   * Created at 2012 01 26 21:56
//   * Modified at: 2012  01 26  21:57
// / ******************************************************************************/ 

using System.Windows;
using System.Windows.Controls;

namespace SqLauncher.Web.UI.Common.DataTemplateSelector
{
    /// <summary>
    ///   ItemsControl that allows the selection of data templates.
    /// </summary>
    public sealed class SelectableItemsControl : ItemsControl
    {
        /// <summary>
        ///   DependencyProperty backing field for the 
        ///   <see cref = "ItemTemplateSelector" /> property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            DependencyProperty.Register(
                "ItemTemplateSelector",
                typeof ( DataTemplateSelector ),
                typeof ( SelectableItemsControl ),
                null );

        /// <summary>
        ///   Gets or sets the DataTemplateSelector that will be used to select a
        ///   template for an item.
        /// </summary>
        public DataTemplateSelector ItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector) GetValue(
                    ItemTemplateSelectorProperty );
            }

            set { this.SetValue( ItemTemplateSelectorProperty, value ); }
        }

        /// <summary>
        ///   Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name = "element">
        ///   The element used to display the specified item.
        /// </param>
        /// <param name = "item">
        ///   The item to display.
        /// </param>
        protected override void PrepareContainerForItemOverride(
            DependencyObject element,
            object item )
        {
            base.PrepareContainerForItemOverride( element, item );

            DataTemplateSelector selector = this.ItemTemplateSelector;
            if ( selector != null ){
                ( (ContentPresenter) element ).ContentTemplate =
                    selector.SelectTemplate( item, element );
            }
        }
    }
}