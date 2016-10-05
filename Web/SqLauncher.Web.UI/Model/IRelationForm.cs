// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   IRelationForm.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 13 8:57 PM
//   * Modified at: 2011  09 13  8:57 PM
// / ******************************************************************************/ 

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Iterface of an relation form.
    /// </summary>
    public interface IRelationForm : IForm
    {
        /// <summary>
        ///   The start point of the line about parent canvas.
        /// </summary>
        RectConnector StartPoint { get; set; }

        /// <summary>
        ///   The destination point of the line about parent canvas.
        /// </summary>
        RectConnector DestinationPoint { get; set; }

        /// <summary>
        ///   The data context.
        /// </summary>
        IRelationViewState DataEntity { get; set; }

        /// <summary>
        ///   Sets or gets visibility property.
        /// </summary>
        bool IsVisible { get; set; }
    }
}