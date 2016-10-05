// /******************************************************************************
//   *
//   * (c) 2012 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DataTemplateSelector.cs
//   * Project: SqLauncher.Web.UI.Common
//   * Description:http://blog.geeky.cc/post/2009/09/11/Silverlight-DataTemplateSelector.aspx
//   * Created at 2012 01 26 21:56
//   * Modified at: 2012  01 26  21:56
// / ******************************************************************************/ 

using System.Windows;

namespace SqLauncher.Web.UI.Common.DataTemplateSelector
{
    /// <summary>
    ///   Provides a way to choose a <see cref = "DataTemplate" /> based on the data 
    ///   object and the data-bound element.
    /// </summary>
    public class DataTemplateSelector
    {
        /// <summary>
        ///   When overridden in a derived class, returns a 
        ///   <see cref = "DataTemplate" /> based on custom logic.
        /// </summary>
        /// <param name = "item">
        ///   The data object for which to select the template.
        /// </param>
        /// <param name = "container">
        ///   The data-bound object.
        /// </param>
        /// <returns>
        ///   Returns a <see cref = "DataTemplate" /> or null. The default value is 
        ///   null.
        /// </returns>
        public virtual DataTemplate SelectTemplate(
            object item,
            DependencyObject container )
        {
            return null;
        }
    }
}