// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   LineDescriptor.cs
//   * Project: SqLauncher.Web.UI
//   * Description:
//   * Created at 2011 09 12 8:02 PM
//   * Modified at: 2011  09 12  8:02 PM
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.UI.Model
{
    /// <summary>
    ///   Represents a line for internal processing
    /// </summary>
    public class LineDescriptor
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Object" /> class.
        /// </summary>
        public LineDescriptor( RectConnector end, RectConnector head )
        {
            _end = end;
            _head = head;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Object" /> class.
        /// </summary>
        public LineDescriptor( RectConnector end )
        {
            _end = end;
        }

        private RectConnector _end;

        /// <summary>
        ///   The end of line.
        /// </summary>
        public RectConnector End
        {
            get { return _end; }
            set { _end = value; }
        }

        private RectConnector _head;

        /// <summary>
        ///   The start of line.
        /// </summary>
        public RectConnector Head
        {
            get { return _head; }
            set { _head = value; }
        }

        /// <summary>
        ///   The lenght of line.
        /// </summary>
        private double _lenght;

        /// <summary>
        ///   The lenght of line.
        /// </summary>
        public double Lenght
        {
            get
            {
                CalcLenght();
                return _lenght;
            }
        }

        /// <summary>
        ///   Calculates the lenght.
        /// </summary>
        private void CalcLenght()
        {
            _lenght =
                Math.Sqrt( ( Head.MiddleSidePoint.Y - End.MiddleSidePoint.Y )*
                           ( Head.MiddleSidePoint.Y - End.MiddleSidePoint.Y ) +
                           ( Head.MiddleSidePoint.X - End.MiddleSidePoint.X )*
                           ( Head.MiddleSidePoint.X - End.MiddleSidePoint.X ) );
        }
    }
}