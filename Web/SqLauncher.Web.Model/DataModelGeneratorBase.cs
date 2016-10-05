// /******************************************************************************
//   *
//   * (c) 2011 Kulik Alexander
//   *     All rights reserved by Kulik Alexander 
//   *     This code is protected by Copyright Law and International Treaties.
//   *     It is for registered and licensed use only.
//   *
//   ******************************************************************************
//   *
//   * File Name:   DataModelGeneratorBase.cs
//   * Project: SqLauncher.Web.Model
//   * Description:
//   * Created at 2011 11 13 13:40
//   * Modified at: 2011  11 13  14:27
// / ******************************************************************************/ 

using System;

namespace SqLauncher.Web.Model
{
    /// <summary>
    ///   The sql generator for DataModel.
    /// </summary>
    public abstract class DataModelGeneratorBase
    {
        /// <summary>
        /// Creates a sql statements by passed datamodel object.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <returns>The generated sql.</returns>
        public abstract string Generate( DataModel dataModel );

        /// <summary>
        /// Occurs whe an erd entity has been processed.
        /// </summary>
        public event EventHandler<ERDEntityProcessedEventArgs> ERDEntityProcessed;

        /// <summary>
        /// The invocator for ERDEntityProcessed event.
        /// </summary>
        /// <param name="erdEntity">The processed erd entity.</param>
        protected void RiseERDEntityProcessed( ERDEntity erdEntity )
        {
            EventHandler<ERDEntityProcessedEventArgs> handler = ERDEntityProcessed;
            if ( handler != null ){
                handler( this, new ERDEntityProcessedEventArgs( erdEntity ) );
            }
        }
    }
}