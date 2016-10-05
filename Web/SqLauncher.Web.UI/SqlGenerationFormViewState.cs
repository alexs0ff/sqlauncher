// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqlGenerationFormViewState.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 11 24 20:52
//   * Modified at: 2011  11 24  20:55
// / ******************************************************************************/ 

using System.Collections.Generic;
using System.Collections.ObjectModel;

using SqLauncher.Web.Model;
using SqLauncher.Web.Model.Interception;
using SqLauncher.Web.UI.Model;

namespace SqLauncher.Web.UI
{
    /// <summary>
    ///   The data model of sql generation form.
    /// </summary>
    public class SqlGenerationFormViewState:BindableModelObject, ISqlGenerationFormViewState
    {
        /// <summary>
        ///   Gets a collection for generated members.
        /// </summary>
        private readonly ICollection<ItemName> _generatedItems = new ObservableCollection<ItemName>();

        /// <summary>
        ///   Gets a collection for generated members.
        /// </summary>
        public ICollection<ItemName> GeneratedItems
        {
            get { return _generatedItems; }
        }

        /// <summary>
        /// The path to sql file.
        /// </summary>
        [NotifyPropertyChanged(RiseValueChanged = false)]
        public virtual string FilePath { get; set; }
    }
}