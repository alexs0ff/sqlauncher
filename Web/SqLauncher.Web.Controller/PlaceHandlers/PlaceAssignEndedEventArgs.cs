using System;

namespace SqLauncher.Web.Controller.PlaceHandlers
{
    /// <summary>
    /// The event args indicates results of  EntityFromPlaceHandler work.
    /// </summary>
    public class PlaceAssignEndedEventArgs:EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.EventArgs"/> class.
        /// </summary>
        public PlaceAssignEndedEventArgs( bool isAssign )
        {
            IsAssign = isAssign;
        }

        /// <summary>
        /// The flag which indicates that new element has a new place.
        /// </summary>
        public bool IsAssign { get; set; }
    }
}
