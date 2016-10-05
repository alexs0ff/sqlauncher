using System.Collections.Generic;

namespace SqLauncher.Web.UI.Common.Measure
{
    /// <summary>
    /// The provider for list of avalible measure units.
    /// </summary>
    public class MeasureListProvider
    {
        /// <summary>
        /// The avalible measures.
        /// </summary>
        public ICollection<string> Measures { get { return MeasureProxy.MeasureStrategies; } }
    }
}
