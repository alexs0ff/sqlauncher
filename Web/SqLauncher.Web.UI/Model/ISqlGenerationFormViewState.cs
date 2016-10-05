using System.Collections.Generic;

using SqLauncher.Web.Model;

namespace SqLauncher.Web.UI.Model
{
    public interface ISqlGenerationFormViewState
    {
        /// <summary>
        ///   Gets a collection for generated members.
        /// </summary>
        ICollection<ItemName> GeneratedItems { get; }

        /// <summary>
        /// The path to sql file.
        /// </summary>
        string FilePath { get; set; }
    }
}