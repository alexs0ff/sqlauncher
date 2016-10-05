using System.Windows;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    /// Represents a connector to a entity side`s.
    /// </summary>
    public class RectConnector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.UI.Model.RectConnector"/> class.
        /// </summary>
        public RectConnector( RectSide rectSide, Point middleSidePoint )
        {
            RectSide = rectSide;
            MiddleSidePoint = middleSidePoint;
        }

        /// <summary>
        /// The side of connector.
        /// </summary>
        public RectSide RectSide { get; set; }

        /// <summary>
        /// coordinates of the middle side
        /// </summary>
        public Point MiddleSidePoint { get; set; }
    }

    /// <summary>
    /// Represents a rect side
    /// </summary>
    public enum RectSide
    {
        Left,
        Top,
        Right,
        Bottom
    }
}
