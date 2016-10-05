// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   SqLitePositionContainer.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 22 20:09
//   * Modified at: 2011  11 22  20:11
// / ******************************************************************************/ 

namespace SqLauncher.Web.Model.SqLite
{
    /// <summary>
    ///   The container for entity sorting issues.
    /// </summary>
    public class SqLitePositionContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqLauncher.Web.Model.SqLite.SqLitePositionContainer"/> class.
        /// </summary>
        public SqLitePositionContainer()
        {
            Position = 1;
        }

        /// <summary>
        ///   Gets or sets the handled entity.
        /// </summary>
        public ERDEntity Entity { get; set; }

        /// <summary>
        ///   Gets or sets the current position.
        /// </summary>
        public int Position { get; set; }
    }
}